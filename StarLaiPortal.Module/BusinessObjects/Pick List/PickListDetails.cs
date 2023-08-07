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

namespace StarLaiPortal.Module.BusinessObjects.Pick_List
{
    [DefaultClassOptions]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("LinkDoc", AppearanceItemType = "Action", TargetItems = "Link", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("UnlinkDoc", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("Pick List Details")]
    [RuleCriteria("PlanQuantity", DefaultContexts.Save, "IsValid = 0", "Plan Quantity cannot 0.")]

    public class PickListDetails : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PickListDetails(Session session)
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
        }

        private ApplicationUser _CreateUser;
        [XafDisplayName("Create User")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
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
        public DateTime? UpdateDate
        {
            get { return _UpdateDate; }
            set
            {
                SetPropertyValue("UpdateDate", ref _UpdateDate, value);
            }
        }

        private vwItemMasters _ItemCode;
        [ImmediatePostData]
        [NoForeignKey]
        [XafDisplayName("Item Code")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemCode", Enabled = false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public vwItemMasters ItemCode
        {
            get { return _ItemCode; }
            set
            {
                SetPropertyValue("ItemCode", ref _ItemCode, value);
                if (!IsLoading && value != null)
                {
                    LegacyItemCode = ItemCode.LegacyItemCode;
                }
                else if (!IsLoading && value != null)
                {
                    LegacyItemCode = null;
                }
            }
        }

        private string _LegacyItemCode;
        [XafDisplayName("Legacy Item Code")]
        [Index(4), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("LegacyItemCode", Enabled = false)]
        public string LegacyItemCode
        {
            get { return _LegacyItemCode; }
            set
            {
                SetPropertyValue("LegacyItemCode", ref _LegacyItemCode, value);
            }
        }

        private string _ItemDesc;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Item Description")]
        [Appearance("ItemDesc", Enabled = false)]
        [Index(3), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string ItemDesc
        {
            get { return _ItemDesc; }
            set
            {
                SetPropertyValue("ItemDesc", ref _ItemDesc, value);
            }
        }

        private string _CatalogNo;
        [XafDisplayName("Catalog No")]
        [Appearance("CatalogNo", Enabled = false)]
        [Index(6), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CatalogNo
        {
            get { return _CatalogNo; }
            set
            {
                SetPropertyValue("CatalogNo", ref _CatalogNo, value);
            }
        }

        private vwWarehouse _Warehouse;
        [NoForeignKey]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse Warehouse
        {
            get { return _Warehouse; }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
            }
        }

        private decimal _PlanQty;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Plan Qty")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal PlanQty
        {
            get { return _PlanQty; }
            set
            {
                SetPropertyValue("PlanQty", ref _PlanQty, value);
            }
        }

        private decimal _PickQty;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Pick Qty")]
        [Appearance("PickQty", Enabled = false)]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal PickQty
        {
            get 
            {
                //if (Session.IsObjectsSaving != true)
                //{
                //    decimal rtn = 0;

                //    if (PickList != null)
                //    {
                //        foreach (PickListDetailsActual dtl in PickList.PickListDetailsActual)
                //        {
                //            if (dtl.ItemCode == ItemCode.ItemCode && dtl.SOBaseDoc == SOBaseDoc && dtl.SOBaseId == SOBaseId)
                //            {
                //                rtn += dtl.PickQty;
                //            }
                //        }
                //    }

                //    return rtn;
                //}
                //else
                //{
                    return _PickQty;
                //}
            }
            set
            {
                SetPropertyValue("PickQty", ref _PickQty, value);
            }
        }

        private DiscrepancyReason _Reason;
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [XafDisplayName("Discrepancy Reason")]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public DiscrepancyReason Reason
        {
            get { return _Reason; }
            set
            {
                SetPropertyValue("Reason", ref _Reason, value);
            }
        }

        private vwBusniessPartner _Customer;
        [NoForeignKey]
        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Index(18), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public vwBusniessPartner Customer
        {
            get { return _Customer; }
            set
            {
                SetPropertyValue("Customer", ref _Customer, value);
            }
        }

        private string _SOBaseDoc;
        [XafDisplayName("SO Doc Num")]
        [Appearance("SOBaseDoc", Enabled = false)]
        [Index(20), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SOBaseDoc
        {
            get { return _SOBaseDoc; }
            set
            {
                SetPropertyValue("SOBaseDoc", ref _SOBaseDoc, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("SAP Sales Order No.")]
        [Index(21), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("SAPSalesOrderNo", Enabled = false)]
        public string SAPSalesOrderNo
        {
            get
            {
                string rtn = null;

                if (this.SOBaseDoc != null)
                {
                    SalesOrder salesorder;
                    salesorder = Session.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", this.SOBaseDoc));

                    if (salesorder != null)
                    {
                        rtn = salesorder.SAPDocNum;
                    }
                }

                return rtn;
            }
        }

        private string _SOBaseId;
        [XafDisplayName("SOBaseId")]
        [Index(23), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SOBaseId
        {
            get { return _SOBaseId; }
            set
            {
                SetPropertyValue("SOBaseId", ref _SOBaseId, value);
            }
        }

        private DateTime _SOCreateDate;
        [XafDisplayName("SOCreateDate")]
        [Index(25), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime SOCreateDate
        {
            get { return _SOCreateDate; }
            set
            {
                SetPropertyValue("SOCreateDate", ref _SOCreateDate, value);
            }
        }

        private DateTime _SOExpectedDate;
        [XafDisplayName("SOExpectedDate")]
        [Index(28), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime SOExpectedDate
        {
            get { return _SOExpectedDate; }
            set
            {
                SetPropertyValue("SOExpectedDate", ref _SOExpectedDate, value);
            }
        }

        private string _SORemarks;
        [XafDisplayName("SORemarks")]
        [Index(30), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SORemarks
        {
            get { return _SORemarks; }
            set
            {
                SetPropertyValue("SORemarks", ref _SORemarks, value);
            }
        }

        private string _SOSalesperson;
        [XafDisplayName("SOSalesperson")]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SOSalesperson
        {
            get { return _SOSalesperson; }
            set
            {
                SetPropertyValue("SOSalesperson", ref _SOSalesperson, value);
            }
        }

        private string _SOTransporter;
        [XafDisplayName("SOTransporter")]
        [Index(35), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SOTransporter
        {
            get { return _SOTransporter; }
            set
            {
                SetPropertyValue("SOTransporter", ref _SOTransporter, value);
            }
        }

        private PriorityType _Priority;
        [XafDisplayName("Priority")]
        [Index(38), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public PriorityType Priority
        {
            get { return _Priority; }
            set
            {
                SetPropertyValue("Priority", ref _Priority, value);
            }
        }

        private DateTime _SODeliveryDate;
        [XafDisplayName("Delivery Date")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime SODeliveryDate
        {
            get { return _SODeliveryDate; }
            set
            {
                SetPropertyValue("SODeliveryDate", ref _SODeliveryDate, value);
            }
        }

        private bool _CopyTo;
        [XafDisplayName("CopyTo")]
        [Index(43), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
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
                if (this.PlanQty <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        private PickList _PickList;
        [Association("PickList-PickListDetails")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("PickList", Enabled = false)]
        public PickList PickList
        {
            get { return _PickList; }
            set { SetPropertyValue("PickList", ref _PickList, value); }
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

                }
            }
        }
    }
}