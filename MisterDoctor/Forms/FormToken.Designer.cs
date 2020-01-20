namespace MisterDoctor.Forms
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblOAuth = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtOAuth = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lnkGetOAuth = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.lnkGetClient = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(12, 9);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(109, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Twitch Bot Username";
            // 
            // lblOAuth
            // 
            this.lblOAuth.AutoSize = true;
            this.lblOAuth.Location = new System.Drawing.Point(12, 35);
            this.lblOAuth.Name = "lblOAuth";
            this.lblOAuth.Size = new System.Drawing.Size(112, 13);
            this.lblOAuth.TabIndex = 2;
            this.lblOAuth.Text = "Twitch Bot OAuth Key";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(130, 6);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(220, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // txtOAuth
            // 
            this.txtOAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOAuth.Location = new System.Drawing.Point(130, 32);
            this.txtOAuth.Name = "txtOAuth";
            this.txtOAuth.Size = new System.Drawing.Size(220, 20);
            this.txtOAuth.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(15, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(275, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lnkGetOAuth
            // 
            this.lnkGetOAuth.AutoSize = true;
            this.lnkGetOAuth.Location = new System.Drawing.Point(127, 63);
            this.lnkGetOAuth.Name = "lnkGetOAuth";
            this.lnkGetOAuth.Size = new System.Drawing.Size(78, 13);
            this.lnkGetOAuth.TabIndex = 5;
            this.lnkGetOAuth.TabStop = true;
            this.lnkGetOAuth.Text = "Get OAuth Key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Twitch Bot Client ID";
            // 
            // txtClientId
            // 
            this.txtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientId.Location = new System.Drawing.Point(130, 86);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(220, 20);
            this.txtClientId.TabIndex = 8;
            // 
            // lnkGetClient
            // 
            this.lnkGetClient.AutoSize = true;
            this.lnkGetClient.Location = new System.Drawing.Point(127, 117);
            this.lnkGetClient.Name = "lnkGetClient";
            this.lnkGetClient.Size = new System.Drawing.Size(67, 13);
            this.lnkGetClient.TabIndex = 9;
            this.lnkGetClient.TabStop = true;
            this.lnkGetClient.Text = "Get Client ID";
            // 
            // FormToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 149);
            this.Controls.Add(this.lnkGetClient);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkGetOAuth);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtOAuth);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblOAuth);
            this.Controls.Add(this.lblUsername);
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
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblOAuth;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtOAuth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel lnkGetOAuth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.LinkLabel lnkGetClient;
    }
}