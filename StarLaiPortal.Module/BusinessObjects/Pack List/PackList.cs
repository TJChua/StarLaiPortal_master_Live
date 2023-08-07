using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Pack_List
{
    [DefaultClassOptions]
    [XafDisplayName("Pack List")]
    [NavigationItem("Pack List")]
    [DefaultProperty("DocNum")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitPA", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelPA", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCopy", AppearanceItemType.Action, "True", TargetItems = "PACopyFromPL", Criteria = "PackingLocation = null", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class PackList : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PackList(Session session)
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
            ExpectedDeliveryDate = DateTime.Now;
            Status = DocStatus.Draft;
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

        private string _DocNum;
        [XafDisplayName("Pack List No")]
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

        private vwBin _PackingLocation;
        [ImmediatePostData]
        [XafDisplayName("Packing Location")]
        [NoForeignKey]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        //[DataSourceCriteria("Inactive = 'N'")]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwBin PackingLocation
        {
            get { return _PackingLocation; }
            set
            {
                SetPropertyValue("PackingLocation", ref _PackingLocation, value);
            }
        }

        private DateTime _DocDate;
        [XafDisplayName("Doc Date")]
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public DateTime DocDate

        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("DocDate", ref _DocDate, value);
            }
        }

        private DateTime _ExpectedDeliveryDate;
        [XafDisplayName("Expected Delivery Date")]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public DateTime ExpectedDeliveryDate

        {
            get { return _ExpectedDeliveryDate; }
            set
            {
                SetPropertyValue("ExpectedDeliveryDate", ref _ExpectedDeliveryDate, value);
            }
        }

        private string _CustomerGroup;
        [XafDisplayName("Customer Group")]
        [Appearance("CustomerGroup", Enabled = false)]
        [Index(10), VisibleInDetailView(false), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerGroup
        {
            get { return _CustomerGroup; }
            set
            {
                SetPropertyValue("CustomerGroup", ref _CustomerGroup, value);
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
                string duppl = null;
                string dupso = null;
                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    if (duppl != dtl.PickListNo)
                    {
                        PickList picklist;
                        picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl.PickListNo));

                        if (picklist != null)
                        {
                            foreach (PickListDetails dtl2 in picklist.PickListDetails)
                            {
                                if (dupso != dtl2.SOBaseDoc)
                                {
                                    if (rtn == null)
                                    {
                                        rtn = dtl2.SOBaseDoc;
                                    }
                                    else
                                    {
                                        rtn = rtn + ", " + dtl2.SOBaseDoc;
                                    }

                                    dupso = dtl2.SOBaseDoc;
                                }
                            }
                        }

                        duppl = dtl.PickListNo;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("SAP SO No.")]
        [Index(16), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("SAPSONo", Enabled = false)]
        public string SAPSONo
        {
            get
            {
                string rtn = null;
                string duppl = null;
                string dupso = null;
                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    if (duppl != dtl.PickListNo)
                    {
                        PickList picklist;
                        picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl.PickListNo));

                        if (picklist != null)
                        {
                            foreach (PickListDetails dtl2 in picklist.PickListDetails)
                            {
                                if (dupso != dtl2.SOBaseDoc)
                                {
                                    SalesOrder salesorder;
                                    salesorder = Session.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", dtl2.SOBaseDoc));

                                    if (salesorder != null)
                                    {
                                        if (rtn == null)
                                        {
                                            rtn = salesorder.SAPDocNum;
                                        }
                                        else
                                        {
                                            rtn = rtn + ", " + salesorder.SAPDocNum;
                                        }
                                    }

                                    dupso = dtl2.SOBaseDoc;
                                }
                            }
                        }

                        duppl = dtl.PickListNo;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("Pick List No.")]
        [Index(17), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PickListNo", Enabled = false)]
        public string PickListNo
        {
            get
            {
                string rtn = null;
                string duppl = null;
                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    if (duppl != dtl.PickListNo)
                    {
                        if (rtn == null)
                        {
                            rtn = dtl.PickListNo;
                        }
                        else
                        {
                            rtn = rtn + ", " + dtl.PickListNo;
                        }

                        duppl = dtl.PickListNo;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("Priority")]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Priority", Enabled = false)]
        public PriorityType Priority
        {
            get
            {
                PriorityType rtn = null;

                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    PickList picklist;
                    picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl.PickListNo));

                    if (picklist != null)
                    {
                        rtn = picklist.PickListDetails.Where(x => x.SOBaseDoc != null).OrderBy(c => c.Priority).Max().Priority;
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("Customer")]
        [Index(19), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Customer", Enabled = false)]
        public string Customer
        {
            get
            {
                string rtn = null;
                string duppl = null;
                string dupcustomer = null;
                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    if (duppl != dtl.PickListNo)
                    {
                        PickList picklist;
                        picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl.PickListNo));

                        if (picklist != null)
                        {
                            foreach (PickListDetails dtl2 in picklist.PickListDetails)
                            {
                                if (dupcustomer != dtl2.Customer.BPName)
                                {
                                    if (rtn == null)
                                    {
                                        rtn = dtl2.Customer.BPName;
                                    }
                                    else
                                    {
                                        rtn = rtn + ", " + dtl2.Customer.BPName;
                                    }

                                    dupcustomer = dtl2.Customer.BPName;
                                }
                            }
                        }

                        duppl = dtl.PickListNo;
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
                foreach (PackListDetails dtl in this.PackListDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("PackList-PackListDetails")]
        [XafDisplayName("Pack List")]
        public XPCollection<PackListDetails> PackListDetails
        {
            get { return GetCollection<PackListDetails>("PackListDetails"); }
        }

        [Association("PackList-PackListDocTrail")]
        [XafDisplayName("Document Trail")]
        public XPCollection<PackListDocTrail> PackListDocTrail
        {
            get { return GetCollection<PackListDocTrail>("PackListDocTrail"); }
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
                    PackListDocTrail ds = new PackListDocTrail(Session);
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
                    this.PackListDocTrail.Add(ds);
                }
            }
        }
    }
}