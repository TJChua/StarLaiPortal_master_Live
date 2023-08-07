using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects
{
    public interface IDatabaseNameParameter
    {
        string DatabaseName { get; set; }
    }
    [DomainComponent]
    [XafDisplayName("Operation Portal Login")]

    public class CustomLogonParametersForStandardAuthentication : AuthenticationStandardLogonParameters, IDatabaseNameParameter
    {
        private string databaseName;
        [XafDisplayName("Company")]
        [EditorAlias("CustomLogin")]
        [ModelDefault("PredefinedValues", MSSqlServerChangeDatabaseHelper.Databases)]
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
    }

    [DomainComponent]
    public class CustomLogonParametersForActiveDirectoryAuthentication : IDatabaseNameParameter
    {
        private string databaseName;
        [XafDisplayName("Company")]
        [EditorAlias("CustomLogin")]
        [ModelDefault("PredefinedValues", MSSqlServerChangeDatabaseHelper.Databases)]
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
    }

    public class MSSqlServerChangeDatabaseHelper
    {
        public const string Databases = "";
        public static void UpdateDatabaseName(XafApplication application, string databaseName)
        {
            DataTable dt = null;
            string sqlQuery = "SELECT * FROM ODBC WHERE DBName = '" + databaseName + "'";
            dt = SQLHandler.GetDataTable(sqlQuery);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    application.ConnectionString = row["ConnString"].ToString();
                }
            }
        }
    }

    public class SQLHandler
    {
        static public DataTable GetDataTable(string command)
        {
            /*
             * SELECT data from 3rd party SQL Server Database to SAP database
             * 
             * */
            DataTable myDataTable = new DataTable();
            if (command == "") return myDataTable;

            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                SqlCommand myCommand = new SqlCommand(command, myConnection)
                {
                    CommandTimeout = 90000
                };
                using (SqlDataAdapter adapter = new SqlDataAdapter(myCommand))
                {
                    adapter.Fill(myDataTable);
                }

                try
                {
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return myDataTable;
        }
    }
}