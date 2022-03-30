using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AxBIPRNDRVLib;

namespace BlackIceFileCaptureTester
{
    public partial class FileCapture : Form
    {
        private StreamReader streamToPrint;
        private readonly Font printFont;
        private Int32 printrequest = 0;
        private Int32 printrequestComplete = 0;

        public FileCapture()
        {
            InitializeComponent();
            TxtSession.Text = System.Diagnostics.Process.GetCurrentProcess().SessionId.ToString(CultureInfo.InvariantCulture);
            BiPrnDrv.CreateControl();
            StartCapture("Black Ice Fax");
            BtnStart.Visible = true;
            BtnStop.Visible = false;
            printFont = new Font("Arial", 10);
            
        }

        private void StartCapture(string szPrinterName)
        {
            //
            // You can set the message interface ID manually if you have already set the current message interface ID in the printer driver.
            // You are able to set the message interface ID in the printer driver through BlackIceDEVMODE DLL or OCX.
            // Note: The message interface ID is used at only Terminal Servers.
            // For more information please check the BlackICEDEVMODE reference in the RTK documentation.
            //

            //int iUserInterfaceID = GetPrivateProfileInt("User Interface IDs", Environment.UserName, 0, szIniFile);

            //GetConfigValue(string sFolder, string sKey, int iDefaultValue)
            //SetConfigValue(string sFolder, string sKey, int iValue)

            BiPrnDrv.SessionID = Convert.ToInt32(TxtSession.Text);
            // Call StartCapture() passing the printer name and the message capture method as parameter
            // MESSAGE_CAPTURE_METHOD_COPYDATA =1
            // MESSAGE_CAPTURE_METHOD_BROADCAST = 2
            // MESSAGE_CAPTURE_METHOD_PIPE = 3

            Int32 messageingInterface = 3;
            if (MESSAGE_CAPTURE_METHOD_COPYDATA.Checked) messageingInterface = 1;
            if (MESSAGE_CAPTURE_METHOD_BROADCAST.Checked) messageingInterface = 2;
            if (MESSAGE_CAPTURE_METHOD_PIPE.Checked) messageingInterface = 3;

            BiPrnDrv.StartCapture(szPrinterName, messageingInterface);  // 3 Setting up PIPE
            BiPrnDrv.PrinterName = szPrinterName;
            Log("Capturing : " + szPrinterName);
            Log("Session Id : " + BiPrnDrv.SessionID);
        }

        private void BiPrnDrv_EndDoc(object sender, _DBiPrnDrvEvents_EndDocEvent e)
        {
            printrequestComplete += 1;
            Log(printrequestComplete + " : " + e.groupFileName);
        }

        private void Log(String message)
        {
            Output.Text += Environment.NewLine;
            Output.Text += String.Format("{0} : {1:H:mm:ss}", message, DateTime.Now);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            streamToPrint = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\TestFile.txt");
            try
            {
                PrinterSettings printer = new PrinterSettings {PrinterName = "Black Ice Fax"};
                if (!printer.IsValid)
                {
                    Log("Black Ice Fax" + "Printer not found");
                    return;
                }

                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                pd.PrinterSettings = printer;

                printrequest += 1;
                // Print the document.
                pd.Print();
                Log(printrequest + " : " + "Print Send");
            }
            finally
            {
                streamToPrint.Close();
            }

            timer1.Enabled = true;



        }

        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line. 
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page. 
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            BtnStop.Visible = true;
            BtnStart.Visible = false;

            timer1.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            BtnStop.Visible = false;
            BtnStart.Visible = true;

            timer1.Enabled = false;
        }

        private void BtnChangeSessionId_Click(object sender, EventArgs e)
        {
            BiPrnDrv.StopCapture();
            StartCapture("Black Ice Fax");
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            Output.Text = String.Empty;
        }

        private void BiPrnDrv_StarDoc(object sender, _DBiPrnDrvEvents_StarDocEvent e)
        {
            Log("Start Document : " + e.groupFileName);
        }
    }
}
