using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SalesRefundControllers : ViewController
    {
        GeneralControllers genCon;
        public SalesRefundControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.SubmitSRefund.Active.SetItemValue("Enabled", false);
            this.CancelSRefund.Active.SetItemValue("Enabled", false);
            this.PreviewSRefund.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "SalesRefunds_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitSRefund.Active.SetItemValue("Enabled", true);
                    this.CancelSRefund.Active.SetItemValue("Enabled", true);
                    this.PreviewSRefund.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitSRefund.Active.SetItemValue("Enabled", false);
                    this.CancelSRefund.Active.SetItemValue("Enabled", false);
                    this.PreviewSRefund.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.SubmitSRefund.Active.SetItemValue("Enabled", false);
                this.CancelSRefund.Active.SetItemValue("Enabled", false);
                this.PreviewSRefund.Active.SetItemValue("Enabled", false);
            }
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

        private void SubmitSRefund_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesRefunds selectedObject = (SalesRefunds)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                SalesRefundDocTrail ds = ObjectSpace.CreateObject<SalesRefundDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.SalesRefundDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                //foreach (SalesRefundDetails dtl in selectedObject.SalesRefundDetails)
                //{
                //    if (dtl.BaseDoc != null)
                //    {
                //        genCon.CloseSalesReturnReq(dtl.BaseDoc, "Close", ObjectSpace);
                //        break;
                //    }
                //}

                IObjectSpace os = Application.CreateObjectSpace();
                SalesRefunds trx = os.FindObject<SalesRefunds>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitSRefund_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelSRefund_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SalesRefunds selectedObject = (SalesRefunds)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            SalesRefundDocTrail ds = ObjectSpace.CreateObject<SalesRefundDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.SalesRefundDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            //foreach (SalesRefundDetails dtl in selectedObject.SalesRefundDetails)
            //{
            //    if (dtl.BaseDoc != null)
            //    {
            //        genCon.CloseSalesReturnReq(dtl.BaseDoc, "Cancel", ObjectSpace);
            //        break;
            //    }
            //}

            IObjectSpace os = Application.CreateObjectSpace();
            SalesRefunds trx = os.FindObject<SalesRefunds>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelSRefund_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PreviewSRefund_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }
    }
}
