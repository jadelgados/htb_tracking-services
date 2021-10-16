using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    [Serializable]
    public class GPSDevice
    {
        #region Properties

        private int _GPSDeviceId;

        public int GPSDeviceId
        {
            get { return _GPSDeviceId; }
            set { _GPSDeviceId = value; }
        }

        private Int16 _DeviceTypeId;

        public Int16 DeviceTypeId
        {
            get { return _DeviceTypeId; }
            set { _DeviceTypeId = value; }
        }

        private string _IMEI;

        public string IMEI
        {
            get { return _IMEI; }
            set { _IMEI = value; }
        }

        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }

        private int _SocketId;

        public int SocketId
        {
            get { return _SocketId; }
            set { _SocketId = value; }
        }


        #endregion
    }
}
