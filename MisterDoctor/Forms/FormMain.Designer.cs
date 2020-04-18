namespace MisterDoctor.Forms
{
    partial class FormMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phrasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblConnect = new System.Windows.Forms.Label();
            this.txtChannel = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.chkConnect = new System.Windows.Forms.CheckBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.stripCooldown = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCooldown = new System.Windows.Forms.Label();
            this.txtCooldown = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblProc = new System.Windows.Forms.Label();
            this.txtProc = new System.Windows.Forms.TextBox();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenToolStripMenuItem,
            this.commandsToolStripMenuItem,
            this.wordListToolStripMenuItem,
            this.phrasesToolStripMenuItem,
            this.ignoreToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(483, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tokenToolStripMenuItem
            // 
            this.tokenToolStripMenuItem.Name = "tokenToolStripMenuItem";
            this.tokenToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.tokenToolStripMenuItem.Text = "Token";
            // 
            // wordListToolStripMenuItem
            // 
            this.wordListToolStripMenuItem.Name = "wordListToolStripMenuItem";
            this.wordListToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.wordListToolStripMenuItem.Text = "Word List";
            // 
            // phrasesToolStripMenuItem
            // 
            this.phrasesToolStripMenuItem.Name = "phrasesToolStripMenuItem";
            this.phrasesToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.phrasesToolStripMenuItem.Text = "Phrases";
            // 
            // ignoreToolStripMenuItem
            // 
            this.ignoreToolStripMenuItem.Name = "ignoreToolStripMenuItem";
            this.ignoreToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ignoreToolStripMenuItem.Text = "Ignore";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // lblConnect
            // 
            this.lblConnect.AutoSize = true;
            this.lblConnect.Location = new System.Drawing.Point(12, 33);
            this.lblConnect.Name = "lblConnect";
            this.lblConnect.Size = new System.Drawing.Size(101, 13);
            this.lblConnect.TabIndex = 1;
            this.lblConnect.Text = "Connect to Channel";
            // 
            // txtChannel
            // 
            this.txtChannel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChannel.Location = new System.Drawing.Point(120, 30);
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.Size = new System.Drawing.Size(173, 20);
            this.txtChannel.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(299, 28);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // chkConnect
            // 
            this.chkConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkConnect.AutoSize = true;
            this.chkConnect.Location = new System.Drawing.Point(380, 32);
            this.chkConnect.Name = "chkConnect";
            this.chkConnect.Size = new System.Drawing.Size(91, 17);
            this.chkConnect.TabIndex = 2;
            this.chkConnect.Text = "Auto Connect";
            this.chkConnect.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripCooldown,
            this.stripConnected});
            this.statusStrip.Location = new System.Drawing.Point(0, 114);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(483, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // stripCooldown
            // 
            this.stripCooldown.Name = "stripCooldown";
            this.stripCooldown.Size = new System.Drawing.Size(118, 17);
            this.stripCooldown.Text = "toolStripStatusLabel1";
            // 
            // stripConnected
            // 
            this.stripConnected.Name = "stripConnected";
            this.stripConnected.Size = new System.Drawing.Size(350, 17);
            this.stripConnected.Spring = true;
            this.stripConnected.Text = "toolStripStatusLabel1";
            this.stripConnected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCooldown
            // 
            this.lblCooldown.AutoSize = true;
            this.lblCooldown.Location = new System.Drawing.Point(12, 85);
            this.lblCooldown.Name = "lblCooldown";
            this.lblCooldown.Size = new System.Drawing.Size(82, 13);
            this.lblCooldown.TabIndex = 6;
            this.lblCooldown.Text = "Cooldown (Sec)";
            // 
            // txtCooldown
            // 
            this.txtCooldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCooldown.Location = new System.Drawing.Point(120, 82);
            this.txtCooldown.Name = "txtCooldown";
            this.txtCooldown.Size = new System.Drawing.Size(173, 20);
            this.txtCooldown.TabIndex = 4;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(299, 80);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // lblProc
            // 
            this.lblProc.AutoSize = true;
            this.lblProc.Location = new System.Drawing.Point(12, 59);
            this.lblProc.Name = "lblProc";
            this.lblProc.Size = new System.Drawing.Size(102, 13);
            this.lblProc.TabIndex = 9;
            this.lblProc.Text = "Proc (Occurance %)";
            // 
            // txtProc
            // 
            this.txtProc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProc.Location = new System.Drawing.Point(120, 56);
            this.txtProc.Name = "txtProc";
            this.txtProc.Size = new System.Drawing.Size(173, 20);
            this.txtProc.TabIndex = 3;
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.commandsToolStripMenuItem.Text = "Commands";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 136);
            this.Controls.Add(this.txtProc);
            this.Controls.Add(this.lblProc);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtCooldown);
            this.Controls.Add(this.lblCooldown);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.chkConnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtChannel);
            this.Controls.Add(this.lblConnect);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordListToolStripMenuItem;
        private System.Windows.Forms.Label lblConnect;
        private System.Windows.Forms.TextBox txtChannel;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.CheckBox chkConnect;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel stripConnected;
        private System.Windows.Forms.Label lblCooldown;
        private System.Windows.Forms.TextBox txtCooldown;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblProc;
        private System.Windows.Forms.TextBox txtProc;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel stripCooldown;
        private System.Windows.Forms.ToolStripMenuItem ignoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phrasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandsToolStripMenuItem;
    }
}

