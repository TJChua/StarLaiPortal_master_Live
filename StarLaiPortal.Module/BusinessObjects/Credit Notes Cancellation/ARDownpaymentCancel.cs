using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation
{
    [DefaultClassOptions]
    [XafDisplayName("AR Downpayment Cancellation")]
    [NavigationItem("AR Downpayment Cancellation")]
    [DefaultProperty("DocNum")]

    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "ARDownpaymentCancel_ListView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideNew1", AppearanceItemType = "Action", TargetItems = "New", Criteria = "(AppStatus in (2))", Context = "ARDownpaymentCancel_DetailView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit1", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitDPCancel", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit1", AppearanceItemType.Action, "True", TargetItems = "SubmitDPCancel", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelDPCancel", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel1", AppearanceItemType.Action, "True", TargetItems = "CancelDPCancel", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    
    public class ARDownpaymentCancel : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ARDownpaymentCancel(Session session)
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
            PostingDate = DateTime.Now;

            Status = DocStatus.Draft;
            AppStatus = ApprovalStatusType.Not_Applicable;
            DocType = DocTypeList.ARDC;
            CancelDate = true;
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

        private vwBusniessPartner _Customer;
        [XafDisplayName("Customer")]
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'C'")]
        [Appearance("Customer", Enabled = false, Criteria = "not IsNew")]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Customer
        {
            get { return _Customer; }
            set
            {
                SetPropertyValue("Customer", ref _Customer, value);
                if (!IsLoading && value != null)
                {
                    CustomerName = Customer.BPName;
                    if (Customer.SlpCode != null)
                    {
                        ContactPerson = Session.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpCode = ?", Customer.SlpCode.SlpCode));
                    }
                }
                else if (!IsLoading && value == null)
                {
                    CustomerName = null;
                    ContactPerson = null;
                }
            }
        }

        private string _CustomerName;
        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                SetPropertyValue("CustomerName", ref _CustomerName, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Doc Date")]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private DateTime _PostingDate;
        [XafDisplayName("Posting Date")]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime PostingDate
        {
            get { return _PostingDate; }
            set
            {
                SetPropertyValue("PostingDate", ref _PostingDate, value);
            }
        }

        private vwSalesPerson _ContactPerson;
        [NoForeignKey]
        [XafDisplayName("Salesperson")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Active = 'Y'")]
        [Index(14), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwSalesPerson ContactPerson
        {
            get { return _ContactPerson; }
            set
            {
                SetPropertyValue("ContactPerson", ref _ContactPerson, value);
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
        [Index(16), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public ApprovalStatusType AppStatus
        {
            get { return _AppStatus; }
            set
            {
                SetPropertyValue("AppStatus", ref _AppStatus, value);
            }
        }

        private string _SAPDocNum;
        [XafDisplayName("SAP CN No.")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get { return _SAPDocNum; }
            set
            {
                SetPropertyValue("SAPDocNum", ref _SAPDocNum, value);
            }
        }

        private bool _CancelDate;
        [ImmediatePostData]
        [XafDisplayName("Payment Cancel by Posting Date?")]
        [Index(19), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public bool CancelDate
        {
            get { return _CancelDate; }
            set
            {
                SetPropertyValue("CancelDate", ref _CancelDate, value);
            }
        }

        private string _AppUser;
        [XafDisplayName("AppUser")]
        [Index(78), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string AppUser
        {
            get { return _AppUser; }
            set
            {
                SetPropertyValue("AppUser", ref _AppUser, value);
            }
        }

        private string _Reference;
        [ImmediatePostData]
        [XafDisplayName("AR Downpayment No.")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(79), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Reference
        {
            get { return _Reference; }
            set
            {
                SetPropertyValue("Reference", ref _Reference, value);
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

        private bool _Sap;
        [XafDisplayName("Sap")]
        [Index(82), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool Sap
        {
            get { return _Sap; }
            set
            {
                SetPropertyValue("Sap", ref _Sap, value);
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
                foreach (ARDownpaymentCancelDetails dtl in this.ARDownpaymentCancelDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid2
        {
            get
            {
                if (this.ContactPerson.Active == "N")
                {
                    return true;
                }

                return false;
            }
        }

        [Association("ARDownpaymentCancel-ARDownpaymentCancelDetails")]
        [XafDisplayName("Content")]
        [Appearance("ARDownpaymentCancelDetails", Enabled = false)]
        public XPCollection<ARDownpaymentCancelDetails> ARDownpaymentCancelDetails
        {
            get { return GetCollection<ARDownpaymentCancelDetails>("ARDownpaymentCancelDetails"); }
        }

        [Association("ARDownpaymentCancel-ARDownpaymentCancellationDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<ARDownpaymentCancellationDocTrail> ARDownpaymentCancellationDocTrail
        {
            get { return GetCollection<ARDownpaymentCancellationDocTrail>("ARDownpaymentCancellationDocTrail"); }
        }

        [Association("ARDownpaymentCancel-ARDownpaymentCancellationAppStage")]
        [XafDisplayName("Approval Stage")]
        public XPCollection<ARDownpaymentCancellationAppStage> ARDownpaymentCancellationAppStage
        {
            get { return GetCollection<ARDownpaymentCancellationAppStage>("ARDownpaymentCancellationAppStage"); }
        }

        [Association("ARDownpaymentCancel-ARDownpaymentCancellationAppStatus")]
        [XafDisplayName("Approval Status")]
        public XPCollection<ARDownpaymentCancellationAppStatus> ARDownpaymentCancellationAppStatus
        {
            get { return GetCollection<ARDownpaymentCancellationAppStatus>("ARDownpaymentCancellationAppStatus"); }
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
                    ARDownpaymentCancellationDocTrail ds = new ARDownpaymentCancellationDocTrail(Session);
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
                    this.ARDownpaymentCancellationDocTrail.Add(ds);
                }
            }
        }
    }
}