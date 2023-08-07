using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

// 2023-07-28 add GRPO Correction ver 0.1

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PurchaseReturnReqControllers : ViewController
    {
        GeneralControllers genCon;
        public PurchaseReturnReqControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.PRRCopyFromPO.Active.SetItemValue("Enabled", false);
            this.PRRInquiryItem.Active.SetItemValue("Enabled", false);
            this.SubmitPRR.Active.SetItemValue("Enabled", false);
            this.CancelPRR.Active.SetItemValue("Enabled", false);
            this.PreviewPRR.Active.SetItemValue("Enabled", false);
            this.ReviewAppPRR.Active.SetItemValue("Enabled", false);
            this.ApproveAppPRR.Active.SetItemValue("Enabled", false);
            this.RejectAppPRR.Active.SetItemValue("Enabled", false);
            this.PRRCopyToPReturn.Active.SetItemValue("Enabled", false);
            this.ApproveAppPRR_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "PurchaseReturnRequests_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitPRR.Active.SetItemValue("Enabled", true);
                    this.CancelPRR.Active.SetItemValue("Enabled", true);
                    //this.PreviewPRR.Active.SetItemValue("Enabled", true);
                    this.PRRCopyToPReturn.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitPRR.Active.SetItemValue("Enabled", false);
                    this.CancelPRR.Active.SetItemValue("Enabled", false);
                    this.PreviewPRR.Active.SetItemValue("Enabled", false);
                    this.PRRCopyToPReturn.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.PRRCopyFromPO.Active.SetItemValue("Enabled", true);
                    this.PRRInquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.PRRCopyFromPO.Active.SetItemValue("Enabled", false);
                    this.PRRInquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "PurchaseReturnRequests_ListView_Approval")
            {
                //this.ReviewAppPRR.Active.SetItemValue("Enabled", true);
                //this.ReviewAppPRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppPRR.Active.SetItemValue("Enabled", true);
                //this.ApproveAppPRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppPRR.Active.SetItemValue("Enabled", true);
                //this.RejectAppPRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppPRR_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "PurchaseReturnRequests_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppPRR_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppPRR_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.PRRCopyFromPO.Active.SetItemValue("Enabled", false);
                this.PRRInquiryItem.Active.SetItemValue("Enabled", false);
                this.SubmitPRR.Active.SetItemValue("Enabled", false);
                this.CancelPRR.Active.SetItemValue("Enabled", false);
                this.PreviewPRR.Active.SetItemValue("Enabled", false);
                this.PRRCopyToPReturn.Active.SetItemValue("Enabled", false);
                this.ApproveAppPRR_Pop.Active.SetItemValue("Enabled", false);
            }

            if (View.Id == "PurchaseReturnRequests_PurchaseReturnRequestDetails_ListView")
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
                        showMsg("Error", "Price not allow 0.", InformationType.Error);
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

        private void PRRInquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void PRRInquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            PurchaseReturnRequests trx = (PurchaseReturnRequests)View.CurrentObject;

            if (trx.DocNum == null)
            {
                string docprefix = genCon.GetDocPrefix();
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.PRR, ObjectSpace, TransferType.NA, trx.Series.Oid, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            PurchaseReturnRequests prr = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = prr.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.PRR;
            ((ItemInquiry)dv.CurrentObject).CardCode = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwBusniessPartner>
                (trx.Supplier.BPCode);

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
               DocTypeList.PRR, "True"));

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

        private void PRRCopyFromPO_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                //try
                //{
                    PurchaseReturnRequests prr = (PurchaseReturnRequests)View.CurrentObject;

                    //if (prr.IsNew == true)
                    //{
                    //    IObjectSpace os = Application.CreateObjectSpace();
                    //    PurchaseReturnRequests newprr = os.CreateObject<PurchaseReturnRequests>();

                    //    foreach (vwPRRPO dtl in e.PopupWindowViewSelectedObjects)
                    //    {
                    //        if (dtl.CardCode != null)
                    //        {
                    //            newprr.Supplier = newprr.Session.GetObjectByKey<vwBusniessPartner>(dtl.CardCode);
                    //        }

                    //        PurchaseReturnRequestDetails newprritem = os.CreateObject<PurchaseReturnRequestDetails>();

                    //        newprritem.ItemCode = newprritem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                    //        newprritem.ItemDesc = dtl.ItemDescrip;
                    //        if (dtl.WhsCode != null)
                    //        {
                    //            newprritem.Warehouse = newprritem.Session.GetObjectByKey<vwWarehouse>(dtl.WhsCode.WarehouseCode);
                    //        }
                    //        newprritem.Quantity = dtl.OpenQty;
                    //        newprritem.RtnQuantity = dtl.OpenQty;
                    //        newprritem.Price = dtl.UnitPrice;
                    //        newprritem.BaseDoc = dtl.BaseEntry.ToString();
                    //        newprritem.BaseId = dtl.BaseLine.ToString();
                    //        newprritem.PODocNum = dtl.SAPDocNum;

                    //        newprr.PurchaseReturnRequestDetails.Add(newprritem);
                    //    }

                    //    ShowViewParameters svp = new ShowViewParameters();
                    //    DetailView dv = Application.CreateDetailView(os, newprr);
                    //    dv.ViewEditMode = ViewEditMode.Edit;
                    //    dv.IsRoot = true;
                    //    svp.CreatedView = dv;

                    //    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    //    showMsg("Success", "Copy Success.", InformationType.Success);
                    //}
                    //else
                    //{
                        foreach (vwPRRPO dtl in e.PopupWindowViewSelectedObjects)
                        {
                            prr.Reference = dtl.VendorRef;
                            // Start ver 0.1
                            prr.GRPOCorrection = true;
                            // End ver 0.1
                            PurchaseReturnRequestDetails newprritem = ObjectSpace.CreateObject<PurchaseReturnRequestDetails>();

                            newprritem.ItemCode = newprritem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newprritem.ItemDesc = dtl.ItemDescrip;
                            if (dtl.WhsCode != null)
                            {
                                newprritem.Warehouse = newprritem.Session.GetObjectByKey<vwWarehouse>(dtl.WhsCode.WarehouseCode);
                            }
                            newprritem.Quantity = dtl.OpenQty;
                            newprritem.RtnQuantity = dtl.OpenQty;
                            newprritem.Price = dtl.UnitPrice;
                            newprritem.BaseDoc = dtl.BaseEntry.ToString();
                            newprritem.BaseId = dtl.BaseLine.ToString();
                            newprritem.PODocNum = dtl.SAPDocNum;

                            prr.PurchaseReturnRequestDetails.Add(newprritem);
                        }

                        if (prr.DocNum == null)
                        {
                            string docprefix = genCon.GetDocPrefix();
                            prr.DocNum = genCon.GenerateDocNum(DocTypeList.PRR, ObjectSpace, TransferType.NA, prr.Series.Oid, docprefix);
                        }
                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();

                        showMsg("Success", "Copy Success.", InformationType.Success);
                    //}
                //}
                //catch (Exception)
                //{
                //    showMsg("Fail", "Copy Fail.", InformationType.Error);
                //}
            }
        }

        private void PRRCopyFromPO_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            PurchaseReturnRequests prr = (PurchaseReturnRequests)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwPRRPO));
            var cs = Application.CreateCollectionSource(os, typeof(vwPRRPO), viewId);
            if (prr.Supplier != null)
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", prr.Supplier.BPCode);
            }
            else
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", "");
            }

            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void SubmitPRR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PurchaseReturnRequests selectedObject = (PurchaseReturnRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.IsValid2 == true)
            {
                showMsg("Error", "Return quantity cannot zero.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid3 == true)
            {
                showMsg("Error", "Bin not enough stock.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid4 == true)
            {
                showMsg("Error", "Price not allow 0.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid == true)
            {
                if (selectedObject.IsValid1 == false)
                {
                    selectedObject.Status = DocStatus.Submitted;
                    PurchaseReturnRequestDocTrail ds = ObjectSpace.CreateObject<PurchaseReturnRequestDocTrail>();
                    ds.DocStatus = DocStatus.Submitted;
                    ds.DocRemarks = p.ParamString;
                    selectedObject.PurchaseReturnRequestDocTrail.Add(ds);

                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();

                    #region Get approval
                    List<string> ToEmails = new List<string>();
                    string emailbody = "";
                    string emailsubject = "";
                    string emailaddress = "";
                    Guid emailuser;
                    DateTime emailtime = DateTime.Now;

                    string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'PurchaseReturnRequest'";
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

                            emailsubject = "Purchase Return Request Approval";
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
                    PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", selectedObject.Oid));
                    openNewView(os, trx, ViewEditMode.View);
                    showMsg("Successful", "Submit Done.", InformationType.Success);
                }
                else
                {
                    showMsg("Error", "Please fill in reason code.", InformationType.Error);
                }
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitPRR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelPRR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PurchaseReturnRequests selectedObject = (PurchaseReturnRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            PurchaseReturnRequestDocTrail ds = ObjectSpace.CreateObject<PurchaseReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.PurchaseReturnRequestDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelPRR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewPRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ReviewAppPRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ApproveAppPRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PurchaseReturnRequests selectedObject = (PurchaseReturnRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Approved;

            PurchaseReturnRequestDocTrail ds = ObjectSpace.CreateObject<PurchaseReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.PurchaseReturnRequestDocTrail.Add(ds);

            PurchaseReturnRequestAppStatus apps = ObjectSpace.CreateObject<PurchaseReturnRequestAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.PurchaseReturnRequestAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppPRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PurchaseReturnRequests selectedObject = (PurchaseReturnRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            PurchaseReturnRequestDocTrail ds = ObjectSpace.CreateObject<PurchaseReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.PurchaseReturnRequestDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void PRRCopyToPReturn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PurchaseReturnRequests prr = (PurchaseReturnRequests)View.CurrentObject;
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturns newpr = os.CreateObject<PurchaseReturns>();

                if (prr.Supplier != null)
                {
                    newpr.Supplier = newpr.Session.GetObjectByKey<vwBusniessPartner>(prr.Supplier.BPCode);
                }
                if (prr.Requestor != null)
                {
                    newpr.Requestor = newpr.Session.GetObjectByKey<vwSalesPerson>(prr.Requestor.SlpCode);
                }
                if (prr.Transporter != null)
                {
                    newpr.Transporter = newpr.Session.GetObjectByKey<vwTransporter>(prr.Transporter.TransporterID);
                }
                if (prr.BillingAddress != null)
                {
                    newpr.BillingAddress = newpr.Session.GetObjectByKey<vwBillingAddress>(prr.BillingAddress.PriKey);
                }
                newpr.BillingAddressfield = prr.BillingAddressfield;
                if (prr.ShippingAddress != null)
                {
                    newpr.ShippingAddress = newpr.Session.GetObjectByKey<vwShippingAddress>(prr.ShippingAddress.PriKey);
                }
                newpr.ShippingAddressfield = prr.ShippingAddressfield;
                newpr.Reference = prr.Reference;
                newpr.Remarks = prr.Remarks;
                if (prr.Series != null)
                {
                    newpr.Series = newpr.Session.GetObjectByKey<Series>(prr.Series.Oid);
                }
                if (prr.ReasonCode != null)
                {
                    newpr.ReasonCode = newpr.Session.GetObjectByKey<vwReasonCode>(prr.ReasonCode.Prikey);
                }
                // Start ver 0.1
                newpr.GRPOCorrection = prr.GRPOCorrection;
                // End ver 0.1

                foreach (PurchaseReturnRequestDetails dtl in prr.PurchaseReturnRequestDetails)
                {
                    PurchaseReturnDetails newprdetails = os.CreateObject<PurchaseReturnDetails>();

                    newprdetails.ItemCode = newprdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                    newprdetails.ItemDesc = dtl.ItemDesc;
                    newprdetails.ItemDetails = dtl.ItemDetails;
                    newprdetails.DefBarcode = dtl.DefBarcode;
                    newprdetails.UOM = dtl.UOM;
                    if (dtl.ReasonCode != null)
                    {
                        newprdetails.ReasonCode = newprdetails.Session.GetObjectByKey<vwReasonCode>(dtl.ReasonCode.Prikey);
                    }
                    if (dtl.Warehouse != null)
                    {
                        newprdetails.Warehouse = newprdetails.Session.GetObjectByKey<vwWarehouse>(dtl.Warehouse.WarehouseCode);
                    }
                    if (dtl.Bin != null)
                    {
                        newprdetails.Bin = newprdetails.Session.GetObjectByKey<vwBin>(dtl.Bin.BinCode);
                    }
                    newprdetails.Quantity = dtl.RtnQuantity;
                    newprdetails.Price = dtl.Price;
                    newprdetails.BaseDoc = prr.DocNum;
                    newprdetails.BaseId = dtl.Oid.ToString();
                    newprdetails.PODocNum = dtl.PODocNum;
                    // Start ver 0.1
                    newprdetails.GRNBaseDoc = dtl.BaseDoc;
                    newprdetails.GRNBaseId = dtl.BaseId;
                    // End ver 0.1
                    newpr.PurchaseReturnDetails.Add(newprdetails);
                }

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(os, newpr);
                dv.ViewEditMode = ViewEditMode.Edit;
                dv.IsRoot = true;
                svp.CreatedView = dv;

                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                showMsg("Success", "Copy Success.", InformationType.Success);
            }
            catch (Exception)
            {
                showMsg("Fail", "Copy Fail.", InformationType.Error);
            }
        }

        private void ApproveAppPRR_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
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
                        foreach (PurchaseReturnRequests dtl in e.SelectedObjects)
                        {
                            IObjectSpace pos = Application.CreateObjectSpace();
                            PurchaseReturnRequests prr = pos.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                            PurchaseReturnRequestAppStatus ds = pos.CreateObject<PurchaseReturnRequestAppStatus>();
                            ds.PurchaseReturnRequests = pos.GetObjectByKey<PurchaseReturnRequests>(prr.Oid);
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
                            prr.PurchaseReturnRequestAppStatus.Add(ds);

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

                            string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + prr.Oid + "', 'PurchaseReturnRequests', " + ((int)appstatus);
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
                                        emailsubject = "Purchase Return Request Approval";
                                    else if (appstatus == ApprovalStatusType.Rejected)
                                        emailsubject = "Purchase Return Request Approval Rejected";

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
                    foreach (PurchaseReturnRequests dtl in e.SelectedObjects)
                    {
                        IObjectSpace pos = Application.CreateObjectSpace();
                        PurchaseReturnRequests prr = pos.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                        PurchaseReturnRequestAppStatus ds = pos.CreateObject<PurchaseReturnRequestAppStatus>();
                        ds.PurchaseReturnRequests = pos.GetObjectByKey<PurchaseReturnRequests>(prr.Oid);
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
                        prr.PurchaseReturnRequestAppStatus.Add(ds);

                        pos.CommitChanges();
                        pos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + prr.Oid + "', 'PurchaseReturnRequests', " + ((int)appstatus);
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
                                    emailsubject = "Purchase Return Request Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "Purchase Return Request Approval Rejected";

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
                        PurchaseReturnRequests trx = tos.FindObject<PurchaseReturnRequests>(new BinaryOperator("Oid", prr.Oid));
                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No Purchase Return Request selected.", InformationType.Error);
            }
        }

        private void ApproveAppPRR_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
    }
}
