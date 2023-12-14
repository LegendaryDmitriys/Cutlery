using System.ComponentModel;

namespace WindowsFormsApp4
{
    partial class Captcha
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Captcha));
            this.capthcaText = new System.Windows.Forms.Label();
            this.inputButton = new System.Windows.Forms.Button();
            this.capthcaInput = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // capthcaText
            // 
            this.capthcaText.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.capthcaText.Location = new System.Drawing.Point(155, 98);
            this.capthcaText.Name = "capthcaText";
            this.capthcaText.Size = new System.Drawing.Size(152, 27);
            this.capthcaText.TabIndex = 0;
            this.capthcaText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputButton
            // 
            this.inputButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.inputButton.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.inputButton.Location = new System.Drawing.Point(155, 213);
            this.inputButton.Name = "inputButton";
            this.inputButton.Size = new System.Drawing.Size(152, 41);
            this.inputButton.TabIndex = 1;
            this.inputButton.Text = "Ввод";
            this.inputButton.UseVisualStyleBackColor = true;
            this.inputButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // capthcaInput
            // 
            this.capthcaInput.Font = new System.Drawing.Font("Comic Sans MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.capthcaInput.Location = new System.Drawing.Point(155, 138);
            this.capthcaInput.Name = "capthcaInput";
            this.capthcaInput.Size = new System.Drawing.Size(152, 37);
            this.capthcaInput.TabIndex = 2;
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.Location = new System.Drawing.Point(174, 18);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(111, 50);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Капча";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.titleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(457, 95);
            this.panel1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Captcha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 308);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.capthcaInput);
            this.Controls.Add(this.inputButton);
            this.Controls.Add(this.capthcaText);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Captcha";
            this.Text = "Столовые приборы";
            this.Load += new System.EventHandler(this.Captcha_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label titleLabel;

        private System.Windows.Forms.Label capthcaText;
        private System.Windows.Forms.Button inputButton;
        private System.Windows.Forms.TextBox capthcaInput;

        #endregion
    }
}