<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/Contacts/AttachItem.js"></script>
    <script type="text/javascript" src="../../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <link rel="Stylesheet" href="../../Content/ItemForm.css" />
    <link rel="Stylesheet" href="../../Content/jQueryUI/StyleSheets/jquery-ui.css" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <table class="parentTable" id="attachItemContent">
        <tr>
            <td class="leftBorderColumn" rowspan="2" style="min-width: 30px;"></td>
            <td class="formHeaderRow" style="width: auto; min-width: 930px;">
                <h1>Assign Contact to Project</h1>
            </td>
        </tr>
        <tr>
            <td class="formMainContent">
                <div class="contentRow"></div>
                <div class="contentRow">
                    <table class="childTable" style="width: 100%">
                        <col width="150" />
                        <tr>
                            <td class="subHeaderRow" colspan="2">
                                <h2>Contact Information</h2>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Contact Name</h4>
                            </td>
                            <td class="contentCell">
                                <select id="contactId" style="width:100%"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <button type="button" id="btnSubmit" onclick="SubmitForm()">Attach</button>
                <button type="button" id="btnCancel" onclick="CancelSubmit()">Cancel</button>
            </td>
        </tr>
    </table>
    <div id="loadingdialog" title="Loading..." style="display: none; text-align: center;">
        <img alt="Loading..." src="../../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display: none; text-align: center;">
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
</asp:Content>
