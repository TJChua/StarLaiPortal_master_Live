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

namespace StarLaiPortal.Module.BusinessObjects.Stock_Adjustment
{
    [DefaultClassOptions]
    [XafDisplayName("Stock Adjustment Request")]
    [NavigationItem("Stock Adjustment")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "StockAdjustmentRequests_ListView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideNew1", AppearanceItemType = "Action", TargetItems = "New", Criteria = "(AppStatus in (2))", Context = "StockAdjustmentRequests_DetailView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit1", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitSAR", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit1", AppearanceItemType.Action, "True", TargetItems = "SubmitSAR", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelSAR", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel1", AppearanceItemType.Action, "True", TargetItems = "CancelSAR", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCopyTo", AppearanceItemType.Action, "True", TargetItems = "SARCopyToSA", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "StockAdjustmentRequests_DetailView_Approval")]
    [Appearance("HideCopyTo1", AppearanceItemType.Action, "True", TargetItems = "SARCopyToSA", Criteria = "(not (Status in (1))) or ((Status in (1)) and (not AppStatus in (0, 1)))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCopyTo2", AppearanceItemType.Action, "True", TargetItems = "SARCopyToSA", Criteria = "CopyTo = 1", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideItemInq", AppearanceItemType.Action, "True", TargetItems = "SARInquiryItem", Criteria = "ReasonCode = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class StockAdjustmentRequests : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public StockAdjustmentRequests(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
            if (user != null)
            {
                CreateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
            }
            else
            {
                CreateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
            }
            CreateDate = DateTime.Now;
            DocDate = DateTime.Now;
            AdjDate = DateTime.Now;

            Status = DocStatus.Draft;
            AppStatus = ApprovalStatusType.Not_Applicable;
            DocType = DocTypeList.SAR;
        }

        private ApplicationUser _CreateUser;
        [XafDisplayName("Create User")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Appearance("CreateUser", Enabled = false)]
        [Index(300), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public ApplicationUser CreateUser
        {
            get { return _CreateUser; }
            set
            {
                SetPropertyValue("CreateUser", ref _CreateUser, value);
            }
        }

        private DateTime? _CreateDate;
        [Index(301), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("CreateDate", Enabled = false)]
        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set
            {
                SetPropertyValue("CreateDate", ref _CreateDate, value);
            }
        }

        private ApplicationUser _UpdateUser;
        [XafDisplayName("Update User"), ToolTip("Enter Text")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Appearance("UpdateUser", Enabled = false)]
        [Index(302), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public ApplicationUser UpdateUser
        {
            get { return _UpdateUser; }
            set
            {
                SetPropertyValue("UpdateUser", ref _UpdateUser, value);
            }
        }

        private DateTime? _UpdateDate;
        [Index(303), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("UpdateDate", Enabled = false)]
        public DateTime? UpdateDate
        {
            get { return _UpdateDate; }
            set
            {
                SetPropertyValue("UpdateDate", ref _UpdateDate, value);
            }
        }

        private DocTypeList _DocType;
        [Appearance("DocType", Enabled = false, Criteria = "not IsNew")]
        [Index(304), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public virtual DocTypeList DocType
        {
            get { return _DocType; }
            set
            {
                SetPropertyValue("DocType", ref _DocType, value);
            }
        }

        private string _DocNum;
        [XafDisplayName("No.")]
        [Appearance("DocNum", Enabled = false)]
        [Index(0), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string DocNum
        {
            get { return _DocNum; }
            set
            {
                SetPropertyValue("DocNum", ref _DocNum, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Date")]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private DateTime _AdjDate;
        [XafDisplayName("Adj. Date")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime AdjDate
        {
            get { return _AdjDate; }
            set
            {
                SetPropertyValue("AdjDate", ref _AdjDate, value);
            }
        }

        private vwReasonCode _ReasonCode;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Type = 'StockAdjustment'")]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Reason Code")]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwReasonCode ReasonCode
        {
            get { return _ReasonCode; }
            set
            {
                SetPropertyValue("ReasonCode", ref _ReasonCode, value);
            }
        }

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DocStatus Status
        {
            get { return _Status; }
            set
            {
                SetPropertyValue("Status", ref _Status, value);
            }
        }

        private ApprovalStatusType _AppStatus;
        [XafDisplayName("Approval Status")]
        [Appearance("AppStatus", Enabled = false)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public ApprovalStatusType AppStatus
        {
            get { return _AppStatus; }
            set
            {
                SetPropertyValue("AppStatus", ref _AppStatus, value);
            }
        }

        private string _AppUser;
        [XafDisplayName("AppUser")]
        [Index(79), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string AppUser
        {
            get { return _AppUser; }
            set
            {
                SetPropertyValue("AppUser", ref _AppUser, value);
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(80), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private bool _CopyTo;
        [XafDisplayName("Copy")]
        [Index(81), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public bool CopyTo
        {
            get { return _CopyTo; }
            set
            {
                SetPropertyValue("CopyTo", ref _CopyTo, value);
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            { return Session.IsNewObject(this); }
        }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                foreach (StockAdjustmentReqDetails dtl in this.StockAdjustmentReqDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("StockAdjustmentRequests-StockAdjustmentReqDetails")]
        [XafDisplayName("Item")]
        public XPCollection<StockAdjustmentReqDetails> StockAdjustmentReqDetails
        {
            get { return GetCollection<StockAdjustmentReqDetails>("StockAdjustmentReqDetails"); }
        }

        [Association("StockAdjustmentRequests-StockAdjustmentReqDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<StockAdjustmentReqDocTrail> StockAdjustmentReqDocTrail
        {
            get { return GetCollection<StockAdjustmentReqDocTrail>("StockAdjustmentReqDocTrail"); }
        }

        [Association("StockAdjustmentRequests-StockAdjustmentReqAppStage")]
        [XafDisplayName("Approval Stage")]
        public XPCollection<StockAdjustmentReqAppStage> StockAdjustmentReqAppStage
        {
            get { return GetCollection<StockAdjustmentReqAppStage>("StockAdjustmentReqAppStage"); }
        }

        [Association("StockAdjustmentRequests-StockAdjustmentReqAppStatus")]
        [XafDisplayName("Approval Status")]
        public XPCollection<StockAdjustmentReqAppStatus> StockAdjustmentReqAppStatus
        {
            get { return GetCollection<StockAdjustmentReqAppStatus>("StockAdjustmentReqAppStatus"); }
        }

        [Association("StockAdjustmentRequests-StockAdjustmentReqAttachment")]
        [XafDisplayName("Attachment")]
        public XPCollection<StockAdjustmentReqAttachment> StockAdjustmentReqAttachment
        {
            get { return GetCollection<StockAdjustmentReqAttachment>("StockAdjustmentReqAttachment"); }
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

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork)
                && (Session.DataLayer != null)
                    && (Session.ObjectLayer is SimpleObjectLayer)
                        )
            {
                ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;
                if (user != null)
                {
                    UpdateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                }
                else
                {
                    UpdateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                }
                UpdateDate = DateTime.Now;

                if (Session.IsNewObject(this))
                {
                    StockAdjustmentReqDocTrail ds = new StockAdjustmentReqDocTrail(Session);
                    ds.DocStatus = DocStatus.Draft;
                    ds.DocRemarks = "";
                    if (user != null)
                    {
                        ds.CreateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                        ds.UpdateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                    }
                    else
                    {
                        ds.CreateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.UpdateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                    }
                    ds.CreateDate = DateTime.Now;
                    ds.UpdateDate = DateTime.Now;
                    this.StockAdjustmentReqDocTrail.Add(ds);
                }
            }
        }
    }
}