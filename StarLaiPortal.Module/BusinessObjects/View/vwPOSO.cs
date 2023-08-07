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
    [XafDisplayName("PO Sales Order Details")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("BoFullName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwPOSO : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwPOSO(Session session)
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
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("Series")]
        [NoForeignKey]
        [Appearance("Series", Enabled = false)]
        [Index(3)]
        public vwSeries Series
        {
            get; set;
        }

        [XafDisplayName("Doc Num")]
        [Appearance("DocNum", Enabled = false)]
        [Index(5)]
        public string DocNum
        {
            get; set;
        }

        [XafDisplayName("SAP Doc Num")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(6)]
        public string SAPDocNum
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("PostingDate", Enabled = false)]
        [Index(8)]
        public DateTime PostingDate
        {
            get; set;
        }

        [XafDisplayName("Delivery Date")]
        [Appearance("DeliveryDate", Enabled = false)]
        [Index(10)]
        public DateTime DeliveryDate
        {
            get; set;
        }

        [XafDisplayName("Priority")]
        [Appearance("Priority", Enabled = false)]
        [Index(13)]
        public PriorityType Priority
        {
            get; set;
        }

        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Index(15), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Customer
        {
            get; set;
        }

        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(18)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [NoForeignKey]
        [Appearance("Transporter", Enabled = false)]
        [Index(20)]
        public vwTransporter Transporter
        {
            get; set;
        }

        [XafDisplayName("Salesperson")]
        [Appearance("Salesperson", Enabled = false)]
        [Index(23)]
        public string Salesperson
        {
            get; set;
        }

        [XafDisplayName("Remarks")]
        [Appearance("Remarks", Enabled = false)]
        [Index(25)]
        public string Remarks
        {
            get; set;
        }

        [XafDisplayName("ItemCode")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(28)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("ItemDesc")]
        [Appearance("ItemDesc", Enabled = false)]
        [Index(30)]
        public string ItemDesc
        {
            get; set;
        }

        [XafDisplayName("Quantity")]
        [DbType("numeric(19,6)")]
        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "{0:n4}")]
        [Appearance("Quantity", Enabled = false)]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal Quantity
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(35)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Mfg Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(38)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Oid")]
        [Appearance("Oid", Enabled = false)]
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Oid
        {
            get; set;
        }

        [XafDisplayName("CreateDate")]
        [Appearance("CreateDate", Enabled = false)]
        [Index(43), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime CreateDate
        {
            get; set;
        }

        [XafDisplayName("Selling Price")]
        [DbType("numeric(19,6)")]
        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "{0:n4}")]
        [Appearance("SellingPrice", Enabled = false)]
        [Index(45), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal SellingPrice
        {
            get; set;
        }
    }
}