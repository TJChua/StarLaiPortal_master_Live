namespace StarLaiPortal.Module.Controllers
{
    partial class ARDCancellationControllers
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
            this.CopyFromDownpayment = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitDPCancel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelDPCancel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ApproveAppARDPC_Pop = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // CopyFromDownpayment
            // 
            this.CopyFromDownpayment.AcceptButtonCaption = null;
            this.CopyFromDownpayment.CancelButtonCaption = null;
            this.CopyFromDownpayment.Caption = "Copy From Downpayment";
            this.CopyFromDownpayment.Category = "ObjectsCreation";
            this.CopyFromDownpayment.ConfirmationMessage = null;
            this.CopyFromDownpayment.Id = "CopyFromDownpayment";
            this.CopyFromDownpayment.ToolTip = null;
            this.CopyFromDownpayment.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CopyFromDownpayment_CustomizePopupWindowParams);
            this.CopyFromDownpayment.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CopyFromDownpayment_Execute);
            // 
            // SubmitDPCancel
            // 
            this.SubmitDPCancel.AcceptButtonCaption = null;
            this.SubmitDPCancel.CancelButtonCaption = null;
            this.SubmitDPCancel.Caption = "Submit";
            this.SubmitDPCancel.Category = "ObjectsCreation";
            this.SubmitDPCancel.ConfirmationMessage = null;
            this.SubmitDPCancel.Id = "SubmitDPCancel";
            this.SubmitDPCancel.ToolTip = null;
            this.SubmitDPCancel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitDPCancel_CustomizePopupWindowParams);
            this.SubmitDPCancel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitDPCancel_Execute);
            // 
            // CancelDPCancel
            // 
            this.CancelDPCancel.AcceptButtonCaption = null;
            this.CancelDPCancel.CancelButtonCaption = null;
            this.CancelDPCancel.Caption = "Cancel";
            this.CancelDPCancel.Category = "ObjectsCreation";
            this.CancelDPCancel.ConfirmationMessage = null;
            this.CancelDPCancel.Id = "CancelDPCancel";
            this.CancelDPCancel.ToolTip = null;
            this.CancelDPCancel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelDPCancel_CustomizePopupWindowParams);
            this.CancelDPCancel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelDPCancel_Execute);
            // 
            // ApproveAppARDPC_Pop
            // 
            this.ApproveAppARDPC_Pop.AcceptButtonCaption = null;
            this.ApproveAppARDPC_Pop.CancelButtonCaption = null;
            this.ApproveAppARDPC_Pop.Caption = "Approve";
            this.ApproveAppARDPC_Pop.Category = "ObjectsCreation";
            this.ApproveAppARDPC_Pop.ConfirmationMessage = null;
            this.ApproveAppARDPC_Pop.Id = "ApproveAppARDPC_Pop";
            this.ApproveAppARDPC_Pop.ToolTip = null;
            this.ApproveAppARDPC_Pop.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ApproveAppARDPC_Pop_CustomizePopupWindowParams);
            this.ApproveAppARDPC_Pop.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ApproveAppARDPC_Pop_Execute);
            // 
            // ARDCancellationControllers
            // 
            this.Actions.Add(this.CopyFromDownpayment);
            this.Actions.Add(this.SubmitDPCancel);
            this.Actions.Add(this.CancelDPCancel);
            this.Actions.Add(this.ApproveAppARDPC_Pop);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CopyFromDownpayment;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitDPCancel;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelDPCancel;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ApproveAppARDPC_Pop;
    }
}
