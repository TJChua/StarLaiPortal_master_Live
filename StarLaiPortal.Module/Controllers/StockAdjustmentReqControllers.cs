using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
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
    public partial class StockAdjustmentReqControllers : ViewController
    {
        GeneralControllers genCon;
        public StockAdjustmentReqControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SARInquiryItem.Active.SetItemValue("Enabled", false);
            this.SubmitSAR.Active.SetItemValue("Enabled", false);
            this.CancelSAR.Active.SetItemValue("Enabled", false);
            this.PreviewSAR.Active.SetItemValue("Enabled", false);
            this.ReviewAppSAR.Active.SetItemValue("Enabled", false);
            this.ApproveAppSAR.Active.SetItemValue("Enabled", false);
            this.RejectAppSAR.Active.SetItemValue("Enabled", false);
            this.SARCopyToSA.Active.SetItemValue("Enabled", false);
            this.ApproveAppSAR_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "StockAdjustmentRequests_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSAR.Active.SetItemValue("Enabled", true);
                    this.CancelSAR.Active.SetItemValue("Enabled", true);
                    //this.PreviewSAR.Active.SetItemValue("Enabled", true);
                    this.SARCopyToSA.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSAR.Active.SetItemValue("Enabled", false);
                    this.CancelSAR.Active.SetItemValue("Enabled", false);
                    this.PreviewSAR.Active.SetItemValue("Enabled", false);
                    this.SARCopyToSA.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.SARInquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SARInquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "StockAdjustmentRequests_ListView_Approval")
            {
                //this.ReviewAppSAR.Active.SetItemValue("Enabled", true);
                //this.ReviewAppSAR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppSAR.Active.SetItemValue("Enabled", true);
                //this.ApproveAppSAR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppSAR.Active.SetItemValue("Enabled", true);
                //this.RejectAppSAR.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppSAR_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "StockAdjustmentRequests_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppSAR_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppSAR_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SARInquiryItem.Active.SetItemValue("Enabled", false);
                this.SubmitSAR.Active.SetItemValue("Enabled", false);
                this.CancelSAR.Active.SetItemValue("Enabled", false);
                this.PreviewSAR.Active.SetItemValue("Enabled", false);
                this.ReviewAppSAR.Active.SetItemValue("Enabled", false);
                this.ApproveAppSAR.Active.SetItemValue("Enabled", false);
                this.RejectAppSAR.Active.SetItemValue("Enabled", false);
                this.SARCopyToSA.Active.SetItemValue("Enabled", false);
                this.ApproveAppSAR_Pop.Active.SetItemValue("Enabled", false);
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

        private void SARInquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void SARInquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            StockAdjustmentRequests trx = (StockAdjustmentRequests)View.CurrentObject;
            string docprefix = genCon.GetDocPrefix();

            if (trx.DocNum == null)
            {
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.SAR, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            StockAdjustmentRequests sar = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = sar.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.SAR;

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
               DocTypeList.SAR, "True"));

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

        private void SubmitSAR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            StockAdjustmentRequests selectedObject = (StockAdjustmentRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                StockAdjustmentReqDocTrail ds = ObjectSpace.CreateObject<StockAdjustmentReqDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.StockAdjustmentReqDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                #region Get approval
                List<string> ToEmails = new List<string>();
                string emailbody = "";
                string emailsubject = "";
                string emailaddress = "";
                Guid emailuser;
                DateTime emailtime = DateTime.Now;

                string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'StockAdjustmentRequest'";
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

                        emailsubject = "Stock Adjustment Request Approval";
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
                StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitSAR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSAR_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            StockAdjustmentRequests selectedObject = (StockAdjustmentRequests)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            StockAdjustmentReqDocTrail ds = ObjectSpace.CreateObject<StockAdjustmentReqDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.StockAdjustmentReqDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSAR_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewSAR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ReviewAppSAR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void ApproveAppSAR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            StockAdjustmentRequests selectedObject = (StockAdjustmentRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Approved;

            StockAdjustmentReqDocTrail ds = ObjectSpace.CreateObject<StockAdjustmentReqDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.StockAdjustmentReqDocTrail.Add(ds);

            StockAdjustmentReqAppStatus apps = ObjectSpace.CreateObject<StockAdjustmentReqAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.StockAdjustmentReqAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppSAR_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            StockAdjustmentRequests selectedObject = (StockAdjustmentRequests)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            StockAdjustmentReqDocTrail ds = ObjectSpace.CreateObject<StockAdjustmentReqDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.StockAdjustmentReqDocTrail.Add(ds);

            StockAdjustmentReqAppStatus apps = ObjectSpace.CreateObject<StockAdjustmentReqAppStatus>();
            apps.AppStatus = ApprovalStatusType.Rejected;
            apps.AppRemarks = "Rejected";
            selectedObject.StockAdjustmentReqAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void SARCopyToSA_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                StockAdjustmentRequests sar = (StockAdjustmentRequests)View.CurrentObject;
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustments newsa = os.CreateObject<StockAdjustments>();

                if (sar.ReasonCode != null)
                {
                    newsa.ReasonCode = newsa.Session.GetObjectByKey<vwReasonCode>(sar.ReasonCode.ReasonCode);
                }
                newsa.AdjDate = sar.AdjDate;
                newsa.Remarks = sar.Remarks;

                foreach (StockAdjustmentReqDetails dtl in sar.StockAdjustmentReqDetails)
                {
                    StockAdjustmentDetails newsadetails = os.CreateObject<StockAdjustmentDetails>();

                    newsadetails.ItemCode = newsadetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                    newsadetails.ItemDesc = dtl.ItemDesc;
                    newsadetails.UOM = dtl.UOM;
                    newsadetails.CatalogNo = dtl.CatalogNo;
                    if (dtl.Warehouse != null)
                    {
                        newsadetails.Warehouse = newsadetails.Session.GetObjectByKey<vwWarehouse>(dtl.Warehouse.WarehouseCode);
                    }
                    if (dtl.Bin != null)
                    {
                        newsadetails.Bin = newsadetails.Session.GetObjectByKey<vwBin>(dtl.Bin.BinCode);
                    }
                    newsadetails.Quantity = dtl.Quantity;
                    newsadetails.BaseDoc = sar.DocNum;
                    newsadetails.BaseId = dtl.Oid.ToString();
                    newsa.StockAdjustmentDetails.Add(newsadetails);
                }

                foreach (StockAdjustmentReqAttachment dtl in sar.StockAdjustmentReqAttachment)
                {
                    StockAdjustmentAttactment newsaatt = os.CreateObject<StockAdjustmentAttactment>();

                    newsaatt.File = newsaatt.Session.GetObjectByKey<FileData>(dtl.File.Oid);
                    newsaatt.Remarks = dtl.Remarks;

                    newsa.StockAdjustmentAttactment.Add(newsaatt);
                }

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(os, newsa);
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

        private void ApproveAppSAR_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
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
                        foreach (StockAdjustmentRequests dtl in e.SelectedObjects)
                        {
                            IObjectSpace sos = Application.CreateObjectSpace();
                            StockAdjustmentRequests sar = sos.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                            StockAdjustmentReqAppStatus ds = sos.CreateObject<StockAdjustmentReqAppStatus>();
                            ds.StockAdjustmentRequests = sos.GetObjectByKey<StockAdjustmentRequests>(sar.Oid);
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
                            sar.StockAdjustmentReqAppStatus.Add(ds);

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

                            string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sar.Oid + "', 'StockAdjustmentRequests', " + ((int)appstatus);
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
                                        emailsubject = "Stock Adjustment Request Approval";
                                    else if (appstatus == ApprovalStatusType.Rejected)
                                        emailsubject = "Stock Adjustment Request Approval Rejected";

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
                    foreach (StockAdjustmentRequests dtl in e.SelectedObjects)
                    {
                        IObjectSpace sos = Application.CreateObjectSpace();
                        StockAdjustmentRequests sar = sos.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", dtl.Oid));

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

                        StockAdjustmentReqAppStatus ds = sos.CreateObject<StockAdjustmentReqAppStatus>();
                        ds.StockAdjustmentRequests = sos.GetObjectByKey<StockAdjustmentRequests>(sar.Oid);
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
                        sar.StockAdjustmentReqAppStatus.Add(ds);

                        sos.CommitChanges();
                        sos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sar.Oid + "', 'StockAdjustmentRequests', " + ((int)appstatus);
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
                                    emailsubject = "Stock Adjustment Request Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "Stock Adjustment Request Approval Rejected";

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
                        StockAdjustmentRequests trx = tos.FindObject<StockAdjustmentRequests>(new BinaryOperator("Oid", sar.Oid));
                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No Stock Adjustment Request selected.", InformationType.Error);
            }
        }

        private void ApproveAppSAR_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
