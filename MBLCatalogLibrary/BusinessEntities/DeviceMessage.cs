using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBLCatalogLibrary.BusinessEntities
{
    [Serializable]
    public class DeviceMessage
    {
        public DateTime MessageReceivedOn { get; set; }
        public double MessageId { get; set; }
        public string MessageBase { get; set; }

        public DeviceMessage()
        {
            this.MessageReceivedOn = DateTime.Now;
        }

        public DeviceMessage(string messageSource)
        {
            this.MessageReceivedOn = DateTime.Now;
            this.MessageBase = messageSource;
        }
    }
}
