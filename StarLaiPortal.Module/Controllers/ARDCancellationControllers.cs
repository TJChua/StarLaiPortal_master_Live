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
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System.Data.SqlClient;
using System.Data;
using StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.View;
using DevExpress.XtraPrinting;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ARDCancellationControllers : ViewController
    {
        GeneralControllers genCon;
        public ARDCancellationControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.CopyFromDownpayment.Active.SetItemValue("Enabled", false);
            this.SubmitDPCancel.Active.SetItemValue("Enabled", false);
            this.CancelDPCancel.Active.SetItemValue("Enabled", false);
            this.ApproveAppARDPC_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "ARDownpaymentCancel_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitDPCancel.Active.SetItemValue("Enabled", true);
                    this.CancelDPCancel.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitDPCancel.Active.SetItemValue("Enabled", false);
                    this.CancelDPCancel.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.CopyFromDownpayment.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.CopyFromDownpayment.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "ARDownpaymentCancel_ListView_Approval")
            {
                this.ApproveAppARDPC_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "ARDownpaymentCancel_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppARDPC_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppARDPC_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.CopyFromDownpayment.Active.SetItemValue("Enabled", false);
                this.SubmitDPCancel.Active.SetItemValue("Enabled", false);
                this.CancelDPCancel.Active.SetItemValue("Enabled", false);
                this.ApproveAppARDPC_Pop.Active.SetItemValue("Enabled", false);
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

        private void CopyFromDownpayment_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                try
                {
                    ARDownpaymentCancel ardpc = (ARDownpaymentCancel)View.CurrentObject;

                    foreach (vwDownpaymentINVGroup group in e.PopupWindowViewSelectedObjects)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        vwDownpaymentINVGroup so = os.FindObject<vwDownpaymentINVGroup>(CriteriaOperator.Parse("SAPDocNum = ?", group.SAPDocNum));

                        if (so == null)
                        {
                            showMsg("Error", "Downpayment created CN, please refresh data.", InformationType.Error);
                            return;
                        }

                        IList<vwDownpaymentINV> solist = os.GetObjects<vwDownpaymentINV>
                            (CriteriaOperator.Parse("SAPDocNum = ?", group.SAPDocNum));

                        string invoiceno = null;
                        foreach (vwDownpaymentINV dtl in solist)
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
                                ardpc.ContactPerson = ObjectSpace.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpName = ?", dtl.Salesperson));
                            }

                            ARDownpaymentCancelDetails newardpcitem = ObjectSpace.CreateObject<ARDownpaymentCancelDetails>();

                            newardpcitem.ItemCode = newardpcitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newardpcitem.ItemDesc = dtl.ItemDescrip;
                            if (dtl.WhsCode != null)
                            {
                                newardpcitem.Warehouse = newardpcitem.Session.GetObjectByKey<vwWarehouse>(dtl.WhsCode.WarehouseCode);
                            }
                            newardpcitem.Quantity = dtl.Quantity;
                            newardpcitem.Price = dtl.UnitPrice;
                            newardpcitem.BaseDoc = dtl.BaseEntry.ToString();
                            newardpcitem.BaseId = dtl.BaseLine.ToString();
                            newardpcitem.SODocNum = dtl.SODocNum;
                            newardpcitem.ARDownpaymentDoc = dtl.PortalNum;
                            if (dtl.CardCode != null)
                            {
                                newardpcitem.Customer = newardpcitem.Session.GetObjectByKey<vwBusniessPartner>(dtl.CardCode);
                            }

                            ardpc.ARDownpaymentCancelDetails.Add(newardpcitem);
                        }

                        ardpc.Reference = invoiceno;
                    }

                    if (ardpc.DocNum == null)
                    {
                        string docprefix = genCon.GetDocPrefix();
                        ardpc.DocNum = genCon.GenerateDocNum(DocTypeList.ARDC, ObjectSpace, TransferType.NA, 0, docprefix);
                    }

                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();

                    showMsg("Success", "Copy Success.", InformationType.Success);
                }
                catch (Exception)
                {
                    showMsg("Fail", "Copy Fail.", InformationType.Error);
                }
            }
        }

        private void CopyFromDownpayment_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            ARDownpaymentCancel ardpc = (ARDownpaymentCancel)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwDownpaymentINVGroup));
            var cs = Application.CreateCollectionSource(os, typeof(vwDownpaymentINVGroup), viewId);
            if (ardpc.Customer != null)
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", ardpc.Customer.BPCode);
            }
            else
            {
                cs.Criteria["CardCode"] = new BinaryOperator("CardCode", "");
            }

            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void SubmitDPCancel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ARDownpaymentCancel selectedObject = (ARDownpaymentCancel)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                ARDownpaymentCancellationDocTrail ds = ObjectSpace.CreateObject<ARDownpaymentCancellationDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.ARDownpaymentCancellationDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                #region Get approval
                List<string> ToEmails = new List<string>();
                string emailbody = "";
                string emailsubject = "";
                string emailaddress = "";
                Guid emailuser;
                DateTime emailtime = DateTime.Now;

                string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'ARDownpaymentCancellation'";
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

                        emailsubject = "AR Downpayment Cancellation Approval";
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
                ARDownpaymentCancel trx = os.FindObject<ARDownpaymentCancel>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitDPCancel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelDPCancel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ARDownpaymentCancel selectedObject = (ARDownpaymentCancel)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            ARDownpaymentCancellationDocTrail ds = ObjectSpace.CreateObject<ARDownpaymentCancellationDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.ARDownpaymentCancellationDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            ARDownpaymentCancel trx = os.FindObject<ARDownpaymentCancel>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelDPCancel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void ApproveAppARDPC_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
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
                        foreach (ARDownpaymentCancel dtl in e.SelectedObjects)
                        {
                            IObjectSpace pos = Application.CreateObjectSpace();
                            ARDownpaymentCancel ardpc = pos.FindObject<ARDownpaymentCancel>(new BinaryOperator("Oid", dtl.Oid));

                            if (ardpc.Status == DocStatus.Submitted && ardpc.AppStatus == ApprovalStatusType.Required_Approval)
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

                                ARDownpaymentCancellationAppStatus ds = pos.CreateObject<ARDownpaymentCancellationAppStatus>();
                                ds.ARDownpaymentCancel = pos.GetObjectByKey<ARDownpaymentCancel>(ardpc.Oid);
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
                                ardpc.ARDownpaymentCancellationAppStatus.Add(ds);

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

                                string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + ardpc.Oid + "', 'ARDownpaymentCancellation', " + ((int)appstatus);
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
                                            emailsubject = "AR Downpayment Cancel Approval";
                                        else if (appstatus == ApprovalStatusType.Rejected)
                                            emailsubject = "AR Downpayment Cancel Rejected";

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
                    foreach (ARDownpaymentCancel dtl in e.SelectedObjects)
                    {
                        IObjectSpace pos = Application.CreateObjectSpace();
                        ARDownpaymentCancel ardpc = pos.FindObject<ARDownpaymentCancel>(new BinaryOperator("Oid", dtl.Oid));

                        if (ardpc.Status == DocStatus.PendPost)
                        {
                            showMsg("Failed", "Document already approved, please refresh data.", InformationType.Error);
                            return;
                        }

                        if (ardpc.AppUser != null)
                        {
                            if (!ardpc.AppUser.Contains(user.Staff.StaffName))
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

                        ARDownpaymentCancellationAppStatus ds = pos.CreateObject<ARDownpaymentCancellationAppStatus>();
                        ds.ARDownpaymentCancel = pos.GetObjectByKey<ARDownpaymentCancel>(ardpc.Oid);
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
                        ardpc.ARDownpaymentCancellationAppStatus.Add(ds);

                        pos.CommitChanges();
                        pos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + ardpc.Oid + "', 'ARDownpaymentCancellation', " + ((int)appstatus);
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
                                    emailsubject = "AR Downpayment Cancel Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "AR Downpayment Cancel Rejected";

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
                        ARDownpaymentCancel trx = tos.FindObject<ARDownpaymentCancel>(new BinaryOperator("Oid", ardpc.Oid));
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

        private void ApproveAppARDPC_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
