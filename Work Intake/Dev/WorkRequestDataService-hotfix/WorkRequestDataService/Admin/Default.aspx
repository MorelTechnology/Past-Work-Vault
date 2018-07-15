<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WorkRequestDataService.Admin.Default" EnableEventValidation="true" %>

<%@ Import Namespace="System.Web.Security" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DSWR Application Admin</title>
    <script type="text/javascript" src="../Scripts/jquery-1.12.4.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.12.1.min.js"></script>
    <link type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.0/themes/smoothness/jquery-ui.css" rel="stylesheet" />

    <script type="text/javascript">
        $(function () {
            $("#adminTabs").tabs
                ({
                    show: { effect: "fadeIn", duration: 300, delay: 10 },
                    hide: { effect: "fadeOut", duration: 300, delay: 10 }
                });
            $("#EditDenials").load("EditDenials.aspx");
            $("#EditSettings").load("EditSettings.aspx");
            <%if (IsEnvironmentAdmin())
        {%>
            $("#EditConfig").load("EditConfig.aspx");
            <%}%>
        });
    </script>
    <style type="text/css">
        .auto-style1 {
            text-align: center !important;
            width:100% !important;
        }
    </style>
</head>
<body>
    <div id="adminTabs" style="font-size: 14px !important;">
        <ul>
            <li><a href="#EditDenials">Denied Requests</a></li>
            <li><a href="#EditSettings">Settings</a></li>
            <%if (IsEnvironmentAdmin())
                {%>
            <li><a href="#EditConfig">Application Configuration</a></li>
            <%}%>
        </ul>
        <asp:Panel ID="EditDenials" runat="server">
            <img src="../Content/images/loading.gif" alt="Loading" />
        </asp:Panel>
        <asp:Panel ID="EditSettings" runat="server">
            <img src="../Content/images/loading.gif" alt="Loading" />
        </asp:Panel>
        <%if (IsEnvironmentAdmin())
            {%>
        <asp:Panel ID="EditConfig" runat="server">
            <img src="../Content/images/loading.gif" alt="Loading" />
        </asp:Panel>
        <%}%>
    </div>
</body>
</html>