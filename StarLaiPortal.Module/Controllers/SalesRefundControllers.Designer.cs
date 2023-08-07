namespace StarLaiPortal.Module.Controllers
{
    partial class SalesRefundControllers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SubmitSRefund = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSRefund = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewSRefund = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitSRefund
            // 
            this.SubmitSRefund.AcceptButtonCaption = null;
            this.SubmitSRefund.CancelButtonCaption = null;
            this.SubmitSRefund.Caption = "Submit";
            this.SubmitSRefund.Category = "ObjectsCreation";
            this.SubmitSRefund.ConfirmationMessage = null;
            this.SubmitSRefund.Id = "SubmitSRefund";
            this.SubmitSRefund.ToolTip = null;
            this.SubmitSRefund.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSRefund_CustomizePopupWindowParams);
            this.SubmitSRefund.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSRefund_Execute);
            // 
            // CancelSRefund
            // 
            this.CancelSRefund.AcceptButtonCaption = null;
            this.CancelSRefund.CancelButtonCaption = null;
            this.CancelSRefund.Caption = "Cancel";
            this.CancelSRefund.Category = "ObjectsCreation";
            this.CancelSRefund.ConfirmationMessage = null;
            this.CancelSRefund.Id = "CancelSRefund";
            this.CancelSRefund.ToolTip = null;
            this.CancelSRefund.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSRefund_CustomizePopupWindowParams);
            this.CancelSRefund.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSRefund_Execute);
            // 
            // PreviewSRefund
            // 
            this.PreviewSRefund.Caption = "Preview";
            this.PreviewSRefund.Category = "ObjectsCreation";
            this.PreviewSRefund.ConfirmationMessage = null;
            this.PreviewSRefund.Id = "PreviewSRefund";
            this.PreviewSRefund.ToolTip = null;
            this.PreviewSRefund.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSRefund_Execute);
            // 
            // SalesRefundControllers
            // 
            this.Actions.Add(this.SubmitSRefund);
            this.Actions.Add(this.CancelSRefund);
            this.Actions.Add(this.PreviewSRefund);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSRefund;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSRefund;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSRefund;
    }
}
