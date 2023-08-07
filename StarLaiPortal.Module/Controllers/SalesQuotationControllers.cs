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
using DevExpress.Web;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Quotation;
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

// 2023-07-28 block submit if no address for OC and OS ver 1.0.7

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class e : ViewController
    {
        GeneralControllers genCon;
        public e()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.BackToInquiry.Active.SetItemValue("Enabled", false);
            this.CreateSalesOrder.Active.SetItemValue("Enabled", false);
            this.CancelSalesOrder.Active.SetItemValue("Enabled", false);
            this.InquiryItem.Active.SetItemValue("Enabled", false);
            this.DuplicateSQ.Active.SetItemValue("Enabled", false);
            this.ReviewAppSQ.Active.SetItemValue("Enabled", false);
            this.ApproveAppSQ.Active.SetItemValue("Enabled", false);
            this.RejectAppSQ.Active.SetItemValue("Enabled", false);
            this.PreviewSQ.Active.SetItemValue("Enabled", false);
            this.ApproveAppSQ_Pop.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesQuotation_DetailView")
            {
                //this.BackToInquiry.Active.SetItemValue("Enabled", true);
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.CreateSalesOrder.Active.SetItemValue("Enabled", true);
                    this.CancelSalesOrder.Active.SetItemValue("Enabled", true);
                    this.DuplicateSQ.Active.SetItemValue("Enabled", true);
                    this.PreviewSQ.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.CreateSalesOrder.Active.SetItemValue("Enabled", false);
                    this.CancelSalesOrder.Active.SetItemValue("Enabled", false);
                    this.DuplicateSQ.Active.SetItemValue("Enabled", false);
                    this.PreviewSQ.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.InquiryItem.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.InquiryItem.Active.SetItemValue("Enabled", false);
                }
            }
            else if (View.Id == "SalesQuotation_ListView_Approval")
            {
                this.ReviewAppSQ.Active.SetItemValue("Enabled", true);
                this.ReviewAppSQ.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.ApproveAppSQ.Active.SetItemValue("Enabled", true);
                //this.ApproveAppSQ.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                //this.RejectAppSQ.Active.SetItemValue("Enabled", true);
                //this.RejectAppSQ.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                this.ApproveAppSQ_Pop.Active.SetItemValue("Enabled", true);
            }
            else if (View.Id == "SalesQuotation_DetailView_Approval")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.ApproveAppSQ_Pop.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ApproveAppSQ_Pop.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                //this.BackToInquiry.Active.SetItemValue("Enabled", false);
                this.CreateSalesOrder.Active.SetItemValue("Enabled", false);
                this.CancelSalesOrder.Active.SetItemValue("Enabled", false);
                this.InquiryItem.Active.SetItemValue("Enabled", false);
                this.DuplicateSQ.Active.SetItemValue("Enabled", false);
                this.ReviewAppSQ.Active.SetItemValue("Enabled", false);
                this.ApproveAppSQ.Active.SetItemValue("Enabled", false);
                this.RejectAppSQ.Active.SetItemValue("Enabled", false);
                this.PreviewSQ.Active.SetItemValue("Enabled", false);
                this.ApproveAppSQ_Pop.Active.SetItemValue("Enabled", false);
            }

            if (View.Id == "SalesQuotation_SalesQuotationDetails_ListView")
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
                    object adjustprice = currentObject.GetType().GetProperty("AdjustedPrice").GetValue(currentObject);
                    object price = currentObject.GetType().GetProperty("Price").GetValue(currentObject);

                    if ((decimal)adjustprice < (decimal)price)
                    {
                        showMsg("Warning", "Adjust price lower than original price.", InformationType.Warning);
                    }

                    object validation = currentObject.GetType().GetProperty("IsValid").GetValue(currentObject);

                    if ((bool)validation == true)
                    {
                        showMsg("Error", "Quantity cannot 0.", InformationType.Error);
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


        private void BackToInquiry_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesOrder selectedObject = (SalesOrder)e.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            ItemInquiry trx = os.FindObject<ItemInquiry>(new BinaryOperator("Oid", 1));

            trx.Cart = selectedObject.Cart;
            trx.Search = null;
            trx.PriceList1 = null;
            trx.PriceList2 = null;
            trx.Stock1 = null;
            trx.Stock2 = null;

            for (int i = 0; trx.ItemInquiryDetails.Count > i;)
            {
                trx.ItemInquiryDetails.Remove(trx.ItemInquiryDetails[i]);
            }

            openNewView(os, trx, ViewEditMode.Edit);
        }

        private void CreateSalesOrder_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesQuotation selectedObject = (SalesQuotation)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            IObjectSpace sq = Application.CreateObjectSpace();
            SalesQuotation sqtrx = sq.FindObject<SalesQuotation>(new BinaryOperator("Oid", selectedObject.Oid));

            if (sqtrx.Status == DocStatus.Submitted)
            {
                showMsg("Failed", "Document already submit, please refresh data.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid5 == true)
            {
                showMsg("Failed", "Credit customer not allow use cash series.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid6 == true)
            {
                showMsg("Failed", "Salesperson already inactive.", InformationType.Error);
                return;
            }

            // Start ver 1.0.7
            if (selectedObject.IsValid7 == true)
            {
                showMsg("Failed", "Cash sales billing and shipping address cannot blank.", InformationType.Error);
                return;
            }

            if (selectedObject.IsValid8 == true)
            {
                showMsg("Failed", "Priority cannot be blank.", InformationType.Error);
                return;
            }
            // End ver 1.0.7

            if (selectedObject.IsValid == false)
            {
                if (selectedObject.IsValid1 == true)
                {
                    if (selectedObject.IsValid2 == false)
                    {
                        //if (selectedObject.IsValid3 == false)
                        //{
                            if (selectedObject.IsValid4 == false)
                            {
                                selectedObject.Status = DocStatus.Submitted;

                                SalesQuotationDocTrail ds = ObjectSpace.CreateObject<SalesQuotationDocTrail>();
                                ds.DocStatus = DocStatus.Submitted;
                                ds.DocRemarks = p.ParamString;
                                selectedObject.SalesQuotationDocTrail.Add(ds);

                                ObjectSpace.CommitChanges();
                                ObjectSpace.Refresh();

                                #region Get approval
                                List<string> ToEmails = new List<string>();
                                string emailbody = "";
                                string emailsubject = "";
                                string emailaddress = "";
                                Guid emailuser;
                                DateTime emailtime = DateTime.Now;

                                string getapproval = "EXEC sp_GetApproval '" + selectedObject.CreateUser.Oid + "', '" + selectedObject.Oid + "', 'SalesQuotation'";
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

                                        emailsubject = "Sales Quotation Approval";
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
                                SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("Oid", selectedObject.Oid));

                                if (trx.AppStatus == ApprovalStatusType.Not_Applicable && trx.Status == DocStatus.Submitted)
                                {
                                    #region Add SO
                                    IObjectSpace sos = Application.CreateObjectSpace();
                                    SalesOrder newSO = sos.CreateObject<SalesOrder>();

                                    string docprefix = genCon.GetDocPrefix();
                                    newSO.DocNum = genCon.GenerateDocNum(DocTypeList.SO, sos, TransferType.NA, 0, docprefix);

                                    if (selectedObject.Customer != null)
                                    {
                                        newSO.Customer = newSO.Session.GetObjectByKey<vwBusniessPartner>(selectedObject.Customer.BPCode);
                                    }
                                    newSO.CustomerName = selectedObject.CustomerName;
                                    if (selectedObject.Transporter != null)
                                    {
                                        newSO.Transporter = newSO.Session.GetObjectByKey<vwTransporter>(selectedObject.Transporter.TransporterID);
                                    }
                                    newSO.ContactNo = selectedObject.ContactNo;
                                    if (selectedObject.ContactPerson != null)
                                    {
                                        newSO.ContactPerson = newSO.Session.GetObjectByKey<vwSalesPerson>(selectedObject.ContactPerson.SlpCode);
                                    }
                                    if (selectedObject.PaymentTerm != null)
                                    {
                                        newSO.PaymentTerm = newSO.Session.GetObjectByKey<vwPaymentTerm>(selectedObject.PaymentTerm.GroupNum);
                                    }
                                    if (selectedObject.Series != null)
                                    {
                                        newSO.Series = newSO.Session.GetObjectByKey<vwSeries>(selectedObject.Series.Series);
                                    }
                                    if (selectedObject.Priority != null)
                                    {
                                        newSO.Priority = newSO.Session.GetObjectByKey<PriorityType>(selectedObject.Priority.Oid);
                                    }
                                    if (selectedObject.BillingAddress != null)
                                    {
                                        newSO.BillingAddress = newSO.Session.GetObjectByKey<vwBillingAddress>(selectedObject.BillingAddress.PriKey);
                                    }
                                    newSO.BillingAddressfield = selectedObject.BillingAddressfield;
                                    if (selectedObject.ShippingAddress != null)
                                    {
                                        newSO.ShippingAddress = newSO.Session.GetObjectByKey<vwShippingAddress>(selectedObject.ShippingAddress.PriKey);
                                    }
                                    newSO.ShippingAddressfield = selectedObject.ShippingAddressfield;
                                    newSO.Remarks = selectedObject.Remarks;
                                    newSO.Attn = selectedObject.Attn;
                                    newSO.RefNo = selectedObject.RefNo;

                                    foreach (SalesQuotationDetails dtl in selectedObject.SalesQuotationDetails)
                                    {
                                        SalesOrderDetails newsodetails = sos.CreateObject<SalesOrderDetails>();

                                        newsodetails.ItemCode = newsodetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newsodetails.ItemDesc = dtl.ItemDesc;
                                        newsodetails.Model = dtl.Model;
                                        newsodetails.CatalogNo = dtl.CatalogNo;
                                        if (dtl.Location != null)
                                        {
                                            newsodetails.Location = newsodetails.Session.GetObjectByKey<vwWarehouse>(dtl.Location.WarehouseCode);
                                        }
                                        newsodetails.Quantity = dtl.Quantity;
                                        newsodetails.Price = dtl.Price;
                                        newsodetails.AdjustedPrice = dtl.AdjustedPrice;
                                        newsodetails.BaseDoc = selectedObject.DocNum;
                                        newsodetails.BaseId = dtl.Oid.ToString();
                                        newSO.SalesOrderDetails.Add(newsodetails);
                                    }

                                    sos.CommitChanges();
                                    #endregion
                                }
                                openNewView(os, trx, ViewEditMode.View);
                                showMsg("Successful", "Submit Done.", InformationType.Success);
                            }
                            else
                            {
                                showMsg("Error", "Sales qty not allow over warehouse available qty.", InformationType.Error);
                            }
                        //}
                        //else
                        //{
                        //    showMsg("Error", "Sales quotation row no allow zero amount.", InformationType.Error);
                        //}
                    }
                    else
                    {
                        showMsg("Error", "Please fill in series and contact person.", InformationType.Error);
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

        private void CreateSalesOrder_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewSO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void CancelSalesOrder_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesQuotation selectedObject = (SalesQuotation)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesQuotationDocTrail ds = ObjectSpace.CreateObject<SalesQuotationDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesQuotationDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSalesOrder_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void InquiryItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void InquiryItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            SalesQuotation trx = (SalesQuotation)View.CurrentObject;

            if (trx.DocNum == null)
            {
                string docprefix = genCon.GetDocPrefix();
                trx.DocNum = genCon.GenerateDocNum(DocTypeList.SQ, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            if (trx.Series != null)
            {
                if (trx.Series.SeriesName == "Dropship")
                {
                    showMsg("Warning", "Please change Shipping Address.", InformationType.Warning);
                }
            }
            IObjectSpace os = Application.CreateObjectSpace();
            SalesQuotation sq = os.FindObject<SalesQuotation>(new BinaryOperator("Oid", trx.Oid));

            IObjectSpace inqos = Application.CreateObjectSpace();
            ItemInquiry addnew = inqos.CreateObject<ItemInquiry>();

            DetailView dv = Application.CreateDetailView(inqos, addnew, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((ItemInquiry)dv.CurrentObject).Cart = sq.DocNum;
            ((ItemInquiry)dv.CurrentObject).DocType = DocTypeList.SQ;
            ((ItemInquiry)dv.CurrentObject).CardCode = ((ItemInquiry)dv.CurrentObject).Session.GetObjectByKey<vwBusniessPartner>
                (trx.Customer.BPCode);

            ItemInquiryDefault defaultdata = inqos.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
             DocTypeList.SQ, "True"));

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

        private void DuplicateSQ_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count == 1)
            {
                try
                {
                    SalesQuotation sq = (SalesQuotation)View.CurrentObject;
                    IObjectSpace os = Application.CreateObjectSpace();
                    SalesQuotation newsq = os.CreateObject<SalesQuotation>();

                    if (sq.Customer != null)
                    {
                        newsq.Customer = newsq.Session.GetObjectByKey<vwBusniessPartner>(sq.Customer.BPCode);
                    }
                    if (sq.Transporter != null)
                    {
                        newsq.Transporter = newsq.Session.GetObjectByKey<vwTransporter>(sq.Transporter.TransporterID);
                    }
                    newsq.ContactNo = sq.ContactNo;
                    if (sq.ContactPerson != null)
                    {
                        newsq.ContactPerson = newsq.Session.GetObjectByKey<vwSalesPerson>(sq.ContactPerson.SlpCode);
                    }
                    if (sq.PaymentTerm != null)
                    {
                        newsq.PaymentTerm = newsq.Session.GetObjectByKey<vwPaymentTerm>(sq.PaymentTerm.GroupNum);
                    }
                    if (sq.Series != null)
                    {
                        newsq.Series = newsq.Session.GetObjectByKey<vwSeries>(sq.Series.Series);
                    }
                    if (sq.Priority != null)
                    {
                        newsq.Priority = newsq.Session.GetObjectByKey<PriorityType>(sq.Priority.Oid);
                    }
                    if (sq.BillingAddress != null)
                    {
                        newsq.BillingAddress = newsq.Session.GetObjectByKey<vwBillingAddress>(sq.BillingAddress.PriKey);
                    }
                    newsq.BillingAddressfield = sq.BillingAddressfield;
                    if (sq.ShippingAddress != null)
                    {
                        newsq.ShippingAddress = newsq.Session.GetObjectByKey<vwShippingAddress>(sq.ShippingAddress.PriKey);
                    }
                    newsq.ShippingAddressfield = sq.ShippingAddressfield;
                    newsq.Remarks = sq.Remarks;
                    newsq.Attn = sq.Attn;
                    newsq.RefNo = sq.RefNo;

                    foreach (SalesQuotationDetails dtl in sq.SalesQuotationDetails)
                    {
                        SalesQuotationDetails newsqdetails = os.CreateObject<SalesQuotationDetails>();

                        newsqdetails.ItemCode = newsqdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                        newsqdetails.ItemDesc = dtl.ItemDesc;
                        newsqdetails.Model = dtl.Model;
                        newsqdetails.CatalogNo = dtl.CatalogNo;
                        if (dtl.Location != null)
                        {
                            newsqdetails.Location = newsqdetails.Session.GetObjectByKey<vwWarehouse>(dtl.Location.WarehouseCode);
                        }
                        newsqdetails.Quantity = dtl.Quantity;
                        newsqdetails.Price = dtl.Price;
                        newsqdetails.AdjustedPrice = dtl.AdjustedPrice;
                        newsq.SalesQuotationDetails.Add(newsqdetails);
                    }

                    ShowViewParameters svp = new ShowViewParameters();
                    DetailView dv = Application.CreateDetailView(os, newsq);
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

        private void ReviewAppSQ_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            SalesQuotation sq = (SalesQuotation)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\QuotationApprReview.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", sq.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + sq.Oid + "_" + user.UserName + "_SQAPP_"
                    + DateTime.Parse(sq.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + sq.Oid + "_" + user.UserName + "_SQAPP_"
                    + DateTime.Parse(sq.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }

        private void ApproveAppSQ_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesQuotation selectedObject = (SalesQuotation)e.CurrentObject;

            #region Add SO
            IObjectSpace sos = Application.CreateObjectSpace();
            SalesOrder newSO = sos.CreateObject<SalesOrder>();

            string docprefix = genCon.GetDocPrefix();
            newSO.DocNum = genCon.GenerateDocNum(DocTypeList.SO, sos, TransferType.NA, 0, docprefix);

            if (selectedObject.Customer != null)
            {
                newSO.Customer = newSO.Session.GetObjectByKey<vwBusniessPartner>(selectedObject.Customer.BPCode);
            }
            newSO.CustomerName = selectedObject.CustomerName;
            if (selectedObject.Transporter != null)
            {
                newSO.Transporter = newSO.Session.GetObjectByKey<vwTransporter>(selectedObject.Transporter.TransporterID);
            }
            newSO.ContactNo = selectedObject.ContactNo;
            if (selectedObject.ContactPerson != null)
            {
                newSO.ContactPerson = newSO.Session.GetObjectByKey<vwSalesPerson>(selectedObject.ContactPerson.SlpCode);
            }
            if (selectedObject.PaymentTerm != null)
            {
                newSO.PaymentTerm = newSO.Session.GetObjectByKey<vwPaymentTerm>(selectedObject.PaymentTerm.GroupNum);
            }
            if (selectedObject.Series != null)
            {
                newSO.Series = newSO.Session.GetObjectByKey<vwSeries>(selectedObject.Series.Series);
            }
            if (selectedObject.Priority != null)
            {
                newSO.Priority = newSO.Session.GetObjectByKey<PriorityType>(selectedObject.Priority.Oid);
            }
            if (selectedObject.BillingAddress != null)
            {
                newSO.BillingAddress = newSO.Session.GetObjectByKey<vwBillingAddress>(selectedObject.BillingAddress.PriKey);
            }
            newSO.BillingAddressfield = selectedObject.BillingAddressfield;
            if (selectedObject.ShippingAddress != null)
            {
                newSO.ShippingAddress = newSO.Session.GetObjectByKey<vwShippingAddress>(selectedObject.ShippingAddress.PriKey);
            }
            newSO.ShippingAddressfield = selectedObject.ShippingAddressfield;
            newSO.Remarks = selectedObject.Remarks;

            foreach (SalesQuotationDetails dtl in selectedObject.SalesQuotationDetails)
            {
                SalesOrderDetails newsodetails = sos.CreateObject<SalesOrderDetails>();

                newsodetails.ItemCode = newsodetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                newsodetails.ItemDesc = dtl.ItemDesc;
                newsodetails.Model = dtl.Model;
                newsodetails.CatalogNo = dtl.CatalogNo;
                if (dtl.Location != null)
                {
                    newsodetails.Location = newsodetails.Session.GetObjectByKey<vwWarehouse>(dtl.Location.WarehouseCode);
                }
                newsodetails.Quantity = dtl.Quantity;
                newsodetails.Price = dtl.Price;
                newsodetails.AdjustedPrice = dtl.AdjustedPrice;
                newsodetails.BaseDoc = selectedObject.DocNum;
                newsodetails.BaseId = dtl.Oid.ToString();
                newSO.SalesOrderDetails.Add(newsodetails);
            }

            sos.CommitChanges();
            #endregion

            selectedObject.Status = DocStatus.Submitted;
            selectedObject.AppStatus = ApprovalStatusType.Approved;

            SalesQuotationDocTrail ds = ObjectSpace.CreateObject<SalesQuotationDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Approved";
            selectedObject.SalesQuotationDocTrail.Add(ds);

            SalesQuotationAppStatus apps = ObjectSpace.CreateObject<SalesQuotationAppStatus>();
            apps.AppStatus = ApprovalStatusType.Approved;
            apps.AppRemarks = "Approved";
            selectedObject.SalesQuotationAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Approve Done.", InformationType.Success);
        }

        private void RejectAppSQ_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SalesQuotation selectedObject = (SalesQuotation)e.CurrentObject;

            selectedObject.AppStatus = ApprovalStatusType.Rejected;

            SalesQuotationDocTrail ds = ObjectSpace.CreateObject<SalesQuotationDocTrail>();
            ds.DocStatus = DocStatus.Submitted;
            ds.DocRemarks = "Rejected";
            selectedObject.SalesQuotationDocTrail.Add(ds);

            SalesQuotationAppStatus apps = ObjectSpace.CreateObject<SalesQuotationAppStatus>();
            apps.AppStatus = ApprovalStatusType.Rejected;
            apps.AppRemarks = "Rejected";
            selectedObject.SalesQuotationAppStatus.Add(apps);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            showMsg("Successful", "Reject Done.", InformationType.Success);
        }

        private void PreviewSQ_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            SalesQuotation sq = (SalesQuotation)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\Quotation.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", sq.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + sq.Oid + "_" + user.UserName + "_SQ_"
                    + DateTime.Parse(sq.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + sq.Oid + "_" + user.UserName + "_SQ_"
                    + DateTime.Parse(sq.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }

        private void ApproveAppSQ_Pop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
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
                        foreach (SalesQuotation dtl in e.SelectedObjects)
                        {
                            IObjectSpace sos = Application.CreateObjectSpace();
                            SalesQuotation sq = sos.FindObject<SalesQuotation>(new BinaryOperator("Oid", dtl.Oid));

                            if (sq.Status == DocStatus.Submitted && sq.AppStatus == ApprovalStatusType.Required_Approval)
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

                                SalesQuotationAppStatus ds = sos.CreateObject<SalesQuotationAppStatus>();
                                ds.PurchaseOrders = sos.GetObjectByKey<SalesQuotation>(sq.Oid);
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
                                sq.SalesQuotationAppStatus.Add(ds);

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

                                string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sq.Oid + "', 'SalesQuotation', " + ((int)appstatus);
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
                                            emailsubject = "Sales Quotation Approval";
                                        else if (appstatus == ApprovalStatusType.Rejected)
                                            emailsubject = "Sales Quotation Approval Rejected";

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

                                IObjectSpace tos = Application.CreateObjectSpace();
                                SalesQuotation trx = tos.FindObject<SalesQuotation>(new BinaryOperator("Oid", dtl.Oid));

                                if (trx.AppStatus == ApprovalStatusType.Approved && trx.Status == DocStatus.Submitted)
                                {
                                    #region Add SO
                                    IObjectSpace aos = Application.CreateObjectSpace();
                                    SalesOrder newSO = aos.CreateObject<SalesOrder>();

                                    string docprefix = genCon.GetDocPrefix();
                                    newSO.DocNum = genCon.GenerateDocNum(DocTypeList.SO, sos, TransferType.NA, 0, docprefix);

                                    if (trx.Customer != null)
                                    {
                                        newSO.Customer = newSO.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                                    }
                                    newSO.CustomerName = trx.CustomerName;
                                    if (trx.Transporter != null)
                                    {
                                        newSO.Transporter = newSO.Session.GetObjectByKey<vwTransporter>(trx.Transporter.TransporterID);
                                    }
                                    newSO.ContactNo = trx.ContactNo;
                                    if (trx.ContactPerson != null)
                                    {
                                        newSO.ContactPerson = newSO.Session.GetObjectByKey<vwSalesPerson>(trx.ContactPerson.SlpCode);
                                    }
                                    if (trx.PaymentTerm != null)
                                    {
                                        newSO.PaymentTerm = newSO.Session.GetObjectByKey<vwPaymentTerm>(trx.PaymentTerm.GroupNum);
                                    }
                                    if (trx.Series != null)
                                    {
                                        newSO.Series = newSO.Session.GetObjectByKey<vwSeries>(trx.Series.Series);
                                    }
                                    if (trx.Priority != null)
                                    {
                                        newSO.Priority = newSO.Session.GetObjectByKey<PriorityType>(trx.Priority.Oid);
                                    }
                                    if (trx.BillingAddress != null)
                                    {
                                        newSO.BillingAddress = newSO.Session.GetObjectByKey<vwBillingAddress>(trx.BillingAddress.PriKey);
                                    }
                                    newSO.BillingAddressfield = trx.BillingAddressfield;
                                    if (trx.ShippingAddress != null)
                                    {
                                        newSO.ShippingAddress = newSO.Session.GetObjectByKey<vwShippingAddress>(trx.ShippingAddress.PriKey);
                                    }
                                    newSO.ShippingAddressfield = trx.ShippingAddressfield;
                                    newSO.Remarks = trx.Remarks;
                                    newSO.Attn = trx.Attn;
                                    newSO.RefNo = trx.RefNo;

                                    foreach (SalesQuotationDetails detail in trx.SalesQuotationDetails)
                                    {
                                        SalesOrderDetails newsodetails = aos.CreateObject<SalesOrderDetails>();

                                        newsodetails.ItemCode = newsodetails.Session.GetObjectByKey<vwItemMasters>(detail.ItemCode.ItemCode);
                                        newsodetails.ItemDesc = detail.ItemDesc;
                                        newsodetails.Model = detail.Model;
                                        newsodetails.CatalogNo = detail.CatalogNo;
                                        if (detail.Location != null)
                                        {
                                            newsodetails.Location = newsodetails.Session.GetObjectByKey<vwWarehouse>(detail.Location.WarehouseCode);
                                        }
                                        newsodetails.Quantity = detail.Quantity;
                                        newsodetails.Price = detail.Price;
                                        newsodetails.AdjustedPrice = detail.AdjustedPrice;
                                        newsodetails.BaseDoc = trx.DocNum;
                                        newsodetails.BaseId = detail.Oid.ToString();
                                        newSO.SalesOrderDetails.Add(newsodetails);
                                    }

                                    aos.CommitChanges();
                                    #endregion
                                }
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
                    foreach (SalesQuotation dtl in e.SelectedObjects)
                    {
                        IObjectSpace sos = Application.CreateObjectSpace();
                        SalesQuotation sq = sos.FindObject<SalesQuotation>(new BinaryOperator("Oid", dtl.Oid));

                        if (sq.Status == DocStatus.Submitted && sq.AppStatus == ApprovalStatusType.Approved)
                        {
                            showMsg("Failed", "Document already approved, please refresh data.", InformationType.Error);
                            return;
                        }

                        if (sq.AppUser != null)
                        {
                            if (!sq.AppUser.Contains(user.Staff.StaffName))
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

                        SalesQuotationAppStatus ds = sos.CreateObject<SalesQuotationAppStatus>();
                        ds.PurchaseOrders = sos.GetObjectByKey<SalesQuotation>(sq.Oid);
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
                        sq.SalesQuotationAppStatus.Add(ds);

                        sos.CommitChanges();
                        sos.Refresh();

                        #region approval

                        List<string> ToEmails = new List<string>();
                        string emailbody = "";
                        string emailsubject = "";
                        string emailaddress = "";
                        Guid emailuser;
                        DateTime emailtime = DateTime.Now;

                        string getapproval = "EXEC sp_Approval '" + user.UserName + "', '" + sq.Oid + "', 'SalesQuotation', " + ((int)appstatus);
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
                                    emailsubject = "Sales Quotation Approval";
                                else if (appstatus == ApprovalStatusType.Rejected)
                                    emailsubject = "Sales Quotation Approval Rejected";

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
                        SalesQuotation trx = tos.FindObject<SalesQuotation>(new BinaryOperator("Oid", dtl.Oid));

                        if (trx.AppStatus == ApprovalStatusType.Approved && trx.Status == DocStatus.Submitted)
                        {
                            #region Add SO
                            IObjectSpace aos = Application.CreateObjectSpace();
                            SalesOrder newSO = aos.CreateObject<SalesOrder>();

                            string docprefix = genCon.GetDocPrefix();
                            newSO.DocNum = genCon.GenerateDocNum(DocTypeList.SO, sos, TransferType.NA, 0, docprefix);

                            if (trx.Customer != null)
                            {
                                newSO.Customer = newSO.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                            }
                            newSO.CustomerName = trx.CustomerName;
                            if (trx.Transporter != null)
                            {
                                newSO.Transporter = newSO.Session.GetObjectByKey<vwTransporter>(trx.Transporter.TransporterID);
                            }
                            newSO.ContactNo = trx.ContactNo;
                            if (trx.ContactPerson != null)
                            {
                                newSO.ContactPerson = newSO.Session.GetObjectByKey<vwSalesPerson>(trx.ContactPerson.SlpCode);
                            }
                            if (trx.PaymentTerm != null)
                            {
                                newSO.PaymentTerm = newSO.Session.GetObjectByKey<vwPaymentTerm>(trx.PaymentTerm.GroupNum);
                            }
                            if (trx.Series != null)
                            {
                                newSO.Series = newSO.Session.GetObjectByKey<vwSeries>(trx.Series.Series);
                            }
                            if (trx.Priority != null)
                            {
                                newSO.Priority = newSO.Session.GetObjectByKey<PriorityType>(trx.Priority.Oid);
                            }
                            if (trx.BillingAddress != null)
                            {
                                newSO.BillingAddress = newSO.Session.GetObjectByKey<vwBillingAddress>(trx.BillingAddress.PriKey);
                            }
                            newSO.BillingAddressfield = trx.BillingAddressfield;
                            if (trx.ShippingAddress != null)
                            {
                                newSO.ShippingAddress = newSO.Session.GetObjectByKey<vwShippingAddress>(trx.ShippingAddress.PriKey);
                            }
                            newSO.ShippingAddressfield = trx.ShippingAddressfield;
                            newSO.Remarks = trx.Remarks;
                            newSO.Attn = trx.Attn;
                            newSO.RefNo = trx.RefNo;

                            foreach (SalesQuotationDetails detail in trx.SalesQuotationDetails)
                            {
                                SalesOrderDetails newsodetails = aos.CreateObject<SalesOrderDetails>();

                                newsodetails.ItemCode = newsodetails.Session.GetObjectByKey<vwItemMasters>(detail.ItemCode.ItemCode);
                                newsodetails.ItemDesc = detail.ItemDesc;
                                newsodetails.Model = detail.Model;
                                newsodetails.CatalogNo = detail.CatalogNo;
                                if (detail.Location != null)
                                {
                                    newsodetails.Location = newsodetails.Session.GetObjectByKey<vwWarehouse>(detail.Location.WarehouseCode);
                                }
                                newsodetails.Quantity = detail.Quantity;
                                newsodetails.Price = detail.Price;
                                newsodetails.AdjustedPrice = detail.AdjustedPrice;
                                newsodetails.BaseDoc = trx.DocNum;
                                newsodetails.BaseId = detail.Oid.ToString();
                                newSO.SalesOrderDetails.Add(newsodetails);
                            }

                            aos.CommitChanges();
                            #endregion
                        }

                        openNewView(tos, trx, ViewEditMode.View);
                        showMsg("Successful", "Approve Done.", InformationType.Success);
                    }
                }
            }
            else
            {
                showMsg("Fail", "No SQ selected.", InformationType.Error);
            }
        }

        private void ApproveAppSQ_Pop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
