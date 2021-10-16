using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HTBDataLayer
{
    public class DataLayer
    {
        #region Properties

        private SqlConnection Connection;

        #endregion

        #region Constructors

        public DataLayer()
        {
            GetConnection();
        }
        #endregion

        #region Methods

        public object  ExecuteScalar(string commandText)
        {
            object result=null;
            try
            {
                using (SqlCommand cmd = new SqlCommand(commandText, this.Connection))
                {
                     result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                result = ex;
            }
            finally
            {
                this.Connection.Close();
            }
            return result;
        }

        public int ExecuteNonQuery(string commandText)
        {

            return ExecuteNonQuery(commandText, this.Connection);
        }

        public int ExecuteNonQuery(string commandText, SqlConnection conn)
        {
            if (this.Connection.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception connEx)
                {

                    throw new Exception("DataLayer.ExecuteNonQuery" + connEx.Message);
                }
            }
            SqlCommand cmd = new SqlCommand(commandText, conn);
            cmd.CommandTimeout = 60000;
            return cmd.ExecuteNonQuery();
        }

        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, "", this.Connection);
        }

        public DataSet ExecuteDataSet(string commandText, string tableName)
        {
            return ExecuteDataSet(commandText, tableName, this.Connection);
        }

        public DataSet ExecuteDataSet(string commandText, string tableName, SqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception connEx)
                {

                    throw new Exception("DataLayer.ExecuteDataSet" + connEx.Message);
                }
            }
            DataSet ds = new DataSet();
            try
            {
                if (commandText == "") return ds;
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.CommandTimeout = 6000;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
            catch (InvalidOperationException)
            {
                SqlConnection conn2 = new SqlConnection();
                conn2 = conn;
                ExecuteDataSet(commandText, tableName, conn2);
            }
            catch (Exception)
            {

            }

            return ds;
        }

        private void GetConnection()
        {
            string strConnection = ConfigurationManager.ConnectionStrings["HTBconnection"].ConnectionString;
            this.Connection = new SqlConnection(strConnection);
            this.Connection.Open();
        }



        #endregion



    }
}
