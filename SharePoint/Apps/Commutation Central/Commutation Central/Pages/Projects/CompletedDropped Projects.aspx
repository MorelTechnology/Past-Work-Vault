<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script type="text/javascript" src="../../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/accounting.js/0.4.1/accounting.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.formatCurrency-1.4.0.min.js"></script>
    <link rel="Stylesheet" href="../../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" type="text/css" href="../../Content/ItemForm.css" />
    <script type="text/javascript" src="../../Scripts/Projects/CompletedDropped Projects.js"></script>
</asp:Content>

<%-- The markup in the following Content element will be placed in the TitleArea of the page --%>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Projects
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
    <asp:Menu ID="quickLaunch" runat="server" MaximumDynamicDisplayLevels="1" Orientation="Vertical" StaticDisplayLevels="3" CssClass="RivernetQuickLaunch">
        <Items>
            <asp:MenuItem></asp:MenuItem>

        </Items>
    </asp:Menu>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <div style="display:table; width: 100%">
        <p>
            <a title="Add a new item to this list or library." class="ms-heroCommandLink" id="idHomePageNewItem" onclick='NewProject()' href="javascript:void(0)"><span class="ms-list-addnew-imgSpan20"><img class="ms-list-addnew-img20" id="idHomePageNewItem-img" src="/_layouts/15/images/spcommon.png?rev=23"></span><span>new item</span></a>
        </p>
        <div style="display:none; color:red;" class="errorCell" id="errorCell">Error</div>
        <h2>Projects</h2>
        <table id="projects" class="display">
            <thead>
                <tr>
                    <th style="text-align: left">ID</th>
                    <th style="text-align: left">Counterparty Name</th>
                    <th style="text-align: left">Primary Manager</th>
                    <th style="text-align: left">Secondary Manager</th>
                    <th style="text-align: left">Lead Office</th>
                    <th style="text-align: left">Assigned Date</th>
                    <th style="text-align: left">Commutation Status</th>
                    <th style="text-align: left">Financial Authority</th>
                </tr>
            </thead>
            <tbody style="text-align: left">
            </tbody>
        </table>
    </div>

    <div id="loadingdialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display: none; text-align: center;">
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
</asp:Content>
