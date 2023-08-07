using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation;
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
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using StarLaiPortal.Module.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 2023-07-28 add AR Downpayment cancalletion ver 1.0.7

namespace StarLaiPortal.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WebModificationControllers : WebModificationsController
    {
        GeneralControllers genCon;
        public WebModificationControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("Enabled", false);
            Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
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

        protected override void Save(SimpleActionExecuteEventArgs args)
        {
            if (View.ObjectTypeInfo.Type == typeof(SalesQuotation))
            {
                SalesQuotation CurrObject = (SalesQuotation)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SQ, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                if (CurrObject.Series != null)
                {
                    if (CurrObject.Series.SeriesName == "Dropship")
                    {
                        genCon.showMsg("Warning", "Please change Shipping Address.", InformationType.Warning);
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesOrder))
            {
                SalesOrder CurrObject = (SalesOrder)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SO, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(PickList))
            {
                PickList CurrObject = (PickList)args.CurrentObject;
                bool over = false;
                string overitem = null;

                foreach (PickListDetails dtl in CurrObject.PickListDetails)
                {
                    int pickqty = 0;
                    foreach (PickListDetailsActual dtl2 in CurrObject.PickListDetailsActual)
                    {
                        if (dtl2.PickListDetailOid == dtl.Oid)
                        {
                            pickqty = pickqty + (int)dtl2.PickQty;
                        }
                    }

                    dtl.PickQty = pickqty;

                    if (pickqty > dtl.PlanQty)
                    {
                        over = true;
                        overitem = dtl.ItemCode.ItemCode;
                    }
                }

                if (over == true)
                {
                    showMsg("Error", "Pick qty more than plan qty. Item : " + overitem, InformationType.Error);
                    return;
                }

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.PL, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(PackList))
            {
                PackList CurrObject = (PackList)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.PAL, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(Load))
            {
                Load CurrObject = (Load)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.Load, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(PurchaseOrders))
            {
                PurchaseOrders CurrObject = (PurchaseOrders)args.CurrentObject;
                bool sellingprice = false;
                bool zerototal = false;
                string sellingitem = null;

                if (CurrObject.PurchaseOrderDetails.Sum(s => s.Total) <= 0)
                {
                    zerototal = true;
                }

                foreach (PurchaseOrderDetails dtl in CurrObject.PurchaseOrderDetails)
                {
                    if (dtl.AdjustedPrice > dtl.SellingPrice && dtl.BaseDoc != null)
                    {
                        if (dtl.Series == "BackOrdP" || dtl.Series == "BackOrdS")
                        {
                            sellingprice = true;
                            if (sellingitem == null)
                            {
                                sellingitem = dtl.ItemCode.ItemCode;
                            }
                            else
                            {
                                sellingitem = sellingitem + ", " + dtl.ItemCode.ItemCode;
                            }
                        }
                    }
                }

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.PO, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();

                if (sellingprice == true && zerototal == false)
                {
                    showMsg("Warning", "Item: " + sellingitem + " adjusted price higher than selling price.", InformationType.Warning);
                }

                if (sellingprice == false && zerototal == true)
                {
                    showMsg("Warning", "Document with 0 amount.", InformationType.Warning);
                }

                if (sellingprice == true && zerototal == true)
                {
                    showMsg("Warning", "Item: " + sellingitem + " adjusted price higher than selling price."
                        + System.Environment.NewLine + System.Environment.NewLine +
                        "Document with 0 amount.", InformationType.Warning);
                }
            }
            else if (View.ObjectTypeInfo.Type == typeof(ASN))
            {
                ASN CurrObject = (ASN)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.ASN, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(GRN))
            {
                GRN CurrObject = (GRN)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.GRN, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                foreach (GRNDetails dtl in CurrObject.GRNDetails)
                {
                    if (dtl.ASNBaseDoc != null)
                    {
                        genCon.CloseASN(dtl.ASNBaseDoc, "Copy", ObjectSpace);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(PurchaseReturnRequests))
            {
                PurchaseReturnRequests CurrObject = (PurchaseReturnRequests)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.PRR, ObjectSpace, TransferType.NA, CurrObject.Series.Oid, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(PurchaseReturns))
            {
                PurchaseReturns CurrObject = (PurchaseReturns)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.PR, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                foreach(PurchaseReturnDetails dtl in CurrObject.PurchaseReturnDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.ClosePurchaseReturnReq(dtl.BaseDoc, "Copy", ObjectSpace, CurrObject.Requestor.SlpCode);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesReturnRequests))
            {
                SalesReturnRequests CurrObject = (SalesReturnRequests)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SRR, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesReturns))
            {
                SalesReturns CurrObject = (SalesReturns)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SR, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                foreach (SalesReturnDetails dtl in CurrObject.SalesReturnDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseSalesReturnReq(dtl.BaseDoc, "Copy", ObjectSpace, CurrObject.Salesperson.SlpCode);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(WarehouseTransferReq))
            {
                WarehouseTransferReq CurrObject = (WarehouseTransferReq)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.WTR, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(WarehouseTransfers))
            {
                WarehouseTransfers CurrObject = (WarehouseTransfers)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.WT, ObjectSpace, CurrObject.TransferType, 0, docprefix);
                }

                foreach (WarehouseTransferDetails dtl in CurrObject.WarehouseTransferDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseWarehouseTransferReq(dtl.BaseDoc, "Copy", ObjectSpace);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(StockAdjustmentRequests))
            {
                StockAdjustmentRequests CurrObject = (StockAdjustmentRequests)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SAR, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(StockAdjustments))
            {
                StockAdjustments CurrObject = (StockAdjustments)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SA, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                foreach (StockAdjustmentDetails dtl in CurrObject.StockAdjustmentDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseWarehouseTransferReq(dtl.BaseDoc, "Copy", ObjectSpace);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if(View.ObjectTypeInfo.Type == typeof(SalesQuotationDetails))
            {
                SalesQuotationDetails CurrObject = (SalesQuotationDetails)args.CurrentObject;

                if (CurrObject.AdjustedPrice < CurrObject.Price)
                {
                    genCon.showMsg("Warning", "Adjust price lower than original price.", InformationType.Warning);
                }

                base.Save(args);
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesOrderCollection))
            {
                SalesOrderCollection CurrObject = (SalesOrderCollection)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.ARD, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesRefundRequests))
            {
                SalesRefundRequests CurrObject = (SalesRefundRequests)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SRF, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(SalesRefunds))
            {
                SalesRefunds CurrObject = (SalesRefunds)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.SRefund, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                foreach (SalesRefundDetails dtl in CurrObject.SalesRefundDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        genCon.CloseSalesRefund(dtl.BaseDoc, "Copy", ObjectSpace);
                        break;
                    }
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            else if (View.ObjectTypeInfo.Type == typeof(DeliveryOrder))
            {
                DeliveryOrder CurrObject = (DeliveryOrder)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.DO, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            // Start ver 1.0.7
            else if (View.ObjectTypeInfo.Type == typeof(ARDownpaymentCancel))
            {
                ARDownpaymentCancel CurrObject = (ARDownpaymentCancel)args.CurrentObject;

                base.Save(args);
                if (CurrObject.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    CurrObject.DocNum = genCon.GenerateDocNum(DocTypeList.ARDC, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
            // End ver 1.0.7
            else
            {
                base.Save(args);
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                View.BreakLinksToControls();
                View.CreateControls();
            }
        }
    }
}
