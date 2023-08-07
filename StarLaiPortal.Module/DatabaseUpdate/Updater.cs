using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Print_Module;
using StarLaiPortal.Module.BusinessObjects.Sales_Quotation;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Dashboard;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.GRN;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Load;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.Reports;
using StarLaiPortal.Module.BusinessObjects.Inquiry_View;

namespace StarLaiPortal.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FirstOrDefault<DomainObject1>(u => u.Name == name);
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
#if !RELEASE
            ApplicationUser sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
                sampleUser.UserName = "User";
                // Set a password if the standard authentication type is used
                sampleUser.SetPassword("");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
            }
            PermissionPolicyRole defaultRole = CreateDefaultRole();
            sampleUser.Roles.Add(defaultRole);

            ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
                userAdmin.UserName = "Admin";
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).
                ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
            }
            // If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);
            PermissionPolicyRole PurchaseRole = CreatePurchaseRole();
            PermissionPolicyRole SalesRole = CreateSalesRole();
            PermissionPolicyRole WhsRole = CreateWhsRole();
            PermissionPolicyRole FinanceRole = CreateFinanceRole();
            PermissionPolicyRole SalesApproveRole = CreateSalesApproveRole();
            PermissionPolicyRole PurchaseApproveRole = CreatePurchaseApproveRole();
            PermissionPolicyRole ReturnApproveRole = CreateReturnApproveRole();
            PermissionPolicyRole WhsApproveRole = CreateWhsApproveRole();
            PermissionPolicyRole FinanceApproveRole = CreateFinanceApproveRole();
            PermissionPolicyRole AccessRole = CreateAccessRole();
            PermissionPolicyRole WhsAccessRole = CreateWhsAccessRole();
            PermissionPolicyRole SalesAccessRole = CreateSalesAccessRole();
            PermissionPolicyRole FinanceAccessRole = CreateFinanceAccessRole();
            PermissionPolicyRole PurchaseAccessRole = CreatePurchaseAccessRole();
            ObjectSpace.CommitChanges(); //This line persists created object(s).
