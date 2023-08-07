namespace StarLaiPortal.Module.Controllers
{
    partial class e
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
            this.BackToInquiry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateSalesOrder = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSalesOrder = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.InquiryItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.DuplicateSQ = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReviewAppSQ = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSQ = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RejectAppSQ = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreviewSQ = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ApproveAppSQ_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // BackToInquiry
            // 
            this.BackToInquiry.Caption = "Back To Inquiry";
            this.BackToInquiry.Category = "ListView";
            this.BackToInquiry.ConfirmationMessage = null;
            this.BackToInquiry.Id = "BackToInquiry";
            this.BackToInquiry.ToolTip = null;
            this.BackToInquiry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BackToInquiry_Execute);
            // 
            // CreateSalesOrder
            // 
            this.CreateSalesOrder.AcceptButtonCaption = null;
            this.CreateSalesOrder.CancelButtonCaption = null;
            this.CreateSalesOrder.Caption = "Submit";
            this.CreateSalesOrder.Category = "ObjectsCreation";
            this.CreateSalesOrder.ConfirmationMessage = null;
            this.CreateSalesOrder.Id = "CreateSalesOrder";
            this.CreateSalesOrder.ToolTip = null;
            this.CreateSalesOrder.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CreateSalesOrder_CustomizePopupWindowParams);
            this.CreateSalesOrder.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CreateSalesOrder_Execute);
            // 
            // CancelSalesOrder
            // 
            this.CancelSalesOrder.AcceptButtonCaption = null;
            this.CancelSalesOrder.CancelButtonCaption = null;
            this.CancelSalesOrder.Caption = "Cancel";
            this.CancelSalesOrder.Category = "ObjectsCreation";
            this.CancelSalesOrder.ConfirmationMessage = null;
            this.CancelSalesOrder.Id = "CancelSalesOrder";
            this.CancelSalesOrder.ToolTip = null;
            this.CancelSalesOrder.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSalesOrder_CustomizePopupWindowParams);
            this.CancelSalesOrder.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSalesOrder_Execute);
            // 
            // InquiryItem
            // 
            this.InquiryItem.AcceptButtonCaption = null;
            this.InquiryItem.CancelButtonCaption = null;
            this.InquiryItem.Caption = "Inquiry";
            this.InquiryItem.Category = "ListView";
            this.InquiryItem.ConfirmationMessage = null;
            this.InquiryItem.Id = "InquiryItem";
            this.InquiryItem.ToolTip = null;
            this.InquiryItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.InquiryItem_CustomizePopupWindowParams);
            this.InquiryItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.InquiryItem_Execute);
            // 
            // DuplicateSQ
            // 
            this.DuplicateSQ.Caption = "Duplicate";
            this.DuplicateSQ.Category = "ObjectsCreation";
            this.DuplicateSQ.ConfirmationMessage = null;
            this.DuplicateSQ.Id = "DuplicateSQ";
            this.DuplicateSQ.ToolTip = null;
            this.DuplicateSQ.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DuplicateSQ_Execute);
            // 
            // ReviewAppSQ
            // 
            this.ReviewAppSQ.Caption = "Review";
            this.ReviewAppSQ.Category = "ListView";
            this.ReviewAppSQ.ConfirmationMessage = null;
            this.ReviewAppSQ.Id = "ReviewAppSQ";
            this.ReviewAppSQ.ToolTip = null;
            this.ReviewAppSQ.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReviewAppSQ_Execute);
            // 
            // ApproveAppSQ
            // 
            this.ApproveAppSQ.Caption = "Approve";
            this.ApproveAppSQ.Category = "ListView";
            this.ApproveAppSQ.ConfirmationMessage = null;
            this.ApproveAppSQ.Id = "ApproveAppSQ";
            this.ApproveAppSQ.ToolTip = null;
            this.ApproveAppSQ.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ApproveAppSQ_Execute);
            // 
            // RejectAppSQ
            // 
            this.RejectAppSQ.Caption = "Reject";
            this.RejectAppSQ.Category = "ListView";
            this.RejectAppSQ.ConfirmationMessage = null;
            this.RejectAppSQ.Id = "RejectAppSQ";
            this.RejectAppSQ.ToolTip = null;
            this.RejectAppSQ.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RejectAppSQ_Execute);
            // 
            // PreviewSQ
            // 
            this.PreviewSQ.Caption = "Preview";
            this.PreviewSQ.Category = "ObjectsCreation";
            this.PreviewSQ.ConfirmationMessage = null;
            this.PreviewSQ.Id = "PreviewSQ";
            this.PreviewSQ.ToolTip = null;
            this.PreviewSQ.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewSQ_Execute);
            // 
            // ApproveAppSQ_Pop
            // 
            this.ApproveAppSQ_Pop.AcceptButtonCaption = null;
            this.ApproveAppSQ_Pop.CancelButtonCaption = null;
            this.ApproveAppSQ_Pop.Caption = "Approve";
            this.ApproveAppSQ_Pop.Category = "ObjectsCreation";
            this.ApproveAppSQ_Pop.ConfirmationMessage = null;
            this.ApproveAppSQ_Pop.Id = "ApproveAppSQ_Pop";
            this.ApproveAppSQ_Pop.ToolTip = null;
            this.ApproveAppSQ_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppSQ_Pop_CustomizePopupWindowParams);
            this.ApproveAppSQ_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppSQ_Pop_Execute);
            // 
            // e
            // 
            this.Actions.Add(this.BackToInquiry);
            this.Actions.Add(this.CreateSalesOrder);
            this.Actions.Add(this.CancelSalesOrder);
            this.Actions.Add(this.InquiryItem);
            this.Actions.Add(this.DuplicateSQ);
            this.Actions.Add(this.ReviewAppSQ);
            this.Actions.Add(this.ApproveAppSQ);
            this.Actions.Add(this.RejectAppSQ);
            this.Actions.Add(this.PreviewSQ);
            this.Actions.Add(this.ApproveAppSQ_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BackToInquiry;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CreateSalesOrder;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSalesOrder;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction InquiryItem;
        private DevExpress.ExpressApp.Actions.SimpleAction DuplicateSQ;
        private DevExpress.ExpressApp.Actions.SimpleAction ReviewAppSQ;
        private DevExpress.ExpressApp.Actions.SimpleAction ApproveAppSQ;
        private DevExpress.ExpressApp.Actions.SimpleAction RejectAppSQ;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewSQ;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppSQ_Pop;
    }
}
