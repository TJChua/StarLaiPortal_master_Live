using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.View
{
    [DefaultClassOptions]
    [XafDisplayName("Pick List Item")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("BoFullName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwPickList : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwPickList(Session session)
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

        [XafDisplayName("Item Code")]
        [Appearance("ItemCode", Enabled = false)]
        [Index(3)]
        public string ItemCode
        {
            get; set;
        }

        [XafDisplayName("Item Desc")]
        [Appearance("ItemDesc", Enabled = false)]
        [Index(5)]
        public string ItemDesc
        {
            get; set;
        }

        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(8)]
        public string CatalogNo
        {
            get; set;
        }

        [XafDisplayName("Pick Qty")]
        [DbType("numeric(19,6)")]
        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "{0:n4}")]
        [Appearance("PickQty", Enabled = false)]
        [Index(10)]
        public decimal PickQty
        {
            get; set;
        }

        [XafDisplayName("To Bin")]
        [Appearance("ToBin", Enabled = false)]
        [Index(13)]
        public string ToBin
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [Appearance("SOTransporter", Enabled = false)]
        [Index(15)]
        public string SOTransporter
        {
            get; set;
        }

        [XafDisplayName("DocNum")]
        [Appearance("DocNum", Enabled = false)]
        [Index(18)]
        public string DocNum
        {
            get; set;
        }

        [XafDisplayName("Oid")]
        [Appearance("Oid", Enabled = false)]
        [Index(20)]
        public string Oid
        {
            get; set;
        }

        private AddPickList _AddPickList;
        [Association("AddPickList-vwPickList")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("AddPickList", Enabled = false)]
        public AddPickList AddPickList
        {
            get { return _AddPickList; }
            set { SetPropertyValue("AddPickList", ref _AddPickList, value); }
        }
    }
}