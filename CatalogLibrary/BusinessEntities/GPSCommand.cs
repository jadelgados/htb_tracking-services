using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    [Serializable]
    public class GPSCommand
    {
        private int _CommandId;

        public int CommandId
        {
            get { return _CommandId; }
            set { _CommandId = value; }
        }

        private string _CommandName;

        public string CommandName
        {
            get { return _CommandName; }
            set { _CommandName = value; }
        }


        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

    }
}
