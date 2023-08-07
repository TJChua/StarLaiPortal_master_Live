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
    [XafDisplayName("Sales Order")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("BoFullName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwPaymentSOGroup : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwPaymentSOGroup(Session session)
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
        [XafDisplayName("DocNum")]
        [Appearance("DocNum", Enabled = false)]
        [Appearance("DocNum1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(0)]
        public string DocNum
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

        [XafDisplayName("Posting Date")]
        [Appearance("PostingDateStr", Enabled = false)]
        [Appearance("PostingDateStr1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(4)]
        public string PostingDateStr
        {
            get; set;
        }

        [XafDisplayName("Delivery Date")]
        [Appearance("DeliveryDateStr", Enabled = false)]
        [Appearance("DeliveryDateStr1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(5)]
        public string DeliveryDateStr
        {
            get; set;
        }

        [XafDisplayName("PostingDate")]
        [Appearance("PostingDate", Enabled = false)]
        [Appearance("PostingDate1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(6), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime PostingDate
        {
            get; set;
        }

        [XafDisplayName("DeliveryDate")]
        [Appearance("DeliveryDate", Enabled = false)]
        [Appearance("DeliveryDate1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(7), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime DeliveryDate
        {
            get; set;
        }

        [XafDisplayName("Priority")]
        [Appearance("Priority", Enabled = false)]
        [Appearance("Priority1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(10)]
        public PriorityType Priority
        {
            get; set;
        }

        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Appearance("Customer1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(13), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Customer
        {
            get; set;
        }

        [XafDisplayName("CustomerName")]
        [Appearance("CustomerName", Enabled = false)]
        [Appearance("CustomerName1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(15)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        //[NoForeignKey]
        [Appearance("Transporter", Enabled = false)]
        [Appearance("Transporter1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(18)]
        public string Transporter
        {
            get; set;
        }

        [XafDisplayName("Salesperson")]
        [Appearance("Salesperson", Enabled = false)]
        [Appearance("Salesperson1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(20)]
        public string Salesperson
        {
            get; set;
        }

        [XafDisplayName("Remarks")]
        [Appearance("Remarks", Enabled = false)]
        [Appearance("Remarks1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(23)]
        public string Remarks
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Appearance("Status1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(25)]
        public string Status
        {
            get; set;
        }

        [XafDisplayName("SAP Doc No")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Appearance("SAPDocNum1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(28)]
        public string SAPDocNum
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Appearance("Warehouse1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(30)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("PartialPicked ")]
        [Appearance("PartialPicked ", Enabled = false)]
        [Appearance("PartialPicked1", BackColor = "#FF7C80", FontColor = "Black", Criteria = "PartialPicked > 0")]
        [Index(33)]
        public int PartialPicked
        {
            get; set;
        }

    }
}