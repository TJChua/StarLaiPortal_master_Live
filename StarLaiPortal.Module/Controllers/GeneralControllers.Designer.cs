namespace StarLaiPortal.Module.Controllers
{
    partial class GeneralControllers
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
            this.ListNewButton = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ListNewButton
            // 
            this.ListNewButton.AcceptButtonCaption = null;
            this.ListNewButton.CancelButtonCaption = null;
            this.ListNewButton.Caption = "Add User";
            this.ListNewButton.Category = "ListView";
            this.ListNewButton.ConfirmationMessage = null;
            this.ListNewButton.Id = "ListNewButton";
            this.ListNewButton.ToolTip = null;
            this.ListNewButton.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ListNewButton_CustomizePopupWindowParams);
            this.ListNewButton.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ListNewButton_Execute);
            // 
            // GeneralControllers
            // 
            this.Actions.Add(this.ListNewButton);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ListNewButton;
    }
}
