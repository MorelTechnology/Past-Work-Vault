<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.validate.js"></script>
    <script type="text/javascript" src="../../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <SharePoint:ScriptLink Name="datepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../Scripts/Projects/NewItem.js"></script>
    <script type="text/javascript" src="../../Scripts/Documents/DocumentManagement.js"></script>
    <link rel="Stylesheet" href="../../Content/ItemForm.css" />
    <link rel="Stylesheet" href="../../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" href="../../Content/jQueryValidation/screen.css" />
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <table class="parentTable" id="newItemContent">
        <tr>
            <td class="leftBorderColumn" rowspan="2" style="min-width: 30px;"></td>
            <td class="formHeaderRow" style="width: auto; min-width: 930px;">
                <h1>Add New Commutation</h1>
            </td>
        </tr>
        <tr>
            <td class="formMainContent">
                <div class="contentRow"></div>
                <div class="contentRow">
                    <table class="childTable" style="width: 100%">
                        <col width="75" />
                        <col />
                        <col width="150" />
                        <col />
                        <col width="140" />
                        <tr>
                            <td class="subHeaderRow" colspan="6">
                                <h2>Project Setup</h2>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="errorCell" colspan="6" id="errorCell">
                                <h4>Placeholder error</h4>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <h4>Requestor:</h4>
                            </td>
                            <td class="contentCell">
                                <SharePoint:ClientPeoplePicker ID="requestorId" runat="server"
                                    PrincipalAccountType="User"
                                    PrincipalSource="All"
                                    AllowMultipleEntities="false"
                                    MaximumEntitySuggestions="50"
                                    Width="100%"
                                    InitialHelpText="Requestor Name"/>
                            </td>
                            <td class="contentCell">
                                <h4>Counterparty Name:</h4>
                            </td>
                            <td class="contentCell">
                                <asp:TextBox ID="counterpartyName" runat="server" Width="100%" required="true" />
                            </td>
                            <td class="contentCell">
                                <h4>Primary Manager:</h4>
                            </td>
                            <td class="contentCell">
                                <SharePoint:ClientPeoplePicker ID="primaryManagerId" runat="server"
                                    PrincipalAccountType="User"
                                    PrincipalSource="All"
                                    AllowMultipleEntities="false"
                                    MaximumEntitySuggestions="50"
                                    Width="100%"
                                    InitialHelpText="Primary Manager"
                                    InitialUserAccounts="trg\mcook"
                                    Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">&nbsp;
                            </td>
                            <td class="contentCell">&nbsp;
                            </td>
                            <td class="contentCell">
                                <h4>Request Type:</h4>
                            </td>
                            <td class="contentCell">
                                <asp:DropDownList ID="requestType" Width="100%" runat="server" required="true" />
                            </td>
                            <td class="contentCell">
                                <h4>Request Date:</h4>
                            </td>
                            <td class="contentCell">
                                <div style="width: auto; white-space: nowrap" id="requestDateContainer">
                                    <asp:TextBox ID="requestDate" runat="server" Width="80%" required="true" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="contentRow">&nbsp;</div>
                <div class="contentRow">
                    <table class="childTable" style="width:100%">
                        <tr>
                            <td class="subHeaderRow">
                                <h2>Document</h2>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <input type="file" id="getProjectFile" style="width:400px;" onchange="FileUploadFieldChanged('Project Document', 'getProjectFile')" class="fileUpload" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td class="contentCell">
                                <table class="childTable display" id="newItemDocuments">
                                    <thead>
                                        <tr>
                                            <th style="width:0px"></th>
                                            <th style="text-align:left">Filename</th>
                                        </tr>
                                    </thead>
                                    <tbody style="text-align:left">
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input type="submit" value="Submit" id="btnSubmit" />
                <button type="button" id="btnCancel" onclick="CancelSubmit()">Cancel</button>
            </td>
        </tr>
    </table>
    <div id="loadingdialog" title="Loading..." style="display:none; text-align:center;">
        <img alt="Loading..." src="../../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display:none; text-align:center;">
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
    <div id="creatingCheckList" title="Creating Deal Check List..." style="display:none; text-align:center;">
        <p>Creating deal check list for project...<br />
            Do not use the back button or refresh the page
        </p>
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
</asp:Content>
