#include "stdafx.h"
#include "KeyboardMouseSimulateDriver.h" 
#include "KeyboardMouseSimulateDriverDefines.h"
#include "ServiceControlManager.h"

#include <cwchar>
#include <cstdlib>
#include <ctime>
#include <cmath>
#include <cstddef>
#include <iostream>

#include <conio.h>
#include <Windows.h>

///SELECT * FROM Win32_Keyboard
///SELECT * FROM Win32_PointingDevice
//https://wiki.osdev.org/%228042%22_PS/2_Controller
//https://wiki.osdev.org/PS/2_Mouse
//https://wiki.osdev.org/PS/2_Keyboard
//// �� 8042 ���̿���оƬ���б��    
//// ���� ��� �ӿ�   
//write_to_port(0x64, 0xa8);
//// ֪ͨ 8042 �¸��ֽڵķ��� 0x60 �����ݽ����� ���  
//write_to_port(0x64, 0xd4);
//// ���� ��� ������  
//write_to_port(0x60, 0xf4);
//// ֪ͨ 8042,�¸��ֽڵķ��� 0x60 ������Ӧ���� 8042 ������Ĵ���  
//write_to_port(0x64, 0x60);
//// ��ɼ��̼� ��� �ӿڼ��ж�  
//write_to_port(0x60, 0x47);


//
typedef BOOL(WINAPI *PFN_IsWow64Process)(HANDLE hProcess, PBOOL bIsWow64Process);
typedef UINT(WINAPI *PFN_GetSystemWow64Directory)(LPTSTR szBuffer, UINT nBufferSize);


//
#define KEYBOARD_CMD     0x64               //��������˿�
#define KEYBOARD_DATA    0x60               //�������ݶ˿�
#define MOUSE_METADATA   0x08               //���Ԫ����


//
unsigned int g_nDriverType = 0;             //������������
bool g_bIs64Bits = false;                   //ϵͳ��Ϣ
bool g_bMouseWheel = false;                 //������
wchar_t g_szDriverId[MAX_PATH] = { 0 };     //����ID
HANDLE g_hDriver = INVALID_HANDLE_VALUE;    //����
//unsigned char g_nMouseMetaData = MOUSE_METADATA;


#pragma region Utilities

bool _stdcall Is64Bits()
{
#ifdef _WIN64

  return true;

#else

  HMODULE hKernel32 = GetModuleHandle(TEXT("kernel32.dll"));
  if (NULL == hKernel32)
  {
    // This shouldn't happen, but if we can't get kernel32's module handle then assume we are on x86.
    // We won't ever install 32-bit drivers on 64-bit machines,  
    // we just want to catch it up front to give users a better error message. 
    return false;
  }


  //PFN_IsWow64Process pfnIsWow64Process = (PFN_IsWow64Process)GetProcAddress(hKernel32, "IsWow64Process");
  //if (NULL == pfnIsWow64Process)
  //  return false;

  //BOOL bIsWow64Process = FALSE;
  //if (pfnIsWow64Process(GetCurrentProcess(), &bIsWow64Process))
  //  return true;
  //return false;


  PFN_GetSystemWow64Directory pfnGetSystemWow64Directory =
    (PFN_GetSystemWow64Directory)GetProcAddress(hKernel32, "GetSystemWow64DirectoryW"); // IsWow64Process
  if (NULL == pfnGetSystemWow64Directory)
  {
    // This most likely means we are running on Windows 2000,  
    // which didn't have this API and didn't have a 64-bit counterpart. 
    return false;
  }

  TCHAR szSystemWow64Directory[32767] = { 0 }; // 32767
  if (0 == pfnGetSystemWow64Directory(szSystemWow64Directory, _countof(szSystemWow64Directory)))
  {
    if (ERROR_CALL_NOT_IMPLEMENTED == GetLastError())
      return false;
  }
  // GetSystemWow64Directory succeeded  so we are on a 64-bit OS. 
  return true;

#endif
}

long long _stdcall Checkout()
{
  return (std::time(nullptr) + 60 * 60 * 8);
}

short _stdcall KeyStatus(unsigned int nKey)
{
  //https://msdn.microsoft.com/zh-cn/library/ms646301.aspx
  return GetKeyState(nKey);
}

bool _stdcall CursorPosition(POINT &stPosition, bool bGetOrSet)
{
  //Set mouse speed, see https://msdn.microsoft.com/en-us/library/ms724947(v=vs.85).aspx
  BOOL bResult = FALSE;

  if (bGetOrSet)
  {
    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms648390(v=vs.85).aspx 
    bResult = GetCursorPos(&stPosition);
  }
  else
  {
    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms648394(v=vs.85).aspx
    bResult = SetCursorPos(stPosition.x, stPosition.y);
  }

  return bResult == TRUE ? true : false;
}

