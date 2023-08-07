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
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo.DB;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

// 2023-07-28 add item button do not pop out ver 1.0.7

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ItemInquiryControllers : ViewController
    {
        GeneralControllers genCon;
        public ItemInquiryControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.Search_ItemInquiry.Active.SetItemValue("Enabled", false);
            this.ViewSales.Active.SetItemValue("Enabled", false);
            this.AddToCartPop.Active.SetItemValue("Enabled", false);
            this.OpenCart.Active.SetItemValue("Enabled", false);
            this.ViewItemPicture.Active.SetItemValue("Enabled", false);
            // Start ver 1.0.7
            this.AddToCartSimple.Active.SetItemValue("Enabled", false);
            // End ver 1.0.7

            if (typeof(ItemInquiry).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(ItemInquiry))
                {
                    this.Search_ItemInquiry.Active.SetItemValue("Enabled", true);
                    this.Search_ItemInquiry.ActionMeaning = ActionMeaning.Accept;
                    //this.OpenCart.Active.SetItemValue("Enabled", true);
                }
            }

            if (typeof(ItemInquiryDetails).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(ItemInquiryDetails))
                {
                    if (View.Id == "ItemInquiry_ItemInquiryDetails_ListView")
                    {
                        // Start ver 1.0.7
                        //this.AddToCartPop.Active.SetItemValue("Enabled", true);
                        this.AddToCartSimple.Active.SetItemValue("Enabled", true);
                        // End ver 1.0.7
                    }
                    else
                    {
                        // Start ver 1.0.7
                        //this.AddToCartPop.Active.SetItemValue("Enabled", false);
                        this.AddToCartSimple.Active.SetItemValue("Enabled", false);
                        // End ver 1.0.7
                    }
                    this.ViewSales.Active.SetItemValue("Enabled", true);
                    this.ViewItemPicture.Active.SetItemValue("Enabled", true);
                    this.ViewItemPicture.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                }
            }
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

        private void Search_ItemInquiry_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string cardcode = "";
            ItemInquiry selectedObject = (ItemInquiry)e.CurrentObject;

            if (selectedObject.CardCode != null)
            {
                cardcode = selectedObject.CardCode.BPCode;
            }

            if (selectedObject.PriceList1 != null && selectedObject.PriceList2 != null 
                && selectedObject.PriceList3 != null && selectedObject.PriceList4 != null 
                && selectedObject.Stock1 != null && selectedObject.Stock2 != null)
            {
                //for (int i = 0; selectedObject.ItemInquiryDetails.Count > i;)
                //{
                //    selectedObject.ItemInquiryDetails.Remove(selectedObject.ItemInquiryDetails[i]);
                //}

                XPObjectSpace persistentObjectSpace = (XPObjectSpace)Application.CreateObjectSpace();
                SelectedData sprocData = persistentObjectSpace.Session.ExecuteSproc("sp_GetItem", new OperandValue(selectedObject.Search),
                    new OperandValue(selectedObject.Exclude),
                    new OperandValue(selectedObject.PriceList1.ListNum), new OperandValue(selectedObject.PriceList2.ListNum),
                    new OperandValue(selectedObject.PriceList3.ListNum), new OperandValue(selectedObject.PriceList4.ListNum),
                    new OperandValue(selectedObject.Stock1.WarehouseCode), new OperandValue(selectedObject.Stock2.WarehouseCode),
                    new OperandValue(selectedObject.Method), new OperandValue(cardcode), new OperandValue(selectedObject.Oid));

                //if (sprocData.ResultSet.Count() > 0)
                //{
                //    if (sprocData.ResultSet[0].Rows.Count() > 0)
                //    {
                //        foreach (SelectStatementResultRow row in sprocData.ResultSet[0].Rows)
                //        {
                //            ItemInquiryDetails searchitem = ObjectSpace.CreateObject<ItemInquiryDetails>();
                //            searchitem.ItemCode = row.Values[0].ToString();
                //            searchitem.ItemDesc = row.Values[1].ToString();
                //            searchitem.Model = row.Values[2].ToString();
                //            searchitem.PriceList1 = selectedObject.PriceList1.ListName;
                //            searchitem.Price1 = Convert.ToDecimal(row.Values[3]);
                //            searchitem.PriceList2 = selectedObject.PriceList2.ListName;
                //            searchitem.Price2 = Convert.ToDecimal(row.Values[4]);
                //            searchitem.StockName1 = selectedObject.Stock1.WarehouseName;
                //            searchitem.Stock1 = Convert.ToDecimal(row.Values[5]);
                //            searchitem.StockName2 = selectedObject.Stock2.WarehouseName;
                //            searchitem.Stock2 = Convert.ToDecimal(row.Values[6]);
                //            searchitem.OriginalCatalog = row.Values[7].ToString();
                //            searchitem.HierarchyLevel1 = row.Values[8].ToString();
                //            searchitem.HierarchyLevel2 = row.Values[9].ToString();
                //            searchitem.HierarchyLevel3 = row.Values[10].ToString();
                //            searchitem.HierarchyLevel4 = row.Values[11].ToString();
                //            searchitem.HierarchyLevel5 = row.Values[12].ToString();
                //            searchitem.Remarks = row.Values[13].ToString();
                //            searchitem.Barcode = row.Values[14].ToString();
                //            searchitem.ForeignName = row.Values[15].ToString();
                //            searchitem.BPCatalogNo = row.Values[16].ToString();

                //            selectedObject.ItemInquiryDetails.Add(searchitem);
                //        }
                //    }
                //}
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
                View.Refresh();
            }
            else
            {
                showMsg("Fail", "PriceList1/2/3/4 and Stock1/2 is empty.", InformationType.Error);
            }
        }

        private void ViewSales_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }

        private void ViewSales_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            ItemInquiryDetails iteminquiry = (ItemInquiryDetails)View.CurrentObject;

            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SalesHistory));

            XPObjectSpace persistentObjectSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var nonPersistentOS = Application.CreateObjectSpace(typeof(SalesHistoryList));
            SalesHistoryList saleslist = nonPersistentOS.CreateObject<SalesHistoryList>();

            if (iteminquiry != null)
            {
                SelectedData sprocData = persistentObjectSpace.Session.ExecuteSproc("sp_GetSales", new OperandValue(iteminquiry.ItemCode));

                int i = 1;

                if (sprocData.ResultSet.Count() > 0)
                {
                    if (sprocData.ResultSet[0].Rows.Count() > 0)
                    {
                        foreach (SelectStatementResultRow row in sprocData.ResultSet[0].Rows)
                        {
                            var itemos = Application.CreateObjectSpace(typeof(SalesHistory));
                            var item = itemos.CreateObject<SalesHistory>();
                            item.Id = i;
                            item.No = i;
                            item.Customer = row.Values[1].ToString();
                            item.SalesDate = (DateTime)row.Values[2];
                            item.Quantity = (decimal)row.Values[3];
                            item.UnitPrice = (decimal)row.Values[4];
                            item.SAPInvoiceNo = row.Values[5].ToString();
                            saleslist.Sales.Add(item);

                            i++;
                        }
                    }
                }
            }

            nonPersistentOS.CommitChanges();

            DetailView detailView = Application.CreateDetailView(nonPersistentOS, saleslist);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            if (iteminquiry != null)
            {
                ((SalesHistoryList)detailView.CurrentObject).ItemCode = iteminquiry.ItemCode + " - " + iteminquiry.ItemDesc;
            }
            e.View = detailView;
            e.DialogController.SaveOnAccept = false;
            e.DialogController.CancelAction.Active["NothingToCancel"] = false;
        }

        private void OpenCart_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ItemInquiry selectedObject = (ItemInquiry)e.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            SalesOrder trx = os.FindObject<SalesOrder>(new BinaryOperator("Cart", selectedObject.Cart));

            if (trx != null)
            {
                openNewView(os, trx, ViewEditMode.Edit);
            }
            else
            {
                showMsg("Error", "Cart not found.", InformationType.Error);
            }
        }

        private void AddToCartPop_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            //ItemInquiryDetails selectedObject = (ItemInquiryDetails)e.CurrentObject;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            int oid = 0;

            //if (selectedObject == null)
            //{
                foreach (ItemInquiryDetails dtl in e.SelectedObjects)
                {
                    oid = dtl.ItemInquiry.Oid;
                    if (dtl.ItemInquiry.DocType == DocTypeList.SQ)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        SalesQuotation newSQ = cos.CreateObject<SalesQuotation>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                SalesQuotationDetails newSQdetail = os.CreateObject<SalesQuotationDetails>();
                                newSQdetail.Postingdate = trx.PostingDate;
                                newSQdetail.Customer = newSQdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                                newSQdetail.Location = newSQdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newSQdetail.ItemCode = newSQdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSQdetail.ItemDesc = dtl.ItemDesc;
                                newSQdetail.Model = dtl.Model;
                                newSQdetail.CatalogNo = dtl.OriginalCatalog;
                                //newSQdetail.Price = dtl.Price1;
                                //newSQdetail.AdjustedPrice = dtl.Price1;
                                trx.SalesQuotationDetails.Add(newSQdetail);
                            }
                            else
                            {
                                newSQ.Priority = newSQ.Session.FindObject<PriorityType>
                                    (CriteriaOperator.Parse("PriorityName = ? and IsActive= ?",
                                    "Normal", "True"));

                                SalesQuotationDetails newSQdetail = cos.CreateObject<SalesQuotationDetails>();
                                newSQdetail.ItemCode = newSQdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSQdetail.ItemDesc = dtl.ItemDesc;
                                newSQdetail.Model = dtl.Model;
                                newSQdetail.CatalogNo = dtl.OriginalCatalog;
                                newSQdetail.Location = newSQdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                //newSQdetail.Price = dtl.Price1;
                                //newSQdetail.AdjustedPrice = dtl.Price1;
                                newSQ.SalesQuotationDetails.Add(newSQdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Sales Quoatation.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.PO)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        PurchaseOrders newPO = cos.CreateObject<PurchaseOrders>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                PurchaseOrderDetails newPOdetail = os.CreateObject<PurchaseOrderDetails>();
                                newPOdetail.Postingdate = trx.PostingDate;
                                newPOdetail.Supplier = newPOdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Supplier.BPCode);
                                newPOdetail.ItemCode = newPOdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newPOdetail.ItemDesc = dtl.ItemDesc;
                                newPOdetail.Model = dtl.Model;
                                newPOdetail.Location = newPOdetail.Session.GetObjectByKey<vwWarehouse>(trx.Warehouse.WarehouseCode);
                                newPOdetail.CatalogNo = dtl.OriginalCatalog;
                                //newPOdetail.Price = dtl.Price1;
                                //newPOdetail.AdjustedPrice = dtl.Price1;
                                trx.PurchaseOrderDetails.Add(newPOdetail);
                            }
                            else
                            {
                                PurchaseOrderDetails newPOdetail = cos.CreateObject<PurchaseOrderDetails>();
                                newPOdetail.ItemCode = newPOdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newPOdetail.ItemDesc = dtl.ItemDesc;
                                newPOdetail.Model = dtl.Model;
                                newPOdetail.Location = newPOdetail.Session.GetObjectByKey<vwWarehouse>(trx.Warehouse.WarehouseCode);
                                newPOdetail.CatalogNo = dtl.OriginalCatalog;
                                //newPOdetail.Price = dtl.Price1;
                                //newPOdetail.AdjustedPrice = dtl.Price1;
                                newPO.PurchaseOrderDetails.Add(newPOdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Purchase Order.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.PRR)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        PurchaseReturnRequests newprr = cos.CreateObject<PurchaseReturnRequests>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                PurchaseReturnRequestDetails newPRRdetail = os.CreateObject<PurchaseReturnRequestDetails>();
                                newPRRdetail.ItemCode = newPRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newPRRdetail.ItemDesc = dtl.ItemDesc;
                                newPRRdetail.UOM = newPRRdetail.ItemCode.UOM;
                                newPRRdetail.Warehouse = newPRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newPRRdetail.Price = dtl.Price1;
                                trx.PurchaseReturnRequestDetails.Add(newPRRdetail);
                            }
                            else
                            {
                                PurchaseReturnRequestDetails newPRRdetail = cos.CreateObject<PurchaseReturnRequestDetails>();
                                newPRRdetail.ItemCode = newPRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newPRRdetail.ItemDesc = dtl.ItemDesc;
                                newPRRdetail.UOM = newPRRdetail.ItemCode.UOM;
                                newPRRdetail.Warehouse = newPRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newPRRdetail.Price = dtl.Price1;
                                newprr.PurchaseReturnRequestDetails.Add(newPRRdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Purchase Return Request.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.SRR)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        SalesReturnRequests newsrr = cos.CreateObject<SalesReturnRequests>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                SalesReturnRequestDetails newSRRdetail = os.CreateObject<SalesReturnRequestDetails>();
                                newSRRdetail.ItemCode = newSRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSRRdetail.ItemDesc = dtl.ItemDesc;
                                newSRRdetail.UOM = newSRRdetail.ItemCode.UOM;
                                newSRRdetail.Warehouse = newSRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newSRRdetail.Price = dtl.Price1;
                                trx.SalesReturnRequestDetails.Add(newSRRdetail);
                            }
                            else
                            {
                                SalesReturnRequestDetails newSRRdetail = cos.CreateObject<SalesReturnRequestDetails>();
                                newSRRdetail.ItemCode = newSRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSRRdetail.ItemDesc = dtl.ItemDesc;
                                newSRRdetail.UOM = newSRRdetail.ItemCode.UOM;
                                newSRRdetail.Warehouse = newSRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newSRRdetail.Price = dtl.Price1;
                                newsrr.SalesReturnRequestDetails.Add(newSRRdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Sales Return Request.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.WTR)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        WarehouseTransferReq newwtr = cos.CreateObject<WarehouseTransferReq>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            WarehouseTransferReq trx = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                WarehouseTransferReqDetails newWTRdetail = os.CreateObject<WarehouseTransferReqDetails>();
                                newWTRdetail.FromWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(trx.FromWarehouse.WarehouseCode);
                                newWTRdetail.ToWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(trx.ToWarehouse.WarehouseCode);
                                newWTRdetail.ItemCode = newWTRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newWTRdetail.ItemDesc = dtl.ItemDesc;
                                newWTRdetail.UOM = newWTRdetail.ItemCode.UOM;
                                newWTRdetail.CatalogNo = dtl.OriginalCatalog;
                                trx.WarehouseTransferReqDetails.Add(newWTRdetail);
                            }
                            else
                            {
                                WarehouseTransferReqDetails newWTRdetail = cos.CreateObject<WarehouseTransferReqDetails>();
                                newWTRdetail.FromWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(newwtr.FromWarehouse.WarehouseCode);
                                newWTRdetail.ToWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(newwtr.ToWarehouse.WarehouseCode);
                                newWTRdetail.ItemCode = newWTRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newWTRdetail.ItemDesc = dtl.ItemDesc;
                                newWTRdetail.UOM = newWTRdetail.ItemCode.UOM;
                                newWTRdetail.CatalogNo = dtl.OriginalCatalog;
                                newwtr.WarehouseTransferReqDetails.Add(newWTRdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Warehouse Transfer Request.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.SAR)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        StockAdjustmentRequests newsar = cos.CreateObject<StockAdjustmentRequests>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                StockAdjustmentReqDetails newSARdetail = os.CreateObject<StockAdjustmentReqDetails>();
                                newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSARdetail.ItemDesc = dtl.ItemDesc;
                                newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                                newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newSARdetail.CatalogNo = dtl.OriginalCatalog;
                                trx.StockAdjustmentReqDetails.Add(newSARdetail);
                            }
                            else
                            {
                                StockAdjustmentReqDetails newSARdetail = cos.CreateObject<StockAdjustmentReqDetails>();
                                newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSARdetail.ItemDesc = dtl.ItemDesc;
                                newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                                newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newSARdetail.CatalogNo = dtl.OriginalCatalog;
                                newsar.StockAdjustmentReqDetails.Add(newSARdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Warehouse Transfer Request.", InformationType.Success);
                    }
                    else if (dtl.ItemInquiry.DocType == DocTypeList.SRF)
                    {
                        IObjectSpace cos = Application.CreateObjectSpace();
                        SalesRefundRequests newsfr = cos.CreateObject<SalesRefundRequests>();
                        IObjectSpace os = Application.CreateObjectSpace();

                        if (dtl.ItemInquiry.Cart != null)
                        {
                            SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                            if (trx != null)
                            {
                                SalesRefundReqDetails newSARdetail = os.CreateObject<SalesRefundReqDetails>();
                                newSARdetail.Customer = newSARdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                                newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSARdetail.ItemDesc = dtl.ItemDesc;
                                newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                                newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                trx.SalesRefundReqDetails.Add(newSARdetail);
                            }
                            else
                            {
                                SalesRefundReqDetails newSARdetail = cos.CreateObject<SalesRefundReqDetails>();
                                newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                                newSARdetail.ItemDesc = dtl.ItemDesc;
                                newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                                newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                                newsfr.SalesRefundReqDetails.Add(newSARdetail);
                            }
                        }
                        else
                        {
                            showMsg("Error", "Please specify cart no.", InformationType.Error);
                            return;
                        }

                        os.CommitChanges();

                        showMsg("Success", "Added to Sales Refund Request.", InformationType.Success);
                    }

                    dtl.ItemInquiry.Search = null;
                    dtl.ItemInquiry.Exclude = null;
                    dtl.ItemInquiry.Method = SearchMethod.AND;
                }

                //selectedObject.ItemInquiry.Search = null;
                //selectedObject.ItemInquiry.Exclude = null;
                //selectedObject.ItemInquiry.Method = SearchMethod.AND;

                //View.ObjectSpace.Delete(((ListView)View).CollectionSource.List);

                string deleterecord = "DELETE FROM ItemInquiryDetails WHERE ItemInquiry = " + oid;
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(deleterecord, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                conn.Close();

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            //}
        }

        private void AddToCartPop_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //ItemInquiryDetails iteminquiry = (ItemInquiryDetails)View.CurrentObject;

            var os = Application.CreateObjectSpace(typeof(Confirmation));
            Confirmation message = os.CreateObject<Confirmation>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in document?";

            //if (iteminquiry != null)
            //{
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.SQ)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Sales Quotation (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    else if (iteminquiry.ItemInquiry.DocType == DocTypeList.PO)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Purchase Order (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.PRR)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Purchase Return Request (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.SRR)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Sales Return Request (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.WTR)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Warehouse Transfer Request (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.SAR)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Stock Adjustment Request (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //    if (iteminquiry.ItemInquiry.DocType == DocTypeList.SRF)
            //    {
            //        ((Confirmation)dv.CurrentObject).Message = "Do you want to proceed to add in this Sales Refund Request (" + iteminquiry.ItemInquiry.Cart + ") ?";
            //    }
            //}
            //else
            //{
            //    ((Confirmation)dv.CurrentObject).Message = "No item selected.";
            //}

            e.View = dv;
        }

        private void ViewItemPicture_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ItemInquiryDetails iteminquiry = (ItemInquiryDetails)View.CurrentObject;

            if (iteminquiry.PictureName != null)
            {
                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("ItemPicturePath").ToString() + iteminquiry.PictureName;
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            else
            {
                showMsg("Error", "No picture file.", InformationType.Error);
            }
        }

        // Start ver 1.0.7
        private void AddToCartSimple_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //ItemInquiryDetails selectedObject = (ItemInquiryDetails)e.CurrentObject;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            int oid = 0;

            //if (selectedObject == null)
            //{
            foreach (ItemInquiryDetails dtl in e.SelectedObjects)
            {
                oid = dtl.ItemInquiry.Oid;
                if (dtl.ItemInquiry.DocType == DocTypeList.SQ)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    SalesQuotation newSQ = cos.CreateObject<SalesQuotation>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        SalesQuotation trx = os.FindObject<SalesQuotation>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            SalesQuotationDetails newSQdetail = os.CreateObject<SalesQuotationDetails>();
                            newSQdetail.Postingdate = trx.PostingDate;
                            newSQdetail.Customer = newSQdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                            newSQdetail.Location = newSQdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newSQdetail.ItemCode = newSQdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSQdetail.ItemDesc = dtl.ItemDesc;
                            newSQdetail.Model = dtl.Model;
                            newSQdetail.CatalogNo = dtl.OriginalCatalog;
                            //newSQdetail.Price = dtl.Price1;
                            //newSQdetail.AdjustedPrice = dtl.Price1;
                            trx.SalesQuotationDetails.Add(newSQdetail);
                        }
                        else
                        {
                            newSQ.Priority = newSQ.Session.FindObject<PriorityType>
                                (CriteriaOperator.Parse("PriorityName = ? and IsActive= ?",
                                "Normal", "True"));

                            SalesQuotationDetails newSQdetail = cos.CreateObject<SalesQuotationDetails>();
                            newSQdetail.ItemCode = newSQdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSQdetail.ItemDesc = dtl.ItemDesc;
                            newSQdetail.Model = dtl.Model;
                            newSQdetail.CatalogNo = dtl.OriginalCatalog;
                            newSQdetail.Location = newSQdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            //newSQdetail.Price = dtl.Price1;
                            //newSQdetail.AdjustedPrice = dtl.Price1;
                            newSQ.SalesQuotationDetails.Add(newSQdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Sales Quoatation.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.PO)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    PurchaseOrders newPO = cos.CreateObject<PurchaseOrders>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            PurchaseOrderDetails newPOdetail = os.CreateObject<PurchaseOrderDetails>();
                            newPOdetail.Postingdate = trx.PostingDate;
                            newPOdetail.Supplier = newPOdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Supplier.BPCode);
                            newPOdetail.ItemCode = newPOdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newPOdetail.ItemDesc = dtl.ItemDesc;
                            newPOdetail.Model = dtl.Model;
                            newPOdetail.Location = newPOdetail.Session.GetObjectByKey<vwWarehouse>(trx.Warehouse.WarehouseCode);
                            newPOdetail.CatalogNo = dtl.OriginalCatalog;
                            //newPOdetail.Price = dtl.Price1;
                            //newPOdetail.AdjustedPrice = dtl.Price1;
                            trx.PurchaseOrderDetails.Add(newPOdetail);
                        }
                        else
                        {
                            PurchaseOrderDetails newPOdetail = cos.CreateObject<PurchaseOrderDetails>();
                            newPOdetail.ItemCode = newPOdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newPOdetail.ItemDesc = dtl.ItemDesc;
                            newPOdetail.Model = dtl.Model;
                            newPOdetail.Location = newPOdetail.Session.GetObjectByKey<vwWarehouse>(trx.Warehouse.WarehouseCode);
                            newPOdetail.CatalogNo = dtl.OriginalCatalog;
                            //newPOdetail.Price = dtl.Price1;
                            //newPOdetail.AdjustedPrice = dtl.Price1;
                            newPO.PurchaseOrderDetails.Add(newPOdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Purchase Order.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.PRR)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    PurchaseReturnRequests newprr = cos.CreateObject<PurchaseReturnRequests>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        PurchaseReturnRequests trx = os.FindObject<PurchaseReturnRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            PurchaseReturnRequestDetails newPRRdetail = os.CreateObject<PurchaseReturnRequestDetails>();
                            newPRRdetail.ItemCode = newPRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newPRRdetail.ItemDesc = dtl.ItemDesc;
                            newPRRdetail.UOM = newPRRdetail.ItemCode.UOM;
                            newPRRdetail.Warehouse = newPRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newPRRdetail.Price = dtl.Price1;
                            trx.PurchaseReturnRequestDetails.Add(newPRRdetail);
                        }
                        else
                        {
                            PurchaseReturnRequestDetails newPRRdetail = cos.CreateObject<PurchaseReturnRequestDetails>();
                            newPRRdetail.ItemCode = newPRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newPRRdetail.ItemDesc = dtl.ItemDesc;
                            newPRRdetail.UOM = newPRRdetail.ItemCode.UOM;
                            newPRRdetail.Warehouse = newPRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newPRRdetail.Price = dtl.Price1;
                            newprr.PurchaseReturnRequestDetails.Add(newPRRdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Purchase Return Request.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.SRR)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    SalesReturnRequests newsrr = cos.CreateObject<SalesReturnRequests>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            SalesReturnRequestDetails newSRRdetail = os.CreateObject<SalesReturnRequestDetails>();
                            newSRRdetail.ItemCode = newSRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSRRdetail.ItemDesc = dtl.ItemDesc;
                            newSRRdetail.UOM = newSRRdetail.ItemCode.UOM;
                            newSRRdetail.Warehouse = newSRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newSRRdetail.Price = dtl.Price1;
                            trx.SalesReturnRequestDetails.Add(newSRRdetail);
                        }
                        else
                        {
                            SalesReturnRequestDetails newSRRdetail = cos.CreateObject<SalesReturnRequestDetails>();
                            newSRRdetail.ItemCode = newSRRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSRRdetail.ItemDesc = dtl.ItemDesc;
                            newSRRdetail.UOM = newSRRdetail.ItemCode.UOM;
                            newSRRdetail.Warehouse = newSRRdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newSRRdetail.Price = dtl.Price1;
                            newsrr.SalesReturnRequestDetails.Add(newSRRdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Sales Return Request.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.WTR)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    WarehouseTransferReq newwtr = cos.CreateObject<WarehouseTransferReq>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        WarehouseTransferReq trx = os.FindObject<WarehouseTransferReq>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            WarehouseTransferReqDetails newWTRdetail = os.CreateObject<WarehouseTransferReqDetails>();
                            newWTRdetail.FromWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(trx.FromWarehouse.WarehouseCode);
                            newWTRdetail.ToWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(trx.ToWarehouse.WarehouseCode);
                            newWTRdetail.ItemCode = newWTRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newWTRdetail.ItemDesc = dtl.ItemDesc;
                            newWTRdetail.UOM = newWTRdetail.ItemCode.UOM;
                            newWTRdetail.CatalogNo = dtl.OriginalCatalog;
                            trx.WarehouseTransferReqDetails.Add(newWTRdetail);
                        }
                        else
                        {
                            WarehouseTransferReqDetails newWTRdetail = cos.CreateObject<WarehouseTransferReqDetails>();
                            newWTRdetail.FromWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(newwtr.FromWarehouse.WarehouseCode);
                            newWTRdetail.ToWarehouse = newWTRdetail.Session.GetObjectByKey<vwWarehouse>(newwtr.ToWarehouse.WarehouseCode);
                            newWTRdetail.ItemCode = newWTRdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newWTRdetail.ItemDesc = dtl.ItemDesc;
                            newWTRdetail.UOM = newWTRdetail.ItemCode.UOM;
                            newWTRdetail.CatalogNo = dtl.OriginalCatalog;
                            newwtr.WarehouseTransferReqDetails.Add(newWTRdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Warehouse Transfer Request.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.SAR)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    StockAdjustmentRequests newsar = cos.CreateObject<StockAdjustmentRequests>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        StockAdjustmentRequests trx = os.FindObject<StockAdjustmentRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            StockAdjustmentReqDetails newSARdetail = os.CreateObject<StockAdjustmentReqDetails>();
                            newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSARdetail.ItemDesc = dtl.ItemDesc;
                            newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                            newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newSARdetail.CatalogNo = dtl.OriginalCatalog;
                            trx.StockAdjustmentReqDetails.Add(newSARdetail);
                        }
                        else
                        {
                            StockAdjustmentReqDetails newSARdetail = cos.CreateObject<StockAdjustmentReqDetails>();
                            newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSARdetail.ItemDesc = dtl.ItemDesc;
                            newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                            newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newSARdetail.CatalogNo = dtl.OriginalCatalog;
                            newsar.StockAdjustmentReqDetails.Add(newSARdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Warehouse Transfer Request.", InformationType.Success);
                }
                else if (dtl.ItemInquiry.DocType == DocTypeList.SRF)
                {
                    IObjectSpace cos = Application.CreateObjectSpace();
                    SalesRefundRequests newsfr = cos.CreateObject<SalesRefundRequests>();
                    IObjectSpace os = Application.CreateObjectSpace();

                    if (dtl.ItemInquiry.Cart != null)
                    {
                        SalesRefundRequests trx = os.FindObject<SalesRefundRequests>(new BinaryOperator("DocNum", dtl.ItemInquiry.Cart));

                        if (trx != null)
                        {
                            SalesRefundReqDetails newSARdetail = os.CreateObject<SalesRefundReqDetails>();
                            newSARdetail.Customer = newSARdetail.Session.GetObjectByKey<vwBusniessPartner>(trx.Customer.BPCode);
                            newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSARdetail.ItemDesc = dtl.ItemDesc;
                            newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                            newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            trx.SalesRefundReqDetails.Add(newSARdetail);
                        }
                        else
                        {
                            SalesRefundReqDetails newSARdetail = cos.CreateObject<SalesRefundReqDetails>();
                            newSARdetail.ItemCode = newSARdetail.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                            newSARdetail.ItemDesc = dtl.ItemDesc;
                            newSARdetail.UOM = newSARdetail.ItemCode.UOM;
                            newSARdetail.Warehouse = newSARdetail.Session.GetObjectByKey<vwWarehouse>(dtl.ItemInquiry.Stock1.WarehouseCode);
                            newsfr.SalesRefundReqDetails.Add(newSARdetail);
                        }
                    }
                    else
                    {
                        showMsg("Error", "Please specify cart no.", InformationType.Error);
                        return;
                    }

                    os.CommitChanges();

                    showMsg("Success", "Added to Sales Refund Request.", InformationType.Success);
                }

                dtl.ItemInquiry.Search = null;
                dtl.ItemInquiry.Exclude = null;
                dtl.ItemInquiry.Method = SearchMethod.AND;
            }

            //selectedObject.ItemInquiry.Search = null;
            //selectedObject.ItemInquiry.Exclude = null;
            //selectedObject.ItemInquiry.Method = SearchMethod.AND;

            //View.ObjectSpace.Delete(((ListView)View).CollectionSource.List);

            string deleterecord = "DELETE FROM ItemInquiryDetails WHERE ItemInquiry = " + oid;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            SqlCommand cmd = new SqlCommand(deleterecord, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
            //}
        }
        // End ver 1.0.7
    }
}
