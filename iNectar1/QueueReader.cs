using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrackingCatalogLibrary;

namespace iNectar1
{
    public class QueueReader
    {
        private string _QueuePath;

        public string QueuePath
        {
            get { return _QueuePath; }
            set { _QueuePath = value; }
        }

        private string _SecondQueuePath;

        public string SecondQueuePath
        {
            get { return _SecondQueuePath; }
            set { _SecondQueuePath = value; }
        }

        private OperationMode _OperationMode;

        public OperationMode OperationMode
        {
            get { return _OperationMode; }
            set { _OperationMode = value; }
        }

        private MessageQueue _MainQueue;

        public MessageQueue MainQueue
        {
            get { return _MainQueue; }
            set { _MainQueue = value; }
        }

        private MessageQueue _SecondQueue;

        public MessageQueue SecondQueue
        {
            get { return _SecondQueue; }
            set { _SecondQueue = value; }
        }

        private int _Index;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }


        public QueueReader(string mainQueue, string secondQueue, OperationMode om, int i)
        {
            this.QueuePath = mainQueue;
            this.SecondQueuePath = secondQueue;
            this.OperationMode = om;
        }

        public async void Start()
        {
            MainQueue = new MessageQueue(this.QueuePath);
            SecondQueue = new MessageQueue(this.SecondQueuePath);

            if (this.OperationMode == OperationMode.QueueReader)
            {
                MainQueue.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String,mscorlib" });
                MainQueue.ReceiveCompleted += mq_ReceiveCompleted;
               // MainQueue.BeginReceive();
                var message = await Task.Factory.FromAsync<System.Messaging.Message>(
                               MainQueue.BeginReceive(),
                               MainQueue.EndReceive);
            }
        }

        void mq_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            System.Messaging.Message m = MainQueue.EndReceive(e.AsyncResult);
            //System.Messaging.Message m = MainQueue.EndReceive(e.AsyncResult);
            m.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "System.String,mscorlib" });
            //if(chkWriteMessageReceived.Checked) AddTextToMessageList(string.Format("{0}: {1}", DateTime.Now, (string)m.Body));
            string _msg = (string)m.Body;

            switch (OperationMode)
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


            MainQueue.BeginReceive();
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
                SendMessageToQueue(SerializeObjectAsXElement(ref newMessage).ToString(), SecondQueue);

                



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
    }
}
