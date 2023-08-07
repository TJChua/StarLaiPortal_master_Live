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
    [NavigationItem("Sales Quotation")]
    [XafDisplayName("Sales Quotation Inquiry")]
    [DefaultProperty("PortalNo")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwInquirySalesQuotation : XPLiteObject
    { // Inherit from a different class to  a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwInquirySalesQuotation(Session session)
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
        [XafDisplayName("PriKey")]
        [Appearance("PriKey", Enabled = false)]
        [Index(0), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("Portal Quotation No.")]
        [Appearance("PortalNo", Enabled = false)]
        [Index(3)]
        public string PortalNo
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(5)]
        public DateTime DocDate
        {
            get; set;
        }

        [XafDisplayName("Expected Delivery Date")]
        [Appearance("DueDate", Enabled = false)]
        [Index(8)]
        public DateTime DueDate
        {
            get; set;
        }

        [XafDisplayName("Create Date")]
        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy hh:mm tt}")]
        [Appearance("CreateDate", Enabled = false)]
        [Index(9)]
        public DateTime CreateDate
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Index(10)]
        public string Status
        {
            get; set;
        }

        [XafDisplayName("Exceed Credit Limit")]
        [Appearance("HitCreditLimit", Enabled = false)]
        [Index(13)]
        public string HitCreditLimit
        {
            get; set;
        }

        [XafDisplayName("Exceed Credit Term")]
        [Appearance("HitCreditTerm", Enabled = false)]
        [Index(15)]
        public string HitCreditTerm
        {
            get; set;
        }

        [XafDisplayName("Price Change")]
        [Appearance("HitPriceChange", Enabled = false)]
        [Index(18)]
        public string HitPriceChange
        {
            get; set;
        }

        [XafDisplayName("Customer Group")]
        [Appearance("CardGroup", Enabled = false)]
        [Index(20)]
        public string CardGroup
        {
            get; set;
        }

        [XafDisplayName("Customer Code")]
        [Appearance("CardCode", Enabled = false)]
        [Index(23)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("Customer Name")]
        [Appearance("CardName", Enabled = false)]
        [Index(25)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("Customer Contact")]
        [Appearance("ContactNo", Enabled = false)]
        [Index(28)]
        public string ContactNo
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [Appearance("Transporter", Enabled = false)]
        [Index(30)]
        public string Transporter
        {
            get; set;
        }

        [XafDisplayName("Salesperson")]
        [Appearance("Salesperson", Enabled = false)]
        [Index(33)]
        public string Salesperson
        {
            get; set;
        }

        [XafDisplayName("Priority")]
        [Appearance("Priority", Enabled = false)]
        [Index(35)]
        public string Priority
        {
            get; set;
        }

        [XafDisplayName("Series")]
        [Appearance("Series", Enabled = false)]
        [Index(36)]
        public string Series
        {
            get; set;
        }

        [XafDisplayName("Amount")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Appearance("Amount", Enabled = false)]
        [Index(38)]
        public decimal Amount
        {
            get; set;
        }

        [XafDisplayName("Remarks")]
        [Appearance("Remarks", Enabled = false)]
        [Index(40)]
        [Size(254)]
        public string Remarks
        {
            get; set;
        }

        [XafDisplayName("Portal SO No.")]
        [Appearance("PortalSONo", Enabled = false)]
        [Index(43)]
        public string PortalSONo
        {
            get; set;
        }

        [XafDisplayName("SAP SO No.")]
        [Appearance("SONo", Enabled = false)]
        [Index(45)]
        public string SONo
        {
            get; set;
        }

        [XafDisplayName("Pick List No.")]
        [Appearance("PickListNo", Enabled = false)]
        [Index(48)]
        public string PickListNo
        {
            get; set;
        }

        [XafDisplayName("Pack List No.")]
        [Appearance("PackListNo", Enabled = false)]
        [Index(50)]
        public string PackListNo
        {
            get; set;
        }

        [XafDisplayName("Loading No.")]
        [Appearance("LoadingNo", Enabled = false)]
        [Index(53)]
        public string LoadingNo
        {
            get; set;
        }

        [XafDisplayName("Portal DO No.")]
        [Appearance("PortalDONo", Enabled = false)]
        [Index(58)]
        public string PortalDONo
        {
            get; set;
        }

        [XafDisplayName("SAP DO No.")]
        [Appearance("SAPDONo", Enabled = false)]
        [Index(60)]
        public string SAPDONo
        {
            get; set;
        }

        [XafDisplayName("SAP Invoice No.")]
        [Appearance("SAPInvNo", Enabled = false)]
        [Index(63)]
        public string SAPInvNo
        {
            get; set;
        }

        [XafDisplayName("Price Change")]
        [Appearance("PriceChange", Enabled = false)]
        [Index(65)]
        public bool PriceChange
        {
            get; set;
        }

        [XafDisplayName("Exceed Limit")]
        [Appearance("ExceedPrice", Enabled = false)]
        [Index(68)]
        public bool ExceedPrice
        {
            get; set;
        }

        [XafDisplayName("Exceed Credit Control")]
        [Appearance("ExceedCreditControl", Enabled = false)]
        [Index(70)]
        public bool ExceedCreditControl
        {
            get; set;
        }
    }
}