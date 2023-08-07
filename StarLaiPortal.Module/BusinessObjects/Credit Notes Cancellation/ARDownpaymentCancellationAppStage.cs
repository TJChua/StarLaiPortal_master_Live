using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation
{
    [DefaultClassOptions]
    [Appearance("ARDCTrxAppStages1", AppearanceItemType = "Action", TargetItems = "New", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ARDCTrxAppStages2", AppearanceItemType = "Action", TargetItems = "Edit", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ARDCTrxAppStages3", AppearanceItemType = "Action", TargetItems = "Link", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ARDCTrxAppStages4", AppearanceItemType = "Action", TargetItems = "Delete", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ARDCTrxAppStages5", AppearanceItemType = "Action", TargetItems = "Unlink", Context = "Any", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [DefaultProperty("StatusInfo")]
    [XafDisplayName("Approval Stage")]
    public class ARDownpaymentCancellationAppStage : XPObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public ARDownpaymentCancellationAppStage(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private Approvals _Approval;
        [XafDisplayName("Approval Stage")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        [Index(10), VisibleInListView(true), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("Approval", Enabled = false)]
        public Approvals Approval
        {
            get { return _Approval; }
            set
            {
                SetPropertyValue("Approval", ref _Approval, value);
            }
        }

        private ARDownpaymentCancel _ARDownpaymentCancel;
        [Association("ARDownpaymentCancel-ARDownpaymentCancellationAppStage")]
        [Index(99), VisibleInListView(false), VisibleInDetailView(true), VisibleInLookupListView(false)]
        [Appearance("ARDownpaymentCancel", Enabled = false)]
        public ARDownpaymentCancel ARDownpaymentCancel
        {
            get { return _ARDownpaymentCancel; }
            set { SetPropertyValue("ARDownpaymentCancel", ref _ARDownpaymentCancel, value); }
        }
    }
}