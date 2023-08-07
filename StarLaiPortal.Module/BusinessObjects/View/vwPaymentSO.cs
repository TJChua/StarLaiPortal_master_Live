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
    [XafDisplayName("Sales Order Items")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("BoFullName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideSearch", AppearanceItemType.Action, "True", TargetItems = "FullTextSearch", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwPaymentSO : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwPaymentSO(Session session)
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
        [Appearance("PriKey1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("Series")]
        //[NoForeignKey]
        [Appearance("Series", Enabled = false)]
        [Appearance("Series1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(3)]
        public string Series
        {
            get; set;
        }

        [XafDisplayName("DocNum")]
        [Appearance("DocNum", Enabled = false)]
        [Appearance("DocNum1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(5)]
        public string DocNum
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("PostingDateStr", Enabled = false)]
        [Appearance("PostingDateStr1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(6)]
        public string PostingDateStr
        {
            get; set;
        }

        [XafDisplayName("Delivery Date")]
        [Appearance("DeliveryDateStr", Enabled = false)]
        [Appearance("DeliveryDateStr1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(7)]
        public string DeliveryDateStr
        {
            get; set;
        }

        [XafDisplayName("PostingDate")]
        [Appearance("PostingDate", Enabled = false)]
        [Appearance("PostingDate1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(8)]
        public DateTime PostingDate
        {
            get; set;
        }

        [XafDisplayName("DeliveryDate")]
        [Appearance("DeliveryDate", Enabled = false)]
        [Appearance("DeliveryDate1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(10)]
        public DateTime DeliveryDate
        {
            get; set;
        }

        [XafDisplayName("Priority")]
        [Appearance("Priority", Enabled = false)]
        [Appearance("Priority1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(13)]
        public PriorityType Priority
        {
            get; set;
        }

        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Appearance("Customer1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(15), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Customer
        {
            get; set;
        }

        [XafDisplayName("CustomerName")]
        [Appearance("CustomerName", Enabled = false)]
        [Appearance("CustomerName1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(18)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        //[NoForeignKey]
        [Appearance("Transporter", Enabled = false)]
        [Appearance("Transporter1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(20)]
        public string Transporter
        {
            get; set;
        }

        [XafDisplayName("Salesperson")]
        [Appearance("Salesperson", Enabled = false)]
        [Appearance("Salesperson1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(23)]
        public string Salesperson
        {
            get; set;
        }

        [XafDisplayName("Remarks")]
        [Appearance("Remarks", Enabled = false)]
        [Appearance("Remarks1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(25)]
        public string Remarks
        {
            get; set;
        }

        [XafDisplayName("ItemCode")]
        [Appearance("ItemCode", Enabled = false)]
        [Appearance("ItemCode1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(28)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("ItemDesc")]
        [Appearance("ItemDesc", Enabled = false)]
        [Appearance("ItemDesc1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
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
        [Appearance("Quantity1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal Quantity
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Appearance("Warehouse1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(35)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Mfg Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Appearance("CatalogNo1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(38)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Oid")]
        [Appearance("Oid", Enabled = false)]
        [Appearance("Oid1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Oid
        {
            get; set;
        }

        [XafDisplayName("CreateDate")]
        [Appearance("CreateDate", Enabled = false)]
        [Appearance("CreateDate1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(43), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime CreateDate
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Appearance("Status1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(45)]
        public string Status
        {
            get; set;
        }

        [XafDisplayName("SAP Doc No")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Appearance("SAPDocNum1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(48)]
        public string SAPDocNum
        {
            get; set;
        }

        [XafDisplayName("InStock")]
        [DbType("numeric(19,6)")]
        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "{0:n4}")]
        [Appearance("InStock", Enabled = false)]
        [Appearance("InStock1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(33), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal InStock
        {
            get; set;
        }

        [XafDisplayName("PartialPicked ")]
        [Appearance("PartialPicked ", Enabled = false)]
        [Appearance("PartialPicked1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(35)]
        public int PartialPicked
        {
            get; set;
        }

    }
}