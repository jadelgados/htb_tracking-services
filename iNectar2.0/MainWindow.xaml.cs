
 using iNectar2._0.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows; 
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using TrackingCatalogLibrary;
using TrackingCatalogLibrary.BusinessEntities;

namespace iNectar2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        DataContext _dataContext;
        private Socket m_mainSocket;
        private AsyncCallback pfnWorkerCallBack;
        private List<MessageQueue> _OutputQueueList;

        private System.Collections.ArrayList m_workerSocketList = ArrayList.Synchronized(new System.Collections.ArrayList());

        // The following variable will keep track of the cumulative 
        // total number of clients connected at any time. Since multiple threads
        // can access this variable, modifying this variable should be done
        // in a thread safe manner
        private int m_clientCount = 0;
        
        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            #region Load parameters

            _dataContext = new DataContext();
            _dataContext.InputQueuePath = ConfigurationManager.AppSettings["Q_MsgReceived"];
            _dataContext.OutputQueuePath = ConfigurationManager.AppSettings["Q_MsgSend"];
            _dataContext.QueueThreads = Convert.ToInt32(ConfigurationManager.AppSettings["QueueThreads"]);
            //_dataContext.QueueThreads = Convert.ToInt32(ConfigurationManager.AppSettings["QueueThreads"]);
            _dataContext.Status = QueueStatus.Stopped;
            //_dataContext.QueueThreadsRunning = 0;
            _dataContext.SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["SocketServerPort"]);
            //_dataContext.NectarServerType = (ServerType)Convert.ToInt32(ConfigurationManager.AppSettings["NectarServerType"]);

            #endregion

            this.DataContext = _dataContext;
        }

        #endregion

        #region GPRS Socket Server methods

        private void StartServiceGPRS()
        {
            try
            {
                //Get Server IP
                _dataContext.SocketServerIP = GetIP();

                // Create the listening socket...
                m_mainSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, _dataContext.SocketServerPort);
                // Bind to local IP Address...
                m_mainSocket.Bind(ipLocal);
                // Start listening...
                m_mainSocket.Listen(10);
                // Create the call back for any client connections...
                m_mainSocket.BeginAccept(new AsyncCallback(OnSocketClientConnect), null);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void OnSocketClientConnect(IAsyncResult asyn)
        {
            try
            {
                // Here we complete/end the BeginAccept() asynchronous call
                // by calling EndAccept() - which returns the reference to
                // a new Socket object
                Socket workerSocket = m_mainSocket.EndAccept(asyn);

                // Now increment the client count for this client 
                // in a thread safe manner
                Interlocked.Increment(ref m_clientCount);

                _dataContext.SocketServerClientsConnected = m_clientCount;

                // Add the workerSocket reference to our ArrayList
                m_workerSocketList.Add(workerSocket);

                // Send a welcome message to client
                //string msg = "<H1>Welcome client " + m_clientCount + "</H1>";
                //SendMsgToClient(msg, m_clientCount);

                //// Update the list box showing the list of clients (thread safe call)
                //UpdateClientListControl();

                // Let the worker Socket do the further processing for the 
                // just connected client
                WaitForData(workerSocket, m_clientCount);

                // Since the main Socket is now free, it can go back and wait for
                // other clients who are attempting to connect
                m_mainSocket.BeginAccept(new AsyncCallback(OnSocketClientConnect), null);
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

        public void WaitForData(System.Net.Sockets.Socket soc, int clientNumber)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    // Specify the call back function which is to be 
                    // invoked when there is any write activity by the 
                    // connected client
                    pfnWorkerCallBack = new AsyncCallback(OnSocketnDataReceived);
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

        public void OnSocketnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                this._dataContext.LastMessageReceivedAt = DateTime.Now;

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

                //Analizar si el mensaje es válido
               if (szData.StartsWith("$"))
                {
                   _dataContext.ReceivedMessages++;
                    //Create new instance for write queue message received
                    using (System.Messaging.MessageQueue q_msgReceived = new System.Messaging.MessageQueue(_dataContext.InputQueuePath))
                    {
                        new Tools().WriteToLog(string.Format("Message Received: SocketId={0}, Message={1}", socketData.m_clientNumber, szData));
                        //Send message to queue
                        new Tools().WriteToLog(string.Format("{0}#{1}", socketData.m_clientNumber, szData));
                        q_msgReceived.Send(string.Format("{0}#{1}",socketData.m_clientNumber, szData));
                        q_msgReceived.Close();
                    }
                }

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

        void CloseSockets()
        {
            if (m_mainSocket != null)
            {
                m_mainSocket.Close();
            }
            Socket workerSocket = null;
            for (int i = 0; i < m_workerSocketList.Count; i++)
            {
                workerSocket = (Socket)m_workerSocketList[i];
                if (workerSocket != null)
                {
                    workerSocket.Close();
                    workerSocket = null;
                }
            }
        }

        #endregion

        #region Queue out methods

        private void InitializeOutputQueues()
        {
                _OutputQueueList = new List<MessageQueue>();
                for (int i = 0; i < _dataContext.QueueThreads; i++)
                {
                    System.Messaging.MessageQueue mq;
                    mq = new System.Messaging.MessageQueue(_dataContext.OutputQueuePath);
                    mq.ReceiveCompleted += mq_ReceiveCompleted;
                    _OutputQueueList.Add(mq);
                    _OutputQueueList.ElementAt(_OutputQueueList.Count - 1).BeginReceive();
                }
        }

        void mq_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue queue = (MessageQueue)sender;
            try
            {
                Message m = queue.EndReceive(asyncResult.AsyncResult);

                m.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String,mscorlib" });
                _dataContext.ReceivedMessages++;
                _dataContext.LastMessageReceivedAt = DateTime.Now;

                string _msg = (string)m.Body;


                SendMsgToDevice(_msg);

            }
            catch (Exception ex)
            {
                //Utils.WriteToLog(ex.Message);
            }
            finally
            {
                if (_dataContext.Status != QueueStatus.Stopped) queue.BeginReceive();
            }
        }

        private void SendMsgToDevice(string msg)
        {
            try
            {
                int _SocketId=0;
                int pos = 0;

                pos = msg.IndexOf('#');

                if (pos >= 0)
                {
                    _SocketId = Convert.ToInt32(msg.Substring(0, pos));
                    pos++;
                }
                else
                    pos = 0;

                msg = msg.Substring(pos);

                SendMessageToDeviceClient(msg, _SocketId);
            }
            catch (Exception)
            {

            }
        }

        void SendMessageToDeviceClient(string msg, int clientNumber)
        {
            try
            {
                new Tools().WriteToLog(string.Format("Send Message to Device: SocketId={0}, Message={1}", clientNumber, msg));
                // Convert the reply to byte array
                //byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);

                //byte[] byData = new Tools().ObjectToByteArray(msg);
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);
                Socket workerSocket = (Socket)m_workerSocketList[clientNumber - 1];
                workerSocket.Send(byData);
            }
            catch (Exception ex)
            {
                //UNHANDLED TODO: write to log
            }
        }
        #endregion


        #region APPs Socket Server methods
        //TO DO
        #endregion

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.Visibility = System.Windows.Visibility.Collapsed;
            cmdStop.Visibility = System.Windows.Visibility.Visible;
            _dataContext.Status = QueueStatus.Started;

            StartServiceGPRS();

            InitializeOutputQueues();
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            CloseSockets();

            cmdStart.Visibility = System.Windows.Visibility.Visible;
            cmdStop.Visibility = System.Windows.Visibility.Collapsed;
            _dataContext.Status = QueueStatus.Stopped;

            //Utils.WriteToLog("Queue stopped.");
            //Utils.WriteToLog("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
        public byte[] dataBuffer = new byte[4096];
    }
}
