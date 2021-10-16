using AppSimulator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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
using TrackingCatalogLibrary.BusinessEntities;

namespace AppSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        AppViewModel _dataContext;
        public Socket m_clientSocket;
        public AsyncCallback m_pfnCallBack;
        IAsyncResult m_result;
        SerialPort _serialPort;
        #endregion

        #region Constructors
        
        public MainWindow()
        {
            InitializeComponent();

            _dataContext = new AppViewModel();

            _dataContext.NectarServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["SocketServerPort"]);
            _dataContext.NectarServerURL = GetIP();
            _dataContext.Devices = new List<GPSDevice>();
            this.DataContext = _dataContext;
        }

        #endregion

        #region Event handlers
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_dataContext.NectarServerURL) || _dataContext.NectarServerPort<=0)
            {
                MessageBox.Show("IP Address and Port Number are required to connect to the Server\n");
                return;
            }
            try
            {
                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Cet the remote IP address
                IPAddress ip = IPAddress.Parse(_dataContext.NectarServerURL);
                int iPortNo = _dataContext.NectarServerPort;
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {
                    
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                string str;
                str = "\nConnection failed, is the server running?\n" + se.Message;
                MessageBox.Show(str);
            }
        }

        #endregion

        #region Methods

        private void WaitForData()
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

        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                object _msgObj = (new Tools().ByteArrayToObject(theSockId.dataBuffer));
                
                if (_msgObj!=null)
	            {
                    AppMessage msg = (AppMessage)_msgObj;

                    switch (msg.MessageType)
                    {
                        case AppMessageType.None:
                            break;
                        case AppMessageType.SocketClientConnected:
                            this._dataContext.AppConnectionId = msg.AppConnectionId;
                            break;
                        case AppMessageType.SocketClientDisonnected:
                            break;
                        case AppMessageType.MessageSent:
                            break;
                        case AppMessageType.MessageReceived:
                            GPSDevice _device = null;
                            _device = _dataContext.Devices.Where(p => p.IMEI == msg.Message.IMEI).FirstOrDefault();
                        
                            if (_device==null)
                            {
                                //No existe en la lista, agregarlo
                                _dataContext.Devices.Add(msg.Device);
                            }

                            VerifyGeofenceAccess(msg);

                            break;
                        default:
                            break;
                    }
                    string _commandCode = string.Empty;
                    if(msg.Message!= null) _commandCode=msg.Message.CommandCode;
                    _dataContext.Log += string.Format("{0:yyy-MM-dd HH:mm:ss.ttt} - {1}: {2}{3}", DateTime.Now, msg.MessageType.ToString(), _commandCode, Environment.NewLine);
                   
	            }
                
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
            finally
            {
                WaitForData();
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
        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[4096];
        }

        public void SendMessageToNectar(AppMessage msg)
        {
            try
            {
                byte[] byData = new Tools().ObjectToByteArray(msg);

                //Use the following code to send bytes
                if(m_clientSocket != null){
                    m_clientSocket.Send (byData);
                }
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        #endregion

        private void cmdSendMsg_Click(object sender, RoutedEventArgs e)
        {
            //Seleccionar el device al que se le va a enviar el mensaje
            //en este caso de ejemplo siempre tomamos el primer device
            GPSDevice _device = new GPSDevice();//_dataContext.Devices[0];
            _device.IMEI = "353358017784062";

            AppMessage msg = new AppMessage()
            {
                MessageType = AppMessageType.MessageSent,
                AppConnectionId = _dataContext.AppConnectionId,
                Command = _dataContext.SelectedCommand,
                VehicleId = _dataContext.VehicleId,
                Message = new TrackingCatalogLibrary.GpsMessage() 
                {
                    IMEI = _device.IMEI,
                    Data=_dataContext.Parameters,
                    CommandCode = _dataContext.SelectedCommand.CommandName,
                    SocketId=_device.SocketId
                }
            };

            SendMessageToNectar(msg);
        }


        private void VerifyGeofenceAccess(AppMessage msg)
        {
            VerifyGeofencePoint info = new DataService().GetGeofenceInfo(msg.Message.Latitude.ToString(), msg.Message.Longitude.ToString());
            if (info!=null)
            {
                if (!string.IsNullOrEmpty(info.Email))
                {
                    string body = string.Format("<h1>Aviso: Transporte llegando al destino</h1><br/>El transporte se encuentra cerca del destino <strong>{0}</strong> ({1:yyyy-MM-dd HH:mm:ss})<br/><br/><a href=\"http://maps.google.com/maps?f=q&hl=es&q={2},{3}&ie=UTF8&z=16&iwloc=addr&om=1\">Ver ubicación en el mapa</a>",
                                info.CompanyName, msg.Message.DateReceived, msg.Message.Latitude, msg.Message.Longitude);
                    SendEmail("schoolbustracker@outlook.com",
                                info.Email, 
                                "Trasporte llegando a destino", 
                                body, 
                                string.Empty);
                }
                if (!string.IsNullOrEmpty(info.PhoneNumber))
                {
                    string smsMsg = string.Format("Transporte llegando destino http://maps.google.com/maps?f=q&hl=es&q={2},{3}&ie=UTF8&z=16&iwloc=addr&om=1",
                                info.CompanyName, DateTime.Now, msg.Message.Latitude, msg.Message.Longitude);

                    SendSMS(smsMsg, info.PhoneNumber);
                }
            }
        }

        private void SendEmail(string from, string to, string subject, string body, string bcc)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress(from);
            email.Subject = subject ;
            email.Body = body;

            string[] emailsTo = to.Split(new Char[]{','});
            for (int i = 0; i < emailsTo.Length; i++)
                email.To.Add(new MailAddress(emailsTo[i].Trim()));

            if (!string.IsNullOrEmpty(bcc))
            {
                string[] bccLIst = bcc.Split(new Char[] { ',' });
                for (int i = 0; i < bccLIst.Length; i++)
                    email.Bcc.Add(new MailAddress(bccLIst[i].Trim()));
            }
            
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.live.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("schoolbustracker@outlook.com", "School@2016");

                smtp.Send(email);

                email.Dispose();
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void SendSMS(string msg, string phoneNumbers)
        {
            try
            {
                
                string[] phoneList = phoneNumbers.Split(new Char[] { ',' });

                for (int i = 0; i < phoneList.Length; i++)
                {
                    //Activar modo SMS en dispositivo GSM
                    _serialPort.Write("AT+CMGF=1\r");
                    System.Threading.Thread.Sleep(1000);

                    //Establecer número de teléfono de destino del SMS
                    _serialPort.Write("AT+CMGS=\"" + phoneList[i].Trim() + "\"\r");
                    System.Threading.Thread.Sleep(1500);
                    //Enviar texto SMS a dispositivo GSM
                    _serialPort.Write(msg + (char)26);

                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void ConnectToGPRSModem()
        {
            int posBusqueda = 0;
            string resultadoDevuelto = null;

            try
            {
                AbrirPuertoSerie();

                //Comprobar si hay un módem GSM conectado al puerto serie
                _serialPort.Write("AT\r");
                //Esperamos un segundo para dar tiempo a 
                //las comunicaciones serie
                System.Threading.Thread.Sleep(1000);
                //Si el comando AT devuelve OK es que hay módem GSM
                if (_serialPort.ReadExisting().IndexOf("OK") > 0)
                {
                    //bePModem.Text = "Módem GSM activo";
                    //bePModem.ForeColor = Color.Green;
                }
                else
                {
                    //bePModem.Text = "Módem GSM no encontrado";
                    //bePModem.ForeColor = Color.Red;
                }
                //Limpiamos el bugger de datos de entrada 
                //devueltos por el módem GSM
                _serialPort.DiscardInBuffer();

                //Comprobar si el módem módem GSM está conectado a una red GSM
                _serialPort.Write("AT+CREG?\r");
                System.Threading.Thread.Sleep(1000);
                //Si el comando AT+CREG? devuelve 0,1 es que hay red móvil
                if (_serialPort.ReadExisting().IndexOf("0,1")>0)
                {
                    //bePRedMovil.Text = "Red móvil disponible";
                    //bePRedMovil.ForeColor = Color.Green;
                }
                else
                {
                    //bePRedMovil.Text = "No hay red móvil";
                    //bePRedMovil.ForeColor = Color.Red;
                }
                _serialPort.DiscardInBuffer();

                //Obtener la intensidad de la señal (cobertura) del módem GSM
                //máximo: 31.99
                _serialPort.Write("AT+CSQ\r");
                System.Threading.Thread.Sleep(1000);
                resultadoDevuelto = _serialPort.ReadExisting();
                posBusqueda = resultadoDevuelto.IndexOf("+CSQ: ");

                _serialPort.DiscardInBuffer();

                //Obtener ID del dispositivo GSM (IMEI)
                _serialPort.Write("AT+CGSN\r");
                System.Threading.Thread.Sleep(1000);
                resultadoDevuelto = _serialPort.ReadExisting();
                posBusqueda = resultadoDevuelto.IndexOf("CGSN");
                if (posBusqueda >= 0)
                {
                    //resultadoDevuelto = quitarSaltosLinea(resultadoDevuelto, " ");
                    //bePID.Text = "IMEI: " + Strings.Replace(Strings.Mid(resultadoDevuelto, posBusqueda + Strings.Len("CGSN") + 1, Strings.Len(resultadoDevuelto)), "OK", "");
                }

                //btConectar.Text = "Desconectar";
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void AbrirPuertoSerie()
        {
            try
            {
                _serialPort = new SerialPort();

                _serialPort.PortName = "COM6";
                _serialPort.BaudRate = 9600;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = System.IO.Ports.StopBits.One;
                _serialPort.Parity = System.IO.Ports.Parity.None;
                _serialPort.Handshake = System.IO.Ports.Handshake.None;
                _serialPort.DtrEnable = false;

                _serialPort.ReadBufferSize = 2048;
                _serialPort.WriteBufferSize = 1024;
                _serialPort.WriteTimeout = 500;
                _serialPort.RtsEnable = true;
                _serialPort.Encoding = System.Text.Encoding.Default;
                _serialPort.Open();

            }
            catch (Exception ex)
            {
                //
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectToGPRSModem();
        }
    }
}
