
namespace MisterDoctor.Forms
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChannel = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtOAuth = new System.Windows.Forms.TextBox();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chkAutoConnect = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusStripSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuPlugins = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.linkOAuth = new System.Windows.Forms.LinkLabel();
            this.linkClientId = new System.Windows.Forms.LinkLabel();
            this.menuIgnored = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connect to Channel";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Twitch Bot Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Twitch Bot OAuth Key";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Twitch Bot Client Key";
            // 
            // txtChannel
            // 
            this.txtChannel.Location = new System.Drawing.Point(150, 36);
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.Size = new System.Drawing.Size(198, 23);
            this.txtChannel.TabIndex = 4;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(150, 65);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(198, 23);
            this.txtUsername.TabIndex = 5;
            // 
            // txtOAuth
            // 
            this.txtOAuth.Location = new System.Drawing.Point(150, 94);
            this.txtOAuth.Name = "txtOAuth";
            this.txtOAuth.Size = new System.Drawing.Size(198, 23);
            this.txtOAuth.TabIndex = 6;
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(150, 123);
            this.txtClient.Name = "txtClient";
            this.txtClient.Size = new System.Drawing.Size(198, 23);
            this.txtClient.TabIndex = 7;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(354, 36);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(104, 23);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.AutoSize = true;
            this.chkAutoConnect.Location = new System.Drawing.Point(354, 67);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(100, 19);
            this.chkAutoConnect.TabIndex = 9;
            this.chkAutoConnect.Text = "Auto Connect";
            this.chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStripSpring,
            this.statusStripConnected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 161);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(476, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusStripSpring
            // 
            this.statusStripSpring.Name = "statusStripSpring";
            this.statusStripSpring.Size = new System.Drawing.Size(373, 17);
            this.statusStripSpring.Spring = true;
            this.statusStripSpring.Text = " ";
            // 
            // statusStripConnected
            // 
            this.statusStripConnected.Name = "statusStripConnected";
            this.statusStripConnected.Size = new System.Drawing.Size(88, 17);
            this.statusStripConnected.Text = "Not Connected";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPlugins,
            this.menuIgnored,
            this.menuAbout});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(476, 24);
            this.menuStrip.TabIndex = 12;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuPlugins
            // 
            this.menuPlugins.Name = "menuPlugins";
            this.menuPlugins.Size = new System.Drawing.Size(58, 20);
            this.menuPlugins.Text = "Plugins";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(52, 20);
            this.menuAbout.Text = "About";
            // 
            // linkOAuth
            // 
            this.linkOAuth.AutoSize = true;
            this.linkOAuth.Location = new System.Drawing.Point(354, 97);
            this.linkOAuth.Name = "linkOAuth";
            this.linkOAuth.Size = new System.Drawing.Size(85, 15);
            this.linkOAuth.TabIndex = 13;
            this.linkOAuth.TabStop = true;
            this.linkOAuth.Text = "Get OAuth Key";
            // 
            // linkClientId
            // 
            this.linkClientId.AutoSize = true;
            this.linkClientId.Location = new System.Drawing.Point(354, 126);
            this.linkClientId.Name = "linkClientId";
            this.linkClientId.Size = new System.Drawing.Size(72, 15);
            this.linkClientId.TabIndex = 14;
            this.linkClientId.TabStop = true;
            this.linkClientId.Text = "Get Client Id";
            // 
            // menuIgnored
            // 
            this.menuIgnored.Name = "menuIgnored";
            this.menuIgnored.Size = new System.Drawing.Size(60, 20);
            this.menuIgnored.Text = "Ignored";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 183);
            this.Controls.Add(this.linkClientId);
            this.Controls.Add(this.linkOAuth);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.chkAutoConnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.txtOAuth);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtChannel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtChannel;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtOAuth;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox chkAutoConnect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusStripSpring;
        private System.Windows.Forms.ToolStripStatusLabel statusStripConnected;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.LinkLabel linkOAuth;
        private System.Windows.Forms.LinkLabel linkClientId;
        private System.Windows.Forms.ToolStripMenuItem menuPlugins;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuIgnored;
    }
}

