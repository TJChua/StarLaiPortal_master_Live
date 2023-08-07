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
using StarLaiPortal.Module.BusinessObjects.Item_Inquiry;
using StarLaiPortal.Module.BusinessObjects.Print_Module;
using StarLaiPortal.Module.BusinessObjects.Reports;
using StarLaiPortal.Module.BusinessObjects.Sales_Order_Collection;
using StarLaiPortal.Module.BusinessObjects.Setup;
using StarLaiPortal.Module.BusinessObjects.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NavigationControllers : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public NavigationControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "ItemInquiry_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ItemInquiry));
                ItemInquiry iteminquiry = objectSpace.FindObject<ItemInquiry>(new BinaryOperator("Oid", 1));

                DocTypes number = objectSpace.FindObject<DocTypes>(new BinaryOperator("BoCode", DocTypeList.SO));

                iteminquiry.Search = null;
                iteminquiry.Cart = number.NextDocNum.ToString();
                iteminquiry.PriceList1 = null;
                iteminquiry.PriceList2 = null;
                iteminquiry.Stock1 = null;
                iteminquiry.Stock2 = null;

                for (int i = 0; iteminquiry.ItemInquiryDetails.Count > i;)
                {
                    iteminquiry.ItemInquiryDetails.Remove(iteminquiry.ItemInquiryDetails[i]);
                }

                DetailView detailView = Application.CreateDetailView(objectSpace, iteminquiry);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;

                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "PrintLabel_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                PrintLabel newprintlabel = objectSpace.CreateObject<PrintLabel>();

                DetailView detailView = Application.CreateDetailView(objectSpace, newprintlabel);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;

                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "SalesOrderCollectionReport_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                SalesOrderCollectionReport newsalescollectionreport = objectSpace.CreateObject<SalesOrderCollectionReport>();

                DetailView detailView = Application.CreateDetailView(objectSpace, newsalescollectionreport);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;

                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "StockReorderingReport_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                StockReorderingReport newstockreorder = objectSpace.CreateObject<StockReorderingReport>();

                DetailView detailView = Application.CreateDetailView(objectSpace, newstockreorder);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;

                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "ItemInquiry_ListView_Report")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                ItemInquiry newiteminquiry = objectSpace.CreateObject<ItemInquiry>();

                DetailView detailView = Application.CreateDetailView(objectSpace, "ItemInquiry_DetailView_Report", true, newiteminquiry);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                ItemInquiryDefault defaultdata = objectSpace.FindObject<ItemInquiryDefault>(CriteriaOperator.Parse("DocType = ? and IsActive= ?",
                    DocTypeList.Reports, "True"));

                if (defaultdata != null)
                {
                    if (defaultdata.PriceList1 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).PriceList1 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwPriceList>
                            (defaultdata.PriceList1.ListNum);
                    }
                    if (defaultdata.PriceList2 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).PriceList2 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwPriceList>
                            (defaultdata.PriceList2.ListNum);
                    }
                    if (defaultdata.PriceList3 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).PriceList3 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwPriceList>
                            (defaultdata.PriceList3.ListNum);
                    }
                    if (defaultdata.PriceList4 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).PriceList4 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwPriceList>
                            (defaultdata.PriceList4.ListNum);
                    }
                    if (defaultdata.Stock1 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).Stock1 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwWarehouse>
                            (defaultdata.Stock1.WarehouseCode);
                    }
                    if (defaultdata.Stock2 != null)
                    {
                        ((ItemInquiry)detailView.CurrentObject).Stock2 = ((ItemInquiry)detailView.CurrentObject).Session.GetObjectByKey<vwWarehouse>
                            (defaultdata.Stock2.WarehouseCode);
                    }
                }

                objectSpace.CommitChanges();
                objectSpace.Refresh();

                e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                e.Handled = true;
            }
        }
    }
}
