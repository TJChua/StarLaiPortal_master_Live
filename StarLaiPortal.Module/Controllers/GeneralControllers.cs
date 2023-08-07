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
using DevExpress.Xpo.DB.Helpers;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.Load;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GeneralControllers : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public GeneralControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.ListNewButton.Active.SetItemValue("Enabled", false);

            if (typeof(Approvals).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(Approvals))
                {
                    this.ListNewButton.Active.SetItemValue("Enabled", true);
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            if (View.Id == "Approvals_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.ListNewButton.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.ListNewButton.Active.SetItemValue("Enabled", false);
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

        public string GenerateDocNum(DocTypeList doctype, IObjectSpace os, TransferType transfertype, int series, string companyprefix)
        {
            string DocNum = null;

            if (doctype == DocTypeList.WT)
            {
                DocTypes snumber = os.FindObject<DocTypes>(CriteriaOperator.Parse("BoCode = ? and TransferType = ?", doctype, transfertype));

                if (DocNum == null)
                {
                    DocNum = snumber.BoName + "-" + companyprefix + "-" + snumber.NextDocNum;
                }

                snumber.CurrectDocNum = snumber.NextDocNum;
                snumber.NextDocNum = snumber.NextDocNum + 1;

                os.CommitChanges();

            }
            else
            {
                if (series != 0)
                {
                    DocTypes snumber = os.FindObject<DocTypes>(CriteriaOperator.Parse("BoCode = ? and Series.Oid = ?", doctype, series));

                    if (DocNum == null)
                    {
                        DocNum = snumber.BoName + "-" + companyprefix + "-" + snumber.NextDocNum;
                    }

                    snumber.CurrectDocNum = snumber.NextDocNum;
                    snumber.NextDocNum = snumber.NextDocNum + 1;

                    os.CommitChanges();
                }
                else
                {
                    DocTypes snumber = os.FindObject<DocTypes>(CriteriaOperator.Parse("BoCode = ?", doctype));

                    if (DocNum == null)
                    {
                        DocNum = snumber.BoName + "-" + companyprefix + "-" + snumber.NextDocNum;
                    }

                    snumber.CurrectDocNum = snumber.NextDocNum;
                    snumber.NextDocNum = snumber.NextDocNum + 1;

                    os.CommitChanges();
                }
            }

            return DocNum;
        }

        public string GetDocPrefix()
        {
            string prefix = null;

            SqlConnection conn = new SqlConnection(getConnectionString());

            string getcompany = "SELECT CompanyPrefix FROM [" + ConfigurationManager.AppSettings.Get("CommonTable").ToString() + "]..ODBC WHERE " +
                "DBName = '" + conn.Database + "'";
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            SqlCommand cmd = new SqlCommand(getcompany, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                prefix = reader.GetString(0);
            }

            return prefix;
        }

        public int ClosePurchaseReturnReq(string BaseId, string Action, IObjectSpace os, int requestor)
        {
            PurchaseReturnRequests prr = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", BaseId));

            if (prr != null)
            {
                if (prr.Requestor == null)
                {
                    prr.Requestor = os.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpCode = ?", requestor));
                }
                if (Action == "Copy")
                {
                    prr.CopyTo = true;
                }
                else if (Action == "Close")
                {
                    prr.Status = DocStatus.Closed;
                }
                else
                {
                    prr.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int CloseSalesReturnReq(string BaseId, string Action, IObjectSpace os, int salesperson)
        {
            SalesReturnRequests srr = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", BaseId));

            if (srr != null)
            {
                if (srr.Salesperson == null)
                {
                    srr.Salesperson = os.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpCode = ?", salesperson));
                }
                if (Action == "Copy")
                {
                    srr.CopyTo = true;
                }
                else if (Action == "Close")
                {
                    srr.Status = DocStatus.Closed;
                }
                else
                {
                    srr.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int CloseWarehouseTransferReq(string BaseId, string Action, IObjectSpace os)
        {
            WarehouseTransferReq wtr = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", BaseId));

            if (wtr != null)
            {
                if (Action == "Copy")
                {
                    wtr.CopyTo = true;
                }
                else if (Action == "Close")
                {
                    wtr.Status = DocStatus.Closed;
                }
                else
                {
                    wtr.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int CloseStockAdjustmentReq(string BaseId, string Action, IObjectSpace os)
        {
            StockAdjustmentRequests sar = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", BaseId));

            if (sar != null)
            {
                if (Action == "Copy")
                {
                    sar.CopyTo = true;
                }
                else if (Action == "Close")
                {
                    sar.Status = DocStatus.Closed;
                }
                else
                {
                    sar.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int CloseSalesRefund(string BaseId, string Action, IObjectSpace os)
        {
            SalesRefundRequests srf = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", BaseId));

            if (srf != null)
            {
                if (Action == "Copy")
                {
                    srf.CopyTo = true;
                }
                else if (Action == "Close")
                {
                    srf.Status = DocStatus.Closed;
                }
                else
                {
                    srf.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int CloseASN(string BaseId, string Action, IObjectSpace os)
        {
            ASN asn = os.FindObject<ASN>(new BinaryOperator("DocNum", BaseId));

            if (asn != null)
            {
                if (asn.RefNo == null)
                {
                    asn.RefNo = " ";
                }
                if (Action == "Copy")
                {
                    asn.CopyTo = true;
                }
                else
                {
                    asn.CopyTo = false;
                }
            }

            os.CommitChanges();

            return 1;
        }

        public int SendEmail(string MailSubject, string MailBody, List<string> ToEmails)
        {
            try
            {
                // return 0 = sent nothing
                // return -1 = sent error
                // return 1 = sent successful
                if (!GeneralSettings.EmailSend) return 0;
                if (ToEmails.Count <= 0) return 0;

                MailMessage mailMsg = new MailMessage();

                mailMsg.From = new MailAddress(GeneralSettings.Email, GeneralSettings.EmailName);

                foreach (string ToEmail in ToEmails)
                {
                    mailMsg.To.Add(ToEmail);
                }

                mailMsg.Subject = MailSubject;
                //mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = MailBody;

                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = GeneralSettings.EmailSSL,
                    UseDefaultCredentials = GeneralSettings.EmailUseDefaultCredential,
                    Host = GeneralSettings.EmailHost,
                    Port = int.Parse(GeneralSettings.EmailPort),
                };

                if (Enum.IsDefined(typeof(SmtpDeliveryMethod), GeneralSettings.DeliveryMethod))
                    smtpClient.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), GeneralSettings.DeliveryMethod);

                if (!smtpClient.UseDefaultCredentials)
                {
                    if (string.IsNullOrEmpty(GeneralSettings.EmailHostDomain))
                        smtpClient.Credentials = new NetworkCredential(GeneralSettings.Email, GeneralSettings.EmailPassword);
                    else
                        smtpClient.Credentials = new NetworkCredential(GeneralSettings.Email, GeneralSettings.EmailPassword, GeneralSettings.EmailHostDomain);
                }

                smtpClient.Send(mailMsg);

                mailMsg.Dispose();
                smtpClient.Dispose();

                return 1;
            }
            catch (Exception ex)
            {
                showMsg("Cannot send email", ex.Message, InformationType.Error);
                return -1;
            }
        }

        public string getConnectionString()
        {
            string connectionString = "";

            ConnectionStringParser helper = new ConnectionStringParser(Application.ConnectionString);
            helper.RemovePartByName("xpodatastorepool");
            connectionString = string.Format(helper.GetConnectionString());

            return connectionString;
        }

        private void ListNewButton_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Approvals masterobject = (Approvals)e.CurrentObject;

            foreach (ApplicationUser dtl in e.PopupWindow.View.SelectedObjects)
            {
                bool dup = false;
                foreach (ApprovalUsers user in masterobject.ApprovalUsers)
                {
                    if (user.User.Oid == dtl.Oid)
                    {
                        dup = true;
                    }
                }

                if (dup == false)
                {
                    ApprovalUsers currentobject = new ApprovalUsers(masterobject.Session);
                    currentobject.User = currentobject.Session.FindObject<ApplicationUser>(new BinaryOperator("Oid", dtl.Oid, BinaryOperatorType.Equal));
                    masterobject.ApprovalUsers.Add(currentobject);
                }
            }

            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ObjectSpace.Refresh();
        }

        private void ListNewButton_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            e.View = Application.CreateListView(typeof(ApplicationUser), true);
        }

        public int GenerateDO(string ConnectionString, Load load, IObjectSpace os, string docprefix)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);

                string getpack = "EXEC GenerateDO '" + load.DocNum + "'";
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(getpack, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", reader.GetString(0)));

                    if (so != null)
                    {
                        string picklistnum = null;
                        DeliveryOrder newdelivery = os.CreateObject<DeliveryOrder>();

                        newdelivery.DocNum = GenerateDocNum(DocTypeList.DO, os, TransferType.NA, 0, docprefix);
                        newdelivery.Customer = newdelivery.Session.GetObjectByKey<vwBusniessPartner>(so.Customer.BPCode);
                        newdelivery.CustomerName = so.CustomerName;
                        newdelivery.Status = DocStatus.Submitted;

                        foreach(LoadDetails dtlload in load.LoadDetails)
                        {
                            PackList pl = os.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtlload.PackList));

                            newdelivery.CustomerGroup = pl.CustomerGroup;
                            foreach (PackListDetails dtlpack in pl.PackListDetails)
                            {
                                if (picklistnum != dtlpack.PickListNo)
                                {
                                    PickList picklist = os.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtlpack.PickListNo));

                                    foreach (PickListDetails dtlpick in picklist.PickListDetails)
                                    {
                                        if (dtlpick.SOBaseDoc == so.DocNum)
                                        {
                                            if (dtlpick.PickQty > 0)
                                            {
                                                DeliveryOrderDetails newdeliveryitem = os.CreateObject<DeliveryOrderDetails>();

                                                newdeliveryitem.ItemCode = newdeliveryitem.Session.GetObjectByKey<vwItemMasters>(dtlpick.ItemCode.ItemCode);
                                                newdeliveryitem.Quantity = dtlpick.PickQty;
                                                newdeliveryitem.PackListLine = dtlpick.Oid.ToString();

                                                //foreach (PickListDetailsActual dtlpickactual in picklist.PickListDetailsActual)
                                                //{
                                                //    if (dtlpickactual.FromBin != null && dtlpickactual.ItemCode.ItemCode == dtlpack.ItemCode.ItemCode)
                                                //    {
                                                //        newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlpickactual.FromBin.Warehouse);
                                                //        newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlpickactual.FromBin.BinCode);
                                                //    }
                                                //}

                                                //temporary use picklist from bin
                                                if (dtlload.Bin != null)
                                                {
                                                    newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlload.Bin.Warehouse);
                                                    newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlload.Bin.BinCode);
                                                }

                                                foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                                                {
                                                    if (dtlsales.ItemCode.ItemCode == dtlpick.ItemCode.ItemCode)
                                                    {
                                                        newdeliveryitem.Price = dtlsales.AdjustedPrice;
                                                    }
                                                }

                                                newdeliveryitem.BaseDoc = load.DocNum.ToString();
                                                newdeliveryitem.BaseId = dtlload.Oid.ToString();
                                                newdeliveryitem.SODocNum = reader.GetString(0);
                                                newdeliveryitem.SOBaseID = dtlpick.SOBaseId;
                                                newdeliveryitem.PickListDocNum = dtlpack.PickListNo;

                                                newdelivery.DeliveryOrderDetails.Add(newdeliveryitem);
                                            }
                                        }
                                    }

                                    picklistnum = dtlpack.PickListNo;
                                }
                            }
                        }

                        os.CommitChanges();
                    }
                }
                conn.Close();
            }
            catch(Exception)
            {
                return 0;
            }

            return 1;
        }

        public int GenerateAutoDO(string ConnectionString, PickList picklist, IObjectSpace os, IObjectSpace packos, IObjectSpace loados,
            IObjectSpace deliveryos, string docprefix)
        {
            try
            {
                if (picklist.Transporter != null)
                {
                    if (picklist.Transporter.U_Type == "OC" || picklist.Transporter.U_Type == "OS" || picklist.IsValid3 == true)
                    {
                        #region Add Pack List
                        string gettobin = "SELECT ToBin FROM PickListDetailsActual WHERE PickList = " + picklist.Oid + " GROUP BY ToBin";
                        SqlConnection conn = new SqlConnection(ConnectionString);
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(gettobin, conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        PackList newpack = packos.CreateObject<PackList>();

                        Load newload = loados.CreateObject<Load>();

                        newload.DocNum = GenerateDocNum(DocTypeList.Load, os, TransferType.NA, 0, docprefix);
                        newload.Status = DocStatus.Submitted;
                        if (picklist.Driver != null)
                        {
                            newload.Driver = newload.Session.GetObjectByKey<vwDriver>(picklist.Driver.DriverCode);
                        }
                        if (picklist.Vehicle != null)
                        {
                            newload.Vehicle = newload.Session.GetObjectByKey<vwVehicle>(picklist.Vehicle.VehicleCode);
                        }

                        while (reader.Read())
                        {
                            newpack.DocNum = GenerateDocNum(DocTypeList.PAL, packos, TransferType.NA, 0, docprefix);

                            newpack.PackingLocation = newpack.Session.GetObjectByKey<vwBin>(reader.GetString(0));
                            newpack.Status = DocStatus.Submitted;
                            newpack.CustomerGroup = picklist.CustomerGroup;

                            foreach (PickListDetailsActual dtl in picklist.PickListDetailsActual)
                            {
                                if (dtl.ToBin.BinCode == reader.GetString(0))
                                {
                                    PackListDetails newpackdetails = packos.CreateObject<PackListDetails>();

                                    newpackdetails.ItemCode = newpackdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                    newpackdetails.ItemDesc = dtl.ItemDesc;
                                    newpackdetails.Bundle = newpackdetails.Session.GetObjectByKey<BundleType>(1);
                                    newpackdetails.Quantity = dtl.PickQty;
                                    newpackdetails.PickListNo = picklist.DocNum;
                                    if (dtl.SOTransporter != null)
                                    {
                                        newpackdetails.Transporter = packos.FindObject<vwTransporter>
                                            (CriteriaOperator.Parse("TransporterName = ?", dtl.SOTransporter));
                                    }
                                    newpackdetails.BaseDoc = picklist.DocNum;

                                    //foreach (PickListDetails dtl2 in picklist.PickListDetails)
                                    //{
                                    //    if (dtl2.ItemCode.ItemCode == dtl.ItemCode.ItemCode && dtl2.SOBaseDoc == dtl.SOBaseDoc)
                                    //    {
                                    //        newpackdetails.BaseId = dtl2.Oid.ToString();
                                    //    }
                                    //}
                                    newpackdetails.BaseId = dtl.PickListDetailOid.ToString();

                                    newpack.PackListDetails.Add(newpackdetails);
                                }
                            }

                            packos.CommitChanges();

                            #region Add Load
                            LoadDetails newloaddetails = loados.CreateObject<LoadDetails>();

                            newloaddetails.PackList = newpack.DocNum;
                            newloaddetails.Bundle = newloaddetails.Session.GetObjectByKey<BundleType>(1);
                            newloaddetails.Bin = newloaddetails.Session.GetObjectByKey<vwBin>(reader.GetString(0));
                            if (picklist.Transporter != null)
                            {
                                newloaddetails.Transporter = picklist.Transporter.TransporterName;
                            }
                            newloaddetails.BaseDoc = newpack.DocNum;
                            newloaddetails.BaseId = newpack.Oid.ToString();

                            newload.LoadDetails.Add(newloaddetails);
                            #endregion
                        }
                        conn.Close();
                        #endregion

                        loados.CommitChanges();

                        #region Add Delivery Order
                        string getso = "SELECT SOBaseDoc FROM PickListDetailsActual WHERE PickList = " + picklist.Oid + " GROUP BY SOBaseDoc";
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd1 = new SqlCommand(getso, conn);
                        SqlDataReader reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", reader1.GetString(0)));

                            if (so != null)
                            {
                                DeliveryOrder newdelivery = deliveryos.CreateObject<DeliveryOrder>();

                                newdelivery.DocNum = GenerateDocNum(DocTypeList.DO, deliveryos, TransferType.NA, 0, docprefix);
                                newdelivery.Customer = newdelivery.Session.GetObjectByKey<vwBusniessPartner>(so.Customer.BPCode);
                                newdelivery.CustomerName = so.CustomerName;
                                newdelivery.Status = DocStatus.Submitted;
                                newdelivery.CustomerGroup = picklist.CustomerGroup;

                                foreach (LoadDetails dtlload in newload.LoadDetails)
                                {
                                    //PackList pl = os.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtlload.PackList));

                                    foreach (PackListDetails dtlpack in newpack.PackListDetails)
                                    {
                                        if (dtlpack.Bundle.BundleID == dtlload.Bundle.BundleID)
                                        {
                                            foreach (PickListDetails dtlpick in picklist.PickListDetails)
                                            {
                                                if (dtlpack.ItemCode.ItemCode == dtlpick.ItemCode.ItemCode &&  dtlpick.SOBaseDoc == so.DocNum &&
                                                    dtlpick.Oid.ToString() == dtlpack.BaseId)
                                                {
                                                    if (dtlpick.PickQty > 0)
                                                    {
                                                        DeliveryOrderDetails newdeliveryitem = deliveryos.CreateObject<DeliveryOrderDetails>();

                                                        newdeliveryitem.ItemCode = newdeliveryitem.Session.GetObjectByKey<vwItemMasters>(dtlpack.ItemCode.ItemCode);
                                                        //temporary use picklist from bin
                                                        if (dtlload.Bin != null)
                                                        {
                                                            newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlload.Bin.Warehouse);
                                                            newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlload.Bin.BinCode);
                                                        }

                                                        //foreach (PickListDetailsActual dtlpick in picklist.PickListDetailsActual)
                                                        //{
                                                        //    if (dtlpick.FromBin != null)
                                                        //    {
                                                        //        newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlpick.FromBin.Warehouse);
                                                        //        newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlpick.FromBin.BinCode);
                                                        //    }
                                                        //}

                                                        newdeliveryitem.Quantity = dtlpack.Quantity;

                                                        foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                                                        {
                                                            if (dtlsales.ItemCode.ItemCode == dtlpack.ItemCode.ItemCode)
                                                            {
                                                                newdeliveryitem.Price = dtlsales.AdjustedPrice;
                                                            }
                                                        }
                                                        newdeliveryitem.BaseDoc = newload.DocNum.ToString();
                                                        newdeliveryitem.BaseId = dtlload.Oid.ToString();
                                                        newdeliveryitem.SODocNum = reader1.GetString(0);
                                                        newdeliveryitem.SOBaseID = dtlpick.SOBaseId;
                                                        newdeliveryitem.PickListDocNum = dtlpack.PickListNo;
                                                        newdeliveryitem.PackListLine = dtlpack.Oid.ToString();

                                                        newdelivery.DeliveryOrderDetails.Add(newdeliveryitem);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                deliveryos.CommitChanges();
                            }
                        }
                        conn.Close();
                        #endregion

                        return 2;
                    }
                }
                //else
                //{
                //    if (picklist.IsValid3 == true)
                //    {
                //        #region Add Pack List
                //        string gettobin = "SELECT ToBin FROM PickListDetailsActual WHERE PickList = " + picklist.Oid + " GROUP BY ToBin";
                //        SqlConnection conn = new SqlConnection(ConnectionString);
                //        if (conn.State == ConnectionState.Open)
                //        {
                //            conn.Close();
                //        }
                //        conn.Open();
                //        SqlCommand cmd = new SqlCommand(gettobin, conn);
                //        SqlDataReader reader = cmd.ExecuteReader();

                //        PackList newpack = packos.CreateObject<PackList>();

                //        Load newload = loados.CreateObject<Load>();

                //        newload.DocNum = GenerateDocNum(DocTypeList.Load, os, TransferType.NA, 0, docprefix);
                //        newload.Status = DocStatus.Submitted;
                //        if (picklist.Driver != null)
                //        {
                //            newload.Driver = newload.Session.GetObjectByKey<vwDriver>(picklist.Driver.DriverCode);
                //        }
                //        if (picklist.Vehicle != null)
                //        {
                //            newload.Vehicle = newload.Session.GetObjectByKey<vwVehicle>(picklist.Vehicle.VehicleCode);
                //        }

                //        while (reader.Read())
                //        {
                //            newpack.DocNum = GenerateDocNum(DocTypeList.PAL, packos, TransferType.NA, 0, docprefix);

                //            newpack.PackingLocation = newpack.Session.GetObjectByKey<vwBin>(reader.GetString(0));
                //            newpack.Status = DocStatus.Submitted;
                //            newpack.CustomerGroup = picklist.CustomerGroup;

                //            foreach (PickListDetailsActual dtl in picklist.PickListDetailsActual)
                //            {
                //                if (dtl.ToBin.BinCode == reader.GetString(0))
                //                {
                //                    PackListDetails newpackdetails = packos.CreateObject<PackListDetails>();

                //                    newpackdetails.ItemCode = newpackdetails.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                //                    newpackdetails.ItemDesc = dtl.ItemDesc;
                //                    newpackdetails.Bundle = newpackdetails.Session.GetObjectByKey<BundleType>(1);
                //                    newpackdetails.Quantity = dtl.PickQty;
                //                    newpackdetails.PickListNo = picklist.DocNum;
                //                    if (dtl.SOTransporter != null)
                //                    {
                //                        newpackdetails.Transporter = packos.FindObject<vwTransporter>
                //                            (CriteriaOperator.Parse("TransporterName = ?", dtl.SOTransporter));
                //                    }
                //                    newpackdetails.BaseDoc = picklist.DocNum;

                //                    foreach (PickListDetails dtl2 in picklist.PickListDetails)
                //                    {
                //                        if (dtl2.ItemCode.ItemCode == dtl.ItemCode.ItemCode && dtl2.SOBaseDoc == dtl.SOBaseDoc)
                //                        {
                //                            newpackdetails.BaseId = dtl2.Oid.ToString();
                //                        }
                //                    }

                //                    newpack.PackListDetails.Add(newpackdetails);
                //                }
                //            }

                //            packos.CommitChanges();

                //            #region Add Load
                //            LoadDetails newloaddetails = loados.CreateObject<LoadDetails>();

                //            newloaddetails.PackList = newpack.DocNum;
                //            newloaddetails.Bundle = newloaddetails.Session.GetObjectByKey<BundleType>(1);
                //            newloaddetails.Bin = newloaddetails.Session.GetObjectByKey<vwBin>(reader.GetString(0));
                //            if (picklist.Transporter != null)
                //            {
                //                newloaddetails.Transporter = picklist.Transporter.TransporterName;
                //            }
                //            newloaddetails.BaseDoc = newpack.DocNum;
                //            newloaddetails.BaseId = newpack.Oid.ToString();

                //            newload.LoadDetails.Add(newloaddetails);
                //            #endregion
                //        }
                //        conn.Close();
                //        #endregion

                //        loados.CommitChanges();

                //        #region Add Delivery Order
                //        string getso = "SELECT SOBaseDoc FROM PickListDetailsActual WHERE PickList = " + picklist.Oid + " GROUP BY SOBaseDoc";
                //        if (conn.State == ConnectionState.Open)
                //        {
                //            conn.Close();
                //        }
                //        conn.Open();
                //        SqlCommand cmd1 = new SqlCommand(getso, conn);
                //        SqlDataReader reader1 = cmd1.ExecuteReader();
                //        while (reader1.Read())
                //        {
                //            SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", reader1.GetString(0)));

                //            if (so != null)
                //            {
                //                DeliveryOrder newdelivery = deliveryos.CreateObject<DeliveryOrder>();

                //                newdelivery.DocNum = GenerateDocNum(DocTypeList.DO, deliveryos, TransferType.NA, 0, docprefix);
                //                newdelivery.Customer = newdelivery.Session.GetObjectByKey<vwBusniessPartner>(so.Customer.BPCode);
                //                newdelivery.CustomerName = so.CustomerName;
                //                newdelivery.Status = DocStatus.Submitted;
                //                newdelivery.CustomerGroup = picklist.CustomerGroup;

                //                foreach (LoadDetails dtlload in newload.LoadDetails)
                //                {
                //                    //PackList pl = os.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtlload.PackList));

                //                    foreach (PackListDetails dtlpack in newpack.PackListDetails)
                //                    {
                //                        if (dtlpack.Bundle.BundleID == dtlload.Bundle.BundleID)
                //                        {
                //                            DeliveryOrderDetails newdeliveryitem = deliveryos.CreateObject<DeliveryOrderDetails>();

                //                            newdeliveryitem.ItemCode = newdeliveryitem.Session.GetObjectByKey<vwItemMasters>(dtlpack.ItemCode.ItemCode);
                //                            //temporary use picklist from bin
                //                            if (dtlload.Bin != null)
                //                            {
                //                                newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlload.Bin.Warehouse);
                //                                newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlload.Bin.BinCode);
                //                            }

                //                            //foreach (PickListDetailsActual dtlpick in picklist.PickListDetailsActual)
                //                            //{
                //                            //    if (dtlpick.FromBin != null)
                //                            //    {
                //                            //        newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlpick.FromBin.Warehouse);
                //                            //        newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlpick.FromBin.BinCode);
                //                            //    }
                //                            //}

                //                            newdeliveryitem.Quantity = dtlpack.Quantity;

                //                            foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                //                            {
                //                                if (dtlsales.ItemCode.ItemCode == dtlpack.ItemCode.ItemCode)
                //                                {
                //                                    newdeliveryitem.Price = dtlsales.AdjustedPrice;
                //                                }
                //                            }
                //                            newdeliveryitem.BaseDoc = newload.DocNum.ToString();
                //                            newdeliveryitem.BaseId = dtlload.Oid.ToString();
                //                            newdeliveryitem.SODocNum = reader1.GetString(0);
                //                            newdeliveryitem.PickListDocNum = dtlpack.PickListNo;
                //                            newdeliveryitem.PackListLine = dtlpack.Oid.ToString();

                //                            newdelivery.DeliveryOrderDetails.Add(newdeliveryitem);
                //                        }
                //                    }
                //                }

                //                deliveryos.CommitChanges();
                //            }
                //        }
                //        conn.Close();
                //        #endregion

                //        return 2;
                //    }
                //}
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }
    }
}
