using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrackingCatalogLibrary.BusinessEntities;

namespace iNectarApps.Models
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _InputQueuePath;
        public string InputQueuePath
        {
            get { return _InputQueuePath; }
            set
            {
                if (value != this._InputQueuePath)
                {
                    this._InputQueuePath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _OutputQueuePath;
        public string OutputQueuePath
        {
            get { return _OutputQueuePath; }
            set
            {
                if (value != this._OutputQueuePath)
                {
                    this._OutputQueuePath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _QueueThreads;

        public int QueueThreads
        {
            get { return _QueueThreads; }
            set
            {
                if (value != this._QueueThreads)
                {
                    this._QueueThreads = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _QueueThreadsRunning;

        public int QueueThreadsRunning
        {
            get { return _QueueThreadsRunning; }
            set
            {
                if (value != this._QueueThreadsRunning)
                {
                    this._QueueThreadsRunning = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private QueueStatus _Status;

        public QueueStatus Status
        {
            get { return _Status; }
            set
            {
                if (value != this._Status)
                {
                    this._Status = value;
                    switch (value)
                    {
                        case QueueStatus.Stopped:
                            this.BackColorStatus = "#ff000";
                            break;
                        case QueueStatus.Started:
                            this.BackColorStatus = "#00ff21";
                            break;
                        default:
                            this.BackColorStatus = "#000000";
                            break;
                    }
                    NotifyPropertyChanged();
                }
            }
        }

        private int _ReceivedMessages;

        public int ReceivedMessages
        {
            get { return _ReceivedMessages; }
            set
            {
                if (value != this._ReceivedMessages)
                {
                    this._ReceivedMessages = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private DateTime _LastMessageReceivedAt;

        public DateTime LastMessageReceivedAt
        {
            get { return _LastMessageReceivedAt; }
            set
            {
                if (value != this._LastMessageReceivedAt)
                {
                    this._LastMessageReceivedAt = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _BackColorStatus;

        public string BackColorStatus
        {
            get { return _BackColorStatus; }
            set
            {
                if (value != this._BackColorStatus)
                {
                    this._BackColorStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _SocketServerPort;

        public int SocketServerPort
        {
            get { return _SocketServerPort; }
            set
            {
                if (value != this._SocketServerPort)
                {
                    this._SocketServerPort = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _SocketServerClientsConnected;

        public int SocketServerClientsConnected
        {
            get { return _SocketServerClientsConnected; }
            set
            {
                if (value != this._SocketServerClientsConnected)
                {
                    this._SocketServerClientsConnected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _SocketServerIP;

        public string SocketServerIP
        {
            get { return _SocketServerIP; }
            set
            {
                if (value != this._SocketServerIP)
                {
                    this._SocketServerIP = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private List<iNectarClient> _Clientes;

        public List<iNectarClient> Clients
        {
            get { return _Clientes; }
            set 
            {
                if (value != this._Clientes)
                {
                    _Clientes = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ServerType NectarServerType { get; set; }
    }

    public enum QueueStatus
    {
        Stopped,
        Started
    }
}
