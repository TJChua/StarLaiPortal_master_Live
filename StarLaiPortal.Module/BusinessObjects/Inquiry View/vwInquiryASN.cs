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
    [XafDisplayName("ASN Inquiry")]
    [NavigationItem("Advanced Shipment Notice")]
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
    public class vwInquiryASN : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryASN(Session session)
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

        [XafDisplayName("Portal ASN No.")]
        [Appearance("PortalNo", Enabled = false)]
        [Index(3)]
        public string PortalNo
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(5)]
        public DateTime DocDate
        {
            get; set;
        }

        [XafDisplayName("Document Date")]
        [Appearance("TaxDate", Enabled = false)]
        [Index(8)]
        public DateTime TaxDate
        {
            get; set;
        }

        [XafDisplayName("ETA Date")]
        [Appearance("ETADate", Enabled = false)]
        [Index(10)]
        public DateTime ETADate
        {
            get; set;
        }

        [XafDisplayName("ESR Date")]
        [Appearance("ESRDate", Enabled = false)]
        [Index(13)]
        public DateTime ESRDate
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

        [XafDisplayName("Print Status")]
        [Appearance("PrintStatus", Enabled = false)]
        [Index(18)]
        public string PrintStatus
        {
            get; set;
        }

        [XafDisplayName("Label Print Count ")]
        [Appearance("LabelPrintCount ", Enabled = false)]
        [Index(19)]
        public int LabelPrintCount
        {
            get; set;
        }

        [XafDisplayName("Vehicle")]
        [Appearance("Vehicle", Enabled = false)]
        [Index(20)]
        public string Vehicle
        {
            get; set;
        }

        [XafDisplayName("Container")]
        [Appearance("Container", Enabled = false)]
        [Index(23)]
        public string Container
        {
            get; set;
        }

        [XafDisplayName("Customer/Vendor Code")]
        [Appearance("CardCode", Enabled = false)]
        [Index(25)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("Customer/Vendor Name")]
        [Appearance("CardName", Enabled = false)]
        [Index(28)]
        [Size(200)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("SAP PO No.")]
        [Appearance("SAPPONo", Enabled = false)]
        [Index(30)]
        public string SAPPONo
        {
            get; set;
        }

        [XafDisplayName("Portal PO No.")]
        [Appearance("PortalPONo", Enabled = false)]
        [Index(33)]
        public string PortalPONo
        {
            get; set;
        }

        [XafDisplayName("Item Code")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(35)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Name")]
        [Appearance("ItemName", Enabled = false)]
        [Index(38)]
        [Size(200)]
        public string ItemName
        {
            get; set;
        }

        [XafDisplayName("Legacy Item Code")]
        [Appearance("LegacyItemCode", Enabled = false)]
        [Index(40)]
        public string LegacyItemCode
        {
            get; set;
        }

        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(43)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Model")]
        [Appearance("Model", Enabled = false)]
        [Index(45)]
        public string Model
        {
            get; set;
        }

        [XafDisplayName("Brand")]
        [Appearance("Brand", Enabled = false)]
        [Index(48)]
        public string Brand
        {
            get; set;
        }

        [XafDisplayName("Planned Qty.")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("PlanQty", Enabled = false)]
        [Index(50)]
        public decimal PlanQty
        {
            get; set;
        }

        [XafDisplayName("UOM")]
        [Appearance("UOM", Enabled = false)]
        [Index(53)]
        public string UOM
        {
            get; set;
        }

        [XafDisplayName("Loaded Qty.")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("LoadQty", Enabled = false)]
        [Index(55)]
        public decimal LoadQty
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(58)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Remark")]
        [Appearance("Remark", Enabled = false)]
        [Index(60)]
        [Size(254)]
        public string Remark
        {
            get; set;
        }

        [XafDisplayName("Reference No.")]
        [Appearance("Reference", Enabled = false)]
        [Index(63)]
        public string Reference
        {
            get; set;
        }
    }
}