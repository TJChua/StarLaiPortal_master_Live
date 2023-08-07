using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Inquiry_View
{
    [DefaultClassOptions]
    [NavigationItem("Reports")]
    [XafDisplayName("Stock Balance")]
    [DefaultProperty("ItemCode")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwInquiryStockBalance : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryStockBalance(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Key]
        [Browsable(true)]
        [XafDisplayName("PriKey")]
        [Appearance("PriKey", Enabled = false)]
        [Index(0), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("Item Code")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(1)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Name")]
        [Appearance("ItemName", Enabled = false)]
        [Index(3)]
        [Size(200)]
        public string ItemName
        {
            get; set;
        }

        [XafDisplayName("Legacy Item Code")]
        [Appearance("LegacyItemCode", Enabled = false)]
        [Index(5)]
        public string LegacyItemCode
        {
            get; set;
        }

        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Model")]
        [Appearance("Model", Enabled = false)]
        [Index(10)]
        public string Model
        {
            get; set;
        }

        [XafDisplayName("Brand")]
        [Appearance("Brand", Enabled = false)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Brand
        {
            get; set;
        }

        [XafDisplayName("Quantity")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Quantity", Enabled = false)]
        [Index(15)]
        public decimal Quantity
        {
            get; set;
        }

        [XafDisplayName("UOM")]
        [Appearance("UOM", Enabled = false)]
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string UOM
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Bin Location")]
        [Appearance("BinLocation", Enabled = false)]
        [Index(23), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string BinLocation
        {
            get; set;
        }
    }
}