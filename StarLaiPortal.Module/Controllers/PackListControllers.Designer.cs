namespace StarLaiPortal.Module.Controllers
{
    partial class PackListControllers
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
            this.PACopyFromPL = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SubmitPA = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CancelPA = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PrintBundlePA = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.AddPackList = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PALBundleID = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.DeletePackList = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PrintBundle = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PACopyFromPL
            // 
            this.PACopyFromPL.AcceptButtonCaption = null;
            this.PACopyFromPL.CancelButtonCaption = null;
            this.PACopyFromPL.Caption = "Copy From Pick List";
            this.PACopyFromPL.Category = "ObjectsCreation";
            this.PACopyFromPL.ConfirmationMessage = null;
            this.PACopyFromPL.Id = "PACopyFromPL";
            this.PACopyFromPL.ToolTip = null;
            this.PACopyFromPL.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PACopyFromPL_CustomizePopupWindowParams);
            this.PACopyFromPL.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PACopyFromPL_Execute);
            // 
            // SubmitPA
            // 
            this.SubmitPA.AcceptButtonCaption = null;
            this.SubmitPA.CancelButtonCaption = null;
            this.SubmitPA.Caption = "Complete Pack";
            this.SubmitPA.Category = "ObjectsCreation";
            this.SubmitPA.ConfirmationMessage = null;
            this.SubmitPA.Id = "SubmitPA";
            this.SubmitPA.ToolTip = null;
            this.SubmitPA.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SubmitPA_CustomizePopupWindowParams);
            this.SubmitPA.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SubmitPA_Execute);
            // 
            // CancelPA
            // 
            this.CancelPA.AcceptButtonCaption = null;
            this.CancelPA.CancelButtonCaption = null;
            this.CancelPA.Caption = "Cancel";
            this.CancelPA.Category = "ObjectsCreation";
            this.CancelPA.ConfirmationMessage = null;
            this.CancelPA.Id = "CancelPA";
            this.CancelPA.ToolTip = null;
            this.CancelPA.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CancelPA_CustomizePopupWindowParams);
            this.CancelPA.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CancelPA_Execute);
            // 
            // PrintBundlePA
            // 
            this.PrintBundlePA.AcceptButtonCaption = null;
            this.PrintBundlePA.CancelButtonCaption = null;
            this.PrintBundlePA.Caption = "Print Bundle";
            this.PrintBundlePA.Category = "ObjectsCreation";
            this.PrintBundlePA.ConfirmationMessage = null;
            this.PrintBundlePA.Id = "PrintBundlePA";
            this.PrintBundlePA.ToolTip = null;
            this.PrintBundlePA.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PrintBundlePA_CustomizePopupWindowParams);
            this.PrintBundlePA.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PrintBundlePA_Execute);
            // 
            // AddPackList
            // 
            this.AddPackList.Caption = "Add";
            this.AddPackList.Category = "ObjectsCreation";
            this.AddPackList.ConfirmationMessage = null;
            this.AddPackList.Id = "AddPackList";
            this.AddPackList.ToolTip = null;
            this.AddPackList.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddPackList_Execute);
            // 
            // PALBundleID
            // 
            this.PALBundleID.Caption = "Bundle ID";
            this.PALBundleID.Category = "ObjectsCreation";
            this.PALBundleID.ConfirmationMessage = null;
            this.PALBundleID.Id = "PALBundleID";
            this.PALBundleID.ToolTip = null;
            this.PALBundleID.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.PALBundleID_Execute);
            // 
            // DeletePackList
            // 
            this.DeletePackList.Caption = "Delete";
            this.DeletePackList.Category = "ObjectsCreation";
            this.DeletePackList.ConfirmationMessage = null;
            this.DeletePackList.Id = "DeletePackList";
            this.DeletePackList.ToolTip = null;
            this.DeletePackList.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeletePackList_Execute);
            // 
            // PrintBundle
            // 
            this.PrintBundle.Caption = "Print Bundle";
            this.PrintBundle.Category = "ObjectsCreation";
            this.PrintBundle.ConfirmationMessage = null;
            this.PrintBundle.Id = "PrintBundle";
            this.PrintBundle.ToolTip = null;
            this.PrintBundle.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrintBundle_Execute);
            // 
            // PackListControllers
            // 
            this.Actions.Add(this.PACopyFromPL);
            this.Actions.Add(this.SubmitPA);
            this.Actions.Add(this.CancelPA);
            this.Actions.Add(this.PrintBundlePA);
            this.Actions.Add(this.AddPackList);
            this.Actions.Add(this.PALBundleID);
            this.Actions.Add(this.DeletePackList);
            this.Actions.Add(this.PrintBundle);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PACopyFromPL;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SubmitPA;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelPA;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PrintBundlePA;
        private DevExpress.ExpressApp.Actions.SimpleAction AddPackList;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction PALBundleID;
        private DevExpress.ExpressApp.Actions.SimpleAction DeletePackList;
        private DevExpress.ExpressApp.Actions.SimpleAction PrintBundle;
    }
}
