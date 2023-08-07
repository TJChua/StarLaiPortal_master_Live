namespace StarLaiPortal.Module.Controllers
{
    partial class SalesRefundReqControllers
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
            this.SFRInquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitSFR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSFR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewSFR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppSFR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSFR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppSFR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SFRCopyToSF = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSFR_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // SFRInquiryItem
            // 
            this.SFRInquiryItem.AcceptButtonCaption = null;
            this.SFRInquiryItem.CancelButtonCaption = null;
            this.SFRInquiryItem.Caption = "Inquiry";
            this.SFRInquiryItem.Category = "ListView";
            this.SFRInquiryItem.ConfirmationMessage = null;
            this.SFRInquiryItem.Id = "SFRInquiryItem";
            this.SFRInquiryItem.ToolTip = null;
            this.SFRInquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SFRInquiryItem_CustomizePopupWindowParams);
            this.SFRInquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SFRInquiryItem_Execute);
            // 
            // SubmitSFR
            // 
            this.SubmitSFR.AcceptButtonCaption = null;
            this.SubmitSFR.CancelButtonCaption = null;
            this.SubmitSFR.Caption = "Submit";
            this.SubmitSFR.Category = "ObjectsCreation";
            this.SubmitSFR.ConfirmationMessage = null;
            this.SubmitSFR.Id = "SubmitSFR";
            this.SubmitSFR.ToolTip = null;
            this.SubmitSFR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSFR_CustomizePopupWindowParams);
            this.SubmitSFR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSFR_Execute);
            // 
            // CancelSFR
            // 
            this.CancelSFR.AcceptButtonCaption = null;
            this.CancelSFR.CancelButtonCaption = null;
            this.CancelSFR.Caption = "Cancel";
            this.CancelSFR.Category = "ObjectsCreation";
            this.CancelSFR.ConfirmationMessage = null;
            this.CancelSFR.Id = "CancelSFR";
            this.CancelSFR.ToolTip = null;
            this.CancelSFR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSFR_CustomizePopupWindowParams);
            this.CancelSFR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSFR_Execute);
            // 
            // PreviewSFR
            // 
            this.PreviewSFR.Caption = "Preview";
            this.PreviewSFR.Category = "ObjectsCreation";
            this.PreviewSFR.ConfirmationMessage = null;
            this.PreviewSFR.Id = "PreviewSFR";
            this.PreviewSFR.ToolTip = null;
            this.PreviewSFR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSFR_Execute);
            // 
            // ReviewAppSFR
            // 
            this.ReviewAppSFR.Caption = "Review";
            this.ReviewAppSFR.Category = "ListView";
            this.ReviewAppSFR.ConfirmationMessage = null;
            this.ReviewAppSFR.Id = "ReviewAppSFR";
            this.ReviewAppSFR.ToolTip = null;
            this.ReviewAppSFR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppSFR_Execute);
            // 
            // ApproveAppSFR
            // 
            this.ApproveAppSFR.Caption = "Approve";
            this.ApproveAppSFR.Category = "ListView";
            this.ApproveAppSFR.ConfirmationMessage = null;
            this.ApproveAppSFR.Id = "ApproveAppSFR";
            this.ApproveAppSFR.ToolTip = null;
            this.ApproveAppSFR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppSFR_Execute);
            // 
            // RejectAppSFR
            // 
            this.RejectAppSFR.Caption = "Reject";
            this.RejectAppSFR.Category = "ListView";
            this.RejectAppSFR.ConfirmationMessage = null;
            this.RejectAppSFR.Id = "RejectAppSFR";
            this.RejectAppSFR.ToolTip = null;
            this.RejectAppSFR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppSFR_Execute);
            // 
            // SFRCopyToSF
            // 
            this.SFRCopyToSF.Caption = "Copy To Sales Refund";
            this.SFRCopyToSF.ConfirmationMessage = null;
            this.SFRCopyToSF.Id = "SFRCopyToSF";
            this.SFRCopyToSF.ToolTip = null;
            this.SFRCopyToSF.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SFRCopyToSF_Execute);
            // 
            // ApproveAppSFR_Pop
            // 
            this.ApproveAppSFR_Pop.AcceptButtonCaption = null;
            this.ApproveAppSFR_Pop.CancelButtonCaption = null;
            this.ApproveAppSFR_Pop.Caption = "Approve";
            this.ApproveAppSFR_Pop.Category = "ObjectsCreation";
            this.ApproveAppSFR_Pop.ConfirmationMessage = null;
            this.ApproveAppSFR_Pop.Id = "ApproveAppSFR_Pop";
            this.ApproveAppSFR_Pop.ToolTip = null;
            this.ApproveAppSFR_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppPRR_Pop_CustomizePopupWindowParams);
            this.ApproveAppSFR_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppPRR_Pop_Execute);
            // 
            // SalesRefundReqControllers
            // 
            this.Actions.Add(this.SFRInquiryItem);
            this.Actions.Add(this.SubmitSFR);
            this.Actions.Add(this.CancelSFR);
            this.Actions.Add(this.PreviewSFR);
            this.Actions.Add(this.ReviewAppSFR);
            this.Actions.Add(this.ApproveAppSFR);
            this.Actions.Add(this.RejectAppSFR);
            this.Actions.Add(this.SFRCopyToSF);
            this.Actions.Add(this.ApproveAppSFR_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SFRInquiryItem;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSFR;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSFR;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSFR;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppSFR;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppSFR;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppSFR;
        private DevExpress.ExpressApp.Actions.SimpleAction SFRCopyToSF;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppSFR_Pop;
    }
}
