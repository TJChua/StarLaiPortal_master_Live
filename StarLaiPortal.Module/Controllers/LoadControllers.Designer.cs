namespace StarLaiPortal.Module.Controllers
{
    partial class LoadControllers
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
            this.LCopyFromPAL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.LGenerateDO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // LCopyFromPAL
            // 
            this.LCopyFromPAL.AcceptButtonCaption = null;
            this.LCopyFromPAL.CancelButtonCaption = null;
            this.LCopyFromPAL.Caption = "Copy From Packlist";
            this.LCopyFromPAL.Category = "ObjectsCreation";
            this.LCopyFromPAL.ConfirmationMessage = null;
            this.LCopyFromPAL.Id = "LCopyFromPAL";
            this.LCopyFromPAL.ToolTip = null;
            this.LCopyFromPAL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.LCopyFromPAL_CustomizePopupWindowParams);
            this.LCopyFromPAL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.LCopyFromPAL_Execute);
            // 
            // SubmitL
            // 
            this.SubmitL.AcceptButtonCaption = null;
            this.SubmitL.CancelButtonCaption = null;
            this.SubmitL.Caption = "Submit";
            this.SubmitL.Category = "ObjectsCreation";
            this.SubmitL.ConfirmationMessage = null;
            this.SubmitL.Id = "SubmitL";
            this.SubmitL.ToolTip = null;
            this.SubmitL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitL_CustomizePopupWindowParams);
            this.SubmitL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitL_Execute);
            // 
            // CancelL
            // 
            this.CancelL.AcceptButtonCaption = null;
            this.CancelL.CancelButtonCaption = null;
            this.CancelL.Caption = "Cancel";
            this.CancelL.Category = "ObjectsCreation";
            this.CancelL.ConfirmationMessage = null;
            this.CancelL.Id = "CancelL";
            this.CancelL.ToolTip = null;
            this.CancelL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelL_CustomizePopupWindowParams);
            this.CancelL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelL_Execute);
            // 
            // LGenerateDO
            // 
            this.LGenerateDO.Caption = "Generate DO";
            this.LGenerateDO.Category = "ObjectsCreation";
            this.LGenerateDO.ConfirmationMessage = null;
            this.LGenerateDO.Id = "LGenerateDO";
            this.LGenerateDO.ToolTip = null;
            this.LGenerateDO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LGenerateDO_Execute);
            // 
            // LoadControllers
            // 
            this.Actions.Add(this.LCopyFromPAL);
            this.Actions.Add(this.SubmitL);
            this.Actions.Add(this.CancelL);
            this.Actions.Add(this.LGenerateDO);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction LCopyFromPAL;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitL;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelL;
        private DevExpress.ExpressApp.Actions.SimpleAction LGenerateDO;
    }
}
