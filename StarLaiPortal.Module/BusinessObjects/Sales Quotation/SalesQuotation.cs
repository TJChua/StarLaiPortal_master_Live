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

// 2023-07-28 block submit if no address for OC and OS ver 1.0.7

namespace StarLaiPortal.Module.BusinessObjects.Sales_Quotation
{
    [DefaultClassOptions]
    [XafDisplayName("Sales Quotation")]
    [NavigationItem("Sales Quotation")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "SalesQuotation_ListView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideNew1", AppearanceItemType = "Action", TargetItems = "New", Criteria = "(AppStatus in (2))", Context = "SalesQuotation_DetailView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit1", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "CreateSalesOrder", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit1", AppearanceItemType.Action, "True", TargetItems = "CreateSalesOrder", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelSalesOrder", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel1", AppearanceItemType.Action, "True", TargetItems = "CancelSalesOrder", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDup", AppearanceItemType.Action, "True", TargetItems = "DuplicateSQ", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "SalesQuotation_DetailView_Approval")]

    [Appearance("HideItemInq", AppearanceItemType.Action, "True", TargetItems = "InquiryItem", Criteria = "Customer = null or Transporter = null or CustomerName = null or ContactNo = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class SalesQuotation : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public SalesQuotation(Session session)
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

            DocType = DocTypeList.SQ;
            Priority = Session.FindObject<PriorityType>(CriteriaOperator.Parse("PriorityName = ? and IsActive= ?",
                         "Normal", "True"));
            Status = DocStatus.Draft;
            AppStatus = ApprovalStatusType.Not_Applicable;
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
        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Index(301), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
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
        [XafDisplayName("Quotation Num")]
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
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Customer
        {
            get { return _Customer; }
            set
            {
                SetPropertyValue("Customer", ref _Customer, value);
                if (!IsLoading && value != null)
                {
                    bool update = false;
                    //if (Customer.GroupName != "Trade Debtor - Cash")
                    //{
                        CustomerName = Customer.BPName;
                    //}
                    //else
                    //{
                    //    CustomerName = null;
                    //}
                    Balance = Customer.Balance;
                    BillingAddress = Session.FindObject<vwBillingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                        , Customer.BillToDef, Customer.BPCode));
                    ShippingAddress = Session.FindObject<vwShippingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                        , Customer.ShipToDef, Customer.BPCode));
                    if (Customer.PaymentTerm != null)
                    {
                        PaymentTerm = Session.FindObject<vwPaymentTerm>(CriteriaOperator.Parse("GroupNum = ?", Customer.PaymentTerm.GroupNum));
                    }
                    ContactNo = Customer.Contact;
                    Currency = Customer.Currency;
                    if (Customer.Transporter != null)
                    {
                        Transporter = Session.FindObject<vwTransporter>(CriteriaOperator.Parse("TransporterID = ?", Customer.Transporter.TransporterID));
                    }
                    if (Customer.SlpCode != null)
                    {
                        ContactPerson = Session.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpCode = ?", Customer.SlpCode.SlpCode));
                    }
                    if (Customer.SalesOrderSeries != null)
                    {
                        Series = Session.FindObject<vwSeries>(CriteriaOperator.Parse("Series = ?", Customer.SalesOrderSeries));
                    }

                    foreach(SalesQuotationDetails dtl in this.SalesQuotationDetails)
                    {
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        dtl.ItemCode, Customer.ListNum, PostingDate.Date, PostingDate.Date, dtl.Quantity, dtl.Quantity));

                        if (tempprice != null)
                        {
                            dtl.Price = tempprice.Price;
                            dtl.AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                dtl.ItemCode.ItemCode, Customer.ListNum));

                            if (temppricelist != null)
                            {
                                dtl.Price = temppricelist.Price;
                                dtl.AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                dtl.Price = 0;
                                dtl.AdjustedPrice = 0;
                            }
                        }
                        
                        update = true;
                    }

                    if (update == true)
                    {
                        this.Session.CommitTransaction();
                    }
                }
                else if (!IsLoading && value == null)
                {
                    CustomerName = null;
                    Balance = 0;
                    BillingAddress = null;
                    ShippingAddress = null;
                    PaymentTerm = null;
                    ContactNo = null;
                    Currency = null;
                    Transporter = null;
                    ContactPerson = null;
                    Series = null;
                }
            }
        }

        private string _CustomerName;
        [ImmediatePostData]
        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false, Criteria = "Customer.GroupName != 'Trade Debtor - Cash'")]
        [RuleRequiredField(DefaultContexts.Save)]
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
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Transporter")]
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
        [ImmediatePostData]
        [XafDisplayName("Contact No")]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "Customer.GroupName = 'Trade Debtor - Cash'")]
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
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Active = 'Y'")]
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
        [XafDisplayName("Credit Balance")]
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
        [XafDisplayName("Payment Term")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Appearance("PaymentTerm", Enabled = false)]
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
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("ObjectCode = '17' and (('@this.Customer.GroupName' != 'Trade Debtor - Cash' and SeriesName != 'Cash') " +
            "or ('@this.Customer.GroupName' = 'Trade Debtor - Cash'))")]
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
                if (!IsLoading)
                {
                    bool update = false;
                    foreach (SalesQuotationDetails dtl in this.SalesQuotationDetails)
                    {
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        dtl.ItemCode, Customer.ListNum, PostingDate.Date, PostingDate.Date, dtl.Quantity, dtl.Quantity));

                        if (tempprice != null)
                        {
                            dtl.Price = tempprice.Price;
                            dtl.AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                dtl.ItemCode.ItemCode, Customer.ListNum));

                            if (temppricelist != null)
                            {
                                dtl.Price = temppricelist.Price;
                                dtl.AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
                                dtl.Price = 0;
                                dtl.AdjustedPrice = 0;
                            }
                        }
                        
                        update = true;
                    }

                    if (update == true)
                    {
                        this.Session.CommitTransaction();
                    }
                }
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
        [Index(27), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private ApprovalStatusType _AppStatus;
        [XafDisplayName("Approval Status")]
        [Appearance("AppStatus", Enabled = false)]
        [Index(28), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public ApprovalStatusType AppStatus
        {
            get { return _AppStatus; }
            set
            {
                SetPropertyValue("AppStatus", ref _AppStatus, value);
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
                    if (SalesQuotationDetails != null)
                        rtn += SalesQuotationDetails.Sum(p => p.Total);

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
        [LookupEditorMode(LookupEditorMode.AllItems)]
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
                else if (!IsLoading && value == null)
                {
                    BillingAddressfield = null;
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
        [LookupEditorMode(LookupEditorMode.AllItems)]
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
                else if (!IsLoading && value == null)
                {
                    ShippingAddressfield = null;
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

        private string _Currency;
        [ImmediatePostData]
        [XafDisplayName("Currency")]
        [Appearance("Currency", Enabled = false)]
        [Index(45), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
        [Index(48), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal CurrencyRate
        {
            get { return _CurrencyRate; }
            set
            {
                SetPropertyValue("CurrencyRate", ref _CurrencyRate, value);
            }
        }

        private bool _PriceChange;
        [XafDisplayName("Price Change")]
        [Appearance("PriceChange", Enabled = false)]
        [Index(50), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public bool PriceChange
        {
            get { return _PriceChange; }
            set
            {
                SetPropertyValue("PriceChange", ref _PriceChange, value);
            }
        }

        private bool _ExceedPrice;
        [XafDisplayName("Exceed Limit")]
        [Appearance("ExceedPrice", Enabled = false)]
        [Index(53), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public bool ExceedPrice
        {
            get { return _ExceedPrice; }
            set
            {
                SetPropertyValue("ExceedPrice", ref _ExceedPrice, value);
            }
        }

        private bool _ExceedCreditControl;
        [XafDisplayName("Exceed Credit Term")]
        [Appearance("ExceedCreditControl", Enabled = false)]
        [Index(55), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public bool ExceedCreditControl
        {
            get { return _ExceedCreditControl; }
            set
            {
                SetPropertyValue("ExceedCreditControl", ref _ExceedCreditControl, value);
            }
        }

        private string _Attn;
        [XafDisplayName("Attn")]
        [Index(58), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
        [Index(60), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string RefNo
        {
            get { return _RefNo; }
            set
            {
                SetPropertyValue("RefNo", ref _RefNo, value);
            }
        }

        private string _AppUser;
        [XafDisplayName("AppUser")]
        [Index(80), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string AppUser
        {
            get { return _AppUser; }
            set
            {
                SetPropertyValue("AppUser", ref _AppUser, value);
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
                if (SalesQuotationDetails.GroupBy(x => x.Location).Count() > 1)
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
                foreach (SalesQuotationDetails dtl in this.SalesQuotationDetails)
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
                if (this.Series == null || this.ContactPerson == null)
                {
                    return true;
                }

                if (this.ContactPerson != null)
                {
                    if (this.ContactPerson.SlpCode == -1)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid3
        {
            get
            {
                foreach (SalesQuotationDetails dtl in this.SalesQuotationDetails)
                {
                   if (dtl.Total <= 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid4
        {
            get
            {
                foreach (SalesQuotationDetails dtl in this.SalesQuotationDetails)
                {
                    if (dtl.Quantity > (decimal)dtl.Available.InStock)
                    {
                        if (this.Series.SeriesName != "BackOrdP" && this.Series.SeriesName != "BackOrdS")
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid5
        {
            get
            {
                if (this.Customer.GroupName != "Trade Debtor - Cash" && this.Series.SeriesName.ToUpper() == "CASH")
                {
                    return true;
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid6
        {
            get
            {
                if (this.ContactPerson != null)
                {
                    if (this.ContactPerson.Active == "N")
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        // Start ver 1.0.7
        [Browsable(false)]
        public bool IsValid7
        {
            get
            {
                if (this.Transporter.U_Type == "OC" || this.Transporter.U_Type == "OS")
                {
                    if (this.BillingAddressfield == null || this.ShippingAddressfield == null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        [Browsable(false)]
        public bool IsValid8
        {
            get
            {
                if (this.Priority == null)
                {
                    return true;
                }

                return false;
            }
        }
        // End ver 1.0.7

        [Association("SalesQuotation-SalesQuotationDetails")]
        [XafDisplayName("Items")]
        public XPCollection<SalesQuotationDetails> SalesQuotationDetails
        {
            get { return GetCollection<SalesQuotationDetails>("SalesQuotationDetails"); }
        }

        [Association("SalesQuotation-SalesQuotationDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<SalesQuotationDocTrail> SalesQuotationDocTrail
        {
            get { return GetCollection<SalesQuotationDocTrail>("SalesQuotationDocTrail"); }
        }

        [Association("SalesQuotation-SalesQuotationAppStage")]
        [XafDisplayName("Approval Stage")]
        public XPCollection<SalesQuotationAppStage> SalesQuotationAppStage
        {
            get { return GetCollection<SalesQuotationAppStage>("SalesQuotationAppStage"); }
        }

        [Association("SalesQuotation-SalesQuotationAppStatus")]
        [XafDisplayName("Approval Status")]
        public XPCollection<SalesQuotationAppStatus> SalesQuotationAppStatus
        {
            get { return GetCollection<SalesQuotationAppStatus>("SalesQuotationAppStatus"); }
        }

        [Association("SalesQuotation-SalesQuotationAttachment")]
        [XafDisplayName("Attachment")]
        public XPCollection<SalesQuotationAttachment> SalesQuotationAttachment
        {
            get { return GetCollection<SalesQuotationAttachment>("SalesQuotationAttachment"); }
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
                    SalesQuotationDocTrail ds = new SalesQuotationDocTrail(Session);
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
                    this.SalesQuotationDocTrail.Add(ds);
                }
            }
        }
    }
}