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
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SalesReturnReqControllers : ViewController
    {
        GeneralControllers genCon;
        public SalesReturnReqControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SRRCopyFromInv.Active.SetItemValue("Enabled", false);
            this.SRRInquiryItem.Active.SetItemValue("Enabled", false);
            this.SubmitSRR.Active.SetItemValue("Enabled", false);
            this.CancelSRR.Active.SetItemValue("Enabled", false);
            this.PreviewSRR.Active.SetItemValue("Enabled", false);
            this.ReviewAppSRR.Active.SetItemValue("Enabled", false);
            this.ApproveAppSRR.Active.SetItemValue("Enabled", false);
            this.RejectAppSRR.Active.SetItemValue("Enabled", false);
            this.SRRPrintLabel.Active.SetItemValue("Enabled", false);
            this.SRRCopyToSR.Active.SetItemValue("Enabled", false);
            this.ApproveAppSRR_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesReturnRequests_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSRR.Active.SetItemValue("Enabled", true);
                    this.CancelSRR.Active.SetItemValue("Enabled", true);
                    //this.PreviewSRR.Active.SetItemValue("Enabled", true);
                    //this.SRRPrintLabel.Active.SetItemValue("Enabled", true);
                    this.SRRCopyToSR.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSRR.Active.SetItemValue("Enabled", false);
                    this.CancelSRR.Active.SetItemValue("Enabled", false);
                    this.PreviewSRR.Active.SetItemValue("Enabled", false);
                    this.SRRPrintLabel.Active.SetItemValue("Enabled", false);
                    this.SRRCopyToSR.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.SRRCopyFromInv.Active.SetItemValue("Enabled", true);
                    this.SRRInquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SRRCopyFromInv.Active.SetItemValue("Enabled", false);
                    this.SRRInquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "SalesReturnRequests_ListView_Approval")
            {
                //this.ReviewAppSRR.Active.SetItemValue("Enabled", true);
                //this.ReviewAppSRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppSRR.Active.SetItemValue("Enabled", true);
                //this.ApproveAppSRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppSRR.Active.SetItemValue("Enabled", true);
                //this.RejectAppSRR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppSRR_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "SalesReturnRequests_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppSRR_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppSRR_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SRRCopyFromInv.Active.SetItemValue("Enabled", false);
                this.SRRInquiryItem.Active.SetItemValue("Enabled", false);
                this.SubmitSRR.Active.SetItemValue("Enabled", false);
                this.CancelSRR.Active.SetItemValue("Enabled", false);
                this.PreviewSRR.Active.SetItemValue("Enabled", false);
                this.ReviewAppSRR.Active.SetItemValue("Enabled", false);
                this.ApproveAppSRR.Active.SetItemValue("Enabled", false);
                this.RejectAppSRR.Active.SetItemValue("Enabled", false);
                this.SRRPrintLabel.Active.SetItemValue("Enabled", false);
                this.SRRCopyToSR.Active.SetItemValue("Enabled", false);
                this.ApproveAppSRR_Pop.Active.SetItemValue("Enabled", false);
            }

            if (View.Id == "SalesReturnRequests_SalesReturnRequestDetails_ListView")
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
                    //object validation = currentObject.GetType().GetProperty("IsValid").GetValue(currentObject);

                    //if ((bool)validation == true)
                    //{
                    //    showMsg("Error", "Price cannot zero.", InformationType.Error);
                    //}

                    object validation1 = currentObject.GetType().GetProperty("IsValid1").GetValue(currentObject);

                    if ((bool)validation1 == true)
                    {
                        showMsg("Error", "Unit Cost cannot zero.", InformationType.Error);
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

        private void SRRCopyFromInv_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                try
                {
                    SalesReturnRequests srr = (SalesReturnRequests)View.CurrentObject;

                    //if (srr.IsNew == true)
                    //{
                    //    IObjectSpace os = Application.CreateObjectSpace();
                    //    SalesReturnRequests newsrr = os.CreateObject<SalesReturnRequests>();

                    //    foreach (vwInvoice dtl in e.PopupWindowViewSelectedObjects)
                    //    {
                    //        if (dtl.CardCode != null)
                    //        {
                    //            newsrr.Customer = newsrr.Session.GetObjectByKey<vwBusniessPartner>(dtl.CardCode);
                    //        }

                    //        SalesReturnRequestDetails newsrritem = os.CreateObject<SalesReturnRequestDetails>();

                    //        newsrritem.ItemCode = newsrritem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                    //        newsrritem.ItemDesc = dtl.ItemDescrip;
                    //        if (dtl.WhsCode != null)
                    //        {
                    //            newsrritem.Warehouse = newsrritem.Session.GetObjectByKey<vwWarehouse>(dtl.WhsCode.WarehouseCode);
                    //        }
                    //        newsrritem.Quantity = dtl.OpenQty;
                    //        newsrritem.RtnQuantity = dtl.OpenQty;
                    //        newsrritem.Price = dtl.UnitPrice;
                    //        newsrritem.BaseDoc = dtl.BaseEntry.ToString();
                    //        newsrritem.BaseId = dtl.BaseLine.ToString();

                    //        newsrr.SalesReturnRequestDetails.Add(newsrritem);
                    //    }

                    //    ShowViewParameters svp = new ShowViewParameters();
                    //    DetailView dv = Application.CreateDetailView(os, newsrr);
                    //    dv.ViewEditMode = ViewEditMode.Edit;
                    //    dv.IsRoot = true;
                    //    svp.CreatedView = dv;

                    //    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    //    showMsg("Success", "Copy Success.", InformationType.Success);
                    //}
                    //else
                    //{
                        string invoiceno = null;
                        foreach (vwInvoice dtl in e.PopupWindowViewSelectedObjects)
                        {
                            if (invoiceno == null)
                            {
                                invoiceno = dtl.SAPDocNum;
                            }
                            else
                            {
                                invoiceno = invoiceno + ", " + dtl.SAPDocNum;
                            }

                            if (dtl.Salesperson != null)
                            {
                                srr.Salesperson = ObjectSpace.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpName = ?", dtl.Salesperson));
                            }

                            SalesReturnRequestDetails newsrritem = ObjectSpace.CreateObject<SalesReturnRequestDetails>();

                            newsrritem.ItemCode = newsrritem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newsrritem.ItemDesc = dtl.ItemDescrip;
                            if (dtl.WhsCode != null)
                            {
                                newsrritem.Warehouse = newsrritem.Session.GetObjectByKey<vwWarehouse>(dtl.WhsCode.WarehouseCode);
                            }
                            newsrritem.Quantity = dtl.OpenQty;
                            newsrritem.RtnQuantity = dtl.OpenQty;
                            newsrritem.Price = dtl.UnitPrice;
                            newsrritem.UnitCost = dtl.UnitCost;
                            newsrritem.BaseDoc = dtl.BaseEntry.ToString();
                            newsrritem.BaseId = dtl.BaseLine.ToString();

                            srr.SalesReturnRequestDetails.Add(newsrritem);

                            showMsg("Success", "Copy Success.", InformationType.Success);
                        }

                        srr.Reference = invoiceno;
                        if (srr.DocNum == null)
                        {
                            string docprefix = genCon.GetDocPrefix();
                            srr.DocNum = genCon.GenerateDocNum(DocTypeList.SRR, ObjectSpace, TransferType.NA, 0, docprefix);
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

        private void SRRCopyFromInv_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            SalesReturnRequests srr = (SalesReturnRequests)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwInvoice));
            var cs = Application.CreateCollectionSource(os, typeof(vwInvoice), viewId);
            if (srr.Customer != null)
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", srr.Customer.BPCode);
            }
            else
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", "");
            }

            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void SubmitSRR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesReturnRequests selectedObject = (SalesReturnRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            
            if (selectedObject.IsValid3 == true)
            {
                showMsg("Error", "Unit Cost cannot zero.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid == true)
            {
                if (selectedObject.IsValid1 == false)
                {
                    if (selectedObject.IsValid2 == false)
                    {
                        selectedObject.Status = DocStatus.Submitted;
                        SalesReturnRequestDocTrail ds = ObjectSpace.CreateObject<SalesReturnRequestDocTrail>();
                        ds.DocStatus = DocStatus.Submitted;
                        ds.DocRemarks = p.ParamString;
                        selectedObject.SalesReturnRequestDocTrail.Add(ds);

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();

                        #region Get approval
                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'SalesReturnRequest'";
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

                                emailsubject = "Sales Return Request Approval";
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
                        SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", selectedObject.Oid));
                        openNewView(os, trx, ViewEditMode.View);
                        showMsg("Successful", "Submit Done.", InformationType.Success);
                    }
                    else
                    {
                        showMsg("Error", "Return qty cannot be zero.", InformationType.Error);
                    }
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

        private void SubmitSRR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSRR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesReturnRequests selectedObject = (SalesReturnRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesReturnRequestDocTrail ds = ObjectSpace.CreateObject<SalesReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesReturnRequestDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSRR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void SRRPrintLabel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }

        private void SRRPrintLabel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {

        }

        private void PreviewSRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void SRRInquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void SRRInquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            SalesReturnRequests trx = (SalesReturnRequests)View.CurrentObject;

            if (trx.DocNum == null)
            {
                string docprefix = genCon.GetDocPrefix();
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.SRR, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesReturnRequests srr = os.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = srr.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.SRR;
            ((ItemInquiry)dv.CurrentObject).CardCode = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwBusniessPartner>
                (trx.Customer.BPCode);

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
                DocTypeList.SRR, "True"));

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

        private void ReviewAppSRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ApproveAppSRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesReturnRequests selectedObject = (SalesReturnRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Approved;

            SalesReturnRequestDocTrail ds = ObjectSpace.CreateObject<SalesReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.SalesReturnRequestDocTrail.Add(ds);

            SalesReturnRequestAppStatus apps = ObjectSpace.CreateObject<SalesReturnRequestAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.SalesReturnRequestAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppSRR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesReturnRequests selectedObject = (SalesReturnRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            SalesReturnRequestDocTrail ds = ObjectSpace.CreateObject<SalesReturnRequestDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.SalesReturnRequestDocTrail.Add(ds);

            SalesReturnRequestAppStatus apps = ObjectSpace.CreateObject<SalesReturnRequestAppStatus>();
            apps.AppStatus = ApprovalStatusType.Rejected;
            apps.AppRemarks = "Rejected";
            selectedObject.SalesReturnRequestAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void SRRCopyToSR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SalesReturnRequests srr = (SalesReturnRequests)View.CurrentObject;
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturns newsr = os.CreateObject<SalesReturns>();

                if (srr.Customer != null)
                {
                    newsr.Customer = newsr.Session.GetObjectByKey<vwBusniessPartner>(srr.Customer.BPCode);
                }
                newsr.CustomerName = srr.CustomerName;
                if (srr.Salesperson != null)
                {
                    newsr.Salesperson = newsr.Session.GetObjectByKey<vwSalesPerson>(srr.Salesperson.SlpCode);
                }
                if (srr.Transporter != null)
                {
                    newsr.Transporter = newsr.Session.GetObjectByKey<vwTransporter>(srr.Transporter.TransporterID);
                }
                if (srr.BillingAddress != null)
                {
                    newsr.BillingAddress = newsr.Session.GetObjectByKey<vwBillingAddress>(srr.BillingAddress.PriKey);
                }
                newsr.BillingAddressfield = srr.BillingAddressfield;
                if (srr.ShippingAddress != null)
                {
                    newsr.ShippingAddress = newsr.Session.GetObjectByKey<vwShippingAddress>(srr.ShippingAddress.PriKey);
                }
                newsr.ShippingAddressfield = srr.ShippingAddressfield;
                newsr.Reference = srr.Reference;
                newsr.Remarks = srr.Remarks;
                if (srr.ReasonCode != null)
                {
                    newsr.ReasonCode = newsr.Session.GetObjectByKey<vwReasonCode>(srr.ReasonCode.Prikey);
                }

                foreach (SalesReturnRequestDetails dtl in srr.SalesReturnRequestDetails)
                {
                    SalesReturnDetails newsrdetails = os.CreateObject<SalesReturnDetails>();

                    newsrdetails.ItemCode = newsrdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                    newsrdetails.ItemDesc = dtl.ItemDesc;
                    newsrdetails.ItemDetails = dtl.ItemDetails;
                    newsrdetails.DefBarcode = dtl.DefBarcode;
                    newsrdetails.UOM = dtl.UOM;
                    if (dtl.ReasonCode != null)
                    {
                        newsrdetails.ReasonCode = newsrdetails.Session.GetObjectByKey<vwReasonCode>(dtl.ReasonCode.Prikey);
                    }
                    if (dtl.Warehouse != null)
                    {
                        newsrdetails.Warehouse = newsrdetails.Session.GetObjectByKey<vwWarehouse>(dtl.Warehouse.WarehouseCode);
                    }
                    if (dtl.Bin != null)
                    {
                        newsrdetails.Bin = newsrdetails.Session.GetObjectByKey<vwBin>(dtl.Bin.BinCode);
                    }
                    newsrdetails.Quantity = dtl.RtnQuantity;
                    newsrdetails.RtnQuantity = dtl.RtnQuantity;
                    newsrdetails.UnitCost = dtl.UnitCost;
                    newsrdetails.Price = dtl.Price;
                    newsrdetails.BaseDoc = srr.DocNum;
                    newsrdetails.BaseId = dtl.Oid.ToString();
                    newsrdetails.InvoiceDoc = dtl.BaseDoc;
                    newsr.SalesReturnDetails.Add(newsrdetails);
                }

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(os, newsr);
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

        private void ApproveAppSRR_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
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
                        foreach (SalesReturnRequests dtl in e.SelectedObjects)
                        {
                            IObjectSpace sos = Application.CreateObjectSpace();
                            SalesReturnRequests srr = sos.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                            SalesReturnRequestAppStatus ds = sos.CreateObject<SalesReturnRequestAppStatus>();
                            ds.SalesReturnRequests = sos.GetObjectByKey<SalesReturnRequests>(srr.Oid);
                            ds.AppStatus = appstatus;
                            if (appstatus == ApprovalStatusType.Rejected)
                            {
                                //sq.Status = DocStatus.New;
                                ds.AppRemarks =
                                    System.Environment.NewLine + "(Reject User: " + user.Staff.StaffName + ")" +
                                    System.Environment.NewLine + "(Reason: Approval Rejected) - " + p.ParamString;
                                ds.CreateUser = sos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            }
                            else
                            {
                                ds.AppRemarks = System.Environment.NewLine + "(Approved User: " + user.Staff.StaffName + ") - " + p.ParamString;
                            }
                            srr.SalesReturnRequestAppStatus.Add(ds);

                            sos.CommitChanges();
                            sos.Refresh();

                            totaldoc++;

                            #region approval

                            List<string> ToEmails = new List<string>();
                            string emailbody = "";
                            string emailsubject = "";
                            string emailaddress = "";
                            Guid emailuser;
                            DateTime emailtime = DateTime.Now;

                            string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + srr.Oid + "', 'SalesReturnRequests', " + ((int)appstatus);
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
                                        emailsubject = "Sales Return Request Approval";
                                    else if (appstatus == ApprovalStatusType.Rejected)
                                        emailsubject = "Sales Return Request Approval Rejected";

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

                        showMsg("Info", "Total Document : " + totaldoc + " Approval Done.", InformationType.Info);

                        ((DevExpress.ExpressApp.ListView)Frame.View).ObjectSpace.Refresh();
                    }
                    catch (Exception ex)
                    {
                        showMsg("Error", ex.Message, InformationType.Error);
                    }
                }

            }
            else if (e.SelectedObjects.Count == 1)
            {
                SqlConnection conn = new SqlConnection(genCon.getConnectionString());
                ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
                ApprovalParameters p = (ApprovalParameters)e.PopupWindow.View.CurrentObject;

                if (p.IsValid == false)
                {
                    foreach (SalesReturnRequests dtl in e.SelectedObjects)
                    {
                        IObjectSpace sos = Application.CreateObjectSpace();
                        SalesReturnRequests srr = sos.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                        SalesReturnRequestAppStatus ds = sos.CreateObject<SalesReturnRequestAppStatus>();
                        ds.SalesReturnRequests = sos.GetObjectByKey<SalesReturnRequests>(srr.Oid);
                        ds.AppStatus = appstatus;
                        if (appstatus == ApprovalStatusType.Rejected)
                        {
                            ds.AppRemarks =
                                System.Environment.NewLine + "(Reject User: " + user.Staff.StaffName + ")" +
                                System.Environment.NewLine + "(Reason: Approval Rejected) - " + p.ParamString;
                            ds.CreateUser = sos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        }
                        else
                        {
                            ds.AppRemarks = System.Environment.NewLine + "(Approved User: " + user.Staff.StaffName + ") - " + p.ParamString;
                        }
                        srr.SalesReturnRequestAppStatus.Add(ds);

                        sos.CommitChanges();
                        sos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + srr.Oid + "', 'SalesReturnRequests', " + ((int)appstatus);
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
                                    emailsubject = "Sales Return Request Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "Sales Return Request Rejected";

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
                        SalesReturnRequests trx = tos.FindObject<SalesReturnRequests>(new BinaryOperator("Oid", srr.Oid));
                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No Sales Return Request selected.", InformationType.Error);
            }
        }

        private void ApproveAppSRR_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
