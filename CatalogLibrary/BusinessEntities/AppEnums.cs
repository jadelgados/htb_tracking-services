using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    public enum AppMessageType
    {
        None,
        SocketClientConnected,
        SocketClientDisonnected,
        MessageSent,
        MessageReceived,
        SetConfig
    }
}
