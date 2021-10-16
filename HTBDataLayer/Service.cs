using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TrackingCatalogLibrary.BusinessEntities;

namespace HTBDataLayer
{
    public class Service
    {
        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public Int64 SaveMessage(XElement messageBody)
        {
            object result = new DataLayer().ExecuteScalar(string.Format("spSaveMessage '{0}'", messageBody.ToString()));

            if (result == null)
            {
                return -1;
            }
            else if (result is Exception)
            {
                return Convert.ToInt64(0);
            }
            {
                return Convert.ToInt64(result);
            }
        }

        public GPSDevice GetDeviceByIMEI(string IMEI)
        {
            GPSDevice device = null;
            try
            {
                DataSet result = new DataLayer().ExecuteDataSet(string.Format("spGetDeviceInfoByIMEI '{0}'", IMEI));

                DataRow data = result.Tables[0].Rows[0];
                device = new GPSDevice();
                device.DeviceTypeId = Convert.ToInt16(data["DeviceTypeId"]);
                device.GPSDeviceId = Convert.ToInt32(data["GPSDeviceId"]);
                device.IMEI = string.Format("{0}", data["IMEI"]);
                device.PhoneNumber = string.Format("{0}", data["PhoneNumber"]);
            }
            catch (Exception)
            {
                device = null;
            }

            return device;
        }
        #endregion
    }
}
