using System.Reflection;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.ReportAppServer.ClientDoc;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.ReportAppServer.ReportDefModel;
using CrystalDecisions.ReportAppServer.CommonControls;
using CrystalDecisions.ReportAppServer.CommLayer;
using CrystalDecisions.ReportAppServer.CommonObjectModel;
using CrystalDecisions.ReportAppServer.ObjectFactory;
using CrystalDecisions.ReportAppServer.Prompting;
using CrystalDecisions.ReportAppServer.DataSetConversion;
using CrystalDecisions.ReportAppServer.DataDefModel;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportAppServer.XmlSerialize;
using System.Timers;

// Added Duplex functionality
// fixed logic, added a checked box to choose new value

namespace Unmanaged_RAS10_CSharp_Printers
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;

        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc1;

        System.Drawing.Printing.PrinterSettings sysPrinterSettings;
        System.Drawing.Printing.PageSettings sysPageSettings;
        CrystalDecisions.ReportAppServer.ReportDefModel.EromPageSettings eromPageSettings;
        CrystalDecisions.ReportAppServer.ReportDefModel.EromPrinterSettings eromPrinterSettings;

        #region CR Labels
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.ComboBox cboCurrentPaperSizes;
		internal System.Windows.Forms.ComboBox cboCurrentPrinters;
		internal System.Windows.Forms.ComboBox cboDefaultPaperTrays;
		internal System.Windows.Forms.ComboBox cboDefaultPaperSizes;
		internal System.Windows.Forms.ComboBox cboDefaultPrinters;
		internal System.Windows.Forms.ComboBox cboCurrentPaperTrays;
		private System.Windows.Forms.Button btnOpenReport;
		internal System.Windows.Forms.RadioButton rdoDefault;
		internal System.Windows.Forms.RadioButton rdoCurrent;
		internal System.Windows.Forms.GroupBox grpBoxCurrent;
		internal System.Windows.Forms.GroupBox grpBoxDefault;
		internal System.Windows.Forms.GroupBox grpBoxDefaultPaper;
		internal System.Windows.Forms.GroupBox grpBoxCurrentPaper;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Button btnSetPrinter;
		private System.Windows.Forms.Button btnSaveRptAs;
		private System.Windows.Forms.Button btnPOController;
        private Button ViewReport;
        private CrystalReportViewer crystalReportViewer1;
        private GroupBox CRPageSetup;
        private TextBox PROrientation;
        private System.Windows.Forms.Label CROrientationLabel;
        private System.Windows.Forms.Label CRPaperSizeLabel;
        private TextBox CRPaperSize;
        private CheckBox chkDissociate;
        private System.Windows.Forms.Label PageOptionslabel;
        private System.Windows.Forms.Label label7;
        private CheckBox CrNoPrinter;
        private TextBox CRBottom;
        private TextBox CRTop;
        private TextBox CRRight;
        private TextBox CRLeft;
        private System.Windows.Forms.Label BottomLabel;
        private System.Windows.Forms.Label Toplabel;
        private System.Windows.Forms.Label Rightlabel;
        private System.Windows.Forms.Label Leftlabel;
        private System.Windows.Forms.Label Marginslabel;
        private Button btnCloserpt;
        private TextBox CRPrinterName;
        private System.Windows.Forms.Label PrinterNamelabel;
        private System.Windows.Forms.Label ReportVersion;
        private TextBox btnReportVersion;
        private TextBox btnReportName;
        private System.Windows.Forms.Label ReportName;
        private System.Windows.Forms.Label rtnSQLStatement;
        private System.Windows.Forms.Label PRTTray;
        private TextBox CRPaperTray;
        private Button btnPrintToPrinter;
        private TextBox btnSavedPrinterName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private TextBox btnReportKind;
        private TextBox btnRecordCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private ComboBox GetPreviewPagesStartwith;
        private Button SetPreviewPagesStartWith;
        private System.Windows.Forms.Label label15;
        private TextBox btnDBDriver;
        private RichTextBox btnSQLStatement;
        private TextBox btnPaperSizeName;
        private Label btrRuntimeVersion;
        private TextBox txtRuntimeVersion;
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
        #endregion CR Labels

        string CRVer;
        private TextBox lblRPTRev;
        private Label label18;
        private Label label;
        private ComboBox cbLastSaveHistory;
        private Button btrRefreshViewer;
        private Button btnZoomFactor;
        private TextBox btnZoomFactorValue;
        private Label label24;
        private Button btnSectionPrintOrientation;
        private TextBox btnCount;
        private Label Count;
        private Label label8;
        private Label label9;
        private ComboBox printerDuplexList;
        private TextBox DefaultUserDuplex;
        private TextBox CurrentUserDuplex;
        private Label label14;
        private Label label16;
        private TextBox SavedDuplexValue;
        private CheckBox chkbUseThisDuplex;
        private GroupBox groupBox1;
        private RadioButton cbPOC;
        private RadioButton cbP2P;
        float isMetric = 0;
        double isMetricTwips = 0.0017639;

        public void createTimer()
        {
            System.Windows.Forms.Timer timerKeepTrack = new System.Windows.Forms.Timer();
            timerKeepTrack.Interval = 1000;
        }

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
            {
                isMetric = 567;
                isMetricTwips = 0.0017639; // centimeters
            }
            else
            {
                isMetric = 1440;
                isMetricTwips = 1440;
            }

            foreach (Assembly MyVerison in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (MyVerison.FullName.Substring(0, 38) == "CrystalDecisions.CrystalReports.Engine")
                {
                    //File:             C:\Windows\assembly\GAC_MSIL\CrystalDecisions.CrystalReports.Engine\13.0.2000.0__692fbea5521e1304\CrystalDecisions.CrystalReports.Engine.dll
                    //InternalName:     Crystal Reports
                    //OriginalFilename: 
                    //FileVersion:      13.0.9.1312
                    //FileDescription:  Crystal Reports
                    //Product:          SBOP Crystal Reports
                    //ProductVersion:   13.0.9.1312
                    //Debug:            False
                    //Patched:          False
                    //PreRelease:       False
                    //PrivateBuild:     False
                    //SpecialBuild:     False
                    //Language:         English (United States)

                    System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MyVerison.Location);
                    txtRuntimeVersion.Text += fileVersionInfo.FileVersion.ToString();

                    CRVer = fileVersionInfo.FileVersion.Substring(0, 2);
                    break;
                }
            }

            //VB code
            //Imports CrystalDecisions.CrystalReports.Engine
            //Imports CrystalDecisions.Shared
            //Imports System.Reflection
            //Imports System.Runtime.InteropServices

            //Public Class Form1

            //    Private Sub CrystalReportViewer1_Load(sender As Object, e As EventArgs) Handles CrystalReportViewer1.Load

            //        For Each MyVerison As Assembly In AppDomain.CurrentDomain.GetAssemblies()
            //            If MyVerison.FullName.Substring(0, 38) = "CrystalDecisions.CrystalReports.Engine" Then
            //                'File:             C:\Windows\assembly\GAC_MSIL\CrystalDecisions.CrystalReports.Engine\13.0.2000.0__692fbea5521e1304\CrystalDecisions.CrystalReports.Engine.dll
            //                'InternalName:     Crystal Reports
            //                'OriginalFilename: 
            //                'FileVersion:      13.0.9.1312
            //                'FileDescription:  Crystal Reports
            //                'Product:          SBOP Crystal Reports
            //                'ProductVersion:   13.0.9.1312
            //                'Debug:            False
            //                'Patched:          False
            //                'PreRelease:       False
            //                'PrivateBuild:     False
            //                'SpecialBuild:     False
            //                'Language:         English (United States)

            //                Dim fileVersionInfo As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(MyVerison.Location)
            //                MessageBox.Show(fileVersionInfo.FileVersion.ToString())

            //                Return
            //            End If
            //        Next


            //        Dim Report As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            //        Report.Load("C:\reports\formulas.rpt")
            //        CrystalReportViewer1.ReportSource = Report
            //    End Sub

            //End Class 


			// 
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.rdoDefault = new System.Windows.Forms.RadioButton();
            this.rdoCurrent = new System.Windows.Forms.RadioButton();
            this.grpBoxDefault = new System.Windows.Forms.GroupBox();
            this.DefaultUserDuplex = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.grpBoxDefaultPaper = new System.Windows.Forms.GroupBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.cboDefaultPaperTrays = new System.Windows.Forms.ComboBox();
            this.cboDefaultPaperSizes = new System.Windows.Forms.ComboBox();
            this.cboDefaultPrinters = new System.Windows.Forms.ComboBox();
            this.grpBoxCurrent = new System.Windows.Forms.GroupBox();
            this.CurrentUserDuplex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.grpBoxCurrentPaper = new System.Windows.Forms.GroupBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.cboCurrentPaperTrays = new System.Windows.Forms.ComboBox();
            this.cboCurrentPaperSizes = new System.Windows.Forms.ComboBox();
            this.cboCurrentPrinters = new System.Windows.Forms.ComboBox();
            this.btnSetPrinter = new System.Windows.Forms.Button();
            this.btnOpenReport = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSaveRptAs = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnPOController = new System.Windows.Forms.Button();
            this.ViewReport = new System.Windows.Forms.Button();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.CRPageSetup = new System.Windows.Forms.GroupBox();
            this.chkbUseThisDuplex = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.printerDuplexList = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.SavedDuplexValue = new System.Windows.Forms.TextBox();
            this.btnPaperSizeName = new System.Windows.Forms.TextBox();
            this.btnSavedPrinterName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.PRTTray = new System.Windows.Forms.Label();
            this.CRPaperTray = new System.Windows.Forms.TextBox();
            this.CRPrinterName = new System.Windows.Forms.TextBox();
            this.PrinterNamelabel = new System.Windows.Forms.Label();
            this.CRBottom = new System.Windows.Forms.TextBox();
            this.CRTop = new System.Windows.Forms.TextBox();
            this.CRRight = new System.Windows.Forms.TextBox();
            this.CRLeft = new System.Windows.Forms.TextBox();
            this.BottomLabel = new System.Windows.Forms.Label();
            this.Toplabel = new System.Windows.Forms.Label();
            this.Rightlabel = new System.Windows.Forms.Label();
            this.Leftlabel = new System.Windows.Forms.Label();
            this.Marginslabel = new System.Windows.Forms.Label();
            this.PROrientation = new System.Windows.Forms.TextBox();
            this.CROrientationLabel = new System.Windows.Forms.Label();
            this.CRPaperSizeLabel = new System.Windows.Forms.Label();
            this.CRPaperSize = new System.Windows.Forms.TextBox();
            this.chkDissociate = new System.Windows.Forms.CheckBox();
            this.PageOptionslabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CrNoPrinter = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnCloserpt = new System.Windows.Forms.Button();
            this.ReportVersion = new System.Windows.Forms.Label();
            this.btnReportVersion = new System.Windows.Forms.TextBox();
            this.btnReportName = new System.Windows.Forms.TextBox();
            this.ReportName = new System.Windows.Forms.Label();
            this.rtnSQLStatement = new System.Windows.Forms.Label();
            this.btnPrintToPrinter = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnReportKind = new System.Windows.Forms.TextBox();
            this.btnRecordCount = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.GetPreviewPagesStartwith = new System.Windows.Forms.ComboBox();
            this.SetPreviewPagesStartWith = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnDBDriver = new System.Windows.Forms.TextBox();
            this.btnSQLStatement = new System.Windows.Forms.RichTextBox();
            this.btrRuntimeVersion = new System.Windows.Forms.Label();
            this.txtRuntimeVersion = new System.Windows.Forms.TextBox();
            this.lblRPTRev = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.cbLastSaveHistory = new System.Windows.Forms.ComboBox();
            this.btrRefreshViewer = new System.Windows.Forms.Button();
            this.btnZoomFactor = new System.Windows.Forms.Button();
            this.btnZoomFactorValue = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btnSectionPrintOrientation = new System.Windows.Forms.Button();
            this.btnCount = new System.Windows.Forms.TextBox();
            this.Count = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPOC = new System.Windows.Forms.RadioButton();
            this.cbP2P = new System.Windows.Forms.RadioButton();
            this.grpBoxDefault.SuspendLayout();
            this.grpBoxDefaultPaper.SuspendLayout();
            this.grpBoxCurrent.SuspendLayout();
            this.grpBoxCurrentPaper.SuspendLayout();
            this.CRPageSetup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoDefault
            // 
            this.rdoDefault.Location = new System.Drawing.Point(280, 8);
            this.rdoDefault.Name = "rdoDefault";
            this.rdoDefault.Size = new System.Drawing.Size(120, 16);
            this.rdoDefault.TabIndex = 9;
            this.rdoDefault.Text = "Use these options";
            this.rdoDefault.CheckedChanged += new System.EventHandler(this.rdoDefault_CheckedChanged);
            // 
            // rdoCurrent
            // 
            this.rdoCurrent.Checked = true;
            this.rdoCurrent.Location = new System.Drawing.Point(8, 8);
            this.rdoCurrent.Name = "rdoCurrent";
            this.rdoCurrent.Size = new System.Drawing.Size(120, 16);
            this.rdoCurrent.TabIndex = 8;
            this.rdoCurrent.TabStop = true;
            this.rdoCurrent.Text = "Use these options";
            this.rdoCurrent.CheckedChanged += new System.EventHandler(this.rdoCurrent_CheckedChanged);
            // 
            // grpBoxDefault
            // 
            this.grpBoxDefault.Controls.Add(this.DefaultUserDuplex);
            this.grpBoxDefault.Controls.Add(this.label9);
            this.grpBoxDefault.Controls.Add(this.Label2);
            this.grpBoxDefault.Controls.Add(this.grpBoxDefaultPaper);
            this.grpBoxDefault.Controls.Add(this.cboDefaultPrinters);
            this.grpBoxDefault.Enabled = false;
            this.grpBoxDefault.Location = new System.Drawing.Point(280, 24);
            this.grpBoxDefault.Name = "grpBoxDefault";
            this.grpBoxDefault.Size = new System.Drawing.Size(256, 192);
            this.grpBoxDefault.TabIndex = 7;
            this.grpBoxDefault.TabStop = false;
            this.grpBoxDefault.Text = "Default User:";
            // 
            // DefaultUserDuplex
            // 
            this.DefaultUserDuplex.Location = new System.Drawing.Point(103, 164);
            this.DefaultUserDuplex.Name = "DefaultUserDuplex";
            this.DefaultUserDuplex.Size = new System.Drawing.Size(100, 20);
            this.DefaultUserDuplex.TabIndex = 117;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 167);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 115;
            this.label9.Text = "Printer Duplex:";
            // 
            // Label2
            // 
            this.Label2.Enabled = false;
            this.Label2.Location = new System.Drawing.Point(16, 16);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(48, 16);
            this.Label2.TabIndex = 6;
            this.Label2.Text = "Printer:";
            // 
            // grpBoxDefaultPaper
            // 
            this.grpBoxDefaultPaper.Controls.Add(this.Label5);
            this.grpBoxDefaultPaper.Controls.Add(this.Label6);
            this.grpBoxDefaultPaper.Controls.Add(this.cboDefaultPaperTrays);
            this.grpBoxDefaultPaper.Controls.Add(this.cboDefaultPaperSizes);
            this.grpBoxDefaultPaper.Enabled = false;
            this.grpBoxDefaultPaper.Location = new System.Drawing.Point(10, 60);
            this.grpBoxDefaultPaper.Name = "grpBoxDefaultPaper";
            this.grpBoxDefaultPaper.Size = new System.Drawing.Size(240, 97);
            this.grpBoxDefaultPaper.TabIndex = 5;
            this.grpBoxDefaultPaper.TabStop = false;
            this.grpBoxDefaultPaper.Text = "paper:";
            // 
            // Label5
            // 
            this.Label5.Enabled = false;
            this.Label5.Location = new System.Drawing.Point(16, 54);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(40, 16);
            this.Label5.TabIndex = 7;
            this.Label5.Text = "source";
            // 
            // Label6
            // 
            this.Label6.Enabled = false;
            this.Label6.Location = new System.Drawing.Point(16, 14);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(40, 16);
            this.Label6.TabIndex = 6;
            this.Label6.Text = "size";
            // 
            // cboDefaultPaperTrays
            // 
            this.cboDefaultPaperTrays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefaultPaperTrays.Enabled = false;
            this.cboDefaultPaperTrays.Location = new System.Drawing.Point(16, 70);
            this.cboDefaultPaperTrays.Name = "cboDefaultPaperTrays";
            this.cboDefaultPaperTrays.Size = new System.Drawing.Size(216, 21);
            this.cboDefaultPaperTrays.TabIndex = 5;
            // 
            // cboDefaultPaperSizes
            // 
            this.cboDefaultPaperSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefaultPaperSizes.Enabled = false;
            this.cboDefaultPaperSizes.Location = new System.Drawing.Point(16, 30);
            this.cboDefaultPaperSizes.Name = "cboDefaultPaperSizes";
            this.cboDefaultPaperSizes.Size = new System.Drawing.Size(216, 21);
            this.cboDefaultPaperSizes.TabIndex = 4;
            // 
            // cboDefaultPrinters
            // 
            this.cboDefaultPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefaultPrinters.Enabled = false;
            this.cboDefaultPrinters.Location = new System.Drawing.Point(24, 33);
            this.cboDefaultPrinters.Name = "cboDefaultPrinters";
            this.cboDefaultPrinters.Size = new System.Drawing.Size(216, 21);
            this.cboDefaultPrinters.TabIndex = 4;
            this.cboDefaultPrinters.SelectedIndexChanged += new System.EventHandler(this.cboDefaultPrinters_SelectedIndexChanged);
            // 
            // grpBoxCurrent
            // 
            this.grpBoxCurrent.Controls.Add(this.CurrentUserDuplex);
            this.grpBoxCurrent.Controls.Add(this.label8);
            this.grpBoxCurrent.Controls.Add(this.Label1);
            this.grpBoxCurrent.Controls.Add(this.grpBoxCurrentPaper);
            this.grpBoxCurrent.Controls.Add(this.cboCurrentPrinters);
            this.grpBoxCurrent.Location = new System.Drawing.Point(8, 24);
            this.grpBoxCurrent.Name = "grpBoxCurrent";
            this.grpBoxCurrent.Size = new System.Drawing.Size(256, 192);
            this.grpBoxCurrent.TabIndex = 6;
            this.grpBoxCurrent.TabStop = false;
            this.grpBoxCurrent.Text = "Current User:";
            this.grpBoxCurrent.Enter += new System.EventHandler(this.grpBoxCurrent_Enter);
            // 
            // CurrentUserDuplex
            // 
            this.CurrentUserDuplex.Location = new System.Drawing.Point(98, 164);
            this.CurrentUserDuplex.Name = "CurrentUserDuplex";
            this.CurrentUserDuplex.Size = new System.Drawing.Size(100, 20);
            this.CurrentUserDuplex.TabIndex = 116;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 113;
            this.label8.Text = "Printer Duplex:";
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(6, 12);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 16);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Printer:";
            // 
            // grpBoxCurrentPaper
            // 
            this.grpBoxCurrentPaper.Controls.Add(this.Label4);
            this.grpBoxCurrentPaper.Controls.Add(this.Label3);
            this.grpBoxCurrentPaper.Controls.Add(this.cboCurrentPaperTrays);
            this.grpBoxCurrentPaper.Controls.Add(this.cboCurrentPaperSizes);
            this.grpBoxCurrentPaper.Location = new System.Drawing.Point(8, 54);
            this.grpBoxCurrentPaper.Name = "grpBoxCurrentPaper";
            this.grpBoxCurrentPaper.Size = new System.Drawing.Size(240, 103);
            this.grpBoxCurrentPaper.TabIndex = 2;
            this.grpBoxCurrentPaper.TabStop = false;
            this.grpBoxCurrentPaper.Text = "paper:";
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(16, 56);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(40, 16);
            this.Label4.TabIndex = 3;
            this.Label4.Text = "source";
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(16, 12);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(40, 16);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "size";
            // 
            // cboCurrentPaperTrays
            // 
            this.cboCurrentPaperTrays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCurrentPaperTrays.Location = new System.Drawing.Point(16, 72);
            this.cboCurrentPaperTrays.Name = "cboCurrentPaperTrays";
            this.cboCurrentPaperTrays.Size = new System.Drawing.Size(216, 21);
            this.cboCurrentPaperTrays.TabIndex = 1;
            // 
            // cboCurrentPaperSizes
            // 
            this.cboCurrentPaperSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCurrentPaperSizes.Location = new System.Drawing.Point(16, 32);
            this.cboCurrentPaperSizes.Name = "cboCurrentPaperSizes";
            this.cboCurrentPaperSizes.Size = new System.Drawing.Size(216, 21);
            this.cboCurrentPaperSizes.TabIndex = 0;
            // 
            // cboCurrentPrinters
            // 
            this.cboCurrentPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCurrentPrinters.Location = new System.Drawing.Point(22, 32);
            this.cboCurrentPrinters.Name = "cboCurrentPrinters";
            this.cboCurrentPrinters.Size = new System.Drawing.Size(216, 21);
            this.cboCurrentPrinters.TabIndex = 1;
            this.cboCurrentPrinters.SelectedIndexChanged += new System.EventHandler(this.cboCurrentPrinters_SelectedIndexChanged);
            // 
            // btnSetPrinter
            // 
            this.btnSetPrinter.Enabled = false;
            this.btnSetPrinter.Location = new System.Drawing.Point(559, 16);
            this.btnSetPrinter.Name = "btnSetPrinter";
            this.btnSetPrinter.Size = new System.Drawing.Size(84, 24);
            this.btnSetPrinter.TabIndex = 10;
            this.btnSetPrinter.Text = "Set Printer";
            this.btnSetPrinter.Click += new System.EventHandler(this.btnSetPrinter_Click);
            // 
            // btnOpenReport
            // 
            this.btnOpenReport.Location = new System.Drawing.Point(13, 226);
            this.btnOpenReport.Name = "btnOpenReport";
            this.btnOpenReport.Size = new System.Drawing.Size(78, 24);
            this.btnOpenReport.TabIndex = 11;
            this.btnOpenReport.Text = "open rpt...";
            this.btnOpenReport.Click += new System.EventHandler(this.btnOpenReport_Click);
            // 
            // btnSaveRptAs
            // 
            this.btnSaveRptAs.Enabled = false;
            this.btnSaveRptAs.Location = new System.Drawing.Point(280, 226);
            this.btnSaveRptAs.Name = "btnSaveRptAs";
            this.btnSaveRptAs.Size = new System.Drawing.Size(78, 24);
            this.btnSaveRptAs.TabIndex = 12;
            this.btnSaveRptAs.Text = "save as...";
            this.btnSaveRptAs.Click += new System.EventHandler(this.btnSaveReportAs_Click);
            // 
            // btnPOController
            // 
            this.btnPOController.Enabled = false;
            this.btnPOController.Location = new System.Drawing.Point(559, 45);
            this.btnPOController.Name = "btnPOController";
            this.btnPOController.Size = new System.Drawing.Size(84, 24);
            this.btnPOController.TabIndex = 13;
            this.btnPOController.Text = "POController";
            this.btnPOController.Click += new System.EventHandler(this.btnPOController_Click_1);
            // 
            // ViewReport
            // 
            this.ViewReport.Enabled = false;
            this.ViewReport.Location = new System.Drawing.Point(191, 226);
            this.ViewReport.Name = "ViewReport";
            this.ViewReport.Size = new System.Drawing.Size(78, 24);
            this.ViewReport.TabIndex = 15;
            this.ViewReport.Text = "View Report";
            this.ViewReport.Click += new System.EventHandler(this.ViewReport_Click);
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Location = new System.Drawing.Point(8, 383);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.PrintMode = CrystalDecisions.Windows.Forms.PrintMode.PrintToPrinter;
            this.crystalReportViewer1.Size = new System.Drawing.Size(1214, 385);
            this.crystalReportViewer1.TabIndex = 34;
            // 
            // CRPageSetup
            // 
            this.CRPageSetup.Controls.Add(this.chkbUseThisDuplex);
            this.CRPageSetup.Controls.Add(this.label14);
            this.CRPageSetup.Controls.Add(this.printerDuplexList);
            this.CRPageSetup.Controls.Add(this.label16);
            this.CRPageSetup.Controls.Add(this.SavedDuplexValue);
            this.CRPageSetup.Controls.Add(this.btnPaperSizeName);
            this.CRPageSetup.Controls.Add(this.btnSavedPrinterName);
            this.CRPageSetup.Controls.Add(this.label10);
            this.CRPageSetup.Controls.Add(this.PRTTray);
            this.CRPageSetup.Controls.Add(this.CRPaperTray);
            this.CRPageSetup.Controls.Add(this.CRPrinterName);
            this.CRPageSetup.Controls.Add(this.PrinterNamelabel);
            this.CRPageSetup.Controls.Add(this.CRBottom);
            this.CRPageSetup.Controls.Add(this.CRTop);
            this.CRPageSetup.Controls.Add(this.CRRight);
            this.CRPageSetup.Controls.Add(this.CRLeft);
            this.CRPageSetup.Controls.Add(this.BottomLabel);
            this.CRPageSetup.Controls.Add(this.Toplabel);
            this.CRPageSetup.Controls.Add(this.Rightlabel);
            this.CRPageSetup.Controls.Add(this.Leftlabel);
            this.CRPageSetup.Controls.Add(this.Marginslabel);
            this.CRPageSetup.Controls.Add(this.PROrientation);
            this.CRPageSetup.Controls.Add(this.CROrientationLabel);
            this.CRPageSetup.Controls.Add(this.CRPaperSizeLabel);
            this.CRPageSetup.Controls.Add(this.CRPaperSize);
            this.CRPageSetup.Controls.Add(this.chkDissociate);
            this.CRPageSetup.Controls.Add(this.PageOptionslabel);
            this.CRPageSetup.Controls.Add(this.label7);
            this.CRPageSetup.Controls.Add(this.CrNoPrinter);
            this.CRPageSetup.Location = new System.Drawing.Point(649, 9);
            this.CRPageSetup.Name = "CRPageSetup";
            this.CRPageSetup.Size = new System.Drawing.Size(235, 368);
            this.CRPageSetup.TabIndex = 37;
            this.CRPageSetup.TabStop = false;
            this.CRPageSetup.Text = "CR Page Setup";
            // 
            // chkbUseThisDuplex
            // 
            this.chkbUseThisDuplex.AutoSize = true;
            this.chkbUseThisDuplex.Location = new System.Drawing.Point(104, 350);
            this.chkbUseThisDuplex.Name = "chkbUseThisDuplex";
            this.chkbUseThisDuplex.Size = new System.Drawing.Size(93, 17);
            this.chkbUseThisDuplex.TabIndex = 115;
            this.chkbUseThisDuplex.Text = "Use this value";
            this.chkbUseThisDuplex.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(22, 329);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 114;
            this.label14.Text = "Printer Duplex:";
            // 
            // printerDuplexList
            // 
            this.printerDuplexList.FormattingEnabled = true;
            this.printerDuplexList.Location = new System.Drawing.Point(104, 326);
            this.printerDuplexList.Name = "printerDuplexList";
            this.printerDuplexList.Size = new System.Drawing.Size(91, 21);
            this.printerDuplexList.TabIndex = 113;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 298);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 13);
            this.label16.TabIndex = 68;
            this.label16.Text = "Saved Duplex Value:";
            // 
            // SavedDuplexValue
            // 
            this.SavedDuplexValue.Location = new System.Drawing.Point(121, 295);
            this.SavedDuplexValue.Name = "SavedDuplexValue";
            this.SavedDuplexValue.Size = new System.Drawing.Size(100, 20);
            this.SavedDuplexValue.TabIndex = 67;
            // 
            // btnPaperSizeName
            // 
            this.btnPaperSizeName.Location = new System.Drawing.Point(78, 149);
            this.btnPaperSizeName.Name = "btnPaperSizeName";
            this.btnPaperSizeName.Size = new System.Drawing.Size(141, 20);
            this.btnPaperSizeName.TabIndex = 64;
            // 
            // btnSavedPrinterName
            // 
            this.btnSavedPrinterName.Location = new System.Drawing.Point(11, 270);
            this.btnSavedPrinterName.Name = "btnSavedPrinterName";
            this.btnSavedPrinterName.Size = new System.Drawing.Size(210, 20);
            this.btnSavedPrinterName.TabIndex = 62;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 256);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 63;
            this.label10.Text = "SavePrinter Name:";
            // 
            // PRTTray
            // 
            this.PRTTray.AutoSize = true;
            this.PRTTray.Location = new System.Drawing.Point(7, 88);
            this.PRTTray.Name = "PRTTray";
            this.PRTTray.Size = new System.Drawing.Size(31, 13);
            this.PRTTray.TabIndex = 21;
            this.PRTTray.Text = "Tray:";
            // 
            // CRPaperTray
            // 
            this.CRPaperTray.Location = new System.Drawing.Point(36, 86);
            this.CRPaperTray.Name = "CRPaperTray";
            this.CRPaperTray.Size = new System.Drawing.Size(183, 20);
            this.CRPaperTray.TabIndex = 20;
            // 
            // CRPrinterName
            // 
            this.CRPrinterName.Location = new System.Drawing.Point(48, 48);
            this.CRPrinterName.Name = "CRPrinterName";
            this.CRPrinterName.Size = new System.Drawing.Size(173, 20);
            this.CRPrinterName.TabIndex = 19;
            // 
            // PrinterNamelabel
            // 
            this.PrinterNamelabel.AutoSize = true;
            this.PrinterNamelabel.Location = new System.Drawing.Point(8, 51);
            this.PrinterNamelabel.Name = "PrinterNamelabel";
            this.PrinterNamelabel.Size = new System.Drawing.Size(40, 13);
            this.PrinterNamelabel.TabIndex = 18;
            this.PrinterNamelabel.Text = "Printer:";
            // 
            // CRBottom
            // 
            this.CRBottom.Location = new System.Drawing.Point(145, 226);
            this.CRBottom.Name = "CRBottom";
            this.CRBottom.Size = new System.Drawing.Size(63, 20);
            this.CRBottom.TabIndex = 17;
            // 
            // CRTop
            // 
            this.CRTop.Location = new System.Drawing.Point(43, 229);
            this.CRTop.Name = "CRTop";
            this.CRTop.Size = new System.Drawing.Size(55, 20);
            this.CRTop.TabIndex = 16;
            // 
            // CRRight
            // 
            this.CRRight.Location = new System.Drawing.Point(145, 196);
            this.CRRight.Name = "CRRight";
            this.CRRight.Size = new System.Drawing.Size(63, 20);
            this.CRRight.TabIndex = 15;
            // 
            // CRLeft
            // 
            this.CRLeft.Location = new System.Drawing.Point(42, 197);
            this.CRLeft.Name = "CRLeft";
            this.CRLeft.Size = new System.Drawing.Size(56, 20);
            this.CRLeft.TabIndex = 14;
            // 
            // BottomLabel
            // 
            this.BottomLabel.AutoSize = true;
            this.BottomLabel.Location = new System.Drawing.Point(104, 229);
            this.BottomLabel.Name = "BottomLabel";
            this.BottomLabel.Size = new System.Drawing.Size(43, 13);
            this.BottomLabel.TabIndex = 13;
            this.BottomLabel.Text = "Bottom:";
            // 
            // Toplabel
            // 
            this.Toplabel.AutoSize = true;
            this.Toplabel.Location = new System.Drawing.Point(10, 229);
            this.Toplabel.Name = "Toplabel";
            this.Toplabel.Size = new System.Drawing.Size(29, 13);
            this.Toplabel.TabIndex = 12;
            this.Toplabel.Text = "Top:";
            // 
            // Rightlabel
            // 
            this.Rightlabel.AutoSize = true;
            this.Rightlabel.Location = new System.Drawing.Point(104, 199);
            this.Rightlabel.Name = "Rightlabel";
            this.Rightlabel.Size = new System.Drawing.Size(35, 13);
            this.Rightlabel.TabIndex = 11;
            this.Rightlabel.Text = "Right:";
            // 
            // Leftlabel
            // 
            this.Leftlabel.AutoSize = true;
            this.Leftlabel.Location = new System.Drawing.Point(11, 201);
            this.Leftlabel.Name = "Leftlabel";
            this.Leftlabel.Size = new System.Drawing.Size(28, 13);
            this.Leftlabel.TabIndex = 10;
            this.Leftlabel.Text = "Left:";
            // 
            // Marginslabel
            // 
            this.Marginslabel.AutoSize = true;
            this.Marginslabel.Location = new System.Drawing.Point(8, 153);
            this.Marginslabel.Name = "Marginslabel";
            this.Marginslabel.Size = new System.Drawing.Size(66, 13);
            this.Marginslabel.TabIndex = 9;
            this.Marginslabel.Text = "Paper Name";
            // 
            // PROrientation
            // 
            this.PROrientation.Location = new System.Drawing.Point(78, 172);
            this.PROrientation.Name = "PROrientation";
            this.PROrientation.Size = new System.Drawing.Size(117, 20);
            this.PROrientation.TabIndex = 8;
            // 
            // CROrientationLabel
            // 
            this.CROrientationLabel.AutoSize = true;
            this.CROrientationLabel.Location = new System.Drawing.Point(8, 175);
            this.CROrientationLabel.Name = "CROrientationLabel";
            this.CROrientationLabel.Size = new System.Drawing.Size(58, 13);
            this.CROrientationLabel.TabIndex = 7;
            this.CROrientationLabel.Text = "Orientation";
            // 
            // CRPaperSizeLabel
            // 
            this.CRPaperSizeLabel.AutoSize = true;
            this.CRPaperSizeLabel.Location = new System.Drawing.Point(8, 129);
            this.CRPaperSizeLabel.Name = "CRPaperSizeLabel";
            this.CRPaperSizeLabel.Size = new System.Drawing.Size(58, 13);
            this.CRPaperSizeLabel.TabIndex = 6;
            this.CRPaperSizeLabel.Text = "Paper Size";
            // 
            // CRPaperSize
            // 
            this.CRPaperSize.Location = new System.Drawing.Point(65, 126);
            this.CRPaperSize.Name = "CRPaperSize";
            this.CRPaperSize.Size = new System.Drawing.Size(155, 20);
            this.CRPaperSize.TabIndex = 5;
            // 
            // chkDissociate
            // 
            this.chkDissociate.AutoSize = true;
            this.chkDissociate.Location = new System.Drawing.Point(11, 110);
            this.chkDissociate.Name = "chkDissociate";
            this.chkDissociate.Size = new System.Drawing.Size(188, 17);
            this.chkDissociate.TabIndex = 3;
            this.chkDissociate.Text = "Dissociate - Editable when printing";
            this.chkDissociate.UseVisualStyleBackColor = true;
            // 
            // PageOptionslabel
            // 
            this.PageOptionslabel.AutoSize = true;
            this.PageOptionslabel.Location = new System.Drawing.Point(8, 72);
            this.PageOptionslabel.Name = "PageOptionslabel";
            this.PageOptionslabel.Size = new System.Drawing.Size(74, 13);
            this.PageOptionslabel.TabIndex = 2;
            this.PageOptionslabel.Text = "Page Options:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Printer Options";
            // 
            // CrNoPrinter
            // 
            this.CrNoPrinter.AutoSize = true;
            this.CrNoPrinter.Location = new System.Drawing.Point(9, 31);
            this.CrNoPrinter.Name = "CrNoPrinter";
            this.CrNoPrinter.Size = new System.Drawing.Size(184, 17);
            this.CrNoPrinter.TabIndex = 0;
            this.CrNoPrinter.Text = "No Printer ( Optimized for screen )";
            this.CrNoPrinter.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(336, 364);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 13);
            this.label13.TabIndex = 66;
            this.label13.Text = "Preview Pages Start with: ";
            // 
            // btnCloserpt
            // 
            this.btnCloserpt.Enabled = false;
            this.btnCloserpt.Location = new System.Drawing.Point(102, 226);
            this.btnCloserpt.Name = "btnCloserpt";
            this.btnCloserpt.Size = new System.Drawing.Size(78, 24);
            this.btnCloserpt.TabIndex = 38;
            this.btnCloserpt.Text = "Close rpt...";
            this.btnCloserpt.UseVisualStyleBackColor = true;
            this.btnCloserpt.Click += new System.EventHandler(this.Closerpt_Click);
            // 
            // ReportVersion
            // 
            this.ReportVersion.AutoSize = true;
            this.ReportVersion.Location = new System.Drawing.Point(10, 307);
            this.ReportVersion.Name = "ReportVersion";
            this.ReportVersion.Size = new System.Drawing.Size(83, 13);
            this.ReportVersion.TabIndex = 40;
            this.ReportVersion.Text = "Report Version: ";
            // 
            // btnReportVersion
            // 
            this.btnReportVersion.Location = new System.Drawing.Point(97, 304);
            this.btnReportVersion.Name = "btnReportVersion";
            this.btnReportVersion.Size = new System.Drawing.Size(41, 20);
            this.btnReportVersion.TabIndex = 41;
            // 
            // btnReportName
            // 
            this.btnReportName.Location = new System.Drawing.Point(84, 282);
            this.btnReportName.Name = "btnReportName";
            this.btnReportName.Size = new System.Drawing.Size(417, 20);
            this.btnReportName.TabIndex = 44;
            this.btnReportName.Text = "Select CeLocale before opening a report: rassdk://d:\\\\RASReports\\\\formulas.rpt";
            // 
            // ReportName
            // 
            this.ReportName.AutoSize = true;
            this.ReportName.Location = new System.Drawing.Point(10, 286);
            this.ReportName.Name = "ReportName";
            this.ReportName.Size = new System.Drawing.Size(73, 13);
            this.ReportName.TabIndex = 45;
            this.ReportName.Text = "Report Name:";
            // 
            // rtnSQLStatement
            // 
            this.rtnSQLStatement.AutoSize = true;
            this.rtnSQLStatement.Location = new System.Drawing.Point(887, 0);
            this.rtnSQLStatement.Name = "rtnSQLStatement";
            this.rtnSQLStatement.Size = new System.Drawing.Size(83, 13);
            this.rtnSQLStatement.TabIndex = 47;
            this.rtnSQLStatement.Text = "Information Box:";
            // 
            // btnPrintToPrinter
            // 
            this.btnPrintToPrinter.Enabled = false;
            this.btnPrintToPrinter.Location = new System.Drawing.Point(559, 74);
            this.btnPrintToPrinter.Name = "btnPrintToPrinter";
            this.btnPrintToPrinter.Size = new System.Drawing.Size(84, 23);
            this.btnPrintToPrinter.TabIndex = 58;
            this.btnPrintToPrinter.Text = "P 2 P";
            this.btnPrintToPrinter.UseVisualStyleBackColor = true;
            this.btnPrintToPrinter.Click += new System.EventHandler(this.btnPrintToPrinter_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(247, 307);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 62;
            this.label11.Text = "Report Kind:";
            // 
            // btnReportKind
            // 
            this.btnReportKind.Location = new System.Drawing.Point(316, 304);
            this.btnReportKind.Name = "btnReportKind";
            this.btnReportKind.Size = new System.Drawing.Size(323, 20);
            this.btnReportKind.TabIndex = 63;
            // 
            // btnRecordCount
            // 
            this.btnRecordCount.Location = new System.Drawing.Point(981, 300);
            this.btnRecordCount.Name = "btnRecordCount";
            this.btnRecordCount.Size = new System.Drawing.Size(109, 20);
            this.btnRecordCount.TabIndex = 64;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(887, 303);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 65;
            this.label12.Text = "Data Row count:";
            // 
            // GetPreviewPagesStartwith
            // 
            this.GetPreviewPagesStartwith.FormattingEnabled = true;
            this.GetPreviewPagesStartwith.Items.AddRange(new object[] {
            "Full Size",
            "Fit Width",
            "Fit Page"});
            this.GetPreviewPagesStartwith.Location = new System.Drawing.Point(473, 360);
            this.GetPreviewPagesStartwith.Name = "GetPreviewPagesStartwith";
            this.GetPreviewPagesStartwith.Size = new System.Drawing.Size(120, 21);
            this.GetPreviewPagesStartwith.TabIndex = 67;
            // 
            // SetPreviewPagesStartWith
            // 
            this.SetPreviewPagesStartWith.Location = new System.Drawing.Point(598, 359);
            this.SetPreviewPagesStartWith.Name = "SetPreviewPagesStartWith";
            this.SetPreviewPagesStartWith.Size = new System.Drawing.Size(37, 23);
            this.SetPreviewPagesStartWith.TabIndex = 68;
            this.SetPreviewPagesStartWith.Text = "Set";
            this.SetPreviewPagesStartWith.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(150, 336);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 13);
            this.label15.TabIndex = 74;
            this.label15.Text = "DB Driver:";
            // 
            // btnDBDriver
            // 
            this.btnDBDriver.Location = new System.Drawing.Point(207, 333);
            this.btnDBDriver.Name = "btnDBDriver";
            this.btnDBDriver.Size = new System.Drawing.Size(252, 20);
            this.btnDBDriver.TabIndex = 75;
            // 
            // btnSQLStatement
            // 
            this.btnSQLStatement.Location = new System.Drawing.Point(890, 16);
            this.btnSQLStatement.Name = "btnSQLStatement";
            this.btnSQLStatement.Size = new System.Drawing.Size(332, 278);
            this.btnSQLStatement.TabIndex = 76;
            this.btnSQLStatement.Text = "Must be logged on before SQL can be retrieved";
            // 
            // btrRuntimeVersion
            // 
            this.btrRuntimeVersion.AutoSize = true;
            this.btrRuntimeVersion.Location = new System.Drawing.Point(10, 336);
            this.btrRuntimeVersion.Name = "btrRuntimeVersion";
            this.btrRuntimeVersion.Size = new System.Drawing.Size(46, 13);
            this.btrRuntimeVersion.TabIndex = 85;
            this.btrRuntimeVersion.Text = "Runtime";
            // 
            // txtRuntimeVersion
            // 
            this.txtRuntimeVersion.Location = new System.Drawing.Point(62, 333);
            this.txtRuntimeVersion.Name = "txtRuntimeVersion";
            this.txtRuntimeVersion.Size = new System.Drawing.Size(84, 20);
            this.txtRuntimeVersion.TabIndex = 86;
            // 
            // lblRPTRev
            // 
            this.lblRPTRev.Location = new System.Drawing.Point(194, 304);
            this.lblRPTRev.Name = "lblRPTRev";
            this.lblRPTRev.Size = new System.Drawing.Size(52, 20);
            this.lblRPTRev.TabIndex = 92;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(144, 307);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(51, 13);
            this.label18.TabIndex = 93;
            this.label18.Text = "Revision:";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(11, 261);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(96, 13);
            this.label.TabIndex = 98;
            this.label.Text = "CRD Save History:";
            // 
            // cbLastSaveHistory
            // 
            this.cbLastSaveHistory.FormattingEnabled = true;
            this.cbLastSaveHistory.Location = new System.Drawing.Point(112, 258);
            this.cbLastSaveHistory.MaxDropDownItems = 20;
            this.cbLastSaveHistory.Name = "cbLastSaveHistory";
            this.cbLastSaveHistory.Size = new System.Drawing.Size(389, 21);
            this.cbLastSaveHistory.TabIndex = 99;
            // 
            // btrRefreshViewer
            // 
            this.btrRefreshViewer.Location = new System.Drawing.Point(369, 226);
            this.btrRefreshViewer.Name = "btrRefreshViewer";
            this.btrRefreshViewer.Size = new System.Drawing.Size(78, 24);
            this.btrRefreshViewer.TabIndex = 101;
            this.btrRefreshViewer.Text = "Refresh";
            this.btrRefreshViewer.UseVisualStyleBackColor = true;
            // 
            // btnZoomFactor
            // 
            this.btnZoomFactor.Location = new System.Drawing.Point(465, 332);
            this.btnZoomFactor.Name = "btnZoomFactor";
            this.btnZoomFactor.Size = new System.Drawing.Size(105, 23);
            this.btnZoomFactor.TabIndex = 105;
            this.btnZoomFactor.Text = "Init. Zoom / Set";
            this.btnZoomFactor.UseVisualStyleBackColor = true;
            this.btnZoomFactor.Click += new System.EventHandler(this.btnZoomFactor_Click);
            // 
            // btnZoomFactorValue
            // 
            this.btnZoomFactorValue.Location = new System.Drawing.Point(588, 334);
            this.btnZoomFactorValue.Name = "btnZoomFactorValue";
            this.btnZoomFactorValue.Size = new System.Drawing.Size(51, 20);
            this.btnZoomFactorValue.TabIndex = 0;
            this.btnZoomFactorValue.Text = "100";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(572, 337);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(16, 13);
            this.label24.TabIndex = 106;
            this.label24.Text = "->";
            // 
            // btnSectionPrintOrientation
            // 
            this.btnSectionPrintOrientation.Location = new System.Drawing.Point(890, 328);
            this.btnSectionPrintOrientation.Name = "btnSectionPrintOrientation";
            this.btnSectionPrintOrientation.Size = new System.Drawing.Size(140, 23);
            this.btnSectionPrintOrientation.TabIndex = 110;
            this.btnSectionPrintOrientation.Text = "Section Print Orientation";
            this.btnSectionPrintOrientation.UseVisualStyleBackColor = true;
            this.btnSectionPrintOrientation.Click += new System.EventHandler(this.btnSectionPrintOrientation_Click);
            // 
            // btnCount
            // 
            this.btnCount.Location = new System.Drawing.Point(1091, 330);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(60, 20);
            this.btnCount.TabIndex = 111;
            // 
            // Count
            // 
            this.Count.AutoSize = true;
            this.Count.Location = new System.Drawing.Point(1047, 333);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(38, 13);
            this.Count.TabIndex = 112;
            this.Count.Text = "Count:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPOC);
            this.groupBox1.Controls.Add(this.cbP2P);
            this.groupBox1.Location = new System.Drawing.Point(507, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(136, 64);
            this.groupBox1.TabIndex = 116;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Viewer Print Mode";
            // 
            // cbPOC
            // 
            this.cbPOC.AutoSize = true;
            this.cbPOC.Location = new System.Drawing.Point(5, 41);
            this.cbPOC.Name = "cbPOC";
            this.cbPOC.Size = new System.Drawing.Size(122, 17);
            this.cbPOC.TabIndex = 1;
            this.cbPOC.Text = "PrintOutputController";
            this.cbPOC.UseVisualStyleBackColor = true;
            this.cbPOC.CheckedChanged += new System.EventHandler(this.cbPOC_CheckedChanged_1);
            // 
            // cbP2P
            // 
            this.cbP2P.AutoSize = true;
            this.cbP2P.Checked = true;
            this.cbP2P.Location = new System.Drawing.Point(5, 19);
            this.cbP2P.Name = "cbP2P";
            this.cbP2P.Size = new System.Drawing.Size(89, 17);
            this.cbP2P.TabIndex = 0;
            this.cbP2P.TabStop = true;
            this.cbP2P.Text = "PrintToPrinter";
            this.cbP2P.UseVisualStyleBackColor = true;
            this.cbP2P.CheckedChanged += new System.EventHandler(this.cbP2P_CheckedChanged_1);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1230, 779);
            this.Controls.Add(this.Count);
            this.Controls.Add(this.btnCount);
            this.Controls.Add(this.btnSectionPrintOrientation);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.btnZoomFactorValue);
            this.Controls.Add(this.btnZoomFactor);
            this.Controls.Add(this.btrRefreshViewer);
            this.Controls.Add(this.cbLastSaveHistory);
            this.Controls.Add(this.label);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.lblRPTRev);
            this.Controls.Add(this.txtRuntimeVersion);
            this.Controls.Add(this.btrRuntimeVersion);
            this.Controls.Add(this.btnSQLStatement);
            this.Controls.Add(this.btnDBDriver);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.SetPreviewPagesStartWith);
            this.Controls.Add(this.GetPreviewPagesStartwith);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnRecordCount);
            this.Controls.Add(this.btnReportKind);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnPrintToPrinter);
            this.Controls.Add(this.rtnSQLStatement);
            this.Controls.Add(this.ReportName);
            this.Controls.Add(this.btnReportName);
            this.Controls.Add(this.btnReportVersion);
            this.Controls.Add(this.ReportVersion);
            this.Controls.Add(this.btnCloserpt);
            this.Controls.Add(this.CRPageSetup);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.ViewReport);
            this.Controls.Add(this.btnPOController);
            this.Controls.Add(this.btnSaveRptAs);
            this.Controls.Add(this.btnOpenReport);
            this.Controls.Add(this.btnSetPrinter);
            this.Controls.Add(this.rdoDefault);
            this.Controls.Add(this.rdoCurrent);
            this.Controls.Add(this.grpBoxDefault);
            this.Controls.Add(this.grpBoxCurrent);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Printers";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpBoxDefault.ResumeLayout(false);
            this.grpBoxDefault.PerformLayout();
            this.grpBoxDefaultPaper.ResumeLayout(false);
            this.grpBoxCurrent.ResumeLayout(false);
            this.grpBoxCurrent.PerformLayout();
            this.grpBoxCurrentPaper.ResumeLayout(false);
            this.CRPageSetup.ResumeLayout(false);
            this.CRPageSetup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        
        static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void EnableDisableDefaultControls(bool flag)
		{
			Label2.Enabled = flag;
			Label5.Enabled = flag;
			Label6.Enabled = flag;		
			grpBoxDefault.Enabled = flag;
			grpBoxDefaultPaper.Enabled = flag;
			cboDefaultPrinters.Enabled = flag;
			cboDefaultPaperSizes.Enabled = flag;
			cboDefaultPaperTrays.Enabled = flag;
		}

		private void EnableDisableCurrentControls(bool flag) 
        {
			Label1.Enabled = flag;
			Label3.Enabled = flag;
			Label4.Enabled = flag;			
			grpBoxCurrent.Enabled = flag;
			grpBoxCurrentPaper.Enabled = flag;
			cboCurrentPrinters.Enabled = flag;
			cboCurrentPaperSizes.Enabled = flag;
			cboCurrentPaperTrays.Enabled = flag;
		}

        // The following routine loads all of the installed and/or available printers under the local user account and fills the drop down list
        // This a Windows Printer collection and not a CR API.
		private void frmMain_Load(object sender, System.EventArgs e)
		{
            if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0) 
			{
                foreach(String myPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters ) 
				{
					cboCurrentPrinters.Items.Add(myPrinter);
				}
				cboCurrentPrinters.SelectedIndex = 0;
			} 
			else
			{
				rdoCurrent.Enabled = false;
				EnableDisableCurrentControls(false);
			}
			//For printers exposed to System account as per MS Kbase 
			//http://support.microsoft.com/default.aspx?scid=kb;en-us;184291

			//Look to HKEY_USERS\.Default\Software\Microsoft\Windows NT\CurrentVersion\Devices
			Microsoft.Win32.RegistryKey mySystemPrinters = 
				Microsoft.Win32.Registry.Users.OpenSubKey(@".DEFAULT\Software\Microsoft\Windows NT\CurrentVersion\Devices");
			foreach (String defaultPrinters in mySystemPrinters.GetValueNames()) 
			{
				cboDefaultPrinters.Items.Add(defaultPrinters);
            }
			if (cboDefaultPrinters.Items.Count > 0) 
			{
				cboDefaultPrinters.SelectedIndex = 0;
			} 
			else 
			{
				rdoDefault.Enabled = false; 
			}
            
            // get the default values for the printers
            // WARNING: When Set Printer or P2P or POC is used this values is set. Must select same as the saved info 
            printerDuplexList.DataSource = System.Enum.GetValues(typeof(PrinterDuplex));
		}

		private void rdoDefault_CheckedChanged(object sender, System.EventArgs e)
		{
			EnableDisableDefaultControls(rdoDefault.Checked);
		}

		private void rdoCurrent_CheckedChanged(object sender, System.EventArgs e)
		{
			EnableDisableCurrentControls(rdoCurrent.Checked);		
		}

        private void cbP2P_CheckedChanged_1(object sender, EventArgs e)
        {
            if (cbP2P.Checked == false)
                crystalReportViewer1.PrintMode = PrintMode.PrintOutputController;
            else
                crystalReportViewer1.PrintMode = PrintMode.PrintToPrinter;
        }

        private void cbPOC_CheckedChanged_1(object sender, EventArgs e)
        {
 
        }

        // this displays all printers available under the Local User account, typically those they have installed.
		private void cboCurrentPrinters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			cboCurrentPaperSizes.Items.Clear(); 
			cboCurrentPaperTrays.Items.Clear();
			System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            System.Drawing.Printing.PageSettings pPage = new PageSettings();

            pDoc.PrinterSettings.PrinterName = this.cboCurrentPrinters.Text;

			foreach(System.Drawing.Printing.PaperSize myPaperSize in pDoc.PrinterSettings.PaperSizes) 
			{
                cboCurrentPaperSizes.Items.Add(myPaperSize.RawKind + ": " + myPaperSize.PaperName);
			}
			if (cboCurrentPaperSizes.Items.Count > 0) 
			{
				cboCurrentPaperSizes.SelectedIndex = 0;
			}
			foreach(System.Drawing.Printing.PaperSource  myPaperSource in pDoc.PrinterSettings.PaperSources) 
			{
				cboCurrentPaperTrays.Items.Add(myPaperSource.RawKind + ": " + myPaperSource.SourceName); 
			}
            if (cboCurrentPaperTrays.Items.Count > 0)
            {
                cboCurrentPaperTrays.SelectedIndex = 0;
            }
            CurrentUserDuplex.Text = pDoc.PrinterSettings.Duplex.ToString();
		}

        // this displays all printers available under the Admin account that has access to all printers.
		private void cboDefaultPrinters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			cboDefaultPaperSizes.Items.Clear(); 
			cboDefaultPaperTrays.Items.Clear(); 
			System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
			
			pDoc.PrinterSettings.PrinterName = this.cboDefaultPrinters.Text;
			foreach(System.Drawing.Printing.PaperSize myPaperSize in pDoc.PrinterSettings.PaperSizes) 
			{
                cboDefaultPaperSizes.Items.Add(myPaperSize.RawKind + ": " + myPaperSize.PaperName); 
			}
			if (cboDefaultPaperSizes.Items.Count > 0) 
			{
				cboDefaultPaperSizes.SelectedIndex = 0;
			}
			foreach(System.Drawing.Printing.PaperSource  myPaperSource in pDoc.PrinterSettings.PaperSources) 
			{
                cboDefaultPaperTrays.Items.Add(myPaperSource.RawKind + ": " + myPaperSource.SourceName); 
			}
			if (cboDefaultPaperTrays.Items.Count > 0) 
			{
				cboDefaultPaperTrays.SelectedIndex = 0;
			}
            DefaultUserDuplex.Text = pDoc.PrinterSettings.Duplex.ToString();
		}

        bool IsRpt = true;
        bool IsLoggedOn; // = false;
        bool CRUserDefined = false;
        bool NoPrinterNoDissoc = false;
        bool DesignPrinternotfound = false;
        bool NoPrinterWEB = false;

		private void grpBoxCurrent_Enter(object sender, System.EventArgs e)
		{
		
		}

        // Open a report and get and fill in the various info boxes
		private void btnOpenReport_Click(object sender, System.EventArgs e) 
		{
            rptClientDoc = new CrystalDecisions.ReportAppServer.ClientDoc.ReportClientDocument(); // ReportClientDocumentClass();
            // set up timers to show how long things take.
            DateTime dtStart;
            TimeSpan difference;

            openFileDialog.Filter = "Crystal Reports (*.rpt)|*.rpt|Crystal Reports Secure (*.rptr)|*.rptr";
            openFileDialog.FilterIndex = 1;

			if (openFileDialog.ShowDialog() == DialogResult.OK) 
			{
				btnOpenReport.Enabled = false;
				btnSaveRptAs.Enabled = false;
                btnCloserpt.Enabled = false;
                btrRefreshViewer.Enabled = false;
				object rptName = openFileDialog.FileName;

                dtStart = DateTime.Now;

                try
                {
                    rpt.Load(rptName.ToString(), OpenReportMethod.OpenReportByTempCopy);
                    difference = DateTime.Now.Subtract(dtStart);
                    btnSQLStatement.Text += "\nReport Document Loaded in: " + difference.Minutes.ToString() + ":" + difference.Seconds.ToString() + "\r\n";
                    cbLastSaveHistory.Text = "";
                    try
                    {
                        // Read File Details from CFileInfo Object
                        for (int x = 0; x < rpt.HistoryInfos.Count; x++)
                        {
                            cbLastSaveHistory.Items.Add(rpt.HistoryInfos[x].BuildVersion.ToString() + ": Date: " + rpt.HistoryInfos[x].SavedDate.ToString());
                        }

                        cbLastSaveHistory.SelectedIndex = 0; // rpt.HistoryInfos.Count - 1;
                        //SP 13
                        //•	RAS .NET SDK 
                        //ReportClientDocument.HistoryInfos[i].SavedDate
                        //ReportClientDocument.HistoryInfos[i].BuildVersion

                        //•	CR .NET SDK 
                        //ReportDocument.HistoryInfos[i].SavedDate
                        //ReportDocument.HistoryInfos[i].BuildVersion
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error: " + ex.Message);
                        cbLastSaveHistory.Text = "This report has no Save history";
                    }

                    // Enable various buttons if report opens
                    btnOpenReport.Enabled = false;
                    btnSaveRptAs.Enabled = true;
                    ViewReport.Enabled = true;
                    btnCloserpt.Enabled = true;
                    ViewReport.Enabled = true;
                    btnSetPrinter.Enabled = true;
                    btnPOController.Enabled = true;
                    btnPrintToPrinter.Enabled = true;
                    btrRefreshViewer.Enabled = true;
                    btnReportName.Text = rptName.ToString(); // +" Record Count: " + rpt.FormatEngine.GetLastPageNumber(new CrystalDecisions.Shared.ReportPageRequestContext()).ToString();
                    btnSavedPrinterName.Text = "Dissociate checked on";
                    btnReportKind.Text = rpt.ReportDefinition.ReportKind.ToString();
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() == "Object reference not set to an instance of an object.")
                        MessageBox.Show("ERROR: Object reference not set to an instance of an object.");
                    else
                        if (ex.Message.ToString() == "External component has thrown an exception.")
                        {
                            MessageBox.Show("ERROR: External component has thrown an exception.");
                        }
                        else
                        {
                            {
                                if (ex.InnerException.Message != null)
                                {
                                    MessageBox.Show("ERROR: " + ex.Message + " ;" + ex.InnerException.Message);

                                    ////Here you can set the exceptions to Search SAP SCN forums for specific errors. Note does need fine tuning.
                                    //string myURL = @"http://search.sap.com/ui/scn#query=" + ex.InnerException.Message + "&startindex=1&filter=scm_a_site%28scm_v_Site11%29&filter=scm_a_modDate%28*%29&timeScope=all";
                                    //string fixedString = myURL.Replace(" ", "%20");

                                    ////System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe", myURL);
                                    //System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe", fixedString);

                                    ////string myURL = @"C:\Program Files (x86)\SAP BusinessObjects\Crystal Reports 2011\Help\en\crw.chm";
                                    ////System.Diagnostics.Process.Start(myURL);
                                    ////cool but of no use to release mode

                                }
                                else
                                {
                                    MessageBox.Show("ERROR: " + ex.Message);
                                }
                            }
                        }

                    // Disable various buttons if report fails to open
                    btnOpenReport.Enabled = true;
                    ViewReport.Enabled = false;
                    btnSaveRptAs.Enabled = false;
                    ViewReport.Enabled = false; 
                    btnCloserpt.Enabled = false;
                    btnSetPrinter.Enabled = false;
                    btnPOController.Enabled = false;
                    btrRefreshViewer.Enabled = false;
                    btnReportKind.Text = "";
                    return;
                }
                
                // then clone the report, it may be needed later on
                rpt1 = (CrystalDecisions.CrystalReports.Engine.ReportDocument)rpt.Clone();
                // this assigns the report to RAS for modification
                rptClientDoc = rpt.ReportClientDocument;
                rptClientDoc1 = rpt.ReportClientDocument;

                // determine the report locale, must be set previous to opening the report:
                CrystalDecisions.ReportAppServer.DataDefModel.CeLocale preferredViewingLocaleID;
                preferredViewingLocaleID = rptClientDoc.LocaleID;

                // check if the report is based on a Command and if so then display the SQL otherwise indicate in Info Box DB connect is not active.
                int dbConCount = rptClientDoc.DatabaseController.GetConnectionInfos().Count;
                String DBDriver = "";
                for (int x = 0; x < dbConCount; x++)
                {
                    try
                    {
                        DBDriver = rptClientDoc.DatabaseController.GetConnectionInfos()[x].Attributes.get_StringValue("Database DLL").ToString();
                        btnDBDriver.Text += DBDriver + " :";
                        if (((dynamic)rptClientDoc.Database.Tables[0].Name) == "Command")
                        {
                            CrystalDecisions.ReportAppServer.Controllers.DatabaseController databaseController = rpt.ReportClientDocument.DatabaseController;
                            ISCRTable oldTable = (ISCRTable)databaseController.Database.Tables[0];

                            btnSQLStatement.Text = "Report is using Command Object: \n" + ((dynamic)oldTable).CommandText.ToString();
                            btnSQLStatement.Text += "\n";

                            IsLoggedOn = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        btnDBDriver.Text = "ERROR: " + ex.Message;
                        return;
                    }
                }

                if (dbConCount == 0)
                    btnDBDriver.Text = "NO Datasource in report";

                // SP 4
                btnReportKind.Text = "Eng: " + rpt.ReportDefinition.ReportKind.ToString() + " : RAS - " + rptClientDoc.ReportDefController.ReportDefinition.ReportKind.ToString();

                // SP 2 addition
                crystalReportViewer1.SetFocusOn(UIComponent.Page);
                
                // Record selection formula with comments included can only be retrieve via RAS
                CrystalDecisions.ReportAppServer.DataDefModel.ISCRFilter myRecordSelectionWithComments;
                myRecordSelectionWithComments = rptClientDoc.DataDefController.DataDefinition.RecordFilter;

                btnReportVersion.Text = rptClientDoc.MajorVersion.ToString() + "." + rptClientDoc.MinorVersion.ToString();
                // SP 13
                lblRPTRev.Text = rpt.SummaryInfo.RevisionNumber.ToString();

                // Now go get all of the saved printer info from the RPT file.
                // Note: if Saved Print Name and Paper size is missing you can only update this info using CR Designer and saving the report. 
                // Line above can be used to see the history and validate if the report has the saved paper info within.
                getPrinterInfoOnOpen(rpt);
            }

            // if the report has saved data show me the record count:
            // this causes a delay due to the engine having to go to the last page to get the count.
            try
            {
                btnRecordCount.Text = rpt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                btnRecordCount.Text = "No data in Report";
            }
		}

        // Now go get all of the saved printer info
        private void getPrinterInfoOnOpen(CrystalDecisions.CrystalReports.Engine.ReportDocument rpt)
        {
            CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();
            CrystalDecisions.CrystalReports.Engine.PrintOptions ENprOpts = rpt.PrintOptions;

            CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions rptPRT = new CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions();
            // cloning here just incase a paper size needs to be set when No Printer is saved in the RPT.
            CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions rptPRTCloned = new CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions();
            PrinterSettings ps = new PrinterSettings();
            PageSettings pgs = new PageSettings();

            rpt.PrintOptions.PaperSize.ToString();

            //Get the Duplex printer setting saved in the report
            SavedDuplexValue.Text = rpt.PrintOptions.PrinterDuplex.ToString();

            rptPRT = rptClientDoc.PrintOutputController.GetPrintOptions();
            rptPRTCloned = rptPRT.Clone(); // if making changes you will need to clone the collection.
            PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.DoNotScale;

            if (rpt.PrintOptions.SavedPrinterName.Length != 0)
            {
                btnSavedPrinterName.Text = rpt.PrintOptions.SavedPrinterName.ToString();
            }
            else
                btnSavedPrinterName.Text = "Printer is Display mode"; // rptPRT.SavedPrinterName.ToString();

            // The following is a single step process to determine what oprion have been saved and how to display that info
            // There is likely a much cleaner and dynamic way to do this so you are welcome to try... This works so I left it as is...
            //SP 8
            try
            {
                if (rptPRT.DriverName == "" && rptPRT.SavedDriverName.ToString() != "winspool") // Page Setup OK button was not clicked
                {
                    CRPrinterName.Text = "Printer is Display mode";
                    float MyHeight = (rptPRT.PageContentHeight / isMetric) + (rptPRT.PageMargins.Top / isMetric) + (rptPRT.PageMargins.Bottom / isMetric);
                    float MyWidth = (rptPRT.PageContentWidth / isMetric) + (rptPRT.PageMargins.Left / isMetric) + (rptPRT.PageMargins.Right / isMetric);
                    // this formats the values to look the same as in CRD
                    string MyWidthST = string.Format("{0:00.000}", MyWidth);
                    string MyHeightST = string.Format("{0:00.000}", MyHeight);
                    CRPaperSize.Text = "Horiz: " + MyWidthST.Trim('0') + " x Vert: " + MyHeightST.TrimStart('0');
                    //btnPaperSizeName.Text = "User Defined Paper size";
                    MyHeight = 0;
                    MyWidth = 0;
                }

                if (rptPRT.DriverName == "" && rptPRT.SavedDriverName.ToString() == "winspool") // Design printer not found
                {
                    CRPrinterName.Text = "Design Printer not found";
                    float MyHeight = (rptPRT.PageContentHeight / 1000); // This is the page size in ABS format so need to divide by 1000 to get into true value.
                    float MyWidth = (rptPRT.PageContentWidth / 1000); // This is the page size in ABS format so need to divide by 1000 to get into true value.
                    // this formats the values to look the same as in CRD
                    string MyWidthST = string.Format("{0:00.000}", MyWidth);
                    string MyHeightST = string.Format("{0:00.000}", MyHeight);
                    CRPaperSize.Text = "Horiz: " + MyWidthST.Trim('0') + " x Vert: " + MyHeightST.TrimStart('0');
                    CRUserDefined = true;
                    DesignPrinternotfound = true;
                }

                if (rptPRT.DriverName == "DISPLAY") // No Printer Selected or user defined paper size - bug in getting paper size for User Defined - ADAPT01728796
                {
                    CRPrinterName.Text = "No Printer Checked";
                    CrNoPrinter.Checked = true;
                    if (rptPRT.SavedPaperName.ToString() == "")
                        CRUserDefined = true;
                    else
                        CRUserDefined = false;
                    float MyHeight = (rptPRT.PageContentHeight / 1000) + (rptPRT.PageMargins.Top / 1440) + (rptPRT.PageMargins.Bottom / 1440);
                    float MyWidth = (rptPRT.PageContentWidth / 1000) + (rptPRT.PageMargins.Left / 1440) + (rptPRT.PageMargins.Right / 1440);
                    // this formats the values to look the same as in CRD
                    string MyWidthST = string.Format("{0:00.000}", MyWidth);
                    string MyHeightST = string.Format("{0:00.000}", MyHeight);
                    CRPaperSize.Text = "W: " + MyWidthST.Trim('0') + " x H: " + MyHeightST.TrimStart('0');
                    DesignPrinternotfound = true;
                    NoPrinterNoDissoc = true;
                    NoPrinterWEB = true;
                }

                if (rptPRT.DriverName == "DISPLAY" && rptPRT.DissociatePageSizeAndPrinterPaperSize.Equals(false)) // No Printer Selected or user defined paper size Dissociate checked off
                {
                    CRPrinterName.Text = "Default Printer used";
                    CRUserDefined = false;
                    if (rptPRT.SavedPaperName.ToString() != "")
                        CRUserDefined = true;
                    else
                        CRUserDefined = false;
                    NoPrinterNoDissoc = true;
                }

                //else // No Printer Selected or user defined paper size - bug in getting paper size for User Defined - ADAPT01728796
                if (rptPRT.DriverName == "DISPLAY" && rptPRT.DissociatePageSizeAndPrinterPaperSize.Equals(true)) // No Printer Selected or user defined paper size Dissociate checked on
                {
                    CRPrinterName.Text = "Default Printer used";
                    CRUserDefined = false;
                    if (rptPRT.SavedPaperName.ToString() != "")
                        CRUserDefined = true;
                    else
                        CRUserDefined = false;
                    //NoPrinterNoDissoc = true;
                }

                if (rptPRT.DriverName.ToString() == "winspool") // this changes to actual printer name if printer is installed
                {
                    CRPrinterName.Text = rptPRT.PrinterName.ToString(); // +"User Defined Paper Size";
                    if (rptPRT.PaperSize.ToString() == "crPaperSizeUser")
                        CRUserDefined = true;
                    else
                        CRUserDefined = false;
                    float MyHeight = (rptPRT.PageContentHeight / isMetric) + (rptPRT.PageMargins.Top / isMetric) + (rptPRT.PageMargins.Bottom / isMetric);
                    float MyWidth = (rptPRT.PageContentWidth / isMetric) +(rptPRT.PageMargins.Left / isMetric) + (rptPRT.PageMargins.Right / isMetric);
                    // this formats the values to look the same as in CRD
                    string MyWidthST = string.Format("{0:00.000}", MyWidth);
                    string MyHeightST = string.Format("{0:00.000}", MyHeight);
                    CRPaperSize.Text = "W: " + MyWidthST.Trim('0') + " x H: " + MyHeightST.TrimStart('0');
                    btnPaperSizeName.Text = rptPRT.SavedPaperName.ToString();
                    DesignPrinternotfound = false;
                }

                if (rptPRT.SavedDriverName.ToString() == "winspool") // this changes to actual printer name if installed
                {
                    if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
                    foreach (String myPrinter1 in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (myPrinter1.ToString() == rptPRT.SavedPrinterName.ToString())
                        {
                            CRPrinterName.Text = myPrinter1.ToString();
                        }
                        //else
                            //CRPrinterName.Text = "Design Printer not found";
                    }
                }

                if (rptPRT.DriverName == "" && rptPRT.SavedDriverName.ToString() == "" && rptPRT.DissociatePageSizeAndPrinterPaperSize.Equals(false)) // Use Default printer - dissociate unchecked
                {
                    CRPrinterName.Text = "Default Printer used";
                    CRUserDefined = false;
                    //NoPrinterNoDissoc = true;
                }
            }
            catch (Exception ex)
            {
                btnSQLStatement.Text = "Unknown Printer error: " + ex.Message;
                return;
            }

            // get the printer name if found
            if (rptPRT.PrinterName != "")
            {
                if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
                {
                    foreach (String myPrinter1 in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (myPrinter1.ToString() == rptPRT.PrinterName.ToString())
                        {
                            CRPrinterName.Text = rptPRT.PrinterName.ToString();
                        }
                    }
                }
            }

            // Now that the info has been determined here's more info on the default Paper Size ENUM's withing CR SDK.
            if (rptPRT.DriverName != "DISPLAY" && rptPRT.DriverName != "")
            #region PaperSource
            {
                if (System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count > 0)
                {
                    foreach (String myPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (myPrinter.ToString() == CRPrinterName.Text.ToString())
                        {
                            {
                                switch (CRPaperTray.Text)
                                {
                                    case "crPaperSourceAuto":
                                        CRPaperTray.Text = "Auto select";
                                        break;
                                    case "crPaperSourceCassette":
                                        CRPaperTray.Text = "Cassette";
                                        break;
                                    case "crPaperSourceEnvelope":
                                        CRPaperTray.Text = "Envelope";
                                        break;
                                    case "crPaperSourceEnvManual":
                                        CRPaperTray.Text = "Envelope manual feed";
                                        break;
                                    case "crPaperSourceFormSource":
                                        CRPaperTray.Text = "Form source";
                                        break;
                                    case "crPaperSourceLargeCapacity":
                                        CRPaperTray.Text = "Large capacity paper tray";
                                        break;
                                    case "crPaperSourceLargeFmt":
                                        CRPaperTray.Text = "Large format paper";
                                        break;
                                    case "crPaperSourceLower":
                                        CRPaperTray.Text = "Lower paper tray";
                                        break;
                                    case "crPaperSourceManual":
                                        CRPaperTray.Text = "Manual feed";
                                        break;
                                    case "crPaperSourceMiddle":
                                        CRPaperTray.Text = "Middle paper tray";
                                        break;
                                    case "crPaperSourceSmallFmt":
                                        CRPaperTray.Text = "Small format paper";
                                        break;
                                    case "crPaperSourceTractor":
                                        CRPaperTray.Text = "Tractor feed";
                                        break;
                                    case "crPaperSourceUpper":
                                        CRPaperTray.Text = "Upper paper tray";
                                        break;
                                    default:
                                        CRPaperTray.Text = "User Defined";
                                        break;
                                }
                            }
                        }
                    }
                }
                #endregion PaperSource
            }

            // MUST GET THIS FROM THE REPORT !!!!! AND THEN BE ABLE TO SET IT Fixed in SP 5
            CRPaperTray.Text += ": " + rptPRT.PaperSource.ToString();
            rpt.PrintOptions.PaperSource = rpt.PrintOptions.PaperSource;

            // Member                        Description 
            // crPaperSourceAuto             Auto select.  
            // crPaperSourceCassette         Cassette.  
            // crPaperSourceEnvelope         Envelope.  
            // crPaperSourceEnvManual        Envelope manual feed.  
            // crPaperSourceFormSource       Form source.  
            // crPaperSourceLargeCapacity    Large capacity paper tray.  
            // crPaperSourceLargeFmt         Large format paper.  
            // crPaperSourceLower            Lower paper tray.  
            // crPaperSourceManual           Manual feed.  
            // crPaperSourceMiddle           Middle paper tray.  
            // crPaperSourceSmallFmt         Small format paper.  
            // crPaperSourceTractor          Tractor feed.  
            // crPaperSourceUpper            Upper paper tray.  

            // If the printer exists then I can get the paper tray from the printer
            //System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            //CRPrinterName.Text = rpt.PrintOptions.PrinterName.ToString();

            // this info is saved in the RPT file
            if (rptPRT.PaperOrientation.ToString() != "crPaperOrientationPortrait")
            {
                PROrientation.Text = "LandScape";
            }
            else
            {
                PROrientation.Text = "Portrait";
            }

            // show margins
            // values are saved in TWIPS in the RPT file
            if (isMetric == 567)
                isMetricTwips = 0.0017639;
            else
                isMetricTwips = 1440;

            double CRLeftInt = (rptPRT.PageMargins.Left * isMetricTwips);
            CRLeft.Text = string.Format("{0:0.000}", CRLeftInt);
            double CRRightInt = (rptPRT.PageMargins.Right * isMetricTwips);
            CRRight.Text = string.Format("{0:0.000}", CRRightInt);
            double CRTopInt = (rptPRT.PageMargins.Top * isMetricTwips);
            CRTop.Text = string.Format("{0:0.000}", CRTopInt);
            double CRBottomInt = (rptPRT.PageMargins.Bottom * isMetricTwips);
            CRBottom.Text = string.Format("{0:0.000}", CRBottomInt);

            // Check if the Dissociate option is checked in the RPT and check the box accordingly.
            if (rptPRT.DissociatePageSizeAndPrinterPaperSize.Equals(true))
            {
                chkDissociate.Checked = true;
            }
            else
            {
                chkDissociate.Checked = false;
            }

            string MypSize = rptPRT.PaperSize.ToString();

            // If No printer is saved in the RPT - You did not click on Page Setup then the default 13 paper sizes are assigned to this value
            if ((!CRUserDefined && !NoPrinterNoDissoc) || DesignPrinternotfound && !NoPrinterWEB) // Note: bug in getting paper size for User Defined - ADAPT01728796 resolved the issue
            #region CRPaperDefaultSize;
            {
                switch (MypSize)
                {
                    case "crPaperSizePaper10x14":
                        CRPaperSize.Text = "10x14";
                        break;
                    case "crPaperSizePaper11x17":
                        CRPaperSize.Text = "11x17";
                        break;
                    case "crPaperSizePaperA3":
                        CRPaperSize.Text = "A3";
                        break;
                    case "crPaperSizePaperA4":
                        CRPaperSize.Text = "A4";
                        break;
                    case "crPaperSizePaperA4Small":
                        CRPaperSize.Text = "A4 Small";
                        break;
                    case "crPaperSizePaperA5":
                        CRPaperSize.Text = "A5";
                        break;
                    case "crPaperSizePaperB4":
                        CRPaperSize.Text = "B4";
                        break;
                    case "crPaperSizePaperB5":
                        CRPaperSize.Text = "B5";
                        break;
                    case "crPaperSizePaperCsheet":
                        CRPaperSize.Text = "C sheet";
                        break;
                    case "crPaperSizePaperDsheet":
                        CRPaperSize.Text = "D sheet";
                        break;
                    case "crPaperSizePaperEnvelope10":
                        CRPaperSize.Text = "Envelope10";
                        break;
                    case "crPaperSizePaperEnvelope11":
                        CRPaperSize.Text = "Envelope11";
                        break;
                    case "crPaperSizePaperEnvelope12":
                        CRPaperSize.Text = "Envelope12";
                        break;
                    case "crPaperSizePaperEnvelope14":
                        CRPaperSize.Text = "Envelope14";
                        break;
                    case "crPaperSizePaperEnvelope9":
                        CRPaperSize.Text = "Envelope9";
                        break;
                    case "crPaperSizePaperEnvelopeB4":
                        CRPaperSize.Text = "EnvelopeB4";
                        break;
                    case "crPaperSizePaperEnvelopeB5":
                        CRPaperSize.Text = "EnvelopeB5";
                        break;
                    case "crPaperSizePaperEnvelopeB6":
                        CRPaperSize.Text = "EnvelopeB6";
                        break;
                    case "crPaperSizePaperEnvelopeC3":
                        CRPaperSize.Text = "EnvelopeC3";
                        break;
                    case "crPaperSizePaperEnvelopeC4":
                        CRPaperSize.Text = "EnvelopeC4";
                        break;
                    case "crPaperSizePaperEnvelopeC5":
                        CRPaperSize.Text = "EnvelopeC5";
                        break;
                    case "crPaperSizePaperEnvelopeC6":
                        CRPaperSize.Text = "EnvelopeC6";
                        break;
                    case "crPaperSizePaperEnvelopeC65":
                        CRPaperSize.Text = "EnvelopeC65";
                        break;
                    case "crPaperSizePaperEnvelopeDL":
                        CRPaperSize.Text = "EnvelopeDL";
                        break;
                    case "crPaperSizePaperEnvelopeItaly":
                        CRPaperSize.Text = "EnvelopeItaly";
                        break;
                    case "crPaperSizePaperEnvelopeMonarch":
                        CRPaperSize.Text = "EnvelopeMonarch";
                        break;
                    case "crPaperSizePaperEnvelopePersonal":
                        CRPaperSize.Text = "Personal";
                        break;
                    case "crPaperSizePaperEsheet":
                        CRPaperSize.Text = "Esheet";
                        break;
                    case "crPaperSizePaperExecutive":
                        CRPaperSize.Text = "Executive";
                        break;
                    case "crPaperSizePaperFanfoldLegalGerman":
                        CRPaperSize.Text = "FanfoldLegalGerman";
                        break;
                    case "crPaperSizePaperFanfoldStdGerman":
                        CRPaperSize.Text = "FanfoldStdGerman";
                        break;
                    case "crPaperSizePaperFanfoldUS":
                        CRPaperSize.Text = "FanfoldUS";
                        break;
                    case "crPaperSizePaperFolio":
                        CRPaperSize.Text = "Folio";
                        break;
                    case "crPaperSizePaperLedger":
                        CRPaperSize.Text = "Ledger";
                        break;
                    case "crPaperSizePaperLegal":
                        CRPaperSize.Text = "Legal";
                        break;
                    case "crPaperSizePaperLetter":
                        CRPaperSize.Text = "Letter";
                        break;
                    case "crPaperSizePaperLetterSmall":
                        CRPaperSize.Text = "LetterSmall";
                        break;
                    case "crPaperSizePaperNote":
                        CRPaperSize.Text = "Note";
                        break;
                    case "crPaperSizePaperQuarto":
                        CRPaperSize.Text = "Quarto";
                        break;
                    case "crPaperSizePaperStatement":
                        CRPaperSize.Text = "Statement";
                        break;
                    case "crPaperSizePaperTabloid":
                        CRPaperSize.Text = "Tabloid";
                        break;
                    case "crPaperSizeDefault":
                        CRPaperSize.Text = CRPaperSize.Text; // + ": " + rptPRT.PaperName.ToString();
                        btnPaperSizeName.Text = "crPaperSizeDefault"; //"Page Setup not saved";
                        break;
                    case "crPaperSizeUser": // bug in getting paper size for User Defined - ADAPT01728796
                        CRPaperSize.Text = "User Defined: " + rptPRT.PaperSize.ToString();
                        //btnPaperSizeName.Text = rptPRT.PaperName.ToString();
                        break;
                    default: // this is printer defined if printer is installed and custom paper size is defined locally
                        //CRPaperSize.Text = "Printer Defined: " + rptPRT.PaperSize.ToString();
                        //CRPaperSize.Text = ;
                        btnPaperSizeName.Text = "ENUM: " + rptPRT.PaperSize.ToString() + " Name: " + rptPRT.SavedPaperName.ToString();
                        break;
                }
            #endregion CRPaperDefaultSize;
            }
            else // getting paper size for User Defined
                if (!NoPrinterNoDissoc && MypSize == "")
                {
                    btnPaperSizeName.Text = "User Defined- No Printer Size defined"; // use values that were previous set after getting them from the report file.
                }
                else
                {
                    switch (MypSize) // no printer and Dissocaite checked, these are the embedded Paper Sizes
                    {
                        case "crPaperSizePaperA3":
                            CRPaperSize.Text = "A3 (297x420mm)";
                            break;
                        case "crPaperSizePaperA4":
                            CRPaperSize.Text = "A4 (210x297mm)";
                            break;
                        case "crPaperSizePaperA4Small":
                            CRPaperSize.Text = "A4 Small(210x297mm)";
                            break;
                        case "crPaperSizePaperA5":
                            CRPaperSize.Text = "A5 (148x210mm)";
                            break;
                        case "crPaperSizePaperB4":
                            CRPaperSize.Text = "B4 (250x354mm)";
                            break;
                        case "crPaperSizePaperB5":
                            CRPaperSize.Text = "B5 (182x257mm)";
                            break;
                        case "crPaperSizePaperExecutive":
                            CRPaperSize.Text = "7.25x 10.5\" (Executive)";
                            break;
                        case "crPaperSizePaperLedger":
                            CRPaperSize.Text = "17x11\" (Ledger)";
                            break;
                        case "crPaperSizePaperLegal":
                            CRPaperSize.Text = "8.5x14\" (Legal)";
                            break;
                        case "crPaperSizePaperLetter":
                            CRPaperSize.Text = "8.5x11\" (Letter)";
                            break;
                        case "crPaperSizePaperLetterSmall":
                            CRPaperSize.Text = "8.5x11\" (Letter Small)";
                            break;
                        case "crPaperSizePaperStatement":
                            CRPaperSize.Text = "5.5x8.5\" (Statement)";
                            break;
                        case "crPaperSizePaperTabloid":
                            CRPaperSize.Text = "11x17\" (Tabloid)";
                            break;
                        case "crPaperSizeUser":
                            //Paper size in text box is handled above nothing to update here
                            break;
                        default:
                            if (btnPaperSizeName.Text != "crPaperSizeDefault" || btnPaperSizeName.Text != "")
                                btnPaperSizeName.Text = "ENUM: " + rptPRT.PaperSize.ToString();
                            if (rptPRT.SavedPaperName != null)
                                btnPaperSizeName.Text += " Name: " + rptPRT.SavedPaperName.ToString();
                            break;
                    }
                }
            NoPrinterNoDissoc = true;
        }

        private void Closerpt_Click(object sender, EventArgs e)
        {
            btnOpenReport.Enabled = true;
            btnSaveRptAs.Enabled = false;
            btnCloserpt.Enabled = false;
            ViewReport.Enabled = false;
            btnSetPrinter.Enabled = false;
            btnPOController.Enabled = false;
            btnPrintToPrinter.Enabled = false;
            btnReportVersion.Text = "";
            btnReportName.Text = "Select CeLocale before opening a report";
            btnSQLStatement.Text = "Log on must be set first before SQL can be retrieved";
            CRPaperTray.Text = "";
            btnSavedPrinterName.Text = "";
            btnReportKind.Text = "";
            btnRecordCount.Text = "";
            GetPreviewPagesStartwith.Text = "";
            btnDBDriver.Text = "";
            btnDBDriver.Text = "";
            btnPaperSizeName.Text = "";
            crystalReportViewer1.SetProductLocale(1033);
            CRUserDefined = false;
            NoPrinterNoDissoc = false;
            DesignPrinternotfound = false;
            NoPrinterWEB = false;
            lblRPTRev.Text = "";
            cbLastSaveHistory.Items.Clear();
            cbLastSaveHistory.Text = "";
            btnZoomFactorValue.Text = "100";
            btrRefreshViewer.Enabled = false;

            chkbUseThisDuplex.Checked = false;
            CrNoPrinter.Checked = false;
            chkDissociate.Checked = false;
            CRPaperSize.Text = "";
            PROrientation.Text = "";
            CRLeft.Text = "";
            CRRight.Text = "";
            CRTop.Text = "";
            CRBottom.Text = "";
            CRPrinterName.Text = "";
            IsLoggedOn = false;

            // close report source
            rpt.Close();

            crystalReportViewer1.ReportSource = null;
            if (!crystalReportViewer1.Disposing)
                btnSQLStatement.Text += "Viewer is disposing";
            else
                btnSQLStatement.Text += "Viewer is disposed";
            crystalReportViewer1.Refresh();
            GC.Collect();
            IsRpt = true;

        }

		private void btnSaveReportAs_Click(object sender, System.EventArgs e)
		{
            saveFileDialog.Filter = "Crystal Reports (*.rpt)|*.rpt";
            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {

                object saveFolder = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                string saveFileName = System.IO.Path.GetFileName(saveFileDialog.FileName);

                if (!IsRpt)
                {
                    rptClientDoc.SaveAs(saveFileName, ref saveFolder,
                        (int)CdReportClientDocumentSaveAsOptionsEnum.cdReportClientDocumentSaveAsOverwriteExisting);
                }
                else
                {
                    try
                    {
                        rpt.SaveAs(saveFolder + "\\" + saveFileName, true);
                    }
                    catch (Exception ex)
                    {
                        btnSQLStatement.Text = "ERROR: " + ex.Message;
                    }
                }
            }
		}

        // update the selected printer to the report
		private void btnSetPrinter_Click(object sender, System.EventArgs e)
		{
            // THIS WORKS BUT IT DOESN'T UPDATE THE REPORT SETTINGS - No Printer to checked off
            System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            System.Drawing.Printing.PageSettings page = new System.Drawing.Printing.PageSettings();

            CrystalDecisions.CrystalReports.Engine.PrintOptions printOptions = rpt.PrintOptions;

            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions rasPROpts = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
            CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions newOpts = new CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions();
            CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions newOptsCloned = new CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions();

            CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();
            CrystalDecisions.ReportAppServer.ReportDefModel.PageMargins crMarg = new CrystalDecisions.ReportAppServer.ReportDefModel.PageMargins();

            // DISPLAY = no printer. 
            // See this blog on how to set no printer http://scn.sap.com/community/crystal-reports-for-visual-studio/blog/2010/09/15/how-to-check-no-printer-on-a-crystal-report-using-the-ras-sdk-in-net
            // requires cloning a report with no printer not checked and then using those properties in the real report
            if (CrNoPrinter.Checked == true)
            {
                CrystalDecisions.CrystalReports.Engine.ReportDocument rpt1 = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc2;

                rpt1.Load(@"c:\reports\ClonePrinterInfo.rpt"); // this report is simply a blank report with the desired printer info saved in it.
                rptClientDoc2 = rpt1.ReportClientDocument;
                newOptsCloned = rptClientDoc2.PrintOutputController.GetPrintOptions();
                newOpts = newOptsCloned;
                
                if (chkDissociate.Checked == true)
                    newOptsCloned.DissociatePageSizeAndPrinterPaperSize = true;
                else
                    newOptsCloned.DissociatePageSizeAndPrinterPaperSize = false;

                rptClientDoc.PrintOutputController.ModifyPrintOptions(newOptsCloned); 
                rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationLandscape);
                rptClientDoc.PrintOutputController.ModifyPrinterName(newOptsCloned.PrinterName);
                rptClientDoc.PrintOutputController.ModifyUserPaperSize(newOptsCloned.PageContentHeight, newOptsCloned.PageContentWidth);
                rptClientDoc.PrintOutputController.ModifyPageMargins(newOptsCloned.PageMargins.Left, newOptsCloned.PageMargins.Right, newOptsCloned.PageMargins.Top, newOptsCloned.PageMargins.Bottom);
            }
            else
            {
                if (rdoCurrent.Checked)
                {
                    newOpts.PrinterName = cboCurrentPrinters.SelectedItem.ToString();
                    newOpts.DriverName = "winspool";

                    string MySTRTemp = cboCurrentPaperSizes.SelectedItem.ToString();
                    int MyENUM = 0;
                    int MyENUM1 = 0;

                    if (chkDissociate.Checked == true)
                        newOpts.DissociatePageSizeAndPrinterPaperSize = true;
                    else
                        newOpts.DissociatePageSizeAndPrinterPaperSize = false;

                    // parse the enum values from the drop down list boxes
                    MyENUM = MySTRTemp.LastIndexOf(@":");
                    MySTRTemp = cboCurrentPaperSizes.Text.Substring(0, MyENUM);
                    
                    // this will return false if enum cannot be converted
                    bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                    newOpts.PaperSize = (CrPaperSizeEnum)MyENUM1;
                    //newOpts.PaperSize = cboCurrentPaperSizes.SelectedIndex;
                    CRPaperSize.Text = cboCurrentPaperSizes.SelectedItem.ToString();

                    // Set the selected Duplex setting and update the report
                    if (chkbUseThisDuplex.Checked)
                        newOpts.PrinterDuplex = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPrinterDuplexEnum)printerDuplexList.SelectedValue;

                    if (cboCurrentPaperTrays.SelectedItem != null)
                    {
                        string MyPaperSource = cboCurrentPaperTrays.SelectedItem.ToString();
                        MySTRTemp = cboCurrentPaperTrays.SelectedItem.ToString();
                        MyENUM = MySTRTemp.LastIndexOf(@":");
                        MySTRTemp = cboCurrentPaperTrays.Text.Substring(0, MyENUM);
                    }

                    // this will return false if enum cannot be converted
                    myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                    newOpts.PaperSource = (CrPaperSourceEnum)MyENUM1;
                }
                else
                {
                    newOpts.PrinterName = cboDefaultPrinters.SelectedItem.ToString();
                    newOpts.DriverName = "winspool";

                    string MySTRTemp = cboDefaultPaperSizes.SelectedItem.ToString();
                    int MyENUM = 0;
                    int MyENUM1 = 0;

                    if (chkDissociate.Checked == true)
                        newOpts.DissociatePageSizeAndPrinterPaperSize = true;
                    else
                        newOpts.DissociatePageSizeAndPrinterPaperSize = false;

                    // parse the enum values from the drop down list boxes
                    MyENUM = MySTRTemp.LastIndexOf(@":");
                    MySTRTemp = cboDefaultPaperSizes.Text.Substring(0, MyENUM);

                    // this will return false if enum cannot be converted
                    bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                    newOpts.PaperSize = (CrPaperSizeEnum)MyENUM1;
                    //newOpts.PaperSize = cboCurrentPaperSizes.SelectedIndex;
                    CRPaperSize.Text = cboDefaultPaperSizes.SelectedItem.ToString();

                    // Set the selected Duplex setting and update the report
                    if (chkbUseThisDuplex.Checked)
                        newOpts.PrinterDuplex = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPrinterDuplexEnum)printerDuplexList.SelectedValue;

                    string MyPaperSource = cboDefaultPaperTrays.SelectedItem.ToString();
                    MySTRTemp = cboDefaultPaperTrays.SelectedItem.ToString();
                    MyENUM = MySTRTemp.LastIndexOf(@":");
                    MySTRTemp = cboDefaultPaperTrays.Text.Substring(0, MyENUM);

                    // this will return false if enum cannot be converted
                    myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                    newOpts.PaperSource = (CrPaperSourceEnum)MyENUM1;
                }
                //Neu JNE
                if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                {
                    // set page margins from centimeters to twips
                    crMarg.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) / isMetricTwips);
                    crMarg.Right = Convert.ToInt32(Double.Parse(CRRight.Text) / isMetricTwips);
                    crMarg.Top = Convert.ToInt32(Double.Parse(CRTop.Text) / isMetricTwips);
                    crMarg.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) / isMetricTwips);
                }
                else
                {
                    // set page margins to inches to twips
                    //crMarg.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) * isMetric);
                    //crMarg.Right = Convert.ToInt32(Double.Parse(CRRight.Text) * isMetric);
                    //crMarg.Top = Convert.ToInt32(Double.Parse(CRTop.Text) * isMetric);
                    //crMarg.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) * isMetric);
                }

                // set page margins
                //crMarg.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) * isMetric);
                //crMarg.Right = Convert.ToInt32(Double.Parse(CRRight.Text) * isMetric);
                //crMarg.Top = Convert.ToInt32(Double.Parse(CRTop.Text) * isMetric);
                //crMarg.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) * isMetric);
                newOpts.PageMargins = crMarg;

                // this info is saved in the RPT file
                if (newOpts.PaperOrientation.ToString() != "crPaperOrientationPortrait")
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationLandscape);
                }
                else
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationPortrait);
                }

                CrNoPrinter.Checked = false;
                CRPrinterName.Text = newOpts.PrinterName;
                if (cboCurrentPaperTrays.SelectedItem != null)
                    CRPaperTray.Text = cboCurrentPaperTrays.SelectedItem.ToString();
                else
                    CRPaperTray.Text = "Printer does not support Paper Trays";
                
                // using RAS to update the Printer properties
                try
                {
                    rptClientDoc.PrintOutputController.ModifyPrintOptions(newOpts);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return;
                }
            }

            // Now that RAS has modified the report use this flag to set the viewers report source to the RAS Report object
            IsRpt = false;
		}

        // Using the POC - PrintOutputController to print the report
        private void btnPOController_Click_1(object sender, System.EventArgs e) // Print To P button
		{
            System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions rasPROpts = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
            CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();
            CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions RASPO = new CrystalDecisions.ReportAppServer.ReportDefModel.PrintOptions();
            CrystalDecisions.ReportAppServer.ReportDefModel.PageMargins crMarg = new CrystalDecisions.ReportAppServer.ReportDefModel.PageMargins();

            RASPO = rptClientDoc.PrintOutputController.GetPrintOptions();

            //pDoc.DefaultPageSettings.PrinterSettings.FromPage = 1;
            //pDoc.DefaultPageSettings.PrinterSettings.ToPage = 1;
  
            switch (GetPreviewPagesStartwith.Text)
            {
                case "Full Size":
                {
                    PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.DoNotScale;
                    //PrintLayout.Centered = true;
                    break;
                }
                case "Fit Width":
                {
                    PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.ShrinkOnly;
                    //PrintLayout.FitHorizontalPages = true;
                    break;
                }
                case "Fit Page":
                {
                    PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.Scale;
                    break;
                }
            }

            if (rdoCurrent.Checked)
            {
                //if (pDoc.PrinterSettings.DefaultPageSettings.Color)
                //{
                //    MessageBox.Show("Printer Supports Color", pDoc.PrinterSettings.SupportsColor.ToString());
                //}

                rasPROpts.PrinterName = cboCurrentPrinters.Text;
                //Yunfeng: 
                int[] sizes = PaperSizeGetter.Get_PaperSizes(cboCurrentPrinters.Text, "");
                int paperSizeid = sizes[this.cboCurrentPaperSizes.SelectedIndex];
                if (paperSizeid > 0)
                {
                    rasPROpts.PaperSize = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPaperSizeEnum)paperSizeid;
                }

                // parse the enum values from the drop down list boxes and set the Paper tray
                // Paper tray
                string MySTRTemp = cboCurrentPaperTrays.SelectedItem.ToString();
                int MyENUM = 0;
                int MyENUM1 = 0;

                string MyPaperSource = cboCurrentPaperTrays.SelectedItem.ToString();
                MySTRTemp = cboCurrentPaperTrays.SelectedItem.ToString();
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboCurrentPaperTrays.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rasPROpts.PaperSource = (CrPaperSourceEnum)MyENUM1;

                // By default all pages are printed. If you want to allow setting a range then create a UI for it.
                //int st;
                //int ot;
                //rasPROpts.NumberOfCopies = 1;
                //rasPROpts.AddPrinterPageRange(1, 1);
                //rasPROpts.AddPrinterPageRange(5, 7);
                //rasPROpts.GetNthPrinterPageRange(1, st, ot);

                // Set the selected Duplex setting and update the report
                if (chkbUseThisDuplex.Checked)
                    rasPROpts.PrinterDuplex = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPrinterDuplexEnum)printerDuplexList.SelectedValue;

                // this info is saved in the RPT file
                if (RASPO.PaperOrientation.ToString() != "crPaperOrientationPortrait")
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationLandscape);
                }
                else
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationPortrait);
                }

                string MyRptName = rpt.FileName.ToString();
                MyRptName = MyRptName.Substring(MyRptName.LastIndexOf(@"\") + 1, (rpt.FileName.Length - 3) - (MyRptName.LastIndexOf(@"\") + 2));
                rasPROpts.JobTitle = MyRptName;
            }
            else // default list
            {
                rasPROpts.PrinterName = cboCurrentPrinters.Text;

                int[] sizes = PaperSizeGetter.Get_PaperSizes(cboCurrentPrinters.Text, "");
                int paperSizeid = sizes[this.cboDefaultPaperSizes.SelectedIndex];
                if (paperSizeid > 0)
                {
                    rasPROpts.PaperSize = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPaperSizeEnum)paperSizeid;
                }

                // parse the enum values from the drop down list boxes and set the Paper tray
                // Paper tray
                string MySTRTemp = cboDefaultPaperTrays.SelectedItem.ToString();
                int MyENUM = 0;
                int MyENUM1 = 0;

                string MyPaperSource = cboDefaultPaperTrays.SelectedItem.ToString();
                MySTRTemp = cboDefaultPaperTrays.SelectedItem.ToString();
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboDefaultPaperTrays.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rasPROpts.PaperSource = (CrPaperSourceEnum)MyENUM1;

                // By default all pages are printed. If you want to allow setting a range then create a UI for it.
                //int st;
                //int ot;
                //rasPROpts.NumberOfCopies = 1;
                //rasPROpts.AddPrinterPageRange(1, 1);
                //rasPROpts.AddPrinterPageRange(5, 7);
                //rasPROpts.GetNthPrinterPageRange(1, st, ot);

                // this info is saved in the RPT file
                if (RASPO.PaperOrientation.ToString() != "crPaperOrientationPortrait")
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationLandscape);
                }
                else
                {
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationPortrait);
                }

                // Get/Set the Duplex of the printer if it's allowed
                // Set the selected Duplex setting and update the report
                if (chkbUseThisDuplex.Checked)
                    rasPROpts.PrinterDuplex = (CrystalDecisions.ReportAppServer.ReportDefModel.CrPrinterDuplexEnum)printerDuplexList.SelectedValue;

                if (chkDissociate.Checked == true)
                {
                    RASPO.DissociatePageSizeAndPrinterPaperSize = true;
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationPortrait);
                }
                else
                {
                    RASPO.DissociatePageSizeAndPrinterPaperSize = false;
                    rptClientDoc.PrintOutputController.ModifyPaperOrientation(CrPaperOrientationEnum.crPaperOrientationLandscape);
                }

                string MyRptName = rpt.FileName.ToString();
                MyRptName = MyRptName.Substring(MyRptName.LastIndexOf(@"\") + 1, (rpt.FileName.Length - 3) - (MyRptName.LastIndexOf(@"\") + 2));
                rasPROpts.JobTitle = MyRptName;
            }

            if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
            {
                // set page margins from centimeters to twips
                crMarg.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) / isMetricTwips);
                crMarg.Right = Convert.ToInt32(Double.Parse(CRRight.Text) / isMetricTwips);
                crMarg.Top = Convert.ToInt32(Double.Parse(CRTop.Text) / isMetricTwips);
                crMarg.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) / isMetricTwips);
            }
            else
            {
                // set page margins to inches to twips
                crMarg.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) * isMetric);
                crMarg.Right = Convert.ToInt32(Double.Parse(CRRight.Text) * isMetric);
                crMarg.Top = Convert.ToInt32(Double.Parse(CRTop.Text) * isMetric);
                crMarg.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) * isMetric);
            }
            RASPO.PageMargins = crMarg;

            // using RAS to update the Printer properties
            try
            {
                rptClientDoc.PrintOutputController.ModifyPrintOptions(RASPO);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return;
            }

            try
            {
                rptClientDoc.PrintOutputController.PrintReport(rasPROpts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
                return;
            }
            //MessageBox.Show("Printing report.", "RAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            IsRpt = false; 
		}

        // Using the P2P - PrintToPrinter or Viewers Print Button to print the report.
        private void btnPrintToPrinter_Click(object sender, EventArgs e)
        {
            System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            CrystalDecisions.Shared.PrintLayoutSettings PrintLayout = new CrystalDecisions.Shared.PrintLayoutSettings();
            System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();

            if (rdoCurrent.Checked)
            {
                printerSettings.PrinterName = cboCurrentPrinters.SelectedItem.ToString();

                System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                // Get/Set the Duplex of the printer if it's allowed
                // Set the selected Duplex setting and update the report
                rpt.PrintOptions.PrinterDuplex = (CrystalDecisions.Shared.PrinterDuplex)printerDuplexList.SelectedValue;

                pSettings.PrinterSettings.PrinterName = cboCurrentPrinters.SelectedItem.ToString();

                // parse the enum values from the drop down list boxes and set the Paper Size
                string MySTRTemp = cboCurrentPaperSizes.SelectedItem.ToString();
                int MyENUM = 0;
                int MyENUM1 = 0;
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboCurrentPaperSizes.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rpt.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)MyENUM1;

                // Paper tray
                string MyPaperSource = cboCurrentPaperTrays.SelectedItem.ToString();
                MySTRTemp = cboCurrentPaperTrays.SelectedItem.ToString();
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboCurrentPaperTrays.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rpt.PrintOptions.PaperSource = (CrystalDecisions.Shared.PaperSource)MyENUM1;

                //pSettings.Margins.Left = 1;
                //pSettings.Margins.Right = 1;
                //pSettings.Margins.Top = 1;
                //pSettings.Margins.Bottom = 1;
                //pSettings.PaperSource = pSettings.PaperSource;

                rpt.PrintOptions.PrinterName = printerSettings.PrinterName.ToString();

                if (chkDissociate.Checked == true)
                {
                    rpt.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
                }
                else
                {
                    rpt.PrintOptions.DissociatePageSizeAndPrinterPaperSize = false;
                }

                // XPS printer only allows 1 copy
                //pDoc.PrinterSettings.Copies = 2;

                //printerSettings.FromPage = 1;
                //printerSettings.ToPage = 1;

                //pSettings.PrinterSettings.FromPage = 1;
                //pSettings.PrinterSettings.ToPage = 1;
                //pSettings.PrinterSettings.PrintRange = PrintRange.SomePages;

                // values are saved in TWIPS in the RPT file
                if (isMetric == 567)
                    isMetricTwips = 0.0017639;
                else
                    isMetricTwips = 1440;

                if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                {
                    // set page margins from centimeters to twips
                    pSettings.Margins.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) / isMetricTwips);
                    pSettings.Margins.Right = Convert.ToInt32(Double.Parse(CRRight.Text) / isMetricTwips);
                    pSettings.Margins.Top = Convert.ToInt32(Double.Parse(CRTop.Text) / isMetricTwips);
                    pSettings.Margins.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) / isMetricTwips);
                    //printerSettings.DefaultPageSettings.Margins.Left = Convert.ToInt32((Double.Parse(CRLeft.Text) * isMetricTwips) * 1000);
                    //printerSettings.DefaultPageSettings.Margins.Right = Convert.ToInt32((Double.Parse(CRRight.Text) / isMetricTwips) * 1000);
                    //printerSettings.DefaultPageSettings.Margins.Top = Convert.ToInt32(Double.Parse(CRTop.Text));
                    //printerSettings.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text));
                }
                else
                {
                    // set page margins to inches to twips
                    pSettings.Margins.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) * isMetric);
                    pSettings.Margins.Right = Convert.ToInt32(Double.Parse(CRRight.Text) * isMetric);
                    pSettings.Margins.Top = Convert.ToInt32(Double.Parse(CRTop.Text) * isMetric);
                    pSettings.Margins.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) * isMetric);
                    //printerSettings.DefaultPageSettings.Margins.Left = Convert.ToInt32(Double.Parse(CRLeft.Text) / isMetricTwips);
                    //printerSettings.DefaultPageSettings.Margins.Right = Convert.ToInt32(Double.Parse(CRRight.Text) / isMetricTwips);
                    //printerSettings.DefaultPageSettings.Margins.Top = Convert.ToInt32(Double.Parse(CRTop.Text) / isMetricTwips);
                    //printerSettings.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(Double.Parse(CRBottom.Text) / isMetricTwips);
                }

                try
                {
                    rpt.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return;
                }
            }
            else // Default printer
            {
                printerSettings.PrinterName = cboDefaultPrinters.SelectedItem.ToString();

                // don't use this, use the new button
                PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.DoNotScale;

                System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings(printerSettings);

                // Get/Set the Duplex of the printer if it's allowed
                rpt.PrintOptions.PrinterDuplex = (CrystalDecisions.Shared.PrinterDuplex)printerDuplexList.SelectedValue;

                rpt.PrintOptions.PrinterName = printerSettings.PrinterName.ToString();

                // parse the enum values from the drop down list boxes and set the Paper Size
                string MySTRTemp = cboDefaultPaperSizes.SelectedItem.ToString();
                int MyENUM = 0;
                int MyENUM1 = 0;
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboDefaultPaperSizes.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                bool myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rpt.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)MyENUM1;

                // Paper tray
                string MyPaperSource = cboDefaultPaperTrays.SelectedItem.ToString();
                MySTRTemp = cboDefaultPaperTrays.SelectedItem.ToString();
                MyENUM = MySTRTemp.LastIndexOf(@":");
                MySTRTemp = cboDefaultPaperTrays.Text.Substring(0, MyENUM);

                // this will return false if enum cannot be converted
                myNum = Int32.TryParse(MySTRTemp, out MyENUM1);
                rpt.PrintOptions.PaperSource = (CrystalDecisions.Shared.PaperSource)MyENUM1;

                //pSettings.Margins.Left = 1;
                //pSettings.Margins.Right = 1;
                //pSettings.Margins.Top = 1;
                //pSettings.Margins.Bottom = 1;
                //pSettings.PaperSource = pSettings.PaperSource;

                if (chkDissociate.Checked == true)
                {
                    rpt.PrintOptions.DissociatePageSizeAndPrinterPaperSize = true;
                }
                else
                {
                    rpt.PrintOptions.DissociatePageSizeAndPrinterPaperSize = false;
                }

                // XPS printer only allows 1 copy
                //pDoc.PrinterSettings.Copies = 2;

                //printerSettings.FromPage = 1;
                //printerSettings.ToPage = 1;
                //PrintLayout.Scaling = PrintLayoutSettings.PrintScaling.Scale;

                //pSettings.PrinterSettings.FromPage = 1;
                //pSettings.PrinterSettings.ToPage = 1;
                //pSettings.PrinterSettings.PrintRange = PrintRange.SomePages;

                // values are saved in TWIPS in the RPT file
                if (isMetric == 567)
                    isMetricTwips = 0.0017639;
                else
                    isMetricTwips = 1440;

                double CRLeftInt = (pSettings.Margins.Left * isMetricTwips);
                CRLeft.Text = string.Format("{0:0.000}", CRLeftInt);
                double CRRightInt = (pSettings.Margins.Right * isMetricTwips);
                CRRight.Text = string.Format("{0:0.000}", CRRightInt);
                double CRTopInt = (pSettings.Margins.Top * isMetricTwips);
                CRTop.Text = string.Format("{0:0.000}", CRTopInt);
                double CRBottomInt = (pSettings.Margins.Bottom * isMetricTwips);
                CRBottom.Text = string.Format("{0:0.000}", CRBottomInt);

                try
                {
                    rpt.PrintToPrinter(printerSettings, pSettings, false, PrintLayout);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: " + ex.Message);
                    return;
                }
            }

            IsRpt = true;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        int btnZoomFactorValueBox = 100; // 100% - 1 is fit width 2 is fit page

        private void btnZoomFactor_Click(object sender, EventArgs e)
        {
            //string btnZoomFactorValue;
            btnZoomFactorValueBox = Convert.ToInt32(btnZoomFactorValue.Text);
            if (btnZoomFactorValueBox != null)
                crystalReportViewer1.Zoom(btnZoomFactorValueBox);
        }

        private void ViewReport_Click(object sender, EventArgs e)
        {
            DateTime dtStart;
            dtStart = DateTime.Now;
            btnSQLStatement.Text += "\nClicked View Report Start time: \n" + dtStart.ToString();

            //crystalReportViewer1.ToolPanelView = ToolPanelViewType.GroupTree;
            crystalReportViewer1.SetFocusOn(UIComponent.GroupTree);

            //crystalReportViewer1.ReuseParameterValuesOnRefresh = true;
            //crystalReportViewer1.ShowGroupTree();

            // Here's where you test if the report has been modified by RAS then set object accordingly
            try
            {
                if (!IsRpt)
                {
                    crystalReportViewer1.ReportSource = rptClientDoc.ReportSource;
                }
                else
                {
                    crystalReportViewer1.ReportSource = rpt;
                }
            }
            catch (Exception ex)
            {
                btnSQLStatement.Text = "ERROR: " + ex.Message;
            }

            // set up the format export types able to be used from Viewers Export button:
            int myFOpts = (int)(
                CrystalDecisions.Shared.ViewerExportFormats.RptFormat |
                CrystalDecisions.Shared.ViewerExportFormats.PdfFormat |
                CrystalDecisions.Shared.ViewerExportFormats.RptrFormat |
                CrystalDecisions.Shared.ViewerExportFormats.XLSXFormat |
                CrystalDecisions.Shared.ViewerExportFormats.CsvFormat |
                CrystalDecisions.Shared.ViewerExportFormats.EditableRtfFormat |
                CrystalDecisions.Shared.ViewerExportFormats.ExcelRecordFormat |
                CrystalDecisions.Shared.ViewerExportFormats.RtfFormat |
                CrystalDecisions.Shared.ViewerExportFormats.WordFormat |
                CrystalDecisions.Shared.ViewerExportFormats.XmlFormat |
                CrystalDecisions.Shared.ViewerExportFormats.ExcelFormat |
                CrystalDecisions.Shared.ViewerExportFormats.ExcelRecordFormat);
            //CrystalDecisions.Shared.ViewerExportFormats.NoFormat); // no exports allowed
            //int myFOpts = (int)(CrystalDecisions.Shared.ViewerExportFormats.AllFormats);

            crystalReportViewer1.AllowedExportFormats = myFOpts;
            //crystalReportViewer1.EnableToolTips = true;
            //crystalReportViewer1.ReuseParameterValuesOnRefresh = true;
            //crystalReportViewer1.ShowParameterPanelButton = false;

            crystalReportViewer1.Zoom(btnZoomFactorValueBox);

            GroupPath gp = new GroupPath();
            string tmp = String.Empty;
            if (IsLoggedOn)
            {
                try
                {
                    rptClientDoc.RowsetController.GetSQLStatement(gp, out tmp);
                    btnSQLStatement.Text += "\n" + tmp + "\n";
                }
                catch (Exception ex)
                {
                    btnSQLStatement.Text += "\nERROR: " + ex.Message;
                }
            }
        }

        // get all of the area's and sections and show the paper orientation selected
        private void btnSectionPrintOrientation_Click(object sender, EventArgs e)
        {
            string textBox1 = String.Empty;
            string textBox2 = String.Empty;
            btnSQLStatement.Text += "\n";
            int flcnt = 0;
            string SecName = "";
            string SecName1 = "";

            //foreach (CrystalDecisions.CrystalReports.Engine.Area CrArea in rpt.ReportDefinition.Areas)
            foreach (CrystalDecisions.ReportAppServer.ReportDefModel.Area CrArea in rptClientDoc.ReportDefinition.Areas)
            {
                try
                {
                    int RawSectID = 1;
                    if (CrArea.Sections.Count > 1)
                    {
                        //foreach (CrystalDecisions.CrystalReports.Engine.Section crSect in CrArea.Sections) // "DetailSection1"
                        foreach (CrystalDecisions.ReportAppServer.ReportDefModel.Section crSect in CrArea.Sections)
                        {
                            //int sectionCodeArea = (crSect.SectionCode / 6000) % 6000; // Area
                            //int sectionCodeSection = (crSect.SectionCode % 6000); // zero based Section
                            //int sectionCodeGroup = (((crSect.SectionCode) / 50) % 120); // group section
                            //int sectionCodeGroupNo = (((crSect.SectionCode) % 50)); // group area
                            int sectionCodeArea = (crSect.SectionCode / 1000) % 1000; // Area
                            int sectionCodeSection = (crSect.SectionCode % 1000); // zero based Section
                            int sectionCodeGroup = (((crSect.SectionCode) / 25) % 40); // group section
                            int sectionCodeGroupNo = (((crSect.SectionCode) % 25)); // group area
                            bool isGRoup = false;

                            SecName1 = CrArea.Kind.ToString();
                            SecName1 = SecName1.Substring(17, (SecName1.Length - 17));

                            int crtoChr = 97;
                            char b, c;

                            if (sectionCodeGroup <= 26 && (SecName1 == "GroupFooter" || SecName1 == "GroupHeader"))
                            {
                                isGRoup = true;
                            }
                            else
                            {  // aa and up
                                if (RawSectID <= 26 && !isGRoup) // a to z
                                {
                                    btnSQLStatement.AppendText(SecName1);
                                    c = (char)(RawSectID + crtoChr - 1);
                                    btnSQLStatement.Text += textBox1 + "  " + c + " : ";
                                }
                                else
                                {
                                    btnSQLStatement.AppendText(SecName1);
                                    if ((char)((RawSectID) % 26) == 0) // test if next "b" and set b back 1 and last letter to z
                                    {
                                        b = (char)(((((RawSectID) / 26) % 26) - 2) + (crtoChr));
                                        c = (char)(122);
                                    }
                                    else
                                    {   // this determines if the name is a or aa.
                                        b = (char)((((RawSectID) / 26) % 26) + crtoChr - 1);
                                        c = (char)((RawSectID % 26) + crtoChr - 1);
                                    }

                                    btnSQLStatement.Text += textBox1 + " " + b + c + " : " + RawSectID + " - ";
                                }
                            }

                            if (isGRoup)
                            {
                                if (sectionCodeGroup == 0) // && (SecName1 == "GroupFooter" || SecName1 == "GroupHeader")) // only one group area now sections below
                                {
                                    btnSQLStatement.AppendText(SecName1 + " #" + (sectionCodeGroupNo + 1) + (char)(sectionCodeGroup + crtoChr) + ": " + crSect.Format.PageOrientation.ToString() + "\n");
                                    isGRoup = false;
                                }
                                else
                                {
                                    btnSQLStatement.AppendText(SecName1 + " #" + (sectionCodeGroupNo + 1) + (char)(sectionCodeGroup + crtoChr) + ": " + crSect.Format.PageOrientation.ToString() + "\n");
                                    isGRoup = false;
                                }
                            }
                            else
                            {
                                textBox1 += crSect.Format.PageOrientation.ToString() + "\n";
                                isGRoup = false;
                            }
                            btnSQLStatement.Text += textBox1;
                            textBox1 = "";
                            btnCount.Text = flcnt.ToString();
                            ++flcnt;
                            RawSectID++;
                        }
                    }
                    else
                    {
                        SecName = CrArea.Kind.ToString();

                        if (SecName == "crAreaSectionKindGroupHeader" || SecName == "crAreaSectionKindGroupFooter")
                        {
                            foreach (CrystalDecisions.ReportAppServer.ReportDefModel.Section crSectName in CrArea.Sections)
                            {
                                //int sectionCodeArea = (crSectName.SectionCode / 6000) % 6000; // Area
                                //int sectionCodeSection = (crSectName.SectionCode % 6000); // zero based Section
                                //int sectionCodeGroup = (((crSectName.SectionCode) / 50) % 120); // group section
                                //int sectionCodeGroupNo = (((crSectName.SectionCode) % 50)); // group area
                                int sectionCodeArea = (crSectName.SectionCode / 1000) % 1000; // Area
                                int sectionCodeSection = (crSectName.SectionCode % 1000); // zero based Section
                                int sectionCodeGroup = (((crSectName.SectionCode) / 25) % 40); // group section
                                int sectionCodeGroupNo = (((crSectName.SectionCode) % 40)); // group area
                                int crtoChr = 97;

                                if (sectionCodeGroup <= 26)
                                {
                                    if (SecName == "crAreaSectionKindGroupFooter")
                                        btnSQLStatement.AppendText("GroupFooter #" + (sectionCodeGroupNo + 1) + (char)(sectionCodeGroup + crtoChr));
                                    else
                                        btnSQLStatement.AppendText("GroupHeader #" + (sectionCodeGroupNo + 1) + (char)(sectionCodeGroup + crtoChr));
                                }
                                else
                                { // Group higher than -aa
                                    char b, c;
                                    if ((char)((sectionCodeSection) % 26) == 0) // test if next "b" and set b back 1 and last letter to z
                                    {
                                        b = (char)(((sectionCodeSection / 26) % 26) + (crtoChr));
                                        c = (char)(122);

                                        btnSQLStatement.AppendText(c + "\n");
                                    }
                                    else
                                    {   // this determines if the name is a or aa.
                                        b = (char)((((sectionCodeSection) / 26) % 26) + (crtoChr));
                                        c = (char)((sectionCodeSection % 26) + crtoChr - 1);

                                        btnSQLStatement.AppendText(" " + b + c + "\n");
                                    }
                                }
                                //if (CrArea.Name.Substring(0, 11) == crSectName.Name.Substring(0, 11))
                                {
                                    btnSQLStatement.AppendText(" - ");
                                    textBox1 += crSectName.Format.PageOrientation.ToString();
                                    btnSQLStatement.Text += textBox1;
                                    btnSQLStatement.AppendText("\n");
                                    textBox1 = "";

                                    ++flcnt;
                                    btnCount.Text = flcnt.ToString();
                                    break;
                                }
                            }
                        }

                        foreach (CrystalDecisions.ReportAppServer.ReportDefModel.Section crSectName in CrArea.Sections)
                        {
                            SecName1 = CrArea.Kind.ToString();
                            SecName1 = SecName1.Substring(17, (SecName1.Length - 17));
                            if (SecName1 == "GroupFooter" || SecName1 == "GroupHeader")
                            {
                                //btnReportObjects.AppendText(SecName1 + " #" + (((crSectName.SectionCode) % 40) + 1) + " : ");
                            }
                            else
                            {
                                btnSQLStatement.AppendText(SecName1 + " : ");
                                textBox1 += crSectName.Format.PageOrientation.ToString();
                                btnSQLStatement.Text += textBox1;
                                btnSQLStatement.AppendText("\n");
                                textBox1 = "";

                                ++flcnt;
                                btnCount.Text = flcnt.ToString();
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    btnSQLStatement.AppendText("More than one Section in Area. 'End' \n");
                }
            }
        }
    }
}