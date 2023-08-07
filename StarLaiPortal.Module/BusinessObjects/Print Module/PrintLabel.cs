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

namespace StarLaiPortal.Module.BusinessObjects.Print_Module
{
    [DefaultClassOptions]
    [NavigationItem("Print Module")]
    [XafDisplayName("Print Label")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideLink", AppearanceItemType.Action, "True", TargetItems = "Link", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideUnlink", AppearanceItemType.Action, "True", TargetItems = "Unlink", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("hideSave", AppearanceItemType = "Action", TargetItems = "Save", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[Appearance("HideResetViewSetting", AppearanceItemType.Action, "True", TargetItems = "ResetViewSettings", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    //[Appearance("HideExport", AppearanceItemType.Action, "True", TargetItems = "Export", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideRefresh", AppearanceItemType.Action, "True", TargetItems = "Refresh", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    public class PrintLabel : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PrintLabel(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            LabelName = "LH";
            ItemQuantity = 1;
        }

        private LabelType _LabelType;
        [ImmediatePostData]
        [XafDisplayName("Label Type")]
        [Index(0), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public LabelType LabelType
        {
            get { return _LabelType; }
            set
            {
                SetPropertyValue("LabelType", ref _LabelType, value);
                if (!IsLoading)
                {
                    if (LabelType == LabelType.RH)
                    {
                        LabelName = "RH";
                    }
                    else if (LabelType == LabelType.LH)
                    {
                        LabelName = "LH";
                    }
                    else if (LabelType == LabelType.Others)
                    {
                        LabelName = "OTHER";
                    }
                }
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(3), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private ReportDocType _Doctype;
        [ImmediatePostData]
        [XafDisplayName("Doc Type")]
        [Index(5), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public ReportDocType Doctype
        {
            get { return _Doctype; }
            set
            {
                SetPropertyValue("Doctype", ref _Doctype, value);
            }
        }

        private string _DocNum;
        [XafDisplayName("Doc Num")]
        [Index(8), VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
        public string DocNum
        {
            get { return _DocNum; }
            set
            {
                SetPropertyValue("DocNum", ref _DocNum, value);
            }
        }

        private vwItemMasters _ItemCode;
        [ImmediatePostData]
        [NoForeignKey]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceCriteria("LabelType = '@this.LabelName'")]
        [XafDisplayName("Item Code")]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public vwItemMasters ItemCode
        {
            get { return _ItemCode; }
            set
            {
                SetPropertyValue("ItemCode", ref _ItemCode, value);
            }
        }

        private decimal _ItemQuantity;
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:N0}")]
        [ModelDefault("EditMask", "d")]
        [XafDisplayName("Item Quantity")]
        [Index(13), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        public decimal ItemQuantity
        {
            get { return _ItemQuantity; }
            set
            {
                SetPropertyValue("ItemQuantity", ref _ItemQuantity, value);
            }
        }

        private decimal _DocQuantity;
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [XafDisplayName("Doc Quantity")]
        [Index(15), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public decimal DocQuantity
        {
            get { return _DocQuantity; }
            set
            {
                SetPropertyValue("DocQuantity", ref _DocQuantity, value);
            }
        }

        private string _LabelName;
        [ImmediatePostData]
        [XafDisplayName("LabelName")]
        [Index(18), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string LabelName
        {
            get { return _LabelName; }
            set
            {
                SetPropertyValue("LabelName", ref _LabelName, value);
            }
        }

        private PrintLabelReprint _Reprint;
        [XafDisplayName("Reprint")]
        [Index(20), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public PrintLabelReprint Reprint
        {
            get { return _Reprint; }
            set
            {
                SetPropertyValue("Reprint", ref _Reprint, value);
            }
        }

        private string _BatchNumber;
        [Appearance("BatchNumber", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Criteria = "Doctype != 0")]
        [XafDisplayName("Batch Number")]
        [Index(23), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        public string BatchNumber
        {
            get { return _BatchNumber; }
            set
            {
                SetPropertyValue("BatchNumber", ref _BatchNumber, value);
            }
        }

        [Association("PrintLabel-PrintLabelDetails")]
        [XafDisplayName("Items")]
        public XPCollection<PrintLabelDetails> PrintLabelDetails
        {
            get { return GetCollection<PrintLabelDetails>("PrintLabelDetails"); }
        }
    }
}