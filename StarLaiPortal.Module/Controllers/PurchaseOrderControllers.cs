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
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PurchaseOrderControllers : ViewController
    {
        GeneralControllers genCon;
        public PurchaseOrderControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SubmitPO.Active.SetItemValue("Enabled", false);
            this.CancelPO.Active.SetItemValue("Enabled", false);
            this.PreviewPO.Active.SetItemValue("Enabled", false);
            this.POInquiryItem.Active.SetItemValue("Enabled", false);
            this.DuplicatePO.Active.SetItemValue("Enabled", false);
            this.ReviewAppPO.Active.SetItemValue("Enabled", false);
            this.ApproveAppPO.Active.SetItemValue("Enabled", false);
            this.RejectAppPO.Active.SetItemValue("Enabled", false);
            this.POCopyFromSO.Active.SetItemValue("Enabled", false);
            this.ApproveAppPO_Pop.Active.SetItemValue("Enabled", false);
            this.PreviewPONoCost.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "PurchaseOrders_DetailView")
            {
                //this.BackToInquiry.Active.SetItemValue("Enabled", true);
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitPO.Active.SetItemValue("Enabled", true);
                    this.CancelPO.Active.SetItemValue("Enabled", true);
                    this.PreviewPO.Active.SetItemValue("Enabled", true);
                    this.DuplicatePO.Active.SetItemValue("Enabled", true);
                    this.PreviewPONoCost.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitPO.Active.SetItemValue("Enabled", false);
                    this.CancelPO.Active.SetItemValue("Enabled", false);
                    this.PreviewPO.Active.SetItemValue("Enabled", false);
                    this.DuplicatePO.Active.SetItemValue("Enabled", false);
                    this.PreviewPONoCost.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.POCopyFromSO.Active.SetItemValue("Enabled", true);
                    this.POInquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.POCopyFromSO.Active.SetItemValue("Enabled", false);
                    this.POInquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "PurchaseOrders_ListView_Approval")
            {
                this.ReviewAppPO.Active.SetItemValue("Enabled", true);
                this.ReviewAppPO.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppPO.Active.SetItemValue("Enabled", true);
                //this.ApproveAppPO.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppPO.Active.SetItemValue("Enabled", true);
                //this.RejectAppPO.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppPO_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "PurchaseOrders_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppPO_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppPO_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SubmitPO.Active.SetItemValue("Enabled", false);
                this.CancelPO.Active.SetItemValue("Enabled", false);
                this.PreviewPO.Active.SetItemValue("Enabled", false);
                this.POInquiryItem.Active.SetItemValue("Enabled", false);
                this.DuplicatePO.Active.SetItemValue("Enabled", false);
                this.ReviewAppPO.Active.SetItemValue("Enabled", false);
                this.ApproveAppPO.Active.SetItemValue("Enabled", false);
                this.RejectAppPO.Active.SetItemValue("Enabled", false);
                this.POCopyFromSO.Active.SetItemValue("Enabled", false);
                this.ApproveAppPO_Pop.Active.SetItemValue("Enabled", false);
                this.PreviewPONoCost.Active.SetItemValue("Enabled", false);
            }

            if (View.Id == "PurchaseOrders_PurchaseOrderDetails_ListView")
            {
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.RowUpdating += new DevExpress.Web.Data.ASPxDataUpdatingEventHandler(Grid_RowUpdating);
            }
        }

        private void Grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (listEditor != null)
            {
                object currentObject = listEditor.Grid.GetRow(listEditor.Grid.EditingRowVisibleIndex);
                if (currentObject != null)
                {
                    object validation = currentObject.GetType().GetProperty("IsValid1").GetValue(currentObject);

                    if ((bool)validation == true)
                    {
                        showMsg("Error", "Back to Back Sales not allow to change quantity.", InformationType.Error);
                    }
                }
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

        private void SubmitPO_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            bool sellingprice = false;
            bool zerototal = false;
            string sellingitem = null;
            PurchaseOrders selectedObject = (PurchaseOrders)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.PurchaseOrderDetails.Sum(s => s.Total) <= 0)
            {
                zerototal = true;
            }

            foreach (PurchaseOrderDetails dtl in selectedObject.PurchaseOrderDetails)
            {
                if (dtl.AdjustedPrice > dtl.SellingPrice && dtl.BaseDoc != null)
                {
                    if (dtl.Series == "BackOrdP" || dtl.Series == "BackOrdS")
                    {
                        sellingprice = true;
                        if (sellingitem == null)
                        {
                            sellingitem = dtl.ItemCode.ItemCode;
                        }
                        else
                        {
                            sellingitem = sellingitem + ", " + dtl.ItemCode.ItemCode;
                        }
                    }
                }
            }

            if (sellingprice == false)
            {
                if (selectedObject.IsValid == false)
                {
                    if (selectedObject.IsValid1 == true)
                    {
                        selectedObject.Status = DocStatus.Submitted;

                        PurchaseOrderDocTrail ds = ObjectSpace.CreateObject<PurchaseOrderDocTrail>();
                        ds.DocStatus = DocStatus.Submitted;
                        ds.DocRemarks = p.ParamString;
                        selectedObject.PurchaseOrderDocTrail.Add(ds);

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();

                        #region Get approval
                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'PurchaseOrders'";
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(getapproval, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetString(1) != "")
                            {
                                emailbody = "Dear Sir/Madam, " + System.Environment.NewLine + System.Environment.NewLine +
                                           reader.GetString(3) + System.Environment.NewLine + GeneralSettings.appurl + reader.GetString(2) +
                                           System.Environment.NewLine + System.Environment.NewLine;

                                emailsubject = "Purchase Order Approval";
                                emailaddress = reader.GetString(1);
                                emailuser = reader.GetGuid(0);

                                ToEmails.Add(emailaddress);
                            }
                        }
                        conn.Close();

                        if (ToEmails.Count > 0)
                        {
                            if (genCon.SendEmail(emailsubject, emailbody, ToEmails) == 1)
                            {
                            }
                        }

                        #endregion

                        IObjectSpace os = Application.CreateObjectSpace();
                        PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("Oid", selectedObject.Oid));

                        if (trx.AppStatus == ApprovalStatusType.Not_Applicable && trx.Status == DocStatus.Submitted)
                        {
                            trx.Status = DocStatus.PendPost;
                            os.CommitChanges();
                            os.Refresh();
                        }

                        IObjectSpace pos = Application.CreateObjectSpace();
                        PurchaseOrders ptrx = pos.FindObject<PurchaseOrders>(new BinaryOperator("Oid", selectedObject.Oid));
                        openNewView(pos, ptrx, ViewEditMode.View);
                        if (sellingprice == false && zerototal == false)
                        {
                            showMsg("Successful", "Submit Done.", InformationType.Success);
                        }
                        else
                        {
                            //    if (sellingprice == true && zerototal == false)
                            //    {
                            //        showMsg("Warning", "Submit Done. Item: " + sellingitem + " adjusted price higher than selling price.", InformationType.Warning);
                            //    }

                            //    if (sellingprice == false && zerototal == true)
                            //    {
                            //        showMsg("Warning", "Submit Done. Document with 0 amount.", InformationType.Warning);
                            //    }

                            //    if (sellingprice == true && zerototal == true)
                            //    {
                            //        showMsg("Warning", "Submit Done. Item: " + sellingitem + " adjusted price higher than selling price."
                            //            + System.Environment.NewLine + System.Environment.NewLine +
                            //            "Document with 0 amount.", InformationType.Warning);
                            //    }
                            //}

                            if (zerototal == false)
                            {
                                showMsg("Successful", "Submit Done.", InformationType.Success);
                            }
                            else
                            {
                                showMsg("Warning", "Submit Done. Document with 0 amount.", InformationType.Warning);
                            }
                        }
                    }
                    else
                    {
                        showMsg("Error", "No Content.", InformationType.Error);
                    }
                }
                else
                {
                    showMsg("Error", "Multiple warehouse in same document.", InformationType.Error);
                }
            }
            else
            {
                showMsg("Error", "Item: " + sellingitem + " adjusted price higher than selling price.", InformationType.Error);
            }
        }

        private void SubmitPO_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelPO_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PurchaseOrders selectedObject = (PurchaseOrders)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            PurchaseOrderDocTrail ds = ObjectSpace.CreateObject<PurchaseOrderDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.PurchaseOrderDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelPO_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewPO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            PurchaseOrders po = (PurchaseOrders)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\PurchaseOrder.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", po.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_PO_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_PO_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);

                po.PrintStatus = PrintStatus.Printed;
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }

        private void POInquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void POInquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            PurchaseOrders trx = (PurchaseOrders)View.CurrentObject;
            string docprefix = genCon.GetDocPrefix();

            if (trx.DocNum == null)
            {
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.PO, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            PurchaseOrders po = os.FindObject<PurchaseOrders>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = po.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.PO;
            ((ItemInquiry)dv.CurrentObject).CardCode = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwBusniessPartner>
                (trx.Supplier.BPCode);

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
                DocTypeList.PO, "True"));

            if (defaultdata != null)
            {
                if (defaultdata.PriceList1 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).PriceList1 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwPriceList>
                        (defaultdata.PriceList1.ListNum);
                }
                if (defaultdata.PriceList2 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).PriceList2 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwPriceList>
                        (defaultdata.PriceList2.ListNum);
                }
                if (defaultdata.PriceList3 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).PriceList3 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwPriceList>
                        (defaultdata.PriceList3.ListNum);
                }
                if (defaultdata.PriceList4 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).PriceList4 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwPriceList>
                        (defaultdata.PriceList4.ListNum);
                }
                if (defaultdata.Stock1 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).Stock1 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwWarehouse>
                        (defaultdata.Stock1.WarehouseCode);
                }
                if (defaultdata.Stock2 != null)
                {
                    ((ItemInquiry)dv.CurrentObject).Stock2 = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwWarehouse>
                        (defaultdata.Stock2.WarehouseCode);
                }
            }

            inqos.CommitChanges();
            inqos.Refresh();

            e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            e.DialogController.AcceptAction.ActionMeaning = ActionMeaning.Unknown;
            e.Maximized = true;

            e.View = dv;
        }

        private void DuplicatePO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count == 1)
            {
                try
                {
                    PurchaseOrders po = (PurchaseOrders)View.CurrentObject;
                    IObjectSpace os = Application.CreateObjectSpace();
                    PurchaseOrders newpo = os.CreateObject<PurchaseOrders>();

                    if (po.Supplier != null)
                    {
                        newpo.Supplier = newpo.Session.GetObjectByKey<vwBusniessPartner>(po.Supplier.BPCode);
                    }
                    newpo.ContactNo = po.ContactNo;
                    if (po.PaymentTerm != null)
                    {
                        newpo.PaymentTerm = newpo.Session.GetObjectByKey<vwPaymentTerm>(po.PaymentTerm.GroupNum);
                    }
                    if (po.BillingAddress != null)
                    {
                        newpo.BillingAddress = newpo.Session.GetObjectByKey<vwBillingAddress>(po.BillingAddress.PriKey);
                    }
                    newpo.BillingAddressfield = po.BillingAddressfield;
                    if (po.ShippingAddress != null)
                    {
                        newpo.ShippingAddress = newpo.Session.GetObjectByKey<vwShippingAddress>(po.ShippingAddress.PriKey);
                    }
                    newpo.ShippingAddressfield = po.ShippingAddressfield;
                    if (po.Series != null)
                    {
                        newpo.Series = newpo.Session.GetObjectByKey<vwSeries>(po.Series.Series);
                    }
                    if (po.Warehouse != null)
                    {
                        newpo.Warehouse = newpo.Session.GetObjectByKey<vwWarehouse>(po.Warehouse.WarehouseCode);
                    }
                    newpo.Remarks = po.Remarks;

                    foreach (PurchaseOrderDetails dtl in po.PurchaseOrderDetails)
                    {
                        PurchaseOrderDetails newpodetails = os.CreateObject<PurchaseOrderDetails>();

                        newpodetails.ItemCode = newpodetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                        newpodetails.ItemDesc = dtl.ItemDesc;
                        newpodetails.Model = dtl.Model;
                        newpodetails.CatalogNo = dtl.CatalogNo;
                        if (dtl.Location != null)
                        {
                            newpodetails.Location = newpodetails.Session.GetObjectByKey<vwWarehouse>(dtl.Location.WarehouseCode);
                        }
                        newpodetails.Quantity = dtl.Quantity;
                        newpodetails.Price = dtl.Price;
                        newpodetails.AdjustedPrice = dtl.AdjustedPrice;
                        newpo.PurchaseOrderDetails.Add(newpodetails);
                    }

                    ShowViewParameters svp = new ShowViewParameters();
                    DetailView dv = Application.CreateDetailView(os, newpo);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;

                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    showMsg("Success", "Duplicate Success.", InformationType.Success);
                }
                catch (Exception)
                {
                    showMsg("Fail", "Duplicate Fail.", InformationType.Error);
                }
            }
            else if (e.SelectedObjects.Count > 1)
            {
                showMsg("Fail", "Duplicate Fail, Selected item more than 1.", InformationType.Error);
            }
            else
            {
                showMsg("Fail", "Duplicate Fail, No selected item.", InformationType.Error);
            }
        }

        private void ReviewAppPO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            PurchaseOrders po = (PurchaseOrders)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\PurchaseOrderApprReview.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", po.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_POReview_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_POReview_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }

        private void ApproveAppPO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PurchaseOrders selectedObject = (PurchaseOrders)e.CurrentObject;

            selectedObject.Status = DocStatus.PendPost;
            selectedObject.AppStatus = ApprovalStatusType.Approved;

            PurchaseOrderDocTrail ds = ObjectSpace.CreateObject<PurchaseOrderDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.PurchaseOrderDocTrail.Add(ds);

            PurchaseOrderAppStatus apps = ObjectSpace.CreateObject<PurchaseOrderAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.PurchaseOrderAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppPO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PurchaseOrders selectedObject = (PurchaseOrders)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            PurchaseOrderDocTrail ds = ObjectSpace.CreateObject<PurchaseOrderDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.PurchaseOrderDocTrail.Add(ds);

            PurchaseOrderAppStatus apps = ObjectSpace.CreateObject<PurchaseOrderAppStatus>();
            apps.AppStatus = ApprovalStatusType.Rejected;
            apps.AppRemarks = "Rejected";
            selectedObject.PurchaseOrderAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void POCopyFromSO_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                try
                {
                    PurchaseOrders po = (PurchaseOrders)View.CurrentObject;

                    foreach (vwPOSO dtl in e.PopupWindowViewSelectedObjects)
                    {
                        vwPOSO so = ObjectSpace.FindObject<vwPOSO>(CriteriaOperator.Parse("Oid = ? and DocNum = ?",
                            dtl.Oid, dtl.DocNum));

                        if (so == null)
                        {
                            showMsg("Error", "SO already created purchase order, please refresh data.", InformationType.Error);
                            return;
                        }

                        PurchaseOrderDetails newpoitem = ObjectSpace.CreateObject<PurchaseOrderDetails>();

                        if (po.Supplier != null)
                        {
                            newpoitem.Supplier = newpoitem.Session.GetObjectByKey<vwBusniessPartner>(po.Supplier.BPCode);
                        }
                        newpoitem.Postingdate = po.PostingDate;
                        newpoitem.ItemCode = newpoitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                        newpoitem.ItemDesc = dtl.ItemDesc;
                        newpoitem.Quantity = dtl.Quantity;
                        if (dtl.Warehouse != null)
                        {
                            newpoitem.Location = newpoitem.Session.GetObjectByKey<vwWarehouse>(dtl.Warehouse);
                        }
                        newpoitem.SellingPrice = dtl.SellingPrice;
                        newpoitem.BaseDoc = dtl.DocNum;
                        newpoitem.BaseId = dtl.Oid;
                        newpoitem.Series = dtl.Series.SeriesName;

                        po.PurchaseOrderDetails.Add(newpoitem);

                        showMsg("Success", "Copy Success.", InformationType.Success);
                    }

                    if (po.DocNum == null)
                    {
                        string docprefix = genCon.GetDocPrefix();
                        po.DocNum = genCon.GenerateDocNum(DocTypeList.PO, ObjectSpace, TransferType.NA, 0, docprefix);
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

        private void POCopyFromSO_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            PurchaseOrders poc = (PurchaseOrders)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwPOSO));
            var cs = Application.CreateCollectionSource(os, typeof(vwPOSO), viewId);
            //if (poc.Supplier != null)
            //{
            //    cs.Criteria["Customer"] = new BinaryOperator("Customer", poc.Supplier.BPCode);
            //}
            if (poc.Warehouse != null)
            {
                cs.Criteria["Warehouse"] = new BinaryOperator("Warehouse", poc.Warehouse.WarehouseCode);
            }
            if (poc.Series != null)
            {
                cs.Criteria["Series.SeriesName"] = new BinaryOperator("Series.SeriesName", poc.Series.SeriesName);
            }
            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void ApproveAppPO_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count > 1)
            {
                int totaldoc = 0;

                SqlConnection conn = new SqlConnection(genCon.getConnectionString());
                ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
                ApprovalParameters p = (ApprovalParameters)e.PopupWindow.View.CurrentObject;

                if (p.IsValid == false)
                {
                    try
                    {
                        foreach (PurchaseOrders dtl in e.SelectedObjects)
                        {
                            IObjectSpace pos = Application.CreateObjectSpace();
                            PurchaseOrders po = pos.FindObject<PurchaseOrders>(new BinaryOperator("Oid", dtl.Oid));

                            if (po.Status == DocStatus.Submitted && po.AppStatus == ApprovalStatusType.Required_Approval)
                            {
                                ApprovalStatusType appstatus = ApprovalStatusType.Required_Approval;

                                if (appstatus == ApprovalStatusType.Not_Applicable)
                                    appstatus = ApprovalStatusType.Required_Approval;

                                if (p.IsErr) return;
                                if (appstatus == ApprovalStatusType.Required_Approval && p.AppStatus == ApprovalActions.NA)
                                {
                                    showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                                    return;
                                }
                                else if (appstatus == ApprovalStatusType.Approved && p.AppStatus == ApprovalActions.Yes)
                                {
                                    showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                                    return;
                                }
                                else if (appstatus == ApprovalStatusType.Rejected && p.AppStatus == ApprovalActions.No)
                                {
                                    showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                                    return;
                                }
                                if (p.AppStatus == ApprovalActions.NA)
                                {
                                    appstatus = ApprovalStatusType.Required_Approval;
                                }
                                if (p.AppStatus == ApprovalActions.Yes)
                                {
                                    appstatus = ApprovalStatusType.Approved;
                                }
                                if (p.AppStatus == ApprovalActions.No)
                                {
                                    appstatus = ApprovalStatusType.Rejected;
                                }

                                PurchaseOrderAppStatus ds = pos.CreateObject<PurchaseOrderAppStatus>();
                                ds.PurchaseOrders = pos.GetObjectByKey<PurchaseOrders>(po.Oid);
                                ds.AppStatus = appstatus;
                                if (appstatus == ApprovalStatusType.Rejected)
                                {
                                    //sq.Status = DocStatus.New;
                                    ds.AppRemarks =
                                        System.Environment.NewLine + "(Reject User: " + user.Staff.StaffName + ")" +
                                        System.Environment.NewLine + "(Reason: Approval Rejected) - " + p.ParamString;
                                    ds.CreateUser = pos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                }
                                else
                                {
                                    ds.AppRemarks = System.Environment.NewLine + "(Approved User: " + user.Staff.StaffName + ") - " + p.ParamString;
                                }
                                po.PurchaseOrderAppStatus.Add(ds);

                                pos.CommitChanges();
                                pos.Refresh();

                                totaldoc++;

                                #region approval

                                List<string> ToEmails = new List<string>();
                                string emailbody = "";
                                string emailsubject = "";
                                string emailaddress = "";
                                Guid emailuser;
                                DateTime emailtime = DateTime.Now;

                                string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + po.Oid + "', 'PurchaseOrders', " + ((int)appstatus);
                                if (conn.State == ConnectionState.Open)
                                {
                                    conn.Close();
                                }
                                conn.Open();
                                SqlCommand cmd = new SqlCommand(getapproval, conn);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    if (reader.GetString(1) != "")
                                    {
                                        emailbody = "Dear Sir/Madam, " + System.Environment.NewLine + System.Environment.NewLine +
                                                    reader.GetString(3) + System.Environment.NewLine + GeneralSettings.appurl + reader.GetString(2) +
                                                    System.Environment.NewLine + System.Environment.NewLine;

                                        if (appstatus == ApprovalStatusType.Approved)
                                            emailsubject = "Purchase Order Approval";
                                        else if (appstatus == ApprovalStatusType.Rejected)
                                            emailsubject = "Purchase Order Approval Rejected";

                                        emailaddress = reader.GetString(1);
                                        emailuser = reader.GetGuid(0);

                                        ToEmails.Add(emailaddress);
                                    }
                                }
                                conn.Close();

                                if (ToEmails.Count > 0)
                                {
                                    if (genCon.SendEmail(emailsubject, emailbody, ToEmails) == 1)
                                    {
                                    }
                                }
                                #endregion

                                //ObjectSpace.CommitChanges(); //This line persists created object(s).
                                //ObjectSpace.Refresh();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        showMsg("Error", ex.Message, InformationType.Error);
                    }

                    showMsg("Info", "Total Document : " + totaldoc + " Approval Done.", InformationType.Info);

                    ((DevExpress.ExpressApp.ListView)Frame.View).ObjectSpace.Refresh();
                }
            }
            else if (e.SelectedObjects.Count == 1)
            {
                SqlConnection conn = new SqlConnection(genCon.getConnectionString());
                ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
                ApprovalParameters p = (ApprovalParameters)e.PopupWindow.View.CurrentObject;

                if (p.IsValid == false)
                {
                    foreach (PurchaseOrders dtl in e.SelectedObjects)
                    {
                        IObjectSpace pos = Application.CreateObjectSpace();
                        PurchaseOrders po = pos.FindObject<PurchaseOrders>(new BinaryOperator("Oid", dtl.Oid));

                        if (po.Status == DocStatus.PendPost)
                        {
                            showMsg("Failed", "Document already approved, please refresh data.", InformationType.Error);
                            return;
                        }

                        if (po.AppUser != null)
                        {
                            if (!po.AppUser.Contains(user.Staff.StaffName))
                            {
                                showMsg("Failed", "Document already approved, please refresh data.", InformationType.Error);
                                return;
                            }
                        }

                        ApprovalStatusType appstatus = ApprovalStatusType.Required_Approval;

                        if (appstatus == ApprovalStatusType.Not_Applicable)
                            appstatus = ApprovalStatusType.Required_Approval;


                        if (p.IsErr) return;
                        if (appstatus == ApprovalStatusType.Required_Approval && p.AppStatus == ApprovalActions.NA)
                        {
                            showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                            return;
                        }
                        else if (appstatus == ApprovalStatusType.Approved && p.AppStatus == ApprovalActions.Yes)
                        {
                            showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                            return;
                        }
                        else if (appstatus == ApprovalStatusType.Rejected && p.AppStatus == ApprovalActions.No)
                        {
                            showMsg("Failed", "Same Approval Status is not allowed.", InformationType.Error);
                            return;
                        }
                        if (p.AppStatus == ApprovalActions.NA)
                        {
                            appstatus = ApprovalStatusType.Required_Approval;
                        }
                        if (p.AppStatus == ApprovalActions.Yes)
                        {
                            appstatus = ApprovalStatusType.Approved;
                        }
                        if (p.AppStatus == ApprovalActions.No)
                        {
                            appstatus = ApprovalStatusType.Rejected;
                        }

                        PurchaseOrderAppStatus ds = pos.CreateObject<PurchaseOrderAppStatus>();
                        ds.PurchaseOrders = pos.GetObjectByKey<PurchaseOrders>(po.Oid);
                        ds.AppStatus = appstatus;
                        if (appstatus == ApprovalStatusType.Rejected)
                        {
                            ds.AppRemarks = 
                                System.Environment.NewLine + "(Reject User: " + user.Staff.StaffName + ")" +
                                System.Environment.NewLine + "(Reason: Approval Rejected) - " + p.ParamString;
                            ds.CreateUser = pos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        }
                        else
                        {
                            ds.AppRemarks = System.Environment.NewLine + "(Approved User: " + user.Staff.StaffName + ") - " + p.ParamString;
                        }
                        po.PurchaseOrderAppStatus.Add(ds);

                        pos.CommitChanges();
                        pos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + po.Oid + "', 'PurchaseOrders', " + ((int)appstatus);
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(getapproval, conn);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetString(1) != "")
                            {
                                emailbody = "Dear Sir/Madam, " + System.Environment.NewLine + System.Environment.NewLine +
                                      reader.GetString(3) + System.Environment.NewLine + GeneralSettings.appurl + reader.GetString(2) +
                                      System.Environment.NewLine + System.Environment.NewLine;

                                if (appstatus == ApprovalStatusType.Approved)
                                    emailsubject = "Purchase Order Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "Purchase Order Approval Rejected";

                                emailaddress = reader.GetString(1);
                                emailuser = reader.GetGuid(0);

                                ToEmails.Add(emailaddress);
                            }
                        }
                        conn.Close();

                        if (ToEmails.Count > 0)
                        {
                            if (genCon.SendEmail(emailsubject, emailbody, ToEmails) == 1)
                            {
                            }
                        }
                        #endregion

                        IObjectSpace tos = Application.CreateObjectSpace();
                        PurchaseOrders trx = tos.FindObject<PurchaseOrders>(new BinaryOperator("Oid", po.Oid));
                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No PO selected.", InformationType.Error);
            }
        }

        private void ApproveAppPO_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            bool err = false;

            ApprovalStatusType appstatus = ApprovalStatusType.Required_Approval;

            IObjectSpace os = Application.CreateObjectSpace();
            DetailView dv = Application.CreateDetailView(os, os.CreateObject<ApprovalParameters>(), true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            switch (appstatus)
            {
                case ApprovalStatusType.Required_Approval:
                    ((ApprovalParameters)dv.CurrentObject).AppStatus = ApprovalActions.NA;
                    break;
                case ApprovalStatusType.Approved:
                    ((ApprovalParameters)dv.CurrentObject).AppStatus = ApprovalActions.Yes;
                    break;
                case ApprovalStatusType.Rejected:
                    ((ApprovalParameters)dv.CurrentObject).AppStatus = ApprovalActions.No;
                    break;
            }
            ((ApprovalParameters)dv.CurrentObject).IsErr = err;
            ((ApprovalParameters)dv.CurrentObject).ActionMessage = "Press choose from approval status 'Yes or No' and press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewPONoCost_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            PurchaseOrders po = (PurchaseOrders)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\PurchaseOrderNoPrice.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", po.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_PONoCost_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + po.Oid + "_" + user.UserName + "_PONoCost_"
                    + DateTime.Parse(po.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);

                po.PrintStatus = PrintStatus.Printed;
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }
    }
}
