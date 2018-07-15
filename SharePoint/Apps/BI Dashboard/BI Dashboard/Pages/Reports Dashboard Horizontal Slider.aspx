<%@ Page Language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=9; IE=8; IE=EDGE" />
    <title></title>

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script type="text/javascript" src="/_layouts/15/SP.Taxonomy.js"></script>
    <script type="text/javascript" src="/_layouts/15/SP.RequestExecutor.js"></script>

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

    <script type="text/javascript" src="../Content/Jssor.Slider/js/jssor.js"></script>
    <script type="text/javascript" src="../Content/Jssor.Slider/js/jssor.slider.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../Scripts/RS.js"></script>
    <script type="text/javascript" src="../Scripts/Reports Dashboard Horizontal Slider.js"></script>

    <link rel="Stylesheet" href="../Content/App.css" />
    <link rel="Stylesheet" href="../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
</head>
<body>
    <div id="content">
        <div id="departmentslider_container" style="position: relative; top: 0px; left: 0px; width: 1220px; height: 500px; background: #fff; overflow: hidden">
            <div u="slides" style="position: relative; left: 0px; top: 29px; width: 1200px; height: 460px; border: 1px solid gray; -webkit-filter: blur(0px); background-color: #fff; overflow: hidden;">
            </div>

            <div u="thumbnavigator" class="jssort12" style="position: relative; width: 950px; height: 30px; left: 0px; top: 0px;">
                <div u="slides" style="top: 0px; left: 0px; border-left: 1px solid gray;">
                    <div u="prototype" class="p" style="position: relative; width: 100px; height: 30px; top: 0; left: 0; padding: 0px;">
                        <div class="w">
                            <div u="thumbnailtemplate" class="c" style="width: 100%; height: 100%; position: relative; top: 0; left: 0; line-height: 28px; text-align: center;"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="loadingDialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../Images/Loading.gif" />
    </div>
</body>
</html>
