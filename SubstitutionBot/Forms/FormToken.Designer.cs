namespace SubstitutionBot.Forms
{
    partial class FormToken
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
            this.lblAppId = new System.Windows.Forms.Label();
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblOAuth = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtOAuth = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAppId
            // 
            this.lblAppId.AutoSize = true;
            this.lblAppId.Location = new System.Drawing.Point(12, 9);
            this.lblAppId.Name = "lblAppId";
            this.lblAppId.Size = new System.Drawing.Size(100, 13);
            this.lblAppId.TabIndex = 0;
            this.lblAppId.Text = "Application Client Id";
            // 
            // txtAppId
            // 
            this.txtAppId.Location = new System.Drawing.Point(15, 25);
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.Size = new System.Drawing.Size(335, 20);
            this.txtAppId.TabIndex = 1;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 58);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(90, 13);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Twitch Username";
            // 
            // lblOAuth
            // 
            this.lblOAuth.AutoSize = true;
            this.lblOAuth.Location = new System.Drawing.Point(12, 107);
            this.lblOAuth.Name = "lblOAuth";
            this.lblOAuth.Size = new System.Drawing.Size(93, 13);
            this.lblOAuth.TabIndex = 3;
            this.lblOAuth.Text = "Twitch OAuth Key";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(15, 74);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(335, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // txtOAuth
            // 
            this.txtOAuth.Location = new System.Drawing.Point(15, 123);
            this.txtOAuth.Name = "txtOAuth";
            this.txtOAuth.Size = new System.Drawing.Size(335, 20);
            this.txtOAuth.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 159);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(275, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 196);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtOAuth);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtAppId);
            this.Controls.Add(this.lblOAuth);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblAppId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormToken";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormToken";
            this.Load += new System.EventHandler(this.FormToken_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAppId;
        private System.Windows.Forms.TextBox txtAppId;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblOAuth;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtOAuth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}