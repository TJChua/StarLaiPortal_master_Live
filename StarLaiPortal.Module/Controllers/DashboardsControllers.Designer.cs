namespace StarLaiPortal.Module.Controllers
{
    partial class DashboardsControllers
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
            this.DashboardWarehouse = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ViewDoc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ViewDashboardDoc = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // DashboardWarehouse
            // 
            this.DashboardWarehouse.Caption = "Warehouse";
            this.DashboardWarehouse.Category = "ObjectsCreation";
            this.DashboardWarehouse.ConfirmationMessage = null;
            this.DashboardWarehouse.Id = "DashboardWarehouse";
            this.DashboardWarehouse.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.DashboardWarehouse.ToolTip = null;
            this.DashboardWarehouse.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DashboardWarehouse_Execute);
            // 
            // ViewDoc
            // 
            this.ViewDoc.Caption = "View";
            this.ViewDoc.Category = "ListView";
            this.ViewDoc.ConfirmationMessage = null;
            this.ViewDoc.Id = "ViewDoc";
            this.ViewDoc.ToolTip = null;
            this.ViewDoc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewDoc_Execute);
            // 
            // ViewDashboardDoc
            // 
            this.ViewDashboardDoc.AcceptButtonCaption = null;
            this.ViewDashboardDoc.CancelButtonCaption = null;
            this.ViewDashboardDoc.Caption = "View";
            this.ViewDashboardDoc.Category = "ListView";
            this.ViewDashboardDoc.ConfirmationMessage = null;
            this.ViewDashboardDoc.Id = "ViewDashboardDoc";
            this.ViewDashboardDoc.ToolTip = null;
            this.ViewDashboardDoc.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewDashboardDoc_CustomizePopupWindowParams);
            this.ViewDashboardDoc.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ViewDashboardDoc_Execute);
            // 
            // DashboardsControllers
            // 
            this.Actions.Add(this.DashboardWarehouse);
            this.Actions.Add(this.ViewDoc);
            this.Actions.Add(this.ViewDashboardDoc);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction DashboardWarehouse;
        private DevExpress.ExpressApp.Actions.SimpleAction ViewDoc;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewDashboardDoc;
    }
}
