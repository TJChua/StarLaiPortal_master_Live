using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Sales_Order
{
    [DefaultClassOptions]
    [XafDisplayName("Sales Order")]
    [NavigationItem("Sales Order")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class SalesOrder : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public SalesOrder(Session session)
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
            DeliveryDate = DateTime.Now;

            DocType = DocTypeList.SO;
            Status = DocStatus.Open;
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

        private string _Cart;
        [XafDisplayName("Cart")]
        [Appearance("Cart", Enabled = false)]
        [Index(0), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Cart
        {
            get { return _Cart; }
            set
            {
                SetPropertyValue("Cart", ref _Cart, value);
            }
        }

        private string _DocNum;
        [XafDisplayName("Order Num")]
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
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'C'")]
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
                    Balance = Customer.Balance;
                    //BillingAddress = Session.FindObject<vwBillingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                    //    , Customer.BillToDef, Customer.BPCode));
                    //ShippingAddress = Session.FindObject<vwShippingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                    //    , Customer.ShipToDef, Customer.BPCode));
                    ContactNo = Customer.Contact;
                    Currency = Customer.Currency;
                }
                else if (!IsLoading && value == null)
                {
                    CustomerName = null;
                    Balance = 0;
                    BillingAddress = null;
                    ShippingAddress = null;
                    ContactNo = null;
                    Currency = null;
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

        private vwTransporter _Transporter;
        [NoForeignKey]
        [XafDisplayName("Transporter")]
        [DataSourceCriteria("IsActive = 'True'")]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwTransporter Transporter
        {
            get { return _Transporter; }
            set
            {
                SetPropertyValue("Transporter", ref _Transporter, value);
            }
        }

        private string _ContactNo;
        [XafDisplayName("Contact No")]
        [Index(13), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string ContactNo
        {
            get { return _ContactNo; }
            set
            {
                SetPropertyValue("ContactNo", ref _ContactNo, value);
            }
        }

        private vwSalesPerson _ContactPerson;
        [NoForeignKey]
        [XafDisplayName("Salesperson")]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwSalesPerson ContactPerson
        {
            get { return _ContactPerson; }
            set
            {
                SetPropertyValue("ContactPerson", ref _ContactPerson, value);
            }
        }

        private decimal _Balance;
        [XafDisplayName("Balance")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(18), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Balance", Enabled = false)]
        public decimal Balance

        {
            get { return _Balance; }
            set
            {
                SetPropertyValue("Balance", ref _Balance, value);
            }
        }

        private vwPaymentTerm _PaymentTerm;
        [NoForeignKey]
        [XafDisplayName("PaymentTerm")]
        [Index(19), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwPaymentTerm PaymentTerm
        {
            get { return _PaymentTerm; }
            set
            {
                SetPropertyValue("PaymentTerm", ref _PaymentTerm, value);
            }
        }

        private vwSeries _Series;
        [NoForeignKey]
        [DataSourceCriteria("ObjectCode = '17'")]
        [XafDisplayName("Series")]
        [Index(20), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwSeries Series
        {
            get { return _Series; }
            set
            {
                SetPropertyValue("Series", ref _Series, value);
            }
        }

        private PriorityType _Priority;
        [XafDisplayName("Priority")]
        [Index(23), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public PriorityType Priority
        {
            get { return _Priority; }
            set
            {
                SetPropertyValue("Priority", ref _Priority, value);
            }
        }

        private DateTime _PostingDate;
        [XafDisplayName("Posting Date")]
        [Index(24), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime PostingDate
        {
            get { return _PostingDate; }
            set
            {
                SetPropertyValue("PostingDate", ref _PostingDate, value);
            }
        }

        private DateTime _DeliveryDate;
        [XafDisplayName("Delivery Date")]
        [Index(25), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime DeliveryDate
        {
            get { return _DeliveryDate; }
            set
            {
                SetPropertyValue("DeliveryDate", ref _DeliveryDate, value);
            }
        }

        private DateTime _ValidUntil;
        [XafDisplayName("Valid Until")]
        [Index(26), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ValidUntil
        {
            get { return _ValidUntil; }
            set
            {
                SetPropertyValue("ValidUntil", ref _ValidUntil, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("DocDate")]
        [Index(28), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private DocStatus _Status;
        [XafDisplayName("SAP SO Status")]
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
                    if (SalesOrderDetails != null)
                        rtn += SalesOrderDetails.Sum(p => p.Total);

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

        private vwBillingAddress _BillingAddress;
        [NoForeignKey]
        [ImmediatePostData]
        [DataSourceCriteria("CardCode = '@this.Customer.BPCode'")]
        [XafDisplayName("Billing Address")]
        [Index(33), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwBillingAddress BillingAddress
        {
            get { return _BillingAddress; }
            set
            {
                SetPropertyValue("BillingAddress", ref _BillingAddress, value);
                if (!IsLoading && value != null)
                {
                    BillingAddressfield = BillingAddress.Address;
                }
            }
        }

        private string _BillingAddressfield;
        [XafDisplayName("Billing Address Field")]
        [Size(254)]
        [Index(35), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string BillingAddressfield
        {
            get { return _BillingAddressfield; }
            set
            {
                SetPropertyValue("BillingAddressfield", ref _BillingAddressfield, value);
            }
        }

        private vwShippingAddress _ShippingAddress;
        [NoForeignKey]
        [ImmediatePostData]
        [DataSourceCriteria("CardCode = '@this.Customer.BPCode'")]
        [XafDisplayName("Shipping Address")]
        [Index(38), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwShippingAddress ShippingAddress
        {
            get { return _ShippingAddress; }
            set
            {
                SetPropertyValue("ShippingAddress", ref _ShippingAddress, value);
                if (!IsLoading && value != null)
                {
                    ShippingAddressfield = ShippingAddress.Address;
                }
            }
        }

        private string _ShippingAddressfield;
        [XafDisplayName("Shipping Address Field")]
        [Size(254)]
        [Index(40), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string ShippingAddressfield
        {
            get { return _ShippingAddressfield; }
            set
            {
                SetPropertyValue("ShippingAddressfield", ref _ShippingAddressfield, value);
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(43), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private string _SAPDocNum;
        [XafDisplayName("SAP SO No.")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(45), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get { return _SAPDocNum; }
            set
            {
                SetPropertyValue("SAPDocNum", ref _SAPDocNum, value);
            }
        }

        private string _Currency;
        [ImmediatePostData]
        [XafDisplayName("Currency")]
        [Appearance("Currency", Enabled = false)]
        [Index(48), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Currency
        {
            get { return _Currency; }
            set
            {
                SetPropertyValue("Currency", ref _Currency, value);
                if (!IsLoading && value != null)
                {
                    vwExchangeRate temprate = Session.FindObject<vwExchangeRate>(CriteriaOperator.Parse("RateDate = ? and Currency = ?",
                          PostingDate.Date, Currency));

                    if (temprate != null)
                    {
                        CurrencyRate = temprate.Rate;
                    }
                    else
                    {
                        if (Currency == "MYR")
                        {
                            CurrencyRate = 1;
                        }
                    }
                }
                else if (!IsLoading && value == null)
                {
                    CurrencyRate = 0;
                }
            }
        }

        private decimal _CurrencyRate;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Currency Rate")]
        [Appearance("CurrencyRate", Enabled = false)]
        [Index(50), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal CurrencyRate
        {
            get { return _CurrencyRate; }
            set
            {
                SetPropertyValue("CurrencyRate", ref _CurrencyRate, value);
            }
        }

        private string _Attn;
        [XafDisplayName("Attn")]
        [Index(53), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Attn
        {
            get { return _Attn; }
            set
            {
                SetPropertyValue("Attn", ref _Attn, value);
            }
        }

        private string _RefNo;
        [XafDisplayName("Reference")]
        [Index(55), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string RefNo
        {
            get { return _RefNo; }
            set
            {
                SetPropertyValue("RefNo", ref _RefNo, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("SQ No.")]
        [Index(15), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("SQNumber", Enabled = false)]
        public string SQNumber
        {
            get
            {
                string rtn = null;
                foreach (SalesOrderDetails dtl in this.SalesOrderDetails)
                {
                    if (dtl.BaseDoc != null)
                    {
                        rtn = dtl.BaseDoc;
                        break;
                    }
                }

                return rtn;
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
                if (SalesOrderDetails.GroupBy(x => x.Location).Count() > 1)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("SalesOrder-SalesOrderDetails")]
        [XafDisplayName("Items")]
        public XPCollection<SalesOrderDetails> SalesOrderDetails
        {
            get { return GetCollection<SalesOrderDetails>("SalesOrderDetails"); }
        }

        [Association("SalesOrder-SalesOrderDocStatus")]
        [XafDisplayName("Document Trail")]
        public XPCollection<SalesOrderDocStatus> SalesOrderDocStatus
        {
            get { return GetCollection<SalesOrderDocStatus>("SalesOrderDocStatus"); }
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
                    SalesOrderDocStatus ds = new SalesOrderDocStatus(Session);
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
                    this.SalesOrderDocStatus.Add(ds);
                }
            }
        }
    }
}