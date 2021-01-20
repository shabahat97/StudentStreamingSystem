using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StudentStreamingSystem.Connection
{
    public class Connections
    {

        public static SqlConnection My_SQL_Connection;
        public static SqlConnection GetConnection()
        {
            if (My_SQL_Connection == null)
            {
                My_SQL_Connection = new SqlConnection();
                My_SQL_Connection.ConnectionString = ConfigurationManager.ConnectionStrings["Streaming_Db"].ToString();
                My_SQL_Connection.Open();
            }
            return My_SQL_Connection;
        }

    }
}