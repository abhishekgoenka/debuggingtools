namespace Activator
{
    partial class FrmMain
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
            this.BtnStart = new System.Windows.Forms.Button();
            this.CboHour = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LstWindows = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(208, 12);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 23);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // CboHour
            // 
            this.CboHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboHour.FormattingEnabled = true;
            this.CboHour.Location = new System.Drawing.Point(81, 11);
            this.CboHour.Name = "CboHour";
            this.CboHour.Size = new System.Drawing.Size(121, 24);
            this.CboHour.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Duration";
            // 
            // LstWindows
            // 
            this.LstWindows.FormattingEnabled = true;
            this.LstWindows.ItemHeight = 16;
            this.LstWindows.Location = new System.Drawing.Point(12, 41);
            this.LstWindows.Name = "LstWindows";
            this.LstWindows.Size = new System.Drawing.Size(271, 196);
            this.LstWindows.TabIndex = 5;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(208, 12);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 6;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 244);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.LstWindows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CboHour);
            this.Controls.Add(this.BtnStart);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.ComboBox CboHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LstWindows;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BtnStop;
    }
}

