namespace StarLaiPortal.Module.Controllers
{
    partial class InquiryViewControllers
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
            this.ViewOpenPickList = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ViewPickListDetailInquiry = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ViewPickListInquiry = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ViewOpenPickList
            // 
            this.ViewOpenPickList.AcceptButtonCaption = null;
            this.ViewOpenPickList.CancelButtonCaption = null;
            this.ViewOpenPickList.Caption = "View";
            this.ViewOpenPickList.Category = "ListView";
            this.ViewOpenPickList.ConfirmationMessage = null;
            this.ViewOpenPickList.Id = "ViewOpenPickList";
            this.ViewOpenPickList.ToolTip = null;
            this.ViewOpenPickList.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewOpenPickList_CustomizePopupWindowParams);
            this.ViewOpenPickList.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ViewOpenPickList_Execute);
            // 
            // ViewPickListDetailInquiry
            // 
            this.ViewPickListDetailInquiry.AcceptButtonCaption = null;
            this.ViewPickListDetailInquiry.CancelButtonCaption = null;
            this.ViewPickListDetailInquiry.Caption = "View";
            this.ViewPickListDetailInquiry.Category = "ListView";
            this.ViewPickListDetailInquiry.ConfirmationMessage = null;
            this.ViewPickListDetailInquiry.Id = "ViewPickListDetailInquiry";
            this.ViewPickListDetailInquiry.ToolTip = null;
            this.ViewPickListDetailInquiry.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewPickListDetailInquiry_CustomizePopupWindowParams);
            this.ViewPickListDetailInquiry.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ViewPickListDetailInquiry_Execute);
            // 
            // ViewPickListInquiry
            // 
            this.ViewPickListInquiry.AcceptButtonCaption = null;
            this.ViewPickListInquiry.CancelButtonCaption = null;
            this.ViewPickListInquiry.Caption = "View";
            this.ViewPickListInquiry.Category = "ListView";
            this.ViewPickListInquiry.ConfirmationMessage = null;
            this.ViewPickListInquiry.Id = "ViewPickListInquiry";
            this.ViewPickListInquiry.ToolTip = null;
            this.ViewPickListInquiry.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ViewPickListInquiry_CustomizePopupWindowParams);
            this.ViewPickListInquiry.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ViewPickListInquiry_Execute);
            // 
            // InquiryViewControllers
            // 
            this.Actions.Add(this.ViewOpenPickList);
            this.Actions.Add(this.ViewPickListDetailInquiry);
            this.Actions.Add(this.ViewPickListInquiry);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewOpenPickList;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewPickListDetailInquiry;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ViewPickListInquiry;
    }
}
