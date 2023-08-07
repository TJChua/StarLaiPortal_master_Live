namespace StarLaiPortal.Module.Controllers
{
    partial class DeliveryOrderControllers
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
            this.DOCopyFromLoading = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitDO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelDO = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewDO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreviewInv = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreviewBundleDO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PrintDO = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DOCopyFromLoading
            // 
            this.DOCopyFromLoading.AcceptButtonCaption = null;
            this.DOCopyFromLoading.CancelButtonCaption = null;
            this.DOCopyFromLoading.Caption = "Copy From Loading";
            this.DOCopyFromLoading.Category = "ObjectsCreation";
            this.DOCopyFromLoading.ConfirmationMessage = null;
            this.DOCopyFromLoading.Id = "DOCopyFromLoading";
            this.DOCopyFromLoading.ToolTip = null;
            this.DOCopyFromLoading.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.DOCopyFromLoading_CustomizePopupWindowParams);
            this.DOCopyFromLoading.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.DOCopyFromLoading_Execute);
            // 
            // SubmitDO
            // 
            this.SubmitDO.AcceptButtonCaption = null;
            this.SubmitDO.CancelButtonCaption = null;
            this.SubmitDO.Caption = "Submit";
            this.SubmitDO.Category = "ObjectsCreation";
            this.SubmitDO.ConfirmationMessage = null;
            this.SubmitDO.Id = "SubmitDO";
            this.SubmitDO.ToolTip = null;
            this.SubmitDO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitDO_CustomizePopupWindowParams);
            this.SubmitDO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitDO_Execute);
            // 
            // CancelDO
            // 
            this.CancelDO.AcceptButtonCaption = null;
            this.CancelDO.CancelButtonCaption = null;
            this.CancelDO.Caption = "Cancel";
            this.CancelDO.Category = "ObjectsCreation";
            this.CancelDO.ConfirmationMessage = null;
            this.CancelDO.Id = "CancelDO";
            this.CancelDO.ToolTip = null;
            this.CancelDO.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelDO_CustomizePopupWindowParams);
            this.CancelDO.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelDO_Execute);
            // 
            // PreviewDO
            // 
            this.PreviewDO.Caption = "Preview DO";
            this.PreviewDO.Category = "ObjectsCreation";
            this.PreviewDO.ConfirmationMessage = null;
            this.PreviewDO.Id = "PreviewDO";
            this.PreviewDO.ToolTip = null;
            this.PreviewDO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewDO_Execute);
            // 
            // PreviewInv
            // 
            this.PreviewInv.Caption = "Preview Invoice";
            this.PreviewInv.Category = "ObjectsCreation";
            this.PreviewInv.ConfirmationMessage = null;
            this.PreviewInv.Id = "PreviewInv";
            this.PreviewInv.ToolTip = null;
            this.PreviewInv.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewInv_Execute);
            // 
            // PreviewBundleDO
            // 
            this.PreviewBundleDO.Caption = "Preview Bundle DO";
            this.PreviewBundleDO.Category = "ObjectsCreation";
            this.PreviewBundleDO.ConfirmationMessage = null;
            this.PreviewBundleDO.Id = "PreviewBundleDO";
            this.PreviewBundleDO.ToolTip = null;
            this.PreviewBundleDO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewBundleDO_Execute);
            // 
            // PrintDO
            // 
            this.PrintDO.Caption = "Print DO";
            this.PrintDO.Category = "ObjectsCreation";
            this.PrintDO.ConfirmationMessage = null;
            this.PrintDO.Id = "PrintDO";
            this.PrintDO.ToolTip = null;
            this.PrintDO.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintDO_Execute);
            // 
            // DeliveryOrderControllers
            // 
            this.Actions.Add(this.DOCopyFromLoading);
            this.Actions.Add(this.SubmitDO);
            this.Actions.Add(this.CancelDO);
            this.Actions.Add(this.PreviewDO);
            this.Actions.Add(this.PreviewInv);
            this.Actions.Add(this.PreviewBundleDO);
            this.Actions.Add(this.PrintDO);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction DOCopyFromLoading;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitDO;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelDO;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewDO;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewInv;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewBundleDO;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintDO;
    }
}
