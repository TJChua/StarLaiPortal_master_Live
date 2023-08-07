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

namespace StarLaiPortal.Module.BusinessObjects.Item_Inquiry
{
    [DefaultClassOptions]
    [DefaultProperty("ItemCode")]
    [Appearance("HideNew", AppearanceItemType.Action, "True", TargetItems = "New", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideEdit", AppearanceItemType.Action, "True", TargetItems = "SwitchToEditMode; Edit", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("HideDelete", AppearanceItemType.Action, "True", TargetItems = "Delete", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide, Context = "Any")]
    [Appearance("LinkDoc", AppearanceItemType = "Action", TargetItems = "Link", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("UnlinkDoc", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "ListView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    
    [XafDisplayName("Item Inquiry Details")]
    public class ItemInquiryDetails : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ItemInquiryDetails(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string _ItemCode;
        [XafDisplayName("Item Code")]
        [Index(0), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemCode", Enabled = false)]
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                SetPropertyValue("ItemCode", ref _ItemCode, value);
            }
        }

        private string _ItemDesc;
        [XafDisplayName("Item Description")]
        [Index(3), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("ItemDesc", Enabled = false)]
        public string ItemDesc

        {
            get { return _ItemDesc; }
            set
            {
                SetPropertyValue("ItemDesc", ref _ItemDesc, value);
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

        private string _Model;
        [XafDisplayName("Model")]
        [Index(5), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Model", Enabled = false)]
        public string Model

        {
            get { return _Model; }
            set
            {
                SetPropertyValue("Model", ref _Model, value);
            }
        }

        private string _PriceList1;
        [XafDisplayName("Price 1")]
        [Index(6), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PriceList1", Enabled = false)]
        public string PriceList1

        {
            get { return _PriceList1; }
            set
            {
                SetPropertyValue("PriceList1", ref _PriceList1, value);
            }
        }

        private decimal _Price1;
        [XafDisplayName("Price 1")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(8), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Price1", Enabled = false)]
        public decimal Price1

        {
            get { return _Price1; }
            set
            {
                SetPropertyValue("Price1", ref _Price1, value);
            }
        }

        private string _PriceList2;
        [XafDisplayName("Price 2")]
        [Index(9), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PriceList2", Enabled = false)]
        public string PriceList2

        {
            get { return _PriceList2; }
            set
            {
                SetPropertyValue("PriceList2", ref _PriceList2, value);
            }
        }

        private decimal _Price2;
        [XafDisplayName("Price 2")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(12), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Price2", Enabled = false)]
        public decimal Price2
        {
            get { return _Price2; }
            set
            {
                SetPropertyValue("Price2", ref _Price2, value);
            }
        }

        private string _PriceList3;
        [XafDisplayName("Price 3")]
        [Index(13), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PriceList3", Enabled = false)]
        public string PriceList3

        {
            get { return _PriceList3; }
            set
            {
                SetPropertyValue("PriceList3", ref _PriceList3, value);
            }
        }

        private decimal _Price3;
        [XafDisplayName("Price 3")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(14), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Price3", Enabled = false)]
        public decimal Price3
        {
            get { return _Price3; }
            set
            {
                SetPropertyValue("Price3", ref _Price3, value);
            }
        }

        private string _PriceList4;
        [XafDisplayName("Price 4")]
        [Index(15), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("PriceList4", Enabled = false)]
        public string PriceList4

        {
            get { return _PriceList4; }
            set
            {
                SetPropertyValue("PriceList4", ref _PriceList4, value);
            }
        }

        private decimal _Price4;
        [XafDisplayName("Price 4")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(16), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Price4", Enabled = false)]
        public decimal Price4
        {
            get { return _Price4; }
            set
            {
                SetPropertyValue("Price4", ref _Price4, value);
            }
        }

        private string _StockName1;
        [XafDisplayName("StockName 1")]
        [Index(17), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("StockName1", Enabled = false)]
        public string StockName1

        {
            get { return _StockName1; }
            set
            {
                SetPropertyValue("StockName1", ref _StockName1, value);
            }
        }

        private decimal _Stock1;
        [XafDisplayName("Stock 1")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(18), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Stock1", Enabled = false)]
        public decimal Stock1

        {
            get { return _Stock1; }
            set
            {
                SetPropertyValue("Stock1", ref _Stock1, value);
            }
        }

        private string _StockName2;
        [XafDisplayName("StockName 2")]
        [Index(19), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("StockName2", Enabled = false)]
        public string StockName2

        {
            get { return _StockName2; }
            set
            {
                SetPropertyValue("StockName2", ref _StockName2, value);
            }
        }

        private decimal _Stock2;
        [XafDisplayName("Stock 2")]
        [DbType("numeric(18,6)")]
        [ModelDefault("DisplayFormat", "{0:n2}")]
        [Index(20), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("Stock2", Enabled = false)]
        public decimal Stock2

        {
            get { return _Stock2; }
            set
            {
                SetPropertyValue("Stock2", ref _Stock2, value);
            }
        }

        private string _OriginalCatalog;
        [XafDisplayName("Original Catalog No.")]
        [Index(21), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(true)]
        [Appearance("OriginalCatalog", Enabled = false)]
        public string OriginalCatalog

        {
            get { return _OriginalCatalog; }
            set
            {
                SetPropertyValue("OriginalCatalog", ref _OriginalCatalog, value);
            }
        }

        private string _HierarchyLevel1;
        [XafDisplayName("Hierarchy Level 1")]
        [Index(22), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("HierarchyLevel1", Enabled = false)]
        public string HierarchyLevel1

        {
            get { return _HierarchyLevel1; }
            set
            {
                SetPropertyValue("HierarchyLevel1", ref _HierarchyLevel1, value);
            }
        }

        private string _HierarchyLevel2;
        [XafDisplayName("Hierarchy Level 2")]
        [Index(23), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("HierarchyLevel2", Enabled = false)]
        public string HierarchyLevel2

        {
            get { return _HierarchyLevel2; }
            set
            {
                SetPropertyValue("HierarchyLevel2", ref _HierarchyLevel2, value);
            }
        }

        private string _HierarchyLevel3;
        [XafDisplayName("Hierarchy Level 3")]
        [Index(25), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("HierarchyLevel3", Enabled = false)]
        public string HierarchyLevel3

        {
            get { return _HierarchyLevel3; }
            set
            {
                SetPropertyValue("HierarchyLevel3", ref _HierarchyLevel3, value);
            }
        }

        private string _HierarchyLevel4;
        [XafDisplayName("Hierarchy Level 4")]
        [Index(28), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("HierarchyLevel4", Enabled = false)]
        public string HierarchyLevel4

        {
            get { return _HierarchyLevel4; }
            set
            {
                SetPropertyValue("HierarchyLevel4", ref _HierarchyLevel4, value);
            }
        }

        private string _HierarchyLevel5;
        [XafDisplayName("Hierarchy Level 5")]
        [Index(30), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("HierarchyLevel5", Enabled = false)]
        public string HierarchyLevel5

        {
            get { return _HierarchyLevel5; }
            set
            {
                SetPropertyValue("HierarchyLevel5", ref _HierarchyLevel5, value);
            }
        }

        private string _Remarks;
        [XafDisplayName("Remarks")]
        [Index(33), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Remarks", Enabled = false)]
        public string Remarks

        {
            get { return _Remarks; }
            set
            {
                SetPropertyValue("Remarks", ref _Remarks, value);
            }
        }

        private string _Barcode;
        [XafDisplayName("Barcode")]
        [Index(35), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Barcode", Enabled = false)]
        public string Barcode

        {
            get { return _Barcode; }
            set
            {
                SetPropertyValue("Barcode", ref _Barcode, value);
            }
        }

        private string _ForeignName;
        [XafDisplayName("Foreign Name")]
        [Index(38), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("ForeignName", Enabled = false)]
        public string ForeignName
        {
            get { return _ForeignName; }
            set
            {
                SetPropertyValue("ForeignName", ref _ForeignName, value);
            }
        }

        private string _BPCatalogNo;
        [XafDisplayName("BP Catalog No")]
        [Index(38), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("BPCatalogNo", Enabled = false)]
        public string BPCatalogNo
        {
            get { return _BPCatalogNo; }
            set
            {
                SetPropertyValue("BPCatalogNo", ref _BPCatalogNo, value);
            }
        }

        private string _PictureName;
        [XafDisplayName("Picture Name")]
        [Index(40), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("PictureName", Enabled = false)]
        public string PictureName
        {
            get { return _PictureName; }
            set
            {
                SetPropertyValue("PictureName", ref _PictureName, value);
            }
        }

        private ItemInquiry _ItemInquiry;
        [Association("ItemInquiry-ItemInquiryDetails")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Appearance("ItemInquiry", Enabled = false)]
        public ItemInquiry ItemInquiry
        {
            get { return _ItemInquiry; }
            set { SetPropertyValue("ItemInquiry", ref _ItemInquiry, value); }
        }
    }
}