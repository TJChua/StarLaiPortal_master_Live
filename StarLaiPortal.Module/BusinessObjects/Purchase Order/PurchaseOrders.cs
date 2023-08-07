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

// 2023-07-20 - hide copy from so button for series indent - ver 1.0.6 (LIVE)


namespace StarLaiPortal.Module.BusinessObjects.Purchase_Order
{
    [DefaultClassOptions]
    [XafDisplayName("Purchase Order")]
    [NavigationItem("Purchase Order")]
    [DefaultProperty("DocNum")]
    [Appearance("HideNew", AppearanceItemType = "Action", TargetItems = "New", Context = "PurchaseOrders_ListView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("HideNew1", AppearanceItemType = "Action", TargetItems = "New", Criteria = "(AppStatus in (2))", Context = "PurchaseOrders_DetailView_Approval", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit1", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitPO", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit1", AppearanceItemType.Action, "True", TargetItems = "SubmitPO", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelPO", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel1", AppearanceItemType.Action, "True", TargetItems = "CancelPO", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideDup", AppearanceItemType.Action, "True", TargetItems = "DuplicatePO", Criteria = "(AppStatus in (2))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "PurchaseOrders_DetailView_Approval")]

    [Appearance("HideItemInq", AppearanceItemType.Action, "True", TargetItems = "POInquiryItem", Criteria = "Supplier = null or Warehouse = null or Series = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCopyFromSO", AppearanceItemType.Action, "True", TargetItems = "POCopyFromSO", Criteria = "Supplier = null or Warehouse = null or Series = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    // Start ver 1.0.6 (LIVE)
    [Appearance("HideCopyFromSO1", AppearanceItemType.Action, "True", TargetItems = "POCopyFromSO", Criteria = "Series.SeriesName = 'Indent'", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    // End ver 1.0.6 (LIVE) 

    public class PurchaseOrders : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PurchaseOrders(Session session)
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

            DocType = DocTypeList.PO;
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
        [XafDisplayName("PO Num")]
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

        private vwBusniessPartner _Supplier;
        [XafDisplayName("Supplier")]
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'S'")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Supplier
        {
            get { return _Supplier; }
            set
            {
                SetPropertyValue("Supplier", ref _Supplier, value);
                if (!IsLoading && value != null)
                {
                    bool update = false;
                    SupplierName = Supplier.BPName;
                    BillingAddress = Session.FindObject<vwBillingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                        , Supplier.BillToDef, Supplier.BPCode));
                    ShippingAddress = Session.FindObject<vwShippingAddress>(CriteriaOperator.Parse("AddressKey = ? and CardCode = ?"
                        , Supplier.ShipToDef, Supplier.BPCode));
                    if (Supplier.PaymentTerm != null)
                    {
                        PaymentTerm = Session.FindObject<vwPaymentTerm>(CriteriaOperator.Parse("GroupNum = ?", Supplier.PaymentTerm.GroupNum));
                    }
                    Currency = Supplier.Currency;
                    if (Supplier.SlpCode != null)
                    {
                        ContactPerson = Session.FindObject<vwSalesPerson>(CriteriaOperator.Parse("SlpCode = ?", Supplier.SlpCode.SlpCode));
                    }
                    ContactNo = Supplier.Contact;
                    DeliveryDate = PostingDate.AddDays(Supplier.LeadTime);

                    foreach (PurchaseOrderDetails dtl in this.PurchaseOrderDetails)
                    {
                        dtl.Supplier = dtl.Session.FindObject<vwBusniessPartner>(CriteriaOperator.Parse("BPCode = ?"
                            , Supplier.BPCode));
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        dtl.ItemCode.ItemCode, Supplier.ListNum, PostingDate.Date, PostingDate.Date, dtl.Quantity, dtl.Quantity));

                        if (tempprice != null)
                        {
                            dtl.AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                dtl.ItemCode.ItemCode, Supplier.ListNum));

                            if (temppricelist != null)
                            {
                                dtl.AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
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
                    SupplierName = null;
                    BillingAddress = null;
                    ShippingAddress = null;
                    PaymentTerm = null;
                    ContactNo = null;
                    Currency = null;
                    DeliveryDate = DateTime.Now;
                }
            }
        }

        private string _SupplierName;
        [XafDisplayName("Supplier Name")]
        [Appearance("SupplierName", Enabled = false)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SupplierName
        {
            get { return _SupplierName; }
            set
            {
                SetPropertyValue("SupplierName", ref _SupplierName, value);
            }
        }

        private vwSalesPerson _ContactPerson;
        [NoForeignKey]
        [XafDisplayName("Salesperson")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Active = 'Y'")]
        [Index(9), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwSalesPerson ContactPerson
        {
            get { return _ContactPerson; }
            set
            {
                SetPropertyValue("ContactPerson", ref _ContactPerson, value);
            }
        }

        private string _ContactNo;
        [XafDisplayName("Contact No")]
        [Index(10), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string ContactNo
        {
            get { return _ContactNo; }
            set
            {
                SetPropertyValue("ContactNo", ref _ContactNo, value);
            }
        }

        private vwPaymentTerm _PaymentTerm;
        [NoForeignKey]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [XafDisplayName("Payment Term")]
        [Appearance("PaymentTerm", Enabled = false)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwPaymentTerm PaymentTerm
        {
            get { return _PaymentTerm; }
            set
            {
                SetPropertyValue("PaymentTerm", ref _PaymentTerm, value);
            }
        }

        private DateTime _PostingDate;
        [ImmediatePostData]
        [XafDisplayName("Posting Date")]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime PostingDate
        {
            get { return _PostingDate; }
            set
            {
                SetPropertyValue("PostingDate", ref _PostingDate, value);
                if (!IsLoading)
                {
                    if (Supplier != null)
                    {
                        DeliveryDate = PostingDate.AddDays(Supplier.LeadTime);
                    }

                    bool update = false;
                    foreach (PurchaseOrderDetails dtl in this.PurchaseOrderDetails)
                    {
                        dtl.Postingdate = PostingDate;
                        vwPriceWithVolumeDiscount tempprice = Session.FindObject<vwPriceWithVolumeDiscount>(CriteriaOperator.Parse(
                        "ItemCode = ? and ListNum = ? and ? >= FromDate and ? <= ToDate and ? >= FromQty and ? <= ToQty",
                        dtl.ItemCode.ItemCode, Supplier.ListNum, PostingDate.Date, PostingDate.Date, dtl.Quantity, dtl.Quantity));

                        if (tempprice != null)
                        {
                            dtl.AdjustedPrice = tempprice.Price;
                        }
                        else
                        {
                            vwPrice temppricelist = Session.FindObject<vwPrice>(CriteriaOperator.Parse("ItemCode = ? and PriceList = ?",
                                dtl.ItemCode.ItemCode, Supplier.ListNum));

                            if (temppricelist != null)
                            {
                                dtl.AdjustedPrice = temppricelist.Price;
                            }
                            else
                            {
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
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DeliveryDate
        {
            get { return _DeliveryDate; }
            set
            {
                SetPropertyValue("DeliveryDate", ref _DeliveryDate, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("DocDate")]
        [Index(20), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private string _SAPPOStatus;
        [XafDisplayName("SAP PO Status")]
        [Appearance("SAPPOStatus", Enabled = false)]
        [Index(22), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPPOStatus
        {
            get { return _SAPPOStatus; }
            set
            {
                SetPropertyValue("SAPPOStatus", ref _SAPPOStatus, value);
            }
        }

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(23), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
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
        [Index(24), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public ApprovalStatusType AppStatus
        {
            get { return _AppStatus; }
            set
            {
                SetPropertyValue("AppStatus", ref _AppStatus, value);
            }
        }

        private PrintStatus _PrintStatus;
        [XafDisplayName("Print Status")]
        [Appearance("PrintStatus", Enabled = false)]
        [Index(25), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public PrintStatus PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetPropertyValue("PrintStatus", ref _PrintStatus, value);
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
                    if (PurchaseOrderDetails != null)
                        rtn += PurchaseOrderDetails.Sum(p => p.Total);

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
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("CardCode = '@this.Supplier.BPCode'")]
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
        [DataSourceCriteria("CardCode = '@this.Supplier.BPCode'")]
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

        private vwSeries _Series;
        [NoForeignKey]
        [ImmediatePostData]
        [DataSourceCriteria("ObjectCode = '22'")]
        [XafDisplayName("Series")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(45), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwSeries Series
        {
            get { return _Series; }
            set
            {
                SetPropertyValue("Series", ref _Series, value);
            }
        }

        private vwWarehouse _Warehouse;
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        //[Appearance("Warehouse", Enabled = false, Criteria = "IsValid1")]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Warehouse")]
        [Index(48), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse Warehouse
        {
            get { return _Warehouse; }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
                if (!IsLoading)
                {
                    bool update = false;
                    foreach (PurchaseOrderDetails dtl in this.PurchaseOrderDetails)
                    {
                        if (this.Warehouse != null)
                        {
                            dtl.Location = Session.FindObject<vwWarehouse>(CriteriaOperator.Parse("WarehouseCode = ?",
                               this.Warehouse.WarehouseCode));
                        }
                        else
                        {
                            dtl.Location = null;
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

        private string _Currency;
        [ImmediatePostData]
        [XafDisplayName("Currency")]
        [Appearance("Currency", Enabled = false)]
        [Index(50), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
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
        [Index(53), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal CurrencyRate
        {
            get { return _CurrencyRate; }
            set
            {
                SetPropertyValue("CurrencyRate", ref _CurrencyRate, value);
            }
        }

        private string _SAPDocNum;
        [XafDisplayName("SAP PO No.")]
        [Appearance("SAPDocNum", Enabled = false)]
        [Index(48), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get { return _SAPDocNum; }
            set
            {
                SetPropertyValue("SAPDocNum", ref _SAPDocNum, value);
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
                if (PurchaseOrderDetails.GroupBy(x => x.Location).Count() > 1)
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
                foreach (PurchaseOrderDetails dtl in this.PurchaseOrderDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("PurchaseOrders-PurchaseOrderDetails")]
        [XafDisplayName("Items")]
        [Appearance("PurchaseOrderDetails", Enabled = false, Criteria = "Warehouse = null")]
        public XPCollection<PurchaseOrderDetails> PurchaseOrderDetails
        {
            get { return GetCollection<PurchaseOrderDetails>("PurchaseOrderDetails"); }
        }

        [Association("PurchaseOrders-PurchaseOrderDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<PurchaseOrderDocTrail> PurchaseOrderDocTrail
        {
            get { return GetCollection<PurchaseOrderDocTrail>("PurchaseOrderDocTrail"); }
        }

        [Association("PurchaseOrders-PurchaseOrderAppStage")]
        [XafDisplayName("Approval Stage")]
        public XPCollection<PurchaseOrderAppStage> PurchaseOrderAppStage
        {
            get { return GetCollection<PurchaseOrderAppStage>("PurchaseOrderAppStage"); }
        }

        [Association("PurchaseOrders-PurchaseOrderAppStatus")]
        [XafDisplayName("Approval Status")]
        public XPCollection<PurchaseOrderAppStatus> PurchaseOrderAppStatus
        {
            get { return GetCollection<PurchaseOrderAppStatus>("PurchaseOrderAppStatus"); }
        }

        [Association("PurchaseOrders-PurchaseOrderAttachment")]
        [XafDisplayName("Attachment")]
        public XPCollection<PurchaseOrderAttachment> PurchaseOrderAttachment
        {
            get { return GetCollection<PurchaseOrderAttachment>("PurchaseOrderAttachment"); }
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
                    PurchaseOrderDocTrail ds = new PurchaseOrderDocTrail(Session);
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
                    this.PurchaseOrderDocTrail.Add(ds);
                }
            }
        }
    }
}