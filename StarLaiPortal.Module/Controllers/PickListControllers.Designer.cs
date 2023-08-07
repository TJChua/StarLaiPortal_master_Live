namespace StarLaiPortal.Module.Controllers
{
    partial class PickListControllers
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
            this.PLCopyFromSOC = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitPL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelPL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewPL = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLCopyFromSOCG = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PLCopyFromPLDetail = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PrintPL = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PLCopyFromSOC
            // 
            this.PLCopyFromSOC.AcceptButtonCaption = null;
            this.PLCopyFromSOC.CancelButtonCaption = null;
            this.PLCopyFromSOC.Caption = "Copy From SO Item";
            this.PLCopyFromSOC.Category = "ObjectsCreation";
            this.PLCopyFromSOC.ConfirmationMessage = null;
            this.PLCopyFromSOC.Id = "PLCopyFromSOC";
            this.PLCopyFromSOC.ToolTip = null;
            this.PLCopyFromSOC.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PLCopyFromSOC_CustomizePopupWindowParams);
            this.PLCopyFromSOC.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PLCopyFromSOC_Execute);
            // 
            // SubmitPL
            // 
            this.SubmitPL.AcceptButtonCaption = null;
            this.SubmitPL.CancelButtonCaption = null;
            this.SubmitPL.Caption = "Submit";
            this.SubmitPL.Category = "ObjectsCreation";
            this.SubmitPL.ConfirmationMessage = null;
            this.SubmitPL.Id = "SubmitPL";
            this.SubmitPL.ToolTip = null;
            this.SubmitPL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitPL_CustomizePopupWindowParams);
            this.SubmitPL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitPL_Execute);
            // 
            // CancelPL
            // 
            this.CancelPL.AcceptButtonCaption = null;
            this.CancelPL.CancelButtonCaption = null;
            this.CancelPL.Caption = "Cancel";
            this.CancelPL.Category = "ObjectsCreation";
            this.CancelPL.ConfirmationMessage = null;
            this.CancelPL.Id = "CancelPL";
            this.CancelPL.ToolTip = null;
            this.CancelPL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelPL_CustomizePopupWindowParams);
            this.CancelPL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelPL_Execute);
            // 
            // PreviewPL
            // 
            this.PreviewPL.Caption = "Preview";
            this.PreviewPL.Category = "ObjectsCreation";
            this.PreviewPL.ConfirmationMessage = null;
            this.PreviewPL.Id = "PreviewPL";
            this.PreviewPL.ToolTip = null;
            this.PreviewPL.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewPL_Execute);
            // 
            // PLCopyFromSOCG
            // 
            this.PLCopyFromSOCG.AcceptButtonCaption = null;
            this.PLCopyFromSOCG.CancelButtonCaption = null;
            this.PLCopyFromSOCG.Caption = "Copy From SO";
            this.PLCopyFromSOCG.Category = "ObjectsCreation";
            this.PLCopyFromSOCG.ConfirmationMessage = null;
            this.PLCopyFromSOCG.Id = "PLCopyFromSOCG";
            this.PLCopyFromSOCG.ToolTip = null;
            this.PLCopyFromSOCG.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PLCopyFromSOCG_CustomizePopupWindowParams);
            this.PLCopyFromSOCG.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PLCopyFromSOCG_Execute);
            // 
            // PLCopyFromPLDetail
            // 
            this.PLCopyFromPLDetail.AcceptButtonCaption = null;
            this.PLCopyFromPLDetail.CancelButtonCaption = null;
            this.PLCopyFromPLDetail.Caption = "Copy From Plan";
            this.PLCopyFromPLDetail.Category = "ListView";
            this.PLCopyFromPLDetail.ConfirmationMessage = null;
            this.PLCopyFromPLDetail.Id = "PLCopyFromPLDetail";
            this.PLCopyFromPLDetail.ToolTip = null;
            this.PLCopyFromPLDetail.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PLCopyFromPLDetail_CustomizePopupWindowParams);
            this.PLCopyFromPLDetail.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PLCopyFromPLDetail_Execute);
            // 
            // PrintPL
            // 
            this.PrintPL.Caption = "Print";
            this.PrintPL.Category = "ObjectsCreation";
            this.PrintPL.ConfirmationMessage = null;
            this.PrintPL.Id = "PrintPL";
            this.PrintPL.ToolTip = null;
            this.PrintPL.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintPL_Execute);
            // 
            // PickListControllers
            // 
            this.Actions.Add(this.PLCopyFromSOC);
            this.Actions.Add(this.SubmitPL);
            this.Actions.Add(this.CancelPL);
            this.Actions.Add(this.PreviewPL);
            this.Actions.Add(this.PLCopyFromSOCG);
            this.Actions.Add(this.PLCopyFromPLDetail);
            this.Actions.Add(this.PrintPL);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PLCopyFromSOC;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitPL;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelPL;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewPL;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PLCopyFromSOCG;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PLCopyFromPLDetail;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintPL;
    }
}
