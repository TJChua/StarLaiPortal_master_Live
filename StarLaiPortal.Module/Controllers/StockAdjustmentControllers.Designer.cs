namespace StarLaiPortal.Module.Controllers
{
    partial class StockAdjustmentControllers
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
            this.SubmitSA = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSA = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewSA = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitSA
            // 
            this.SubmitSA.AcceptButtonCaption = null;
            this.SubmitSA.CancelButtonCaption = null;
            this.SubmitSA.Caption = "Submit";
            this.SubmitSA.Category = "ObjectsCreation";
            this.SubmitSA.ConfirmationMessage = null;
            this.SubmitSA.Id = "SubmitSA";
            this.SubmitSA.ToolTip = null;
            this.SubmitSA.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSA_CustomizePopupWindowParams);
            this.SubmitSA.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSA_Execute);
            // 
            // CancelSA
            // 
            this.CancelSA.AcceptButtonCaption = null;
            this.CancelSA.CancelButtonCaption = null;
            this.CancelSA.Caption = "Cancel";
            this.CancelSA.Category = "ObjectsCreation";
            this.CancelSA.ConfirmationMessage = null;
            this.CancelSA.Id = "CancelSA";
            this.CancelSA.ToolTip = null;
            this.CancelSA.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSA_CustomizePopupWindowParams);
            this.CancelSA.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSA_Execute);
            // 
            // PreviewSA
            // 
            this.PreviewSA.Caption = "Preview";
            this.PreviewSA.Category = "ObjectsCreation";
            this.PreviewSA.ConfirmationMessage = null;
            this.PreviewSA.Id = "PreviewSA";
            this.PreviewSA.ToolTip = null;
            this.PreviewSA.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSA_Execute);
            // 
            // StockAdjustmentControllers
            // 
            this.Actions.Add(this.SubmitSA);
            this.Actions.Add(this.CancelSA);
            this.Actions.Add(this.PreviewSA);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSA;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSA;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSA;
    }
}
