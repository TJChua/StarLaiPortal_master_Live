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

namespace StarLaiPortal.Module.BusinessObjects.View
{
    [DefaultClassOptions]
    [XafDisplayName("Last Purchase Price")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("ItemCode")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwLastPurchasePrice : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwLastPurchasePrice(Session session)
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
        [XafDisplayName("ItemCode")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(0)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("PrevP")]
        [Appearance("PrevP", Enabled = false)]
        [Index(5), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal PrevP
        {
            get; set;
        }

        [XafDisplayName("PrevD")]
        [Appearance("PrevD", Enabled = false)]
        [Index(10), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime PrevD
        {
            get; set;
        }

        [XafDisplayName("PrevPO")]
        [Appearance("PrevPO", Enabled = false)]
        [Index(15), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string PrevPO
        {
            get; set;
        }

        //[XafDisplayName("PrevLoc")]
        //[Appearance("PrevLoc", Enabled = false)]
        //[Index(20), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public string PrevLoc
        //{
        //    get; set;
        //}

        [XafDisplayName("PrevAmt")]
        [Appearance("PrevAmt", Enabled = false)]
        [Index(25), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal PrevAmt
        {
            get; set;
        }

        //[XafDisplayName("Vendor")]
        //[Appearance("Vendor", Enabled = false)]
        //[Index(30), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public string Vendor
        //{
        //    get; set;
        //}
    }
}