using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TrackingCatalogLibrary;

namespace iNectar1
{
    public partial class Main : Form
    {
        delegate void BindMessageListToDataGridDelegate();
        public delegate void UpdateRichEditCallback(string text);

        //private List<QueueReader> ReceivedMessages = new List<QueueReader>();
        private OperationMode CurrentOperationMode;
        private int ServerPort;
        private int ClientsConnected;

        //List<QueueReader> MainQueueList;
        //System.Messaging.MessageQueue MainQueue;
        //System.Messaging.MessageQueue secondQueue;

        private QueueServiceStatus _QueueServiceStatus;
        private List<MessageQueue> _InputQueueList;
        private int QueueThreads;
        private Socket m_mainSocket;
        private System.Collections.ArrayList m_workerSocketList =
               ArrayList.Synchronized(new System.Collections.ArrayList());
        public AsyncCallback pfnWorkerCallBack;

        public int TotalMessagesReceived = 0;

        #region General


        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        private async void LoadData()
        {
            richTextBoxReceivedMsg.Text = "Started";
            if (ConfigurationManager.AppSettings["OperationMode"].ToLower() == "duplex")
            {
                this.CurrentOperationMode = OperationMode.Duplex;
                this.ServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["DuplexServerPort"]);
                this.lblPort.Text = string.Format("{0}", this.ServerPort);

                CreateClientService();
            }
            else
            {
                this.CurrentOperationMode = OperationMode.QueueReader;
                QueueThreads = Convert.ToInt32(ConfigurationManager.AppSettings["QueueThreads"]);
                _InputQueueList = new List<MessageQueue>();
                for (int i = 0; i < QueueThreads; i++)
                {
                    MessageQueue mq = new MessageQueue(txtQueue.Text);
                    nq.ReceiveCompleted += mq_ReceiveCompleted;
                    _InputQueueList.Add(mq);
                }
            }

            ShowHideServerControls(this.CurrentOperationMode == OperationMode.Duplex);

            this.Text = string.Format("iNectar ({0})", this.CurrentOperationMode.ToString());
            this.lblClientsConnected.Text = string.Format("{0}", this.ClientsConnected);

            txtQueue.Text = ConfigurationManager.AppSettings["MessageInputQueue"];
            txtQueue.Enabled = false;



        }
        private void SaveMessageToDataBase(string msg)
        {
            try
            {
                string[] data;
                GpsMessage newMessage = new GpsMessage();

                #region Create GPS Message

                newMessage.MessageHeader = msg.Substring(0, 2);

                msg = msg.Substring(2);
                data = msg.Split(new Char[] { ',' });


                newMessage.PackageFlag = data[0].Substring(0, 1);
                newMessage.ContentLength = Convert.ToInt32(data[0].Substring(1));
                newMessage.IMEI = data[1];
                newMessage.CommandCode = data[2];
                if (newMessage.CommandCode == "A70")
                {
                    //Get Autorize phoneNumbers
                    List<string> numbers = new List<string>();
                    for (int i = 3; i < data.Length; i++)
                    {
                        numbers.Add(data[i].IndexOf("*") >= 0 ? data[i].Substring(0, data[i].IndexOf("*")) : data[i]);
                    }
                    newMessage.AutPhoneNumbers = string.Join(";", numbers.ToArray());
                }
                else
                {
                    newMessage.EventCode = data[3];

                    if (newMessage.EventCode.IndexOf("*") >= 0)
                    {
                        //Mensaje de confirmación
                        newMessage.EventCode = newMessage.EventCode.Substring(0, newMessage.EventCode.IndexOf("*"));
                    }

                    if (data.Length > 4)
                    {
                        newMessage.Latitude = Convert.ToDouble(data[4]);
                        newMessage.Longitude = Convert.ToDouble(data[5]);
                        newMessage.DateMessage = new DateTime(2000 + Convert.ToInt32(data[6].Substring(0, 2)),
                                                                Convert.ToInt32(data[6].Substring(2, 2)),
                                                                Convert.ToInt32(data[6].Substring(4, 2)),
                                                                Convert.ToInt32(data[6].Substring(6, 2)),
                                                                Convert.ToInt32(data[6].Substring(8, 2)),
                                                                Convert.ToInt32(data[6].Substring(10, 2)));
                        newMessage.GPSStatus = data[7];
                        newMessage.Satellites = Convert.ToInt32(data[8]);
                        newMessage.GSMSignal = Convert.ToInt32(data[9]);
                        newMessage.Speed = Convert.ToDouble(data[10]);
                        newMessage.Heading = Convert.ToDouble(data[11]);
                        newMessage.HDOP = Convert.ToDouble(data[12]);
                        newMessage.Altitude = Convert.ToDouble(data[13]);
                        newMessage.Mileage = Convert.ToDouble(data[14]);
                        newMessage.Runtime = Convert.ToDouble(data[15]);
                        newMessage.MessageBaseID = data[16];
                        newMessage.GPSStatus = data[17];
                        newMessage.AD = data[18];
                        if (data[data.Length - 1].IndexOf("*") >= 0) newMessage.Checksum = data[data.Length - 1].Substring(data[data.Length - 1].IndexOf("*") + 1, 2);
                    }
                }

                #endregion

                XElement body = SerializeObjectAsXElement(ref newMessage);
                Int64 messageId = new HTBDataLayer.Service().SaveMessage(body);

                newMessage.MessageId = messageId;

                //Put message into the second queue
                SendMessageToQueue(SerializeObjectAsXElement(ref newMessage).ToString(), secondQueue);

                //#region Enviar mensaje a los clientes conectados

                //SendMsgToClient(newMessage, 1);

                //#endregion
            }
            catch (Exception)
            {

            }


        }

