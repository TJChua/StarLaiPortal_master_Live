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

namespace StarLaiPortal.Module.BusinessObjects.Dashboard
{
    [DefaultClassOptions]
    [NavigationItem("Dashboards")]
    [XafDisplayName("Dashboards")]
    //[ImageName("BO_Contact")]
    [DefaultProperty("BoFullName")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]

    public class Dashboards : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Dashboards(Session session)
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

        [XafDisplayName("Dashboard Type")]
        [Appearance("DashboardType", Enabled = false)]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string DashboardType
        {
            get; set;
        }

        [XafDisplayName("Transaction Type")]
        [Appearance("TransactionType", Enabled = false)]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DocTypeList TransactionType
        {
            get; set;
        }

        [XafDisplayName("Pending Transaction")]
        [Appearance("PendingTransaction", Enabled = false)]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public int PendingTransaction
        {
            get; set;
        }

        [XafDisplayName("Doc Num")]
        [Appearance("DocNum", Enabled = false)]
        [Index(10), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string DocNum
        {
            get; set;
        }

        [XafDisplayName("CreateDate")]
        [Appearance("Create Date", Enabled = false)]
        [Index(13), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CreateDate
        {
            get; set;
        }

        [XafDisplayName("Warehouse")]
        [Appearance("Warehouse", Enabled = false)]
        [Index(15), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Warehouse
        {
            get; set;
        }

        [XafDisplayName("BP Code")]
        [Appearance("CardCode", Enabled = false)]
        [Index(18), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CardCode
        {
            get; set;
        }

        [XafDisplayName("BP Name")]
        [Appearance("CardName", Enabled = false)]
        [Index(20), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string CardName
        {
            get; set;
        }

        [XafDisplayName("Posting Date")]
        [Appearance("DocDate", Enabled = false)]
        [Index(23), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DocDate
        {
            get; set;
        }

        [XafDisplayName("Delivery Date")]
        [Appearance("DueDate", Enabled = false)]
        [Index(25), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public DateTime DueDate
        {
            get; set;
        }

        [XafDisplayName("Remark")]
        [Appearance("Remark", Enabled = false)]
        [Index(28), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Remark
        {
            get; set;
        }

        [XafDisplayName("Transporter")]
        [Appearance("Transporter", Enabled = false)]
        [Index(30), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Transporter
        {
            get; set;
        }

        [XafDisplayName("Series")]
        [Appearance("Series", Enabled = false)]
        [Index(33), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Series
        {
            get; set;
        }

        [XafDisplayName("Status")]
        [Appearance("Status", Enabled = false)]
        [Size(200)]
        [Index(35), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Status
        {
            get; set;
        }

        [XafDisplayName("Sales Order")]
        [Appearance("SalesOrder", Enabled = false)]
        [Index(38), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string SalesOrder
        {
            get; set;
        }

        [XafDisplayName("Salesperson")]
        [Appearance("Salesperson", Enabled = false)]
        [Index(40), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Salesperson
        {
            get; set;
        }

        [XafDisplayName("Department")]
        [Appearance("Department", Enabled = false)]
        [Index(43), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(false)]
        public string Department
        {
            get; set;
        }
    }
}