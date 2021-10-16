namespace iNectar
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtQueue = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkWriteMessageReceived = new System.Windows.Forms.CheckBox();
            this.richTextBoxReceivedMsg = new System.Windows.Forms.RichTextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblLabelPort = new System.Windows.Forms.Label();
            this.lblClientsConnected = new System.Windows.Forms.Label();
            this.lblLabelClientsConnected = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Queue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(445, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(497, 14);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(29, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Stop";
            // 
            // txtQueue
            // 
            this.txtQueue.Location = new System.Drawing.Point(59, 11);
            this.txtQueue.Name = "txtQueue";
            this.txtQueue.Size = new System.Drawing.Size(367, 20);
            this.txtQueue.TabIndex = 4;
            this.txtQueue.Text = ".\\Private$\\htb";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblClientsConnected);
            this.panel1.Controls.Add(this.lblLabelClientsConnected);
            this.panel1.Controls.Add(this.lblPort);
            this.panel1.Controls.Add(this.lblLabelPort);
            this.panel1.Controls.Add(this.chkWriteMessageReceived);
            this.panel1.Controls.Add(this.txtQueue);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(682, 63);
            this.panel1.TabIndex = 5;
            // 
            // chkWriteMessageReceived
            // 
            this.chkWriteMessageReceived.AutoSize = true;
            this.chkWriteMessageReceived.Location = new System.Drawing.Point(59, 37);
            this.chkWriteMessageReceived.Name = "chkWriteMessageReceived";
            this.chkWriteMessageReceived.Size = new System.Drawing.Size(228, 17);
            this.chkWriteMessageReceived.TabIndex = 5;
            this.chkWriteMessageReceived.Text = "Show datetime when receive any message";
            this.chkWriteMessageReceived.UseVisualStyleBackColor = true;
            // 
            // richTextBoxReceivedMsg
            // 
            this.richTextBoxReceivedMsg.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.richTextBoxReceivedMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxReceivedMsg.Location = new System.Drawing.Point(0, 63);
            this.richTextBoxReceivedMsg.Name = "richTextBoxReceivedMsg";
            this.richTextBoxReceivedMsg.ReadOnly = true;
            this.richTextBoxReceivedMsg.Size = new System.Drawing.Size(682, 261);
            this.richTextBoxReceivedMsg.TabIndex = 10;
            this.richTextBoxReceivedMsg.Text = "";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(497, 37);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(19, 13);
            this.lblPort.TabIndex = 7;
            this.lblPort.Text = "80";
            this.lblPort.Visible = false;
            // 
            // lblLabelPort
            // 
            this.lblLabelPort.AutoSize = true;
            this.lblLabelPort.Location = new System.Drawing.Point(445, 37);
            this.lblLabelPort.Name = "lblLabelPort";
            this.lblLabelPort.Size = new System.Drawing.Size(26, 13);
            this.lblLabelPort.TabIndex = 6;
            this.lblLabelPort.Text = "Port";
            this.lblLabelPort.Visible = false;
            // 
            // lblClientsConnected
            // 
            this.lblClientsConnected.AutoSize = true;
            this.lblClientsConnected.Location = new System.Drawing.Point(648, 37);
            this.lblClientsConnected.Name = "lblClientsConnected";
            this.lblClientsConnected.Size = new System.Drawing.Size(13, 13);
            this.lblClientsConnected.TabIndex = 9;
            this.lblClientsConnected.Text = "5";
            this.lblClientsConnected.Visible = false;
            // 
            // lblLabelClientsConnected
            // 
            this.lblLabelClientsConnected.AutoSize = true;
            this.lblLabelClientsConnected.Location = new System.Drawing.Point(551, 37);
            this.lblLabelClientsConnected.Name = "lblLabelClientsConnected";
            this.lblLabelClientsConnected.Size = new System.Drawing.Size(92, 13);
            this.lblLabelClientsConnected.TabIndex = 8;
            this.lblLabelClientsConnected.Text = "Clients connected";
            this.lblLabelClientsConnected.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 324);
            this.Controls.Add(this.richTextBoxReceivedMsg);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iNectar";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtQueue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkWriteMessageReceived;
        private System.Windows.Forms.RichTextBox richTextBoxReceivedMsg;
        private System.Windows.Forms.Label lblClientsConnected;
        private System.Windows.Forms.Label lblLabelClientsConnected;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblLabelPort;
    }
}

