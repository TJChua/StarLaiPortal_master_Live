using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB.Helpers;
using StarLaiPortal.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    public partial class WindowControllers : WindowController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public WindowControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            string ActionItemId = "MyDetails";
            string CompanyName = null;
            SqlConnection conn = new SqlConnection(getConnectionString());
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            string getcompany = "SELECT Description FROM [" + ConfigurationManager.AppSettings.Get("CommonTable").ToString() + "]..ODBC WHERE " +
                "DBName = '" + conn.Database + "'";
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            SqlCommand cmd = new SqlCommand(getcompany, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CompanyName = reader.GetString(0);
            }

            MyDetailsController myDetailsController = Frame.GetController<MyDetailsController>();
            if (myDetailsController != null)
            {
                if (user.Staff != null && user.Staff.StaffName != null)
                {
                    myDetailsController.Actions[ActionItemId].Caption = CompanyName + " (" + user.Staff.StaffName + ")";
                }
                else
                {
                    myDetailsController.Actions[ActionItemId].Caption = CompanyName + " (No Name)";
                }
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public string getConnectionString()
        {
            string connectionString = "";

            ConnectionStringParser helper = new ConnectionStringParser(Application.ConnectionString);
            helper.RemovePartByName("xpodatastorepool");
            connectionString = string.Format(helper.GetConnectionString());

            return connectionString;
        }
    }
}
