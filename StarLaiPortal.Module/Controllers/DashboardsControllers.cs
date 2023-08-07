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
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.ExpressApp.Web.Templates.ActionContainers.Menu;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Dashboard;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.GRN;
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
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DashboardsControllers : ViewController<ListView>
    {
        ASPxGridListEditor gridListEditor = null;
        public DashboardsControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.DashboardWarehouse.Active.SetItemValue("Enabled", false);
            this.ViewDoc.Active.SetItemValue("Enabled", false);
            this.ViewDashboardDoc.Active.SetItemValue("Enabled", false);

            if (typeof(Dashboards).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(Dashboards))
                {
                    DashboardWarehouse.Items.Clear();

                    foreach (vwWarehouse warehouse in View.ObjectSpace.CreateCollection(typeof(vwWarehouse), null))
                    {
                        DashboardWarehouse.Items.Add(new ChoiceActionItem(warehouse.WarehouseCode, warehouse.WarehouseCode));
                    }

                    this.DashboardWarehouse.Active.SetItemValue("Enabled", true);
                    DashboardWarehouse.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
                    DashboardWarehouse.CustomizeControl += action_CustomizeControl;

                    //this.ViewDoc.Active.SetItemValue("Enabled", true);
                    //this.ViewDoc.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;

                    this.ViewDashboardDoc.Active.SetItemValue("Enabled", true);
                    this.ViewDashboardDoc.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            gridListEditor = View.Editor as ASPxGridListEditor;
            if (gridListEditor != null && gridListEditor.Grid != null)
            {
                gridListEditor.Grid.HtmlRowPrepared += Grid_HtmlRowPrepared;
            }
        }

        void Grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType == GridViewRowType.Group)
            {
                e.Row.BackColor = Color.FromArgb(228, 241, 254);
                e.Row.ForeColor = Color.Black;

                //string targetColumnName1 = "DashboardType";
                //string board = (string)e.GetValue(targetColumnName1);
                //if (board.ToString().ToUpper() == "INBOUND")
                //{
                //    e.Row.BackColor = Color.FromArgb(255, 0, 0);
                //}
                //else if (board.ToString().ToUpper() == "OUTBOUND")
                //{
                //    e.Row.BackColor = Color.FromArgb(255, 111, 0);
                //}
            }
        }

        protected override void OnDeactivated()
        {
            if (gridListEditor != null && gridListEditor.Grid != null)
            {
                gridListEditor.Grid.HtmlRowPrepared -= Grid_HtmlRowPrepared;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void action_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            SingleChoiceActionAsModeMenuActionItem actionItem = e.Control as SingleChoiceActionAsModeMenuActionItem;
            if (actionItem != null && actionItem.Action.PaintStyle == DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption)
            {
                DropDownSingleChoiceActionControlBase control = (DropDownSingleChoiceActionControlBase)actionItem.Control;
                control.Label.Text = actionItem.Action.Caption;
                control.Label.Style["padding-right"] = "5px";
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

        private void DashboardWarehouse_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (e.SelectedChoiceActionItem.Id != null)
            {
                ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[Warehouse] = ?",
                    e.SelectedChoiceActionItem.Id);
            }
        }

        private void ViewDoc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Dashboards selectedObject = (Dashboards)e.CurrentObject;

            if (selectedObject.TransactionType == DocTypeList.SQ)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.ASN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                ASN trx = os.FindObject<ASN>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PAL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PackList trx = os.FindObject<PackList>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.ARD)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrderCollection trx = os.FindObject<SalesOrderCollection>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.DO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DeliveryOrder trx = os.FindObject<DeliveryOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.GRN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                GRN trx = os.FindObject<GRN>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.Load)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Load trx = os.FindObject<Load>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturns trx = os.FindObject<PurchaseReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SA)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustments trx = os.FindObject<StockAdjustments>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SAR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrder trx = os.FindObject<SalesOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturns trx = os.FindObject<SalesReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRefund)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefunds trx = os.FindObject<SalesRefunds>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRF)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.WT)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransfers trx = os.FindObject<WarehouseTransfers>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.WTR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransferReq trx = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }
        }

        private void ViewDashboardDoc_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Dashboards selectedObject = (Dashboards)e.CurrentObject;

            if (selectedObject.TransactionType == DocTypeList.SQ)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.ASN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                ASN trx = os.FindObject<ASN>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PAL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PackList trx = os.FindObject<PackList>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.ARD)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrderCollection trx = os.FindObject<SalesOrderCollection>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.DO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DeliveryOrder trx = os.FindObject<DeliveryOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.GRN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                GRN trx = os.FindObject<GRN>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.Load)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Load trx = os.FindObject<Load>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.PR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturns trx = os.FindObject<PurchaseReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SA)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustments trx = os.FindObject<StockAdjustments>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SAR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrder trx = os.FindObject<SalesOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturns trx = os.FindObject<SalesReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRefund)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefunds trx = os.FindObject<SalesRefunds>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRF)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.SRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.WT)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransfers trx = os.FindObject<WarehouseTransfers>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }

            if (selectedObject.TransactionType == DocTypeList.WTR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransferReq trx = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", selectedObject.DocNum));
                openNewView(os, trx, ViewEditMode.View);
            }
        }

        private void ViewDashboardDoc_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            Dashboards selectedObject = (Dashboards)View.CurrentObject;

            if (selectedObject.TransactionType == DocTypeList.SQ)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesQuotation_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.ARD)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrderCollection trx = os.FindObject<SalesOrderCollection>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesOrderCollection_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.ASN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                ASN trx = os.FindObject<ASN>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "ASN_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.DO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DeliveryOrder trx = os.FindObject<DeliveryOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "DeliveryOrder_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.GRN)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                GRN trx = os.FindObject<GRN>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "GRN_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.Load)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Load trx = os.FindObject<Load>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "Load_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.PAL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PackList trx = os.FindObject<PackList>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "PackList_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.PL)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "PickList_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.PO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "PurchaseOrders_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.PR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturns trx = os.FindObject<PurchaseReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "PurchaseReturns_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.PRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "PurchaseReturnRequests_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SA)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustments trx = os.FindObject<StockAdjustments>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "StockAdjustments_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SAR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "StockAdjustmentRequests_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SO)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesOrder trx = os.FindObject<SalesOrder>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesOrder_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturns trx = os.FindObject<SalesReturns>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesReturns_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SRefund)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefunds trx = os.FindObject<SalesRefunds>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesRefunds_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SRF)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesRefundRequests_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.SRR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "SalesReturnRequests_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.WT)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransfers trx = os.FindObject<WarehouseTransfers>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "WarehouseTransfers_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }

            if (selectedObject.TransactionType == DocTypeList.WTR)
            {
                IObjectSpace os = Application.CreateObjectSpace();
                WarehouseTransferReq trx = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", selectedObject.DocNum));

                DetailView detailView = Application.CreateDetailView(os, "WarehouseTransferReq_DetailView_Dashboard", true, trx);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
                e.View = detailView;
                e.DialogController.AcceptAction.Caption = "Go To Document";
                e.Maximized = true;
                //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            }
        }
    }
}
