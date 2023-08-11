using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using SAPbobsCOM;
using StarLaiPortal.Module.BusinessObjects;
using StarLaiPortal.Module.BusinessObjects.Credit_Notes_Cancellation;
using StarLaiPortal.Module.BusinessObjects.Delivery_Order;
using StarLaiPortal.Module.BusinessObjects.GRN;
using StarLaiPortal.Module.BusinessObjects.Pick_List;
using StarLaiPortal.Module.BusinessObjects.Purchase_Order;
using StarLaiPortal.Module.BusinessObjects.Purchase_Return;
using StarLaiPortal.Module.BusinessObjects.Sales_Order;
using StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection;
using StarLaiPortal.Module.BusinessObjects.Sales_Refund;
using StarLaiPortal.Module.BusinessObjects.Sales_Return;
using StarLaiPortal.Module.BusinessObjects.Stock_Adjustment;
using StarLaiPortal.Module.BusinessObjects.View;
using StarLaiPortal.Module.BusinessObjects.Warehouse_Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 2023-07-28 add GRPO Correction ver 1.0.7

namespace PortalIntegration
{
    class Code
    {
        private SortedDictionary<string, List<string>> logs = new SortedDictionary<string, List<string>>();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DataSourceConnectionString"].ToString());

        public Code(SecurityStrategyComplex security, IObjectSpaceProvider ObjectSpaceProvider)
        {
            logs.Clear();
            WriteLog("[Log]", "--------------------------------------------------------------------------------");
            WriteLog("[Log]", "Post Begin:[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "]");

            #region Connect to SAP  
            SAPCompany sap = new SAPCompany();
            if (sap.connectSAP())
            {
                WriteLog("[Log]", "Connected to SAP:[" + sap.oCom.CompanyName + "] Time:[" + DateTime.Now.ToString("hh:mm:ss tt") + "]");
            }
            else
            {
                WriteLog("[Error]", "SAP Connection:[" + sap.oCom.CompanyDB + "] Message:[" + sap.errMsg + "] Time:[" + DateTime.Now.ToString("hh: mm:ss tt") + "]");
                sap.oCom = null;
                goto EndApplication;
            }
            #endregion

            try
            {
                string temp = "";
                IObjectSpace ListObjectSpace = ObjectSpaceProvider.CreateObjectSpace();
                IObjectSpace securedObjectSpace = ObjectSpaceProvider.CreateObjectSpace();

                temp = ConfigurationManager.AppSettings["SOPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--SO Posting Start--");

                    #region SO 
                    IList<SalesOrder> solist = ListObjectSpace.GetObjects<SalesOrder>
                    (CriteriaOperator.Parse("Sap = ?", 0));

                    foreach (SalesOrder dtlso in solist)
                    {
                        try
                        {
                            IObjectSpace soos = ObjectSpaceProvider.CreateObjectSpace();
                            SalesOrder soobj = soos.GetObjectByKey<SalesOrder>(dtlso.Oid);

                            #region Post SO
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int temppo = 0;

                            temppo = PostSOtoSAP(soobj, ObjectSpaceProvider, sap);
                            if (temppo == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                //soobj.Status = DocStatus.Post;
                                soobj.Sap = true;

                                SalesOrderDocStatus ds = soos.CreateObject<SalesOrderDocStatus>();
                                ds.CreateUser = soos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                soobj.SalesOrderDocStatus.Add(ds);

                                GC.Collect();
                            }
                            else if (temppo <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            soos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: POST SO Failed - OID : " + dtlso.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--SO Posting End--");
                }

                temp = ConfigurationManager.AppSettings["POPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--PO Posting Start--");

                    #region PO 
                    IList<PurchaseOrders> polist = ListObjectSpace.GetObjects<PurchaseOrders>
                    (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 4));

                    foreach (PurchaseOrders dtlpo in polist)
                    {
                        try
                        {
                            IObjectSpace poos = ObjectSpaceProvider.CreateObjectSpace();
                            PurchaseOrders poobj = poos.GetObjectByKey<PurchaseOrders>(dtlpo.Oid);

                            #region Post PO
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int temppo = 0;

                            temppo = PostPOtoSAP(poobj, ObjectSpaceProvider, sap);
                            if (temppo == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                poobj.Status = DocStatus.Post;
                                poobj.Sap = true;

                                PurchaseOrderDocTrail ds = poos.CreateObject<PurchaseOrderDocTrail>();
                                ds.CreateUser = poos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                poobj.PurchaseOrderDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (temppo <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            poos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: POST PO Failed - OID : " + dtlpo.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--PO Posting End--");
                }

                temp = ConfigurationManager.AppSettings["GRNPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--GRN Posting Start--");

                    #region GRN
                    IList<GRN> grnlist = ListObjectSpace.GetObjects<GRN>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (GRN dtlgrn in grnlist)
                    {
                        try
                        {
                            IObjectSpace grnos = ObjectSpaceProvider.CreateObjectSpace();
                            GRN grnobj = grnos.GetObjectByKey<GRN>(dtlgrn.Oid);

                            #region Post GRN
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int tempgrn = 0;

                            tempgrn = PostGRNtoSAP(grnobj, ObjectSpaceProvider, sap);
                            if (tempgrn == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                grnobj.Sap = true;
                                grnobj.Status = DocStatus.Post;

                                GRNDocTrail ds = grnos.CreateObject<GRNDocTrail>();
                                ds.CreateUser = grnos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                grnobj.GRNDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (tempgrn <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            grnos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: GRN Post Failed - OID : " + dtlgrn.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--GRN Posting End--");
                }

                temp = ConfigurationManager.AppSettings["PReturnPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Purchase Return Posting Start--");
      
                    #region Purchase Return
                    IList<PurchaseReturns> preturnlist = ListObjectSpace.GetObjects<PurchaseReturns>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (PurchaseReturns dtlpreturn in preturnlist)
                    {
                        try
                        {
                            IObjectSpace pros = ObjectSpaceProvider.CreateObjectSpace();
                            PurchaseReturns probj = pros.GetObjectByKey<PurchaseReturns>(dtlpreturn.Oid);

                            #region Post Purchase Return
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int temppr = 0;

                            temppr = PostPReturntoSAP(probj, ObjectSpaceProvider, sap);
                            if (temppr == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                probj.Sap = true;
                                probj.Status = DocStatus.Post;

                                PurchaseReturnDocTrail ds = pros.CreateObject<PurchaseReturnDocTrail>();
                                ds.CreateUser = pros.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                probj.PurchaseReturnDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (temppr <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            pros.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Purchase Return Post Failed - OID : " + dtlpreturn.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Purchase Return Posting End--");
                }

                temp = ConfigurationManager.AppSettings["SReturnPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Sales Return Posting Start--");

                    #region Sales Return
                    IList<SalesReturns> sreturnlist = ListObjectSpace.GetObjects<SalesReturns>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (SalesReturns dtlsreturn in sreturnlist)
                    {
                        try
                        {
                            IObjectSpace pros = ObjectSpaceProvider.CreateObjectSpace();
                            SalesReturns srobj = pros.GetObjectByKey<SalesReturns>(dtlsreturn.Oid);

                            #region Post Sales Return
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int tempsr = 0;

                            tempsr = PostSReturntoSAP(srobj, ObjectSpaceProvider, sap);
                            if (tempsr == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                srobj.Sap = true;
                                srobj.Status = DocStatus.Post;

                                SalesReturnDocTrail ds = pros.CreateObject<SalesReturnDocTrail>();
                                ds.CreateUser = pros.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                srobj.SalesReturnDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (tempsr <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            pros.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Sales Return Post Failed - OID : " + dtlsreturn.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Sales Return Posting End--");
                }

                temp = ConfigurationManager.AppSettings["WhsTransferPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Warehouse Transfer Posting Start--");

                    #region Warehouse Transfer
                    IList<WarehouseTransfers> wtlist = ListObjectSpace.GetObjects<WarehouseTransfers>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (WarehouseTransfers dtlwt in wtlist)
                    {
                        try
                        {
                            IObjectSpace wtos = ObjectSpaceProvider.CreateObjectSpace();
                            WarehouseTransfers wtobj = wtos.GetObjectByKey<WarehouseTransfers>(dtlwt.Oid);

                            #region Post Warehouse Transfer
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int tempto = 0;

                            tempto = PostWTIntoSAP(wtobj, ObjectSpaceProvider, sap);
                            if (tempto == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                wtobj.Sap = true;
                                wtobj.Status = DocStatus.Post;

                                WarehouseTransfersDocTrail ds = wtos.CreateObject<WarehouseTransfersDocTrail>();
                                ds.CreateUser = wtos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                wtobj.WarehouseTransfersDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (tempto <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            wtos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Warehouse Transfer Post Failed - OID : " + dtlwt.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Warehouse Transfer Posting End--");
                }

                temp = ConfigurationManager.AppSettings["SAPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Stock Adjustment Posting Start--");

                    #region Stock Adjustment

                    IList<StockAdjustments> salist = ListObjectSpace.GetObjects<StockAdjustments>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (StockAdjustments dtlsa in salist)
                    {
                        try
                        {
                            IObjectSpace saos = ObjectSpaceProvider.CreateObjectSpace();
                            StockAdjustments saobj = saos.GetObjectByKey<StockAdjustments>(dtlsa.Oid);

                            bool postfail = false;
                            bool positive = true;
                            bool negatif = true;
                            bool postissue = true;


                            foreach (StockAdjustmentDetails dtl in saobj.StockAdjustmentDetails)
                            {
                                if (dtl.Quantity > 0 && dtl.Sap == false)
                                {
                                    positive = false;
                                }

                                if (dtl.Quantity < 0 && dtl.Sap == false)
                                {
                                    negatif = false;
                                }
                            }

                            if (negatif == false)
                            {
                                #region Post Goods Issue
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int tempccout = 0;

                                tempccout = PostSAOuttoSAP(saobj, ObjectSpaceProvider, sap);
                                if (tempccout == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    foreach (StockAdjustmentDetails dtl in saobj.StockAdjustmentDetails)
                                    {
                                        if (dtl.Quantity < 0)
                                        {
                                            dtl.Sap = true;
                                        }
                                    }

                                    GC.Collect();
                                }
                                else if (tempccout <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    postfail = true;
                                    postissue = false;

                                    GC.Collect();
                                }
                                #endregion

                                saos.CommitChanges();
                            }

                            if (positive == false && postissue == true)
                            {
                                #region Post Goods Receipt
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int tempccin = 0;

                                tempccin = PostSAINtoSAP(saobj, ObjectSpaceProvider, sap);
                                if (tempccin == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    foreach (StockAdjustmentDetails dtl in saobj.StockAdjustmentDetails)
                                    {
                                        if (dtl.Quantity > 0)
                                        {
                                            dtl.Sap = true;
                                        }
                                    }

                                    GC.Collect();
                                }
                                else if (tempccin <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    postfail = true;

                                    GC.Collect();
                                }
                                #endregion
                            }

                            if (postfail == false)
                            {
                                saobj.Sap = true;

                                StockAdjustmentDocTrail ds = saos.CreateObject<StockAdjustmentDocTrail>();
                                ds.CreateUser = saos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                saobj.StockAdjustmentDocTrail.Add(ds);

                                saos.CommitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Stock Adjustment Post Failed - OID : " + dtlsa.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion
                
                    WriteLog("[INFO]", "--Stock Adjustment Posting End--");
                }

                temp = ConfigurationManager.AppSettings["CNPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Sales Refund Posting Start--");

                    #region Sales Refund
                    IList<SalesRefundRequests> cnlist = ListObjectSpace.GetObjects<SalesRefundRequests>
                        (CriteriaOperator.Parse("Sap = ? and ((Status = ? and AppStatus = ?) or (Status = ? and AppStatus = ?))", 0, 4, 0, 4, 1));

                    foreach (SalesRefundRequests dtlcn in cnlist)
                    {
                        try
                        {
                            IObjectSpace cnos = ObjectSpaceProvider.CreateObjectSpace();
                            SalesRefundRequests cnobj = cnos.GetObjectByKey<SalesRefundRequests>(dtlcn.Oid);

                            #region Post CN
                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                            int tempto = 0;

                            tempto = PostCNtoSAP(cnobj, ObjectSpaceProvider, sap);
                            if (tempto == 1)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                cnobj.Sap = true;
                                cnobj.Status = DocStatus.Post;

                                SalesRefundReqDocTrail ds = cnos.CreateObject<SalesRefundReqDocTrail>();
                                ds.CreateUser = cnos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                cnobj.SalesRefundReqDocTrail.Add(ds);

                                GC.Collect();
                            }
                            else if (tempto <= 0)
                            {
                                if (sap.oCom.InTransaction)
                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                GC.Collect();
                            }
                            #endregion

                            cnos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Sales Refund Post Failed - OID : " + dtlcn.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Sales Refund Posting End--");
                }

                temp = ConfigurationManager.AppSettings["ARDPPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--AR Downpayment Posting Start--");

                    #region AR Downpayment
                    IList<SalesOrderCollection> dplist = ListObjectSpace.GetObjects<SalesOrderCollection>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (SalesOrderCollection dtldp in dplist)
                    {
                        try
                        {
                            IObjectSpace dpos = ObjectSpaceProvider.CreateObjectSpace();
                            SalesOrderCollection dpobj = dpos.GetObjectByKey<SalesOrderCollection>(dtldp.Oid);
                            bool postfail = false;
                            bool postdp = false;
                            bool postpayment = false;
                            bool pendingsales = false;

                            foreach (SalesOrderCollectionDetails dtl in dpobj.SalesOrderCollectionDetails)
                            {
                                if (dtl.Sap == false)
                                {
                                    #region Post AR Downpayment
                                    if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                    int tempto = 0;

                                    tempto = PostDPtoSAP(dpobj, dtl.SalesOrder, ObjectSpaceProvider, sap);
                                    if (tempto == 1)
                                    {
                                        if (sap.oCom.InTransaction)
                                            sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                        dtl.Sap = true;
                                        postdp = true;

                                        dpos.CommitChanges();

                                        GC.Collect();
                                    }
                                    else if (tempto <= 0)
                                    {
                                        if (sap.oCom.InTransaction)
                                            sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                        postfail = true;

                                        GC.Collect();
                                    }
                                    else if (tempto == 2)
                                    {
                                        if (sap.oCom.InTransaction)
                                            sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                        postfail = true;
                                        pendingsales = true;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    postdp = true;
                                }

                                if (dtl.SapPayment == false)
                                {
                                    int dpdocentry = 0;
                                    string getdpDocentry = "SELECT DocEntry FROM [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() +
                                            "]..ODPI WHERE U_PortalDocNum = '" + dpobj.DocNum + "' AND U_SoDocNumber = '" + dtl.SalesOrder + "'";
                                    if (conn.State == ConnectionState.Open)
                                    {
                                        conn.Close();
                                    }
                                    conn.Open();
                                    SqlCommand cmd = new SqlCommand(getdpDocentry, conn);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        dpdocentry = reader.GetInt32(0);
                                    }
                                    conn.Close();

                                    if (dtl.PaymentAmount > 0 && dpdocentry > 0)
                                    {
                                        #region Post AR Downpayment Payment
                                        if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                        int tempardp = 0;

                                        tempardp = PostDPPaymenttoSAP(dpobj, dtl, dtl.SalesOrder, ObjectSpaceProvider, sap, dpdocentry);
                                        if (tempardp == 1)
                                        {
                                            if (sap.oCom.InTransaction)
                                                sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                            dtl.SapPayment = true;
                                            postpayment = true;

                                            dpos.CommitChanges();

                                            GC.Collect();
                                        }
                                        else if (tempardp <= 0)
                                        {
                                            if (sap.oCom.InTransaction)
                                                sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                            postfail = true;

                                            GC.Collect();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        postpayment = true;
                                    }
                                }
                                else
                                {
                                    postpayment = true;
                                }
                            }

                            if (postfail == false && postdp == true && postpayment == true)
                            {
                                dpobj.Status = DocStatus.Post;
                                dpobj.Sap = true;

                                SalesOrderCollectionDocStatus ds = dpos.CreateObject<SalesOrderCollectionDocStatus>();
                                ds.CreateUser = dpos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                dpobj.SalesOrderCollectionDocStatus.Add(ds);

                                dpos.CommitChanges();
                            }

                            if (pendingsales == true)
                            {
                                SalesOrderCollectionDocStatus ds = dpos.CreateObject<SalesOrderCollectionDocStatus>();
                                ds.CreateUser = dpos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Pending Sales Posting.";
                                dpobj.SalesOrderCollectionDocStatus.Add(ds);

                                dpos.CommitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: AR Downpayment Post Failed - OID : " + dtldp.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--AR Downpayment Posting End--");
                }

                temp = ConfigurationManager.AppSettings["PickListPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Pick List Start--");

                    #region Pick List 
                    IList<PickList> pllist = ListObjectSpace.GetObjects<PickList>
                    (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (PickList dtlpl in pllist)
                    {
                        try
                        {
                            IObjectSpace plos = ObjectSpaceProvider.CreateObjectSpace();
                            PickList plobj = plos.GetObjectByKey<PickList>(dtlpl.Oid);
                            bool post = true;

                            string getPLWhs = "SELECT Warehouse FROM PickListDetails WHERE PickList = " + plobj.Oid + " GROUP BY Warehouse";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(getPLWhs, conn);
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                #region Post Pick List
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int temppo = 0;

                                temppo = PostPLtoSAP(plobj, reader.GetString(0), ObjectSpaceProvider, sap);
                                if (temppo == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    //plobj.Status = DocStatus.Post;

                                    foreach (PickListDetailsActual dtl in plobj.PickListDetailsActual)
                                    {
                                        if (dtl.Warehouse.WarehouseCode == reader.GetString(0))
                                        {
                                            dtl.Sap = true;
                                        }
                                    }

                                    plos.CommitChanges();

                                    GC.Collect();
                                }
                                else if (temppo <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    GC.Collect();
                                }
                                else if (temppo == 2)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    foreach (PickListDetailsActual dtl in plobj.PickListDetailsActual)
                                    {
                                        if (dtl.Warehouse.WarehouseCode == reader.GetString(0))
                                        {
                                            dtl.Sap = true;
                                        }
                                    }

                                    plos.CommitChanges();

                                    GC.Collect();
                                }
                                #endregion
                            }
                            conn.Close();

                            foreach (PickListDetailsActual dtl2 in plobj.PickListDetailsActual)
                            {
                                if (dtl2.Sap == false)
                                {
                                    post = false;
                                }
                            }

                            if (post == true)
                            {
                                plobj.Sap = true;

                                PickListDocTrail ds = plos.CreateObject<PickListDocTrail>();
                                ds.CreateUser = plos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                plobj.PickListDocTrail.Add(ds);
                            }

                            plos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: POST Pick List Failed - OID : " + dtlpl.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Pick List End--");
                }

                temp = ConfigurationManager.AppSettings["DOPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Delivery Order Posting Start--");

                    #region Delivery Order
                    IList<DeliveryOrder> dolist = ListObjectSpace.GetObjects<DeliveryOrder>
                        (CriteriaOperator.Parse("Sap = ? and Status = ?", 0, 1));

                    foreach (DeliveryOrder dtldo in dolist)
                    {
                        try
                        {
                            IObjectSpace doos = ObjectSpaceProvider.CreateObjectSpace();
                            DeliveryOrder doobj = doos.GetObjectByKey<DeliveryOrder>(dtldo.Oid);

                            if (doobj.SapDO == false)
                            {
                                #region Post DO
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int tempdo = 0;

                                tempdo = PostARDOtoSAP(doobj, ObjectSpaceProvider, sap);
                                if (tempdo == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    doobj.SapDO = true;
                                    doos.CommitChanges();

                                    GC.Collect();
                                }
                                else if (tempdo <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    GC.Collect();
                                }
                                #endregion
                            }

                            if (doobj.SapINV == false && doobj.SapDO == true)
                            {
                                #region Post INV
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int tempdo = 0;

                                tempdo = PostDOtoSAP(doobj, ObjectSpaceProvider, sap);
                                if (tempdo == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    doobj.SapINV = true;
                                    doos.CommitChanges();

                                    GC.Collect();
                                }
                                else if (tempdo <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    GC.Collect();
                                }
                                #endregion
                            }

                            if (doobj.SapDO == true && doobj.SapINV == true)
                            {
                                doobj.Status = DocStatus.Post;
                                doobj.Sap = true;

                                DeliveryOrderDocTrail ds = doos.CreateObject<DeliveryOrderDocTrail>();
                                ds.CreateUser = doos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.Post;
                                ds.DocRemarks = "Posted SAP";
                                doobj.DeliveryOrderDocTrail.Add(ds);

                                doos.CommitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Delivery Order Post Failed - OID : " + dtldo.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Delivery Order Posting End--");
                }

                // Start ver 1.0.7
                temp = ConfigurationManager.AppSettings["CNCancelPost"].ToString().ToUpper();
                if (temp == "Y" || temp == "YES" || temp == "TRUE" || temp == "1")
                {
                    WriteLog("[INFO]", "--Downpayment Cancellation Posting Start--");

                    #region Downpayment Cancellation
                    IList<ARDownpaymentCancel> cnlist = ListObjectSpace.GetObjects<ARDownpaymentCancel>
                        (CriteriaOperator.Parse("Sap = ? and ((Status = ? and AppStatus = ?) or (Status = ? and AppStatus = ?))", 0, 1, 0, 1, 1));

                    foreach (ARDownpaymentCancel dtlcn in cnlist)
                    {
                        try
                        {
                            IObjectSpace cnos = ObjectSpaceProvider.CreateObjectSpace();
                            ARDownpaymentCancel cnobj = cnos.GetObjectByKey<ARDownpaymentCancel>(dtlcn.Oid);

                            string sonum = null;
                            bool fail = false;
                            foreach(ARDownpaymentCancelDetails dtl in cnobj.ARDownpaymentCancelDetails)
                            {
                                if (dtl.BaseDoc != null)
                                {
                                    if (dtl.BaseDoc != sonum)
                                    {
                                        string getdoDocentry = "SELECT DocNum FROM [" +
                                            ConfigurationManager.AppSettings["CompanyDB"].ToString() + "]..RCT2 WHERE DocEntry = "
                                            + dtl.BaseDoc;
                                        if (conn.State == ConnectionState.Open)
                                        {
                                            conn.Close();
                                        }
                                        conn.Open();
                                        SqlCommand cmd1 = new SqlCommand(getdoDocentry, conn);
                                        SqlDataReader reader1 = cmd1.ExecuteReader();
                                        while (reader1.Read())
                                        {
                                            #region Cancel DP
                                            if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                            int temppayment = 0;

                                            temppayment = PostCancelPaymenttoSAP(cnobj, reader1.GetInt32(0), ObjectSpaceProvider, sap);
                                            if (temppayment == 1)
                                            {
                                                if (sap.oCom.InTransaction)
                                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                                dtl.SapPayment = true;
                                                cnos.CommitChanges();

                                                GC.Collect();
                                            }
                                            else if (temppayment <= 0)
                                            {
                                                if (sap.oCom.InTransaction)
                                                    sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                                fail = true;
                                                GC.Collect();
                                            }
                                            else if (temppayment == 2)
                                            {
                                                sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                                dtl.SapPayment = true;
                                                cnos.CommitChanges();

                                                GC.Collect();
                                            }
                                            #endregion
                                        }
                                        conn.Close();

                                        sonum = dtl.BaseDoc;
                                    }
                                }
                            }

                            if (fail == false)
                            {
                                #region Post CN
                                if (!sap.oCom.InTransaction) sap.oCom.StartTransaction();

                                int tempto = 0;

                                tempto = PostCNCanceltoSAP(cnobj, ObjectSpaceProvider, sap);
                                if (tempto == 1)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    cnobj.Status = DocStatus.Post;
                                    cnobj.Sap = true;

                                    ARDownpaymentCancellationDocTrail ds = cnos.CreateObject<ARDownpaymentCancellationDocTrail>();
                                    ds.CreateUser = cnos.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                    ds.CreateDate = DateTime.Now;
                                    ds.DocStatus = DocStatus.Post;
                                    ds.DocRemarks = "Posted SAP";
                                    cnobj.ARDownpaymentCancellationDocTrail.Add(ds);

                                    GC.Collect();
                                }
                                else if (tempto <= 0)
                                {
                                    if (sap.oCom.InTransaction)
                                        sap.oCom.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                                    GC.Collect();
                                }
                                #endregion
                            }

                            cnos.CommitChanges();
                        }
                        catch (Exception ex)
                        {
                            WriteLog("[Error]", "Message: Sales Refund Post Failed - OID : " + dtlcn.Oid + " (" + ex.Message + ")");
                        }
                    }
                    #endregion

                    WriteLog("[INFO]", "--Downpayment Cancellation Posting End--");
                }
                // End ver 1.0.7

                #region Update DocNum
                SqlCommand TransactionNotification = new SqlCommand("", conn);
                TransactionNotification.CommandTimeout = 600;

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                try
                {
                    conn.Open();

                    TransactionNotification.CommandText = "EXEC sp_UpdSapDocNum '" + ConfigurationManager.AppSettings["CompanyDB"].ToString() + "'";

                    SqlDataReader reader = TransactionNotification.ExecuteReader();
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    //da.Dispose();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    WriteLog("[Error]", "Message:" + ex.Message);
                    conn.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog("[Error]", "Message:" + ex.Message);
            }

            // End Post ======================================================================================
            EndApplication:
            return;
        }

        private void WriteLog(string lvl, string str)
        {
            FileStream fileStream = null;

            string filePath = "C:\\" + ConfigurationManager.AppSettings["LogFileName"].ToString() + "\\";
            filePath = filePath + "[" + "Posting Status" + "] Log_" + System.DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

            FileInfo fileInfo = new FileInfo(filePath);
            DirectoryInfo dirInfo = new DirectoryInfo(fileInfo.DirectoryName);
            if (!dirInfo.Exists) dirInfo.Create();

            if (!fileInfo.Exists)
            {
                fileStream = fileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(filePath, FileMode.Append);
            }

            StreamWriter log = new StreamWriter(fileStream);
            string status = lvl.ToString().Replace("[Log]", "");

            //For Portal_UpdateStatus_Log
            log.WriteLine("{0}{1}", status, str.ToString());

            log.Close();
        }

        public int PostSOtoSAP(SalesOrder oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    bool found = false;
                    DateTime postdate = DateTime.Now;

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    int sapempid = 0;
                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.CustomerName;
                    oDoc.DocDate = oTargetDoc.PostingDate;
                    oDoc.DocDueDate = oTargetDoc.DeliveryDate;
                    oDoc.TaxDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                    if (oTargetDoc.Transporter != null)
                    {
                        oDoc.UserFields.Fields.Item("U_Transporter").Value = oTargetDoc.Transporter.TransporterID;
                    }
                    if (oTargetDoc.Priority != null)
                    {
                        oDoc.UserFields.Fields.Item("U_Priority").Value = oTargetDoc.Priority.PriorityName;
                    }
                    if (oTargetDoc.ContactPerson != null)
                    {
                        oDoc.SalesPersonCode = oTargetDoc.ContactPerson.SlpCode;
                    }
                    if (oTargetDoc.BillingAddress != null)
                    {
                        oDoc.PayToCode = oTargetDoc.BillingAddress.AddressKey;
                    }
                    if (oTargetDoc.BillingAddressfield != null)
                    {
                        oDoc.Address = oTargetDoc.BillingAddressfield;
                    }
                    if (oTargetDoc.ShippingAddress != null)
                    {
                        oDoc.ShipToCode = oTargetDoc.ShippingAddress.AddressKey;
                    }
                    if (oTargetDoc.ShippingAddressfield != null)
                    {
                        oDoc.Address2 = oTargetDoc.ShippingAddressfield;
                    }

                    if (oTargetDoc.Series != null)
                    {
                        oDoc.Series = int.Parse(oTargetDoc.Series.Series);
                    }

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;

                    int cnt = 0;
                    foreach (SalesOrderDetails dtl in oTargetDoc.SalesOrderDetails)
                    {
                        //if (dtl.LineAmount > 0)
                        //{
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        //oDoc.Lines.VatGroup = dtl.Tax.BoCode;
                        //oDoc.Lines.TaxTotal = (double)dtl.TaxAmount;
                        oDoc.Lines.WarehouseCode = dtl.Location.WarehouseCode;
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDetails = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.AdjustedPrice;
                    }
                   
                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        SalesOrder obj = osupdate.GetObjectByKey<SalesOrder>(oTargetDoc.Oid);

                        SalesOrderDocStatus ds = osupdate.CreateObject<SalesOrderDocStatus>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.SalesOrderDocStatus.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: SO Posting :" + oTargetDoc + "-" + temp);
                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesOrder obj = osupdate.GetObjectByKey<SalesOrder>(oTargetDoc.Oid);

                SalesOrderDocStatus ds = osupdate.CreateObject<SalesOrderDocStatus>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesOrderDocStatus.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: SO Posting :" + oTargetDoc + "-" + ex.Message);
                return -1;
            }
        }

        public int PostPOtoSAP(PurchaseOrders oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    bool found = false;
                    DateTime postdate = DateTime.Now;

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    if (oTargetDoc.PurchaseOrderAttachment != null && oTargetDoc.PurchaseOrderAttachment.Count > 0)
                    {
                        foreach (PurchaseOrderAttachment obj in oTargetDoc.PurchaseOrderAttachment)
                        {
                            string fullpath = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString() + g.ToString() + obj.File.FileName;
                            using (System.IO.FileStream fs = System.IO.File.OpenWrite(fullpath))
                            {
                                obj.File.SaveToStream(fs);
                            }
                        }
                    }

                    int sapempid = 0;
                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.CardCode = oTargetDoc.Supplier.BPCode;
                    oDoc.CardName = oTargetDoc.SupplierName;
                    oDoc.DocDate = oTargetDoc.PostingDate;
                    oDoc.DocDueDate = oTargetDoc.DeliveryDate;
                    //oDoc.TaxDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    if (oTargetDoc.Series != null)
                    {
                        oDoc.Series = int.Parse(oTargetDoc.Series.Series);
                    }

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;

                    int cnt = 0;
                    foreach (PurchaseOrderDetails dtl in oTargetDoc.PurchaseOrderDetails)
                    {
                        //if (dtl.LineAmount > 0)
                        //{
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        //oDoc.Lines.VatGroup = dtl.Tax.BoCode;
                        //oDoc.Lines.TaxTotal = (double)dtl.TaxAmount;
                        oDoc.Lines.WarehouseCode = dtl.Location.WarehouseCode;
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDetails = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.AdjustedPrice;
                        if (dtl.BaseDoc != null)
                        {
                            oDoc.Lines.UserFields.Fields.Item("U_BaseDocNum").Value = dtl.BaseDoc;
                        }

                    }
                    if (oTargetDoc.PurchaseOrderAttachment != null && oTargetDoc.PurchaseOrderAttachment.Count > 0)
                    {
                        cnt = 0;
                        SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)sap.oCom.GetBusinessObject(BoObjectTypes.oAttachments2);
                        foreach (PurchaseOrderAttachment dtl in oTargetDoc.PurchaseOrderAttachment)
                        {

                            cnt++;
                            if (cnt == 1)
                            {
                                if (oAtt.Lines.Count == 0)
                                    oAtt.Lines.Add();
                            }
                            else
                                oAtt.Lines.Add();

                            string attfile = "";
                            string[] fexe = dtl.File.FileName.Split('.');
                            if (fexe.Length <= 2)
                                attfile = fexe[0];
                            else
                            {
                                for (int x = 0; x < fexe.Length - 1; x++)
                                {
                                    if (attfile == "")
                                        attfile = fexe[x];
                                    else
                                        attfile += "." + fexe[x];
                                }
                            }
                            oAtt.Lines.FileName = g.ToString() + attfile;
                            if (fexe.Length > 1)
                                oAtt.Lines.FileExtension = fexe[fexe.Length - 1];
                            string path = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString();
                            path = path.Replace("\\\\", "\\");
                            path = path.Substring(0, path.Length - 1);
                            oAtt.Lines.SourcePath = path;
                            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                        }
                        int iAttEntry = -1;
                        if (oAtt.Add() == 0)
                        {
                            iAttEntry = int.Parse(sap.oCom.GetNewObjectKey());
                        }
                        else
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            PurchaseOrders obj = osupdate.GetObjectByKey<PurchaseOrders>(oTargetDoc.Oid);

                            PurchaseOrderDocTrail ds = osupdate.CreateObject<PurchaseOrderDocTrail>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.PurchaseOrderDocTrail.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: PO Attachement Error :" + oTargetDoc + "-" + temp);
                            return -1;
                        }
                        oDoc.AttachmentEntry = iAttEntry;
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        PurchaseOrders obj = osupdate.GetObjectByKey<PurchaseOrders>(oTargetDoc.Oid);

                        PurchaseOrderDocTrail ds = osupdate.CreateObject<PurchaseOrderDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.PurchaseOrderDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: PO Posting :" + oTargetDoc + "-" + temp);
                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                PurchaseOrders obj = osupdate.GetObjectByKey<PurchaseOrders>(oTargetDoc.Oid);

                PurchaseOrderDocTrail ds = osupdate.CreateObject<PurchaseOrderDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.PurchaseOrderDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: PO Posting :" + oTargetDoc + "-" + ex.Message);
                return -1;
            }
        }

        public int PostGRNtoSAP(GRN oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    bool found = false;
                    string asndoc = null;
                    string dupasndoc = null;
                    DateTime postdate = DateTime.Now;
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    int sapempid = 0;

                    SAPbobsCOM.Documents oDoc = null;
                    SAPbobsCOM.Recordset rs = null;

                    rs = (SAPbobsCOM.Recordset)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.CardCode = oTargetDoc.Supplier.BPCode;
                    oDoc.CardName = oTargetDoc.SupplierName;
                    oDoc.DocDate = oTargetDoc.DocDate;
                    //oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                    if (oTargetDoc.InvoiceNo != null)
                    {
                        oDoc.NumAtCard = oTargetDoc.InvoiceNo;
                    }

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;

                    int cnt = 0;
                    foreach (GRNDetails dtl in oTargetDoc.GRNDetails)
                    {
                        if (dtl.Received > 0)
                        {
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                //oDoc.Lines.BatchNumbers.Add();
                                //oDoc.Lines.BatchNumbers.SetCurrentLine(oDoc.Lines.Count - 1);
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            //oDoc.Lines.TaxCode = dtl.Tax.BoCode;
                            //oDoc.Lines.TaxTotal = (double)dtl.TaxAmount;
                            oDoc.Lines.WarehouseCode = dtl.Location.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.Received;// * (double)link.Packsize;
                            if (dtl.DiscrepancyReason != null)
                            {
                                oDoc.Lines.UserFields.Fields.Item("U_DiscreReason").Value = dtl.DiscrepancyReason;
                            }
                            if (dtl.DefBin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.DefBin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.Received;
                            }
                            //oDoc.Lines.UnitPrice = (double)dtl.UnitPrice;// / oDoc.Lines.Quantity;

                            if (dtl.BaseDoc != null)
                            {
                                oDoc.Lines.BaseType = 22;
                                oDoc.Lines.BaseEntry = int.Parse(dtl.BaseDoc);//Docentry
                                oDoc.Lines.BaseLine = int.Parse(dtl.BaseId);//line no

                                string po = "SELECT DocNum FROM [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() +
                                    "]..OPOR WHERE DocEntry = " + int.Parse(dtl.BaseDoc);

                                rs.DoQuery(po);

                                if (rs.RecordCount > 0)
                                {
                                    oDoc.Comments = rs.Fields.Item("DocNum").Value.ToString();
                                }
                            }

                            if (dtl.ASNBaseDoc != null)
                            {
                                oDoc.Lines.BaseType = 22;
                                oDoc.Lines.BaseEntry = int.Parse(dtl.ASNPOBaseDoc);//Docentry
                                oDoc.Lines.BaseLine = int.Parse(dtl.ASNPOBaseId);//line no

                                string po = "SELECT DocNum FROM [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() +
                                    "]..OPOR WHERE DocEntry = " + int.Parse(dtl.ASNPOBaseDoc);

                                rs.DoQuery(po);

                                if (rs.RecordCount > 0)
                                {
                                    oDoc.Comments = rs.Fields.Item("DocNum").Value.ToString();
                                }

                                if (dupasndoc != dtl.ASNBaseDoc)
                                {
                                    if (asndoc != null)
                                    {
                                        asndoc = asndoc + ", " + dtl.ASNBaseDoc;
                                    }
                                    else
                                    {
                                        asndoc = dtl.ASNBaseDoc;
                                    }

                                    dupasndoc = dtl.ASNBaseDoc;
                                }
                            }
                        }
                    }

                    if (asndoc != null)
                    {
                        int countchar = 0;
                        foreach (char c in asndoc)
                        {
                            countchar++;
                        }
                        if (countchar >= 99)
                        {
                            oDoc.UserFields.Fields.Item("U_PortalASNNum").Value = asndoc.Substring(1, 99).ToString();
                        }
                        else
                        {
                            oDoc.UserFields.Fields.Item("U_PortalASNNum").Value = asndoc;
                        }
                    }
               
                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        GRN obj = osupdate.GetObjectByKey<GRN>(oTargetDoc.Oid);

                        GRNDocTrail ds = osupdate.CreateObject<GRNDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.GRNDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: GRN Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                GRN obj = osupdate.GetObjectByKey<GRN>(oTargetDoc.Oid);

                GRNDocTrail ds = osupdate.CreateObject<GRNDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.GRNDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: GRN Posting :" + oTargetDoc + "-" + ex.Message);
                return -1;
            }
        }

        public int PostPReturntoSAP(PurchaseReturns oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    bool found = false;
                    DateTime postdate = DateTime.Now;
                
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    int sapempid = 0;

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.CardCode = oTargetDoc.Supplier.BPCode;
                    oDoc.CardName = oTargetDoc.SupplierName;
                    oDoc.DocDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;

                    int cnt = 0;
                    foreach (PurchaseReturnDetails dtl in oTargetDoc.PurchaseReturnDetails)
                    {
                        //if (dtl.Total > 0)
                        //{
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            //oDoc.Lines.TaxCode = dtl.Tax.BoCode;
                            //oDoc.Lines.TaxTotal = (double)dtl.TaxAmount;
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();
                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.Quantity;
                            oDoc.Lines.UnitPrice = (double)dtl.Price;
                            if (dtl.PODocNum != null)
                            {
                              oDoc.Lines.UserFields.Fields.Item("U_BaseDocNum").Value = dtl.PODocNum;
                            }

                            if (dtl.Bin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                            }

                            // Start ver 1.0.7
                            if (oTargetDoc.GRPOCorrection == true)
                            {
                                if (dtl.GRNBaseDoc != null)
                                {
                                    oDoc.Lines.BaseType = 20;
                                    oDoc.Lines.BaseEntry = int.Parse(dtl.GRNBaseDoc);
                                    oDoc.Lines.BaseLine = int.Parse(dtl.GRNBaseId);
                                }
                            }
                            // End ver 1.0.7
                        //}
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        PurchaseReturns obj = osupdate.GetObjectByKey<PurchaseReturns>(oTargetDoc.Oid);

                        PurchaseReturnDocTrail ds = osupdate.CreateObject<PurchaseReturnDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.PurchaseReturnDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Purchase Return Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                PurchaseReturns obj = osupdate.GetObjectByKey<PurchaseReturns>(oTargetDoc.Oid);

                PurchaseReturnDocTrail ds = osupdate.CreateObject<PurchaseReturnDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.PurchaseReturnDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Purchase Return Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostSReturntoSAP(SalesReturns oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    bool found = false;
                    DateTime postdate = DateTime.Now;

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    int sapempid = 0;

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.CustomerName;
                    oDoc.DocDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.NumAtCard = oTargetDoc.Reference;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;

                    int cnt = 0;
                    foreach (SalesReturnDetails dtl in oTargetDoc.SalesReturnDetails)
                    {
                        //if (dtl.Total > 0)
                        //{
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            //oDoc.Lines.TaxCode = dtl.Tax.BoCode;
                            //oDoc.Lines.TaxTotal = (double)dtl.TaxAmount;
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();
                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.RtnQuantity;
                            oDoc.Lines.UnitPrice = (double)dtl.Price;
                            if (dtl.ReasonCode != null)
                            {
                                oDoc.Lines.UserFields.Fields.Item("U_SalReturnReason").Value = dtl.ReasonCode.ReasonCode;
                            }
                            if (dtl.UnitCost != 0)
                            {
                                oDoc.Lines.EnableReturnCost = BoYesNoEnum.tYES;
                                oDoc.Lines.ReturnCost = (double)dtl.UnitCost;
                            }

                            if (dtl.InvoiceDoc != null)
                            {
                                oDoc.Lines.UserFields.Fields.Item("U_BaseDocNum").Value = dtl.InvoiceDoc;
                            }
                            if (dtl.Bin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.RtnQuantity;
                            }

                            IObjectSpace osreq = ObjectSpaceProvider.CreateObjectSpace();
                            SalesReturnRequests srr = osreq.FindObject<SalesReturnRequests>(CriteriaOperator.Parse("DocNum = ?", dtl.BaseDoc));

                            foreach (SalesReturnRequestDetails srrdetail in srr.SalesReturnRequestDetails)
                            {
                                if (srrdetail.BaseDoc != null)
                                {
                                    oDoc.DocumentReferences.ReferencedDocEntry = int.Parse(srrdetail.BaseDoc);
                                    oDoc.DocumentReferences.ReferencedObjectType = ReferencedObjectTypeEnum.rot_SalesInvoice;
                                    oDoc.DocumentReferences.Remark = oTargetDoc.DocNum;
                                }
                            }
                        //}
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        SalesReturns obj = osupdate.GetObjectByKey<SalesReturns>(oTargetDoc.Oid);

                        SalesReturnDocTrail ds = osupdate.CreateObject<SalesReturnDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.SalesReturnDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Sales Return Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesReturns obj = osupdate.GetObjectByKey<SalesReturns>(oTargetDoc.Oid);

                SalesReturnDocTrail ds = osupdate.CreateObject<SalesReturnDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesReturnDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Sales Return Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostWTIntoSAP(WarehouseTransfers oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    if (oTargetDoc.WarehouseTransferAttachment != null && oTargetDoc.WarehouseTransferAttachment.Count > 0)
                    {
                        foreach (WarehouseTransferAttachment obj in oTargetDoc.WarehouseTransferAttachment)
                        {
                            string fullpath = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString() + g.ToString() + obj.File.FileName;
                            using (System.IO.FileStream fs = System.IO.File.OpenWrite(fullpath))
                            {
                                obj.File.SaveToStream(fs);
                            }
                        }
                    }

                    SAPbobsCOM.StockTransfer oDoc = null;

                    oDoc = (SAPbobsCOM.StockTransfer)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                    if (oTargetDoc.Supplier != null)
                    {
                        oDoc.CardCode = oTargetDoc.Supplier.BPCode;
                        oDoc.CardName = oTargetDoc.Supplier.BPName;
                    }
                    oDoc.DocDate = oTargetDoc.TransferDate;
                    //oDoc.TaxDate = oTargetDoc.DeliveryDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.FromWarehouse = oTargetDoc.FromWarehouse.WarehouseCode;
                    oDoc.ToWarehouse = oTargetDoc.ToWarehouse.WarehouseCode;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    int cnt = 0;
                    foreach (WarehouseTransferDetails dtl in oTargetDoc.WarehouseTransferDetails)
                    {
                        if (dtl.Quantity > 0)
                        {
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.Quantity;
                            oDoc.Lines.FromWarehouseCode = oTargetDoc.FromWarehouse.WarehouseCode;
                            oDoc.Lines.WarehouseCode = oTargetDoc.ToWarehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            if (dtl.FromBin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.FromBin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                                oDoc.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                            }

                            if (dtl.ToBin != null)
                            {
                                oDoc.Lines.BinAllocations.Add();
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.ToBin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                                oDoc.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                            }
                        }
                    }
                    if (oTargetDoc.WarehouseTransferAttachment != null && oTargetDoc.WarehouseTransferAttachment.Count > 0)
                    {
                        cnt = 0;
                        SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)sap.oCom.GetBusinessObject(BoObjectTypes.oAttachments2);
                        foreach (WarehouseTransferAttachment dtl in oTargetDoc.WarehouseTransferAttachment)
                        {

                            cnt++;
                            if (cnt == 1)
                            {
                                if (oAtt.Lines.Count == 0)
                                    oAtt.Lines.Add();
                            }
                            else
                                oAtt.Lines.Add();

                            string attfile = "";
                            string[] fexe = dtl.File.FileName.Split('.');
                            if (fexe.Length <= 2)
                                attfile = fexe[0];
                            else
                            {
                                for (int x = 0; x < fexe.Length - 1; x++)
                                {
                                    if (attfile == "")
                                        attfile = fexe[x];
                                    else
                                        attfile += "." + fexe[x];
                                }
                            }
                            oAtt.Lines.FileName = g.ToString() + attfile;
                            if (fexe.Length > 1)
                                oAtt.Lines.FileExtension = fexe[fexe.Length - 1];
                            string path = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString();
                            path = path.Replace("\\\\", "\\");
                            path = path.Substring(0, path.Length - 1);
                            oAtt.Lines.SourcePath = path;
                            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                        }
                        int iAttEntry = -1;
                        if (oAtt.Add() == 0)
                        {
                            iAttEntry = int.Parse(sap.oCom.GetNewObjectKey());
                        }
                        else
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            WarehouseTransfers obj = osupdate.GetObjectByKey<WarehouseTransfers>(oTargetDoc.Oid);

                            WarehouseTransfersDocTrail ds = osupdate.CreateObject<WarehouseTransfersDocTrail>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.WarehouseTransfersDocTrail.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: Warehoouse Transfer Attachement Error :" + oTargetDoc + "-" + temp);

                            return -1;
                        }
                        oDoc.AttachmentEntry = iAttEntry;
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        WarehouseTransfers obj = osupdate.GetObjectByKey<WarehouseTransfers>(oTargetDoc.Oid);

                        WarehouseTransfersDocTrail ds = osupdate.CreateObject<WarehouseTransfersDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.WarehouseTransfersDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Warehouse Transfer Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                WarehouseTransfers obj = osupdate.GetObjectByKey<WarehouseTransfers>(oTargetDoc.Oid);

                WarehouseTransfersDocTrail ds = osupdate.CreateObject<WarehouseTransfersDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.WarehouseTransfersDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Warehouse Transfer Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostSAOuttoSAP(StockAdjustments oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    bool found = false;

                    DateTime postdate = DateTime.Now;

                    foreach (StockAdjustmentDetails dtl in oTargetDoc.StockAdjustmentDetails)
                    {
                        found = true;
                    }
                    if (!found) return 0;

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    if (oTargetDoc.StockAdjustmentAttactment != null && oTargetDoc.StockAdjustmentAttactment.Count > 0)
                    {
                        foreach (StockAdjustmentAttactment obj in oTargetDoc.StockAdjustmentAttactment)
                        {
                            string fullpath = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString() + g.ToString() + obj.File.FileName;
                            using (System.IO.FileStream fs = System.IO.File.OpenWrite(fullpath))
                            {
                                obj.File.SaveToStream(fs);
                            }
                        }
                    }

                    int sapempid = 0;

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.DocDate = postdate;

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;
                    oDoc.DocDate = oTargetDoc.AdjDate;
                    oDoc.TaxDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    int cnt = 0;
                    foreach (StockAdjustmentDetails dtl in oTargetDoc.StockAdjustmentDetails)
                    {
                        if (dtl.Quantity < 0)
                        {
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                //oDoc.Lines.BatchNumbers.Add();
                                //oDoc.Lines.BatchNumbers.SetCurrentLine(oDoc.Lines.Count - 1);
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            if (oTargetDoc.ReasonCode.GLAcc != null)
                            {
                                oDoc.Lines.AccountCode = oTargetDoc.ReasonCode.GLAcc;
                            }
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)(dtl.Quantity - dtl.Quantity - dtl.Quantity);

                            if (dtl.Bin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)(dtl.Quantity - dtl.Quantity - dtl.Quantity);
                            }
                        }
                    }
                    if (oTargetDoc.StockAdjustmentAttactment != null && oTargetDoc.StockAdjustmentAttactment.Count > 0)
                    {
                        cnt = 0;
                        SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)sap.oCom.GetBusinessObject(BoObjectTypes.oAttachments2);
                        foreach (StockAdjustmentAttactment dtl in oTargetDoc.StockAdjustmentAttactment)
                        {

                            cnt++;
                            if (cnt == 1)
                            {
                                if (oAtt.Lines.Count == 0)
                                    oAtt.Lines.Add();
                            }
                            else
                                oAtt.Lines.Add();

                            string attfile = "";
                            string[] fexe = dtl.File.FileName.Split('.');
                            if (fexe.Length <= 2)
                                attfile = fexe[0];
                            else
                            {
                                for (int x = 0; x < fexe.Length - 1; x++)
                                {
                                    if (attfile == "")
                                        attfile = fexe[x];
                                    else
                                        attfile += "." + fexe[x];
                                }
                            }
                            oAtt.Lines.FileName = g.ToString() + attfile;
                            if (fexe.Length > 1)
                                oAtt.Lines.FileExtension = fexe[fexe.Length - 1];
                            string path = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString();
                            path = path.Replace("\\\\", "\\");
                            path = path.Substring(0, path.Length - 1);
                            oAtt.Lines.SourcePath = path;
                            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                        }
                        int iAttEntry = -1;
                        if (oAtt.Add() == 0)
                        {
                            iAttEntry = int.Parse(sap.oCom.GetNewObjectKey());
                        }
                        else
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                            StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.StockAdjustmentDocTrail.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: Stock Adjustment(Issue) Attachement Error :" + oTargetDoc + "-" + temp);

                            return -1;
                        }
                        oDoc.AttachmentEntry = iAttEntry;
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                        StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.StockAdjustmentDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Stock Adjustment(Issue) Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.StockAdjustmentDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Stock Adjustment(Issue) Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostSAINtoSAP(StockAdjustments oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    bool found = false;
                    DateTime postdate = DateTime.Now;

                    foreach (StockAdjustmentDetails dtl in oTargetDoc.StockAdjustmentDetails)
                    {
                        found = true;
                    }
                    if (!found) return 0;

                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    if (oTargetDoc.StockAdjustmentAttactment != null && oTargetDoc.StockAdjustmentAttactment.Count > 0)
                    {
                        foreach (StockAdjustmentAttactment obj in oTargetDoc.StockAdjustmentAttactment)
                        {
                            string fullpath = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString() + g.ToString() + obj.File.FileName;
                            using (System.IO.FileStream fs = System.IO.File.OpenWrite(fullpath))
                            {
                                obj.File.SaveToStream(fs);
                            }
                        }
                    }

                    int sapempid = 0;

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

                    oDoc.DocType = BoDocumentTypes.dDocument_Items;
                    oDoc.DocDate = postdate;

                    if (sapempid > 0)
                        oDoc.DocumentsOwner = sapempid;
                    oDoc.DocDate = oTargetDoc.AdjDate;
                    oDoc.TaxDate = oTargetDoc.DocDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    int cnt = 0;
                    foreach (StockAdjustmentDetails dtl in oTargetDoc.StockAdjustmentDetails)
                    {
                        if (dtl.Quantity > 0)
                        {
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                //oDoc.Lines.BatchNumbers.Add();
                                //oDoc.Lines.BatchNumbers.SetCurrentLine(oDoc.Lines.Count - 1);
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.Quantity;
                            if (oTargetDoc.ReasonCode.GLAcc != null)
                            {
                                oDoc.Lines.AccountCode = oTargetDoc.ReasonCode.GLAcc;
                            }
                            oDoc.Lines.UnitPrice = 0.01;

                            if (dtl.Bin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                            }
                        }
                    }
                    if (oTargetDoc.StockAdjustmentAttactment != null && oTargetDoc.StockAdjustmentAttactment.Count > 0)
                    {
                        cnt = 0;
                        SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)sap.oCom.GetBusinessObject(BoObjectTypes.oAttachments2);
                        foreach (StockAdjustmentAttactment dtl in oTargetDoc.StockAdjustmentAttactment)
                        {

                            cnt++;
                            if (cnt == 1)
                            {
                                if (oAtt.Lines.Count == 0)
                                    oAtt.Lines.Add();
                            }
                            else
                                oAtt.Lines.Add();

                            string attfile = "";
                            string[] fexe = dtl.File.FileName.Split('.');
                            if (fexe.Length <= 2)
                                attfile = fexe[0];
                            else
                            {
                                for (int x = 0; x < fexe.Length - 1; x++)
                                {
                                    if (attfile == "")
                                        attfile = fexe[x];
                                    else
                                        attfile += "." + fexe[x];
                                }
                            }
                            oAtt.Lines.FileName = g.ToString() + attfile;
                            if (fexe.Length > 1)
                                oAtt.Lines.FileExtension = fexe[fexe.Length - 1];
                            string path = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString();
                            path = path.Replace("\\\\", "\\");
                            path = path.Substring(0, path.Length - 1);
                            oAtt.Lines.SourcePath = path;
                            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                        }
                        int iAttEntry = -1;
                        if (oAtt.Add() == 0)
                        {
                            iAttEntry = int.Parse(sap.oCom.GetNewObjectKey());
                        }
                        else
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                            StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.StockAdjustmentDocTrail.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: Stock Adjustment(Receipt) Attachement Error :" + oTargetDoc + "-" + temp);

                            return -1;
                        }
                        oDoc.AttachmentEntry = iAttEntry;
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                        StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.StockAdjustmentDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Stock Adjustment(Receipt) Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                StockAdjustments obj = osupdate.GetObjectByKey<StockAdjustments>(oTargetDoc.Oid);

                StockAdjustmentDocTrail ds = osupdate.CreateObject<StockAdjustmentDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.StockAdjustmentDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Stock Adjustment(Receipt) Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostCNtoSAP(SalesRefundRequests oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.Customer.BPName;
                    oDoc.DocDate = oTargetDoc.PostingDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                    if (oTargetDoc.Reference != null)
                    {
                        oDoc.NumAtCard = oTargetDoc.Reference;
                    }
                    if (oTargetDoc.ContactPerson != null)
                    {
                        oDoc.SalesPersonCode = oTargetDoc.ContactPerson.SlpCode;
                    }

                    int cnt = 0;
                    foreach (SalesRefundReqDetails dtl in oTargetDoc.SalesRefundReqDetails)
                    {
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDescription = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.Price;
                        oDoc.Lines.WithoutInventoryMovement = BoYesNoEnum.tYES;
                        if (dtl.Warehouse != null)
                        {
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                        }
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();
                        if (dtl.ReasonCode != null)
                        {
                            oDoc.Lines.UserFields.Fields.Item("U_SalReturnReason").Value = dtl.ReasonCode.ReasonCode;
                        }

                        if (dtl.Bin != null)
                        {
                            oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                            oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                        }
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        SalesRefundRequests obj = osupdate.GetObjectByKey<SalesRefundRequests>(oTargetDoc.Oid);

                        SalesRefundReqDocTrail ds = osupdate.CreateObject<SalesRefundReqDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.SalesRefundReqDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Sales Refund Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesRefundRequests obj = osupdate.GetObjectByKey<SalesRefundRequests>(oTargetDoc.Oid);

                SalesRefundReqDocTrail ds = osupdate.CreateObject<SalesRefundReqDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesRefundReqDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Sales Refund Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostDPtoSAP(SalesOrderCollection oTargetDoc, string sodocnum, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace();
                SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", sodocnum));

                if (so != null)
                {
                    if (so.SAPDocNum != null)
                    {
                        Guid g;
                        // Create and display the value of two GUIDs.
                        g = Guid.NewGuid();

                        SAPbobsCOM.Documents oDoc = null;

                        oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDownPayments);

                        oDoc.CardCode = so.Customer.BPCode;
                        oDoc.CardName = so.CustomerName;
                        oDoc.DocDate = oTargetDoc.DocDate;
                        oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                        oDoc.UserFields.Fields.Item("U_SoDocNumber").Value = so.DocNum;
                        oDoc.DownPaymentType = DownPaymentTypeEnum.dptInvoice;
                        if (so.BillingAddressfield != null)
                        {
                            oDoc.Address = so.BillingAddressfield;
                        }

                        int cnt = 0;
                        foreach (SalesOrderDetails dtl in so.SalesOrderDetails)
                        {
                            cnt++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemDesc;
                            oDoc.Lines.Quantity = (double)dtl.Quantity;
                            oDoc.Lines.UnitPrice = (double)dtl.AdjustedPrice;
                            //if (oTargetDoc.PaymentType != null)
                            //{
                            //    oDoc.Lines.AccountCode = oTargetDoc.PaymentType.GLAccount;
                            //}
                            if (dtl.Location != null)
                            {
                                oDoc.Lines.WarehouseCode = dtl.Location.WarehouseCode;
                            }
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            if (dtl.SAPDocEntry != 0)
                            {
                                oDoc.Lines.BaseType = 17;
                                oDoc.Lines.BaseEntry = dtl.SAPDocEntry;
                                oDoc.Lines.BaseLine = dtl.SAPBaseLine;
                            }
                        }

                        int rc = oDoc.Add();
                        if (rc != 0)
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            SalesOrderCollection obj = osupdate.GetObjectByKey<SalesOrderCollection>(oTargetDoc.Oid);

                            SalesOrderCollectionDocStatus ds = osupdate.CreateObject<SalesOrderCollectionDocStatus>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.SalesOrderCollectionDocStatus.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: AR Downpayment Posting :" + oTargetDoc + "-" + temp);

                            return -1;
                        }
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesOrderCollection obj = osupdate.GetObjectByKey<SalesOrderCollection>(oTargetDoc.Oid);

                SalesOrderCollectionDocStatus ds = osupdate.CreateObject<SalesOrderCollectionDocStatus>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesOrderCollectionDocStatus.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: AR Downpayment Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostDPPaymenttoSAP(SalesOrderCollection oTargetDoc, SalesOrderCollectionDetails detail, string sodocnum, 
            IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap, int DPDocEntry)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                //foreach (SalesOrderCollectionDetails payment in oTargetDoc.SalesOrderCollectionDetails)
                //{
                    if (detail.SalesOrder == sodocnum)
                    {
                        IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace();
                        SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", sodocnum));

                        if (so != null)
                        {
                            Guid g;
                            // Create and display the value of two GUIDs.
                            g = Guid.NewGuid();

                            SAPbobsCOM.Payments oDoc = null;

                            oDoc = (SAPbobsCOM.Payments)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                            oDoc.CardCode = so.Customer.BPCode;
                            oDoc.CardName = so.CustomerName;
                            oDoc.DocDate = oTargetDoc.DocDate;
                            oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                            if (so.BillingAddressfield != null)
                            {
                                oDoc.Address = so.BillingAddressfield;
                            }

                            if (detail.PaymentAmount > 0)
                            {
                                if (oTargetDoc.PaymentType.PaymentMean == "CASH")
                                {
                                    oDoc.CashSum += Convert.ToDouble(detail.PaymentAmount);
                                    if (detail.GLAccount != null)
                                        oDoc.CashAccount = detail.GLAccount.AcctCode;
                                }

                                if (oTargetDoc.PaymentType.PaymentMean == "CCARD")
                                {
                                    oDoc.CreditCards.CreditSum += Convert.ToDouble(detail.PaymentAmount);
                                    oDoc.CreditCards.CreditAcct = detail.GLAccount.AcctCode;
                                    oDoc.CreditCards.PaymentMethodCode = oTargetDoc.PaymentType.CCardPayMethodCode;
                                    oDoc.CreditCards.VoucherNum = oTargetDoc.ReferenceNum;
                                    oDoc.CreditCards.CardValidUntil = DateTime.Parse("01/12/" + DateTime.Today.Year);
                                    oDoc.CreditCards.CreditCardNumber = oTargetDoc.CreditCardNum;
                                    oDoc.CreditCards.CreditCard = oTargetDoc.PaymentType.CCardPayMethodCode;
                                }

                                if (oTargetDoc.PaymentType.PaymentMean == "TRANSFER")
                                {
                                    oDoc.TransferSum += Convert.ToDouble(detail.PaymentAmount);
                                    oDoc.TransferAccount = detail.GLAccount.AcctCode;
                                    oDoc.TransferReference = oTargetDoc.ReferenceNum;
                                }

                                if (oTargetDoc.PaymentType.PaymentMean == "CHEQUE")
                                {
                                    oDoc.Checks.CheckSum += Convert.ToDouble(detail.PaymentAmount);
                                    oDoc.Checks.CheckAccount = detail.GLAccount.AcctCode;
                                    oDoc.Checks.BankCode = oTargetDoc.ChequeBank.BankCode;
                                    oDoc.Checks.CheckNumber = int.Parse(oTargetDoc.CheckNum);
                                }
                            }

                            oDoc.Invoices.DocEntry = DPDocEntry;
                            oDoc.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                            oDoc.Invoices.SumApplied = Convert.ToDouble(detail.Total);
                            oDoc.Invoices.Add();

                            int rc = oDoc.Add();
                            if (rc != 0)
                            {
                                string temp = sap.oCom.GetLastErrorDescription();
                                if (sap.oCom.InTransaction)
                                {
                                    sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                                }

                                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                                SalesOrderCollection obj = osupdate.GetObjectByKey<SalesOrderCollection>(oTargetDoc.Oid);

                                SalesOrderCollectionDocStatus ds = osupdate.CreateObject<SalesOrderCollectionDocStatus>();
                                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                                ds.CreateDate = DateTime.Now;
                                ds.DocStatus = DocStatus.PendPost;
                                ds.DocRemarks = "SAP Error:" + temp;
                                obj.SalesOrderCollectionDocStatus.Add(ds);

                                osupdate.CommitChanges();

                                WriteLog("[Error]", "Message: AR Downpayment Payment Posting :" + oTargetDoc + "-" + temp);

                                return -1;
                            }
                            return 1;
                        }
                    }
                    return 0;
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesOrderCollection obj = osupdate.GetObjectByKey<SalesOrderCollection>(oTargetDoc.Oid);

                SalesOrderCollectionDocStatus ds = osupdate.CreateObject<SalesOrderCollectionDocStatus>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesOrderCollectionDocStatus.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: AR Downpayment Payment Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostDOtoSAP(DeliveryOrder oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.Customer.BPName;
                    oDoc.DocDate = DateTime.Now;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    int cnt = 0;
                    foreach (DeliveryOrderDetails dtl in oTargetDoc.DeliveryOrderDetails)
                    {
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDescription = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.Price;
                        if (dtl.Warehouse != null)
                        {
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                        }
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                        if (dtl.Bin != null)
                        {
                            oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                            oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                        }

                        string getdoDocentry = "SELECT T1.DocEntry, T1.LineNum, T1.U_PortalLineOid " +
                         "From [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() + "]..ODLN T0 " +
                         "INNER join [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() + "]..DLN1 T1 on T0.DocEntry = T1.DocEntry " +
                         "WHERE U_PortalDocNum = '" + oTargetDoc.DocNum + "'";
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd1 = new SqlCommand(getdoDocentry, conn);
                        SqlDataReader reader1 = cmd1.ExecuteReader();
                        while (reader1.Read())
                        {
                            if (reader1.GetString(2) == dtl.Oid.ToString())
                            {
                                oDoc.Lines.BaseType = 15;
                                oDoc.Lines.BaseEntry = reader1.GetInt32(0);
                                oDoc.Lines.BaseLine = reader1.GetInt32(1);
                            }
                        }
                        conn.Close();

                        IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace();
                        SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", dtl.SODocNum));

                        if (so.Series.SeriesName == "Cash")
                        {
                            IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                            vwSeries series = fos.FindObject<vwSeries>(CriteriaOperator.Parse("SeriesName = ? and ObjectCode = ?",
                                "Cash", "13"));

                            if (series != null)
                            {
                                oDoc.Series = int.Parse(series.Series);
                            }
                        }
                        else
                        {
                            IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                            vwSeries series = fos.FindObject<vwSeries>(CriteriaOperator.Parse("SeriesName = ? and ObjectCode = ?",
                                "Term", "13"));

                            if (series != null)
                            {
                                oDoc.Series = int.Parse(series.Series);
                            }
                        }
                    }

                    string getdpDocentry = "SELECT T0.DocEntry, T0.DocTotal FROM [" + ConfigurationManager.AppSettings["CompanyDB"].ToString() + "]..ODPI T0 " +
                          "LEFT JOIN DeliveryOrderDetails T1 on T0.U_SoDocNumber = T1.SODocNum COLLATE DATABASE_DEFAULT " +
                          "WHERE T1.DeliveryOrder = " + oTargetDoc.Oid + " " +
                          "GROUP BY T0.DocEntry, T0.DocTotal";
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(getdpDocentry, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SAPbobsCOM.DownPaymentsToDraw dpm = oDoc.DownPaymentsToDraw;
                        dpm.DocEntry = reader.GetInt32(0);
                        dpm.AmountToDraw = (double)oTargetDoc.DeliveryOrderDetails.Sum(s => s.Total);
                        dpm.Add();
                    }
                    conn.Close();

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        DeliveryOrder obj = osupdate.GetObjectByKey<DeliveryOrder>(oTargetDoc.Oid);

                        DeliveryOrderDocTrail ds = osupdate.CreateObject<DeliveryOrderDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.DeliveryOrderDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Delivery Order Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                DeliveryOrder obj = osupdate.GetObjectByKey<DeliveryOrder>(oTargetDoc.Oid);

                DeliveryOrderDocTrail ds = osupdate.CreateObject<DeliveryOrderDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.DeliveryOrderDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Delivery Order Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostARDOtoSAP(DeliveryOrder oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);

                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.Customer.BPName;
                    oDoc.DocDate = DateTime.Now;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                    int cnt = 0;
                    foreach (DeliveryOrderDetails dtl in oTargetDoc.DeliveryOrderDetails)
                    {
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDescription = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.Price;
                        if (dtl.Warehouse != null)
                        {
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                        }
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                        if (dtl.Bin != null)
                        {
                            oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                            oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                        }

                        IObjectSpace os = ObjectSpaceProvider.CreateObjectSpace();
                        SalesOrder so = os.FindObject<SalesOrder>(CriteriaOperator.Parse("DocNum = ?", dtl.SODocNum));

                        foreach (SalesOrderDetails dtlsales in so.SalesOrderDetails)
                        {
                            if (dtlsales.SAPDocEntry != 0 && dtlsales.Oid.ToString() == dtl.SOBaseID)
                            {
                                oDoc.Lines.BaseType = 17;
                                oDoc.Lines.BaseEntry = dtlsales.SAPDocEntry;
                                oDoc.Lines.BaseLine = dtlsales.SAPBaseLine;
                            }
                        }
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        DeliveryOrder obj = osupdate.GetObjectByKey<DeliveryOrder>(oTargetDoc.Oid);

                        DeliveryOrderDocTrail ds = osupdate.CreateObject<DeliveryOrderDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.DeliveryOrderDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Delivery Order(DO) Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                DeliveryOrder obj = osupdate.GetObjectByKey<DeliveryOrder>(oTargetDoc.Oid);

                DeliveryOrderDocTrail ds = osupdate.CreateObject<DeliveryOrderDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.DeliveryOrderDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Delivery Order(DO) Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostPLtoSAP(PickList oTargetDoc, string warehouse, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (oTargetDoc.Sap)
                    return 0;

                IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                Guid g;
                // Create and display the value of two GUIDs.
                g = Guid.NewGuid();

                if (oTargetDoc.PickListAttachment != null && oTargetDoc.PickListAttachment.Count > 0)
                {
                    foreach (PickListAttachment obj in oTargetDoc.PickListAttachment)
                    {
                        string fullpath = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString() + g.ToString() + obj.File.FileName;
                        using (System.IO.FileStream fs = System.IO.File.OpenWrite(fullpath))
                        {
                            obj.File.SaveToStream(fs);
                        }
                    }
                }

                SAPbobsCOM.StockTransfer oDoc = null;

                oDoc = (SAPbobsCOM.StockTransfer)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                //oDoc.CardCode = oTargetDoc.Vendor.BoCode;
                //oDoc.CardName = oTargetDoc.Vendor.BoName;
                oDoc.DocDate = oTargetDoc.DocDate;
                oDoc.TaxDate = oTargetDoc.DeliveryDate;
                oDoc.Comments = oTargetDoc.Remarks;
                oDoc.FromWarehouse = warehouse;
                oDoc.ToWarehouse = warehouse;
                oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;

                int cnt = 0;
                int itemcount = 0;
                foreach (PickListDetailsActual dtl in oTargetDoc.PickListDetailsActual)
                {
                    if (dtl.Warehouse.WarehouseCode == warehouse)
                    {
                        if (dtl.FromBin.BinCode != dtl.ToBin.BinCode)
                        {
                            cnt++;
                            itemcount++;
                            if (cnt == 1)
                            {
                            }
                            else
                            {
                                oDoc.Lines.Add();
                                oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                            }

                            oDoc.Lines.FromWarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                            oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                            oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                            oDoc.Lines.ItemDescription = dtl.ItemCode.ItemName;
                            oDoc.Lines.Quantity = (double)dtl.PickQty;

                            if (dtl.FromBin != null)
                            {
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.FromBin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.PickQty;
                                oDoc.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                            }

                            if (dtl.ToBin != null)
                            {
                                oDoc.Lines.BinAllocations.Add();
                                oDoc.Lines.BinAllocations.BinAbsEntry = dtl.ToBin.AbsEntry;
                                oDoc.Lines.BinAllocations.Quantity = (double)dtl.PickQty;
                                oDoc.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                            }
                        }
                    }
                }
                if (oTargetDoc.PickListAttachment != null && oTargetDoc.PickListAttachment.Count > 0)
                {
                    cnt = 0;
                    SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)sap.oCom.GetBusinessObject(BoObjectTypes.oAttachments2);
                    foreach (PickListAttachment dtl in oTargetDoc.PickListAttachment)
                    {

                        cnt++;
                        if (cnt == 1)
                        {
                            if (oAtt.Lines.Count == 0)
                                oAtt.Lines.Add();
                        }
                        else
                            oAtt.Lines.Add();

                        string attfile = "";
                        string[] fexe = dtl.File.FileName.Split('.');
                        if (fexe.Length <= 2)
                            attfile = fexe[0];
                        else
                        {
                            for (int x = 0; x < fexe.Length - 1; x++)
                            {
                                if (attfile == "")
                                    attfile = fexe[x];
                                else
                                    attfile += "." + fexe[x];
                            }
                        }
                        oAtt.Lines.FileName = g.ToString() + attfile;
                        if (fexe.Length > 1)
                            oAtt.Lines.FileExtension = fexe[fexe.Length - 1];
                        string path = ConfigurationManager.AppSettings["B1AttachmentPath"].ToString();
                        path = path.Replace("\\\\", "\\");
                        path = path.Substring(0, path.Length - 1);
                        oAtt.Lines.SourcePath = path;
                        oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                    }
                    int iAttEntry = -1;
                    if (oAtt.Add() == 0)
                    {
                        iAttEntry = int.Parse(sap.oCom.GetNewObjectKey());
                    }
                    else
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        PickList obj = osupdate.GetObjectByKey<PickList>(oTargetDoc.Oid);

                        PickListDocTrail ds = osupdate.CreateObject<PickListDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.PickListDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Pick List Attachement Error :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    oDoc.AttachmentEntry = iAttEntry;
                }

                if (itemcount < 1)
                {
                    return 2;
                }

                int rc = oDoc.Add();
                if (rc != 0)
                {
                    string temp = sap.oCom.GetLastErrorDescription();
                    if (sap.oCom.InTransaction)
                    {
                        sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }

                    IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                    PickList obj = osupdate.GetObjectByKey<PickList>(oTargetDoc.Oid);

                    PickListDocTrail ds = osupdate.CreateObject<PickListDocTrail>();
                    ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                    ds.CreateDate = DateTime.Now;
                    ds.DocRemarks = "SAP Error:" + temp;
                    obj.PickListDocTrail.Add(ds);

                    osupdate.CommitChanges();

                    WriteLog("[Error]", "Message: Pick List Posting :" + oTargetDoc + "-" + temp);

                    return -1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                PickList obj = osupdate.GetObjectByKey<PickList>(oTargetDoc.Oid);

                PickListDocTrail ds = osupdate.CreateObject<PickListDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.PickListDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Pick List Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        // Start ver 1.0.7
        public int PostCNCanceltoSAP(ARDownpaymentCancel oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                if (!oTargetDoc.Sap)
                {
                    IObjectSpace fos = ObjectSpaceProvider.CreateObjectSpace();
                    Guid g;
                    // Create and display the value of two GUIDs.
                    g = Guid.NewGuid();

                    SAPbobsCOM.Documents oDoc = null;

                    oDoc = (SAPbobsCOM.Documents)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                    oDoc.CardCode = oTargetDoc.Customer.BPCode;
                    oDoc.CardName = oTargetDoc.Customer.BPName;
                    oDoc.DocDate = oTargetDoc.PostingDate;
                    oDoc.Comments = oTargetDoc.Remarks;
                    oDoc.UserFields.Fields.Item("U_PortalDocNum").Value = oTargetDoc.DocNum;
                    if (oTargetDoc.Reference != null)
                    {
                        oDoc.NumAtCard = oTargetDoc.Reference;
                    }
                    if (oTargetDoc.ContactPerson != null)
                    {
                        oDoc.SalesPersonCode = oTargetDoc.ContactPerson.SlpCode;
                    }

                    int cnt = 0;
                    foreach (ARDownpaymentCancelDetails dtl in oTargetDoc.ARDownpaymentCancelDetails)
                    {
                        cnt++;
                        if (cnt == 1)
                        {
                        }
                        else
                        {
                            oDoc.Lines.Add();
                            oDoc.Lines.SetCurrentLine(oDoc.Lines.Count - 1);
                        }

                        oDoc.Lines.ItemCode = dtl.ItemCode.ItemCode;
                        oDoc.Lines.ItemDescription = dtl.ItemDesc;
                        oDoc.Lines.Quantity = (double)dtl.Quantity;
                        oDoc.Lines.UnitPrice = (double)dtl.Price;
                        //oDoc.Lines.WithoutInventoryMovement = BoYesNoEnum.tYES;
                        if (dtl.Warehouse != null)
                        {
                            oDoc.Lines.WarehouseCode = dtl.Warehouse.WarehouseCode;
                        }
                        oDoc.Lines.UserFields.Fields.Item("U_PortalLineOid").Value = dtl.Oid.ToString();

                        if (dtl.Bin != null)
                        {
                            oDoc.Lines.BinAllocations.BinAbsEntry = dtl.Bin.AbsEntry;
                            oDoc.Lines.BinAllocations.Quantity = (double)dtl.Quantity;
                        }

                        if (dtl.BaseDoc != null)
                        {
                            oDoc.Lines.BaseType = 203;
                            oDoc.Lines.BaseEntry = int.Parse(dtl.BaseDoc);
                            oDoc.Lines.BaseLine = int.Parse(dtl.BaseId);
                        }
                    }

                    int rc = oDoc.Add();
                    if (rc != 0)
                    {
                        string temp = sap.oCom.GetLastErrorDescription();
                        if (sap.oCom.InTransaction)
                        {
                            sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                        }

                        IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                        SalesRefundRequests obj = osupdate.GetObjectByKey<SalesRefundRequests>(oTargetDoc.Oid);

                        SalesRefundReqDocTrail ds = osupdate.CreateObject<SalesRefundReqDocTrail>();
                        ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                        ds.CreateDate = DateTime.Now;
                        ds.DocStatus = DocStatus.PendPost;
                        ds.DocRemarks = "SAP Error:" + temp;
                        obj.SalesRefundReqDocTrail.Add(ds);

                        osupdate.CommitChanges();

                        WriteLog("[Error]", "Message: Sales Refund Posting :" + oTargetDoc + "-" + temp);

                        return -1;
                    }
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                SalesRefundRequests obj = osupdate.GetObjectByKey<SalesRefundRequests>(oTargetDoc.Oid);

                SalesRefundReqDocTrail ds = osupdate.CreateObject<SalesRefundReqDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.SalesRefundReqDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: Sales Refund Posting :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }

        public int PostCancelPaymenttoSAP(ARDownpaymentCancel doc, int oTargetDoc, IObjectSpaceProvider ObjectSpaceProvider, SAPCompany sap)
        {
            // return 0 = post nothing
            // return -1 = posting error
            // return 1 = posting successful
            try
            {
                Guid g;
                // Create and display the value of two GUIDs.
                g = Guid.NewGuid();

                SAPbobsCOM.Payments oDoc = null;

                oDoc = (SAPbobsCOM.Payments)sap.oCom.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                oDoc.GetByKey(oTargetDoc);

                if (oDoc.Cancelled == BoYesNoEnum.tNO)
                {
                    int rc = oDoc.Cancel();
                    if (rc != 0)
                    {
                        {
                            string temp = sap.oCom.GetLastErrorDescription();
                            if (sap.oCom.InTransaction)
                            {
                                sap.oCom.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }

                            IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                            ARDownpaymentCancel obj = osupdate.GetObjectByKey<ARDownpaymentCancel>(doc.Oid);

                            ARDownpaymentCancellationDocTrail ds = osupdate.CreateObject<ARDownpaymentCancellationDocTrail>();
                            ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                            ds.CreateDate = DateTime.Now;
                            ds.DocStatus = DocStatus.PendPost;
                            ds.DocRemarks = "SAP Error:" + temp;
                            obj.ARDownpaymentCancellationDocTrail.Add(ds);

                            osupdate.CommitChanges();

                            WriteLog("[Error]", "Message: AR Downpayment Cancel :" + oTargetDoc + "-" + temp);

                            return -1;
                        }
                    }
                    return 1;
                }
                return 2;
            }
            catch (Exception ex)
            {
                IObjectSpace osupdate = ObjectSpaceProvider.CreateObjectSpace();
                ARDownpaymentCancel obj = osupdate.GetObjectByKey<ARDownpaymentCancel>(doc.Oid);

                ARDownpaymentCancellationDocTrail ds = osupdate.CreateObject<ARDownpaymentCancellationDocTrail>();
                ds.CreateUser = osupdate.GetObjectByKey<ApplicationUser>(Guid.Parse("100348B5-290E-47DF-9355-557C7E2C56D3"));
                ds.CreateDate = DateTime.Now;
                ds.DocStatus = DocStatus.PendPost;
                ds.DocRemarks = "SAP Error:" + ex.Message;
                obj.ARDownpaymentCancellationDocTrail.Add(ds);

                osupdate.CommitChanges();

                WriteLog("[Error]", "Message: AR Downpayment Cancel :" + oTargetDoc + "-" + ex.Message);

                return -1;
            }
        }
        // End ver 1.0.7
    }
}