#endif
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }

        private PermissionPolicyRole CreateSalesRole()
        {
            PermissionPolicyRole SalesRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "SalesRole"));
            if (SalesRole == null)
            {
                SalesRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                SalesRole.Name = "SalesRole";

                SalesRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                SalesRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module/Items/PrintLabel_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Quotation", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Quotation/Items/SalesQuotation_ListView", SecurityPermissionState.Allow);
              
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order/Items/SalesOrder_ListView", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order/Items/SalesOrderCollection_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order/Items/SalesOrder_ListView", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order/Items/SalesOrderCollection_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Dashboards", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Dashboards/Items/Dashboards_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order/Items/PurchaseOrders_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice/Items/ASN_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturnRequests_ListView", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturns_ListView", SecurityPermissionState.Allow);

                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturnRequests_ListView", SecurityPermissionState.Allow);
                SalesRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturns_ListView", SecurityPermissionState.Allow);

                //Print
                SalesRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SQ
                SalesRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SO
                SalesRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Downpayment
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Item Inquiry
                SalesRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                SalesRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //ASN
                SalesRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Purchase Return 
                SalesRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Purchase Return Req
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Sales Return 
                SalesRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Sales Return Req
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Dashboard
                SalesRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                SalesRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                SalesRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                SalesRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                SalesRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }
            return SalesRole;
        }

        private PermissionPolicyRole CreatePurchaseRole()
        {
            PermissionPolicyRole PurchaseRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "PurchaseRole"));
            if (PurchaseRole == null)
            {
                PurchaseRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                PurchaseRole.Name = "PurchaseRole";

                PurchaseRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                PurchaseRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module/Items/PrintLabel_ListView", SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order/Items/PurchaseOrders_ListView", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order/Items/Purchase Order Approval", SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload/Items/GRN_ListView", SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice/Items/ASN_ListView", SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturnRequests_ListView", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturns_ListView", SecurityPermissionState.Allow);

                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturnRequests_ListView", SecurityPermissionState.Allow);
                PurchaseRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturns_ListView", SecurityPermissionState.Allow);

                //Purchase Order
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Unload
                PurchaseRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //ASN
                PurchaseRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Purchase Return 
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Purchase Return Req
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Sales Return 
                PurchaseRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Sales Return Req
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Dashboard
                PurchaseRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                PurchaseRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                PurchaseRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                PurchaseRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                PurchaseRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return PurchaseRole;
        }

        private PermissionPolicyRole CreateWhsRole()
        {
            PermissionPolicyRole WhsRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "WhsRole"));
            if (WhsRole == null)
            {
                WhsRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                WhsRole.Name = "WhsRole";

                WhsRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                WhsRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Print Module/Items/PrintLabel_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Pick List", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Pick List/Items/PickList_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Pack List", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Pack List/Items/PackList_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Loading Bay", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Loading Bay/Items/Load_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload/Items/GRN_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Unload/Items/GRN_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Advanced Shipment Notice/Items/ASN_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturnRequests_ListView", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/PurchaseReturns_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturnRequests_ListView", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/SalesReturns_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Warehouse Transfer", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Warehouse Transfer/Items/WarehouseTransferReq_ListView", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Warehouse Transfer/Items/WarehouseTransfers_ListView", SecurityPermissionState.Allow);

                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Stock Adjustment", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Stock Adjustment/Items/StockAdjustmentRequests_ListView", SecurityPermissionState.Allow);
                WhsRole.AddNavigationPermission(@"Application/NavigationItems/Items/Stock Adjustment/Items/StockAdjustments_ListView", SecurityPermissionState.Allow);

                //Pick List
                WhsRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Pack List
                WhsRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Loading Bay
                WhsRole.AddTypePermissionsRecursively<Load>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Unload
                WhsRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //ASN
                WhsRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Purchase Return 
                WhsRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Purchase Return Req
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Sales Return 
                WhsRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Sales Return Req
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Warehouse Transfer
                WhsRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Warehouse Transfer Req
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Stock Adjustment
                WhsRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Stock Adjustment Req
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Dashboard
                WhsRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                WhsRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                WhsRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                WhsRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                WhsRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return WhsRole;
        }

        private PermissionPolicyRole CreateFinanceRole()
        {
            PermissionPolicyRole FinanceRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "FinanceRole"));
            if (FinanceRole == null)
            {
                FinanceRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                FinanceRole.Name = "FinanceRole";

                FinanceRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                FinanceRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                FinanceRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order", SecurityPermissionState.Allow);
                FinanceRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Order/Items/SalesOrderCollection_ListView", SecurityPermissionState.Allow);

                FinanceRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Refund", SecurityPermissionState.Allow);
                FinanceRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Refund/Items/SalesRefundRequests_ListView", SecurityPermissionState.Allow);

                //AR Downpayment
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Sales Refund
                FinanceRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);

                FinanceRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //Dashboard
                FinanceRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                FinanceRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                FinanceRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                FinanceRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                FinanceRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return FinanceRole;
        }

        private PermissionPolicyRole CreateSalesApproveRole()
        {
            PermissionPolicyRole SalesApproveRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "SalesApproveRole"));
            if (SalesApproveRole == null)
            {
                SalesApproveRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                SalesApproveRole.Name = "SalesApproveRole";

                SalesApproveRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                SalesApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                SalesApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Quotation", SecurityPermissionState.Allow);
                SalesApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Quotation/Items/Sales Quotation Approval", SecurityPermissionState.Allow);

                //SQ
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                SalesApproveRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                SalesApproveRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                SalesApproveRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                SalesApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                SalesApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return SalesApproveRole;
        }

        private PermissionPolicyRole CreatePurchaseApproveRole()
        {
            PermissionPolicyRole PurchaseApproveRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "PurchaseApproveRole"));
            if (PurchaseApproveRole == null)
            {
                PurchaseApproveRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                PurchaseApproveRole.Name = "PurchaseApproveRole";

                PurchaseApproveRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                PurchaseApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order", SecurityPermissionState.Allow);
                PurchaseApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Order/Items/Purchase Order Approval", SecurityPermissionState.Allow);

                //Purchase Order
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                PurchaseApproveRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                PurchaseApproveRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                PurchaseApproveRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                PurchaseApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                PurchaseApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return PurchaseApproveRole;
        }

        private PermissionPolicyRole CreateReturnApproveRole()
        {
            PermissionPolicyRole ReturnApproveRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "ReturnApproveRole"));
            if (ReturnApproveRole == null)
            {
                ReturnApproveRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                ReturnApproveRole.Name = "ReturnApproveRole";

                ReturnApproveRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                ReturnApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                ReturnApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                ReturnApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return", SecurityPermissionState.Allow);
                ReturnApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Purchase Return/Items/Purchase Return Request - Approval", SecurityPermissionState.Allow);

                ReturnApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return", SecurityPermissionState.Allow);
                ReturnApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Return/Items/Sales Return Request - Approval", SecurityPermissionState.Allow);

                //Purchase Return Req
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Sales Return Req
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //Setup
                ReturnApproveRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                ReturnApproveRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                ReturnApproveRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                ReturnApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                ReturnApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return ReturnApproveRole;
        }

        private PermissionPolicyRole CreateWhsApproveRole()
        {
            PermissionPolicyRole WhsApproveRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "WhsApproveRole"));
            if (WhsApproveRole == null)
            {
                WhsApproveRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                WhsApproveRole.Name = "WhsApproveRole";

                WhsApproveRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                WhsApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                WhsApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Warehouse Transfer", SecurityPermissionState.Allow);
                WhsApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Warehouse Transfer/Items/Warehouse Transfer Request - Approval", SecurityPermissionState.Allow);

                WhsApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Stock Adjustment", SecurityPermissionState.Allow);
                WhsApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Stock Adjustment/Items/Stock Adjustment Request - Approval", SecurityPermissionState.Allow);

                //Warehouse Transfer Req
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Stock Adjustment Req
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //Setup
                WhsApproveRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                WhsApproveRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                WhsApproveRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                WhsApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                WhsApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return WhsApproveRole;
        }

        private PermissionPolicyRole CreateFinanceApproveRole()
        {
            PermissionPolicyRole FinanceApproveRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "FinanceApproveRole"));
            if (FinanceApproveRole == null)
            {
                FinanceApproveRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                FinanceApproveRole.Name = "FinanceApproveRole";

                FinanceApproveRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                FinanceApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceApproveRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                FinanceApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Refund", SecurityPermissionState.Allow);
                FinanceApproveRole.AddNavigationPermission(@"Application/NavigationItems/Items/Sales Refund/Items/Sales Refund - Approval", SecurityPermissionState.Allow);

                //Sales Refund
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
            
                //Setup
                FinanceApproveRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);

                //SAP
                FinanceApproveRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //SystemUsers
                FinanceApproveRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                FinanceApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                FinanceApproveRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
            }

            return FinanceApproveRole;
        }

        private PermissionPolicyRole CreateAccessRole()
        {
            PermissionPolicyRole AccessRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "AccessRole"));
            if (AccessRole == null)
            {
                AccessRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                AccessRole.Name = "AccessRole";

                AccessRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                AccessRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                AccessRole.AddMemberPermission<ApplicationUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);

                //SystemUsers
                AccessRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Delivery Order
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //GRN
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Item Inquiry
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Load
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PackList
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PickList
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PrintModule
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Return
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Report
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order Collection
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Quotation
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Refund
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Return
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Setup
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Adjustment
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Warehouse Transfer
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                //AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SAP
                AccessRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLastPurchasePrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLoadingSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLocalBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceWithVolumeDiscount>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Inquiry View
                AccessRole.AddTypePermissionsRecursively<vwInquiryARDownpayment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryDelivery>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryGRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoading>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoadingDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryOpenPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseReturn>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalancebyWhs>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockMovement>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockReordering>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryWarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Audit Trail
                AccessRole.AddTypePermissionsRecursively<AuditDataItemPersistent>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AuditedObjectWeakReference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }

            return AccessRole;
        }

        private PermissionPolicyRole CreateWhsAccessRole()
        {
            PermissionPolicyRole AccessRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "WhsAccessRole"));
            if (AccessRole == null)
            {
                AccessRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                AccessRole.Name = "WhsAccessRole";

                //SystemUsers
                AccessRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Delivery Order
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //GRN
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Item Inquiry
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Load
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PackList
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PickList
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PrintModule
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Return
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Report
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order Collection
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Quotation
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Refund
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Return
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Setup
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Adjustment
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Warehouse Transfer
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SAP
                AccessRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLastPurchasePrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLoadingSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLocalBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceWithVolumeDiscount>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Inquiry View
                AccessRole.AddTypePermissionsRecursively<vwInquiryARDownpayment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryDelivery>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryGRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoading>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoadingDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryOpenPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseReturn>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalancebyWhs>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockMovement>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockReordering>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryWarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Audit Trail
                AccessRole.AddTypePermissionsRecursively<AuditDataItemPersistent>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AuditedObjectWeakReference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }

            return AccessRole;
        }

        private PermissionPolicyRole CreateSalesAccessRole()
        {
            PermissionPolicyRole AccessRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "SalesAccessRole"));
            if (AccessRole == null)
            {
                AccessRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                AccessRole.Name = "SalesAccessRole";

                //SystemUsers
                AccessRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Delivery Order
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //GRN
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Item Inquiry
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Load
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PackList
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PickList
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PrintModule
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Return
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Report
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order Collection
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Quotation
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Refund
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Return
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Setup
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Adjustment
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Warehouse Transfer
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SAP
                AccessRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLastPurchasePrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLoadingSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLocalBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceWithVolumeDiscount>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Inquiry View
                AccessRole.AddTypePermissionsRecursively<vwInquiryARDownpayment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryDelivery>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryGRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoading>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoadingDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryOpenPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseReturn>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalancebyWhs>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockMovement>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockReordering>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryWarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Audit Trail
                AccessRole.AddTypePermissionsRecursively<AuditDataItemPersistent>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AuditedObjectWeakReference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }

            return AccessRole;
        }

        private PermissionPolicyRole CreateFinanceAccessRole()
        {
            PermissionPolicyRole AccessRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "FinanceAccessRole"));
            if (AccessRole == null)
            {
                AccessRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                AccessRole.Name = "FinanceAccessRole";

                //SystemUsers
                AccessRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Delivery Order
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //GRN
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Item Inquiry
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Load
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PackList
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PickList
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PrintModule
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Return
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Report
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order Collection
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Quotation
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Refund
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Return
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Setup
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Adjustment
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Warehouse Transfer
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SAP
                AccessRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLastPurchasePrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLoadingSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLocalBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceWithVolumeDiscount>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Inquiry View
                AccessRole.AddTypePermissionsRecursively<vwInquiryARDownpayment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryDelivery>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryGRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoading>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoadingDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryOpenPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseReturn>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalancebyWhs>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockMovement>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockReordering>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryWarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Audit Trail
                AccessRole.AddTypePermissionsRecursively<AuditDataItemPersistent>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AuditedObjectWeakReference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }

            return AccessRole;
        }

        private PermissionPolicyRole CreatePurchaseAccessRole()
        {
            PermissionPolicyRole AccessRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "PurchaseAccessRole"));
            if (AccessRole == null)
            {
                AccessRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                AccessRole.Name = "PurchaseAccessRole";

                //SystemUsers
                AccessRole.AddTypePermissionsRecursively<ApplicationUser>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ASNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //ASN
                AccessRole.AddTypePermissionsRecursively<Dashboards>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Delivery Order
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DeliveryOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //GRN
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRN>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<GRNDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Item Inquiry
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiry>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Load
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Load>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<LoadDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PackList
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AddPickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PackListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PickList
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickList>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDetailsActual>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PickListDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //PrintModule
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabel>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PrintLabelDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Order
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrderDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseOrders>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Purchase Return
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PurchaseReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Report
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockReorderingReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrder>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Order Collection
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollection>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionDocStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesOrderCollectionReport>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Quotation
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotation>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesQuotationDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Refund
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefundRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesRefunds>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Return
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequestDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturnRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<SalesReturns>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Setup
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Approvals>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ApprovalUsers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<BundleType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DiscrepancyReason>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<DocTypes>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<ItemInquiryDefault>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PaymentType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<PriorityType>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Series>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StaffInfo>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<Transporter>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Sales Adjustment
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentAttactment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustmentRequests>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<StockAdjustments>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Warehouse Transfer
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReq>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStage>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAppStatus>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqAttachment>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDetails>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransferReqDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfers>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Write, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<WarehouseTransfersDocTrail>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //SAP
                AccessRole.AddTypePermissionsRecursively<vwASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwASNDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBillingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBin>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwBusniessPartner>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDepartment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwDriver>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwExchangeRate>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwItemMasters>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLastPurchasePrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLoadingSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwLocalBank>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwOpenSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentSOGroup>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentTerm>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPaymentType>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPODetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPOSO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPrice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPriceWithVolumeDiscount>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwPRRPO>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwReasonCode>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSalesPerson>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwSeries>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwShippingAddress>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwTransporter>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwVehicle>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwWarehouse>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //Inquiry View
                AccessRole.AddTypePermissionsRecursively<vwInquiryARDownpayment>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryASN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryDelivery>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryGRN>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryInvoice>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoading>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryLoadingDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryOpenPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPackList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickList>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPickListDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseOrders>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryPurchaseReturn>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrder>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesOrderDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotation>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesQuotationDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquirySalesReturnRequestDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockAdjustmentDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalance>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockBalancebyWhs>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockMovement>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryStockReordering>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<vwInquiryWarehouseTransferDetails>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);

                //File data
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileData>(SecurityOperations.Delete, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Create, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<FileAttachmentBase>(SecurityOperations.Delete, SecurityPermissionState.Allow);

                //Audit Trail
                AccessRole.AddTypePermissionsRecursively<AuditDataItemPersistent>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                AccessRole.AddTypePermissionsRecursively<AuditedObjectWeakReference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }

            return AccessRole;
        }
    }
}
