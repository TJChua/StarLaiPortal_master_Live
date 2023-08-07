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
    [XafDisplayName("Bundle ID Inquiry")]
    [NavigationItem("Pack List")]
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
    public class vwInquiryBundleID : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryBundleID(Session session)
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

        [XafDisplayName("Portal Pack List No.")]
        [Appearance("PortalNo", Enabled = false)]
        [Index(3)]
        public string PortalNo
        {
            get; set;
        }

        [XafDisplayName("Loading Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("LoadingDate", Enabled = false)]
        [Index(5)]
        public DateTime LoadingDate
        {
            get; set;
        }

        [XafDisplayName("Bundle ID")]
        [Appearance("BundleID", Enabled = false)]
        [Index(8)]
        public string BundleID
        {
            get; set;
        }

        [XafDisplayName("Item Code")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(10)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Legacy Item Code")]
        [Appearance("LegacyItemCode", Enabled = false)]
        [Index(11)]
        public string LegacyItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Name")]
        [Appearance("ItemName", Enabled = false)]
        [Index(13)]
        public string ItemName
        {
            get; set;
        }

        [XafDisplayName("SO No.")]
        [Appearance("SONo", Enabled = false)]
        [Index(15)]
        public string SONo
        {
            get; set;
        }

        [XafDisplayName("SO Date")]
        [Appearance("SODate", Enabled = false)]
        [Index(18)]
        public DateTime SODate
        {
            get; set;
        }

        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(20)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("Transporter Name")]
        [Appearance("TransporterName", Enabled = false)]
        [Index(23)]
        public string TransporterName
        {
            get; set;
        }

        [XafDisplayName("Pick List Doc Num")]
        [Appearance("PickListDocNum", Enabled = false)]
        [Index(25)]
        public string PickListDocNum
        {
            get; set;
        }

        [XafDisplayName("Pack Date")]
        [Appearance("PackDate", Enabled = false)]
        [Index(28)]
        public DateTime PackDate
        {
            get; set;
        }

        [XafDisplayName("Pack Time")]
        [Appearance("PackTime", Enabled = false)]
        [Index(30)]
        public string PackTime
        {
            get; set;
        }
    }
}