namespace BlackIceFileCaptureTester
{
    partial class FileCapture
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileCapture));
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.BiPrnDrv = new AxBIPRNDRVLib.AxBiPrnDrv();
            this.Output = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblSession = new System.Windows.Forms.Label();
            this.TxtSession = new System.Windows.Forms.TextBox();
            this.BtnChangeSessionId = new System.Windows.Forms.Button();
            this.BtnClearLogs = new System.Windows.Forms.Button();
            this.MESSAGE_CAPTURE_METHOD_COPYDATA = new System.Windows.Forms.RadioButton();
            this.MESSAGE_CAPTURE_METHOD_BROADCAST = new System.Windows.Forms.RadioButton();
            this.MESSAGE_CAPTURE_METHOD_PIPE = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.BiPrnDrv)).BeginInit();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // BiPrnDrv
            // 
            this.BiPrnDrv.Enabled = true;
            this.BiPrnDrv.Location = new System.Drawing.Point(20, 20);
            this.BiPrnDrv.Name = "BiPrnDrv";
            this.BiPrnDrv.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("BiPrnDrv.OcxState")));
            this.BiPrnDrv.Size = new System.Drawing.Size(64, 32);
            this.BiPrnDrv.TabIndex = 0;
            this.BiPrnDrv.StarDoc += new AxBIPRNDRVLib._DBiPrnDrvEvents_StarDocEventHandler(this.BiPrnDrv_StarDoc);
            this.BiPrnDrv.EndDoc += new AxBIPRNDRVLib._DBiPrnDrvEvents_EndDocEventHandler(this.BiPrnDrv_EndDoc);
            // 
            // Output
            // 
            this.Output.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Output.Location = new System.Drawing.Point(0, 267);
            this.Output.Margin = new System.Windows.Forms.Padding(4);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(722, 275);
            this.Output.TabIndex = 1;
            this.Output.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(635, 237);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 23);
            this.BtnStart.TabIndex = 2;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(635, 237);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 3;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // LblSession
            // 
            this.LblSession.AutoSize = true;
            this.LblSession.Location = new System.Drawing.Point(12, 25);
            this.LblSession.Name = "LblSession";
            this.LblSession.Size = new System.Drawing.Size(76, 16);
            this.LblSession.TabIndex = 4;
            this.LblSession.Text = "Session Id";
            // 
            // TxtSession
            // 
            this.TxtSession.Location = new System.Drawing.Point(94, 22);
            this.TxtSession.Name = "TxtSession";
            this.TxtSession.Size = new System.Drawing.Size(100, 23);
            this.TxtSession.TabIndex = 5;
            // 
            // BtnChangeSessionId
            // 
            this.BtnChangeSessionId.Location = new System.Drawing.Point(200, 22);
            this.BtnChangeSessionId.Name = "BtnChangeSessionId";
            this.BtnChangeSessionId.Size = new System.Drawing.Size(201, 23);
            this.BtnChangeSessionId.TabIndex = 6;
            this.BtnChangeSessionId.Text = "Change Session id";
            this.BtnChangeSessionId.UseVisualStyleBackColor = true;
            this.BtnChangeSessionId.Click += new System.EventHandler(this.BtnChangeSessionId_Click);
            // 
            // BtnClearLogs
            // 
            this.BtnClearLogs.Location = new System.Drawing.Point(530, 237);
            this.BtnClearLogs.Name = "BtnClearLogs";
            this.BtnClearLogs.Size = new System.Drawing.Size(99, 23);
            this.BtnClearLogs.TabIndex = 7;
            this.BtnClearLogs.Text = "Clear Logs";
            this.BtnClearLogs.UseVisualStyleBackColor = true;
            this.BtnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // MESSAGE_CAPTURE_METHOD_COPYDATA
            // 
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.AutoSize = true;
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.Location = new System.Drawing.Point(94, 69);
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.Name = "MESSAGE_CAPTURE_METHOD_COPYDATA";
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.Size = new System.Drawing.Size(301, 20);
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.TabIndex = 8;
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.Text = "MESSAGE_CAPTURE_METHOD_COPYDATA";
            this.MESSAGE_CAPTURE_METHOD_COPYDATA.UseVisualStyleBackColor = true;
            // 
            // MESSAGE_CAPTURE_METHOD_BROADCAST
            // 
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.AutoSize = true;
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.Location = new System.Drawing.Point(94, 95);
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.Name = "MESSAGE_CAPTURE_METHOD_BROADCAST";
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.Size = new System.Drawing.Size(309, 20);
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.TabIndex = 9;
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.Text = "MESSAGE_CAPTURE_METHOD_BROADCAST";
            this.MESSAGE_CAPTURE_METHOD_BROADCAST.UseVisualStyleBackColor = true;
            // 
            // MESSAGE_CAPTURE_METHOD_PIPE
            // 
            this.MESSAGE_CAPTURE_METHOD_PIPE.AutoSize = true;
            this.MESSAGE_CAPTURE_METHOD_PIPE.Checked = true;
            this.MESSAGE_CAPTURE_METHOD_PIPE.Location = new System.Drawing.Point(94, 121);
            this.MESSAGE_CAPTURE_METHOD_PIPE.Name = "MESSAGE_CAPTURE_METHOD_PIPE";
            this.MESSAGE_CAPTURE_METHOD_PIPE.Size = new System.Drawing.Size(258, 20);
            this.MESSAGE_CAPTURE_METHOD_PIPE.TabIndex = 10;
            this.MESSAGE_CAPTURE_METHOD_PIPE.TabStop = true;
            this.MESSAGE_CAPTURE_METHOD_PIPE.Text = "MESSAGE_CAPTURE_METHOD_PIPE";
            this.MESSAGE_CAPTURE_METHOD_PIPE.UseVisualStyleBackColor = true;
            // 
            // FileCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 542);
            this.Controls.Add(this.MESSAGE_CAPTURE_METHOD_PIPE);
            this.Controls.Add(this.MESSAGE_CAPTURE_METHOD_BROADCAST);
            this.Controls.Add(this.MESSAGE_CAPTURE_METHOD_COPYDATA);
            this.Controls.Add(this.BtnClearLogs);
            this.Controls.Add(this.BtnChangeSessionId);
            this.Controls.Add(this.TxtSession);
            this.Controls.Add(this.LblSession);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.Output);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FileCapture";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.BiPrnDrv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PrintDialog printDialog1;
        private AxBIPRNDRVLib.AxBiPrnDrv BiPrnDrv;
        private System.Windows.Forms.RichTextBox Output;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label LblSession;
        private System.Windows.Forms.TextBox TxtSession;
        private System.Windows.Forms.Button BtnChangeSessionId;
        private System.Windows.Forms.Button BtnClearLogs;
        private System.Windows.Forms.RadioButton MESSAGE_CAPTURE_METHOD_COPYDATA;
        private System.Windows.Forms.RadioButton MESSAGE_CAPTURE_METHOD_BROADCAST;
        private System.Windows.Forms.RadioButton MESSAGE_CAPTURE_METHOD_PIPE;
    }
}

