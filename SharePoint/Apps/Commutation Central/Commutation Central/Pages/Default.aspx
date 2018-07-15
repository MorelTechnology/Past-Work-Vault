<%-- The following 4 lines are ASP.NET directives needed when using SharePoint components --%>

<%@ Page Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" MasterPageFile="~masterurl/default.master" Language="C#" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%-- The markup and script in the following Content element will be placed in the <head> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script type="text/javascript" src="../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/accounting.js/0.4.1/accounting.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.8.3/moment.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.formatCurrency-1.4.0.min.js"></script>
    <link rel="Stylesheet" href="../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" type="text/css" href="../Content/ItemForm.css" />
    <script type="text/javascript" src="../Scripts/Default.js"></script>
</asp:Content>

<%-- The markup in the following Content element will be placed in the TitleArea of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Commutation Central Dashboard
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
    <asp:Menu ID="quickLaunch" runat="server" MaximumDynamicDisplayLevels="1" Orientation="Vertical" StaticDisplayLevels="3" CssClass="RivernetQuickLaunch">
        <Items>
            <asp:MenuItem></asp:MenuItem>

        </Items>
    </asp:Menu>
</asp:Content>

<%-- The markup and script in the following Content element will be placed in the <body> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="display:table; width: 100%">
        <div style="display:none; color:red;" class="errorCell" id="errorCell">Error</div>
        <h2>Projects</h2>
        <table id="projects" class="display">
            <thead>
                <tr>
                    <th style="width: 0px"></th>
                    <th style="width: 0px"></th>
                    <th style="width: 0px"></th>
                    <th style="text-align: left;">Counterparty Name</th>
                    <th style="text-align: left;">Primary Manager</th>
                    <th style="text-align: left;">Secondary Manager</th>
                    <th style="text-align: left;">Lead Office</th>
                    <th style="text-align: left;">Assigned Date</th>
                    <th style="text-align: left;">Last Modified</th>
                    <th style="text-align: left;">Commutation Status</th>
                    <th style="text-align: left;">Total Assumed</th>
                    <th style="text-align: left;">Total Ceded</th>
                    <th style="text-align: left;">Total Net</th>
                </tr>
            </thead>
            <tbody style="text-align: left">
            </tbody>
        </table>
        <div>&nbsp;</div>
        <h2>Activities</h2>
        <table id="activities" class="display">
            <thead>
                <tr>
                    <th style="width: 0px"></th>
                    <th style="text-align: left">Activity Title</th>
                    <th style="text-align: left">Counterparty Name</th>
                    <th style="text-align: left">Assigned To</th>
                    <th style="text-align: left">Created By</th>
                    <th style="text-align: left">Activity Category</th>
                    <th style="text-align: left;">Due Date</th>
                    <th style="text-align: left;">Entry Date</th>
                    <th style="text-align: left">Activity Status</th>
                </tr>
            </thead>
            <tbody style="text-align: left">
            </tbody>
        </table>
    </div>

    <div id="loadingdialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display: none; text-align: center;">
        <img alt="Saving..." src="../Images/uploading.gif" />
    </div>

</asp:Content>
