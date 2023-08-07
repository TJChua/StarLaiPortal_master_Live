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

namespace StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice
{
    [DefaultClassOptions]
    [XafDisplayName("ASN")]
    [NavigationItem("Advanced Shipment Notice")]
    [DefaultProperty("DocNum")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitASN", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelASN", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideClose", AppearanceItemType.Action, "True", TargetItems = "CloseASN", Criteria = "not (Status in (1))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HideCopyTo", AppearanceItemType.Action, "True", TargetItems = "ASNCopyToGRN", Criteria = "not (Status in (1))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCopyTo1", AppearanceItemType.Action, "True", TargetItems = "ASNCopyToGRN", Criteria = "CopyTo = 1", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    [Appearance("HidePrint", AppearanceItemType.Action, "True", TargetItems = "PreviewASN", Criteria = "(Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    
    [Appearance("HideASNCopyFromPO", AppearanceItemType.Action, "True", TargetItems = "ASNCopyFromPO", Criteria = "RefNo = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideASNCopyFromPODetails", AppearanceItemType.Action, "True", TargetItems = "ASNCopyFromPODetails", Criteria = "RefNo = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class ASN : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ASN(Session session)
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
            RefDate = DateTime.Now;
            ETA = DateTime.Now;
            ESR = DateTime.Now;

            Status = DocStatus.Draft;
            DocType = DocTypeList.ASN;
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

        private vwBusniessPartner _Supplier;
        [XafDisplayName("Supplier")]
        [NoForeignKey]
        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("ValidFor = 'Y' and CardType = 'S'")]
        [Appearance("Supplier", Enabled = false, Criteria = "IsValid")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBusniessPartner Supplier
        {
            get { return _Supplier; }
            set
            {
                SetPropertyValue("Supplier", ref _Supplier, value);
                if (!IsLoading && value != null)
                {
                    SupplierName = Supplier.BPName;
                }
                else if (!IsLoading && value == null)
                {
                    SupplierName = null;
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

        private string _RefNo;
        [ImmediatePostData]
        [XafDisplayName("Ref No")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string RefNo
        {
            get { return _RefNo; }
            set
            {
                SetPropertyValue("RefNo", ref _RefNo, value);
            }
        }

        private vwVehicle _Vehicle;
        [NoForeignKey]
        [XafDisplayName("Vehicle")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwVehicle Vehicle
        {
            get { return _Vehicle; }
            set
            {
                SetPropertyValue("Vehicle", ref _Vehicle, value);
            }
        }

        private string _Container;
        [XafDisplayName("Container / External Vehicle No")]
        [Index(15), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Container
        {
            get { return _Container; }
            set
            {
                SetPropertyValue("Container", ref _Container, value);
            }
        }

        private vwWarehouse _Warehouse;
        [NoForeignKey]
        [XafDisplayName("Warehouse")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [Index(18), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public vwWarehouse Warehouse
        {
            get { return _Warehouse; }
            set
            {
                SetPropertyValue("Warehouse", ref _Warehouse, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Date")]
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("_DocDate", ref _DocDate, value);
            }
        }

        private DateTime _RefDate;
        [XafDisplayName("Ref Date")]
        [Index(23), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime RefDate
        {
            get { return _RefDate; }
            set
            {
                SetPropertyValue("RefDate", ref _RefDate, value);
            }
        }

        private DateTime _ETA;
        [XafDisplayName("ETA")]
        [Index(25), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime ETA
        {
            get { return _ETA; }
            set
            {
                SetPropertyValue("ETA", ref _ETA, value);
            }
        }

        private DateTime _ESR;
        [XafDisplayName("ESR")]
        [Index(28), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime ESR
        {
            get { return _ESR; }
            set
            {
                SetPropertyValue("ESR", ref _ESR, value);
            }
        }

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DocStatus Status
        {
            get { return _Status; }
            set
            {
                SetPropertyValue("Status", ref _Status, value);
            }
        }

        private PrintStatus _PrintStatus;
        [XafDisplayName("Print Status")]
        [Appearance("PrintStatus", Enabled = false)]
        [Index(31), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public PrintStatus PrintStatus
        {
            get { return _PrintStatus; }
            set
            {
                SetPropertyValue("PrintStatus", ref _PrintStatus, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("PO No.")]
        [Index(32), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PONo", Enabled = false)]
        public string PONo
        {
            get
            {
                string rtn = null;
                string dupso = null;
                foreach (ASNDetails dtl in this.ASNDetails)
                {
                    if (dupso != dtl.PORefNo)
                    {
                        if (rtn == null)
                        {
                            rtn = dtl.PORefNo;
                        }
                        else
                        {
                            rtn = rtn + ", " + dtl.PORefNo;
                        }

                        dupso = dtl.PORefNo;
                    }
                }

                return rtn;
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(50), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private string _BatchNumber;
        [XafDisplayName("Batch Number")]
        [Index(51), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string BatchNumber
        {
            get { return _BatchNumber; }
            set
            {
                SetPropertyValue("BatchNumber", ref _BatchNumber, value);
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
                foreach(ASNDetails dtl in this.ASNDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("ASN-ASNDetails")]
        [XafDisplayName("Content")]
        public XPCollection<ASNDetails> ASNDetails
        {
            get { return GetCollection<ASNDetails>("ASNDetails"); }
        }

        [Association("ASN-ASNDocTrail")]
        [XafDisplayName("Status History")]
        public XPCollection<ASNDocTrail> ASNDocTrail
        {
            get { return GetCollection<ASNDocTrail>("ASNDocTrail"); }
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
                    ASNDocTrail ds = new ASNDocTrail(Session);
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
                    this.ASNDocTrail.Add(ds);
                }
            }
        }
    }
}