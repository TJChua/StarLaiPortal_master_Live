using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SalesReturnControllers : ViewController
    {
        GeneralControllers genCon;
        public SalesReturnControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SubmitSR.Active.SetItemValue("Enabled", false);
            this.CancelSR.Active.SetItemValue("Enabled", false);
            this.PreviewSR.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesReturns_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSR.Active.SetItemValue("Enabled", true);
                    this.CancelSR.Active.SetItemValue("Enabled", true);
                    this.PreviewSR.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSR.Active.SetItemValue("Enabled", false);
                    this.CancelSR.Active.SetItemValue("Enabled", false);
                    this.PreviewSR.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SubmitSR.Active.SetItemValue("Enabled", false);
                this.CancelSR.Active.SetItemValue("Enabled", false);
                this.PreviewSR.Active.SetItemValue("Enabled", false);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public void openNewView(IObjectSpace os, object target, ViewEditMode viewmode)
        {
            ShowViewParameters svp = new ShowViewParameters();
            DetailView dv = Application.CreateDetailView(os, target);
            dv.ViewEditMode = viewmode;
            dv.IsRoot = true;
            svp.CreatedView = dv;

            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

        }
        public void showMsg(string caption, string msg, InformationType msgtype)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 3000;
            //options.Message = string.Format("{0} task(s) have been successfully updated!", e.SelectedObjects.Count);
            options.Message = string.Format("{0}", msg);
            options.Type = msgtype;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = caption;
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void SubmitSR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesReturns selectedObject = (SalesReturns)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            //if (selectedObject.IsValid2 == true)
            //{
            //    showMsg("Error", "Price cannot zero.", InformationType.Error);
            //    return;
            //}

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                SalesReturnDocTrail ds = ObjectSpace.CreateObject<SalesReturnDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.SalesReturnDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                foreach (SalesReturnDetails dtl in selectedObject.SalesReturnDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseSalesReturnReq(dtl.BaseDoc, "Close", ObjectSpace, selectedObject.Salesperson.SlpCode);
                        break;
                    }
                }

                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturns trx = os.FindObject<SalesReturns>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitSR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesReturns selectedObject = (SalesReturns)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesReturnDocTrail ds = ObjectSpace.CreateObject<SalesReturnDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesReturnDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            foreach (SalesReturnDetails dtl in selectedObject.SalesReturnDetails)
            {
                if (dtl.BaseDoc != null)
                {
                    genCon.CloseSalesReturnReq(dtl.BaseDoc, "Cancel", ObjectSpace, selectedObject.Salesperson.SlpCode);
                    break;
                }
            }

            IObjectSpace os = Application.CreateObjectSpace();
            SalesReturns trx = os.FindObject<SalesReturns>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewSR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            SalesReturns sreturn = (SalesReturns)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\SalesReturn.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", sreturn.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + sreturn.Oid + "_" + user.UserName + "_SReturn_"
                    + DateTime.Parse(sreturn.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + sreturn.Oid + "_" + user.UserName + "_SReturn_"
                    + DateTime.Parse(sreturn.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }
    }
}
