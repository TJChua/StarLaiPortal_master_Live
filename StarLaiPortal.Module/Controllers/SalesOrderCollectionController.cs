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
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection;
using StarLaiPortal.Module.BusinessObjects.View;
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
    public partial class SalesOrderCollectionController : ViewController
    {
        GeneralControllers genCon;
        public SalesOrderCollectionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SOCCopyFromSO.Active.SetItemValue("Enabled", false);
            this.SubmitSOC.Active.SetItemValue("Enabled", false);
            this.CancelSOC.Active.SetItemValue("Enabled", false);
            this.PrintARDownpayment.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesOrderCollection_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.SOCCopyFromSO.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SOCCopyFromSO.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSOC.Active.SetItemValue("Enabled", true);
                    this.CancelSOC.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSOC.Active.SetItemValue("Enabled", false);
                    this.CancelSOC.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "SalesOrderCollectionReport_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.PrintARDownpayment.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.PrintARDownpayment.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SOCCopyFromSO.Active.SetItemValue("Enabled", false);
                this.SubmitSOC.Active.SetItemValue("Enabled", false);
                this.CancelSOC.Active.SetItemValue("Enabled", false);
                this.PrintARDownpayment.Active.SetItemValue("Enabled", false);
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

        private void SOCCopyFromSO_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                try
                {
                    SalesOrderCollection soc = (SalesOrderCollection)View.CurrentObject;

                    //if (soc.IsNew == true)
                    //{
                    //    IObjectSpace os = Application.CreateObjectSpace();
                    //    SalesOrderCollection newsoc = os.CreateObject<SalesOrderCollection>();

                    //    foreach (vwOpenSO dtl in e.PopupWindowViewSelectedObjects)
                    //    {
                    //        newsoc.Customer = newsoc.Session.GetObjectByKey<vwBusniessPartner>(dtl.Customer);
                    //        newsoc.CustomerName = dtl.CustomerName;

                    //        SalesOrderCollectionDetails newsocitem = os.CreateObject<SalesOrderCollectionDetails>();

                    //        newsocitem.SalesOrder = dtl.DocNum;
                    //        newsocitem.OrderDate = dtl.OrderDate;
                    //        newsocitem.Total = dtl.DocTotal;

                    //        newsoc.SalesOrderCollectionDetails.Add(newsocitem);
                    //    }

                    //    ShowViewParameters svp = new ShowViewParameters();
                    //    DetailView dv = Application.CreateDetailView(os, newsoc);
                    //    dv.ViewEditMode = ViewEditMode.Edit;
                    //    dv.IsRoot = true;
                    //    svp.CreatedView = dv;

                    //    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    //    showMsg("Success", "Copy Success.", InformationType.Success);
                    //}
                    //else
                    //{
                        foreach (vwOpenSO dtl in e.PopupWindowViewSelectedObjects)
                        {
                            soc.CustomerName = dtl.CustomerName;
                            SalesOrderCollectionDetails newsocitem = ObjectSpace.CreateObject<SalesOrderCollectionDetails>();

                            newsocitem.SalesOrder = dtl.DocNum;
                            newsocitem.CustomerName = dtl.CustomerName;
                            newsocitem.GLAccount = newsocitem.Session.GetObjectByKey<vwBank>(soc.PaymentType.GLAccount);
                            newsocitem.OrderDate = dtl.OrderDate;
                            newsocitem.Total = dtl.DocTotal;
                            newsocitem.PaymentAmount = dtl.DocTotal;

                            soc.SalesOrderCollectionDetails.Add(newsocitem);

                            showMsg("Success", "Copy Success.", InformationType.Success);
                        }

                        if (soc.DocNum == null)
                        {
                            string docprefix = genCon.GetDocPrefix();
                            soc.DocNum = genCon.GenerateDocNum(DocTypeList.ARD, ObjectSpace, TransferType.NA, 0, docprefix);
                        }
                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    //}
                }
                catch (Exception)
                {
                    showMsg("Fail", "Copy Fail.", InformationType.Error);
                }
            }
        }

        private void SOCCopyFromSO_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            SalesOrderCollection soc = (SalesOrderCollection)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwOpenSO));
            var cs = Application.CreateCollectionSource(os, typeof(vwOpenSO), viewId);
            if (soc.Customer != null)
            {
                cs.Criteria["Customer"] = new BinaryOperator("Customer", soc.Customer.BPCode);
            }
            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void SubmitSOC_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesOrderCollection selectedObject = (SalesOrderCollection)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                SalesOrderCollectionDocStatus ds = ObjectSpace.CreateObject<SalesOrderCollectionDocStatus>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.SalesOrderCollectionDocStatus.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrderCollection trx = os.FindObject<SalesOrderCollection>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitSOC_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSOC_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesOrderCollection selectedObject = (SalesOrderCollection)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesOrderCollectionDocStatus ds = ObjectSpace.CreateObject<SalesOrderCollectionDocStatus>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesOrderCollectionDocStatus.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesOrderCollection trx = os.FindObject<SalesOrderCollection>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSOC_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PrintARDownpayment_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            SalesOrderCollectionReport print = (SalesOrderCollectionReport)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\SalesCollectionSummary.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("FromDate", print.FromDate.Date);
                doc.SetParameterValue("ToDate", print.ToDate.Date);
                if (user.Staff != null)
                {
                    if (user.Staff.StaffName != null)
                    {
                        doc.SetParameterValue("PrintByUser", user.Staff.StaffName);
                    }
                }

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + print.Oid + "_" + user.UserName + "_DPSummary_"
                    + DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + print.Oid + "_" + user.UserName + "_DPSummary_"
                    + DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd") + ".pdf";
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
