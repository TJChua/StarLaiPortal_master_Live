namespace StarLaiPortal.Module.Controllers
{
    partial class PrintLabelControllers
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
            this.AddReportItem = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RetriveDocItem = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PrintLabel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddReportItem
            // 
            this.AddReportItem.Caption = "Add Item";
            this.AddReportItem.Category = "ListView";
            this.AddReportItem.ConfirmationMessage = null;
            this.AddReportItem.Id = "AddReportItem";
            this.AddReportItem.ToolTip = null;
            this.AddReportItem.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddReportItem_Execute);
            // 
            // RetriveDocItem
            // 
            this.RetriveDocItem.Caption = "Retrive Doc Item";
            this.RetriveDocItem.Category = "ListView";
            this.RetriveDocItem.ConfirmationMessage = null;
            this.RetriveDocItem.Id = "RetriveDocItem";
            this.RetriveDocItem.ToolTip = null;
            this.RetriveDocItem.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RetriveDocItem_Execute);
            // 
            // PrintLabel
            // 
            this.PrintLabel.Caption = "Print";
            this.PrintLabel.Category = "ObjectsCreation";
            this.PrintLabel.ConfirmationMessage = null;
            this.PrintLabel.Id = "PrintLabel";
            this.PrintLabel.ToolTip = null;
            this.PrintLabel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintLabel_Execute);
            // 
            // PrintLabelControllers
            // 
            this.Actions.Add(this.AddReportItem);
            this.Actions.Add(this.RetriveDocItem);
            this.Actions.Add(this.PrintLabel);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AddReportItem;
        private DevExpress.ExpressApp.Actions.SimpleAction RetriveDocItem;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintLabel;
    }
}
