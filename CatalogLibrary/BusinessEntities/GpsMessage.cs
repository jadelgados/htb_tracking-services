using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TrackingCatalogLibrary
{
    [Serializable]

    public class GpsMessage
    {
        #region Properties

        private Int64? _MessageId;

        public Int64? MessageId
        {
            get { return _MessageId; }
            set { _MessageId = value; }
        }
        

        [XmlIgnore]
        public bool IdSpecified
        {
            get
            {
                return _Id.HasValue;
            }
        }

        private int? _Id;

        public int? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _MessageHeader;

        public string MessageHeader
        {
            get { return _MessageHeader; }
            set { _MessageHeader = value; }
        }

        [XmlIgnore]
        public bool DateReceivedSpecified
        {
            get
            {
                return _DateReceived.HasValue;
            }
        }

        private DateTime? _DateReceived;

        public DateTime? DateReceived
        {
            get { return _DateReceived; }
            set { _DateReceived = value; }
        }
        //public DateTime DateReceived { get; set; }
        public string PackageFlag { get; set; }
        public int ContentLength { get; set; }
        public string IMEI { get; set; }
        public string CommandCode { get; set; }

        private string _EventCode;
        public string EventCode
        {
            get { return _EventCode; }
            set { _EventCode = value; }
        }

        [XmlIgnore]
        public bool DateMessageSpecified
        {
            get
            {
                return _DateMessage.HasValue;
            }
        }

        private DateTime? _DateMessage;

        public DateTime? DateMessage
        {
            get { return _DateMessage; }
            set { _DateMessage = value; }
        }


        private double _Latitude;

        public double Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }


        private double _Longitude;

        public double Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        private string _GPSStatus;

        public string GPSStatus
        {
            get { return _GPSStatus; }
            set { _GPSStatus = value; }
        }

        [XmlIgnore]
        public bool SatellitesSpecified
        {
            get
            {
                return _Satellites.HasValue;
            }
        }

        private int? _Satellites;

        public int? Satellites
        {
            get { return _Satellites; }
            set { _Satellites = value; }
        }

        [XmlIgnore]
        public bool GSMSignalSpecified
        {
            get
            {
                return _GSMSignal.HasValue;
            }
        }
        private int? _GSMSignal;

        public int? GSMSignal
        {
            get { return _GSMSignal; }
            set { _GSMSignal = value; }
        }

        [XmlIgnore]
        public bool SpeedSpecified
        {
            get
            {
                return _Speed.HasValue;
            }
        }

        private double? _Speed;

        public double? Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        [XmlIgnore]
        public bool HeadingSpecified
        {
            get
            {
                return _Heading.HasValue;
            }
        }

        private double? _Heading;

        public double? Heading
        {
            get { return _Heading; }
            set { _Heading = value; }
        }

        [XmlIgnore]
        public bool HDOPSpecified
        {
            get
            {
                return _HDOP.HasValue;
            }
        }

        private double? _HDOP;

        public double? HDOP
        {
            get { return _HDOP; }
            set { _HDOP = value; }
        }

        [XmlIgnore]
        public bool AltitudeSpecified
        {
            get
            {
                return _Altitude.HasValue;
            }
        }

        private double? _Altitude;

        public double? Altitude
        {
            get { return _Altitude; }
            set { _Altitude = value; }
        }

        [XmlIgnore]
        public bool MileageSpecified
        {
            get
            {
                return _Mileage.HasValue;
            }
        }

        private double? _Mileage;

        public double? Mileage
        {
            get { return _Mileage; }
            set { _Mileage = value; }
        }

        [XmlIgnore]
        public bool RuntimeSpecified
        {
            get
            {
                return _Runtime.HasValue;
            }
        }

        private double? _Runtime;

        public double? Runtime
        {
            get { return _Runtime; }
            set { _Runtime = value; }
        }

        private string _MessageBaseID;

        public string MessageBaseID
        {
            get { return _MessageBaseID; }
            set { _MessageBaseID = value; }
        }

        private string _InputsState;

        public string InputsState
        {
            get { return _InputsState; }
            set { _InputsState = value; }
        }

        private string _AD;

        public string AD
        {
            get { return _AD; }
            set { _AD = value; }
        }

        private string _RFID;

        public string RFID
        {
            get { return _RFID; }
            set { _RFID = value; }
        }

        [XmlIgnore]
        public bool FenceSpecified
        {
            get
            {
                return _Fence.HasValue;
            }
        }

        private int? _Fence;

        public int? Fence
        {
            get { return _Fence; }
            set { _Fence = value; }
        }

        private string _TemperatureSensor;

        public string TemperatureSensor
        {
            get { return _TemperatureSensor; }
            set { _TemperatureSensor = value; }
        }

        private string _TemperatureIndex;

        public string TemperatureIndex
        {
            get { return _TemperatureIndex; }
            set { _TemperatureIndex = value; }
        }

        private string _CustomizeData;

        public string CustomizeData
        {
            get { return _CustomizeData; }
            set { _CustomizeData = value; }
        }

        [XmlIgnore]
        public bool ProtocolVersionSpecified
        {
            get
            {
                return _ProtocolVersion.HasValue;
            }
        }

        private double? _ProtocolVersion;

        public double? ProtocolVersion
        {
            get { return _ProtocolVersion; }
            set { _ProtocolVersion = value; }
        }

        private string _FuelPercentage;

        public string FuelPercentage
        {
            get { return _FuelPercentage; }
            set { _FuelPercentage = value; }
        }

        private string _Checksum;

        public string Checksum
        {
            get { return _Checksum; }
            set { _Checksum = value; }
        }

        private string _AutPhoneNumbers;

        public string AutPhoneNumbers
        {
            get { return _AutPhoneNumbers; }
            set { _AutPhoneNumbers = value; }
        }

        private string _Data;

        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        private int _SocketId;

        public int SocketId
        {
            get { return _SocketId; }
            set { _SocketId = value; }
        }



        #endregion

        #region Methods

        public string GetMessage()
        {
            string msg = string.Format(",{0},{1}{2}*##==", 
                                this.IMEI,
                                this.CommandCode,
                                string.IsNullOrEmpty(this.Data) ? "" : string.Format(",{0}", this.Data));
            this.ContentLength = msg.Length;

            msg = string.Format("{0}{1}{2}{3}",
                                this.MessageHeader,
                                this.PackageFlag,
                                this.ContentLength,
                                msg.Substring(0, msg.IndexOf("*")+1));
            msg += GetCheksum(msg) + Environment.NewLine;
            return string.Format("{0}#{1}", SocketId, msg);
        }

        private string GetCheksum(string data)
        {
            string result=string.Empty;
            if (!string.IsNullOrEmpty(data))
            {
                int total = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    total += Convert.ToInt32(data[i]);
                }

                result = String.Format("{0:X}", total);
            }
            if (result.Length > 2) result = result.Substring(result.Length - 2);
            return result;
        }
        #endregion
    }
}