#pragma endregion


#pragma region Interface

BOOL _stdcall ReadPortValue(HANDLE pHandle, WORD nAddress, PDWORD pValue, BYTE nSize = MAPVK_VSC_TO_VK)
{
  DWORD nReturned = 0;

  if (TYPE_DRIVER_WINIO == g_nDriverType)
  {
    WinIoPort stPort;
    stPort.m_nPortSize = nSize;
    stPort.m_nPortAddress = nAddress;

#ifdef _WIN64

    return DeviceIoControl(pHandle, IOCTL_WINIO_READPORT, &stPort, sizeof(WinIoPort), pValue, sizeof(DWORD), &nReturned, NULL);

#elif _WIN32

    // If this is a 64 bit OS, we must use the driver to access I/O ports 
    // even if the application is 32 bit
    if (!g_bIs64Bits)
    {
      switch (nSize)
      {
      case 1:
        *pValue = _inp(nAddress);
        break;
      case 2:
        *pValue = _inpw(nAddress);
        break;
      case 4:
        *pValue = _inpd(nAddress);
        break;
      }
      return true;
    }
    return DeviceIoControl(pHandle, IOCTL_WINIO_READPORT, &stPort, sizeof(WinIoPort), pValue, sizeof(DWORD), &nReturned, NULL);

#endif
  }
  else if (TYPE_DRIVER_WINRING0 == g_nDriverType)
  {
    WORD nValue = 0;
    BOOL nResult = DeviceIoControl(pHandle, IOCTL_OLS_READ_IO_PORT_BYTE, &nAddress, sizeof(nAddress), &nValue, sizeof(nValue), &nReturned, NULL);

    *pValue = nValue;

    return nResult;
  }
  else
    return false;
}

BOOL _stdcall WritePortValue(HANDLE pHandle, WORD nAddress, DWORD nValue, BYTE nSize = MAPVK_VSC_TO_VK)
{
  DWORD nReturned = 0;

  if (TYPE_DRIVER_WINIO == g_nDriverType)
  {
    WinIoPort stPort;
    stPort.m_nPortSize = nSize;
    stPort.m_nPortValue = nValue;
    stPort.m_nPortAddress = nAddress;

#ifdef _WIN64

    return DeviceIoControl(pHandle, IOCTL_WINIO_WRITEPORT, &stPort, sizeof(WinIoPort), NULL, 0, &nReturned, NULL);

#elif _WIN32

    // If this is a 64 bit OS, we must use the driver to access I/O ports 
    // even if the application is 32 bit
    if (!g_bIs64Bits)
    {
      switch (nSize)
      {
      case 1:
        _outp(nAddress, nValue);
        break;
      case 2:
        _outpw(nAddress, (WORD)nValue);
        break;
      case 4:
        _outpd(nAddress, nValue);
        break;
      }
      return true;
    }
    return DeviceIoControl(pHandle, IOCTL_WINIO_WRITEPORT, &stPort, sizeof(WinIoPort), NULL, 0, &nReturned, NULL);

#endif
  }
  else if (TYPE_DRIVER_WINRING0 == g_nDriverType)
  {
    WinRing0Port stPort;
    stPort.m_nPort = nAddress;
    stPort.m_nPortSize = (unsigned char)nValue;

    DWORD nLength = offsetof(WinRing0Port, m_nPortSize) + sizeof(stPort.m_nPortSize);
    return DeviceIoControl(pHandle, IOCTL_OLS_WRITE_IO_PORT_BYTE, &stPort, nLength, NULL, 0, &nReturned, NULL);
  }
  else
    return false;
}

void _stdcall TillIBF(HANDLE pHandle)
{
  DWORD nValue = 0;
  do
  {
    ReadPortValue(pHandle, KEYBOARD_CMD, &nValue);
  } while (0x02 & nValue); //д����ǰ�ȴ�Iutput Register Empty, Bit:1 = 0
  //} while (!(0x02 & nValue)); //������ǰ�ȴ�Iutput Register Full, Bit:1 = 1
}

void _stdcall TillOBF(HANDLE pHandle)
{
  DWORD nValue = 0;
  do
  {
    ReadPortValue(pHandle, KEYBOARD_CMD, &nValue);
  } while (0x01 & nValue); //д����ǰ�ȴ�Output Register Empty, Bit:0 = 0
  //} while (!(0x01 & nValue)); //������ǰ�ȴ�Output Register Full, Bit:0 = 0
}

