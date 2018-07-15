<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SQLToCalendar.aspx.cs" Inherits="SQLToCalendarWeb.Pages.SQLToCalendar" Async="true" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title></title>
    <script type="text/javascript">
        // Set the style of the client web part page to be consistent with the host web.
        (function () {
            'use strict';

            var hostUrl = '';
            if (document.URL.indexOf('?') != -1) {
                var params = document.URL.split('?')[1].split('&');
                for (var i = 0; i < params.length; i++) {
                    var p = decodeURIComponent(params[i]);
                    if (/^SPHostUrl=/i.test(p)) {
                        hostUrl = p.split('=')[1];
                        document.write('<link rel="stylesheet" href="' + hostUrl + '/_layouts/15/defaultcss.ashx" />');
                        break;
                    }
                }
            }
            if (hostUrl == '') {
                document.write('<link rel="stylesheet" href="/_layouts/15/1033/styles/themable/corev15.css" />');
            }
        })();
    </script>
    <link href="../Addins/jQueryUI/Stylesheets/jquery-ui.css" rel="stylesheet" />
    <link href="../Styles/fullcalendar.css" rel="stylesheet" />
    <link href="../Styles/fullcalendar.print.css" rel="stylesheet" media="print" />
    <script src="../Scripts/moment.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/fullcalendar.min.js" type="text/javascript"></script>
    <script src="../Scripts/RS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            RS.Styling.RenderCalendar("calendar");
            RS.BuildDOM.doneLoading();
            $("#ddlDepartment").change(function () {
                RS.BuildDOM.loading("content", true, "center", "top");
            });
        });
    </script>
</head>
<body>
    <form runat="server">
        <div id="content">
            <input type="hidden" id="eventsJsonArray" runat="server" />
            <input type="hidden" id="departmentsJsonArray" runat="server" />
            <div>
                <div><span class="ms-h2"><img src="../Images/calendar.jpg" style="height: 32px; width: 32px; margin: 0px 5px;" /><a style="vertical-align: top;" href="../FileRef/Time Off.oft">Click here to add an event to the Time Off Calendar!</a></span></div>
                <div><span class="ms-h2"><img src="../Images/outlook.jpg" style="height: 32px; width: 32px; margin: 0px 5px;" /><a style="vertical-align: top;" href="../FileRef/TimeOff.bat">Click here to connect the Time Off Calendar to Outlook!</a></span></div>
                <span class="ms-h3">Filter by department: </span>
                <asp:DropDownList ID="ddlDepartment" runat="server" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <div id="legend">
                <span class="ms-h3">Legend:</span>
                <span id="legendAcquisitions" style="display:none"><span style="background-color: #335ba5;">&nbsp;&nbsp;&nbsp;</span><span> = Acquisitions</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendActuarial" style="display:none"><span style="background-color: #0d830e;">&nbsp;&nbsp;&nbsp;</span><span> = Actuarial</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendClaims" style="display:none"><span style="background-color: #1793ea;">&nbsp;&nbsp;&nbsp;</span><span> = Claims</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendCommutations" style="display:none"><span style="background-color: #00282c;">&nbsp;&nbsp;&nbsp;</span><span> = Commutations</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendEXCO" style="display:none"><span style="background-color: #d11202;">&nbsp;&nbsp;&nbsp;</span><span> = EXCO</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendFinance" style="display:none"><span style="background-color: #94b111;">&nbsp;&nbsp;&nbsp;</span><span> = Finance</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendHumanResources" style="display:none"><span style="background-color: #eb17ae;">&nbsp;&nbsp;&nbsp;</span><span> = Human Resources</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendOfficeOfGeneralCounsel" style="display:none"><span style="background-color: #bb2170;">&nbsp;&nbsp;&nbsp;</span><span> = Office of General Counsel</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendOperations" style="display:none"><span style="background-color: #597628;">&nbsp;&nbsp;&nbsp;</span><span> = Operations</span>&nbsp;&nbsp;&nbsp;</span>
                <span id="legendReinsurance" style="display:none"><span style="background-color: #a97b9e;">&nbsp;&nbsp;&nbsp;</span><span> = Reinsurance</span>&nbsp;&nbsp;&nbsp;</span>
            </div>
            <div id="calendar" style="padding: 10px;"></div>
        </div>

        <div id="loadingDialog" title="Loading..." style="display: none; text-align: center;">
            <img alt="Loading..." src="../Images/Loading.gif" />
        </div>
    </form>
</body>
</html>
