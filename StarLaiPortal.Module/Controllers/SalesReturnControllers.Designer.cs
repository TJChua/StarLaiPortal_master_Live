namespace StarLaiPortal.Module.Controllers
{
    partial class SalesReturnControllers
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
            this.SubmitSR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewSR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitSR
            // 
            this.SubmitSR.AcceptButtonCaption = null;
            this.SubmitSR.CancelButtonCaption = null;
            this.SubmitSR.Caption = "Submit";
            this.SubmitSR.Category = "ObjectsCreation";
            this.SubmitSR.ConfirmationMessage = null;
            this.SubmitSR.Id = "SubmitSR";
            this.SubmitSR.ToolTip = null;
            this.SubmitSR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSR_CustomizePopupWindowParams);
            this.SubmitSR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSR_Execute);
            // 
            // CancelSR
            // 
            this.CancelSR.AcceptButtonCaption = null;
            this.CancelSR.CancelButtonCaption = null;
            this.CancelSR.Caption = "Cancel";
            this.CancelSR.Category = "ObjectsCreation";
            this.CancelSR.ConfirmationMessage = null;
            this.CancelSR.Id = "CancelSR";
            this.CancelSR.ToolTip = null;
            this.CancelSR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSR_CustomizePopupWindowParams);
            this.CancelSR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSR_Execute);
            // 
            // PreviewSR
            // 
            this.PreviewSR.Caption = "Preview";
            this.PreviewSR.Category = "ObjectsCreation";
            this.PreviewSR.ConfirmationMessage = null;
            this.PreviewSR.Id = "PreviewSR";
            this.PreviewSR.ToolTip = null;
            this.PreviewSR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSR_Execute);
            // 
            // SalesReturnControllers
            // 
            this.Actions.Add(this.SubmitSR);
            this.Actions.Add(this.CancelSR);
            this.Actions.Add(this.PreviewSR);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSR;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSR;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSR;
    }
}
