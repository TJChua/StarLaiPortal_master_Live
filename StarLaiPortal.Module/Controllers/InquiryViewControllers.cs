using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects.Inquiry_View;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InquiryViewControllers : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public InquiryViewControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.ViewOpenPickList.Active.SetItemValue("Enabled", false);
            this.ViewPickListDetailInquiry.Active.SetItemValue("Enabled", false);
            this.ViewPickListInquiry.Active.SetItemValue("Enabled", false);

            if (typeof(vwInquiryOpenPickList).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(vwInquiryOpenPickList))
                {
                    this.ViewOpenPickList.Active.SetItemValue("Enabled", true);
                    this.ViewOpenPickList.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                }
            }

            if (typeof(vwInquiryPickListDetails).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(vwInquiryPickListDetails))
                {
                    this.ViewPickListDetailInquiry.Active.SetItemValue("Enabled", true);
                    this.ViewPickListDetailInquiry.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                }
            }

            if (typeof(vwInquiryPickList).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(vwInquiryPickList))
                {
                    this.ViewPickListInquiry.Active.SetItemValue("Enabled", true);
                    this.ViewPickListInquiry.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        public void openNewView(IObjectSpace os, object target, ViewEditMode viewmode)
        {
            ShowViewParameters svp = new ShowViewParameters();
            DetailView dv = Application.CreateDetailView(os, target);
            dv.ViewEditMode = viewmode;
            dv.IsRoot = true;
            svp.CreatedView = dv;

            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

        }

        public void showMsg(string caption, string msg, InformationType msgtype)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 3000;
            //options.Message = string.Format("{0} task(s) have been successfully updated!", e.SelectedObjects.Count);
            options.Message = string.Format("{0}", msg);
            options.Type = msgtype;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = caption;
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void ViewOpenPickList_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            vwInquiryOpenPickList selectedObject = (vwInquiryOpenPickList)e.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));
            openNewView(os, trx, ViewEditMode.View);
        }

        private void ViewOpenPickList_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            vwInquiryOpenPickList selectedObject = (vwInquiryOpenPickList)View.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));

            DetailView detailView = Application.CreateDetailView(os, "PickList_DetailView_Dashboard", true, trx);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
            e.View = detailView;
            e.DialogController.AcceptAction.Caption = "Go To Document";
            e.Maximized = true;
            //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
        }

        private void ViewPickListDetailInquiry_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            vwInquiryPickListDetails selectedObject = (vwInquiryPickListDetails)e.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));
            openNewView(os, trx, ViewEditMode.View);
        }

        private void ViewPickListDetailInquiry_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            vwInquiryPickListDetails selectedObject = (vwInquiryPickListDetails)View.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));

            DetailView detailView = Application.CreateDetailView(os, "PickList_DetailView_Dashboard", true, trx);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
            e.View = detailView;
            e.DialogController.AcceptAction.Caption = "Go To Document";
            e.Maximized = true;
            //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
        }

        private void ViewPickListInquiry_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            vwInquiryPickList selectedObject = (vwInquiryPickList)e.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));
            openNewView(os, trx, ViewEditMode.View);
        }

        private void ViewPickListInquiry_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            vwInquiryPickList selectedObject = (vwInquiryPickList)View.CurrentObject;

            IObjectSpace os = Application.CreateObjectSpace();
            PickList trx = os.FindObject<PickList>(new BinaryOperator("DocNum", selectedObject.PortalNo));

            DetailView detailView = Application.CreateDetailView(os, "PickList_DetailView_Dashboard", true, trx);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
            e.View = detailView;
            e.DialogController.AcceptAction.Caption = "Go To Document";
            e.Maximized = true;
            //e.DialogController.CancelAction.Active["NothingToCancel"] = false;
        }
    }
}
