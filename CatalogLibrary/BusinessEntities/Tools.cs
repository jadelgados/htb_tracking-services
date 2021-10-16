using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TrackingCatalogLibrary.BusinessEntities
{
    public class Tools
    {
        #region Constants

        public const string _FLAGS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string MESSAGE_HEADER = "@@";

        #endregion

        #region Properties

        string DirectoryName;
        string FilePrefix;

        #endregion

        #region Constructors

        public Tools()
        {
            this.DirectoryName = ConfigurationManager.AppSettings["LogDirectory"];
            this.FilePrefix = ConfigurationManager.AppSettings["LogFilePrefix"];
            //Me.DirectoryName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\RTBISTelephony_Log"
            if (string.IsNullOrEmpty(this.DirectoryName)) this.DirectoryName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\iNectar2.0Log";
        }

        #endregion

        #region Methods

        public byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public object ByteArrayToObject(byte[] array)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(array, 0, array.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GenerateMessage(GpsMessage msg)
        {
            Random rnd = new Random();

            msg.MessageHeader = MESSAGE_HEADER;
            msg.PackageFlag = _FLAGS[rnd.Next(_FLAGS.Length - 1)].ToString();
            return msg.GetMessage();
        }

        public void WriteToLog(string msg)
        {
            try
            {
                if (!System.IO.Directory.Exists(this.DirectoryName))
                    System.IO.Directory.CreateDirectory(this.DirectoryName);
                dynamic _message = string.Format("{0} - {1}", System.DateTime.Now, msg);
                using (StreamWriter w = new StreamWriter(this.DirectoryName + string.Format("\\{0}_{1:yyyyMMdd_HH}.log", this.FilePrefix, DateTime.Now), true))
                {
                    w.WriteLine(_message);
                    w.Close();
                    w.Flush();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void CopyObjectProperties(object scr, object dst)
        {
            PropertyInfo[] properties = scr.GetType().GetProperties();

            foreach (PropertyInfo info in properties.ToList())
            {
                try
                {
                    object value = info.GetValue(scr, null);
                    dst.GetType().GetProperty(info.Name).SetValue(dst, value, null);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        #endregion
    }
}
