<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SQLToCalendarWeb.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title></title>
    <link rel="Stylesheet" href="../Styles/MetroJs.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/RS.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            RS.Styling.ApplyMasterPage();
            RS.Styling.ApplyMetroTiles("#tile1, #tile2");
            RS.BuildDOM.CorrectRelativeLinks();
        });
    </script>
    <script type="text/javascript" src="../Scripts/MetroJs.js"></script>
</head>
<body style="display: none">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="false" EnablePartialRendering="true" EnableScriptGlobalization="false" EnableScriptLocalization="true" />
        <div id="chrome_ctrl_placeholder"></div>
        <h1 class="ms-accentText">Application Settings</h1>
        <div id="MainContent">
            <div class="live-tile" id="tile1" data-mode="slide" data-speed="600" data-start-now="false" data-stops="0px, 70%" data-repeat="0" data-stack="false">
                <div style="background-color: rgba(0,0,0,0.6)">
                <a class="full" href="{SPAppWebUrl}/lists/SQL Configuration?{StandardTokens}" style="text-decoration: none;">
                    <p>SQL Configuration</p>
                    <span class="tile-title">Configure the SQL database options for this app</span>
                </a>
            </div>
            <div style="background-color: #0072c6">
                <a class="full" href="{SPAppWebUrl}/lists/SQL Configuration?{StandardTokens}" style="text-decoration: none">
                <img alt="SQL Configuration" src="../Images/data_configuration.png" style="width: 140px; height: 140px; padding: 10px;" />
                    </a>
            </div>
            </div>
            <div class="live-tile" id="tile2" data-mode="slide" data-speed="600" data-start-now="false" data-stops="0px, 70%" data-repeat="0" data-stack="false">
                <div style="background-color: rgba(0,0,0,0.6)">
                <a class="full" href="SQLToCalendar.aspx?{StandardTokens}" style="text-decoration: none;">
                    <p>Full Page Preview</p>
                    <span class="tile-title">View a full page preview of the calendar</span>
                </a>
            </div>
            <div style="background-color: #0072c6">
                <a class="full" href="SQLToCalendar.aspx?{StandardTokens}" style="text-decoration: none">
                <img alt="Full Page Preview" src="../Images/Full Screen.png" style="width: 140px; height: 140px; padding: 10px;" />
                    </a>
            </div>
            </div>
        </div>

        <div id="loadingDialog" title="Loading..." style="display: none; text-align: center;">
            <img alt="Loading..." src="../Images/Loading.gif" />
        </div>
    </form>
</body>
</html>
