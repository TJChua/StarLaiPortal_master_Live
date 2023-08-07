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

namespace StarLaiPortal.Module.BusinessObjects.Item_Inquiry
{
    [DefaultClassOptions]
    [XafDisplayName("Item Inquiry")]
    [DefaultProperty("Cart")]

    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("SaveDocRecord", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("SaveAndCloseDocRecord", AppearanceItemType = "Action", TargetItems = "SaveAndClose", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "Cancel", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class ItemInquiry : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ItemInquiry(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private ApplicationUser _CreateUser;
        [XafDisplayName("Create User")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Appearance("CreateUser", Enabled = false)]
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
        [Appearance("CreateDate", Enabled = false)]
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
        [Appearance("UpdateUser", Enabled = false)]
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
        [Appearance("UpdateDate", Enabled = false)]
        public DateTime? UpdateDate
        {
            get { return _UpdateDate; }
            set
            {
                SetPropertyValue("UpdateDate", ref _UpdateDate, value);
            }
        }

        private string _Search;
        [XafDisplayName("Search")]
        [Index(0), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Search
        {
            get { return _Search; }
            set
            {
                SetPropertyValue("Search", ref _Search, value);
            }
        }

        private string _Exclude;
        [XafDisplayName("Exclude")]
        [Index(1), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Exclude
        {
            get { return _Exclude; }
            set
            {
                SetPropertyValue("Exclude", ref _Exclude, value);
            }
        }

        private string _Cart;
        [XafDisplayName("Doc Num")]
        [Appearance("Cart", Enabled = false)]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Cart
        {
            get { return _Cart; }
            set
            {
                SetPropertyValue("Cart", ref _Cart, value);
            }
        }

        private vwPriceList _PriceList1;
        [NoForeignKey]
        [XafDisplayName("Price 1")]
        [Appearance("PriceList1", Enabled = false, Criteria = "Cart != null")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList1
        {
            get { return _PriceList1; }
            set
            {
                SetPropertyValue("PriceList1", ref _PriceList1, value);
            }
        }

        private vwPriceList _PriceList2;
        [NoForeignKey]
        [XafDisplayName("Price 2")]
        [Appearance("PriceList2", Enabled = false, Criteria = "Cart != null")]
        [Index(7), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList2
        {
            get { return _PriceList2; }
            set
            {
                SetPropertyValue("PriceList2", ref _PriceList2, value);
            }
        }

        private vwPriceList _PriceList3;
        [NoForeignKey]
        [XafDisplayName("Price 3")]
        [Appearance("PriceList3", Enabled = false, Criteria = "Cart != null")]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList3
        {
            get { return _PriceList3; }
            set
            {
                SetPropertyValue("PriceList3", ref _PriceList3, value);
            }
        }

        private vwPriceList _PriceList4;
        [NoForeignKey]
        [XafDisplayName("Price 4")]
        [Appearance("PriceList4", Enabled = false, Criteria = "Cart != null")]
        [Index(9), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList4
        {
            get { return _PriceList4; }
            set
            {
                SetPropertyValue("PriceList4", ref _PriceList4, value);
            }
        }

        private vwWarehouse _Stock1;
        [NoForeignKey]
        [XafDisplayName("Stock 1")]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwWarehouse Stock1
        {
            get { return _Stock1; }
            set
            {
                SetPropertyValue("Stock1", ref _Stock1, value);
            }
        }

        private vwWarehouse _Stock2;
        [NoForeignKey]
        [XafDisplayName("Stock 2")]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwWarehouse Stock2
        {
            get { return _Stock2; }
            set
            {
                SetPropertyValue("Stock2", ref _Stock2, value);
            }
        }

        private SearchMethod _Method;
        [XafDisplayName("Method")]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public SearchMethod Method
        {
            get { return _Method; }
            set
            {
                SetPropertyValue("Method", ref _Method, value);
            }
        }

        private DocTypeList _DocType;
        [XafDisplayName("DocType")]
        [Index(18), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DocTypeList DocType
        {
            get { return _DocType; }
            set
            {
                SetPropertyValue("DocType", ref _DocType, value);
            }
        }

        private vwBusniessPartner _CardCode;
        [NoForeignKey]
        [XafDisplayName("CardCode")]
        [Index(20), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwBusniessPartner CardCode
        {
            get { return _CardCode; }
            set
            {
                SetPropertyValue("CardCode", ref _CardCode, value);
            }
        }

        [Association("ItemInquiry-ItemInquiryDetails")]
        [XafDisplayName("Items")]
        public XPCollection<ItemInquiryDetails> ItemInquiryDetails
        {
            get { return GetCollection<ItemInquiryDetails>("ItemInquiryDetails"); }
        }
    }
}