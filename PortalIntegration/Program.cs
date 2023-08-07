using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation;
using StarLaiPortal.Module.BusinessObjects.Dashboard;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.GRN;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Load;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection;
using StarLaiPortal.Module.BusinessObjects.Sales_Quotation;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// 2023-07-28 add Downpayment cancellation ver 1.0.7

namespace PortalIntegration
{
    static class Program
    {
        private static System.Threading.Mutex mutex = null;
        [STAThread]
        static void Main()
        {
            const string appName = "Portal Integration";
            bool createdNew;

            mutex = new System.Threading.Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Application.Exit();
                return;
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            RegisterEntities();
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
            string connectionString = ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ConnectionString;
            IObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);

            #region Allow Store Proc
            ((SecuredObjectSpaceProvider)objectSpaceProvider).AllowICommandChannelDoWithSecurityContext = true;
            #endregion

            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            PortalIntegration mainForm = new PortalIntegration(security, objectSpaceProvider);

            mainForm.defuserid = ConfigurationManager.AppSettings["DataSourceUserID"].ToString();
            mainForm.defpassword = ConfigurationManager.AppSettings["DataSourcePassword"].ToString();
            string temp = ConfigurationManager.AppSettings["AutoPostAfterLogin"].ToString().ToUpper();
            if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                mainForm.autopostafterlogin = true;
            else
                mainForm.autopostafterlogin = false;

            temp = "";
            temp = ConfigurationManager.AppSettings["AutoLogin"].ToString().ToUpper();
            if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                mainForm.autologin = true;
            else
                mainForm.autologin = false;

            Application.Run(mainForm);
        }

        private static void RegisterEntities()
        {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();

            XafTypesInfo.Instance.RegisterEntity(typeof(ASN));
            XafTypesInfo.Instance.RegisterEntity(typeof(ASNDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(ASNDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(Dashboards));

            XafTypesInfo.Instance.RegisterEntity(typeof(GRN));
            XafTypesInfo.Instance.RegisterEntity(typeof(GRNDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(GRNDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(ItemInquiry));
            XafTypesInfo.Instance.RegisterEntity(typeof(ItemInquiryDetails));

            XafTypesInfo.Instance.RegisterEntity(typeof(Load));
            XafTypesInfo.Instance.RegisterEntity(typeof(LoadDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(LoadDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(AddPickList));
            XafTypesInfo.Instance.RegisterEntity(typeof(AddPickListDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PackList));
            XafTypesInfo.Instance.RegisterEntity(typeof(PackListDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PackListDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(PickList));
            XafTypesInfo.Instance.RegisterEntity(typeof(PickListAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(PickListDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PickListDetailsActual));
            XafTypesInfo.Instance.RegisterEntity(typeof(PickListDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrders));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrderAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrderAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrderAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrderDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseOrderDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnRequests));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnRequestAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnRequestAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnRequestDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnRequestDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturns));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(PurchaseReturnDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrder));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderDocStatus));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollection));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollectionDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollectionDocStatus));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotation));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotationAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotationAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotationAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotationDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesQuotationDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnRequests));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnRequestAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnRequestAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnRequestDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnRequestDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturns));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesReturnDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentRequests));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentReqAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentReqAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentReqAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentReqDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentReqDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustments));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentDocTrail));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(StockAdjustmentAttactment));

            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReq));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReqAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReqAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReqAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReqDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferReqDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransfers));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransfersDocTrail));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferAttachment));
            XafTypesInfo.Instance.RegisterEntity(typeof(WarehouseTransferDetails));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesRefundRequests));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesRefundReqDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesRefundReqAppStage));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesRefundReqAppStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesRefundReqDocTrail));

            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollection));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollectionDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollectionDocStatus));
            XafTypesInfo.Instance.RegisterEntity(typeof(SalesOrderCollectionDocStatus));

            XafTypesInfo.Instance.RegisterEntity(typeof(DeliveryOrder));
            XafTypesInfo.Instance.RegisterEntity(typeof(DeliveryOrderDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(DeliveryOrderDocTrail));

            // Start ver 1.0.7
            XafTypesInfo.Instance.RegisterEntity(typeof(ARDownpaymentCancel));
            XafTypesInfo.Instance.RegisterEntity(typeof(ARDownpaymentCancelDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(ARDownpaymentCancellationDocTrail));
            // End ver 1.0.7

            //Setup
            XafTypesInfo.Instance.RegisterEntity(typeof(Approvals));
            XafTypesInfo.Instance.RegisterEntity(typeof(ApprovalUsers));
            XafTypesInfo.Instance.RegisterEntity(typeof(BundleType));
            XafTypesInfo.Instance.RegisterEntity(typeof(DiscrepancyReason));
            XafTypesInfo.Instance.RegisterEntity(typeof(DocTypes));
            XafTypesInfo.Instance.RegisterEntity(typeof(ItemInquiryDefault));
            XafTypesInfo.Instance.RegisterEntity(typeof(PaymentType));
            XafTypesInfo.Instance.RegisterEntity(typeof(PriorityType));
            XafTypesInfo.Instance.RegisterEntity(typeof(StaffInfo));
            XafTypesInfo.Instance.RegisterEntity(typeof(Transporter));
            XafTypesInfo.Instance.RegisterEntity(typeof(Series));

            //View
            XafTypesInfo.Instance.RegisterEntity(typeof(vwASN));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwASNDetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwBillingAddress));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwBin));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwBusniessPartner));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwDepartment));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwDriver));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwInvoice));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwItemMasters));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwOpenSO));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPackList));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPaymentSO));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPaymentSOGroup));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPaymentTerm));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPickList));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPO));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPODetails));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPriceList));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPRRPO));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwReasonCode));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwSalesPerson));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwSeries));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwShippingAddress));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwStockBalance));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwTransporter));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwVehicle));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwWarehouse));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPaymentTerm));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPrice));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwPOSO));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwBank));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwLastPurchasePrice));
            XafTypesInfo.Instance.RegisterEntity(typeof(vwLocalBank));

            XafTypesInfo.Instance.RegisterEntity(typeof(ApplicationUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(GeneralSettings));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}
