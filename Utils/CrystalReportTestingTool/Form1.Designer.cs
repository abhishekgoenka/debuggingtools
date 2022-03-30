namespace CrystalReportTestingTool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.TxtPrinterName = new System.Windows.Forms.TextBox();
            this.BtnPrintReport = new System.Windows.Forms.Button();
            this.TxtMsg = new System.Windows.Forms.RichTextBox();
            this.TxtDBServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtDatabase = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtPwd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtOrgId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtSiteId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnValidatePrinter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Printer Name";
            // 
            // TxtPrinterName
            // 
            this.TxtPrinterName.Location = new System.Drawing.Point(194, 46);
            this.TxtPrinterName.Name = "TxtPrinterName";
            this.TxtPrinterName.Size = new System.Drawing.Size(591, 28);
            this.TxtPrinterName.TabIndex = 1;
            // 
            // BtnPrintReport
            // 
            this.BtnPrintReport.Location = new System.Drawing.Point(639, 299);
            this.BtnPrintReport.Name = "BtnPrintReport";
            this.BtnPrintReport.Size = new System.Drawing.Size(146, 32);
            this.BtnPrintReport.TabIndex = 2;
            this.BtnPrintReport.Text = "Print Report";
            this.BtnPrintReport.UseVisualStyleBackColor = true;
            this.BtnPrintReport.Click += new System.EventHandler(this.BtnPrintReport_Click);
            // 
            // TxtMsg
            // 
            this.TxtMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TxtMsg.Location = new System.Drawing.Point(0, 345);
            this.TxtMsg.Name = "TxtMsg";
            this.TxtMsg.Size = new System.Drawing.Size(801, 126);
            this.TxtMsg.TabIndex = 3;
            this.TxtMsg.Text = "";
            // 
            // TxtDBServer
            // 
            this.TxtDBServer.Location = new System.Drawing.Point(194, 80);
            this.TxtDBServer.Name = "TxtDBServer";
            this.TxtDBServer.Size = new System.Drawing.Size(591, 28);
            this.TxtDBServer.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "DBServer";
            // 
            // TxtDatabase
            // 
            this.TxtDatabase.Location = new System.Drawing.Point(194, 114);
            this.TxtDatabase.Name = "TxtDatabase";
            this.TxtDatabase.Size = new System.Drawing.Size(591, 28);
            this.TxtDatabase.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Database";
            // 
            // TxtUserName
            // 
            this.TxtUserName.Location = new System.Drawing.Point(194, 148);
            this.TxtUserName.Name = "TxtUserName";
            this.TxtUserName.Size = new System.Drawing.Size(591, 28);
            this.TxtUserName.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "User Name";
            // 
            // TxtPwd
            // 
            this.TxtPwd.Location = new System.Drawing.Point(194, 182);
            this.TxtPwd.Name = "TxtPwd";
            this.TxtPwd.PasswordChar = '*';
            this.TxtPwd.Size = new System.Drawing.Size(591, 28);
            this.TxtPwd.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Password";
            // 
            // TxtOrgId
            // 
            this.TxtOrgId.Location = new System.Drawing.Point(194, 216);
            this.TxtOrgId.Name = "TxtOrgId";
            this.TxtOrgId.Size = new System.Drawing.Size(591, 28);
            this.TxtOrgId.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Org Id";
            // 
            // TxtSiteId
            // 
            this.TxtSiteId.Location = new System.Drawing.Point(194, 250);
            this.TxtSiteId.Name = "TxtSiteId";
            this.TxtSiteId.Size = new System.Drawing.Size(591, 28);
            this.TxtSiteId.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Site Id";
            // 
            // BtnValidatePrinter
            // 
            this.BtnValidatePrinter.Location = new System.Drawing.Point(444, 299);
            this.BtnValidatePrinter.Name = "BtnValidatePrinter";
            this.BtnValidatePrinter.Size = new System.Drawing.Size(189, 32);
            this.BtnValidatePrinter.TabIndex = 16;
            this.BtnValidatePrinter.Text = "Validate Printer";
            this.BtnValidatePrinter.UseVisualStyleBackColor = true;
            this.BtnValidatePrinter.Click += new System.EventHandler(this.BtnValidatePrinter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 471);
            this.Controls.Add(this.BtnValidatePrinter);
            this.Controls.Add(this.TxtSiteId);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TxtOrgId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TxtPwd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxtUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtDatabase);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtDBServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMsg);
            this.Controls.Add(this.BtnPrintReport);
            this.Controls.Add(this.TxtPrinterName);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crystal Report Testing Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtPrinterName;
        private System.Windows.Forms.Button BtnPrintReport;
        private System.Windows.Forms.RichTextBox TxtMsg;
        private System.Windows.Forms.TextBox TxtDBServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtDatabase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtPwd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtOrgId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtSiteId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnValidatePrinter;
    }
}

