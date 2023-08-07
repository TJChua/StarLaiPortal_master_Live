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

namespace StarLaiPortal.Module.BusinessObjects
{
    [DomainComponent]
    [NonPersistent]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [XafDisplayName("Sales History")]
    public class SalesHistory
    {
        [Browsable(false), DevExpress.ExpressApp.Data.Key]
        public int Id;

        [XafDisplayName("No")]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("No", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("No1", Enabled = false)]
        public int No { get; set; }

        [XafDisplayName("Customer")]
        [Index(1), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Customer", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("Customer1", Enabled = false)]
        public string Customer { get; set; }

        [XafDisplayName("Sales Date")]
        [Index(2), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("SalesDate", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("SalesDate1", Enabled = false)]
        public DateTime SalesDate { get; set; }

        [XafDisplayName("Quantity")]
        [Index(3), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Quantity", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("Quantity1", Enabled = false)]
        [ModelDefault("DisplayFormat", "n2")]
        public decimal Quantity { get; set; }

        [XafDisplayName("Unit Price")]
        [Index(4), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("UnitPrice", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("UnitPrice1", Enabled = false)]
        [ModelDefault("DisplayFormat", "n2")]
        public decimal UnitPrice { get; set; }

        [XafDisplayName("SAP Invoice No")]
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("SAPInvoiceNo", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("SAPInvoiceNo1", Enabled = false)]
        public string SAPInvoiceNo { get; set; }

        [Browsable(false)]
        public bool IsErr { get; set; }
    }

    [DomainComponent]
    [NonPersistent]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class SalesHistoryList
    {
        [XafDisplayName("Item Code")]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemCode", Enabled = false)]
        public string ItemCode { get; set; }

        private BindingList<SalesHistory> saleshistory;
        public SalesHistoryList()
        {
            saleshistory = new BindingList<SalesHistory>();
        }
        public BindingList<SalesHistory> Sales { get { return saleshistory; } }
    }

    [DomainComponent]
    [NonPersistent]
    public class Confirmation
    { 
        [XafDisplayName("Message")]
        [Appearance("Message", Enabled = false)]
        public string Message { get; set; }
    }

    [DomainComponent]
    [NonPersistent]
    [XafDisplayName("Confirmation")]
    public class StringParameters
    {
        // Add this property as the key member in the CustomizeTypesInfo event
        [XafDisplayName("Remarks")]
        [Appearance("ParamString", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [NonPersistentDc]
        public string ParamString { get; set; }

        //[XafDisplayName("Important")]
        [Appearance("ActionMessage", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("ActionMessage2", Enabled = false, FontColor = "Red")]
        [NonPersistentDc]
        public string ActionMessage { get; set; }

        [Browsable(false)]
        //[NonPersistentDc]
        public bool IsErr { get; set; }
    }

    [DomainComponent]
    [NonPersistent]
    [XafDisplayName("Approval")]
    [RuleCriteria("ApprovalRemarks", DefaultContexts.Save, "IsValid = 0", "Please fill in reason.")]
    public class ApprovalParameters : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ApprovalParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        // Add this property as the key member in the CustomizeTypesInfo event
        [XafDisplayName("Approval Status")]
        [Appearance("Approved", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        public ApprovalActions AppStatus { get; set; }

        [XafDisplayName("Remarks")]
        [Appearance("ParamString", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        public string ParamString { get; set; }

        //[XafDisplayName("Important")]
        [Appearance("ActionMessage", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "IsErr")]
        [Appearance("ActionMessage2", Enabled = false, FontColor = "Red")]
        public string ActionMessage { get; set; }

        [Browsable(false)]
        public bool IsErr { get; set; }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                if ((ParamString == null || ParamString == "") && AppStatus == ApprovalActions.No)
                {
                    return true;
                }

                return false;
            }
        }
    }
}