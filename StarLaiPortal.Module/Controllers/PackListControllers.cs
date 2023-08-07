using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.ExpressApp.Web.Templates.ActionContainers.Menu;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PackListControllers : ViewController
    {
        GeneralControllers genCon;
        public PackListControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.PACopyFromPL.Active.SetItemValue("Enabled", false);
            this.SubmitPA.Active.SetItemValue("Enabled", false);
            this.CancelPA.Active.SetItemValue("Enabled", false);
            this.PrintBundlePA.Active.SetItemValue("Enabled", false);
            this.AddPackList.Active.SetItemValue("Enabled", false);
            this.PALBundleID.Active.SetItemValue("Enabled", false);
            this.DeletePackList.Active.SetItemValue("Enabled", false);
            this.PrintBundle.Active.SetItemValue("Enabled", false);

            if (typeof(vwPickList).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(vwPickList))
                {
                    this.AddPackList.Active.SetItemValue("Enabled", true);

                    PALBundleID.Items.Clear();

                    foreach (BundleType bundle in View.ObjectSpace.CreateCollection(typeof(BundleType), null))
                    {
                        PALBundleID.Items.Add(new ChoiceActionItem(bundle.BundleName, bundle.BundleName));
                    }
                    this.PALBundleID.Active.SetItemValue("Enabled", true);
                    PALBundleID.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
                    PALBundleID.CustomizeControl += action_CustomizeControl;
                }
            }

            if (typeof(AddPickListDetails).IsAssignableFrom(View.ObjectTypeInfo.Type))
            {
                if (View.ObjectTypeInfo.Type == typeof(AddPickListDetails))
                {
                    this.DeletePackList.Active.SetItemValue("Enabled", true);
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "PackList_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.PACopyFromPL.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.PACopyFromPL.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitPA.Active.SetItemValue("Enabled", true);
                    this.CancelPA.Active.SetItemValue("Enabled", true);
                    //this.PrintBundlePA.Active.SetItemValue("Enabled", true);
                    this.PrintBundle.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitPA.Active.SetItemValue("Enabled", false);
                    this.CancelPA.Active.SetItemValue("Enabled", false);
                    this.PrintBundlePA.Active.SetItemValue("Enabled", false);
                    this.PrintBundle.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.PACopyFromPL.Active.SetItemValue("Enabled", false);
                this.SubmitPA.Active.SetItemValue("Enabled", false);
                this.CancelPA.Active.SetItemValue("Enabled", false);
                this.PrintBundlePA.Active.SetItemValue("Enabled", false);
                this.PrintBundle.Active.SetItemValue("Enabled", false);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void action_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            SingleChoiceActionAsModeMenuActionItem actionItem = e.Control as SingleChoiceActionAsModeMenuActionItem;
            if (actionItem != null && actionItem.Action.PaintStyle == DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption)
            {
                DropDownSingleChoiceActionControlBase control = (DropDownSingleChoiceActionControlBase)actionItem.Control;
                control.Label.Text = actionItem.Action.Caption;
                control.Label.Style["padding-right"] = "5px";
            }
        }

        public void openNewView(IObjectSpace os, object target, ViewEditMode viewmode)
        {
            ShowViewParameters svp = new ShowViewParameters();
            DetailView dv = Application.CreateDetailView(os, target);
            dv.ViewEditMode = viewmode;
            dv.IsRoot = true;
            svp.CreatedView = dv;

            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

        }
        public void showMsg(string caption, string msg, InformationType msgtype)
        {
            MessageOptions options = new MessageOptions();
            options.Duration = 3000;
            //options.Message = string.Format("{0} task(s) have been successfully updated!", e.SelectedObjects.Count);
            options.Message = string.Format("{0}", msg);
            options.Type = msgtype;
            options.Web.Position = InformationPosition.Right;
            options.Win.Caption = caption;
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        private void PACopyFromPL_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PackList packlist = (PackList)View.CurrentObject;
            AddPickList addnew = (AddPickList)e.PopupWindow.View.CurrentObject;

            foreach (AddPickListDetails dtl in addnew.AddPickListDetails)
            {
                if (packlist.DocNum == null)
                {
                    string docprefix = genCon.GetDocPrefix();
                    packlist.DocNum = genCon.GenerateDocNum(DocTypeList.PAL, ObjectSpace, TransferType.NA, 0, docprefix);
                }

                PackListDetails details = ObjectSpace.CreateObject<PackListDetails>();
                details.ItemCode = details.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode);
                details.ItemDesc = dtl.ItemDesc;
                details.CatalogNo = dtl.CatalogNo;
                if (dtl.Bundle != null)
                {
                    details.Bundle = details.Session.FindObject<BundleType>
                        (CriteriaOperator.Parse("Oid = ?", dtl.Bundle.Oid));
                }
                details.PickListNo = dtl.PickListNo;
                if (dtl.Transporter != null)
                {
                    details.Transporter = details.Session.FindObject<vwTransporter>
                        (CriteriaOperator.Parse("TransporterID = ?", dtl.Transporter.TransporterID));
                }
                details.Quantity = dtl.Quantity;
                details.BaseDoc = dtl.BaseDoc;
                details.BaseId = dtl.BaseId;

                PickList picklist = ObjectSpace.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtl.PickListNo));

                foreach (PickListDetailsActual dtlpickactual in picklist.PickListDetailsActual)
                {
                    if (dtlpickactual.Oid.ToString() == dtl.BaseId)
                    {
                        foreach (PickListDetails dtlpick in picklist.PickListDetails)
                        {
                            if (dtlpick.Oid == dtlpickactual.PickListDetailOid)
                            {
                                details.Customer = details.Session.GetObjectByKey<vwBusniessPartner>(dtlpick.Customer.BPCode);
                            }
                        }
                    }
                }

                packlist.CustomerGroup = picklist.CustomerGroup;

                packlist.PackListDetails.Add(details);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void PACopyFromPL_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            PackList pal = (PackList)View.CurrentObject;

            if (pal.DocNum == null)
            {
                string docprefix = genCon.GetDocPrefix();
                pal.DocNum = genCon.GenerateDocNum(DocTypeList.PAL, ObjectSpace, TransferType.NA, 0, docprefix);
            }

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            AddPickList addpicklist = os.CreateObject<AddPickList>();

            DetailView dv = Application.CreateDetailView(os, addpicklist, true);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((AddPickList)dv.CurrentObject).ToBin = ((AddPickList)dv.CurrentObject).Session.GetObjectByKey<vwBin>(pal.PackingLocation.BinCode);
            ((AddPickList)dv.CurrentObject).Bundle = null;

            ((AddPickList)dv.CurrentObject).DocNum = pal.DocNum;
            os.CommitChanges();

            e.DialogController.CancelAction.Active["NothingToCancel"] = false;
            e.View = dv;
        }

        private void SubmitPA_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PackList selectedObject = (PackList)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                PackListDocTrail ds = ObjectSpace.CreateObject<PackListDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.PackListDocTrail.Add(ds);

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                IObjectSpace os = Application.CreateObjectSpace();
                PackList trx = os.FindObject<PackList>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitPA_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelPA_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            PackList selectedObject = (PackList)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            PackListDocTrail ds = ObjectSpace.CreateObject<PackListDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.PackListDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            PackList trx = os.FindObject<PackList>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelPA_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void PrintBundlePA_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }

        private void PrintBundlePA_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {

        }

        private void AddPackList_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count > 0)
            {
                try
                {
                    foreach (vwPickList dtl in e.SelectedObjects)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        AddPickList newpal = os.FindObject<AddPickList>(new BinaryOperator("Oid", dtl.AddPickList.Oid));

                        AddPickListDetails newpalitem = os.CreateObject<AddPickListDetails>();

                        newpalitem.ItemCode = dtl.ItemCode;
                        newpalitem.ItemDesc = dtl.ItemDesc;
                        newpalitem.CatalogNo = dtl.CatalogNo;
                      
                        newpalitem.Bundle = newpalitem.Session.FindObject<BundleType>
                            (CriteriaOperator.Parse("BundleName = ?", PALBundleID.SelectedItem.Id));
                        newpalitem.PickListNo = dtl.DocNum;
                        if (dtl.SOTransporter != null)
                        {
                            newpalitem.Transporter = newpalitem.Session.FindObject<vwTransporter>
                                (CriteriaOperator.Parse("TransporterName = ?", dtl.SOTransporter));
                        }
                        newpalitem.Quantity = dtl.PickQty;
                        newpalitem.BaseDoc = dtl.DocNum;
                        newpalitem.BaseId = dtl.Oid;

                        newpal.AddPickListDetails.Add(newpalitem);

                        os.CommitChanges();
                        os.Refresh();
                    }

                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();

                    showMsg("Success", "Add Success.", InformationType.Success);
                }
                catch (Exception)
                {
                    showMsg("Fail", "Copy Fail.", InformationType.Error);
                }
            }
        }

        private void PALBundleID_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

        }

        private void FilterBin_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {

        }

        private void DeletePackList_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count > 0)
            {
                try
                {
                    foreach (AddPickListDetails dtl in e.SelectedObjects)
                    {
                        dtl.AddPickList = null;
                    }
                    
                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();

                    showMsg("Success", "Delete Success.", InformationType.Success);
                }
                catch (Exception)
                {
                    showMsg("Fail", "Delete Fail.", InformationType.Error);
                }
            }
        }

        private void PrintBundle_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            PackList pal = (PackList)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\Bundle.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("dockey@", pal.Oid);
                doc.SetParameterValue("dbName@", conn.Database);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + pal.Oid + "_" + user.UserName + "_PAL_"
                    + DateTime.Parse(pal.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + pal.Oid + "_" + user.UserName + "_PAL_"
                    + DateTime.Parse(pal.DocDate.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }
    }
}
