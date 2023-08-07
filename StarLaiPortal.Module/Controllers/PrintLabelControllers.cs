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
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Advanced_Shipment_Notice;
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Print_Module;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Quotation;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
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
    public partial class PrintLabelControllers : ViewController
    {
        GeneralControllers genCon;
        public PrintLabelControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            this.AddReportItem.Active.SetItemValue("Enabled", false);
            this.RetriveDocItem.Active.SetItemValue("Enabled", false);
            this.PrintLabel.Active.SetItemValue("Enabled", false);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GeneralControllers>();

            if (View.Id == "PrintLabel_DetailView")
            {
                if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
                {
                    this.AddReportItem.Active.SetItemValue("Enabled", true);
                    this.RetriveDocItem.Active.SetItemValue("Enabled", true);
                    this.PrintLabel.Active.SetItemValue("Enabled", true);
                }
                else
                {
                    this.AddReportItem.Active.SetItemValue("Enabled", false);
                    this.RetriveDocItem.Active.SetItemValue("Enabled", false);
                    this.PrintLabel.Active.SetItemValue("Enabled", false);
                }
            }
            else
            {
                this.AddReportItem.Active.SetItemValue("Enabled", false);
                this.RetriveDocItem.Active.SetItemValue("Enabled", false);
                this.PrintLabel.Active.SetItemValue("Enabled", false);
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

        private void AddReportItem_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PrintLabel selectedObject = (PrintLabel)e.CurrentObject;

            if (selectedObject.ItemCode != null)
            {
                if (selectedObject.ItemQuantity > 0)
                {
                    PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                    newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(selectedObject.ItemCode.ItemCode);
                    newitem.LabelType = selectedObject.LabelType;
                    newitem.Quantity = selectedObject.ItemQuantity;
                    newitem.Remarks = selectedObject.Remarks;

                    selectedObject.PrintLabelDetails.Add(newitem);

                    selectedObject.ItemCode = null;

                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();
                }
                else
                {
                    showMsg("Error", "No quantity.", InformationType.Error);
                }
            }
            else
            {
                showMsg("Error", "No item selected.", InformationType.Error);
            }
        }

        private void RetriveDocItem_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PrintLabel selectedObject = (PrintLabel)e.CurrentObject;
            string labeltype = null;
            if (selectedObject.LabelType == LabelType.RH)
            {
                labeltype = "RH";
            }
            else if (selectedObject.LabelType == LabelType.LH)
            {
                labeltype = "LH";
            }
            else if (selectedObject.LabelType == LabelType.Others)
            {
                labeltype = "OTHER";
            }

            if (selectedObject.DocNum != null)
            {
                if (selectedObject.Doctype == ReportDocType.ASN)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    ASN trx = os.FindObject<ASN>(new BinaryOperator("DocNum", selectedObject.DocNum));

                    if (trx != null)
                    {
                        foreach (ASNDetails dtl in trx.ASNDetails)
                        {
                            if (dtl.UnloadQty > 0)
                            {
                                if (dtl.ItemCode.LabelType.ToString() == labeltype)
                                {
                                    if (selectedObject.Reprint == PrintLabelReprint.No && dtl.LabelPrintCount == 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.UnloadQty;
                                        newitem.Doctype = ReportDocType.ASN;
                                        newitem.Remarks = trx.BatchNumber;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                    else if (selectedObject.Reprint == PrintLabelReprint.Yes && dtl.LabelPrintCount > 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.UnloadQty;
                                        newitem.Doctype = ReportDocType.ASN;
                                        newitem.Remarks = trx.BatchNumber;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                }
                            }
                        }

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    }
                    else
                    {
                        showMsg("Error", "Document not found.", InformationType.Error);
                    }
                }

                if (selectedObject.Doctype == ReportDocType.PO)
                {
                    PurchaseOrders trx = ObjectSpace.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", selectedObject.DocNum));

                    if (trx != null)
                    {
                        foreach (PurchaseOrderDetails dtl in trx.PurchaseOrderDetails)
                        {
                            if (dtl.Quantity > 0)
                            {
                                if (dtl.ItemCode.LabelType.ToString() == labeltype)
                                {
                                    if (selectedObject.Reprint == PrintLabelReprint.No && dtl.LabelPrintCount == 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.Quantity;
                                        newitem.Doctype = ReportDocType.PO;
                                        newitem.Remarks = selectedObject.Remarks;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                    else if (selectedObject.Reprint == PrintLabelReprint.Yes && dtl.LabelPrintCount > 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.Quantity;
                                        newitem.Doctype = ReportDocType.PO;
                                        newitem.Remarks = selectedObject.Remarks;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                }
                            }
                        }

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    }
                    else
                    {
                        showMsg("Error", "Document not found.", InformationType.Error);
                    }
                }

                if (selectedObject.Doctype == ReportDocType.SRR)
                {
                    SalesReturnRequests trx = ObjectSpace.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", selectedObject.DocNum));

                    if (trx != null)
                    {
                        foreach (SalesReturnRequestDetails dtl in trx.SalesReturnRequestDetails)
                        {
                            if (dtl.RtnQuantity > 0)
                            {
                                if (dtl.ItemCode.LabelType.ToString() == labeltype)
                                {
                                    if (selectedObject.Reprint == PrintLabelReprint.No && dtl.LabelPrintCount == 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.RtnQuantity;
                                        newitem.Doctype = ReportDocType.SRR;
                                        newitem.Remarks = selectedObject.Remarks;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                    else if (selectedObject.Reprint == PrintLabelReprint.Yes && dtl.LabelPrintCount > 0)
                                    {
                                        PrintLabelDetails newitem = ObjectSpace.CreateObject<PrintLabelDetails>();

                                        newitem.ItemCode = newitem.Session.GetObjectByKey<vwItemMasters>(dtl.ItemCode.ItemCode);
                                        newitem.LabelType = selectedObject.LabelType;
                                        newitem.DocNum = selectedObject.DocNum;
                                        newitem.Quantity = dtl.RtnQuantity;
                                        newitem.Doctype = ReportDocType.SRR;
                                        newitem.Remarks = selectedObject.Remarks;
                                        newitem.PrintCount = dtl.LabelPrintCount;

                                        selectedObject.PrintLabelDetails.Add(newitem);
                                    }
                                }
                            }
                        }

                        ObjectSpace.CommitChanges();
                        ObjectSpace.Refresh();
                    }
                    else
                    {
                        showMsg("Error", "Document not found.", InformationType.Error);
                    }
                }
            }
            else
            {
                showMsg("Error", "No Document Number.", InformationType.Error);
            }
        }

        private void PrintLabel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            string strServer;
            string strDatabase;
            string strUserID;
            string strPwd;
            string filename;

            SqlConnection conn = new SqlConnection(genCon.getConnectionString());
            PrintLabel print = (PrintLabel)View.CurrentObject;
            ApplicationUser user = (ApplicationUser)SecuritySystem.CurrentUser;

            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();

            try
            {
                ReportDocument doc = new ReportDocument();
                strServer = ConfigurationManager.AppSettings.Get("SQLserver").ToString();
                doc.Load(HttpContext.Current.Server.MapPath("~\\Reports\\BarcodeLabel.rpt"));
                strDatabase = conn.Database;
                strUserID = ConfigurationManager.AppSettings.Get("SQLID").ToString();
                strPwd = ConfigurationManager.AppSettings.Get("SQLPass").ToString();
                doc.DataSourceConnections[0].SetConnection(strServer, strDatabase, strUserID, strPwd);
                doc.Refresh();

                doc.SetParameterValue("OID", print.Oid);

                filename = ConfigurationManager.AppSettings.Get("ReportPath").ToString() + conn.Database
                    + "_" + print.Oid + "_" + user.UserName + "_Bundle_"
                    + DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd") + ".pdf";

                doc.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
                doc.Close();
                doc.Dispose();

                string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                    ConfigurationManager.AppSettings.Get("PrintPath").ToString() + conn.Database
                    + "_" + print.Oid + "_" + user.UserName + "_Bundle_"
                    + DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd") + ".pdf";
                var script = "window.open('" + url + "');";

                WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", script);

                foreach (PrintLabelDetails dtl in print.PrintLabelDetails)
                {
                    if (dtl.DocNum != null)
                    {
                        if (dtl.Doctype == ReportDocType.ASN)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            ASN trx = os.FindObject<ASN>(new BinaryOperator("DocNum", dtl.DocNum));

                            foreach(ASNDetails trxdetail in trx.ASNDetails)
                            {
                                if (trxdetail.ItemCode.ItemCode == dtl.ItemCode.ItemCode)
                                {
                                    trxdetail.LabelPrintCount++;
                                    break;
                                }
                            }

                            os.CommitChanges();
                            os.Refresh();
                        }

                        if (dtl.Doctype == ReportDocType.PO)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            PurchaseOrders trx = os.FindObject<PurchaseOrders>(new BinaryOperator("DocNum", dtl.DocNum));

                            foreach (PurchaseOrderDetails trxdetail in trx.PurchaseOrderDetails)
                            {
                                if (trxdetail.ItemCode.ItemCode == dtl.ItemCode.ItemCode)
                                {
                                    trxdetail.LabelPrintCount++;
                                    break;
                                }
                            }

                            os.CommitChanges();
                            os.Refresh();
                        }


                        if (dtl.Doctype == ReportDocType.SRR)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            SalesReturnRequests trx = os.FindObject<SalesReturnRequests>(new BinaryOperator("DocNum", dtl.DocNum));

                            foreach (SalesReturnRequestDetails trxdetail in trx.SalesReturnRequestDetails)
                            {
                                if (trxdetail.ItemCode.ItemCode == dtl.ItemCode.ItemCode)
                                {
                                    trxdetail.LabelPrintCount++;
                                    break;
                                }
                            }

                            os.CommitChanges();
                            os.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showMsg("Fail", ex.Message, InformationType.Error);
            }
        }
    }
}
