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

namespace StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection
{
    [DefaultClassOptions]
    [XafDisplayName("AR Downpayment")]
    [NavigationItem("Sales Order")]
    [DefaultProperty("DocNum")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitSOC", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelSOC", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCopyFromSO", AppearanceItemType.Action, "True", TargetItems = "SOCCopyFromSO", Criteria = "PaymentType = null or Customer = null or ReferenceNum = null or " +
        "(PaymentType.PaymentCode = 'CHEQUE' and (ChequeBank = null or CheckNum = null)) or (PaymentType.PaymentCode = 'CREDITCARD' and (CreditCardNum = null or CreditCardValidUntil = null))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class SalesOrderCollection : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public SalesOrderCollection(Session session)
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
            Status = DocStatus.Draft;

            DocType = DocTypeList.ARD;
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
        [XafDisplayName("Doc Num")]
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
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'C' and (GroupName = 'Trade Debtor - Cash')")]
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
                }
                else if (!IsLoading && value == null)
                {
                    CustomerName = null;
                }
            }
        }

        private string _CustomerName;
        [XafDisplayName("Customer Name")]
        //[Appearance("CustomerName", Enabled = false)]
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
        [XafDisplayName("Doc Date")]
        [Index(9), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public DateTime DocDate

        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("DocDate", ref _DocDate, value);
            }
        }

        private vwPaymentType _PaymentType;
        [NoForeignKey]
        [ImmediatePostData]
        [XafDisplayName("Payment Type")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwPaymentType PaymentType
        {
            get { return _PaymentType; }
            set
            {
                SetPropertyValue("PaymentType", ref _PaymentType, value);
                if (!IsLoading)
                {
                    ChequeBank = null;
                    CheckNum = null;
                    CreditCardNum = null;
                    CreditCardValidUntil = null;
                }
            }
        }

        private vwLocalBank _ChequeBank;
        [NoForeignKey]
        [ImmediatePostData]
        [XafDisplayName("Cheque Bank")]
        [Appearance("ChequeBank", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "PaymentType.PaymentMean != 'CHEQUE'")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "PaymentType.PaymentMean = 'CHEQUE'")]
        [Index(11), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwLocalBank ChequeBank
        {
            get { return _ChequeBank; }
            set
            {
                SetPropertyValue("ChequeBank", ref _ChequeBank, value);
            }
        }

        private string _ReferenceNum;
        [ImmediatePostData]
        [XafDisplayName("Reference Num.")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string ReferenceNum
        {
            get { return _ReferenceNum; }
            set
            {
                SetPropertyValue("ReferenceNum", ref _ReferenceNum, value);
            }
        }

        private string _CreditCardNum;
        [ImmediatePostData]
        [XafDisplayName("Credit Card Num.")]
        [ModelDefault("DisplayFormat", "{0:d}")]
        [ModelDefault("EditMask", "0000")]
        [Appearance("CreditCardNum", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "PaymentType.PaymentMean != 'CCARD'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "PaymentType.PaymentMean = 'CCARD'")]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CreditCardNum
        {
            get { return _CreditCardNum; }
            set
            {
                SetPropertyValue("CreditCardNum", ref _CreditCardNum, value);
            }
        }

        private string _CreditCardValidUntil;
        [ImmediatePostData]
        [XafDisplayName("Card Valid Until")]
        //[Appearance("CreditCardValidUntil", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "PaymentType.PaymentMean != 'CCARD'")]
        [Appearance("CreditCardValidUntil", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        //[RuleRequiredField(DefaultContexts.Save, TargetCriteria = "PaymentType.PaymentMean = 'CCARD'")]
        [ModelDefault("EditMask", "00/00")]
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CreditCardValidUntil
        {
            get { return _CreditCardValidUntil; }
            set
            {
                SetPropertyValue("CreditCardValidUntil", ref _CreditCardValidUntil, value);
            }
        }

        private string _CheckNum;
        [ImmediatePostData]
        [XafDisplayName("Cheque Num.")]
        [ModelDefault("DisplayFormat", "{0:d}")]
        [ModelDefault("EditMask", "000000")]
        [Appearance("CheckNum", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "PaymentType.PaymentMean != 'CHEQUE'")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "PaymentType.PaymentMean = 'CHEQUE'")]
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CheckNum
        {
            get { return _CheckNum; }
            set
            {
                SetPropertyValue("CheckNum", ref _CheckNum, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("SO No.")]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("SONumber", Enabled = false)]
        public string SONumber
        {
            get
            {
                string rtn = null;
                string dupso = null;
                foreach (SalesOrderCollectionDetails dtl in this.SalesOrderCollectionDetails)
                {
                    if (dupso != dtl.SalesOrder)
                    {
                        if (rtn == null)
                        {
                            rtn = dtl.SalesOrder;
                        }
                        else
                        {
                            rtn = rtn + ", " + dtl.SalesOrder;
                        }

                        dupso = dtl.SalesOrder;
                    }
                }

                return rtn;
            }
        }

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(29), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DocStatus Status
        {
            get { return _Status; }
            set
            {
                SetPropertyValue("Status", ref _Status, value);
            }
        }

        private decimal _Total;
        [XafDisplayName("Total")]
        [Appearance("Total", Enabled = false)]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(30), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal Total
        {
            get
            {
                if (Session.IsObjectsSaving != true)
                {
                    decimal rtn = 0;
                    if (SalesOrderCollectionDetails != null)
                        rtn += SalesOrderCollectionDetails.Sum(p => p.Total);

                    return rtn;
                }
                else
                {
                    return _Total;
                }
            }
            set
            {
                SetPropertyValue("Total", ref _Total, value);
            }
        }

        private decimal _PaymentAmount;
        [XafDisplayName("Payment Amount")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(33), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal PaymentAmount
        {
            get { return _PaymentAmount; }
            set
            {
                SetPropertyValue("PaymentAmount", ref _PaymentAmount, value);
            }
        }

        private bool _Sap;
        [XafDisplayName("Sap")]
        [Index(35), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
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
                foreach (SalesOrderCollectionDetails dtl in this.SalesOrderCollectionDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid1
        {
            get
            {
                foreach (SalesOrderCollectionDetails dtl in this.SalesOrderCollectionDetails)
                {
                    if (dtl.PaymentAmount <= 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [Association("SalesOrderCollection-SalesOrderCollectionDetails")]
        [XafDisplayName("Sales Order")]
        public XPCollection<SalesOrderCollectionDetails> SalesOrderCollectionDetails
        {
            get { return GetCollection<SalesOrderCollectionDetails>("SalesOrderCollectionDetails"); }
        }

        [Association("SalesOrderCollection-SalesOrderCollectionDocStatus")]
        [XafDisplayName("Status History")]
        public XPCollection<SalesOrderCollectionDocStatus> SalesOrderCollectionDocStatus
        {
            get { return GetCollection<SalesOrderCollectionDocStatus>("SalesOrderCollectionDocStatus"); }
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
                    SalesOrderCollectionDocStatus ds = new SalesOrderCollectionDocStatus(Session);
                    ds.DocStatus = DocStatus.Draft;
                    ds.DocRemarks = "";
                    if (user != null)
                    {
                        ds.CreateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                        ds.UpdateUser = Session.GetObjectByKey<ApplicationUser>(user.Oid);
                    }
                    else
                    {
                        UpdateUser = Session.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                    }
                    ds.CreateDate = DateTime.Now;
                    ds.UpdateDate = DateTime.Now;
                    this.SalesOrderCollectionDocStatus.Add(ds);
                }
            }
        }
    }
}