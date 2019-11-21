namespace MisterDoctor.Forms
{
    partial class FormSettings
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
            this.txtIgnore = new System.Windows.Forms.TextBox();
            this.lblMaxWords = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtMaxWords = new System.Windows.Forms.TextBox();
            this.lblIgnore = new System.Windows.Forms.Label();
            this.lblUnignore = new System.Windows.Forms.Label();
            this.txtUnignore = new System.Windows.Forms.TextBox();
            this.lblGoodbot = new System.Windows.Forms.Label();
            this.txtGoodbot = new System.Windows.Forms.TextBox();
            this.lblBadbot = new System.Windows.Forms.Label();
            this.txtBadbot = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtIgnore
            // 
            this.txtIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIgnore.Location = new System.Drawing.Point(138, 38);
            this.txtIgnore.Name = "txtIgnore";
            this.txtIgnore.Size = new System.Drawing.Size(128, 20);
            this.txtIgnore.TabIndex = 1;
            // 
            // lblMaxWords
            // 
            this.lblMaxWords.AutoSize = true;
            this.lblMaxWords.Location = new System.Drawing.Point(12, 15);
            this.lblMaxWords.Name = "lblMaxWords";
            this.lblMaxWords.Size = new System.Drawing.Size(120, 13);
            this.lblMaxWords.TabIndex = 12;
            this.lblMaxWords.Text = "Max words in message :";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(191, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(110, 146);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtMaxWords
            // 
            this.txtMaxWords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaxWords.Location = new System.Drawing.Point(138, 12);
            this.txtMaxWords.Name = "txtMaxWords";
            this.txtMaxWords.Size = new System.Drawing.Size(128, 20);
            this.txtMaxWords.TabIndex = 0;
            // 
            // lblIgnore
            // 
            this.lblIgnore.AutoSize = true;
            this.lblIgnore.Location = new System.Drawing.Point(12, 41);
            this.lblIgnore.Name = "lblIgnore";
            this.lblIgnore.Size = new System.Drawing.Size(93, 13);
            this.lblIgnore.TabIndex = 13;
            this.lblIgnore.Text = "Ignore Command :";
            // 
            // lblUnignore
            // 
            this.lblUnignore.AutoSize = true;
            this.lblUnignore.Location = new System.Drawing.Point(12, 67);
            this.lblUnignore.Name = "lblUnignore";
            this.lblUnignore.Size = new System.Drawing.Size(106, 13);
            this.lblUnignore.TabIndex = 14;
            this.lblUnignore.Text = "Unignore Command :";
            // 
            // txtUnignore
            // 
            this.txtUnignore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnignore.Location = new System.Drawing.Point(138, 64);
            this.txtUnignore.Name = "txtUnignore";
            this.txtUnignore.Size = new System.Drawing.Size(128, 20);
            this.txtUnignore.TabIndex = 2;
            // 
            // lblGoodbot
            // 
            this.lblGoodbot.AutoSize = true;
            this.lblGoodbot.Location = new System.Drawing.Point(12, 93);
            this.lblGoodbot.Name = "lblGoodbot";
            this.lblGoodbot.Size = new System.Drawing.Size(105, 13);
            this.lblGoodbot.TabIndex = 16;
            this.lblGoodbot.Text = "Goodbot Response :";
            // 
            // txtGoodbot
            // 
            this.txtGoodbot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGoodbot.Location = new System.Drawing.Point(138, 90);
            this.txtGoodbot.Name = "txtGoodbot";
            this.txtGoodbot.Size = new System.Drawing.Size(128, 20);
            this.txtGoodbot.TabIndex = 3;
            // 
            // lblBadbot
            // 
            this.lblBadbot.AutoSize = true;
            this.lblBadbot.Location = new System.Drawing.Point(12, 119);
            this.lblBadbot.Name = "lblBadbot";
            this.lblBadbot.Size = new System.Drawing.Size(98, 13);
            this.lblBadbot.TabIndex = 18;
            this.lblBadbot.Text = "Badbot Response :";
            // 
            // txtBadbot
            // 
            this.txtBadbot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBadbot.Location = new System.Drawing.Point(138, 116);
            this.txtBadbot.Name = "txtBadbot";
            this.txtBadbot.Size = new System.Drawing.Size(128, 20);
            this.txtBadbot.TabIndex = 4;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 181);
            this.Controls.Add(this.txtBadbot);
            this.Controls.Add(this.lblBadbot);
            this.Controls.Add(this.txtGoodbot);
            this.Controls.Add(this.lblGoodbot);
            this.Controls.Add(this.txtUnignore);
            this.Controls.Add(this.lblUnignore);
            this.Controls.Add(this.txtIgnore);
            this.Controls.Add(this.lblIgnore);
            this.Controls.Add(this.lblMaxWords);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtMaxWords);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIgnore;
        private System.Windows.Forms.Label lblMaxWords;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtMaxWords;
        private System.Windows.Forms.Label lblIgnore;
        private System.Windows.Forms.Label lblUnignore;
        private System.Windows.Forms.TextBox txtUnignore;
        private System.Windows.Forms.Label lblGoodbot;
        private System.Windows.Forms.TextBox txtGoodbot;
        private System.Windows.Forms.Label lblBadbot;
        private System.Windows.Forms.TextBox txtBadbot;
    }
}