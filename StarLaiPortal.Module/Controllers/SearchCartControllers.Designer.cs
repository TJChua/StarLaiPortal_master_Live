namespace StarLaiPortal.Module.Controllers
{
    partial class SearchCartControllers
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
            this.SearchOpenCart = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SearchOpenCart
            // 
            this.SearchOpenCart.Caption = "Open Cart";
            this.SearchOpenCart.ConfirmationMessage = null;
            this.SearchOpenCart.Id = "SearchOpenCart";
            this.SearchOpenCart.ToolTip = null;
            this.SearchOpenCart.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SearchOpenCart_Execute);
            // 
            // SearchCartControllers
            // 
            this.Actions.Add(this.SearchOpenCart);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SearchOpenCart;
    }
}
