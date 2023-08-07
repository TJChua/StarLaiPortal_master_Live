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
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Quotation;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 2023-07-28 add AR Downpayment Cancel ver 1.0.7

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FilterControllers : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public FilterControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            if (View.ObjectTypeInfo.Type == typeof(SalesQuotation))
            {
                if (View.Id == "SalesQuotation_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(PurchaseOrders))
            {
                if (View.Id == "PurchaseOrders_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(PurchaseReturnRequests))
            {
                if (View.Id == "PurchaseReturnRequests_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(SalesReturnRequests))
            {
                if (View.Id == "SalesReturnRequests_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(WarehouseTransferReq))
            {
                if (View.Id == "WarehouseTransferReq_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(StockAdjustmentRequests))
            {
                if (View.Id == "StockAdjustmentRequests_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(SalesRefundRequests))
            {
                if (View.Id == "SalesRefundRequests_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }

            // Start ver 1.0.7
            if (View.ObjectTypeInfo.Type == typeof(ARDownpaymentCancel))
            {
                if (View.Id == "ARDownpaymentCancel_ListView_Approval")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[AppStatus] = ? and Contains([AppUser],?)",
                        2, user.Staff.StaffName);
                }
            }
            // End ver 1.0.7
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
