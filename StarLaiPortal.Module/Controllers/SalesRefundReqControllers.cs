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
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
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
    public partial class SalesRefundReqControllers : ViewController
    {
        GeneralControllers genCon;
        public SalesRefundReqControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SFRInquiryItem.Active.SetItemValue("Enabled", false);
            this.SubmitSFR.Active.SetItemValue("Enabled", false);
            this.CancelSFR.Active.SetItemValue("Enabled", false);
            this.PreviewSFR.Active.SetItemValue("Enabled", false);
            this.ReviewAppSFR.Active.SetItemValue("Enabled", false);
            this.ApproveAppSFR.Active.SetItemValue("Enabled", false);
            this.RejectAppSFR.Active.SetItemValue("Enabled", false);
            this.SFRCopyToSF.Active.SetItemValue("Enabled", false);
            this.ApproveAppSFR_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesRefundRequests_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSFR.Active.SetItemValue("Enabled", true);
                    this.CancelSFR.Active.SetItemValue("Enabled", true);
                    //this.PreviewSAR.Active.SetItemValue("Enabled", true);
                    //this.SFRCopyToSF.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSFR.Active.SetItemValue("Enabled", false);
                    this.CancelSFR.Active.SetItemValue("Enabled", false);
                    this.PreviewSFR.Active.SetItemValue("Enabled", false);
                    this.SFRCopyToSF.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.SFRInquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SFRInquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "SalesRefundRequests_ListView_Approval")
            {
                //this.ReviewAppSAR.Active.SetItemValue("Enabled", true);
                //this.ReviewAppSAR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppSFR.Active.SetItemValue("Enabled", true);
                //this.ApproveAppSFR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppSFR.Active.SetItemValue("Enabled", true);
                //this.RejectAppSFR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppSFR_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "SalesRefundRequests_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppSFR_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppSFR_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SFRInquiryItem.Active.SetItemValue("Enabled", false);
                this.SubmitSFR.Active.SetItemValue("Enabled", false);
                this.CancelSFR.Active.SetItemValue("Enabled", false);
                this.PreviewSFR.Active.SetItemValue("Enabled", false);
                this.ReviewAppSFR.Active.SetItemValue("Enabled", false);
                this.ApproveAppSFR.Active.SetItemValue("Enabled", false);
                this.RejectAppSFR.Active.SetItemValue("Enabled", false);
                this.SFRCopyToSF.Active.SetItemValue("Enabled", false);
                this.ApproveAppSFR_Pop.Active.SetItemValue("Enabled", false);
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

        private void SFRInquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void SFRInquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            SalesRefundRequests trx = (SalesRefundRequests)View.CurrentObject;

            if (trx.IsNew == true)
            {
                string docprefix = genCon.GetDocPrefix();
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.SRF, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesRefundRequests sfr = os.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = sfr.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.SRF;

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
               DocTypeList.SRF, "True"));

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

        private void SubmitSFR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesRefundRequests selectedObject = (SalesRefundRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.IsValid2 == true)
            {
                showMsg("Failed", "Salesperson already inactive.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                SalesRefundReqDocTrail ds = ObjectSpace.CreateObject<SalesRefundReqDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.SalesRefundReqDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                #region Get approval
                List<string> ToEmails = new List<string>();
                string emailbody = "";
                string emailsubject = "";
                string emailaddress = "";
                Guid emailuser;
                DateTime emailtime = DateTime.Now;

                string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'SalesRefundRequest'";
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

                        emailsubject = "Sales Refund Request Approval";
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
                SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", selectedObject.Oid));

                if (trx.AppStatus == ApprovalStatusType.Not_Applicable && trx.Status == DocStatus.Submitted)
                {
                    trx.Status = DocStatus.PendPost;
                    os.CommitChanges();
                    os.Refresh();
                }

                IObjectSpace sos = Application.CreateObjectSpace();
                SalesRefundRequests strx = sos.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(sos, strx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitSFR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSFR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesRefundRequests selectedObject = (SalesRefundRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesRefundReqDocTrail ds = ObjectSpace.CreateObject<SalesRefundReqDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesRefundReqDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSFR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewSFR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ReviewAppSFR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ApproveAppSFR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesRefundRequests selectedObject = (SalesRefundRequests)e.CurrentObject;

            selectedObject.Status = DocStatus.PendPost;
            selectedObject.AppStatus = ApprovalStatusType.Approved;

            SalesRefundReqDocTrail ds = ObjectSpace.CreateObject<SalesRefundReqDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.SalesRefundReqDocTrail.Add(ds);

            SalesRefundReqAppStatus apps = ObjectSpace.CreateObject<SalesRefundReqAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.SalesRefundReqAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppSFR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesRefundRequests selectedObject = (SalesRefundRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            SalesRefundReqDocTrail ds = ObjectSpace.CreateObject<SalesRefundReqDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.SalesRefundReqDocTrail.Add(ds);

            SalesRefundReqAppStatus apps = ObjectSpace.CreateObject<SalesRefundReqAppStatus>();
            apps.AppStatus = ApprovalStatusType.Rejected;
            apps.AppRemarks = "Rejected";
            selectedObject.SalesRefundReqAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void SFRCopyToSF_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SalesRefundRequests srf = (SalesRefundRequests)View.CurrentObject;
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefunds newsrf = os.CreateObject<SalesRefunds>();

                if (srf.Customer != null)
                {
                    newsrf.Customer = newsrf.Session.GetObjectByKey<vwBusniessPartner>(srf.Customer.BPCode);
                }
                newsrf.Reference = srf.Reference;
                newsrf.Remarks = srf.Remarks;

                foreach (SalesRefundReqDetails dtl in srf.SalesRefundReqDetails)
                {
                    SalesRefundDetails newsrfdetails = os.CreateObject<SalesRefundDetails>();

                    newsrfdetails.ItemCode = newsrfdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                    newsrfdetails.ItemDesc = dtl.ItemDesc;
                    newsrfdetails.ItemDetails = dtl.ItemDetails;
                    newsrfdetails.UOM = dtl.UOM;
                    if (dtl.Warehouse != null)
                    {
                        newsrfdetails.Warehouse = newsrfdetails.Session.GetObjectByKey<vwWarehouse>(dtl.Warehouse.WarehouseCode);
                    }
                    if (dtl.Bin != null)
                    {
                        newsrfdetails.Bin = newsrfdetails.Session.GetObjectByKey<vwBin>(dtl.Bin.BinCode);
                    }
                    newsrfdetails.Quantity = dtl.Quantity;
                    newsrfdetails.Price = dtl.Price;
                    newsrfdetails.BaseDoc = srf.DocNum;
                    newsrfdetails.BaseId = dtl.Oid.ToString();
                    newsrf.SalesRefundDetails.Add(newsrfdetails);
                }

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(os, newsrf);
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
                        foreach (SalesRefundRequests dtl in e.SelectedObjects)
                        {
                            IObjectSpace sos = Application.CreateObjectSpace();
                            SalesRefundRequests sfr = sos.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", dtl.Oid));

                            if (sfr.Status == DocStatus.Submitted && sfr.AppStatus == ApprovalStatusType.Required_Approval)
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

                                SalesRefundReqAppStatus ds = sos.CreateObject<SalesRefundReqAppStatus>();
                                ds.SalesRefundRequests = sos.GetObjectByKey<SalesRefundRequests>(sfr.Oid);
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
                                sfr.SalesRefundReqAppStatus.Add(ds);

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

                                string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sfr.Oid + "', 'SalesRefundReq', " + ((int)appstatus);
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
                    foreach (SalesRefundRequests dtl in e.SelectedObjects)
                    {
                        IObjectSpace sos = Application.CreateObjectSpace();
                        SalesRefundRequests sfr = sos.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", dtl.Oid));

                        if (sfr.Status == DocStatus.Submitted && sfr.AppStatus == ApprovalStatusType.Approved)
                        {
                            showMsg("Failed", "Document already approved, please refresh data.", InformationType.Error);
                            return;
                        }

                        if (sfr.AppUser != null)
                        {
                            if (!sfr.AppUser.Contains(user.Staff.StaffName))
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

                        SalesRefundReqAppStatus ds = sos.CreateObject<SalesRefundReqAppStatus>();
                        ds.SalesRefundRequests = sos.GetObjectByKey<SalesRefundRequests>(sfr.Oid);
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
                        sfr.SalesRefundReqAppStatus.Add(ds);

                        sos.CommitChanges();
                        sos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sfr.Oid + "', 'SalesRefundReq', " + ((int)appstatus);
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
                        SalesRefundRequests trx = tos.FindObject<SalesRefundRequests>(new BinaryOperator("Oid", dtl.Oid));
                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No CN selected.", InformationType.Error);
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
