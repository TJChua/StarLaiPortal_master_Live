using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Purchase_Order
{
    [DefaultClassOptions]
    //[Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("LinkDoc", AppearanceItemType = "Action", TargetItems = "Link", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("UnlinkDoc", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("Purchase Order Details")]
    [RuleCriteria("POQuantity", DefaultContexts.Save, "IsValid = 0", "Quantity cannot 0.")]
    [RuleCriteria("POBacktoBackQuantity", DefaultContexts.Save, "IsValid1 = 0", "Back to Back Sales not allow to change quantity.")]

    public class PurchaseOrderDetails : XPObject    
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PurchaseOrderDetails(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
            if (user != null)
            {
                CreateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
            }
            else
            {
                CreateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
            }
            CreateDate = DateTime.Now;
            Quantity = 1;
        }

        private ApplicationUser _CreateUser;
        [XafDisplayName("Create User")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Index(300), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public ApplicationUser CreateUser
        {
            get { return _CreateUser; }
            set
            {
                SetPropertyValue("CreateUser", ref _CreateUser, value);
            }
        }

        private DateTime? _CreateDate;
        [Index(301), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set
            {
                SetPropertyValue("CreateDate", ref _CreateDate, value);
            }
        }

        private ApplicationUser _UpdateUser;
        [XafDisplayName("Update User"), ToolTip("Enter Text")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Index(302), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public ApplicationUser UpdateUser
        {
            get { return _UpdateUser; }
            set
            {
                SetPropertyValue("UpdateUser", ref _UpdateUser, value);
            }
        }

        private DateTime? _UpdateDate;
        [Index(303), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime? UpdateDate
        {
            get { return _UpdateDate; }
            set
            {
                SetPropertyValue("UpdateDate", ref _UpdateDate, value);
            }
        }

        private vwItemMasters _ItemCode;
        [ImmediatePostData]
        [NoForeignKey]
        [XafDisplayName("Item Code")]
        [DataSourceCriteria("frozenFor = 'N'")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemCode", Enabled = false, Criteria = "not IsNew")]
        [RuleRequiredField(DefaultContexts.Save)]
        public vwItemMasters ItemCode

        {
            get { return _ItemCode; }
            set
            {
                SetPropertyValue("ItemCode", ref _ItemCode, value);
                if (!IsLoading && value != null)
                {
                    ItemDesc = ItemCode.ItemName;
                    Model = ItemCode.Model;
                    CatalogNo = ItemCode.CatalogNo;
                    UOM = ItemCode.UOM;
                    LegacyItemCode = ItemCode.LegacyItemCode;

                    if (Supplier != null)
                    {
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        ItemCode.ItemCode, Supplier.ListNum, Postingdate.Date, Postingdate.Date, Quantity, Quantity));

                        if (tempprice != null)
                        {
                            AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                ItemCode.ItemCode, Supplier.ListNum));

                            if (temppricelist != null)
                            {
                                AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                AdjustedPrice = 0;
                            }
                        }
                    }

                    Price = ItemCode.LastPurchasePrice;
                }
                else if (!IsLoading && value == null)
                {
                    ItemDesc = null;
                    Model = null;
                    CatalogNo = null;
                    UOM = null;
                    LegacyItemCode = null;
                    Price = 0;
                    AdjustedPrice = 0;
                }
            }
        }

        private string _LegacyItemCode;
        [XafDisplayName("Legacy Item Code")]
        [Index(2), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("LegacyItemCode", Enabled = false)]
        public string LegacyItemCode
        {
            get { return _LegacyItemCode; }
            set
            {
                SetPropertyValue("LegacyItemCode", ref _LegacyItemCode, value);
            }

        }

        private string _ItemDesc;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Item Description")]
        [Appearance("ItemDesc", Enabled = false)]
        [Index(3), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string ItemDesc
        {
            get { return _ItemDesc; }
            set
            {
                SetPropertyValue("ItemDesc", ref _ItemDesc, value);
            }
        }

        private string _Model;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Model")]
        [Appearance("Model", Enabled = false)]
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string Model
        {
            get { return _Model; }
            set
            {
                SetPropertyValue("Model", ref _Model, value);
            }
        }

        private string _CatalogNo;
        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CatalogNo
        {
            get { return _CatalogNo; }
            set
            {
                SetPropertyValue("CatalogNo", ref _CatalogNo, value);
            }
        }

        private string _UOM;
        [XafDisplayName("UOM Group")]
        [Appearance("UOM", Enabled = false)]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string UOM
        {
            get { return _UOM; }
            set
            {
                SetPropertyValue("UOM", ref _UOM, value);
            }
        }

        private vwWarehouse _Location;
        [NoForeignKey]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Appearance("Warehouse", Enabled = false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Warehouse")]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse Location
        {
            get { return _Location; }
            set
            {
                SetPropertyValue("Location", ref _Location, value);
            }
        }

        private decimal _Quantity;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Quantity")]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
                if (!IsLoading)
                {
                    if (Supplier != null)
                    {
                        //vwPrice tempprice = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                        //    ItemCode.ItemCode, Supplier.ListNum));

                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        ItemCode.ItemCode, Supplier.ListNum, Postingdate.Date, Postingdate.Date, Quantity, Quantity));

                        if (tempprice != null)
                        {
                            AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                ItemCode.ItemCode, Supplier.ListNum));

                            if (temppricelist != null)
                            {
                                AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                AdjustedPrice = 0;
                            }
                        }
                    }

                    Total = Quantity * AdjustedPrice;
                }
            }
        }

        private decimal _SellingPrice;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Selling Price")]
        [Appearance("SellingPrice", Enabled = false)]
        [Index(16), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal SellingPrice
        {
            get { return _SellingPrice; }
            set
            {
                SetPropertyValue("SellingPrice", ref _SellingPrice, value);
            }
        }

        private decimal _Price;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Last Purchase Price")]
        [Appearance("Price", Enabled = false)]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Price
        {
            get { return _Price; }
            set
            {
                SetPropertyValue("Price", ref _Price, value);
            }
        }

        private decimal _AdjustedPrice;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Unit Price")]
        [Index(19), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal AdjustedPrice
        {
            get { return _AdjustedPrice; }
            set
            {
                SetPropertyValue("AdjustedPrice", ref _AdjustedPrice, value);
                if (!IsLoading)
                {
                    Total = Quantity * AdjustedPrice;
                }
            }
        }

        private decimal _Total;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Total")]
        [Appearance("Total", Enabled = false)]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Total
        {
            get { return _Total; }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
            }
        }

        private vwBusniessPartner _Supplier;
        [NoForeignKey]
        [ImmediatePostData]
        [XafDisplayName("Supplier")]
        [Appearance("Supplier", Enabled = false)]
        [Index(23), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwBusniessPartner Supplier
        {
            get { return _Supplier; }
            set
            {
                SetPropertyValue("Supplier", ref _Supplier, value);
            }
        }

        private string _BaseDoc;
        [XafDisplayName("SO Doc Num")]
        [Index(25), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string BaseDoc
        {
            get { return _BaseDoc; }
            set
            {
                SetPropertyValue("BaseDoc", ref _BaseDoc, value);
            }
        }

        private string _BaseId;
        [XafDisplayName("BaseId")]
        [Index(28), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string BaseId
        {
            get { return _BaseId; }
            set
            {
                SetPropertyValue("BaseId", ref _BaseId, value);
            }
        }

        private int _LabelPrintCount;
        [XafDisplayName("LabelPrintCount")]
        [Index(30), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int LabelPrintCount
        {
            get { return _LabelPrintCount; }
            set
            {
                SetPropertyValue("LabelPrintCount", ref _LabelPrintCount, value);
            }
        }

        private string _Series;
        [XafDisplayName("Series")]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Series
        {
            get { return _Series; }
            set
            {
                SetPropertyValue("Series", ref _Series, value);
            }
        }

        private DateTime _Postingdate;
        [XafDisplayName("Postingdate")]
        [Index(35), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime Postingdate
        {
            get { return _Postingdate; }
            set
            {
                SetPropertyValue("Postingdate", ref _Postingdate, value);
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            { return Session.IsNewObject(this); }
        }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                if (this.Quantity <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid1
        {
            get
            {
               if (this.BaseDoc != null)
                {
                    SalesOrder salesorder;
                    salesorder = Session.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", this.BaseDoc));

                    if (salesorder != null)
                    {
                        foreach (SalesOrderDetails dtl in salesorder.SalesOrderDetails)
                        {
                            if (dtl.Oid.ToString() == this.BaseId && (this.Series == "BackOrdS" || this.Series == "BackOrdP"))
                            {
                                if (dtl.Quantity != this.Quantity)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                return false;
            }
        }

        private PurchaseOrders _PurchaseOrders;
        [Association("PurchaseOrders-PurchaseOrderDetails")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("PurchaseOrders", Enabled = false)]
        public PurchaseOrders PurchaseOrders
        {
            get { return _PurchaseOrders; }
            set { SetPropertyValue("PurchaseOrders", ref _PurchaseOrders, value); }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork)
                && (Session.DataLayer != null)
                    && (Session.ObjectLayer is SimpleObjectLayer)
                        )
            {
                ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
                if (user != null)
                {
                    UpdateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                }
                else
                {
                    UpdateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                }
                UpdateDate = DateTime.Now;

                if (Session.IsNewObject(this))
                {

                }
            }
        }
    }
}