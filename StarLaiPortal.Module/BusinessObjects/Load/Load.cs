using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Load
{
    [DefaultClassOptions]
    [XafDisplayName("Loading")]
    [NavigationItem("Loading Bay")]
    [DefaultProperty("DocNum")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideSubmit", AppearanceItemType.Action, "True", TargetItems = "SubmitL", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideCancel", AppearanceItemType.Action, "True", TargetItems = "CancelL", Criteria = "not (Status in (0))", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
   
    public class Load : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Load(Session session)
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
            LoadingDate = DateTime.Now;
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
        [XafDisplayName("Loading No")]
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

        private DateTime _DocDate;
        [XafDisplayName("Doc Date")]
        [Index(3), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [RuleRequiredField(DefaultContexts.Save)]
        public DateTime DocDate

        {
            get { return _DocDate; }
            set
            {
                SetPropertyValue("DocDate", ref _DocDate, value);
            }
        }

        private DateTime _LoadingDate;
        [XafDisplayName("Loading Date")]
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public DateTime LoadingDate

        {
            get { return _LoadingDate; }
            set
            {
                SetPropertyValue("LoadingDate", ref _LoadingDate, value);
            }
        }

        private vwDriver _Driver;
        [NoForeignKey]
        [XafDisplayName("Driver")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwDriver Driver
        {
            get { return _Driver; }
            set
            {
                SetPropertyValue("Driver", ref _Driver, value);
            }
        }

        private vwVehicle _Vehicle;
        [NoForeignKey]
        [XafDisplayName("Vehicle")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public vwVehicle Vehicle
        {
            get { return _Vehicle; }
            set
            {
                SetPropertyValue("Vehicle", ref _Vehicle, value);
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(13), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        [NonPersistent]
        [XafDisplayName("Pack List No.")]
        [Index(14), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PackListNo", Enabled = false)]
        public string PackListNo
        {
            get
            {
                string rtn = null;
                string dupno = null;
                foreach (LoadDetails dtl in this.LoadDetails)
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
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("SONumber", Enabled = false)]
        public string SONumber
        {
            get
            {
                string rtn = null;
                string duppl = null;
                string dupso = null;
                foreach (LoadDetails dtl in this.LoadDetails)
                {
                    PackList packlist;
                    packlist = Session.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtl.PackList));

                    if (packlist != null)
                    {
                        rtn = packlist.SONumber;
                        //foreach (PackListDetails dtl2 in packlist.PackListDetails)
                        //{
                        //    if (duppl != dtl2.PickListNo)
                        //    {
                        //        PickList picklist;
                        //        picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl2.PickListNo));

                        //        if (picklist != null)
                        //        {
                        //            foreach (PickListDetails dtl3 in picklist.PickListDetails)
                        //            {
                        //                if (dupso != dtl3.SOBaseDoc)
                        //                {
                        //                    if (rtn == null)
                        //                    {
                        //                        rtn = dtl3.SOBaseDoc;
                        //                    }
                        //                    else
                        //                    {
                        //                        rtn = rtn + ", " + dtl3.SOBaseDoc;
                        //                    }

                        //                    dupso = dtl3.SOBaseDoc;
                        //                }
                        //            }
                        //        }

                        //        duppl = dtl2.PickListNo;
                        //    }
                        //}
                    }
                }

                return rtn;
            }
        }

        [NonPersistent]
        [XafDisplayName("Priority")]
        [Index(17), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Priority", Enabled = false)]
        public PriorityType Priority
        {
            get
            {
                PriorityType rtn = null;

                foreach (LoadDetails dtl in this.LoadDetails)
                {
                    PackList packlist;
                    packlist = Session.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtl.PackList));

                    if (packlist != null)
                    {
                        rtn = packlist.Priority;
                        //foreach (PackListDetails dtl2 in packlist.PackListDetails)
                        //{
                        //    PickList picklist;
                        //    picklist = Session.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl2.PickListNo));

                        //    if (picklist != null)
                        //    {
                        //        rtn = picklist.PickListDetails.Where(x => x.SOBaseDoc != null).OrderBy(c => c.Priority).Max().Priority;
                        //    }
                        //}
                    }
                }

                return rtn;
            }
        }

        private int _Total;
        [XafDisplayName("Total Bundle")]
        [Appearance("Total", Enabled = false)]
        [Index(25), VisibleInDetailView(true), VisibleInListView(false), VisibleInLookupListView(false)]
        public int Total
        {
            get
            {
                if (Session.IsObjectsSaving != true)
                {
                    int rtn = 0;
                    if (LoadDetails != null)
                        rtn = LoadDetails.GroupBy(x => x.Bundle).Count();

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

        private DocStatus _Status;
        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(50), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
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
                foreach (LoadDetails dtl in this.LoadDetails)
                {
                    return true;
                }

                return false;
            }
        }

        [Association("Load-LoadDetails")]
        [XafDisplayName("Load List")]
        public XPCollection<LoadDetails> LoadDetails
        {
            get { return GetCollection<LoadDetails>("LoadDetails"); }
        }

        [Association("Load-LoadDocTrail")]
        [XafDisplayName("Document Trail")]
        public XPCollection<LoadDocTrail> LoadDocTrail
        {
            get { return GetCollection<LoadDocTrail>("LoadDocTrail"); }
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
                    LoadDocTrail ds = new LoadDocTrail(Session);
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
                    this.LoadDocTrail.Add(ds);
                }
            }
        }
    }
}