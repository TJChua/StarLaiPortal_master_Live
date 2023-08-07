using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalIntegration
{
    class SAPCompany
    {
        public Company oCom;

        public string errMsg { get; set; }
        public string UserID { get; set; }

        public bool connectSAP()
        {
            if (oCom != null)
            {
                if (oCom.Connected) return true;
                else oCom = null;
            }
            oCom = new SAPbobsCOM.Company();
            string dbServerType = ConfigurationManager.AppSettings.Get("dbServerType");
            if (dbServerType == "MSSQL2005")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
            }
            else if (dbServerType == "MSSQL2008")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
            }
            else if (dbServerType == "MSSQL2012")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            }
            else if (dbServerType == "MSSQL2014")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
            }
            else if (dbServerType == "MSSQL2016")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
            }
            else if (dbServerType == "MSSQL2017")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
            }
            else if (dbServerType == "MSSQL2019")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            }
            else if (dbServerType == "HANADB")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            }
            else if (dbServerType == "DB_2")
            {
                oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_DB_2;
            }
            oCom.Server = ConfigurationManager.AppSettings.Get("Server");
            oCom.DbUserName = ConfigurationManager.AppSettings.Get("dbuser");
            oCom.DbPassword = ConfigurationManager.AppSettings.Get("dbpass");
            oCom.CompanyDB = ConfigurationManager.AppSettings.Get("CompanyDB");
            oCom.UserName = ConfigurationManager.AppSettings.Get("UserName");
            oCom.Password = ConfigurationManager.AppSettings.Get("Password");
            //oCom.LicenseServer = ConfigurationManager.AppSettings.Get("LicenseServer");
            oCom.language = SAPbobsCOM.BoSuppLangs.ln_English;

            if (oCom.Connect() != 0)
            {
                errMsg = oCom.GetLastErrorDescription();
                return false;
            }
            return true;
        }
    }
}
