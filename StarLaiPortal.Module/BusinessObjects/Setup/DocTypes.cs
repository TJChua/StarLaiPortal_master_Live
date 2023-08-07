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
    [DefaultProperty("BoFullName")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [RuleCriteria("DocTypesDeleteRule", DefaultContexts.Delete, "1=0", "Cannot Delete.")]

    public class DocTypes : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public DocTypes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            IsActive = true;
            BoCode = DocTypeList.SO;
        }

        private DocTypeList _BoCode;
        [XafDisplayName("Code"), ToolTip("Enter Text")]
        [ImmediatePostData]
        [Appearance("BoCode", Enabled = false, Criteria = "not IsNew")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(0)]
        public DocTypeList BoCode
        {
            get { return _BoCode; }
            set
            {
                SetPropertyValue("BoCode", ref _BoCode, value);
                if (!IsLoading)
                {
                    BoName = BoCode.ToString();
                }
            }
        }

        private string _BoName;
        [XafDisplayName("Prefix"), ToolTip("Enter Text")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(1)]
        public string BoName
        {
            get { return _BoName; }
            set
            {
                SetPropertyValue("BoName", ref _BoName, value);
            }
        }

        private int _CurrectDocNum;
        [XafDisplayName("Currect DocNum")]
        [Index(3)]
        public int CurrectDocNum
        {
            get { return _CurrectDocNum; }
            set
            {
                SetPropertyValue("CurrectDocNum", ref _CurrectDocNum, value);
            }

        }

        private int _NextDocNum;
        [XafDisplayName("Next DocNum")]
        [Index(5)]
        public int NextDocNum
        {
            get { return _NextDocNum; }
            set
            {
                SetPropertyValue("NextDocNum", ref _NextDocNum, value);
            }
        }

        private TransferType _TransferType;
        [XafDisplayName("Transfer Type")]
        [Index(8)]
        [Appearance("TransferType", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "BoCode != 13")]
        public TransferType TransferType
        {
            get { return _TransferType; }
            set
            {
                SetPropertyValue("TransferType", ref _TransferType, value);
            }
        }

        private Series _Series;
        [DataSourceCriteria("DocType = '@this.BoCode'")]
        [XafDisplayName("Series")]
        [Index(9), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public Series Series
        {
            get { return _Series; }
            set
            {
                SetPropertyValue("Series", ref _Series, value);
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

        [Index(30), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string BoFullName
        {
            get { return BoCode + "-" + BoName; }
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