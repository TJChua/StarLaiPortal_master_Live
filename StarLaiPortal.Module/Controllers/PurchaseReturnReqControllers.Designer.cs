namespace StarLaiPortal.Module.Controllers
{
    partial class PurchaseReturnReqControllers
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
            this.PRRInquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PRRCopyFromPO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitPRR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelPRR = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewPRR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppPRR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppPRR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppPRR = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PRRCopyToPReturn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppPRR_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // PRRInquiryItem
            // 
            this.PRRInquiryItem.AcceptButtonCaption = null;
            this.PRRInquiryItem.CancelButtonCaption = null;
            this.PRRInquiryItem.Caption = "Inquiry";
            this.PRRInquiryItem.Category = "ListView";
            this.PRRInquiryItem.ConfirmationMessage = null;
            this.PRRInquiryItem.Id = "PRRInquiryItem";
            this.PRRInquiryItem.ToolTip = null;
            this.PRRInquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PRRInquiryItem_CustomizePopupWindowParams);
            this.PRRInquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PRRInquiryItem_Execute);
            // 
            // PRRCopyFromPO
            // 
            this.PRRCopyFromPO.AcceptButtonCaption = null;
            this.PRRCopyFromPO.CancelButtonCaption = null;
            this.PRRCopyFromPO.Caption = "Copy From GRPO";
            this.PRRCopyFromPO.Category = "ObjectsCreation";
            this.PRRCopyFromPO.ConfirmationMessage = null;
            this.PRRCopyFromPO.Id = "PRRCopyFromPO";
            this.PRRCopyFromPO.ToolTip = null;
            this.PRRCopyFromPO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PRRCopyFromPO_CustomizePopupWindowParams);
            this.PRRCopyFromPO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PRRCopyFromPO_Execute);
            // 
            // SubmitPRR
            // 
            this.SubmitPRR.AcceptButtonCaption = null;
            this.SubmitPRR.CancelButtonCaption = null;
            this.SubmitPRR.Caption = "Submit";
            this.SubmitPRR.Category = "ObjectsCreation";
            this.SubmitPRR.ConfirmationMessage = null;
            this.SubmitPRR.Id = "SubmitPRR";
            this.SubmitPRR.ToolTip = null;
            this.SubmitPRR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitPRR_CustomizePopupWindowParams);
            this.SubmitPRR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitPRR_Execute);
            // 
            // CancelPRR
            // 
            this.CancelPRR.AcceptButtonCaption = null;
            this.CancelPRR.CancelButtonCaption = null;
            this.CancelPRR.Caption = "Cancel";
            this.CancelPRR.Category = "ObjectsCreation";
            this.CancelPRR.ConfirmationMessage = null;
            this.CancelPRR.Id = "CancelPRR";
            this.CancelPRR.ToolTip = null;
            this.CancelPRR.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelPRR_CustomizePopupWindowParams);
            this.CancelPRR.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelPRR_Execute);
            // 
            // PreviewPRR
            // 
            this.PreviewPRR.Caption = "Preview";
            this.PreviewPRR.Category = "ObjectsCreation";
            this.PreviewPRR.ConfirmationMessage = null;
            this.PreviewPRR.Id = "PreviewPRR";
            this.PreviewPRR.ToolTip = null;
            this.PreviewPRR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewPRR_Execute);
            // 
            // ReviewAppPRR
            // 
            this.ReviewAppPRR.Caption = "Review";
            this.ReviewAppPRR.Category = "ListView";
            this.ReviewAppPRR.ConfirmationMessage = null;
            this.ReviewAppPRR.Id = "ReviewAppPRR";
            this.ReviewAppPRR.ToolTip = null;
            this.ReviewAppPRR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppPRR_Execute);
            // 
            // ApproveAppPRR
            // 
            this.ApproveAppPRR.Caption = "Approve";
            this.ApproveAppPRR.Category = "ListView";
            this.ApproveAppPRR.ConfirmationMessage = null;
            this.ApproveAppPRR.Id = "ApproveAppPRR";
            this.ApproveAppPRR.ToolTip = null;
            this.ApproveAppPRR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppPRR_Execute);
            // 
            // RejectAppPRR
            // 
            this.RejectAppPRR.Caption = "Reject";
            this.RejectAppPRR.Category = "ListView";
            this.RejectAppPRR.ConfirmationMessage = null;
            this.RejectAppPRR.Id = "RejectAppPRR";
            this.RejectAppPRR.ToolTip = null;
            this.RejectAppPRR.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppPRR_Execute);
            // 
            // PRRCopyToPReturn
            // 
            this.PRRCopyToPReturn.Caption = "Copy To Purchase Return";
            this.PRRCopyToPReturn.Category = "ObjectsCreation";
            this.PRRCopyToPReturn.ConfirmationMessage = null;
            this.PRRCopyToPReturn.Id = "PRRCopyToPReturn";
            this.PRRCopyToPReturn.ToolTip = null;
            this.PRRCopyToPReturn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PRRCopyToPReturn_Execute);
            // 
            // ApproveAppPRR_Pop
            // 
            this.ApproveAppPRR_Pop.AcceptButtonCaption = null;
            this.ApproveAppPRR_Pop.CancelButtonCaption = null;
            this.ApproveAppPRR_Pop.Caption = "Approve";
            this.ApproveAppPRR_Pop.Category = "ObjectsCreation";
            this.ApproveAppPRR_Pop.ConfirmationMessage = null;
            this.ApproveAppPRR_Pop.Id = "ApproveAppPRR_Pop";
            this.ApproveAppPRR_Pop.ToolTip = null;
            this.ApproveAppPRR_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppPRR_Pop_CustomizePopupWindowParams);
            this.ApproveAppPRR_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppPRR_Pop_Execute);
            // 
            // PurchaseReturnReqControllers
            // 
            this.Actions.Add(this.PRRInquiryItem);
            this.Actions.Add(this.PRRCopyFromPO);
            this.Actions.Add(this.SubmitPRR);
            this.Actions.Add(this.CancelPRR);
            this.Actions.Add(this.PreviewPRR);
            this.Actions.Add(this.ReviewAppPRR);
            this.Actions.Add(this.ApproveAppPRR);
            this.Actions.Add(this.RejectAppPRR);
            this.Actions.Add(this.PRRCopyToPReturn);
            this.Actions.Add(this.ApproveAppPRR_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PRRInquiryItem;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PRRCopyFromPO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitPRR;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelPRR;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewPRR;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppPRR;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppPRR;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppPRR;
        private DevExpress.ExpressApp.Actions.SimpleAction PRRCopyToPReturn;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppPRR_Pop;
    }
}
