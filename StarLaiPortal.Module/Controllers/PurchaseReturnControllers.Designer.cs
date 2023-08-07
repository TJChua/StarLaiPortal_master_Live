namespace StarLaiPortal.Module.Controllers
{
    partial class PurchaseReturnControllers
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
            this.SubmitPReturn = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelPReturn = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewPReturn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitPReturn
            // 
            this.SubmitPReturn.AcceptButtonCaption = null;
            this.SubmitPReturn.CancelButtonCaption = null;
            this.SubmitPReturn.Caption = "Submit";
            this.SubmitPReturn.Category = "ObjectsCreation";
            this.SubmitPReturn.ConfirmationMessage = null;
            this.SubmitPReturn.Id = "SubmitPReturn";
            this.SubmitPReturn.ToolTip = null;
            this.SubmitPReturn.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitPReturn_CustomizePopupWindowParams);
            this.SubmitPReturn.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitPReturn_Execute);
            // 
            // CancelPReturn
            // 
            this.CancelPReturn.AcceptButtonCaption = null;
            this.CancelPReturn.CancelButtonCaption = null;
            this.CancelPReturn.Caption = "Cancel";
            this.CancelPReturn.Category = "ObjectsCreation";
            this.CancelPReturn.ConfirmationMessage = null;
            this.CancelPReturn.Id = "CancelPReturn";
            this.CancelPReturn.ToolTip = null;
            this.CancelPReturn.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelPReturn_CustomizePopupWindowParams);
            this.CancelPReturn.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelPReturn_Execute);
            // 
            // PreviewPReturn
            // 
            this.PreviewPReturn.Caption = "Preview";
            this.PreviewPReturn.Category = "ObjectsCreation";
            this.PreviewPReturn.ConfirmationMessage = null;
            this.PreviewPReturn.Id = "PreviewPReturn";
            this.PreviewPReturn.ToolTip = null;
            this.PreviewPReturn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewPReturn_Execute);
            // 
            // PurchaseReturnControllers
            // 
            this.Actions.Add(this.SubmitPReturn);
            this.Actions.Add(this.CancelPReturn);
            this.Actions.Add(this.PreviewPReturn);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitPReturn;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelPReturn;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewPReturn;
    }
}
