using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingCatalogLibrary.BusinessEntities;

namespace AppSimulator.Models
{
    public class DataService
    {
        public VerifyGeofencePoint GetGeofenceInfo(string lat, string lgn)
        {
            VerifyGeofencePoint info = new VerifyGeofencePoint();
            try
            {
                var result = new HTBTrackerEntities().spVerifyGeofencePoint(lat, lgn).FirstOrDefault();
                if (result != null)
                    Tools.CopyObjectProperties(result, info);
                    
            }
            catch (Exception ex)
            {
                //NOT HANDLED
            }
            
            return info;
        }
    }
}
