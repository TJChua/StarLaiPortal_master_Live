namespace StarLaiPortal.Module.Controllers
{
    partial class SalesOrderCollectionController
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
            this.SOCCopyFromSO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitSOC = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelSOC = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PrintARDownpayment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SOCCopyFromSO
            // 
            this.SOCCopyFromSO.AcceptButtonCaption = null;
            this.SOCCopyFromSO.CancelButtonCaption = null;
            this.SOCCopyFromSO.Caption = "Copy From SO";
            this.SOCCopyFromSO.Category = "ObjectsCreation";
            this.SOCCopyFromSO.ConfirmationMessage = null;
            this.SOCCopyFromSO.Id = "SOCCopyFromSO";
            this.SOCCopyFromSO.ToolTip = null;
            this.SOCCopyFromSO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SOCCopyFromSO_CustomizePopupWindowParams);
            this.SOCCopyFromSO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SOCCopyFromSO_Execute);
            // 
            // SubmitSOC
            // 
            this.SubmitSOC.AcceptButtonCaption = null;
            this.SubmitSOC.CancelButtonCaption = null;
            this.SubmitSOC.Caption = "Submit";
            this.SubmitSOC.Category = "ObjectsCreation";
            this.SubmitSOC.ConfirmationMessage = null;
            this.SubmitSOC.Id = "SubmitSOC";
            this.SubmitSOC.ToolTip = null;
            this.SubmitSOC.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitSOC_CustomizePopupWindowParams);
            this.SubmitSOC.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitSOC_Execute);
            // 
            // CancelSOC
            // 
            this.CancelSOC.AcceptButtonCaption = null;
            this.CancelSOC.CancelButtonCaption = null;
            this.CancelSOC.Caption = "Cancel";
            this.CancelSOC.Category = "ObjectsCreation";
            this.CancelSOC.ConfirmationMessage = null;
            this.CancelSOC.Id = "CancelSOC";
            this.CancelSOC.ToolTip = null;
            this.CancelSOC.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelSOC_CustomizePopupWindowParams);
            this.CancelSOC.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelSOC_Execute);
            // 
            // PrintARDownpayment
            // 
            this.PrintARDownpayment.Caption = "Print";
            this.PrintARDownpayment.Category = "ObjectsCreation";
            this.PrintARDownpayment.ConfirmationMessage = null;
            this.PrintARDownpayment.Id = "PrintARDownpayment";
            this.PrintARDownpayment.ToolTip = null;
            this.PrintARDownpayment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintARDownpayment_Execute);
            // 
            // SalesOrderCollectionController
            // 
            this.Actions.Add(this.SOCCopyFromSO);
            this.Actions.Add(this.SubmitSOC);
            this.Actions.Add(this.CancelSOC);
            this.Actions.Add(this.PrintARDownpayment);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SOCCopyFromSO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitSOC;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelSOC;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintARDownpayment;
    }
}
