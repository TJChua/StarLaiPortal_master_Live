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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.View
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class vwLoadingSO : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public vwLoadingSO(Session session)
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
        [Index(0), VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string PriKey
        {
            get; set;
        }

        [XafDisplayName("Loading Doc Num")]
        [Appearance("LoadingDocNum", Enabled = false)]
        [Index(3)]
        public string LoadingDocNum
        {
            get; set;
        }

        [XafDisplayName("Customer")]
        [Appearance("Customer", Enabled = false)]
        [Index(5)]
        public string Customer
        {
            get; set;
        }

        [XafDisplayName("CustomerName")]
        [Appearance("CustomerName", Enabled = false)]
        [Index(8)]
        public string CustomerName
        {
            get; set;
        }

        [XafDisplayName("SO Doc Num")]
        [Appearance("SODocNum", Enabled = false)]
        [Index(10)]
        public string SODocNum
        {
            get; set;
        }

        [XafDisplayName("Pick List Doc Num")]
        [NoForeignKey]
        [Appearance("PickListDocNum", Enabled = false)]
        [Index(11)]
        public PickList PickListDocNum
        {
            get; set;
        }

        //[XafDisplayName("ItemCode")]
        //[NoForeignKey]
        //[Appearance("ItemCode", Enabled = false)]
        //[Index(13)]
        //public vwItemMasters ItemCode
        //{
        //    get; set;
        //}

        //[XafDisplayName("PickQty")]
        //[DbType("numeric(19,6)")]
        //[ModelDefault("DisplayFormat", "{0:n4}")]
        //[ModelDefault("EditMask", "{0:n4}")]
        //[Appearance("PickQty", Enabled = false)]
        //[Index(15)]
        //public decimal PickQty
        //{
        //    get; set;
        //}
    }
}