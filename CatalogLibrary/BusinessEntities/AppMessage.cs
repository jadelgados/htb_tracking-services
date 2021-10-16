using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    [Serializable]
    public class AppMessage
    {
        public AppMessageType MessageType { get; set; }
        public int AppConnectionId { get; set; }

        public GPSCommand Command { get; set; }
        public GpsMessage Message { get; set; }
        public int VehicleId { get; set; }
        public GPSDevice Device { get; set; }
    }
}
