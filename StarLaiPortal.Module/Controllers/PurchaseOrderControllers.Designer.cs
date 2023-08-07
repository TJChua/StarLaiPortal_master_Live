namespace StarLaiPortal.Module.Controllers
{
    partial class PurchaseOrderControllers
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
            this.SubmitPO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelPO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewPO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POInquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.DuplicatePO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppPO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppPO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppPO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POCopyFromSO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ApproveAppPO_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewPONoCost = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubmitPO
            // 
            this.SubmitPO.AcceptButtonCaption = null;
            this.SubmitPO.CancelButtonCaption = null;
            this.SubmitPO.Caption = "Submit";
            this.SubmitPO.Category = "ObjectsCreation";
            this.SubmitPO.ConfirmationMessage = null;
            this.SubmitPO.Id = "SubmitPO";
            this.SubmitPO.ToolTip = null;
            this.SubmitPO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitPO_CustomizePopupWindowParams);
            this.SubmitPO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitPO_Execute);
            // 
            // CancelPO
            // 
            this.CancelPO.AcceptButtonCaption = null;
            this.CancelPO.CancelButtonCaption = null;
            this.CancelPO.Caption = "Cancel PO";
            this.CancelPO.Category = "ObjectsCreation";
            this.CancelPO.ConfirmationMessage = null;
            this.CancelPO.Id = "CancelPO";
            this.CancelPO.ToolTip = null;
            this.CancelPO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelPO_CustomizePopupWindowParams);
            this.CancelPO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelPO_Execute);
            // 
            // PreviewPO
            // 
            this.PreviewPO.Caption = "Preview";
            this.PreviewPO.Category = "ObjectsCreation";
            this.PreviewPO.ConfirmationMessage = null;
            this.PreviewPO.Id = "PreviewPO";
            this.PreviewPO.ToolTip = null;
            this.PreviewPO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewPO_Execute);
            // 
            // POInquiryItem
            // 
            this.POInquiryItem.AcceptButtonCaption = null;
            this.POInquiryItem.CancelButtonCaption = null;
            this.POInquiryItem.Caption = "Inquiry";
            this.POInquiryItem.Category = "ListView";
            this.POInquiryItem.ConfirmationMessage = null;
            this.POInquiryItem.Id = "POInquiryItem";
            this.POInquiryItem.ToolTip = null;
            this.POInquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.POInquiryItem_CustomizePopupWindowParams);
            this.POInquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.POInquiryItem_Execute);
            // 
            // DuplicatePO
            // 
            this.DuplicatePO.Caption = "Duplicate";
            this.DuplicatePO.Category = "ObjectsCreation";
            this.DuplicatePO.ConfirmationMessage = null;
            this.DuplicatePO.Id = "DuplicatePO";
            this.DuplicatePO.ToolTip = null;
            this.DuplicatePO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DuplicatePO_Execute);
            // 
            // ReviewAppPO
            // 
            this.ReviewAppPO.Caption = "Review";
            this.ReviewAppPO.Category = "ListView";
            this.ReviewAppPO.ConfirmationMessage = null;
            this.ReviewAppPO.Id = "ReviewAppPO";
            this.ReviewAppPO.ToolTip = null;
            this.ReviewAppPO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppPO_Execute);
            // 
            // ApproveAppPO
            // 
            this.ApproveAppPO.Caption = "Approve";
            this.ApproveAppPO.Category = "ListView";
            this.ApproveAppPO.ConfirmationMessage = null;
            this.ApproveAppPO.Id = "ApproveAppPO";
            this.ApproveAppPO.ToolTip = null;
            this.ApproveAppPO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppPO_Execute);
            // 
            // RejectAppPO
            // 
            this.RejectAppPO.Caption = "Reject";
            this.RejectAppPO.Category = "ListView";
            this.RejectAppPO.ConfirmationMessage = null;
            this.RejectAppPO.Id = "RejectAppPO";
            this.RejectAppPO.ToolTip = null;
            this.RejectAppPO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppPO_Execute);
            // 
            // POCopyFromSO
            // 
            this.POCopyFromSO.AcceptButtonCaption = null;
            this.POCopyFromSO.CancelButtonCaption = null;
            this.POCopyFromSO.Caption = "Copy From SO";
            this.POCopyFromSO.Category = "ObjectsCreation";
            this.POCopyFromSO.ConfirmationMessage = null;
            this.POCopyFromSO.Id = "POCopyFromSO";
            this.POCopyFromSO.ToolTip = null;
            this.POCopyFromSO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.POCopyFromSO_CustomizePopupWindowParams);
            this.POCopyFromSO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.POCopyFromSO_Execute);
            // 
            // ApproveAppPO_Pop
            // 
            this.ApproveAppPO_Pop.AcceptButtonCaption = null;
            this.ApproveAppPO_Pop.CancelButtonCaption = null;
            this.ApproveAppPO_Pop.Caption = "Approve";
            this.ApproveAppPO_Pop.Category = "ObjectsCreation";
            this.ApproveAppPO_Pop.ConfirmationMessage = null;
            this.ApproveAppPO_Pop.Id = "ApproveAppPO_Pop";
            this.ApproveAppPO_Pop.ToolTip = null;
            this.ApproveAppPO_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppPO_Pop_CustomizePopupWindowParams);
            this.ApproveAppPO_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppPO_Pop_Execute);
            // 
            // PreviewPONoCost
            // 
            this.PreviewPONoCost.Caption = "Preview (No Cost)";
            this.PreviewPONoCost.Category = "ObjectsCreation";
            this.PreviewPONoCost.ConfirmationMessage = null;
            this.PreviewPONoCost.Id = "PreviewPONoCost";
            this.PreviewPONoCost.ToolTip = null;
            this.PreviewPONoCost.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewPONoCost_Execute);
            // 
            // PurchaseOrderControllers
            // 
            this.Actions.Add(this.SubmitPO);
            this.Actions.Add(this.CancelPO);
            this.Actions.Add(this.PreviewPO);
            this.Actions.Add(this.POInquiryItem);
            this.Actions.Add(this.DuplicatePO);
            this.Actions.Add(this.ReviewAppPO);
            this.Actions.Add(this.ApproveAppPO);
            this.Actions.Add(this.RejectAppPO);
            this.Actions.Add(this.POCopyFromSO);
            this.Actions.Add(this.ApproveAppPO_Pop);
            this.Actions.Add(this.PreviewPONoCost);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitPO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelPO;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewPO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction POInquiryItem;
        private DevExpress.ExpressApp.Actions.SimpleAction DuplicatePO;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppPO;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppPO;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppPO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction POCopyFromSO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppPO_Pop;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewPONoCost;
    }
}
