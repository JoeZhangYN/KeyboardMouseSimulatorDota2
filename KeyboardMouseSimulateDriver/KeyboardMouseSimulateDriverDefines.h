#pragma once 
#ifndef __H_KEYBOARD_MOUSE_SIMULATE_DRIVER_DEFINES_H__
#define __H_KEYBOARD_MOUSE_SIMULATE_DRIVER_DEFINES_H__


#include <winioctl.h>


#define TYPE_DRIVER_EVENT           0x03
#define TYPE_DRIVER_WINIO           0x02
#define TYPE_DRIVER_WINRING0        0x01

#define NAME_DRIVER_WINIO           L"WINIO"
#define NAME_DRIVER_WINRING0        L"WinRing0_1_2_0"

#define NAME_FILE32_WINIO           L"WinIo32.sys"
#define NAME_FILE64_WINIO           L"WinIo64.sys"
#define NAME_FILE32_WINRING0        L"WinRing0.sys"
#define NAME_FILE64_WINRING0        L"WinRing0x64.sys"


#pragma region WinIO

// Define the various device type values.  Note that values used by Microsoft Corporation
// are in the range 0-32767, and 32768-65535 are reserved for use by customers.
#define FILE_DEVICE_WINIO 0x8010

// Macro definition for defining IOCTL and FSCTL function control codes.
// Note that function codes 0-2047 are reserved for Microsoft Corporation, and 2048-4095 are reserved for customers.
#define IOCTL_WINIO_INDEX 0x0810

// Define our own private IOCTL
#define IOCTL_WINIO_MAPPHYSTOLIN \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_WINIO_UNMAPPHYSADDR \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX + 1, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_WINIO_ENABLEDIRECTIO \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX + 2, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_WINIO_DISABLEDIRECTIO \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX + 3, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_WINIO_READPORT \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX + 4, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_WINIO_WRITEPORT \
    CTL_CODE(FILE_DEVICE_WINIO, IOCTL_WINIO_INDEX + 5, METHOD_BUFFERED, FILE_ANY_ACCESS)

#pragma pack(push, 1)

struct WinIoPort
{
  USHORT m_nPortAddress;      //Address
  ULONG m_nPortValue;         //Value
  UCHAR m_nPortSize;          //Size
};

#pragma pack(pop)

#pragma endregion



#pragma region WinRing0

// The Device type codes form 32768 to 65535 are for customer use.
#define FILE_DEVICE_WINRING0    40000

// The IOCTL function codes from 0x800 to 0xFFF are for customer use.
#define IOCTL_OLS_GET_DRIVER_VERSION \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x800, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_GET_REFCOUNT \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x801, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_READ_MSR \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x821, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_WRITE_MSR \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x822, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_READ_PMC \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x823, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_HALT \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x824, METHOD_BUFFERED, FILE_ANY_ACCESS)

#define IOCTL_OLS_READ_IO_PORT \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x831, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_WRITE_IO_PORT \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x832, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#define IOCTL_OLS_READ_IO_PORT_BYTE \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x833, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_READ_IO_PORT_WORD \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x834, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_READ_IO_PORT_DWORD \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x835, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_WRITE_IO_PORT_BYTE \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x836, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#define IOCTL_OLS_WRITE_IO_PORT_WORD \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x837, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#define IOCTL_OLS_WRITE_IO_PORT_DWORD \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x838, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#define IOCTL_OLS_READ_MEMORY \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x841, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_WRITE_MEMORY \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x842, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#define IOCTL_OLS_READ_PCI_CONFIG \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x851, METHOD_BUFFERED, FILE_READ_ACCESS)

#define IOCTL_OLS_WRITE_PCI_CONFIG \
	CTL_CODE(FILE_DEVICE_WINRING0, 0x852, METHOD_BUFFERED, FILE_WRITE_ACCESS)

#pragma pack(push, 4)

struct WinRing0Port
{
  ULONG	m_nPort;
  union
  {
    ULONG   m_nPortValue;        //Value
    USHORT  m_nPortAddress;      //Address
    UCHAR   m_nPortSize;         //Size
  };
};

#pragma pack(pop)

#pragma endregion


#endif // !__H_KEYBOARD_MOUSE_SIMULATE_DRIVER_DEFINES_H__
