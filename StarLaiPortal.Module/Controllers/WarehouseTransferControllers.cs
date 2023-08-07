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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
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
    public partial class WarehouseTransferControllers : ViewController
    {
        GeneralControllers genCon;
        public WarehouseTransferControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SubmitWT.Active.SetItemValue("Enabled", false);
            this.CancelWT.Active.SetItemValue("Enabled", false);
            this.PreviewWT.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "WarehouseTransfers_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitWT.Active.SetItemValue("Enabled", true);
                    this.CancelWT.Active.SetItemValue("Enabled", true);
                    this.PreviewWT.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitWT.Active.SetItemValue("Enabled", false);
                    this.CancelWT.Active.SetItemValue("Enabled", false);
                    this.PreviewWT.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SubmitWT.Active.SetItemValue("Enabled", false);
                this.CancelWT.Active.SetItemValue("Enabled", false);
                this.PreviewWT.Active.SetItemValue("Enabled", false);
            }

            if (View.Id == "WarehouseTransfers_WarehouseTransferDetails_ListView")
            {
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.RowUpdating += new DevExpress.Web.Data.ASPxDataUpdatingEventHandler(Grid_RowUpdating);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void Grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (listEditor != null)
            {
                object currentObject = listEditor.Grid.GetRow(listEditor.Grid.EditingRowVisibleIndex);
                if (currentObject != null)
                {
                    object validation = currentObject.GetType().GetProperty("IsValid").GetValue(currentObject);

                    if ((bool)validation == true)
                    {
                        showMsg("Error", "Bin not enough stock.", InformationType.Error);
                    }

                    object validation1 = currentObject.GetType().GetProperty("IsValid1").GetValue(currentObject);

                    if ((bool)validation1 == true)
                    {
                        showMsg("Error", "Cannot transfer within same bin.", InformationType.Error);
                    }
                }
            }
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

        private void SubmitWT_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            WarehouseTransfers selectedObject = (WarehouseTransfers)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            if (selectedObject.IsValid2 == true)
            {
                showMsg("Error", "Bin not enough stock.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid3 == true)
            {
                showMsg("Error", "Cannot transfer within same bin.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid4 == true)
            {
                showMsg("Error", "Cannot transfer without bin.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                WarehouseTransfersDocTrail ds = ObjectSpace.CreateObject<WarehouseTransfersDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.WarehouseTransfersDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                foreach (WarehouseTransferDetails dtl in selectedObject.WarehouseTransferDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseWarehouseTransferReq(dtl.BaseDoc, "Close", ObjectSpace);
                        break;
                    }
                }

                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransfers trx = os.FindObject<WarehouseTransfers>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitWT_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelWT_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            WarehouseTransfers selectedObject = (WarehouseTransfers)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            WarehouseTransfersDocTrail ds = ObjectSpace.CreateObject<WarehouseTransfersDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.WarehouseTransfersDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            foreach (WarehouseTransferDetails dtl in selectedObject.WarehouseTransferDetails)
            {
                if (dtl.BaseDoc != null)
                {
                    genCon.CloseWarehouseTransferReq(dtl.BaseDoc, "Cancel", ObjectSpace);
                    break;
                }
            }

            IObjectSpace os = Application.CreateObjectSpace();
            WarehouseTransfers trx = os.FindObject<WarehouseTransfers>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelWT_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewWT_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            WarehouseTransfers wt = (WarehouseTransfers)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\WarehouseTransfer.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", wt.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + wt.Oid + "_" + user.UserName + "_WT_"
                    + DateTime.Parse(wt.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + wt.Oid + "_" + user.UserName + "_WT_"
                    + DateTime.Parse(wt.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
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