void _stdcall MBCTillOBF(HANDLE pHandle)
{
  DWORD nValue = 0;
  do
  {
    ReadPortValue(pHandle, KEYBOARD_CMD, &nValue);
  } while ((0x20 & nValue) && (0x01 & nValue)); //Empty
  //} while (!(0x20 & nValue) || !(0x01 & nValue)); //Full
}

void _stdcall KBCTillOBF(HANDLE pHandle)
{
  DWORD nValue = 0;
  do
  {
    ReadPortValue(pHandle, KEYBOARD_CMD, &nValue);
  } while (0x01 & nValue); //д����ǰ�ȴ�Output Register Empty, Bit:0 = 0
  //} while ((0x20 & nValue) || !(0x01 & nValue)); //������ǰ�ȴ�Output Register Full, Bit:0 = 0, Bit:1 = 1
}

#pragma endregion



void _stdcall Uninitialize()
{
  if (TYPE_DRIVER_EVENT != g_nDriverType && INVALID_HANDLE_VALUE != g_hDriver)
  {
    // Disable I/O port access if running on a 32 bit OS
    if (TYPE_DRIVER_WINIO == g_nDriverType)
    {
      if (!g_bIs64Bits)
      {
        DWORD nBytesReturned;
        DeviceIoControl(g_hDriver, IOCTL_WINIO_DISABLEDIRECTIO, NULL, 0, NULL, 0, &nBytesReturned, NULL);
      }
    }
    else if (TYPE_DRIVER_WINRING0 == g_nDriverType)
    {
      DWORD nLength, nRefCount = 0;
      DeviceIoControl(g_hDriver, IOCTL_OLS_GET_REFCOUNT, NULL, 0, &nRefCount, sizeof(nRefCount), &nLength, NULL);
    }

    CloseHandle(g_hDriver);
  }

  CServiceControlManager::Stop(g_szDriverId);
  CServiceControlManager::Delete(g_szDriverId);

  g_hDriver = INVALID_HANDLE_VALUE;
}


void _stdcall KeyboardEnable(bool bEnable)
{
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, bEnable ? 0xAE : 0xAD);
}

bool _stdcall KeyDown(unsigned int nKey)
{
  BOOL bResult = true;
  //https://msdn.microsoft.com/en-us/library/ms646306(VS.85).aspx
  unsigned int nMapVirtualKey = MapVirtualKey(nKey, MAPVK_VK_TO_VSC);

  if (TYPE_DRIVER_EVENT == g_nDriverType)
  {
    //https://msdn.microsoft.com/en-us/library/ms646304(VS.85).aspx
    keybd_event((unsigned char)nKey, (unsigned char)nMapVirtualKey, 0, 0);
  }
  else
  {
    KBCTillOBF(g_hDriver);

    //TillIBF(g_hDriver);
    //bResult &= WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD2);      

    //TillIBF(g_hDriver);
    //bResult &= WritePortValue(g_hDriver, KEYBOARD_DATA, 0xE2); // 0x60); // 

    TillIBF(g_hDriver);
    bResult &= WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD2);

    TillIBF(g_hDriver);
    bResult &= WritePortValue(g_hDriver, KEYBOARD_DATA, nMapVirtualKey);

    //TillIBF(g_hDriver);

    //KBCTillOBF(g_hDriver);
  }

  return bResult ? true : false;
}

bool _stdcall KeyUp(unsigned int nKey)
{
  BOOL bResult = true;
  //https://msdn.microsoft.com/en-us/library/ms646306(VS.85).aspx
  unsigned int nMapVirtualKey = MapVirtualKey(nKey, MAPVK_VK_TO_VSC);

  if (TYPE_DRIVER_EVENT == g_nDriverType)
  {
    //https://msdn.microsoft.com/en-us/library/ms646304(VS.85).aspx
    keybd_event((unsigned char)nKey, (unsigned char)nMapVirtualKey, KEYEVENTF_KEYUP, 0);
  }
  else
  {
    KBCTillOBF(g_hDriver);

    //TillIBF(g_hDriver);
    //bResult &= WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD2);

    //TillIBF(g_hDriver);
    //bResult &= WritePortValue(g_hDriver, KEYBOARD_DATA, 0xE0); // 0x60); //

    TillIBF(g_hDriver);
    bResult &= WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD2);

    TillIBF(g_hDriver);
    bResult &= WritePortValue(g_hDriver, KEYBOARD_DATA, nMapVirtualKey | 0x80);

    //TillIBF(g_hDriver);

    //KBCTillOBF(g_hDriver);
  }

  return bResult ? true : false;
}


