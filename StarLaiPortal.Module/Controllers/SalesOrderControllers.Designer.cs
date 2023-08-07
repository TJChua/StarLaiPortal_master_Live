namespace StarLaiPortal.Module.Controllers
{
    partial class SalesOrderControllers
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
            this.PreviewSO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PreviewSO
            // 
            this.PreviewSO.Caption = "Preview";
            this.PreviewSO.Category = "ObjectsCreation";
            this.PreviewSO.ConfirmationMessage = null;
            this.PreviewSO.Id = "PreviewSO";
            this.PreviewSO.ToolTip = null;
            this.PreviewSO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSO_Execute);
            // 
            // SalesOrderControllers
            // 
            this.Actions.Add(this.PreviewSO);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSO;
    }
}
