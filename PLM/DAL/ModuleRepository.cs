using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace PLM
{
    public class ModuleRepository : IDisposable
    {
        private Module module;

        public ModuleRepository()
        {
            module = new Module();
            module.Pictures = new List<Picture>();
        }

        private DataSet GetDataSet()
        {
            DataSet module_ds = new DataSet();

            string connString = GetConnectionString();
            //TODO - * will be specific module?
            string sqlCommandString = "SELECT * from Modules";

            SqlConnection sqlConn = new SqlConnection(connString);
            SqlCommand sqlCommand = new SqlCommand(sqlCommandString, sqlConn);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);

            using (sqlConn)
            {
                using (sqlCommand)
                {
                    try
                    {
                        sqlConn.Open();
                        //TODO - Change Modules
                        sqlAdapter.Fill(module_ds, "Modules");
                    }
                    catch (SqlException sqlEx)
                    {
                        Console.WriteLine("SQL Exception: {0}", sqlEx.Message);
                    }
                }
            }
            return module_ds;
        }

        public void Dispose()
        {

        }

        private static string GetConnectionString()
        {
            string returnValue = null;

            var settings = ConfigurationManager.ConnectionStrings["PLMConnection"];

            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}