namespace StarLaiPortal.Module.Controllers
{
    partial class StockReorderingControllers
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
            this.PrintStockReordering = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PrintStockReordering
            // 
            this.PrintStockReordering.Caption = "Print";
            this.PrintStockReordering.Category = "ObjectsCreation";
            this.PrintStockReordering.ConfirmationMessage = null;
            this.PrintStockReordering.Id = "PrintStockReordering";
            this.PrintStockReordering.ToolTip = null;
            this.PrintStockReordering.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintStockReordering_Execute);
            // 
            // StockReorderingControllers
            // 
            this.Actions.Add(this.PrintStockReordering);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PrintStockReordering;
    }
}
