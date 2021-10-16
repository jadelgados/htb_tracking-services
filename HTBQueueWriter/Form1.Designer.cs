namespace HTBQueueWriter
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
            this.txtQueue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNumberOfMessages = new System.Windows.Forms.TextBox();
            this.txtImei = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtParameters = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdGenerate = new System.Windows.Forms.Button();
            this.txtFlag = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtQueue
            // 
            this.txtQueue.Location = new System.Drawing.Point(89, 7);
            this.txtQueue.Name = "txtQueue";
            this.txtQueue.Size = new System.Drawing.Size(383, 20);
            this.txtQueue.TabIndex = 9;
            this.txtQueue.Text = ".\\Private$\\MainInputServer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Queue";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtMessage);
            this.panel1.Location = new System.Drawing.Point(13, 170);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 143);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmdWrite);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 42);
            this.panel2.TabIndex = 1;
            // 
            // cmdWrite
            // 
            this.cmdWrite.Location = new System.Drawing.Point(447, 10);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.Size = new System.Drawing.Size(75, 23);
            this.cmdWrite.TabIndex = 0;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.UseVisualStyleBackColor = true;
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(0, 0);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(522, 77);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Message";
            // 
            // txtNumberOfMessages
            // 
            this.txtNumberOfMessages.Location = new System.Drawing.Point(478, 7);
            this.txtNumberOfMessages.Name = "txtNumberOfMessages";
            this.txtNumberOfMessages.Size = new System.Drawing.Size(44, 20);
            this.txtNumberOfMessages.TabIndex = 10;
            this.txtNumberOfMessages.Text = "1";
            this.txtNumberOfMessages.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtImei
            // 
            this.txtImei.Location = new System.Drawing.Point(89, 35);
            this.txtImei.Name = "txtImei";
            this.txtImei.Size = new System.Drawing.Size(163, 20);
            this.txtImei.TabIndex = 12;
            this.txtImei.Text = "353351050023969";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "IMEI";
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(89, 61);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(163, 20);
            this.txtCommand.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Command";
            // 
            // txtParameters
            // 
            this.txtParameters.Location = new System.Drawing.Point(89, 87);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new System.Drawing.Size(163, 20);
            this.txtParameters.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Parameters";
            // 
            // cmdGenerate
            // 
            this.cmdGenerate.Location = new System.Drawing.Point(447, 117);
            this.cmdGenerate.Name = "cmdGenerate";
            this.cmdGenerate.Size = new System.Drawing.Size(75, 23);
            this.cmdGenerate.TabIndex = 17;
            this.cmdGenerate.Text = "Generate";
            this.cmdGenerate.UseVisualStyleBackColor = true;
            this.cmdGenerate.Click += new System.EventHandler(this.cmdGenerate_Click);
            // 
            // txtFlag
            // 
            this.txtFlag.Location = new System.Drawing.Point(89, 113);
            this.txtFlag.Name = "txtFlag";
            this.txtFlag.Size = new System.Drawing.Size(41, 20);
            this.txtFlag.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Flag";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 490);
            this.Controls.Add(this.txtFlag);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdGenerate);
            this.Controls.Add(this.txtParameters);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtImei);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumberOfMessages);
            this.Controls.Add(this.txtQueue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Queue Writer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtQueue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cmdWrite;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNumberOfMessages;
        private System.Windows.Forms.TextBox txtImei;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtParameters;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdGenerate;
        private System.Windows.Forms.TextBox txtFlag;
        private System.Windows.Forms.Label label6;
    }
}

