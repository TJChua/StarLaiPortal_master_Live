using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.XtraGrid.Views.Grid;
using StarLaiPortal.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarLaiPortal.Module.Web.Editors
{
    public partial class DynamicListViewEditor : ViewController
    {
        private ASPxGridListEditor gridListEditor;
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public DynamicListViewEditor()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        //    protected override void OnActivated()
        //    {
        //        base.OnActivated();
        //        // Perform various tasks depending on the target View.
        //    }
        //    protected override void OnViewControlsCreated()
        //    {
        //        base.OnViewControlsCreated();
        //        // Access and customize the target View control.
        //        gridListEditor = View.Editor as ASPxGridListEditor;
        //        if (gridListEditor != null)
        //        {
        //            InitColumns();
        //        }
        //    }

        //    private void InitColumns()
        //    {
        //        string allfield = null;
        //        int i = 0;
        //        foreach (ItemInquiryField inquiryfield in View.ObjectSpace.CreateCollection(typeof(ItemInquiryField), CriteriaOperator.Parse("IsActive = ?", true)))
        //        {
        //            if (allfield == null)
        //            {
        //                allfield = inquiryfield.ColumnName.ToString() + "|" + inquiryfield.DataType;
        //            }
        //            else
        //            {
        //                allfield = allfield + "," + inquiryfield.ColumnName.ToString() + "|" + inquiryfield.DataType;
        //            }
        //        }

        //        string[] field = allfield.Split(',');
        //        foreach (string node_id in field)
        //        {
        //            string[] datatype = node_id.Split('|');

        //            GridViewDataColumn data_column = new GridViewDataSpinEditColumn();

        //            if (datatype[1] == DataType.String.ToString())
        //            {
        //                data_column.UnboundType = DevExpress.Data.UnboundColumnType.String;
        //            }
        //            else if (datatype[1] == DataType.Decimal.ToString())
        //            {
        //                data_column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
        //            }
        //            else if(datatype[1] == DataType.Int.ToString())
        //            {
        //                data_column.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
        //            }
        //            else if(datatype[1] == DataType.DateTime.ToString())
        //            {
        //                data_column.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
        //            }

        //            data_column.FieldName = datatype[0];
        //            data_column.Caption = datatype[0];
        //            data_column.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
        //            data_column.ReadOnly = false;
        //            data_column.MinWidth = 20;
        //            data_column.Settings.AllowSort = DevExpress.Utils.DefaultBoolean.True;
        //            data_column.ShowInCustomizationForm = false;
        //            data_column.Settings.AllowFilterBySearchPanel = DevExpress.Utils.DefaultBoolean.True;
        //            gridListEditor.Grid.Columns.Add(data_column);
        //        }
        //    }

        //    protected override void OnDeactivated()
        //    {
        //        // Unsubscribe from previously subscribed events and release other references and resources.
        //        base.OnDeactivated();
        //    }
    }
}
