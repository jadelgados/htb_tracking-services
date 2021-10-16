using TrackingCatalogLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HTBQueueWriter
{
    public partial class Form1 : Form
    {
        System.Messaging.MessageQueue mq;
        const string _FLAGS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string MESSAGE_HEADER = "@@";

        Timer t = new Timer();
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdWrite_Click(object sender, EventArgs e)
        {
            mq = new System.Messaging.MessageQueue(txtQueue.Text);
            WriteToQueue();
            //t.Interval = 1000;
            //t.Tick += t_Tick;
            //t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            WriteToQueue();
            
        }

        private void WriteToQueue()
        {
            try
            {
                int _numberOfMessages = 1;
                if (!string.IsNullOrEmpty(txtNumberOfMessages.Text)) _numberOfMessages = Convert.ToInt32(txtNumberOfMessages.Text);
                for (int i = 0; i < _numberOfMessages; i++)
                {
                    mq.Send(txtMessage.Text);
                }
            }
            catch (Exception ex)
            {
                mq = null;
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //mq = new System.Messaging.MessageQueue(txtQueue.Text);
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            GenerateMessage(txtImei.Text, txtCommand.Text, txtParameters.Text, txtFlag.Text);
        }

        private void GenerateMessage(string imei, string command, string data, string flag)
        {
            GpsMessage msg = new GpsMessage();
            Random rnd = new Random();

            msg.MessageHeader = MESSAGE_HEADER;
            if (string.IsNullOrEmpty(flag))
            {
                msg.PackageFlag = _FLAGS[rnd.Next(_FLAGS.Length - 1)].ToString();
            }
            else
                msg.PackageFlag = flag;
            
            msg.IMEI = imei;
            msg.CommandCode = command;
            msg.Data = data;

            string m = msg.GetMessage();

            txtMessage.Text = m;
        }
    }
}
