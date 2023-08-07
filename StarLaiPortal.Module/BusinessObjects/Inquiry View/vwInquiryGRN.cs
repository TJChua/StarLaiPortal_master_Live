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
    [XafDisplayName("GRPO Inquiry")]
    [NavigationItem("GRPO")]
    [DefaultProperty("PortalNo")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwInquiryGRN : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryGRN(Session session)
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

        [XafDisplayName("Portal Transaction No.")]
        [Appearance("PortalNo", Enabled = false)]
        [Index(3)]
        public string PortalNo
        {
            get; set;
        }

        [XafDisplayName("SAP GRPO No.")]
        [Appearance("SAPNo", Enabled = false)]
        [Index(5)]
        public string SAPNo
        {
            get; set;
        }

        [XafDisplayName("SAP PO No.")]
        [Appearance("SAPPONo", Enabled = false)]
        [Index(6)]
        public string SAPPONo
        {
            get; set;
        }

        [XafDisplayName("Portal PO No.")]
        [Appearance("PortalPONo", Enabled = false)]
        [Index(8)]
        public string PortalPONo
        {
            get; set;
        }

        [XafDisplayName("ASN No.")]
        [Appearance("ASNNo", Enabled = false)]
        [Index(10)]
        public string ASNNo
        {
            get; set;
        }

        [XafDisplayName("Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(13)]
        public DateTime DocDate
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(15)]
        public string Status
        {
            get; set;
        }
        [XafDisplayName("Customer/Vendor Code")]
        [Appearance("CardCode", Enabled = false)]
        [Index(18)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("Customer/Vendor Name")]
        [Appearance("CardName", Enabled = false)]
        [Index(20)]
        [Size(200)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("Item Code")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(23)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Name")]
        [Appearance("ItemName", Enabled = false)]
        [Index(25)]
        public string ItemName
        {
            get; set;
        }

        [XafDisplayName("Legacy Item Code")]
        [Appearance("LegacyItemCode", Enabled = false)]
        [Index(25)]
        public string LegacyItemCode
        {
            get; set;
        }

        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(28)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Model")]
        [Appearance("Model", Enabled = false)]
        [Index(30)]
        public string Model
        {
            get; set;
        }
        [XafDisplayName("Brand")]
        [Appearance("Brand", Enabled = false)]
        [Index(33)]
        public string Brand
        {
            get; set;
        }

        [XafDisplayName("ASN Qty")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("ASNQty ", Enabled = false)]
        [Index(35)]
        public decimal ASNQty
        {
            get; set;
        }

        [XafDisplayName("Received Qty")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("ReceivedQty", Enabled = false)]
        [Index(35)]
        public decimal ReceivedQty
        {
            get; set;
        }

        [XafDisplayName("Discrepancy Qty")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("DiscrepancyQty", Enabled = false)]
        [Index(35)]
        public decimal DiscrepancyQty
        {
            get; set;
        }

        [XafDisplayName("Discrepancy Reason")]
        [Appearance("DiscrepancyReason", Enabled = false)]
        [Size(200)]
        [Index(38)]
        public string DiscrepancyReason
        {
            get; set;
        }

        [XafDisplayName("UOM")]
        [Appearance("UOM", Enabled = false)]
        [Index(38)]
        public string UOM
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(40)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Bin Location")]
        [Appearance("Bin", Enabled = false)]
        [Index(43)]
        public string Bin
        {
            get; set;
        }

        [XafDisplayName("Remark")]
        [Appearance("Remark", Enabled = false)]
        [Index(45)]
        [Size(254)]
        public string Remark
        {
            get; set;
        }

        [XafDisplayName("Reference No.")]
        [Appearance("Reference", Enabled = false)]
        [Index(48)]
        public string Reference
        {
            get; set;
        }
    }
}