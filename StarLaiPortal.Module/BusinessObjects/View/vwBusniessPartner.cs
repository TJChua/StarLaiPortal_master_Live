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

// 2023-07-28 add createdate ver 1.0.7

namespace StarLaiPortal.Module.BusinessObjects.View
{
    [DefaultClassOptions]
    [NavigationItem("SAP")]
    [XafDisplayName("Business Partner")]
    [DefaultProperty("BoFullName")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class vwBusniessPartner : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwBusniessPartner(Session session)
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
        [Index(0)]
        public string BPCode
        {
            get; set;
        }

        [XafDisplayName("BP Name")]
        [Appearance("BPName", Enabled = false)]
        [Index(3)]
        public string BPName
        {
            get; set;
        }

        [XafDisplayName("CardType")]
        [Appearance("CardType", Enabled = false)]
        [Index(5)]
        public string CardType
        {
            get; set;
        }

        [XafDisplayName("Balance")]
        [Appearance("Balance", Enabled = false)]
        [Index(6)]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public decimal Balance
        {
            get; set;
        }

        [XafDisplayName("Credit Limit")]
        [Appearance("CreditLimit", Enabled = false)]
        [Index(7)]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        public decimal CreditLimit
        {
            get; set;
        }

        [XafDisplayName("ValidFor")]
        [Appearance("ValidFor", Enabled = false)]
        [Index(8)]
        public string ValidFor
        {
            get; set;
        }

        [XafDisplayName("BillToDef")]
        [Appearance("BillToDef", Enabled = false)]
        [Index(10)]
        public string BillToDef
        {
            get; set;
        }

        [XafDisplayName("ShipToDef")]
        [Appearance("ShipToDef", Enabled = false)]
        [Index(12)]
        public string ShipToDef
        {
            get; set;

        }

        [XafDisplayName("Contact")]
        [Appearance("Contact", Enabled = false)]
        [Index(13)]
        public string Contact
        {
            get; set;

        }

        [XafDisplayName("PaymentTerm")]
        [NoForeignKey]
        [Appearance("PaymentTerm", Enabled = false)]
        [Index(15)]
        public vwPaymentTerm PaymentTerm
        {
            get; set;
        }

        [XafDisplayName("ListNum")]
        [Appearance("ListNum", Enabled = false)]
        [Index(18)]
        public int ListNum
        {
            get; set;
        }

        [XafDisplayName("Currency")]
        [Appearance("Currency", Enabled = false)]
        [Index(20)]
        public string Currency
        {
            get; set;
        }

        [XafDisplayName("GroupCode")]
        [Appearance("GroupCode", Enabled = false)]
        [Index(23)]
        public int GroupCode
        {
            get; set;
        }

        [XafDisplayName("SlpCode")]
        [Appearance("SlpCode", Enabled = false)]
        [Index(25)]
        [NoForeignKey]
        public vwSalesPerson SlpCode
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [Appearance("Transporter", Enabled = false)]
        [Index(28)]
        [NoForeignKey]
        public vwTransporter Transporter
        {
            get; set;
        }

        [XafDisplayName("SalesOrderSeries")]
        [Appearance("SalesOrderSeries", Enabled = false)]
        [Index(30)]
        public string SalesOrderSeries
        {
            get; set;
        }

        [XafDisplayName("GroupName")]
        [Appearance("GroupName", Enabled = false)]
        [Index(33)]
        public string GroupName
        {
            get; set;
        }

        [XafDisplayName("LeadTime")]
        [Appearance("LeadTime", Enabled = false)]
        [Index(35)]
        public int LeadTime
        {
            get; set;
        }

        // Start ver 1.0.7
        [XafDisplayName("Create Date")]
        [Appearance("CreateDate", Enabled = false)]
        [Index(38)]
        public string CreateDate
        {
            get; set;
        }
        // End ver 1.0.7

        [Index(88), VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(true)]
        public string BoFullName
        {
            get { return BPCode + "-" + BPName; }
        }
    }
}