using System;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardMouseSimulatorDota2
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //using (frmKeyboardMouseSimulatorDemo frm = new frmKeyboardMouseSimulatorDemo())
            //  Application.Run(frm);

            using (Form1 frm = new Form1())
            {
                Application.Run(frm);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            //try
            //{
            //    if (System.IO.File.Exists(FileName))
            //    {
            //        System.IO.File.Delete(FileName);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show("资源清理失败.\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
    }
}