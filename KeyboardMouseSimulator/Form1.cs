using System;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;
using KeyboardMouseSimulatorDota2.Picture_Dota2;
using TestKeyboard.PressKey;
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
        /// 模拟按键
        /// </summary>
        private IPressKey mPressKey;


        /// <summary>
        ///     案件触发，名称指定操作
        ///     如直接写在方法内则在按键触发前生效
        ///     例如可以切假腿放技能
        ///     if (e.KeyValue == (int)Keys.A && (int)Control.ModifierKeys == (int)Keys.Alt) && (int)Control.ModifierKeys == (int)Keys.Control)
        ///     ctrl + alt + A 捕获
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

                    m_trdKeyboardInterval = new Thread(远程飞斧);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 冰女

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

            #region 蓝猫

            else if (textBox1.Text.Trim() == "蓝猫")
            {
                if (e.KeyValue == (uint)Keys.W)
                {
                    label1.Text = "W";

                    m_trdKeyboardInterval = new Thread(拉接平A);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint)Keys.Q)
                {
                    label1.Text = "Q";

                    m_trdKeyboardInterval = new Thread(残影接平A);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint)Keys.Q)
                {
                    label1.Text = "R";

                    m_trdKeyboardInterval = new Thread(滚接平A);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 小骷髅

            else if (textBox1.Text.Trim() == "小骷髅")
            {
                if (e.KeyValue == (uint) Keys.Q)
                {
                    label1.Text = "Q";

                    切假腿放技能();

                    m_trdKeyboardInterval = new Thread(扫射接勋章);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint) Keys.E)
                {
                    label1.Text = "E";

                    切假腿放技能();
                }
            }

            #endregion

            #region 小松鼠

            else if (textBox1.Text.Trim() == "小松鼠")
            {
                if (e.KeyValue == (uint)Keys.D)
                {
                    label1.Text = "D";

                    m_trdKeyboardInterval = new Thread(捆接种树);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 拍拍

            else if (textBox1.Text.Trim() == "拍拍")
            {
                if (e.KeyValue == (uint)Keys.W)
                {
                    label1.Text = "W";

                    切假腿放技能();

                    m_trdKeyboardInterval = new Thread(超强力量平A);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint)Keys.Q)
                {
                    label1.Text = "Q";

                    切假腿放技能();

                    m_trdKeyboardInterval = new Thread(震撼大地接平A);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 小鱼人

            else if (textBox1.Text.Trim() == "小鱼人")
            {
                if (e.KeyValue == (uint)Keys.Q)
                {
                    label1.Text = "Q";

                    切假腿放技能();

                    m_trdKeyboardInterval = new Thread(黑暗契约力量);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint)Keys.W)
                {
                    label1.Text = "W";

                    切假腿放技能();

                    m_trdKeyboardInterval = new Thread(跳水敏捷);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
            }

            #endregion

            #region 宙斯

            else if (textBox1.Text.Trim() == "宙斯")
            {
                if (e.KeyValue == (uint)Keys.Q)
                {
                    label1.Text = "Q";

                    m_trdKeyboardInterval = new Thread(弧形闪电去后摇);
                    m_trdKeyboardInterval.IsBackground = false;
                    m_trdKeyboardInterval.Priority = ThreadPriority.Highest;
                    m_trdKeyboardInterval.Start();
                }
                else if (e.KeyValue == (uint)Keys.W)
                {
                    label1.Text = "W";

                    m_trdKeyboardInterval = new Thread(雷击去后摇);
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
            if (textBox1.Text == "影魔") pictureBox1.Image = Resource_Picture.吹风CD;
            else if (textBox1.Text == "黑鸟") pictureBox1.Image = Resource_Picture.关释放;
            else if (textBox1.Text == "军团") pictureBox1.Image = Resource_Picture.释放强攻;
            else if (textBox1.Text == "孽主") pictureBox1.Image = Resource_Picture.释放深渊;
            else if (textBox1.Text == "巨魔") pictureBox1.Image = Resource_Picture.远斧头;
            else if (textBox1.Text == "冰女") pictureBox1.Image = Resource_Picture.释放冰封禁制;
            else if (textBox1.Text == "蓝猫") pictureBox1.Image = Resource_Picture.释放电子旋涡;
            else if (textBox1.Text == "小骷髅") pictureBox1.Image = Resource_Picture.敏捷腿;
            else if (textBox1.Text == "小松鼠") pictureBox1.Image = Resource_Picture.释放野地奇袭;
            else if (textBox1.Text == "小鱼人") pictureBox1.Image = Resource_Picture.敏捷腿;
            else if (textBox1.Text == "拍拍") pictureBox1.Image = Resource_Picture.敏捷腿;
            else if (textBox1.Text == "宙斯") pictureBox1.Image = Resource_Picture.释放弧形闪电;
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
            SetWindowPos(Handle, -1, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0010 | 0x0080);

            // 设置本窗体为活动窗体
            SetActiveWindow(Handle);
            SetForegroundWindow(Handle);

            // 设置窗体置顶
            TopMost = true;
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
            g_yxc(110, 550, 415);
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
            KeyPress((uint) Keys.W);

            int w_down = 0;

            while (w_down == 0)
                if (FindPicture(Resource_Picture.关释放, CaptureScreen(859, 939, 64, 62)).Count > 0)
                {
                    w_down = 1;
                    Thread.Sleep(dyd);
                    mouse_right_click();
                    Thread.Sleep(yd);
                    KeyPress((uint) Keys.S);
                    Thread.Sleep(dd);
                    KeyPress((uint) Keys.Space);
                }
        }

        #endregion

        #region 军团

        private void 决斗()
        {
            int all_done = 0;

            while (all_done == 0)
            {
                if (FindPicture(Resource_Picture.强攻CD, CaptureScreen(857, 939, 70, 72)).Count > 0)
                {
                    KeyPress((uint) Keys.D);
                    KeyPress((uint) Keys.D);

                    int w_down = 0;

                    while (w_down == 0)
                        if (FindPicture(Resource_Picture.释放强攻, CaptureScreen(857, 939, 70, 72)).Count > 0)
                        {
                            Thread.Sleep(95);
                            w_down = 1;
                        }
                }

                KeyPress((uint) Keys.Space);

                int jd_count = 0;

                while (FindPicture(Resource_Picture.天堂, CaptureScreen(1245, 939, 67, 58), matchRate: 1).Count > 0)
                {
                    KeyPress((uint) Keys.C);
                    jd_count++;

                    if (jd_count >= 1) Thread.Sleep(100);
                    else if (jd_count >= 3)
                        break;
                }

                if (jd_count >= 3) break;

                jd_count = 0;

                while (FindPicture(Resource_Picture.勇气, CaptureScreen(1116, 990, 67, 58), matchRate: 1).Count > 0)
                {
                    KeyPress((uint)Keys.V);
                    jd_count++;

                    if (jd_count >= 1) Thread.Sleep(100);
                    else if (jd_count >= 3)
                        break;
                }

                jd_count = 0;

                if (FindPicture(Resource_Picture.黑黄, CaptureScreen(1115, 941, 67, 58), matchRate: 1).Count > 0)
                    KeyPress((uint) Keys.Z);

                if (FindPicture(Resource_Picture.刃甲, CaptureScreen(1180, 939, 67, 58), matchRate: 1).Count > 0)
                    KeyPress((uint) Keys.X);

                if (FindPicture(Resource_Picture.臂章, CaptureScreen(1115, 941, 67, 58), matchRate: 1).Count > 0)
                    KeyPress((uint)Keys.Z);


                while (FindPicture(Resource_Picture.决斗CD, CaptureScreen(991, 939, 60, 70)).Count > 0)
                {
                    KeyPress((uint) Keys.R);
                    jd_count++;
                    if (jd_count >= 1) Thread.Sleep(100);
                    else if (jd_count >= 3)
                        break;
                }

                all_done = 1;
            }
        }

        #endregion

        #region 影魔

        private void 吹风摇大()
        {
            int w_down = 0;

            KeyPress((uint) Keys.Space);

            while (w_down == 0)
                if (FindPicture(Resource_Picture.吹风CD, CaptureScreen(1291, 991, 60, 45)).Count > 0)
                {
                    w_down = 1;
                    KeyPress((uint) Keys.M);
                    Thread.Sleep(830);
                    KeyPress((uint) Keys.R);
                }
        }

        #endregion

        #region 孽主

        private void 深渊火雨阿托斯()
        {
            int all_done = 0;

            KeyPress((uint) Keys.W);

            while (all_done == 0)
                if (FindPicture(Resource_Picture.释放深渊, CaptureScreen(857, 939, 70, 72)).Count > 0)
                {
                    Thread.Sleep(400);
                    KeyPress((uint) Keys.S);

                    KeyPress((uint) Keys.Q);

                    Thread.Sleep(640);

                    KeyPress((uint) Keys.S);
                    Thread.Sleep(800);

                    KeyPress((uint) Keys.X);

                    all_done = 1;
                }
        }

        #endregion

        #region 巨魔

        private void 远程飞斧()
        {
            if (FindPicture(Resource_Picture.远斧头, CaptureScreen(839, 939, 70, 72)).Count > 0)
            {
                KeyPress((uint) Keys.Q);

                Thread.Sleep(50);

                KeyPress((uint) Keys.W);

                Thread.Sleep(205);

                mouse_right_click();

                KeyPress((uint) Keys.Q);
            }
        }

        #endregion

        #region 冰女
        /// <summary>
        /// 基本无缝控制
        /// 但因为本身是软控制，不要强求
        /// </summary>
        private void 冰封禁制接陨星锤()
        {
            KeyPress((uint) Keys.W);

            int w_down = 0;

            while (w_down == 0)
                if (FindPicture(Resource_Picture.释放冰封禁制, CaptureScreen(859, 939, 64, 62)).Count > 0)
                {
                    Thread.Sleep(365);
                    KeyPress((uint) Keys.Space);
                    w_down = 1;
                }
        }

        #endregion

        #region 蓝猫

        private void 拉接平A()
        {
            int w_down = 0;

            while (w_down == 0)
                if (FindPicture(Resource_Picture.释放电子旋涡, CaptureScreen(859, 939, 64, 62)).Count > 0)
                {
                    Thread.Sleep(285);
                    KeyPress((uint)Keys.A);
                    w_down = 1;
                }
        }

        private void 残影接平A()
        {
            Thread.Sleep(10);
            KeyPress((uint)Keys.A);
        }

        private void 滚接平A()
        {
            int r_down = 0;

            while (r_down == 0)
                if (FindPicture(Resource_Picture.释放球状闪电, CaptureScreen(991, 939, 64, 62)).Count > 0)
                {
                    Thread.Sleep(150);
                    KeyPress((uint)Keys.A);
                    r_down = 1;
                }
        }

        #endregion

        #region 小骷髅

        private void 扫射接勋章()
        {
            平A敏捷腿(0);

            // 勋章放在c位置
            if (FindPicture(Resource_Picture.勋章, CaptureScreen(1250, 940, 64, 50)).Count > 0)
            {
                KeyPress((uint)Keys.C);
            }

            KeyPress((uint)Keys.A);
        }

        #endregion

        #region 小松鼠

        private void 捆接种树()
        {
            KeyPress((uint)Keys.W);

            var q_down = 0;

            while (q_down == 0)
            {
                if (FindPicture(Resource_Picture.释放野地奇袭, CaptureScreen(861, 940, 64, 62)).Count > 0)
                {
                    q_down = 1;
                    Thread.Sleep(223);
                    KeyPress((uint)Keys.Q);
                }
            }
        }

        #endregion

        #region 拍拍

        private void 超强力量平A()
        {
            var w_down = 0;
            while (w_down == 0)
            {
                if (FindPicture(Resource_Picture.释放超强力量, CaptureScreen(861, 940, 64, 62)).Count > 0)
                {
                    w_down = 1;
                    平A敏捷腿(235);
                    KeyPress((uint)Keys.A);
                }
            }
        }

        private void 震撼大地接平A()
        {
            平A敏捷腿();
            KeyPress((uint) Keys.A);
        }

        #endregion

        #region 小鱼人

        private void 黑暗契约力量()
        {
            平A敏捷腿();

            Thread.Sleep(1330);

            切力量腿();

            Thread.Sleep(1000);

            平A敏捷腿();
        }

        private void 跳水敏捷()
        {
            平A敏捷腿();
        }

        #endregion

        #region 宙斯

        private void 弧形闪电去后摇()
        {
            var q_down = 0;

            while (q_down == 0)
            {
                if (FindPicture(Resource_Picture.释放弧形闪电, CaptureScreen(783, 944, 55, 60)).Count > 0)
                {
                    q_down= 1;
                    Thread.Sleep(Convert.ToInt16(textBox2.Text));
                    KeyPress((uint)Keys.S);
                }
            }
        }

        private void 雷击去后摇()
        {
            var w_down = 0;

            while (w_down == 0)
            {
                if (FindPicture(Resource_Picture.释放雷击, CaptureScreen(782, 935, 146, 72)).Count > 0)
                {
                    w_down = 1;
                    Thread.Sleep(Convert.ToInt16(textBox2.Text));
                    KeyPress((uint)Keys.S);
                }
            }
        }


        #endregion

        #region 通用

        private void 切假腿放技能()
        {
            if (FindPicture(Resource_Picture.力量腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                KeyPress((uint)Keys.V);
            }
            else if (FindPicture(Resource_Picture.敏捷腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                KeyPress((uint)Keys.V);
                KeyPress((uint)Keys.V);
            }
        }

        private void 平A敏捷腿(int wait_time = 70)
        {
            Thread.Sleep(70);

            if (FindPicture(Resource_Picture.力量腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                Thread.Sleep(wait_time - 70);
                KeyPress((uint)Keys.V);
                KeyPress((uint)Keys.V);
            }
            else if (FindPicture(Resource_Picture.智力腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                Thread.Sleep(wait_time - 70);
                KeyPress((uint)Keys.V);
            }
        }

        private void 切力量腿(int wait_time = 70)
        {
            Thread.Sleep(70);

            if (FindPicture(Resource_Picture.敏捷腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                Thread.Sleep(wait_time - 70);
                KeyPress((uint)Keys.V);
            }
            else if (FindPicture(Resource_Picture.智力腿, CaptureScreen(1118, 990, 64, 50)).Count > 0)
            {
                Thread.Sleep(wait_time - 70);
                KeyPress((uint)Keys.V);
                KeyPress((uint)Keys.V);
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

        private void KeyUp(uint key)
        {
            KeyboardMouseSimulateDriverAPI.KeyUp(key);
        }

        private void KeyDown(uint key)
        {
            KeyboardMouseSimulateDriverAPI.KeyDown(key);
        }

        private void KeyPress(uint key)
        {
            KeyDown(key);
            KeyUp(key);
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