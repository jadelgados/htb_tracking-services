namespace MBLClient
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnClear = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxConnectStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.richTextTxMessage = new System.Windows.Forms.RichTextBox();
            this.richTextRxMessage = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(288, 360);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(64, 24);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "Clear";
            this.btnClear.Visible = false;
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.BackColor = System.Drawing.Color.Red;
            this.buttonDisconnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDisconnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonDisconnect.Location = new System.Drawing.Point(429, 6);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(146, 33);
            this.buttonDisconnect.TabIndex = 30;
            this.buttonDisconnect.Text = "Disconnet From Server";
            this.buttonDisconnect.UseVisualStyleBackColor = false;
            this.buttonDisconnect.Click += new System.EventHandler(this.ButtonDisconnectClick);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(333, 180);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(240, 24);
            this.buttonSendMessage.TabIndex = 29;
            this.buttonSendMessage.Text = "Send Message";
            this.buttonSendMessage.Click += new System.EventHandler(this.ButtonSendMessageClick);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(295, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Connection Status";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(352, 360);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(64, 24);
            this.buttonClose.TabIndex = 27;
            this.buttonClose.Text = "Close";
            this.buttonClose.Visible = false;
            // 
            // textBoxConnectStatus
            // 
            this.textBoxConnectStatus.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxConnectStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxConnectStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBoxConnectStatus.Location = new System.Drawing.Point(423, 59);
            this.textBoxConnectStatus.Name = "textBoxConnectStatus";
            this.textBoxConnectStatus.ReadOnly = true;
            this.textBoxConnectStatus.Size = new System.Drawing.Size(153, 13);
            this.textBoxConnectStatus.TabIndex = 26;
            this.textBoxConnectStatus.Text = "Not Connected";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Message To Server";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Message From Server";
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.Green;
            this.buttonConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConnect.ForeColor = System.Drawing.Color.Yellow;
            this.buttonConnect.Location = new System.Drawing.Point(294, 6);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(132, 33);
            this.buttonConnect.TabIndex = 23;
            this.buttonConnect.Text = "Connect To Server";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnectClick);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(110, 36);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxPort.TabIndex = 22;
            this.textBoxPort.Text = "50001";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 21;
            this.label2.Text = "Server Port";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Server IP Address";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(110, 12);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(152, 20);
            this.textBoxIP.TabIndex = 19;
            // 
            // richTextTxMessage
            // 
            this.richTextTxMessage.Location = new System.Drawing.Point(9, 88);
            this.richTextTxMessage.Name = "richTextTxMessage";
            this.richTextTxMessage.Size = new System.Drawing.Size(564, 89);
            this.richTextTxMessage.TabIndex = 18;
            this.richTextTxMessage.Text = "";
            // 
            // richTextRxMessage
            // 
            this.richTextRxMessage.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.richTextRxMessage.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.richTextRxMessage.Location = new System.Drawing.Point(9, 229);
            this.richTextRxMessage.Name = "richTextRxMessage";
            this.richTextRxMessage.ReadOnly = true;
            this.richTextRxMessage.Size = new System.Drawing.Size(564, 128);
            this.richTextRxMessage.TabIndex = 17;
            this.richTextRxMessage.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(580, 365);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxConnectStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.richTextTxMessage);
            this.Controls.Add(this.richTextRxMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "MBL Client";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxConnectStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.RichTextBox richTextTxMessage;
        private System.Windows.Forms.RichTextBox richTextRxMessage;
    }
}
