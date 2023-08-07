using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Inquiry_View
{
    [DefaultClassOptions]
    [NavigationItem("Reports")]
    [XafDisplayName("Customer Aging")]
    [DefaultProperty("BPCode")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwInquiryCurrentARAging : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquiryCurrentARAging(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        [Key]
        [Browsable(true)]
        [XafDisplayName("BP Code")]
        [Appearance("BPCode", Enabled = false)]
        [Index(0), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string BPCode
        {
            get; set;
        }

        [XafDisplayName("Customer Name")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("Payment Term")]
        [Appearance("PaymentTerm", Enabled = false)]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string PaymentTerm
        {
            get; set;
        }

        [XafDisplayName("Payment Cr. Date")]
        [Appearance("PaymentCrDate", Enabled = false)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string PaymentCrDate
        {
            get; set;
        }

        [XafDisplayName("Payment Amt.")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("PaymentAmt", Enabled = false)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal PaymentAmt
        {
            get; set;
        }

        [XafDisplayName("Overdue")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Overdue", Enabled = false)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Overdue
        {
            get; set;
        }

        [XafDisplayName("Jan")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Jan", Enabled = false)]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Jan
        {
            get; set;
        }

        [XafDisplayName("Feb")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Feb", Enabled = false)]
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Feb
        {
            get; set;
        }

        [XafDisplayName("Mar")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Mar", Enabled = false)]
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Mar
        {
            get; set;
        }

        [XafDisplayName("Apr")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Apr", Enabled = false)]
        [Index(23), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Apr
        {
            get; set;
        }

        [XafDisplayName("May")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("May", Enabled = false)]
        [Index(25), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal May
        {
            get; set;
        }

        [XafDisplayName("Jun")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Jun", Enabled = false)]
        [Index(28), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Jun
        {
            get; set;
        }

        [XafDisplayName("Jul")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Jul", Enabled = false)]
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Jul
        {
            get; set;
        }

        [XafDisplayName("Aug")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Aug", Enabled = false)]
        [Index(33), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Aug
        {
            get; set;
        }

        [XafDisplayName("Sep")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Sep", Enabled = false)]
        [Index(35), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Sep
        {
            get; set;
        }

        [XafDisplayName("Oct")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Oct", Enabled = false)]
        [Index(38), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Oct
        {
            get; set;
        }

        [XafDisplayName("Nov")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Nov", Enabled = false)]
        [Index(40), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Nov
        {
            get; set;
        }

        [XafDisplayName("Dec")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Dec", Enabled = false)]
        [Index(43), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Dec
        {
            get; set;
        }

        [XafDisplayName("Aging Balance")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("AgingBalance", Enabled = false)]
        [Index(45), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal AgingBalance
        {
            get; set;
        }

        [XafDisplayName("Credit Limit")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("CreditLimit", Enabled = false)]
        [Index(48), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal CreditLimit
        {
            get; set;
        }

        [XafDisplayName("Deviation")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Deviation", Enabled = false)]
        [Index(50), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public decimal Deviation
        {
            get; set;
        }

        [XafDisplayName("Latest Payment No")]
        [Appearance("LatestPaymentNo", Enabled = false)]
        [Index(53), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string LatestPaymentNo
        {
            get; set;
        }

        [XafDisplayName("Payment Posting Date")]
        [Appearance("PaymentPostingDate", Enabled = false)]
        [Index(55), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string PaymentPostingDate
        {
            get; set;
        }

        [XafDisplayName("Customer Group")]
        [Appearance("CustomerGroup", Enabled = false)]
        [Index(58), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CustomerGroup
        {
            get; set;
        }
    }
}