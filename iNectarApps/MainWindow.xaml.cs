using iNectarApps.Models;
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

namespace iNectarApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        List<MessageQueue> _InputQueueList;
        DataContext _dataContext;
        private Socket m_mainSocket;
        public AsyncCallback pfnWorkerCallBack;
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
            _dataContext.Status = QueueStatus.Stopped;
            _dataContext.QueueThreadsRunning = 0;
            _dataContext.SocketServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["SocketServerPort"]);
            _dataContext.Clients = new List<iNectarClient>();

            #endregion

            this.DataContext = _dataContext;
        }

        #endregion

        #region QUEUE methods

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

                new Tools().WriteToLog(string.Format("Message received: {0}", _msg));
                SaveMessageToDataBase(_msg);
                
            }
            catch (Exception ex)
            {
                new Tools().WriteToLog(string.Format("ERROR: mq_ReceiveCompleted= {0}", ex.Message));
            }
            finally
            {
                //if (_dataContext.Status != QueueStatus.Stopped) 
                if (!queue.CanRead) new Tools().WriteToLog("Queue can't read");
                queue.BeginReceive();
            }
        }

        private void SaveMessageToDataBase(string msg)
        {
            try
            {
                string[] data;
                GpsMessage newMessage = new GpsMessage();

                #region Create GPS Message
                int pos = 0;

                pos = msg.IndexOf('#');

                if (pos >= 0)
                {
                    newMessage.SocketId = Convert.ToInt32(msg.Substring(0, pos));
                    pos++;
                }
                else
                    pos = 0;

                msg = msg.Substring(pos);

                newMessage.MessageHeader = msg.Substring(0, 2);

                msg = msg.Substring(2);
                data = msg.Split(new Char[] { ',' });

                newMessage.PackageFlag = data[0].Substring(0, 1);
                newMessage.ContentLength = Convert.ToInt32(data[0].Substring(1));
                newMessage.IMEI = data[1];
                //identificar si el dispositivo se encuentra en la BD
                GPSDevice device = new HTBDataLayer.Service().GetDeviceByIMEI(newMessage.IMEI);
                if (device != null)
                {
                    #region El dispositivo existe en la BD, continuar el proceso

                    //Get SocketID
                    device.SocketId = newMessage.SocketId;

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

                    #region Analizar el mensaje


                    iNectarClient agent = null;
                    //Revisar si algun agente tiene abierto el dispositivo
                    if (_dataContext.Clients.Count>0) //Hay agentes conectados
                    {
                        
                        foreach (iNectarClient client in _dataContext.Clients)
                        {
                            var _device = client.Devices.Where(p => p.IMEI == newMessage.IMEI).FirstOrDefault();
                            if (_device != null)
                            {
                                agent = client;
                                break; //Algun agente tiene el dispositivo asignado
                            }
                        }

                        if (agent==null) //Nimgun agente tiene asignado el dispositivo
                        {
                            //Buscar el agente que tenga menos dispositivos
                            foreach (iNectarClient client in _dataContext.Clients)
                            {
                                if (client.Devices.Count == 0)
                                {
                                    //No tienen ningun dispositivo, le asigno el dispositivo actual
                                    agent = client;
                                    agent.Devices.Add(device);
                                    break;
                                }

                                else if (agent == null)
                                    agent = client;
                                else if (client.Devices.Count < agent.Devices.Count) agent = client; //B usco el que tiene menos dispositivos
                            }
                        }

                        if (agent!=null)
                        {
                            AppMessage _appMsg = new AppMessage() { 
                                MessageType= AppMessageType.MessageReceived,
                                Message=newMessage,
                                Device=device
                            };
                            SendMsgToClient(_appMsg, agent.AppConnectionId);
                        }
                    }


                    #endregion

                    #endregion
                }
                else
                    new Tools().WriteToLog(string.Format("El dispositivo no esta registrado, IMEI={0}", newMessage.IMEI));

            }
            catch (Exception ex)
            {
                new Tools().WriteToLog(string.Format("ERROR: SaveMessageToDataBase= {0}", ex.Message));
            }
        }

        #endregion

        #region GPRS Socket Server methods

        private void StartServiceApps()
        {
            try
            {
                #region Initializa sockets
                
                _dataContext.SocketServerIP = GetIP();

                // Create the listening socket...
                m_mainSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, _dataContext.SocketServerPort);
                // Bind to local IP Address...
                m_mainSocket.Bind(ipLocal);
                // Start listening...
                m_mainSocket.Listen(4);
                // Create the call back for any client connections...
                m_mainSocket.BeginAccept(new AsyncCallback(OnSocketClientConnect), null);

                #endregion

                #region Initialize queues

                _InputQueueList = new List<MessageQueue>();
                for (int i = 0; i < _dataContext.QueueThreads; i++)
                {
                    System.Messaging.MessageQueue mq;
                    mq = new System.Messaging.MessageQueue(_dataContext.InputQueuePath);
                    mq.ReceiveCompleted += mq_ReceiveCompleted;
                    _InputQueueList.Add(mq);
                    _InputQueueList.ElementAt(_InputQueueList.Count - 1).BeginReceive();
                }

                #endregion
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

                //Add client to list
                this._dataContext.Clients.Add(new iNectarClient() { 
                    AppConnectionId=m_clientCount, 
                    Devices=new List<GPSDevice>()
                });

                AppMessage msg = new AppMessage() { 
                    MessageType= AppMessageType.SocketClientConnected,
                    AppConnectionId = m_clientCount
                };
                SendMsgToClient(msg, m_clientCount);

                //// Update the list box showing the list of clients (thread safe call)
                //UpdateClientListControl();

                // Let the worker Socket do the further processing for the 
                // just connected client
                WaitForData(workerSocket, m_clientCount);

                // Since the main Socket is now free, it can go back and wait for
                // other clients who are attempting to connect
                m_mainSocket.BeginAccept(new AsyncCallback(OnSocketClientConnect), null);
            }
            catch (ObjectDisposedException dex)
            {
                new Tools().WriteToLog(string.Format("ERROR: OnSocketClientConnect= {0}", dex.Message));
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                new Tools().WriteToLog(string.Format("ERROR: OnSocketClientConnect= {0}", se.Message));
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
            SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
            try
            {
                object _objMsg = (new Tools().ByteArrayToObject(theSockId.dataBuffer));

                if (_objMsg != null)
	            {
                    AppMessage msg = (AppMessage)_objMsg;

                    switch (msg.MessageType)
                    {
                        case AppMessageType.None:
                            break;
                        case AppMessageType.SocketClientConnected:
                            break;
                        case AppMessageType.SocketClientDisonnected:
                            break;
                        case AppMessageType.MessageSent:
                            SendMessageToDevice(msg);
                            break;
                        case AppMessageType.MessageReceived:
                            break;
                        default:
                            break;
                    }
	            }
                
                // Continue the waiting for data on the Socket
                WaitForData(theSockId.m_currentSocket, theSockId.m_clientNumber);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054) // Error code for Connection reset by peer
                {
                    string msg = "Client " + theSockId.m_clientNumber + " Disconnected" + "\n";

                    // Remove the reference to the worker socket of the closed client
                    // so that this object will get garbage collected
                    m_workerSocketList[theSockId.m_clientNumber - 1] = null;
                }
                else
                {
                    MessageBox.Show(se.Message);
                }
            }
        }

        private void SendMessageToDevice(AppMessage msg)
        {
            iNectarClient client = _dataContext.Clients.Where(p => p.AppConnectionId == msg.AppConnectionId).FirstOrDefault();

            var device = client.Devices.Where(p => p.IMEI == msg.Message.IMEI).FirstOrDefault();

            if (device==null)
            {
                //Add to list
                //Need to get it from DB
                client.Devices.Add(new GPSDevice() { 
                    IMEI = msg.Message.IMEI
                });
            }

            //Construir mensaje
            string msg2Send = new Tools(). GenerateMessage(msg.Message);
            
            //5.- Enviar el mensaje al dispositivo (iNectar 2.0)
            using (System.Messaging.MessageQueue q_msgReceived = new System.Messaging.MessageQueue(_dataContext.OutputQueuePath))
            {
                //Send message to queue
                q_msgReceived.Send(msg2Send);
                q_msgReceived.Close();
            }

            //6.- Confirmar al cliente que el mensaje fue enviado
            AppMessage msgClient = new AppMessage() 
            {
                MessageType= AppMessageType.MessageSent
            };
            SendMsgToClient(msgClient, msg.AppConnectionId);
        }

       

        void SendMsgToClient(AppMessage msg, int clientNumber)
        {
            try
            {
                // Convert the reply to byte array
                //byte[] byData = System.Text.Encoding.ASCII.GetBytes(msg);

                byte[] byData = new Tools().ObjectToByteArray(msg);
                Socket workerSocket = (Socket)m_workerSocketList[clientNumber - 1];
                workerSocket.Send(byData);
            }
            catch (Exception ex)
            {
                new Tools().WriteToLog(string.Format("ERROR: SendMsgToClient= {0}", ex.Message));
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


        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            cmdStart.Visibility = System.Windows.Visibility.Collapsed;
            cmdStop.Visibility = System.Windows.Visibility.Visible;
            _dataContext.Status = QueueStatus.Started;

            StartServiceApps();
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            CloseSockets();
            cmdStart.Visibility = System.Windows.Visibility.Visible;
            cmdStop.Visibility = System.Windows.Visibility.Collapsed;
            _dataContext.Status = QueueStatus.Stopped;
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
