using System;
using System.Threading;
using System.Windows.Forms;
using static KeyboardMouseSimulatorDota2.PictureProcessing;
using static KeyboardMouseSimulatorDota2.SetWindowTop;

namespace KeyboardMouseSimulatorDota2
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///     用于捕获按键
        /// </summary>
        private readonly KeyboardHook k_hook = new KeyboardHook();

        /// <summary>
        ///     用于按键触发后的操作
        /// </summary>
        private Thread m_trdKeyboardInterval;

        /// <summary>
        ///     按键钩子，用于捕获按下的键
        /// </summary>
        private KeyEventHandler myKeyEventHandeler; //按键钩子


        /// <summary>
        ///     案件触发，名称指定操作
        ///     如直接写在方法内则在按键触发前生效
        ///     例如可以切假腿放技能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            #region 影魔

            if (textBox1.Text == "影魔")
            {
                if (e.KeyValue == (uint) Keys.D)
                {
                    label1.Text = "Space";

                    m_trdKeyboardInterval = new Thread(吹风摇大);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.AboveNormal;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 黑鸟

            else if (textBox1.Text == "黑鸟")
            {
                if (e.KeyValue == (uint) Keys.D3)
                {
                    label1.Text = "3";

                    m_trdKeyboardInterval = new Thread(g_yxc_cy);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.AboveNormal;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint) Keys.D2)
                {
                    label1.Text = "2";

                    m_trdKeyboardInterval = new Thread(g_yxc_y);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.AboveNormal;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint) Keys.D1)
                {
                    label1.Text = "1";

                    m_trdKeyboardInterval = new Thread(g_yxc_fwn);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.AboveNormal;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint) Keys.E)
                {
                    label1.Text = "E";

                    m_trdKeyboardInterval = new Thread(g_yxc_cg);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.AboveNormal;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 军团

            else if (textBox1.Text == "军团")
            {
                if (e.KeyValue == (uint) Keys.E)
                {
                    label1.Text = "E";

                    if (m_trdKeyboardInterval != null && m_trdKeyboardInterval.IsAlive) return;
                    m_trdKeyboardInterval = new Thread(决斗);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint) Keys.F)
                {
                    label1.Text = "D";
                    m_trdKeyboardInterval?.Abort();
                }
            }

            #endregion

            #region 孽主

            else if (textBox1.Text == "孽主")
            {
                if (e.KeyValue == (uint) Keys.E)
                {
                    label1.Text = "E";

                    m_trdKeyboardInterval = new Thread(深渊火雨阿托斯);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 巨魔

            else if (textBox1.Text.Trim() == "巨魔")
            {
                if (e.KeyValue == (uint) Keys.D)
                {
                    label1.Text = "D";

                    if (m_trdKeyboardInterval != null && m_trdKeyboardInterval.IsAlive) return;
                    m_trdKeyboardInterval = new Thread(远程飞斧);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            else if (textBox1.Text.Trim() == "冰女")
            {
                if (e.KeyValue == (uint) Keys.E)
                {
                    label1.Text = "E";

                    m_trdKeyboardInterval = new Thread(冰封禁制接陨星锤);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion
        }

        /// <summary>
        ///     更改起始加载图片
        /// </summary>
        private void change_pic()
        {
            if (textBox1.Text == "影魔") pictureBox1.Load("R:\\吹风CD.bmp");
            else if (textBox1.Text == "黑鸟") pictureBox1.Load("R:\\关释放.bmp");
            else if (textBox1.Text == "军团") pictureBox1.Load("R:\\释放强攻.bmp");
            else if (textBox1.Text == "孽主") pictureBox1.Load("R:\\释放深渊.bmp");
            else if (textBox1.Text == "巨魔") pictureBox1.Load("R:\\远斧头.bmp");
            else if (textBox1.Text == "冰女") pictureBox1.Load("R:\\释放冰封禁制.bmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            冰封禁制接陨星锤();
        }

        #region 页面初始化和注销

        /// <summary>
        ///     页面初始化
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            change_pic();
            startListen();
        }

        /// <summary>
        ///     开始监听和初始化模拟
        /// </summary>
        public void startListen()
        {
            myKeyEventHandeler = hook_KeyDown;
            k_hook.KeyDownEvent += myKeyEventHandeler; // 绑定对应处理函数
            k_hook.Start(); // 安装键盘钩子

            // 初始化键盘鼠标模拟，仅模仿系统函数，winIo 和 WinRing0 需要额外的操作
            KeyboardMouseSimulateDriverAPI.Initialize((uint) SimulateWays.Event);

            // 设置窗体显示在最上层
            SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0010 | 0x0080);

            // 设置本窗体为活动窗体
            SetActiveWindow(this.Handle);
            SetForegroundWindow(this.Handle);

            // 设置窗体置顶
            this.TopMost = true;
        }

        /// <summary>
        ///     取消监听和注销模拟
        /// </summary>
        public void stopListen()
        {
            k_hook.KeyDownEvent -= myKeyEventHandeler; // 取消按键事件
            myKeyEventHandeler = null;
            k_hook.Stop(); // 关闭键盘钩子

            // 取消当前操作
            m_trdKeyboardInterval?.Abort();
            // 释放
            m_trdKeyboardInterval = null;

            // 注销按键模拟
            KeyboardMouseSimulateDriverAPI.Uninitialize();
        }

        /// <summary>
        ///     页面关闭运行，释放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopListen();
        }

        #endregion

        #region Dota2具体实现

        #region 黑鸟

        private void g_yxc_fwn()
        {
            g_yxc(110, 0, 955);
        }

        private void g_yxc_cg()
        {
            g_yxc(110, 550, 405);
        }

        private void g_yxc_y()
        {
            g_yxc(110, 890, 0);
        }

        private void g_yxc_cy()
        {
            g_yxc(125, 725, 195);
        }

        private void g_yxc(int dyd, int yd, int dd)
        {
            keypress((uint) Keys.W);

            int w_down = 0;

            while (w_down == 0)
                if (FindPicture("R:\\关释放.bmp", CaptureScreen(859, 939, 64, 62)).Count > 0)
                {
                    w_down = 1;
                    Thread.Sleep(dyd);
                    mouse_right_click();
                    Thread.Sleep(yd);
                    keypress((uint) Keys.S);
                    Thread.Sleep(dd);
                    keypress((uint) Keys.Space);
                }
        }

        #endregion


        #region 军团

        private void 决斗()
        {
            int all_done = 0;

            while (all_done == 0)
            {
                if (FindPicture("R:\\强攻CD.bmp", CaptureScreen(857, 939, 70, 72)).Count > 0)
                {
                    keypress((uint) Keys.D);
                    keypress((uint) Keys.D);

                    int w_down = 0;

                    while (w_down == 0)
                        if (FindPicture("R:\\释放强攻.bmp", CaptureScreen(857, 939, 70, 72)).Count > 0)
                        {
                            Thread.Sleep(95);
                            w_down = 1;
                        }
                }

                keypress((uint) Keys.Space);

                int jd_count = 0;

                while (FindPicture("R:\\天堂.bmp", CaptureScreen(1245, 939, 67, 58), matchRate: 1).Count > 0)
                {
                    keypress((uint) Keys.C);
                    Thread.Sleep(50);
                    jd_count++;

                    if (jd_count >= 10) break;
                }

                if (jd_count >= 10) break;

                jd_count = 0;

                if (FindPicture("R:\\黑黄.bmp", CaptureScreen(1115, 941, 67, 58), matchRate: 1).Count > 0)
                    keypress((uint) Keys.Z);

                if (FindPicture("R:\\刃甲.bmp", CaptureScreen(1180, 939, 67, 58), matchRate: 1).Count > 0)
                    keypress((uint) Keys.X);


                while (FindPicture("R:\\决斗CD.bmp", CaptureScreen(991, 939, 60, 70)).Count > 0)
                {
                    keypress((uint) Keys.R);
                    Thread.Sleep(50);
                    jd_count++;
                    if (jd_count >= 10) break;
                }

                all_done = 1;
            }
        }

        #endregion

        #region 影魔

        private void 吹风摇大()
        {
            int w_down = 0;

            keypress((uint) Keys.Space);

            while (w_down == 0)
                if (FindPicture("R:\\吹风CD.bmp", CaptureScreen(1291, 991, 60, 45)).Count > 0)
                {
                    w_down = 1;
                    keypress((uint) Keys.M);
                    Thread.Sleep(830);
                    keypress((uint) Keys.R);
                }
        }

        #endregion

        #region 孽主

        private void 深渊火雨阿托斯()
        {
            int all_done = 0;

            keypress((uint) Keys.W);

            while (all_done == 0)
                if (FindPicture("R:\\释放深渊.bmp", CaptureScreen(857, 939, 70, 72)).Count > 0)
                {
                    Thread.Sleep(400);
                    keypress((uint) Keys.S);

                    keypress((uint) Keys.Q);

                    Thread.Sleep(640);

                    keypress((uint) Keys.S);
                    Thread.Sleep(800);

                    keypress((uint) Keys.X);

                    all_done = 1;
                }
        }

        #endregion

        #region 巨魔

        private void 远程飞斧()
        {
            if (FindPicture("R:\\远斧头.bmp", CaptureScreen(839, 939, 70, 72)).Count > 0)
            {
                keypress((uint) Keys.Q);

                Thread.Sleep(50);

                keypress((uint) Keys.W);

                Thread.Sleep(205);

                mouse_right_click();

                keypress((uint) Keys.Q);
            }
        }

        #endregion

        #region 冰女

        private void 冰封禁制接陨星锤()
        {
            keypress((uint) Keys.W);

            int w_down = 0;

            while (w_down == 0)
                if (FindPicture("R:\\释放冰封禁制.bmp", CaptureScreen(859, 939, 64, 62)).Count > 0)
                {
                    Thread.Sleep(200);
                    keypress((uint) Keys.Space);
                    w_down = 1;
                }
        }

        #endregion

        #endregion

        #region 模拟按键

        private void mouse_right_click()
        {
            KeyboardMouseSimulateDriverAPI.MouseDown((uint) KeyboardMouseSimulatorDota2.MouseButtons.RightDown);
            KeyboardMouseSimulateDriverAPI.MouseUp((uint) KeyboardMouseSimulatorDota2.MouseButtons.RightUp);
        }

        private void keypress(uint key)
        {
            KeyboardMouseSimulateDriverAPI.KeyDown(key);
            KeyboardMouseSimulateDriverAPI.KeyUp(key);
        }

        #endregion

        #region 未使用的使用方法

        //// 初始化
        //Parameters stParameter = new Parameters();
        //stParameter.m_nCursorPositionX = 500;
        //stParameter.m_nCursorPositionY = 400;

        //stParameter.m_nMouseButtons = KeyboardMouseSimulatorDemo.MouseButtons.LeftUp;
        //stParameter.m_nMouseButtons = KeyboardMouseSimulatorDemo.MouseButtons.RightDown;
        //stParameter.m_nMouseButtons = KeyboardMouseSimulatorDemo.MouseButtons.Move;


        //// true 绝对移动 falsh 相对移动
        //KeyboardMouseSimulateDriverAPI.MouseMove(500, 500, true);

        //// 左键单击
        //KeyboardMouseSimulateDriverAPI.MouseDown((uint) KeyboardMouseSimulatorDemo.MouseButtons.LeftDown);
        //KeyboardMouseSimulateDriverAPI.MouseUp((uint) KeyboardMouseSimulatorDemo.MouseButtons.LeftUp);

        //// 右键单击
        //KeyboardMouseSimulateDriverAPI.MouseDown((uint) KeyboardMouseSimulatorDemo.MouseButtons.RightDown);
        //KeyboardMouseSimulateDriverAPI.MouseUp((uint) KeyboardMouseSimulatorDemo.MouseButtons.RightUp);

        //// 按空格键
        //KeyboardMouseSimulateDriverAPI.KeyDown((uint) Keys.Space);
        //KeyboardMouseSimulateDriverAPI.KeyUp((uint) Keys.Space);

        #endregion
    }
}