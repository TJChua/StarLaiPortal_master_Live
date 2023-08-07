namespace StarLaiPortal.Module.Controllers
{
    partial class WarehouseTransferReqControllers
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
            this.WTRInquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitWTR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelWTR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewWTR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppWTR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppWTR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppWTR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.WTRCopyToWT = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppWTR_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // WTRInquiryItem
            // 
            this.WTRInquiryItem.AcceptButtonCaption = null;
            this.WTRInquiryItem.CancelButtonCaption = null;
            this.WTRInquiryItem.Caption = "Inquiry";
            this.WTRInquiryItem.Category = "ListView";
            this.WTRInquiryItem.ConfirmationMessage = null;
            this.WTRInquiryItem.Id = "WTRInquiryItem";
            this.WTRInquiryItem.ToolTip = null;
            this.WTRInquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.WTRInquiryItem_CustomizePopupWindowParams);
            this.WTRInquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.WTRInquiryItem_Execute);
            // 
            // SubmitWTR
            // 
            this.SubmitWTR.AcceptButtonCaption = null;
            this.SubmitWTR.CancelButtonCaption = null;
            this.SubmitWTR.Caption = "Submit";
            this.SubmitWTR.Category = "ObjectsCreation";
            this.SubmitWTR.ConfirmationMessage = null;
            this.SubmitWTR.Id = "SubmitWTR";
            this.SubmitWTR.ToolTip = null;
            this.SubmitWTR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitWTR_CustomizePopupWindowParams);
            this.SubmitWTR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitWTR_Execute);
            // 
            // CancelWTR
            // 
            this.CancelWTR.AcceptButtonCaption = null;
            this.CancelWTR.CancelButtonCaption = null;
            this.CancelWTR.Caption = "Cancel";
            this.CancelWTR.Category = "ObjectsCreation";
            this.CancelWTR.ConfirmationMessage = null;
            this.CancelWTR.Id = "CancelWTR";
            this.CancelWTR.ToolTip = null;
            this.CancelWTR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelWTR_CustomizePopupWindowParams);
            this.CancelWTR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelWTR_Execute);
            // 
            // PreviewWTR
            // 
            this.PreviewWTR.Caption = "Preview";
            this.PreviewWTR.Category = "ObjectsCreation";
            this.PreviewWTR.ConfirmationMessage = null;
            this.PreviewWTR.Id = "PreviewWTR";
            this.PreviewWTR.ToolTip = null;
            this.PreviewWTR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewWTR_Execute);
            // 
            // ReviewAppWTR
            // 
            this.ReviewAppWTR.Caption = "Review";
            this.ReviewAppWTR.Category = "ListView";
            this.ReviewAppWTR.ConfirmationMessage = null;
            this.ReviewAppWTR.Id = "ReviewAppWTR";
            this.ReviewAppWTR.ToolTip = null;
            this.ReviewAppWTR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppWTR_Execute);
            // 
            // ApproveAppWTR
            // 
            this.ApproveAppWTR.Caption = "Approve";
            this.ApproveAppWTR.Category = "ListView";
            this.ApproveAppWTR.ConfirmationMessage = null;
            this.ApproveAppWTR.Id = "ApproveAppWTR";
            this.ApproveAppWTR.ToolTip = null;
            this.ApproveAppWTR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppWTR_Execute);
            // 
            // RejectAppWTR
            // 
            this.RejectAppWTR.Caption = "Reject";
            this.RejectAppWTR.Category = "ListView";
            this.RejectAppWTR.ConfirmationMessage = null;
            this.RejectAppWTR.Id = "RejectAppWTR";
            this.RejectAppWTR.ToolTip = null;
            this.RejectAppWTR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppWTR_Execute);
            // 
            // WTRCopyToWT
            // 
            this.WTRCopyToWT.Caption = "Copy To Warehouse Transfer";
            this.WTRCopyToWT.Category = "ObjectsCreation";
            this.WTRCopyToWT.ConfirmationMessage = null;
            this.WTRCopyToWT.Id = "WTRCopyToWT";
            this.WTRCopyToWT.ToolTip = null;
            this.WTRCopyToWT.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.WTRCopyToWT_Execute);
            // 
            // ApproveAppWTR_Pop
            // 
            this.ApproveAppWTR_Pop.AcceptButtonCaption = null;
            this.ApproveAppWTR_Pop.CancelButtonCaption = null;
            this.ApproveAppWTR_Pop.Caption = "Approve";
            this.ApproveAppWTR_Pop.Category = "ObjectsCreation";
            this.ApproveAppWTR_Pop.ConfirmationMessage = null;
            this.ApproveAppWTR_Pop.Id = "ApproveAppWTR_Pop";
            this.ApproveAppWTR_Pop.ToolTip = null;
            this.ApproveAppWTR_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppWTR_Pop_CustomizePopupWindowParams);
            this.ApproveAppWTR_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppWTR_Pop_Execute);
            // 
            // WarehouseTransferReqControllers
            // 
            this.Actions.Add(this.WTRInquiryItem);
            this.Actions.Add(this.SubmitWTR);
            this.Actions.Add(this.CancelWTR);
            this.Actions.Add(this.PreviewWTR);
            this.Actions.Add(this.ReviewAppWTR);
            this.Actions.Add(this.ApproveAppWTR);
            this.Actions.Add(this.RejectAppWTR);
            this.Actions.Add(this.WTRCopyToWT);
            this.Actions.Add(this.ApproveAppWTR_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction WTRInquiryItem;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitWTR;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelWTR;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewWTR;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppWTR;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppWTR;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppWTR;
        private DevExpress.ExpressApp.Actions.SimpleAction WTRCopyToWT;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppWTR_Pop;
    }
}
