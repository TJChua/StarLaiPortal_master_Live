namespace StarLaiPortal.Module.Controllers
{
    partial class ItemInquiryControllers
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
            this.Search_ItemInquiry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ViewSales = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.OpenCart = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddToCartPop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ViewItemPicture = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddToCartSimple = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Search_ItemInquiry
            // 
            this.Search_ItemInquiry.Caption = "Search";
            this.Search_ItemInquiry.Category = "ListView";
            this.Search_ItemInquiry.ConfirmationMessage = null;
            this.Search_ItemInquiry.Id = "Search_ItemInquiry";
            this.Search_ItemInquiry.ToolTip = null;
            this.Search_ItemInquiry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Search_ItemInquiry_Execute);
            // 
            // ViewSales
            // 
            this.ViewSales.AcceptButtonCaption = null;
            this.ViewSales.CancelButtonCaption = null;
            this.ViewSales.Caption = "View Sales";
            this.ViewSales.ConfirmationMessage = null;
            this.ViewSales.Id = "ViewSales";
            this.ViewSales.ToolTip = null;
            this.ViewSales.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewSales_CustomizePopupWindowParams);
            this.ViewSales.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ViewSales_Execute);
            // 
            // OpenCart
            // 
            this.OpenCart.Caption = "Open Cart";
            this.OpenCart.Category = "ListView";
            this.OpenCart.ConfirmationMessage = null;
            this.OpenCart.Id = "OpenCart";
            this.OpenCart.ToolTip = null;
            this.OpenCart.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenCart_Execute);
            // 
            // AddToCartPop
            // 
            this.AddToCartPop.AcceptButtonCaption = null;
            this.AddToCartPop.CancelButtonCaption = null;
            this.AddToCartPop.Caption = "Add";
            this.AddToCartPop.ConfirmationMessage = null;
            this.AddToCartPop.Id = "AddToCartPop";
            this.AddToCartPop.ToolTip = null;
            this.AddToCartPop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.AddToCartPop_CustomizePopupWindowParams);
            this.AddToCartPop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.AddToCartPop_Execute);
            // 
            // ViewItemPicture
            // 
            this.ViewItemPicture.Caption = "View";
            this.ViewItemPicture.Category = "ListView";
            this.ViewItemPicture.ConfirmationMessage = null;
            this.ViewItemPicture.Id = "ViewItemPicture";
            this.ViewItemPicture.ToolTip = null;
            this.ViewItemPicture.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewItemPicture_Execute);
            // 
            // AddToCartSimple
            // 
            this.AddToCartSimple.Caption = "Add";
            this.AddToCartSimple.ConfirmationMessage = null;
            this.AddToCartSimple.Id = "AddToCartSimple";
            this.AddToCartSimple.ToolTip = null;
            this.AddToCartSimple.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddToCartSimple_Execute);
            // 
            // ItemInquiryControllers
            // 
            this.Actions.Add(this.Search_ItemInquiry);
            this.Actions.Add(this.ViewSales);
            this.Actions.Add(this.OpenCart);
            this.Actions.Add(this.AddToCartPop);
            this.Actions.Add(this.ViewItemPicture);
            this.Actions.Add(this.AddToCartSimple);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Search_ItemInquiry;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewSales;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenCart;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction AddToCartPop;
        private DevExpress.ExpressApp.Actions.SimpleAction ViewItemPicture;
        private DevExpress.ExpressApp.Actions.SimpleAction AddToCartSimple;
    }
}
