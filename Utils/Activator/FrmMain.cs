using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Activator.BussinessLogic;
using Activator.Contracts;

namespace Activator
{
    public partial class FrmMain : Form
    {
        private readonly IInput keyboardInput = new KeyboardInput();
        private readonly IWin32 win32 = new Win32();
        private DateTime waitInterval = DateTime.Now;
        private readonly Random rnd = new Random(3);

        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();

            //fill windows
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    LstWindows.Items.Add(new ProcessInfo
                    {
                        MainWindowTitle = process.MainWindowTitle,
                        ProcessId = process.Id
                    });
                }
            }

            //fill duration combo
            for (Int32 hr = 1; hr < 12; hr++)
            {
                CboHour.Items.Add(hr);
            }
            CboHour.SelectedIndex = 0;

            if (LstWindows.Items.Count > 0) LstWindows.SelectedIndex = 0;
            BtnStart.Visible = true;
            BtnStop.Visible = false;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            
            
            if (LstWindows.SelectedIndex == -1)
            {
                MessageBox.Show("Select window to activate");
                return;
            }

            WindowState = FormWindowState.Minimized;
            BtnStart.Visible = false;
            BtnStop.Visible = true;
            
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            Int32 val = rnd.Next(1, 100);
            ProcessInfo processInfo = LstWindows.SelectedItem as ProcessInfo;
            if (processInfo != null && val % 2 == 0)
            {

                //Find the window, using the Window Title
                IntPtr hWnd = win32.FindWindow(Process.GetProcessById(processInfo.ProcessId).MainWindowTitle);
                if (hWnd.ToInt32() > 0 && Process.GetProcessById(processInfo.ProcessId).Responding) //If found
                {    
                    win32.SetForegroundWindow(hWnd); //Activate it
                    keyboardInput.SendInput();
                }
                else
                {
                    Debugger.Break();
                }
                //if (waitInterval < DateTime.Now)
                //{
                //    waitInterval = DateTime.Now.Add(TimeSpan.FromMinutes(2));
                //    Thread.Sleep(TimeSpan.FromMinutes(1));
                //}
            }
            timer1.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            BtnStart.Visible = true;
            BtnStop.Visible = false;
        }

        private class ProcessInfo
        {
            public String MainWindowTitle { get; set; }
            public Int32 ProcessId { get; set; }
            public override string ToString()
            {
                return MainWindowTitle;
            }
        }
    }
}