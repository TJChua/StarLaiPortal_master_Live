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
    [XafDisplayName("Open Pick List")]
    [NavigationItem("Pick List")]
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
    public class vwInquiryOpenPickList : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryOpenPickList(Session session)
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

        [XafDisplayName("Portal Pick List No.")]
        [Appearance("PortalNo", Enabled = false)]
        [Index(3)]
        public string PortalNo
        {
            get; set;
        }

        [XafDisplayName("Series")]
        [Appearance("Series", Enabled = false)]
        [Index(5)]
        public string Series
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(8)]
        public DateTime DocDate
        {
            get; set;
        }

        [XafDisplayName("Expected Delivery Date")]
        [Appearance("DueDate", Enabled = false)]
        [Index(10)]
        public DateTime DueDate
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(13)]
        public string Status
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [Appearance("Transporter", Enabled = false)]
        [Index(15)]
        public string Transporter
        {
            get; set;
        }

        [XafDisplayName("Priority")]
        [Appearance("Priority", Enabled = false)]
        [Index(14)]
        public string Priority
        {
            get; set;
        }

        [XafDisplayName("Picker")]
        [Appearance("Picker", Enabled = false)]
        [Index(18)]
        public string Picker
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(20)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("Customer Group")]
        [Appearance("CardGroup", Enabled = false)]
        [Index(23)]
        public string CardGroup
        {
            get; set;
        }

        [XafDisplayName("Customer Code")]
        [Appearance("CardCode", Enabled = false)]
        [Index(25)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("Customer Name")]
        [Appearance("CardName", Enabled = false)]
        [Index(28)]
        [Size(200)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("Portal SO No.")]
        [Appearance("PortalSONo", Enabled = false)]
        [Index(30)]
        public string PortalSONo
        {
            get; set;
        }

        [XafDisplayName("SAP SO No.")]
        [Appearance("SAPSONo", Enabled = false)]
        [Index(33)]
        public string SAPSONo
        {
            get; set;
        }

        [XafDisplayName("Remark")]
        [Appearance("Remark", Enabled = false)]
        [Index(35)]
        [Size(254)]
        public string Remark
        {
            get; set;
        }

        [XafDisplayName("SO Delivery Date")]
        [Appearance("SODeliveryDate", Enabled = false)]
        [Index(38)]
        public DateTime SODeliveryDate
        {
            get; set;
        }

        [XafDisplayName("Print Status")]
        [Appearance("PrintStatus", Enabled = false)]
        [Index(40)]
        public string PrintStatus
        {
            get; set;
        }

        [XafDisplayName("Print Count")]
        [Appearance("PrintCount", Enabled = false)]
        [Index(43)]
        public int PrintCount
        {
            get; set;
        }
    }
}