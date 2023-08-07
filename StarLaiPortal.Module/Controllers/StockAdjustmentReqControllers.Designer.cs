namespace StarLaiPortal.Module.Controllers
{
    partial class StockAdjustmentReqControllers
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
            this.SARInquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitSAR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSAR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewSAR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppSAR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSAR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppSAR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SARCopyToSA = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSAR_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // SARInquiryItem
            // 
            this.SARInquiryItem.AcceptButtonCaption = null;
            this.SARInquiryItem.CancelButtonCaption = null;
            this.SARInquiryItem.Caption = "Inquiry";
            this.SARInquiryItem.Category = "ListView";
            this.SARInquiryItem.ConfirmationMessage = null;
            this.SARInquiryItem.Id = "SARInquiryItem";
            this.SARInquiryItem.ToolTip = null;
            this.SARInquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SARInquiryItem_CustomizePopupWindowParams);
            this.SARInquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SARInquiryItem_Execute);
            // 
            // SubmitSAR
            // 
            this.SubmitSAR.AcceptButtonCaption = null;
            this.SubmitSAR.CancelButtonCaption = null;
            this.SubmitSAR.Caption = "Submit";
            this.SubmitSAR.Category = "ObjectsCreation";
            this.SubmitSAR.ConfirmationMessage = null;
            this.SubmitSAR.Id = "SubmitSAR";
            this.SubmitSAR.ToolTip = null;
            this.SubmitSAR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSAR_CustomizePopupWindowParams);
            this.SubmitSAR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSAR_Execute);
            // 
            // CancelSAR
            // 
            this.CancelSAR.AcceptButtonCaption = null;
            this.CancelSAR.CancelButtonCaption = null;
            this.CancelSAR.Caption = "Cancel";
            this.CancelSAR.Category = "ObjectsCreation";
            this.CancelSAR.ConfirmationMessage = null;
            this.CancelSAR.Id = "CancelSAR";
            this.CancelSAR.ToolTip = null;
            this.CancelSAR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSAR_CustomizePopupWindowParams);
            this.CancelSAR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSAR_Execute);
            // 
            // PreviewSAR
            // 
            this.PreviewSAR.Caption = "Preview";
            this.PreviewSAR.Category = "ObjectsCreation";
            this.PreviewSAR.ConfirmationMessage = null;
            this.PreviewSAR.Id = "PreviewSAR";
            this.PreviewSAR.ToolTip = null;
            this.PreviewSAR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSAR_Execute);
            // 
            // ReviewAppSAR
            // 
            this.ReviewAppSAR.Caption = "Review";
            this.ReviewAppSAR.Category = "ListView";
            this.ReviewAppSAR.ConfirmationMessage = null;
            this.ReviewAppSAR.Id = "ReviewAppSAR";
            this.ReviewAppSAR.ToolTip = null;
            this.ReviewAppSAR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppSAR_Execute);
            // 
            // ApproveAppSAR
            // 
            this.ApproveAppSAR.Caption = "Approve";
            this.ApproveAppSAR.Category = "ListView";
            this.ApproveAppSAR.ConfirmationMessage = null;
            this.ApproveAppSAR.Id = "ApproveAppSAR";
            this.ApproveAppSAR.ToolTip = null;
            this.ApproveAppSAR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppSAR_Execute);
            // 
            // RejectAppSAR
            // 
            this.RejectAppSAR.Caption = "Reject";
            this.RejectAppSAR.Category = "ListView";
            this.RejectAppSAR.ConfirmationMessage = null;
            this.RejectAppSAR.Id = "RejectAppSAR";
            this.RejectAppSAR.ToolTip = null;
            this.RejectAppSAR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppSAR_Execute);
            // 
            // SARCopyToSA
            // 
            this.SARCopyToSA.Caption = "Copy To Stock Adjustment";
            this.SARCopyToSA.Category = "ObjectsCreation";
            this.SARCopyToSA.ConfirmationMessage = null;
            this.SARCopyToSA.Id = "SARCopyToSA";
            this.SARCopyToSA.ToolTip = null;
            this.SARCopyToSA.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SARCopyToSA_Execute);
            // 
            // ApproveAppSAR_Pop
            // 
            this.ApproveAppSAR_Pop.AcceptButtonCaption = null;
            this.ApproveAppSAR_Pop.CancelButtonCaption = null;
            this.ApproveAppSAR_Pop.Caption = "Approve";
            this.ApproveAppSAR_Pop.Category = "ObjectsCreation";
            this.ApproveAppSAR_Pop.ConfirmationMessage = null;
            this.ApproveAppSAR_Pop.Id = "ApproveAppSAR_Pop";
            this.ApproveAppSAR_Pop.ToolTip = null;
            this.ApproveAppSAR_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppSAR_Pop_CustomizePopupWindowParams);
            this.ApproveAppSAR_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppSAR_Pop_Execute);
            // 
            // StockAdjustmentReqControllers
            // 
            this.Actions.Add(this.SARInquiryItem);
            this.Actions.Add(this.SubmitSAR);
            this.Actions.Add(this.CancelSAR);
            this.Actions.Add(this.PreviewSAR);
            this.Actions.Add(this.ReviewAppSAR);
            this.Actions.Add(this.ApproveAppSAR);
            this.Actions.Add(this.RejectAppSAR);
            this.Actions.Add(this.SARCopyToSA);
            this.Actions.Add(this.ApproveAppSAR_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SARInquiryItem;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSAR;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSAR;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSAR;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppSAR;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppSAR;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppSAR;
        private DevExpress.ExpressApp.Actions.SimpleAction SARCopyToSA;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppSAR_Pop;
    }
}
