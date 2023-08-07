using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.View
{
    [DefaultClassOptions]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [XafDisplayName("PO Item")]
    public class vwPODetails : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwPODetails(Session session)
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
        //private string _DocNo;
        [XafDisplayName("PriKey")]
        [Appearance("PriKey", Enabled = false)]
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("SAP Doc Num")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(3), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get; set;
        }

        [XafDisplayName("PO No")]
        [Appearance("PortalNum", Enabled = false)]
        [Index(5), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string PortalNum
        {
            get; set;
        }

        [XafDisplayName("Item Code")]
        [NoForeignKey]
        [Appearance("ItemCode", Enabled = false)]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Description")]
        [Appearance("ItemDescrip", Enabled = false)]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string ItemDescrip
        {
            get; set;
        }

        [XafDisplayName("Legacy Item Code")]
        [Appearance("LegacyItemCode", Enabled = false)]
        [Index(11), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string LegacyItemCode
        {
            get; set;
        }

        [XafDisplayName("UOM Group")]
        [Appearance("UOM", Enabled = false)]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string UOM
        {
            get; set;
        }

        [XafDisplayName("Open Qty")]
        [Appearance("OpenQty", Enabled = false)]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(15), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal OpenQty
        {
            get; set;
        }

        [XafDisplayName("Quantity")]
        [Appearance("Quantity", Enabled = false)]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Quantity
        {
            get; set;
        }

        [XafDisplayName("Tax")]
        [Appearance("Tax", Enabled = false)]
        [Index(20), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Tax
        {
            get; set;
        }

        [XafDisplayName("Unit Price")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("UnitPrice", Enabled = false)]
        [Index(23), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal UnitPrice
        {
            get; set;
        }

        [XafDisplayName("Total")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Total", Enabled = false)]
        [Index(25), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Total
        {
            get; set;
        }

        [XafDisplayName("CardCode")]
        [Appearance("CardCode", Enabled = false)]
        [Index(28), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("CardName")]
        [Appearance("CardName", Enabled = false)]
        [Index(30), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("BaseEntry")]
        [Appearance("BaseEntry", Enabled = false)]
        [Index(32), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int BaseEntry
        {
            get; set;
        }

        [XafDisplayName("BaseLine")]
        [Appearance("BaseLine", Enabled = false)]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int BaseLine
        {
            get; set;
        }

        [XafDisplayName("WhsCode")]
        [Appearance("WhsCode", Enabled = false)]
        [Index(35), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwWarehouse WhsCode
        {
            get; set;
        }

        [XafDisplayName("DefBin")]
        [Appearance("DefBin", Enabled = false)]
        [Index(36), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string DefBin
        {
            get; set;
        }

        [XafDisplayName("CatalogNum")]
        [Appearance("CatalogNum", Enabled = false)]
        [Index(38), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CatalogNum
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("PostingDate", Enabled = false)]
        [Index(40), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string PostingDate
        {
            get; set;
        }

        [XafDisplayName("Document Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(43), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string DocDate
        {
            get; set;
        }

        [XafDisplayName("Create User")]
        [Appearance("CreateUser", Enabled = false)]
        [Index(45), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public StaffInfo CreateUser
        {
            get; set;
        }

        [XafDisplayName("PO Series")]
        [NoForeignKey]
        [Appearance("Series", Enabled = false)]
        [Index(48), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwSeries Series
        {
            get; set;
        }
    }
}