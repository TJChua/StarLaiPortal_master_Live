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
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Pack_List
{
    [DefaultClassOptions]
    [Appearance("APLTrxDocStatuses1", AppearanceItemType = "Action", TargetItems = "New", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("APLTrxDocStatuses2", AppearanceItemType = "Action", TargetItems = "Edit", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("APLTrxDocStatuses3", AppearanceItemType = "Action", TargetItems = "Link", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("APLTrxDocStatuses4", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("APLTrxDocStatuses5", AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [DefaultProperty("Bundle")]
    [XafDisplayName("Copy From Pick List")]
    public class AddPickList : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public AddPickList(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private BundleType _Bundle;
        [XafDisplayName("Bundle")]
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public BundleType Bundle
        {
            get { return _Bundle; }
            set
            {
                SetPropertyValue("Bundle", ref _Bundle, value);
            }
        }

        private vwBin _ToBin;
        [NoForeignKey]
        [XafDisplayName("ToBin")]
        [Index(3), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwBin ToBin
        {
            get { return _ToBin; }
            set
            {
                SetPropertyValue("ToBin", ref _ToBin, value);
            }
        }

        private string _DocNum;
        [XafDisplayName("DocNum")]
        [Index(5), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string DocNum
        {
            get { return _DocNum; }
            set
            {
                SetPropertyValue("DocNum", ref _DocNum, value);
            }
        }

        [Association("AddPickList-vwPickList")]
        [XafDisplayName("Pack List")]
        [Appearance("vwPickList", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public XPCollection<vwPickList> vwPickList
        {
            get {return GetCollection<vwPickList>("vwPickList");}
        }

        [XafDisplayName("Pack List")]
        public XPCollection<vwPickList> vwPickList2
        {
            get
            {
                CriteriaOperator filter = CriteriaOperator.Parse("ToBin = ?", this.ToBin.BinCode);
                return new XPCollection<vwPickList>(this.vwPickList, filter);
            }
        }

        [Association("AddPickList-AddPickListDetails")]
        [XafDisplayName("Pick List Cart")]
        public XPCollection<AddPickListDetails> AddPickListDetails
        {
            get { return GetCollection<AddPickListDetails>("AddPickListDetails"); }
        }
    }
}