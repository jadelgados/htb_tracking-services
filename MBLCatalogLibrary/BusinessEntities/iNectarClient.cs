using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBLCatalogLibrary.BusinessEntities
{
    public class iNectarClient
    {
        #region Properties

        private int _UserId;

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        private List<GPSDevice> _Devices;

        public List<GPSDevice> Devices
        {
            get { return _Devices; }
            set { _Devices = value; }
        }


        #endregion
    }
}
