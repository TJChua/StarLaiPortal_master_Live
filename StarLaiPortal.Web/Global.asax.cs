using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Web.Routing;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;

namespace StarLaiPortal.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            RouteTable.Routes.RegisterXafRoutes();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new StarLaiPortalAspNetApplication());
            SecurityStrategy security = WebApplication.Instance.GetSecurityStrategy();
            security.RegisterXPOAdapterProviders();
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.Settings.DefaultVerticalTemplateContentPath = "DefaultVerticalTemplateContent1.ascx";
            DefaultVerticalTemplateContent1.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();

            #region GeneralSettings
            string temp = "";

            temp = ConfigurationManager.AppSettings["EmailSend"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailSend = false;
            if (temp.ToUpper() == "Y" || temp.ToUpper() == "YES" || temp.ToUpper() == "TRUE" || temp == "1")
                StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailSend = true;

            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailHost = ConfigurationManager.AppSettings["EmailHost"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailHostDomain = ConfigurationManager.AppSettings["EmailHostDomain"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailPort = ConfigurationManager.AppSettings["EmailPort"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.Email = ConfigurationManager.AppSettings["Email"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailPassword = ConfigurationManager.AppSettings["EmailPassword"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailName = ConfigurationManager.AppSettings["EmailName"].ToString();

            temp = ConfigurationManager.AppSettings["EmailSSL"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailSSL = false;
            if (temp.ToUpper() == "Y" || temp.ToUpper() == "YES" || temp.ToUpper() == "TRUE" || temp == "1")
                StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailSSL = true;

            temp = ConfigurationManager.AppSettings["EmailUseDefaultCredential"].ToString();
            StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailUseDefaultCredential = false;
            if (temp.ToUpper() == "Y" || temp.ToUpper() == "YES" || temp.ToUpper() == "TRUE" || temp == "1")
                StarLaiPortal.Module.BusinessObjects.GeneralSettings.EmailUseDefaultCredential = true;

            StarLaiPortal.Module.BusinessObjects.GeneralSettings.DeliveryMethod = ConfigurationManager.AppSettings["DeliveryMethod"].ToString();

            StarLaiPortal.Module.BusinessObjects.GeneralSettings.appurl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri; // + requestManager.GetQueryString(shortcut)

            #endregion
            WebApplication.Instance.CustomizeFormattingCulture += Instance_CustomizeFormattingCulture;

            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        private void Instance_CustomizeFormattingCulture(object sender, CustomizeFormattingCultureEventArgs e)
        {
            e.FormattingCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
