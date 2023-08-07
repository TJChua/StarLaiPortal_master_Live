<%@ Page Language="C#" AutoEventWireup="true" Inherits="Default" EnableViewState="false"
    ValidateRequest="false" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.2, Version=22.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.2, Version=22.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Main Page</title>
    <meta http-equiv="Expires" content="0" />
</head>
<body class="VerticalTemplate">
    <style>
        .Caption
        {
            color:black !important;
            font-weight:bold !important;
        }
        .dxgvHeader_XafTheme
        {
            color:black !important;
            font-weight:bold !important;
        }
        .dx-vam
        {
            font-weight:initial !important;
        }
        .dxnb-ghtext
        {
            color:black !important;
        }
        .dxnb-item
        {
            font-weight:bold !important;
        }
        .dx-wrap
        {
            font-weight:bold !important;
        }
        .dxnb-ghtext
        {
            font-weight:initial !important;
        }
/*        .dxeEditArea_XafTheme.dxeDisabled_XafTheme
        {
            background-color:antiquewhite;
        }
        .dxeDisabled_XafTheme, .dxeDisabled_XafTheme td.dxe
        {
            background-color:antiquewhite;
        }*/
/*      .dxgvTable_XafTheme
        {
              background-color:lightyellow !important;
        }*/
    </style>
    <form id="form2" runat="server">
    <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
    <div runat="server" id="Content" />
    </form>
</body>
</html>