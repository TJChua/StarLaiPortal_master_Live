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
    [DefaultProperty("StaffName")]
    [XafDisplayName("Staff Info")]
    //[Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("hideSaveAndClose", AppearanceItemType = "Action", TargetItems = "SaveAndClose", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hideSaveAndNew", AppearanceItemType = "Action", TargetItems = "SaveAndNew", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class StaffInfo : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public StaffInfo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            IsActive = true;
        }

        private string _StaffID;
        [ImmediatePostData]
        [XafDisplayName("Staff ID")]
        [Appearance("StaffID", Enabled = false, Criteria = "not IsNew")]
        [Index(0)]
        public string StaffID
        {
            get { return _StaffID; }
            set
            {
                SetPropertyValue("StaffID", ref _StaffID, value);
            }
        }

        private string _StaffName;
        [ImmediatePostData]
        [XafDisplayName("Staff Name")]
        [Index(3)]
        public string StaffName
        {
            get { return _StaffName; }
            set
            {
                SetPropertyValue("StaffName", ref _StaffName, value);
            }
        }

        private string _StaffEmail;
        [ImmediatePostData]
        [XafDisplayName("Staff Email")]
        [Index(5)]
        public string StaffEmail
        {
            get { return _StaffEmail; }
            set
            {
                SetPropertyValue("StaffEmail", ref _StaffEmail, value);
            }
        }

        private string _StaffContactNo;
        [ImmediatePostData]
        [XafDisplayName("Staff Contact No")]
        [Index(8)]
        public string StaffContactNo
        {
            get { return _StaffContactNo; }
            set
            {
                SetPropertyValue("StaffContactNo", ref _StaffContactNo, value);
            }
        }

        private vwDepartment _Department;
        [NoForeignKey]
        [XafDisplayName("Department")]
        [Index(9), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public vwDepartment Department
        {
            get { return _Department; }
            set
            {
                SetPropertyValue("Department", ref _Department, value);
            }
        }

        private bool _IsActive;
        [XafDisplayName("Active")]
        [Index(30)]
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