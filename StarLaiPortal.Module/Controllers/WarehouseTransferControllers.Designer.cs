namespace StarLaiPortal.Module.Controllers
{
    partial class WarehouseTransferControllers
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
            this.SubmitWT = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelWT = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewWT = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitWT
            // 
            this.SubmitWT.AcceptButtonCaption = null;
            this.SubmitWT.CancelButtonCaption = null;
            this.SubmitWT.Caption = "Submit";
            this.SubmitWT.Category = "ObjectsCreation";
            this.SubmitWT.ConfirmationMessage = null;
            this.SubmitWT.Id = "SubmitWT";
            this.SubmitWT.ToolTip = null;
            this.SubmitWT.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitWT_CustomizePopupWindowParams);
            this.SubmitWT.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitWT_Execute);
            // 
            // CancelWT
            // 
            this.CancelWT.AcceptButtonCaption = null;
            this.CancelWT.CancelButtonCaption = null;
            this.CancelWT.Caption = "Cancel";
            this.CancelWT.Category = "ObjectsCreation";
            this.CancelWT.ConfirmationMessage = null;
            this.CancelWT.Id = "CancelWT";
            this.CancelWT.ToolTip = null;
            this.CancelWT.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelWT_CustomizePopupWindowParams);
            this.CancelWT.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelWT_Execute);
            // 
            // PreviewWT
            // 
            this.PreviewWT.Caption = "Preview";
            this.PreviewWT.Category = "ObjectsCreation";
            this.PreviewWT.ConfirmationMessage = null;
            this.PreviewWT.Id = "PreviewWT";
            this.PreviewWT.ToolTip = null;
            this.PreviewWT.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewWT_Execute);
            // 
            // WarehouseTransferControllers
            // 
            this.Actions.Add(this.SubmitWT);
            this.Actions.Add(this.CancelWT);
            this.Actions.Add(this.PreviewWT);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitWT;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelWT;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewWT;
    }
}