        public static System.Xml.Linq.XElement SerializeObjectAsXElement<T>(ref T obj)
        {
            XElement result = default(XElement);
            System.Xml.Serialization.XmlSerializerNamespaces namespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using (StringWriter w = new StringWriter())
            {

                dynamic x = new System.Xml.Serialization.XmlSerializer(typeof(T));
                x.Serialize(w, obj, namespaces);
                result = XElement.Parse(w.ToString());
                w.Close();
            }
            return result;
        }



        private void ShowHideServerControls(bool showControl)
        {
            lblLabelPort.Visible = showControl;
            lblPort.Visible = showControl;
            lblLabelClientsConnected.Visible = showControl;
            lblClientsConnected.Visible = showControl;
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        #endregion


        #region Input Queue methods and events

        void mq_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            System.Messaging.Message m = ((MessageQueue)sender).EndReceive(e.AsyncResult);
           
            m.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String,mscorlib" });
            if(chkWriteMessageReceived.Checked) AddTextToMessageList(string.Format("{0}: {1}", DateTime.Now, (string)m.Body));
            string _msg = (string)m.Body;

            switch (CurrentOperationMode)
            {
                case OperationMode.QueueReader:
                    if (_msg.Length > 5)
                    {
                        SaveMessageToDataBase(_msg);
                    }
                    break;
                case OperationMode.Analyzer:
                    AnalyzeMessage(_msg);
                    break;
                case OperationMode.Duplex:
                    break;
                default:
                    break;
            }
            if(_QueueServiceStatus==QueueServiceStatus.Started) ((MessageQueue)sender).BeginReceive();
        }

        #endregion

       
        private void AddTextToMessageList(string msg)
        {
            // Check to see if this method is called from a thread 
            // other than the one created the control
            if (InvokeRequired)
            {
                // We cannot update the GUI on this thread.
                // All GUI controls are to be updated by the main (GUI) thread.
                // Hence we will use the invoke method on the control which will
                // be called when the Main thread is free
                // Do UI update on UI thread
                object[] pList = { msg };
                richTextBoxReceivedMsg.BeginInvoke(new UpdateRichEditCallback(OnUpdateRichEdit), pList);
            }
            else
            {
                // This is the main thread which created this control, hence update it
                // directly 
                OnUpdateRichEdit(msg);
            }
        }

        private void OnUpdateRichEdit(string msg)
        {
            TotalMessagesReceived++;
            
            richTextBoxReceivedMsg.Text= string.Format("{0:yyyy-MM-DD HH:mm:ss.fff}: Total messages received={1}", DateTime.Now, TotalMessagesReceived);
        }


        #region Socket Services

