<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title></title>

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script type="text/javascript" src="/_layouts/15/SP.Taxonomy.js"></script>

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

    <script type="text/javascript" src="../Scripts/RS.js"></script>
    <script type="text/javascript" src="../Scripts/Reports Dashboard.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>

    <link rel="Stylesheet" href="../Content/App.css" />
    <link rel="Stylesheet" href="../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
</head>
<body>
    <div id="content">
        <div id="error" style="display: none; color: red;"></div>
        <div id="dropDownSelection" style="font-size: 18px; font-weight: bold;">
            Select a department:
            <select id="departmentDropDown" runat="server" style="font-size: medium;">
                <option value="">Select a department...</option>
            </select>
        </div>
        <div id="formTabs" style="display: none; min-width: 660px;">
            
            <ul>
                <li><a href="#currentTab">Current</a></li>
                <li><a href="#historyTab">History</a></li>
            </ul>
            <div id="currentTab">
                <table id="currentReportsContainer">
                    <tr>
                        <td style="vertical-align: top;">
                            <table id="currentReports" class="display" style="min-width: 480px;">
                                <thead>
                                    <tr>
                                        <th style="text-align: left">Report Name</th>
                                        <th style="text-align: left; min-width: 230px">Most Recent Distribution</th>
                                        <th>Report URL</th>
                                    </tr>
                                </thead>
                                <tbody style="text-align: left;">
                                </tbody>
                            </table>
                        </td>
                        <td id="currentReportCell" style="min-width: 100px; vertical-align: top">
                            <div id="currentReportDetails" style="margin: 20px; border: thin black solid; display: none;">
                                <p style="margin-left: 5px">Report Description:</p>
                                <p id="currentReportDescription" style="margin-left: 5px"></p>
                            </div>
                            <div id="currentReportImage" style="margin: 20px; border: thin black solid; display: none; max-width: 400px; overflow: scroll;">
                                <p style="margin-left: 5px">Report Preview:</p>
                                <p id="currentReportPreview" style="margin-left: 5px">
                                    <a id="currentReportLink" href="">
                                        <img id="currentReportImg" src="/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png" /></a>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="historyTab">
                <table id="historicalReportsContainer">
                    <tr>
                        <td style="vertical-align: top">
                            <table id="historicalReports" class="display" style="min-width: 480px">
                                <thead>
                                    <tr>
                                        <th style="text-align: left">Report Name</th>
                                    </tr>
                                </thead>
                                <tbody style="text-align: left">
                                </tbody>
                            </table>
                        </td>
                        <td id="historicalReportCell" style="min-width: 500px; vertical-align: top">
                            <div id="historicalReportsDetail" style="display: none; border: thin black solid;">
                                <p>Report History:</p>
                                <table id="historicalReportsTable" class="display">
                                    <thead>
                                        <tr>
                                            <th style="text-align: left">Filename</th>
                                            <th style="text-align: left">Valuation Date</th>
                                            <th style="text-align: left">Distribution Date</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="loadingDialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../Images/Loading.gif" />
    </div>
</body>
</html>
