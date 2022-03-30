namespace DecryptConfig
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
            this.BtnDecrypt = new System.Windows.Forms.Button();
            this.CboSections = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnDecrypt
            // 
            this.BtnDecrypt.Location = new System.Drawing.Point(295, 86);
            this.BtnDecrypt.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDecrypt.Name = "BtnDecrypt";
            this.BtnDecrypt.Size = new System.Drawing.Size(100, 28);
            this.BtnDecrypt.TabIndex = 0;
            this.BtnDecrypt.Text = "Decrypt";
            this.BtnDecrypt.UseVisualStyleBackColor = true;
            this.BtnDecrypt.Click += new System.EventHandler(this.BtnDecrypt_Click);
            // 
            // CboSections
            // 
            this.CboSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboSections.FormattingEnabled = true;
            this.CboSections.Location = new System.Drawing.Point(135, 17);
            this.CboSections.Name = "CboSections";
            this.CboSections.Size = new System.Drawing.Size(260, 24);
            this.CboSections.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Section";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 144);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CboSections);
            this.Controls.Add(this.BtnDecrypt);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Decrypt app.config";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnDecrypt;
        private System.Windows.Forms.ComboBox CboSections;
        private System.Windows.Forms.Label label2;
    }
}