        private void CreateClientService()
        {
            try
            {
                // Check the port value
                if (this.ServerPort == 0) return;

                ////Start queue for writing
                //mq = new System.Messaging.MessageQueue(ConfigurationManager.AppSettings["DuplexQueueApp"]);

                // Create the listening socket...
                m_mainSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, this.ServerPort);
                // Bind to local IP Address...
                m_mainSocket.Bind(ipLocal);
                // Start listening...
                m_mainSocket.Listen(4);
                // Create the call back for any client connections...
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

                lblStatus.Text = "Started";
                lblServerIP.Text = GetIP();

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                // Here we complete/end the BeginAccept() asynchronous call
                // by calling EndAccept() - which returns the reference to
                // a new Socket object
                Socket workerSocket = m_mainSocket.EndAccept(asyn);

                // Now increment the client count for this client 
                // in a thread safe manner
                Interlocked.Increment(ref this.ClientsConnected);

                // Add the workerSocket reference to our ArrayList
                m_workerSocketList.Add(workerSocket);

                //// Send a welcome message to client
                //string msg = "Welcome client ";// +m_clientCount + "\n";
                //SendMsgToClient(msg, this.ClientsConnected);

                // Update the list box showing the list of clients (thread safe call)


                // Let the worker Socket do the further processing for the 
                // just connected client
                WaitForData(workerSocket, this.ClientsConnected);

                // Since the main Socket is now free, it can go back and wait for
                // other clients who are attempting to connect
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

                UpdateClientsConnected();

            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }

        }

        void SendMsgToClient(GpsMessage msg, int clientNumber)
        {
            //// Convert the reply to byte array
            //byte[] byData = System.Text.Encoding.ASCII.GetBytes("Alfredo_test");

            //Buscar si existe algun cliente que deba recibir el mensaje
            //Si no hay verificar si el mensaje debe ser mostrado a algun cliente conectado.




            Socket workerSocket = (Socket)m_workerSocketList[clientNumber - 1];
            workerSocket.Send(ObjectToByteArray(msg));

            //workerSocket.Send(byData);
        }

        public void WaitForData(System.Net.Sockets.Socket soc, int clientNumber)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    // Specify the call back function which is to be 
                    // invoked when there is any write activity by the 
                    // connected client
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket(soc, clientNumber);

                soc.BeginReceive(theSocPkt.dataBuffer, 0,
                    theSocPkt.dataBuffer.Length,
                    SocketFlags.None,
                    pfnWorkerCallBack,
                    theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                // Complete the BeginReceive() asynchronous call by EndReceive() method
                // which will return the number of characters written to the stream 
                // by the client
                int iRx = socketData.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                // Extract the characters as a buffer
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(socketData.dataBuffer,
                    0, iRx, chars, 0);

                System.String szData = new System.String(chars);
                string msg = "" + socketData.m_clientNumber + ":";

                //using (StreamWriter w = new StreamWriter(@"MessageServer.log", true))
                //{
                //    w.WriteLine(string.Format("{0}: {1}", DateTime.Now , msg + szData));
                //    w.Close();
                //}

                //// Send back the reply to the client
                //string replyMsg = "Server Reply:" + szData.ToUpper();
                //// Convert the reply to byte array
                //byte[] byData = System.Text.Encoding.ASCII.GetBytes(replyMsg);

                //Socket workerSocket = (Socket)socketData.m_currentSocket;
                //workerSocket.Send(byData);


                // Continue the waiting for data on the Socket
                WaitForData(socketData.m_currentSocket, socketData.m_clientNumber);

            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054) // Error code for Connection reset by peer
                {
                    string msg = "Client " + socketData.m_clientNumber + " Disconnected" + "\n";

                    // Remove the reference to the worker socket of the closed client
                    // so that this object will get garbage collected
                    m_workerSocketList[socketData.m_clientNumber - 1] = null;
                }
                else
                {
                    MessageBox.Show(se.Message);
                }
            }
        }

        private void UpdateClientsConnected()
        {
            string msg = string.Format("{0}", this.ClientsConnected);
            if (InvokeRequired)
            {
                this.lblClientsConnected.BeginInvoke(new UpdateRichEditCallback(OnUpdateClientConnected), msg);
            }
            else
            {
                OnUpdateClientConnected(msg);
            }
        }

        private void OnUpdateClientConnected(string msg)
        {
            this.lblClientsConnected.Text = msg;
        }

        String GetIP()
        {
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);

            // Grab the first IP addresses
            String IPStr = "";

            if (iphostentry.AddressList.Length > 2)
            {
                IPStr = iphostentry.AddressList[2].ToString();
                return IPStr;
            }

            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                IPStr = ipaddress.ToString();
                return IPStr;
            }
            return IPStr;
        }

        #endregion


        #region Analyzer

        private void AnalyzeMessage(string msg)
        {

        }

        private void SendMessageToQueue(string message, MessageQueue _mq)
        {
            try
            {
                _mq.Send(message);
            }
            catch (Exception)
            {
            }

        }

        #endregion


        //private void StartReaders()
        //{
        //    while (true)
        //    {
        //        int msgCount = 0;

        //        Parallel.ForEach(Enumerable.Range(0, 20), (i) =>
        //        {
        //            MessageQueue queue = new MessageQueue(ConfigurationManager.AppSettings["MessageInputQueue"]);

        //            try
        //            {
        //                System.Messaging.Message msg = queue.Receive(TimeSpan.Zero);

        //                msg.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String,mscorlib" });
        //                string _msg = (string)msg.Body;

        //                switch (CurrentOperationMode)
        //                {
        //                    case OperationMode.QueueReader:
        //                        if (_msg.Length > 5)
        //                        {
        //                            SaveMessageToDataBase(_msg);
        //                        }
        //                        break;
        //                    case OperationMode.Analyzer:
        //                        AnalyzeMessage(_msg);
        //                        break;
        //                    case OperationMode.Duplex:
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                Interlocked.Increment(ref msgCount);
        //            }
        //            catch (MessageQueueException mqex)
        //            {
        //                if (mqex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
        //                {
        //                    return; // nothing in queue
        //                }
        //                else throw;
        //            }
        //        }
        //        );
        //        //if (msgCount < 20) Thread.Sleep(1000); // nothing more to do, take a break
        //    }

        //}

        private void cmdStart_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
    public enum OperationMode
    {
        QueueReader,
        Analyzer,
        Duplex
    }

    public enum QueueServiceStatus
    {
        Stopped,
        Started
    }

    public class SocketPacket
    {
        // Constructor which takes a Socket and a client number
        public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
        {
            m_currentSocket = socket;
            m_clientNumber = clientNumber;
        }
        public System.Net.Sockets.Socket m_currentSocket;
        public int m_clientNumber;
        // Buffer to store the data sent by the client
        public byte[] dataBuffer = new byte[1024];
    }
}
