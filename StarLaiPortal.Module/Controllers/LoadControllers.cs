using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.Load;
using StarLaiPortal.Module.BusinessObjects.Pack_List;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class LoadControllers : ViewController
    {
        GeneralControllers genCon;
        public LoadControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.LCopyFromPAL.Active.SetItemValue("Enabled", false);
            this.SubmitL.Active.SetItemValue("Enabled", false);
            this.CancelL.Active.SetItemValue("Enabled", false);
            this.LGenerateDO.Active.SetItemValue("Enabled", false);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "Load_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.LCopyFromPAL.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.LCopyFromPAL.Active.SetItemValue("Enabled", false);
                }

                if (((DetailView)View).ViewEditMode == ViewEditMode.View)
                {
                    this.SubmitL.Active.SetItemValue("Enabled", true);
                    this.CancelL.Active.SetItemValue("Enabled", true);
                    //this.LGenerateDO.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.SubmitL.Active.SetItemValue("Enabled", false);
                    this.CancelL.Active.SetItemValue("Enabled", false);
                    this.LGenerateDO.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.LCopyFromPAL.Active.SetItemValue("Enabled", false);
                this.SubmitL.Active.SetItemValue("Enabled", false);
                this.CancelL.Active.SetItemValue("Enabled", false);
                this.LGenerateDO.Active.SetItemValue("Enabled", false);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
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

        private void LCopyFromPAL_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (e.PopupWindowViewSelectedObjects.Count > 0)
            {
                try
                {
                    Load load = (Load)View.CurrentObject;

                    //if (load.IsNew == true)
                    //{
                    //    IObjectSpace os = Application.CreateObjectSpace();
                    //    Load newload = os.CreateObject<Load>();

                    //    foreach (vwPackList dtl in e.PopupWindowViewSelectedObjects)
                    //    {
                    //        LoadDetails newloaditem = os.CreateObject<LoadDetails>();

                    //        newloaditem.PackList = dtl.DocNum;
                    //        if (dtl.Bundle != null)
                    //        {
                    //            newloaditem.Bundle = newloaditem.Session.GetObjectByKey<BundleType>(dtl.Bundle.Oid);
                    //        }
                    //        newloaditem.BaseDoc = dtl.DocNum;
                    //        newload.LoadDetails.Add(newloaditem);
                    //    }

                    //    ShowViewParameters svp = new ShowViewParameters();
                    //    DetailView dv = Application.CreateDetailView(os, newload);
                    //    dv.ViewEditMode = ViewEditMode.Edit;
                    //    dv.IsRoot = true;
                    //    svp.CreatedView = dv;

                    //    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    //    showMsg("Success", "Copy Success.", InformationType.Success);
                    //}
                    //else
                    //{
                        foreach (vwPackList dtl in e.PopupWindowViewSelectedObjects)
                        {
                            LoadDetails newloaditem = ObjectSpace.CreateObject<LoadDetails>();

                            newloaditem.PackList = dtl.DocNum;
                            if (dtl.Bundle != null)
                            {
                                newloaditem.Bundle = newloaditem.Session.GetObjectByKey<BundleType>(dtl.Bundle.Oid);
                            }
                            newloaditem.BaseDoc = dtl.DocNum;
                            load.LoadDetails.Add(newloaditem);

                            if (load.DocNum == null)
                            {
                               string docprefix = genCon.GetDocPrefix();
                               load.DocNum = genCon.GenerateDocNum(DocTypeList.Load, ObjectSpace, TransferType.NA, 0, docprefix);
                            }

                            showMsg("Success", "Copy Success.", InformationType.Success);
                        }

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    //}
                }
                catch (Exception)
                {
                    showMsg("Fail", "Copy Fail.", InformationType.Error);
                }
            }
        }

        private void LCopyFromPAL_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            Load load = (Load)View.CurrentObject;

            var os = Application.CreateObjectSpace();
            var viewId = Application.FindListViewId(typeof(vwPackList));
            var cs = Application.CreateCollectionSource(os, typeof(vwPackList), viewId);
            var lv1 = Application.CreateListView(viewId, cs, true);
            e.View = lv1;
        }

        private void SubmitL_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Load selectedObject = (Load)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;
            SqlConnection conn = new SqlConnection(genCon.getConnectionString());

            if (selectedObject.IsValid == true)
            {
                selectedObject.Status = DocStatus.Submitted;
                LoadDocTrail ds = ObjectSpace.CreateObject<LoadDocTrail>();
                ds.DocStatus = DocStatus.Submitted;
                ds.DocRemarks = p.ParamString;
                selectedObject.LoadDocTrail.Add(ds);

                //Create DO
                string getpack = "EXEC GenerateDO '" + selectedObject.DocNum + "'";
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(getpack, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SalesOrder so = ObjectSpace.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", reader.GetString(0)));

                    if (so != null)
                    {
                        string picklistnum = null;
                        IObjectSpace deiveryos = Application.CreateObjectSpace();
                        DeliveryOrder newdelivery = deiveryos.CreateObject<DeliveryOrder>();

                        string docprefix = genCon.GetDocPrefix();
                        newdelivery.DocNum = genCon.GenerateDocNum(DocTypeList.DO, deiveryos, TransferType.NA, 0, docprefix);
                        newdelivery.Customer = newdelivery.Session.GetObjectByKey<vwBusniessPartner>(so.Customer.BPCode);
                        newdelivery.CustomerName = so.CustomerName;
                        newdelivery.Status = DocStatus.Submitted;

                        foreach (LoadDetails dtlload in selectedObject.LoadDetails)
                        {
                            PackList pl = deiveryos.FindObject<PackList>(CriteriaOperator.Parse("DocNum = ?", dtlload.PackList));

                            //newdelivery.CustomerGroup = pl.CustomerGroup;
                            //foreach (PackListDetails dtlpack in pl.PackListDetails)
                            //{
                            //    if (dtlpack.Bundle.BundleID == dtlload.Bundle.BundleID)
                            //    {
                            //        DeliveryOrderDetails newdeliveryitem = deiveryos.CreateObject<DeliveryOrderDetails>();

                            //        newdeliveryitem.ItemCode = newdeliveryitem.Session.GetObjectByKey<vwItemMasters>(dtlpack.ItemCode.ItemCode);
                            //        //if (dtlload.Bin != null)
                            //        //{
                            //        //    newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlload.Bin.Warehouse);
                            //        //    newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlload.Bin.BinCode);
                            //        //}

                            //        PickList pick = ObjectSpace.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtlpack.PickListNo));

                            //        foreach (PickListDetailsActual dtlpick in pick.PickListDetailsActual)
                            //        {
                            //            if (dtlpick.FromBin != null)
                            //            {
                            //                newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlpick.FromBin.Warehouse);
                            //                newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlpick.FromBin.BinCode);
                            //            }
                            //        }

                            //        newdeliveryitem.Quantity = dtlpack.Quantity;

                            //        foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                            //        {
                            //            if (dtlsales.ItemCode.ItemCode == dtlpack.ItemCode.ItemCode)
                            //            {
                            //                newdeliveryitem.Price = dtlsales.Price;
                            //            }
                            //        }
                            //        newdeliveryitem.BaseDoc = selectedObject.DocNum.ToString();
                            //        newdeliveryitem.BaseId = dtlload.Oid.ToString();
                            //        newdeliveryitem.SODocNum = reader.GetString(0);
                            //        newdeliveryitem.PickListDocNum = dtlpack.PickListNo;
                            //        newdeliveryitem.PackListLine = dtlpack.Oid.ToString();

                            //        newdelivery.DeliveryOrderDetails.Add(newdeliveryitem);
                            //    }
                            //}

                            newdelivery.CustomerGroup = pl.CustomerGroup;
                            foreach (PackListDetails dtlpack in pl.PackListDetails)
                            {
                                if (picklistnum != dtlpack.PickListNo)
                                {
                                    PickList picklist = ObjectSpace.FindObject<PickList>(CriteriaOperator.Parse("DocNum = ?", dtlpack.PickListNo));

                                    foreach (PickListDetails dtlpick in picklist.PickListDetails)
                                    {
                                        if (dtlpick.SOBaseDoc == so.DocNum)
                                        {
                                            if (dtlpick.PickQty > 0)
                                            {
                                                DeliveryOrderDetails newdeliveryitem = deiveryos.CreateObject<DeliveryOrderDetails>();

                                                newdeliveryitem.ItemCode = newdeliveryitem.Session.GetObjectByKey<vwItemMasters>(dtlpick.ItemCode.ItemCode);
                                                newdeliveryitem.Quantity = dtlpick.PickQty;
                                                newdeliveryitem.PackListLine = dtlpick.Oid.ToString();

                                                //foreach (PickListDetailsActual dtlpickactual in picklist.PickListDetailsActual)
                                                //{
                                                //    if (dtlpickactual.FromBin != null && dtlpickactual.ItemCode.ItemCode == dtlpack.ItemCode.ItemCode)
                                                //    {
                                                //        newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlpickactual.FromBin.Warehouse);
                                                //        newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlpickactual.FromBin.BinCode);
                                                //    }
                                                //}

                                                //temporary use picklist from bin
                                                if (dtlload.Bin != null)
                                                {
                                                    newdeliveryitem.Warehouse = newdeliveryitem.Session.GetObjectByKey<vwWarehouse>(dtlload.Bin.Warehouse);
                                                    newdeliveryitem.Bin = newdeliveryitem.Session.GetObjectByKey<vwBin>(dtlload.Bin.BinCode);
                                                }

                                                foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                                                {
                                                    if (dtlsales.ItemCode.ItemCode == dtlpick.ItemCode.ItemCode)
                                                    {
                                                        newdeliveryitem.Price = dtlsales.AdjustedPrice;
                                                    }
                                                }

                                                newdeliveryitem.BaseDoc = selectedObject.DocNum.ToString();
                                                newdeliveryitem.BaseId = dtlload.Oid.ToString();
                                                newdeliveryitem.SODocNum = reader.GetString(0);
                                                newdeliveryitem.SOBaseID = dtlpick.SOBaseId;
                                                newdeliveryitem.PickListDocNum = dtlpack.PickListNo;

                                                newdelivery.DeliveryOrderDetails.Add(newdeliveryitem);
                                            }
                                        }
                                    }

                                    picklistnum = dtlpack.PickListNo;
                                }
                            }
                        }

                        deiveryos.CommitChanges();
                    }
                }
                conn.Close();

                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();

                IObjectSpace os = Application.CreateObjectSpace();
                Load trx = os.FindObject<Load>(new BinaryOperator("Oid", selectedObject.Oid));
                openNewView(os, trx, ViewEditMode.View);
                showMsg("Successful", "Submit Done.", InformationType.Success);
            }
            else
            {
                showMsg("Error", "No Content.", InformationType.Error);
            }
        }

        private void SubmitL_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void CancelL_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Load selectedObject = (Load)e.CurrentObject;
            StringParameters p = (StringParameters)e.PopupWindow.View.CurrentObject;
            if (p.IsErr) return;

            selectedObject.Status = DocStatus.Cancelled;
            LoadDocTrail ds = ObjectSpace.CreateObject<LoadDocTrail>();
            ds.DocStatus = DocStatus.Cancelled;
            ds.DocRemarks = p.ParamString;
            selectedObject.LoadDocTrail.Add(ds);

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            IObjectSpace os = Application.CreateObjectSpace();
            Load trx = os.FindObject<Load>(new BinaryOperator("Oid", selectedObject.Oid));
            openNewView(os, trx, ViewEditMode.View);
            showMsg("Successful", "Cancel Done.", InformationType.Success);
        }

        private void CancelL_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(StringParameters));
            StringParameters message = os.CreateObject<StringParameters>();

            DetailView dv = Application.CreateDetailView(os, message);
            dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            ((StringParameters)dv.CurrentObject).IsErr = false;
            ((StringParameters)dv.CurrentObject).ActionMessage = "Press OK to CONFIRM the action and SAVE, else press Cancel.";

            e.View = dv;
        }

        private void LGenerateDO_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }
    }
}
