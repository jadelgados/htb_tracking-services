using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrackingCatalogLibrary.BusinessEntities;

namespace AppSimulator.Models
{
    public class AppViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties
        
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string _NectarServerURL;

        public string NectarServerURL
        {
            get { return _NectarServerURL; }
            set 
            { 
                _NectarServerURL = value;
                NotifyPropertyChanged();
            }
        }

        private int _NectarServerPort;

        public int NectarServerPort
        {
            get { return _NectarServerPort; }
            set { _NectarServerPort = value; }
        }

        private int _VehicleId;

        public int VehicleId
        {
            get { return _VehicleId; }
            set 
            { 
                _VehicleId = value;
                NotifyPropertyChanged();
            }
        }

        private string _Log;

        public string Log
        {
            get { return _Log; }
            set 
            { 
                _Log = value;
                NotifyPropertyChanged();
            }
        }

        private List<GPSCommand> _CommandList;

        public List<GPSCommand> CommandList
        {
            get { return _CommandList; }
            set 
            { 
                _CommandList = value;
                NotifyPropertyChanged();
            }
        }

        private int _AppConnectionId;

        public int AppConnectionId
        {
            get { return _AppConnectionId; }
            set 
            { 
                _AppConnectionId = value;
                NotifyPropertyChanged();
            }
        }

        private GPSCommand _SelectedCommand;

        public GPSCommand SelectedCommand
        {
            get { return _SelectedCommand; }
            set 
            { 
                _SelectedCommand = value;
                NotifyPropertyChanged();
            }
        }


        private string _Parameters;

        public string Parameters
        {
            get { return _Parameters; }
            set
            {
                _Parameters = value;
                NotifyPropertyChanged();
            }
        }

        private List<GPSDevice> _Devices;

        public List<GPSDevice> Devices
        {
            get { return _Devices; }
            set 
            {
                if (_Devices!=value)
                {
                    _Devices = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #endregion

        #region Constructors

        public AppViewModel()
        {
            VehicleId = 1;
            CommandList = new List<GPSCommand>();
            CommandList.Add(new GPSCommand() { CommandId = 1, CommandName = "A10", Description= "Track on demand" });
            CommandList.Add(new GPSCommand() { CommandId = 3, CommandName = "A12", Description= "Track by Time Interval" });
            CommandList.Add(new GPSCommand() { CommandId = 5, CommandName = "A14", Description = "Track by Distance Interval" });
            CommandList.Add(new GPSCommand() { CommandId = 23, CommandName = "B07", Description = "Set Speeding Alarm" });
            CommandList.Add(new GPSCommand() { CommandId = 3, CommandName = "A12", Description = "Track by Time Interval" });
            CommandList.Add(new GPSCommand() { CommandId = 3, CommandName = "A12", Description = "Track by Time Interval" });
            CommandList.Add(new GPSCommand() { CommandId = 3, CommandName = "AFF", Description = "Delete GPRS Event in Queue Buffer" });
        }
        #endregion

    }
}
