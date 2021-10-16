using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace MBLClient
{
    

    public partial class Main : System.Windows.Forms.Form
    {
        byte[] m_dataBuffer = new byte[10];
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;
        public Socket m_clientSocket;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            textBoxIP.Text = GetIP();
        }

        void ButtonCloseClick(object sender, System.EventArgs e)
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Close();
                m_clientSocket = null;
            }
            Close();
        }

        void ButtonConnectClick(object sender, System.EventArgs e)
        {
            // See if we have text on the IP and Port text fields
            if (textBoxIP.Text == "" || textBoxPort.Text == "")
            {
                MessageBox.Show("IP Address and Port Number are required to connect to the Server\n");
                return;
            }
            try
            {
                UpdateControls(false);
                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Cet the remote IP address
                IPAddress ip = IPAddress.Parse(textBoxIP.Text);
                int iPortNo = System.Convert.ToInt32(textBoxPort.Text);
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {

                    UpdateControls(true);
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                string str;
                str = "\nConnection failed, is the server running?\n" + se.Message;
                MessageBox.Show(str);
                UpdateControls(false);
            }
        }
        void ButtonSendMessageClick(object sender, System.EventArgs e)
        {
            try
            {
                string msg = richTextTxMessage.Text;
                // New code to send strings
                NetworkStream networkStream = new NetworkStream(m_clientSocket);
                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(networkStream);
                streamWriter.WriteLine(msg);
                streamWriter.Flush();

                /* Use the following code to send bytes
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString ());
                if(m_clientSocket != null){
                    m_clientSocket.Send (byData);
                }
                */
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        public void WaitForData()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = m_clientSocket;
                // Start listening to the data asynchronously
                m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                        0, theSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBack,
                                                        theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }
        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[1024];
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                //richTextRxMessage.Text = richTextRxMessage.Text + szData;
                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        private void UpdateControls(bool connected)
        {
            buttonConnect.Enabled = !connected;
            buttonDisconnect.Enabled = connected;
            string connectStatus = connected ? "Connected" : "Not Connected";
            textBoxConnectStatus.Text = connectStatus;
        }
        void ButtonDisconnectClick(object sender, System.EventArgs e)
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Close();
                m_clientSocket = null;
                UpdateControls(false);
            }
        }
        //----------------------------------------------------	
        // This is a helper function used (for convenience) to 
        // get the IP address of the local machine
        //----------------------------------------------------
        String GetIP()
        {
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Grab the first IP addresses
            String IPStr = "";
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                IPStr = ipaddress.ToString();
                return IPStr;
            }
            return IPStr;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            richTextRxMessage.Clear();
        }
    }
}
