<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE" />
    <title>GAIT Dashboard</title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.10/css/dataTables.jqueryui.min.css" />
    <link rel="stylesheet" href="../Content/App.css" />
    <link rel="stylesheet" href="../Content/MetroJs.min.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <SharePoint:ScriptLink ID="ScriptLink1" Name="init.js" runat="server" OnDemand="false" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink2" Name="sp.init.js" runat="server" OnDemand="false" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink4" Name="sp.core.js" runat="server" OnDemand="false" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink7" Name="sp.ui.dialog.js" runat="server" OnDemand="false" LoadAfterUI="true" Localizable="false" />
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
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/dataTables.jqueryui.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.10.6/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/numeral.js/1.4.5/numeral.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
    <script type="text/javascript" src="../Scripts/MetroJs.min.js"></script>
    <script type="text/javascript" src="../Scripts/RS.GAIT.js"></script>
    <script type="text/javascript" src="../Scripts/GAITDashboard/GAITDashboard.js"></script>
    <script type="text/javascript" src="../Scripts/InitiativesDashboard/InitiativesDashboard.js"></script>
    <script type="text/javascript" src="../Scripts/WorkMattersDashboard/WorkMattersDashboard.js"></script>
</head>
<body>
    <div id="Content" class="maincontent">
        <div id="DashboardTabs">
            <ul>
                <li><a href="#InitiativesDashboard">Initiatives</a></li>
                <li><a href="#WorkMatterDashboard">Work Matters</a></li>
                <li><a href="#AdminControlPanel">Admin Control Panel</a></li>
            </ul>
            <div id="InitiativesDashboard" class="dashboard">
                <table id="tblInitiativesDashboard" class="display" style="display: none">
                    <thead>
                        <tr>
                            <th colspan="5">Initiative</th>
                            <th colspan="5">Task</th>
                        </tr>
                        <tr>
                            <th>Short Name</th>
                            <th>Title</th>
                            <th>Initiative Owner(s)</th>
                            <th>Date Plan Approved</th>
                            <th>Initiative Status</th>
                            <th>Next Scheduled Task/Milestone</th>
                            <th>Task Owner(s)</th>
                            <th>Due Date</th>
                            <th>Milestone Status</th>
                            <th>Status Comment</th>
                        </tr>
                    </thead>
                </table>
                <div id="InitiativeActions" class="slidetiles" style="min-height:200px;">

                </div>
            </div>
            <div id="WorkMatterDashboard" class="dashboard">
                <table id="tblWorkMattersDashboard" class="display" style="display: none;">
                    <thead>
                        <tr>
                            <th colspan="4">Work Matter</th>
                            <th colspan="6">Task</th>
                        </tr>
                        <tr>
                            <th>WM Name</th>
                            <th>WM No.</th>
                            <th>WM Owner</th>
                            <th>Portfolio(s)</th>
                            <th>Next Milestone/Task</th>
                            <th>Task Owner</th>
                            <th>Due Date</th>
                            <th>Status</th>
                            <th>Issue Category</th>
                            <th>Comments</th>
                        </tr>
                    </thead>
                </table>
                <div id="WorkMatterActions" class="slidetiles" style="min-height:200px;">

                </div>
            </div>
            <div id="AdminControlPanel" class="slidetiles controlpanel" style="min-height:200px;min-width:1200px;">

            </div>
        </div>
        <div id="dialog-message" title="Working...">
            <img src="../Images/loading.gif" />
        </div>
    </div>
</body>
</html>
