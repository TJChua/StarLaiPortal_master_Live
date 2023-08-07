using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Sales_Quotation
{
    [DefaultClassOptions]
    //[Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("LinkDoc", AppearanceItemType = "Action", TargetItems = "Link", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("UnlinkDoc", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("Sales Quotation Details")]

    [RuleCriteria("SQQuantity", DefaultContexts.Save, "IsValid = 0", "Quantity cannot 0.")]

    public class SalesQuotationDetails : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public SalesQuotationDetails(Session session)
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
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [XafDisplayName("Item Code")]
        [DataSourceCriteria("frozenFor = 'N'")]
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
                    if (Location != null)
                    {
                        Available = Session.FindObject<vwStockBalance>(CriteriaOperator.Parse("ItemCode = ? and WhsCode = ?",
                            ItemCode, Location.WarehouseCode));
                    }
                    LegacyItemCode = ItemCode.LegacyItemCode;

                    if (Customer != null)
                    {
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                       ItemCode.ItemCode, Customer.ListNum, Postingdate.Date, Postingdate.Date, Quantity, Quantity));

                        if (tempprice != null)
                        {
                            Price = tempprice.Price;
                            AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                ItemCode.ItemCode, Customer.ListNum));

                            if (temppricelist != null)
                            {
                                Price = temppricelist.Price;
                                AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                Price = 0;
                                AdjustedPrice = 0;
                            }
                        }
                    }
                }
                else if (!IsLoading && value == null)
                {
                    ItemDesc = null;
                    Model = null;
                    CatalogNo = null;
                    Available = null;
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
        [Index(6), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CatalogNo
        {
            get { return _CatalogNo; }
            set
            {
                SetPropertyValue("CatalogNo", ref _CatalogNo, value);
            }
        }

        private vwWarehouse _Location;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Loc")]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse Location
        {
            get { return _Location; }
            set
            {
                SetPropertyValue("Location", ref _Location, value);
                if (!IsLoading && value != null)
                {
                    if (ItemCode != null)
                    {
                        Available = Session.FindObject<vwStockBalance>(CriteriaOperator.Parse("ItemCode = ? and WhsCode = ?",
                            ItemCode, Location.WarehouseCode));
                    }
                }
                else if (!IsLoading && value == null)
                {
                    Available = null;
                }
            }
        }

        private decimal _Quantity;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Quantity")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                SetPropertyValue("Quantity", ref _Quantity, value);
                if (!IsLoading && value != 0)
                {
                    if (Customer != null)
                    {
                        //vwPrice tempprice = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                        //    ItemCode.ItemCode, Supplier.ListNum));

                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        ItemCode.ItemCode, Customer.ListNum, Postingdate.Date, Postingdate.Date, Quantity, Quantity));

                        if (tempprice != null)
                        {
                            Price = tempprice.Price;
                            AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                ItemCode.ItemCode, Customer.ListNum));

                            if (temppricelist != null)
                            {
                                Price = temppricelist.Price;
                                AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                Price = 0;
                                AdjustedPrice = 0;
                            }
                        }
                    }

                    Total = Quantity * AdjustedPrice;
                }
            }
        }

        private vwStockBalance _Available;
        [ImmediatePostData]
        [NoForeignKey]
        //[DbType("numeric(18,6)")]
        //[ModelDefault("DisplayFormat", "{0:N0}")]
        //[ModelDefault("EditMask", "{0:N0}")]
        [XafDisplayName("Available")]
        [Appearance("Available", Enabled = false)]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwStockBalance Available
        {
            get { return _Available; }
            set
            {
                SetPropertyValue("Available", ref _Available, value);
            }
        }

        private decimal _Price;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Price")]
        [Appearance("Price", Enabled = false)]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
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
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
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

        private vwBusniessPartner _Customer;
        [NoForeignKey]
        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Index(23), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwBusniessPartner Customer
        {
            get { return _Customer; }
            set
            {
                SetPropertyValue("Customer", ref _Customer, value);
            }
        }

        private DateTime _Postingdate;
        [XafDisplayName("Postingdate")]
        [Index(25), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
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

        private SalesQuotation _SalesQuotation;
        [Association("SalesQuotation-SalesQuotationDetails")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("SalesQuotation", Enabled = false)]
        public SalesQuotation SalesQuotation
        {
            get { return _SalesQuotation; }
            set { SetPropertyValue("SalesQuotation", ref _SalesQuotation, value); }
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