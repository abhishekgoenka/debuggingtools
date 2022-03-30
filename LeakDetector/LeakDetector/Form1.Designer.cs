namespace LeakDetector
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
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.ListAssembly = new Infragistics.Win.UltraWinListView.UltraListView();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ultraButton3 = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ListAssembly)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraButton1
            // 
            this.ultraButton1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton1.Location = new System.Drawing.Point(25, 387);
            this.ultraButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(100, 28);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "ultraButton1";
            this.ultraButton1.Click += new System.EventHandler(this.ultraButton1_Click);
            // 
            // ListAssembly
            // 
            this.ListAssembly.Dock = System.Windows.Forms.DockStyle.Top;
            this.ListAssembly.Location = new System.Drawing.Point(0, 0);
            this.ListAssembly.Name = "ListAssembly";
            this.ListAssembly.Size = new System.Drawing.Size(836, 348);
            this.ListAssembly.TabIndex = 1;
            this.ListAssembly.Text = "ultraListView1";
            this.ListAssembly.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.List;
            this.ListAssembly.ViewSettingsList.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            // 
            // ultraButton2
            // 
            this.ultraButton2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton2.Location = new System.Drawing.Point(133, 387);
            this.ultraButton2.Margin = new System.Windows.Forms.Padding(4);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(100, 28);
            this.ultraButton2.TabIndex = 2;
            this.ultraButton2.Text = "ultraButton2";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 377);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 401);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(525, 401);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(707, 401);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(704, 377);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "label6";
            // 
            // ultraButton3
            // 
            this.ultraButton3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraButton3.Location = new System.Drawing.Point(133, 355);
            this.ultraButton3.Margin = new System.Windows.Forms.Padding(4);
            this.ultraButton3.Name = "ultraButton3";
            this.ultraButton3.Size = new System.Drawing.Size(100, 28);
            this.ultraButton3.TabIndex = 9;
            this.ultraButton3.Text = "ultraButton3";
            this.ultraButton3.Click += new System.EventHandler(this.ultraButton3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 428);
            this.Controls.Add(this.ultraButton3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ultraButton2);
            this.Controls.Add(this.ListAssembly);
            this.Controls.Add(this.ultraButton1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ListAssembly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.UltraWinListView.UltraListView ListAssembly;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Infragistics.Win.Misc.UltraButton ultraButton3;
    }
}

