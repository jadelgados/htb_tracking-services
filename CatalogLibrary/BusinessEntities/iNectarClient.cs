using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    public class iNectarClient
    {
        #region Properties

        private int _AppConnectionId;

        public int AppConnectionId
        {
            get { return _AppConnectionId; }
            set { _AppConnectionId = value; }
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
