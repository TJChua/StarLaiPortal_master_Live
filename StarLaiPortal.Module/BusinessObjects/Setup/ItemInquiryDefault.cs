using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Setup
{
    [DefaultClassOptions]
    [NavigationItem("Setup")]
    [DefaultProperty("DocType")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [RuleCriteria("ItemInquiryDefDeleteRule", DefaultContexts.Delete, "1=0", "Cannot Delete.")]

    public class ItemInquiryDefault : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ItemInquiryDefault(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            IsActive = true;
        }

        private DocTypeList _DocType;
        [XafDisplayName("Doc Type")]
        [RuleUniqueValue]
        [ImmediatePostData]
        [Appearance("DocType", Enabled = false, Criteria = "not IsNew")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(0)]
        public DocTypeList DocType
        {
            get { return _DocType; }
            set
            {
                SetPropertyValue("DocType", ref _DocType, value);
            }
        }

        private vwPriceList _PriceList1;
        [NoForeignKey]
        [XafDisplayName("PriceList 1")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList1
        {
            get { return _PriceList1; }
            set
            {
                SetPropertyValue("PriceList1", ref _PriceList1, value);
            }
        }

        private vwPriceList _PriceList2;
        [NoForeignKey]
        [XafDisplayName("PriceList 2")]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList2
        {
            get { return _PriceList2; }
            set
            {
                SetPropertyValue("PriceList2", ref _PriceList2, value);
            }
        }

        private vwPriceList _PriceList3;
        [NoForeignKey]
        [XafDisplayName("PriceList 3")]
        [Index(9), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList3
        {
            get { return _PriceList3; }
            set
            {
                SetPropertyValue("PriceList3", ref _PriceList3, value);
            }
        }

        private vwPriceList _PriceList4;
        [NoForeignKey]
        [XafDisplayName("PriceList 4")]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwPriceList PriceList4
        {
            get { return _PriceList4; }
            set
            {
                SetPropertyValue("PriceList4", ref _PriceList4, value);
            }
        }

        private vwWarehouse _Stock1;
        [NoForeignKey]
        [XafDisplayName("Stock 1")]
        [Index(11), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwWarehouse Stock1
        {
            get { return _Stock1; }
            set
            {
                SetPropertyValue("Stock1", ref _Stock1, value);
            }
        }

        private vwWarehouse _Stock2;
        [NoForeignKey]
        [XafDisplayName("Stock 2")]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwWarehouse Stock2
        {
            get { return _Stock2; }
            set
            {
                SetPropertyValue("Stock2", ref _Stock2, value);
            }
        }

        private bool _IsActive;
        [XafDisplayName("Active")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [Index(20)]
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                SetPropertyValue("IsActive", ref _IsActive, value);
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            { return Session.IsNewObject(this); }
        }

        private XPCollection<AuditDataItemPersistent> auditTrail;
        public XPCollection<AuditDataItemPersistent> AuditTrail
        {
            get
            {
                if (auditTrail == null)
                {
                    auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditTrail;
            }
        }
    }
}