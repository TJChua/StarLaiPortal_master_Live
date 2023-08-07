namespace StarLaiPortal.Module.Controllers
{
    partial class GRNControllers
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
            this.GRNCopyFromPO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.GRNCopyFromASN = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitGRN = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelGRN = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewGRN = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GRNCopyFromPO
            // 
            this.GRNCopyFromPO.AcceptButtonCaption = null;
            this.GRNCopyFromPO.CancelButtonCaption = null;
            this.GRNCopyFromPO.Caption = "Copy From PO";
            this.GRNCopyFromPO.Category = "ObjectsCreation";
            this.GRNCopyFromPO.ConfirmationMessage = null;
            this.GRNCopyFromPO.Id = "GRNCopyFromPO";
            this.GRNCopyFromPO.ToolTip = null;
            this.GRNCopyFromPO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.GRNCopyFromPO_CustomizePopupWindowParams);
            this.GRNCopyFromPO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.GRNCopyFromPO_Execute);
            // 
            // GRNCopyFromASN
            // 
            this.GRNCopyFromASN.AcceptButtonCaption = null;
            this.GRNCopyFromASN.CancelButtonCaption = null;
            this.GRNCopyFromASN.Caption = "Copy From ASN";
            this.GRNCopyFromASN.Category = "ObjectsCreation";
            this.GRNCopyFromASN.ConfirmationMessage = null;
            this.GRNCopyFromASN.Id = "GRNCopyFromASN";
            this.GRNCopyFromASN.ToolTip = null;
            this.GRNCopyFromASN.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.GRNCopyFromASN_CustomizePopupWindowParams);
            this.GRNCopyFromASN.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.GRNCopyFromASN_Execute);
            // 
            // SubmitGRN
            // 
            this.SubmitGRN.AcceptButtonCaption = null;
            this.SubmitGRN.CancelButtonCaption = null;
            this.SubmitGRN.Caption = "Submit";
            this.SubmitGRN.Category = "ObjectsCreation";
            this.SubmitGRN.ConfirmationMessage = null;
            this.SubmitGRN.Id = "SubmitGRN";
            this.SubmitGRN.ToolTip = null;
            this.SubmitGRN.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitGRN_CustomizePopupWindowParams);
            this.SubmitGRN.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitGRN_Execute);
            // 
            // CancelGRN
            // 
            this.CancelGRN.AcceptButtonCaption = null;
            this.CancelGRN.CancelButtonCaption = null;
            this.CancelGRN.Caption = "Cancel";
            this.CancelGRN.Category = "ObjectsCreation";
            this.CancelGRN.ConfirmationMessage = null;
            this.CancelGRN.Id = "CancelGRN";
            this.CancelGRN.ToolTip = null;
            this.CancelGRN.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelGRN_CustomizePopupWindowParams);
            this.CancelGRN.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelGRN_Execute);
            // 
            // PreviewGRN
            // 
            this.PreviewGRN.Caption = "Preview";
            this.PreviewGRN.Category = "ObjectsCreation";
            this.PreviewGRN.ConfirmationMessage = null;
            this.PreviewGRN.Id = "PreviewGRN";
            this.PreviewGRN.ToolTip = null;
            this.PreviewGRN.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewGRN_Execute);
            // 
            // GRNControllers
            // 
            this.Actions.Add(this.GRNCopyFromPO);
            this.Actions.Add(this.GRNCopyFromASN);
            this.Actions.Add(this.SubmitGRN);
            this.Actions.Add(this.CancelGRN);
            this.Actions.Add(this.PreviewGRN);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction GRNCopyFromPO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction GRNCopyFromASN;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitGRN;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelGRN;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewGRN;
    }
}
