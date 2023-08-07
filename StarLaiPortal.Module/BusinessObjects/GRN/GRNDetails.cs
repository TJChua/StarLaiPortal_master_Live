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

namespace StarLaiPortal.Module.BusinessObjects.GRN
{
    [DefaultClassOptions]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("LinkDoc", AppearanceItemType = "Action", TargetItems = "Link", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("UnlinkDoc", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [XafDisplayName("GRN Details")]

    public class GRNDetails : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public GRNDetails(Session session)
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

        private string _PONo;
        [XafDisplayName("PO No.")]
        [Appearance("PONo", Enabled = false)]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string PONo
        {
            get { return _PONo; }
            set
            {
                SetPropertyValue("PONo", ref _PONo, value);
            }
        }

        private vwItemMasters _ItemCode;
        [ImmediatePostData]
        [NoForeignKey]
        [XafDisplayName("Item Code")]
        [Index(3), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
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
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
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
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string CatalogNo
        {
            get { return _CatalogNo; }
            set
            {
                SetPropertyValue("CatalogNo", ref _CatalogNo, value);
            }
        }

        private vwWarehouse _Location;
        [NoForeignKey]
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Loc")]
        [Appearance("Location", Enabled = false)]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwWarehouse Location
        {
            get { return _Location; }
            set
            {
                SetPropertyValue("Location", ref _Location, value);
            }
        }

        private vwBin _DefBin;
        [NoForeignKey]
        [XafDisplayName("Def Bin")]
        [Appearance("DefBin", Enabled = false)]
        [Index(15), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwBin DefBin
        {
            get { return _DefBin; }
            set
            {
                SetPropertyValue("DefBin", ref _DefBin, value);
            }
        }

        private decimal _Received;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Received")]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Received
        {
            get { return _Received; }
            set
            {
                SetPropertyValue("Received", ref _Received, value);
                if (!IsLoading)
                {
                    if (Received > OpenQty)
                    {
                        Received = OpenQty;
                    }

                    if (Received < 0)
                    {
                        Received = 0;
                    }

                    Variance = OpenQty - Received;
                }
            }
        }

        private decimal _OpenQty;
        [ImmediatePostData]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Open Qty")]
        [Appearance("OpenQty", Enabled = false)]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal OpenQty
        {
            get { return _OpenQty; }
            set
            {
                SetPropertyValue("OpenQty", ref _OpenQty, value);
            }
        }

        private decimal _Variance;
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Variance")]
        [Appearance("Variance", Enabled = false)]
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal Variance
        {
            get { return _Variance; }
            set
            {
                SetPropertyValue("Variance", ref _Variance, value);
            }
        }

        private string _DiscrepancyReason;
        [XafDisplayName("Discrepancy Reason")]
        [Size(200)]
        [Index(22), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string DiscrepancyReason
        {
            get { return _DiscrepancyReason; }
            set
            {
                SetPropertyValue("PORefNo", ref _DiscrepancyReason, value);
            }
        }

        private string _PORefNo;
        [XafDisplayName("PO Ref. No.")]
        [Appearance("PORefNo", Enabled = false)]
        [Index(23), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public string PORefNo
        {
            get { return _PORefNo; }
            set
            {
                SetPropertyValue("PORefNo", ref _PORefNo, value);
            }
        }

        private string _BaseDoc;
        [XafDisplayName("BaseDoc")]
        [Index(25), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string BaseDoc
        {
            get { return _BaseDoc; }
            set
            {
                SetPropertyValue("BaseDoc", ref _BaseDoc, value);
            }
        }

        private string _BaseId;
        [XafDisplayName("BaseId")]
        [Index(28), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string BaseId
        {
            get { return _BaseId; }
            set
            {
                SetPropertyValue("BaseId", ref _BaseId, value);
            }
        }

        private string _ASNBaseDoc;
        [XafDisplayName("ASNBaseDoc")]
        [Index(30), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ASNBaseDoc
        {
            get { return _ASNBaseDoc; }
            set
            {
                SetPropertyValue("ASNBaseDoc", ref _ASNBaseDoc, value);
            }
        }

        private string _ASNBaseId;
        [XafDisplayName("ASNBaseId")]
        [Index(33), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ASNBaseId
        {
            get { return _ASNBaseId; }
            set
            {
                SetPropertyValue("ASNBaseId", ref _ASNBaseId, value);
            }
        }

        private string _ASNPOBaseDoc;
        [XafDisplayName("ASNPOBaseDoc")]
        [Index(35), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ASNPOBaseDoc
        {
            get { return _ASNPOBaseDoc; }
            set
            {
                SetPropertyValue("ASNPOBaseDoc", ref _ASNPOBaseDoc, value);
            }
        }

        private string _ASNPOBaseId;
        [XafDisplayName("ASNBaseId")]
        [Index(38), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ASNPOBaseId
        {
            get { return _ASNPOBaseId; }
            set
            {
                SetPropertyValue("ASNPOBaseId", ref _ASNPOBaseId, value);
            }
        }

        private string _BaseType;
        [XafDisplayName("BaseType")]
        [Index(40), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string BaseType
        {
            get { return _BaseType; }
            set
            {
                SetPropertyValue("BaseType", ref _BaseType, value);
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
                if (this.Received <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        private GRN _GRN;
        [Association("GRN-GRNDetails")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("GRN", Enabled = false)]
        public GRN GRN
        {
            get { return _GRN; }
            set { SetPropertyValue("GRN", ref _GRN, value); }
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