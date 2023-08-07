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

namespace StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer
{
    [DefaultClassOptions]
    [XafDisplayName("Warehouse Transfer Request")]
    [NavigationItem("Warehouse Transfer")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "WarehouseTransferReq_ListView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideNew1", AppearanceItemType = "Action", TargetItems = "New", Criteria = "(AppStatus in (2))", Context = "WarehouseTransferReq_DetailView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit1", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitWTR", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit1", AppearanceItemType.Action, "True", TargetItems = "SubmitWTR", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelWTR", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel1", AppearanceItemType.Action, "True", TargetItems = "CancelWTR", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCopyTo", AppearanceItemType.Action, "True", TargetItems = "WTRCopyToWT", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "WarehouseTransferReq_DetailView_Approval")]
    [Appearance("HideCopyTo1", AppearanceItemType.Action, "True", TargetItems = "WTRCopyToWT", Criteria = "(not (Status in (1))) or ((Status in (1)) and (not AppStatus in (0, 1)))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCopyTo2", AppearanceItemType.Action, "True", TargetItems = "WTRCopyToWT", Criteria = "CopyTo = 1", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideItemInq", AppearanceItemType.Action, "True", TargetItems = "WTRInquiryItem", Criteria = "FromWarehouse = null || ToWarehouse = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class WarehouseTransferReq : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public WarehouseTransferReq(Session session)
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
            TransferDate = DateTime.Now;

            Status = DocStatus.Draft;
            AppStatus = ApprovalStatusType.Not_Applicable;
            DocType = DocTypeList.WTR;
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
        [Index(301), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("Create Date", Enabled = false)]
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

        private vwBusniessPartner _Supplier;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'S'")]
        [XafDisplayName("Business Partner")]
        [Index(2), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Supplier
        {
            get { return _Supplier; }
            set
            {
                SetPropertyValue("Supplier", ref _Supplier, value);
            }
        }

        private string _Picker;
        [XafDisplayName("Picker")]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Picker
        {
            get { return _Picker; }
            set
            {
                SetPropertyValue("Picker", ref _Picker, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Date")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private DateTime _TransferDate;
        [XafDisplayName("Transfer Date")]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime TransferDate
        {
            get { return _TransferDate; }
            set
            {
                SetPropertyValue("TransferDate", ref _TransferDate, value);
            }
        }

        private vwWarehouse _FromWarehouse;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Transfer Out")]
        [DataSourceCriteria("Inactive = 'N'")]
        [Appearance("FromWarehouse", Enabled = false, Criteria = "IsValid")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse FromWarehouse
        {
            get { return _FromWarehouse; }
            set
            {
                SetPropertyValue("FromWarehouse", ref _FromWarehouse, value);
            }
        }

        private vwWarehouse _ToWarehouse;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Transfer In")]
        [DataSourceCriteria("Inactive = 'N'")]
        [Appearance("ToWarehouse", Enabled = false, Criteria = "IsValid")]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse ToWarehouse
        {
            get { return _ToWarehouse; }
            set
            {
                SetPropertyValue("ToWarehouse", ref _ToWarehouse, value);
            }
        }

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
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
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
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
        [XafDisplayName("CopyTo")]
        [Index(81), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
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
                foreach (WarehouseTransferReqDetails dtl in this.WarehouseTransferReqDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("WarehouseTransferReq-WarehouseTransferReqDetails")]
        [XafDisplayName("Item")]
        public XPCollection<WarehouseTransferReqDetails> WarehouseTransferReqDetails
        {
            get { return GetCollection<WarehouseTransferReqDetails>("WarehouseTransferReqDetails"); }
        }

        [Association("WarehouseTransferReq-WarehouseTransferReqDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<WarehouseTransferReqDocTrail> WarehouseTransferReqDocTrail
        {
            get { return GetCollection<WarehouseTransferReqDocTrail>("WarehouseTransferReqDocTrail"); }
        }

        [Association("WarehouseTransferReq-WarehouseTransferReqAppStage")]
        [XafDisplayName("Approval Stage")]
        public XPCollection<WarehouseTransferReqAppStage> WarehouseTransferReqAppStage
        {
            get { return GetCollection<WarehouseTransferReqAppStage>("WarehouseTransferReqAppStage"); }
        }

        [Association("WarehouseTransferReq-WarehouseTransferReqAppStatus")]
        [XafDisplayName("Approval Status")]
        public XPCollection<WarehouseTransferReqAppStatus> WarehouseTransferReqAppStatus
        {
            get { return GetCollection<WarehouseTransferReqAppStatus>("WarehouseTransferReqAppStatus"); }
        }

        [Association("WarehouseTransferReq-WarehouseTransferReqAttachment")]
        [XafDisplayName("Attachment")]
        public XPCollection<WarehouseTransferReqAttachment> WarehouseTransferReqAttachment
        {
            get { return GetCollection<WarehouseTransferReqAttachment>("WarehouseTransferReqAttachment"); }
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
                    WarehouseTransferReqDocTrail ds = new WarehouseTransferReqDocTrail(Session);
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
                    this.WarehouseTransferReqDocTrail.Add(ds);
                }
            }
        }
    }
}