void _stdcall MouseEnable(bool bEnable)
{
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, bEnable ? 0xA8 : 0xA7);
}
/*
�������
��һ���ֽ�
7 y�����־
6 x�����־
5 y�ź�λ
4 x�ź�λ
3 ���λ����1,Ҳ���� 0x08
2 ����м�
1 ����Ҽ�
0 ������
�ڶ����ֽ� xλ��
�������ֽ� yλ��
*/
void _stdcall MouseControl(unsigned int nButton, int nX = 0x00, int nY = 0x00)
{
  unsigned char g_nMouseMetaData = MOUSE_METADATA;

  if (MOUSEEVENTF_MOVE & nButton)
  {
    if (MOUSEEVENTF_ABSOLUTE & nButton)
    {
      //Current Cursor Position
      //POINT stCurrent;
      //CursorPosition(stCurrent, true);

      //if (stCurrent.x >= nX) 
      //  nX = -(stCurrent.x - nX); 
      //else 
      //  nX = (nX - stCurrent.x); 

      //if (stCurrent.y >= nY) 
      //  nY = -(stCurrent.y - nY); 
      //else 
      //  nY = (nY - stCurrent.y); 
    }
  }

  //Move
  //Destination
  unsigned char nDestX = (std::abs(nX) & 0xFF);
  unsigned char nDestY = (std::abs(nY) & 0xFF);
  //��
  if (nY <= 0)
  {
    //Move up one 0x08, 0x00, 0x01
    g_nMouseMetaData &= ~0x20;
  }
  //��
  else
  {
    //Move down one 0x28, 0x00, 0xFF
    g_nMouseMetaData |= 0x20;
    nDestY = (~nDestY + 1);//����(ȡ��+1)
  }
  //��
  if (nX < 0)
  {
    //Move left one 0x18, 0xFF, 0x00
    g_nMouseMetaData |= 0x10;
    nDestX = (~nDestX + 1);//����(ȡ��+1)
  }
  //��
  else
  {
    //Move right one 0x08, 0x01, 0x00
    g_nMouseMetaData &= ~0x10;
  }

  //Click
  switch (nButton)
  {
  case MOUSEEVENTF_LEFTDOWN:
    g_nMouseMetaData |= 0x01; //= 0x09;
    break;
  case MOUSEEVENTF_LEFTUP:
    g_nMouseMetaData &= ~0x01; //= 0x08;
    break;
  case MOUSEEVENTF_RIGHTDOWN:
    g_nMouseMetaData |= 0x02; //= 0x0A;
    break;
  case MOUSEEVENTF_RIGHTUP:
    g_nMouseMetaData &= ~0x02; //= 0x08;
    break;
  case MOUSEEVENTF_MIDDLEDOWN:
    g_nMouseMetaData |= 0x04; //= 0x0C;
    break;
  case MOUSEEVENTF_MIDDLEUP:
    g_nMouseMetaData &= ~0x04; //= 0x08;
    break;
  case MOUSEEVENTF_XDOWN:
    break;
  case MOUSEEVENTF_XUP:
    break;
  case MOUSEEVENTF_WHEEL:
    break;
  }

  MBCTillOBF(g_hDriver);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD3);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_DATA, g_nMouseMetaData);

  MBCTillOBF(g_hDriver);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD3);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_DATA, nDestX);

  MBCTillOBF(g_hDriver);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD3);
  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_DATA, nDestY);

  if (g_bMouseWheel)
  {
    MBCTillOBF(g_hDriver);
    TillIBF(g_hDriver);
    WritePortValue(g_hDriver, KEYBOARD_CMD, 0xD3);
    TillIBF(g_hDriver);
    WritePortValue(g_hDriver, KEYBOARD_DATA, 0x00);
  }
}

