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
    [XafDisplayName("Pick List Details Actual")]
    public class PickListDetailsActual : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PickListDetailsActual(Session session)
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
        [XafDisplayName("ItemCode")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemCode", Enabled = false,Criteria = "not IsNew")]
        [RuleRequiredField(DefaultContexts.Save)]
        public vwItemMasters ItemCode

        {
            get { return _ItemCode; }
            set
            {
                SetPropertyValue("ItemCode", ref _ItemCode, value);
                if(!IsLoading && value != null)
                {
                    ItemDesc = ItemCode.ItemName;
                    CatalogNo = ItemCode.CatalogNo;
                }
                else if(!IsLoading && value == null)
                {
                    ItemDesc = null;
                    CatalogNo = null;
                }
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
        [ImmediatePostData]
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
                if (!IsLoading && value != null)
                {
                    FromBin = Session.FindObject<vwBin>(CriteriaOperator.Parse("AbsEntry = ?", Warehouse.DftBinAbs));
                }
            }
        }

        private decimal _PickQty;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Pick Qty")]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal PickQty
        {
            get { return _PickQty; }
            set
            {
                SetPropertyValue("PickQty", ref _PickQty, value);
                if (!IsLoading)
                {
                    if (PickQty <= 0)
                    {
                        PickQty = 1;
                    }
                }
            }
        }

        private vwBin _FromBin;
        [NoForeignKey]
        [XafDisplayName("From Bin")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Warehouse = '@this.Warehouse.WarehouseCode'")]
        //[Appearance("FromBin", Enabled = false)]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwBin FromBin
        {
            get { return _FromBin; }
            set
            {
                SetPropertyValue("FromBin", ref _FromBin, value);
            }
        }

        private vwBin _ToBin;
        [NoForeignKey]
        [XafDisplayName("To Bin")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("Warehouse = '@this.Warehouse.WarehouseCode'")]
        //[Appearance("ToBin", Enabled = false)]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwBin ToBin
        {
            get { return _ToBin; }
            set
            {
                SetPropertyValue("ToBin", ref _ToBin, value);
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(19), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private string _SOBaseDoc;
        [XafDisplayName("SO Doc Num")]
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

        private string _SAPDocNum;
        [XafDisplayName("SAP Doc Num")]
        [Index(43), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string SAPDocNum
        {
            get { return _SAPDocNum; }
            set
            {
                SetPropertyValue("SAPDocNum", ref _SAPDocNum, value);
            }
        }

        private int _PickListDetailOid;
        [XafDisplayName("PickListDetailOid")]
        [Index(45), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int PickListDetailOid
        {
            get { return _PickListDetailOid; }
            set
            {
                SetPropertyValue("PickListDetailOid", ref _PickListDetailOid, value);
            }
        }

        private bool _Sap;
        [XafDisplayName("Sap")]
        [Index(48), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
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

        private PickList _PickList;
        [Association("PickList-PickListDetailsActual")]
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