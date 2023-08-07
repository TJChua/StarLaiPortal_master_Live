using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Delivery_Order
{
    [DefaultClassOptions]
    [XafDisplayName("Delivery Order")]
    [NavigationItem("Delivery Order")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitDO", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelDO", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class DeliveryOrder : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public DeliveryOrder(Session session)
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
            DocType = DocTypeList.DO;
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
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
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
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Customer
        {
            get { return _Customer; }
            set
            {
                SetPropertyValue("Customer", ref _Customer, value);
                if (!IsLoading && value != null)
                {
                    CustomerName = Customer.BPName;
                    CustomerGroup = Customer.GroupName;
                }
                else if (!IsLoading && value == null)
                {
                    CustomerName = null;
                    CustomerGroup = null;
                }
            }
        }

        private string _CustomerName;
        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                SetPropertyValue("CustomerName", ref _CustomerName, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Date")]
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

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(20), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("Loading No.")]
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("LoadingNo", Enabled = false)]
        public string LoadingNo
        {
            get
            {
                string rtn = null;
                string dupno = null;
                foreach (DeliveryOrderDetails dtl in this.DeliveryOrderDetails)
                {
                    if (dupno != dtl.BaseDoc)
                    {
                        if (rtn == null)
                        {
                            rtn = dtl.BaseDoc;
                        }
                        else
                        {
                            rtn = rtn + ", " + dtl.BaseDoc;
                        }

                        dupno = dtl.BaseDoc;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("SO No.")]
        [Index(23), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("SONo", Enabled = false)]
        public string SONo
        {
            get
            {
                string rtn = null;
                string dupso = null;
                foreach (DeliveryOrderDetails dtl in this.DeliveryOrderDetails)
                {
                    if (dupso != dtl.SODocNum)
                    {
                        if (rtn == null)
                        {
                            rtn = dtl.SODocNum;
                        }
                        else
                        {
                            rtn = rtn + ", " + dtl.SODocNum;
                        }

                        dupso = dtl.SODocNum;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("Priority")]
        [Index(25), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Priority", Enabled = false)]
        public PriorityType Priority
        {
            get
            {
                PriorityType rtn = null;

                foreach (DeliveryOrderDetails dtl in this.DeliveryOrderDetails)
                {
                    SalesOrder salesorder;
                    salesorder = Session.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", dtl.SODocNum));

                    if (salesorder != null)
                    {
                        rtn = salesorder.Priority;
                    }
                }

                return rtn;
            }
        }

        private string _SAPDocNum;
        [XafDisplayName("SAP AR Inv Num")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get { return _SAPDocNum; }
            set
            {
                SetPropertyValue("SAPDocNum", ref _SAPDocNum, value);
            }
        }

        private string _SAPDODocNum;
        [XafDisplayName("SAP AR DO Num")]
        [Appearance("SAPDODocNum", Enabled = false)]
        [Index(31), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPDODocNum
        {
            get { return _SAPDODocNum; }
            set
            {
                SetPropertyValue("SAPDODocNum", ref _SAPDODocNum, value);
            }
        }

        private string _CustomerGroup;
        [XafDisplayName("Customer Group")]
        [Appearance("CustomerGroup", Enabled = false)]
        [Index(33), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerGroup
        {
            get { return _CustomerGroup; }
            set
            {
                SetPropertyValue("CustomerGroup", ref _CustomerGroup, value);
            }
        }

        private int _DOPrintCount;
        [XafDisplayName("DO Print Count")]
        [Appearance("DOPrintCount", Enabled = false)]
        [Index(35), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public int DOPrintCount
        {
            get { return _DOPrintCount; }
            set
            {
                SetPropertyValue("DOPrintCount", ref _DOPrintCount, value);
            }
        }

        private DateTime _DOPrintDate;
        [XafDisplayName("DO Last Print Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("DOPrintDate", Enabled = false)]
        [Index(38), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DOPrintDate
        {
            get { return _DOPrintDate; }
            set
            {
                SetPropertyValue("DOPrintDate", ref _DOPrintDate, value);
            }
        }

        private int _INVPrintCount;
        [XafDisplayName("INV Print Count")]
        [Appearance("INVPrintCount", Enabled = false)]
        [Index(40), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public int INVPrintCount
        {
            get { return _INVPrintCount; }
            set
            {
                SetPropertyValue("INVPrintCount", ref _INVPrintCount, value);
            }
        }

        private DateTime _INVPrintDate;
        [XafDisplayName("INV Last Print Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("INVPrintDate", Enabled = false)]
        [Index(43), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime INVPrintDate
        {
            get { return _INVPrintDate; }
            set
            {
                SetPropertyValue("INVPrintDate", ref _INVPrintDate, value);
            }
        }

        private int _BundleDOPrintCount;
        [XafDisplayName("Bundle DO Print Count")]
        [Appearance("BundleDOPrintCount", Enabled = false)]
        [Index(40), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public int BundleDOPrintCount
        {
            get { return _BundleDOPrintCount; }
            set
            {
                SetPropertyValue("BundleDOPrintCount", ref _BundleDOPrintCount, value);
            }
        }

        private DateTime _BundleDOPrintDate;
        [XafDisplayName("Bundle DO Last Print Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("BundleDOPrintDate", Enabled = false)]
        [Index(43), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime BundleDOPrintDate
        {
            get { return _BundleDOPrintDate; }
            set
            {
                SetPropertyValue("BundleDOPrintDate", ref _BundleDOPrintDate, value);
            }
        }

        private bool _Sap;
        [XafDisplayName("Sap")]
        [Index(80), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool Sap
        {
            get { return _Sap; }
            set
            {
                SetPropertyValue("Sap", ref _Sap, value);
            }
        }

        private bool _SapDO;
        [XafDisplayName("SapDO")]
        [Index(81), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool SapDO
        {
            get { return _SapDO; }
            set
            {
                SetPropertyValue("SapDO", ref _SapDO, value);
            }
        }

        private bool _SapINV;
        [XafDisplayName("SapINV")]
        [Index(82), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool SapINV
        {
            get { return _SapINV; }
            set
            {
                SetPropertyValue("SapINV", ref _SapINV, value);
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            { return Session.IsNewObject(this); }
        }

        [Association("DeliveryOrder-DeliveryOrderDetails")]
        [XafDisplayName("Content")]
        public XPCollection<DeliveryOrderDetails> DeliveryOrderDetails
        {
            get { return GetCollection<DeliveryOrderDetails>("DeliveryOrderDetails"); }
        }

        [Association("DeliveryOrder-DeliveryOrderDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<DeliveryOrderDocTrail> DeliveryOrderDocTrail
        {
            get { return GetCollection<DeliveryOrderDocTrail>("DeliveryOrderDocTrail"); }
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
                    DeliveryOrderDocTrail ds = new DeliveryOrderDocTrail(Session);
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
                    this.DeliveryOrderDocTrail.Add(ds);
                }
            }
        }
    }
}