bool _stdcall MouseDown(unsigned int nButtons)
{
  if (TYPE_DRIVER_EVENT == g_nDriverType)
  {
    //https://msdn.microsoft.com/en-us/library/ms646260(VS.85).aspx
    mouse_event(nButtons | MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
  }
  else
  {
    MouseControl(nButtons);
  }

  return true;
}

bool _stdcall MouseUp(unsigned int nButtons)
{
  if (TYPE_DRIVER_EVENT == g_nDriverType)
  {
    //https://msdn.microsoft.com/en-us/library/ms646260(VS.85).aspx
    mouse_event(nButtons | MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
  }
  else
  {
    MouseControl(nButtons);
  }

  return true;
}

bool _stdcall MouseMove(int nX, int nY, bool bAorR)
{
  //mouse move is between [0, 65535]
  //move by logical pixels of desktop
  double nDestX = nX * 65535.F / GetSystemMetrics(SM_CXSCREEN);
  double nDestY = nY * 65535.F / GetSystemMetrics(SM_CYSCREEN);

  if (TYPE_DRIVER_EVENT == g_nDriverType)
  {
    //https://msdn.microsoft.com/en-us/library/ms646260(VS.85).aspx
    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
    mouse_event((bAorR ? (MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE) : (MOUSEEVENTF_MOVE)),
      static_cast<unsigned long>(nDestX), static_cast<unsigned long>(nDestY),
      0, 0);
  }
  else
  {
    MouseControl((bAorR ? (MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE) : (MOUSEEVENTF_MOVE)),
      //static_cast<int>(nDestX), static_cast<int>(nDestY));
      nX, nY);
  }

  return true;
}

bool _stdcall MouseWheel()
{
  return (2 < GetSystemMetrics(SM_CMOUSEBUTTONS)) && (0 != GetSystemMetrics(SM_MOUSEWHEELPRESENT));
}

void _stdcall Interrupt(bool bEnable)
{
  KeyboardEnable(false);
  MouseEnable(false);

  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, 0x20);

  TillIBF(g_hDriver);
  DWORD nValue = 0;
  ReadPortValue(g_hDriver, KEYBOARD_DATA, &nValue);

  if (bEnable)
    nValue |= 0x01;
  else
    nValue &= ~0x01;

  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_CMD, 0x60);

  TillIBF(g_hDriver);
  WritePortValue(g_hDriver, KEYBOARD_DATA, nValue);

  KeyboardEnable(true);
  MouseEnable(true);
}

int _stdcall Initialize(unsigned int nDriverType)
{
  g_nDriverType = nDriverType;

  if (TYPE_DRIVER_EVENT == g_nDriverType)
    return 0;

  g_bIs64Bits = Is64Bits();
  g_bMouseWheel = MouseWheel();

  wchar_t szDriverFile[MAX_PATH * 2] = { 0 };
  if (TYPE_DRIVER_WINIO == g_nDriverType)
  {
    std::swprintf(g_szDriverId, sizeof(g_szDriverId), L"%s", NAME_DRIVER_WINIO);
    std::swprintf(szDriverFile, sizeof(szDriverFile), L"\\\\.\\%s", g_szDriverId);
  }
  else if (TYPE_DRIVER_WINRING0 == g_nDriverType)
  {
    std::swprintf(g_szDriverId, sizeof(g_szDriverId), L"%s", NAME_DRIVER_WINRING0);
    std::swprintf(szDriverFile, sizeof(szDriverFile), L"\\\\.\\%s", g_szDriverId);
  }
  else
    return -1;

  // If the driver is not running, install it
  g_hDriver = CreateFile(szDriverFile, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
  if (INVALID_HANDLE_VALUE == g_hDriver)
  {
    wchar_t szModuleFileName[MAX_PATH * 2] = { 0 };
    if (GetModuleFileName(GetModuleHandle(NULL), szModuleFileName, sizeof(szModuleFileName)))
    {
      wchar_t* szLastSlash = std::wcsrchr(szModuleFileName, '\\');
      if (NULL != szLastSlash)
        szLastSlash[1] = '\0';

      if (TYPE_DRIVER_WINIO == g_nDriverType)
      {
        if (g_bIs64Bits)
          wcscat_s(szModuleFileName, NAME_FILE64_WINIO);
        else
          wcscat_s(szModuleFileName, NAME_FILE32_WINIO);
      }
      else if (TYPE_DRIVER_WINRING0 == g_nDriverType)
      {
        if (g_bIs64Bits)
          wcscat_s(szModuleFileName, NAME_FILE64_WINRING0);
        else
          wcscat_s(szModuleFileName, NAME_FILE32_WINRING0);
      }
      else
        return -2;

    }

    if (!CServiceControlManager::Create(szModuleFileName, g_szDriverId))
      return GetLastError();
    if (!CServiceControlManager::Start(g_szDriverId))
      return GetLastError();
  }

  g_hDriver = CreateFile(szDriverFile, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
  if (INVALID_HANDLE_VALUE == g_hDriver)
    return GetLastError();

  // Enable I/O port access for this process if running on a 32 bit OS
  if (TYPE_DRIVER_WINIO == g_nDriverType)
  {
    if (!g_bIs64Bits)
    {
      DWORD nBytesReturned = 0;
      if (!DeviceIoControl(g_hDriver, IOCTL_WINIO_ENABLEDIRECTIO, NULL, 0, NULL, 0, &nBytesReturned, NULL))
        return GetLastError();
    }
  }

  return 0;
}

