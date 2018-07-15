<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../../Scripts/CompatibilityCheck.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.validate.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.formatCurrency-1.4.0.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/accounting.js/0.4.1/accounting.min.js"></script>
    <script type="text/javascript" src="../../Scripts/QuickLaunch/QuickLaunch.js"></script>
    <SharePoint:ScriptLink ID="ScriptLink1" name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink2" name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink3" name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink4" name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink5" name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink6" name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink7" name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <SharePoint:ScriptLink ID="ScriptLink8" name="sp.ui.dialog.js" runat="server" LoadAfterUI="true" Localizable="false" />
    <script type ="text/javascript" src="../../Scripts/Projects/EditItem.js"></script>
    <script type="text/javascript" src="../../Scripts/Documents/DocumentManagement.js"></script>
    <link rel="Stylesheet" href="../../Content/ItemForm.css" />
    <link rel="Stylesheet" href="../../Content/jQueryUI/StyleSheets/jquery-ui.css" />
    <link rel="Stylesheet" href="../../Content/jQueryValidation/screen.css" />
    <link rel="Stylesheet" href="https://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css" />
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" ID="noQuickLaunchMainContent" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <table class="parentTable" id="editItemContent">
        <tr>
            <td class="leftBorderColumn" rowspan="2" style="min-width: 30px;"></td>
            <td class="formHeaderRow" style="width: auto; min-width: 930px;">
                <h1>Commutation Details</h1>
            </td>
        </tr>
        <tr>
            <td class="formMainContent">
                <div class="contentRow" id="formTabs">
                    <ul>
                        <li><a href="#projectTab">Project</a></li>
                        <li><a href="#partiesTab">Parties</a></li>
                        <li><a href="#financialTab">Financial</a></li>
                        <li><a href="#activitiesTab">Activities</a></li>
                        <li><a href="#notesTab">Notes</a></li>
                        <li><a href="#checkListTab">Check List</a></li>
                    </ul>
                    <div class="contentRow" id="projectTab">
                        <table class="childTable" style="width: 100%">
                            <col width="150" />
                            <col width="250" />
                            <col width="155" />
                            <col width="250" />
                            <col width="130" />
                            <col width="150" />
                            <tr>
                                <td class="subHeaderRow" colspan="6">
                                    <h2>Project Information</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Counterparty Name:</h4>
                                </td>
                                <td class="contentCell" colspan="3">
                                    <input type="text" id="counterpartyName" name="counterpartyName" style="width:100%" onchange="TextFieldChanged(event, 'Title')" required="true" class="counterpartyNameField" />
                                </td>
                                <td class="contentCell">
                                    <h4>Project ID:</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="projectId" style="width:100%" onchange="TextFieldChanged(event, 'CommProjectID')" class="projectIdField" />
                                </td>
                            </tr>
                            <tr>
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
                                        InitialHelpText="Primary Manager" OnUserResolvedClientScript="PrimaryManagerFieldChanged" Enabled="false" />
                                </td>
                                <td class="contentCell">
                                    <h4>Secondary Manager:</h4>
                                </td>
                                <td class="contentCell">
                                    <SharePoint:ClientPeoplePicker ID="secondaryManagerId" runat="server"
                                        PrincipalAccountType="User"
                                        PrincipalSource="All"
                                        AllowMultipleEntities="false"
                                        MaximumEntitySuggestions="50"
                                        Width="100%"
                                        InitialHelpText="Secondary Manager" OnUserResolvedClientScript="SecondaryManagerFieldChanged" />
                                </td>
                                <td class="contentCell">
                                    <h4>Lead Office:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="leadOfficeId" style="width:100%" onchange="TextFieldChanged(event, 'LeadOfficeId')" />
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
                                        InitialHelpText="Requestor" OnUserResolvedClientScript="RequestorFieldChanged" />
                                </td>
                                <td class="contentCell">
                                    <h4>Request Type:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="requestTypeId" style="width:100%" onchange="TextFieldChanged(event, 'RequestTypeId')" required="true" />
                                </td>
                                <td class="contentCell">
                                    <h4>Request Date:</h4>
                                </td>
                                <td class="contentCell">
                                    <div style="width: auto; white-space: nowrap" id="requestDateContainer">
                                        <input type="text" name="requestDate" id="requestDate" style="width:80%" onchange="DateFieldChanged(event, 'RequestDate')" required="true" class="datefield" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Commutation Type:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="commutationTypeId" style="width:100%" onchange="TextFieldChanged(event, 'CommutationTypeId')" />
                                </td>
                                <td class="contentCell">
                                    <h4>Commutation Status:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="commutationStatusId" style="width:100%" onchange="TextFieldChanged(event, 'CommutationStatusId')" required="true" />
                                </td>
                                <td class="hiddenContentCell">
                                    <h4>Dropped Reason:</h4>
                                </td>
                                <td class="hiddenContentCell">
                                    <select id="droppedReasonId" style="width:100%" onchange="TextFieldChanged(event, 'DroppedReasonId')" name="DroppedReason" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Oversight Manager:</h4>
                                </td>
                                <td class="contentCell">
                                    <SharePoint:ClientPeoplePicker ID="oversightManagerId" runat="server"
                                        PrincipalAccountType="User"
                                        PrincipalSource="All"
                                        AllowMultipleEntities="false"
                                        MaximumEntitySuggestions="50"
                                        Width="100%"
                                        InitialHelpText="Oversight Manager" OnUserResolvedClientScript="OversightManagerFieldChanged" Enabled="false" />
                                </td>
                                <td class="contentCell">
                                    <h4>Deal Priority:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="dealPriorityId" style="width:100%" onchange="TextFieldChanged(event, 'DealPriorityId')" />
                                </td>
                                <td class="contentCell">
                                    <h4>Company Status:</h4>
                                </td>
                                <td class="contentCell">
                                    <select id="companyStatusId" style="width:100%" onchange="TextFieldChanged(event, 'CompanyStatusId')" />
                                </td>
                            </tr>
                        </table>
                        <div class="contentRow">&nbsp;</div>
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow">
                                    <h2>Document</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <input type="file" id="getProjectFile" style="width:400px;" onchange="FileUploadFieldChanged('Project Document', 'getProjectFile', 'Project')" class="fileUpload" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <table id="projectDocuments" class="childTable display">
                                        <thead>
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Filename</th>
                                                <th style="text-align:left">Document ID</th>
                                                <th style="text-align:left">Date Modified</th>
                                                <th style="text-align:left">Modified By</th>
                                                <th style="text-align:left">Version</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentRow" id="partiesTab">
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow" colspan="2">
                                    <h2>Companies in Scope</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <input type="text" id="newCompanyInScope" style="width:100%" />
                                </td>
                                <td class="contentCell">
                                    <button type="button" id="btnAddCompanyInScope" onclick="AddCompanyInScope()">Add Company</button>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell" colspan="2">
                                    <table id="companiesInScope" class="childTable display">
                                        <thead>
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Company Name</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="contentRow">&nbsp;</div>
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow" colspan="2">
                                    <h2>Fairfax Entities in Scope</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <select id="fairfaxEntityId" style="width:100%" />
                                </td>
                                <td class="contentCell">
                                    <button type="button" id="btnAddFairfaxEntityInScope" onclick="AddFairfaxEntityInScope()">Add Fairfax Entity</button>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell" colspan="2">
                                    <table id="fairfaxEntitiesInScope" class="childTable display">
                                        <thead>
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Fairfax Entity</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="contentRow">&nbsp;</div>
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow" colspan="2">
                                    <h2>Contacts</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell" colspan="2">
                                    <button type="button" id="btnAddContact" onclick="AddContact()">Add Contact</button>
                                    <button type="button" id="btnAttachContact" onclick="AttachContact()">Assign a Contact to this project</button>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell" colspan="2">
                                    <table id="contactsInScope" class="childTable display">
                                        <thead >
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Last Name</th>
                                                <th style="text-align:left">First Name</th>
                                                <th style="text-align:left">Email</th>
                                                <th style="text-align:left">Phone</th>
                                                <th style="text-align:left">Address</th>
                                                <th style="text-align:left">City</th>
                                                <th style="text-align:left">State</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentRow" id="financialTab">
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow" colspan="6">
                                    <h2>Financial</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Project ID:</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="projectId2" style="width:100%" onchange="TextFieldChanged(event, 'CommProjectID')" class="projectIdField" />
                                </td>
                                <td class="contentCell">
                                    <h4>Counterparty Name:</h4>
                                </td>
                                <td class="contentCell" colspan="3">
                                    <input type="text" id="counterpartyName2" name="counterpartyName" style="width:100%" onchange="TextFieldChanged(event, 'Title')" required class="counterpartyNameField" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Authority:</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="authority" style="width:100%" onchange="TextFieldChanged(event, 'FinancialAuthority')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <h4>Granted By:</h4>
                                </td>
                                <td class="contentCell">
                                    <SharePoint:ClientPeoplePicker ID="financialAuthorityGrantedById" runat="server"
                                        PrincipalAccountType="User"
                                        PrincipalSource="All"
                                        AllowMultipleEntities="false"
                                        MaximumEntitySuggestions="50"
                                        Width="100%"
                                        InitialHelpText="Granted By" OnUserResolvedClientScript="FinancialAuthorityGrantedByFieldChanged" />
                                </td>
                                <td class="contentCell">
                                    <h4>Granted By Date:</h4>
                                </td>
                                <td class="contentCell">
                                    <div style="width: auto; white-space: nowrap" id="financialAuthorityGrantedByDateContainer">
                                        <input type="text" name="financialAuthorityGrantedByDate" id="financialAuthorityGrantedByDate" style="width: 80%" onchange="DateFieldChanged(event, 'FinancialAuthorityGrantedByDate')" required="true" class="datefield" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="contentRow">&nbsp;</div>
                        <table class="childTable">
                            <tr>
                                <td class="subHeaderRow" colspan="2">
                                    <h2>Preliminary Estimate</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Valuation Date:</h4>
                                </td>
                                <td class="contentCell">
                                    <div style="width: auto; white-space: nowrap" id="preliminaryValuationDateContainer">
                                        <input type="text" id="preliminaryValuationDate" style="width: 80%" onchange="DateFieldChanged(event, 'PreliminaryValuationDate')" class="datefield"/>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Total (Assumed):</h4>
                                </td>
                                <td class="contentCell">
                                    (<input type="text" id="totalAssumed" style="width:95%" onchange="TextFieldChanged(event, 'TotalAssumed')" class="currency negative" />)
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Total Ceded</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="totalCeded" style="width:100%" onchange="TextFieldChanged(event, 'TotalCeded')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Total Net</h4>
                                </td>
                                <td class="contentCell">
                                    <label id="totalNet" style="width:100%" class="currency" />
                                </td>
                            </tr>
                        </table>
                        <div class="contentRow">&nbsp;</div>
                        <table class="childTable">
                            <tr>
                                <td class="subHeaderRow" colspan="3">
                                    <h2>Final Commutation Values</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Valuation Date:</h4>
                                </td>
                                <td class="contentCell">
                                    <div style="width: auto; white-space: nowrap" id="finalValuationDateContainer">
                                        <input type="text" id="finalValuationDate" style="width: 80%" onchange="DateFieldChanged(event, 'FinalValuationDate')" class="datefield"/>
                                    </div>
                                </td>
                                <td class="contentCell">

                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4></h4>
                                </td>
                                <td class="contentCell">
                                    <h4><b>Book Value</b></h4>
                                </td>
                                <td class="contentCell">
                                    <h4><b>Commutation Value</b></h4>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Assumed Liabilities</h4>
                                </td>
                                <td class="contentCell">
                                    (<input type="text" id="assumedBook" style="width:95%" onchange="TextFieldChanged(event, 'AssumedBook')" class="currency negative" />)
                                </td>
                                <td class="contentCell">
                                    (<input type="text" id="assumedCommutation" style="width:95%" onchange="TextFieldChanged(event, 'AssumedCommutation')" class="currency negative" />)
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Ceded Recoverables</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="cededBook" style="width:100%" onchange="TextFieldChanged(event, 'CededBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="cededCommutation" style="width:100%" onchange="TextFieldChanged(event, 'CededCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Net</h4>
                                </td>
                                <td class="contentCell">
                                    <label id="netBook" style="width:100%" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <label id="netCommutation" style="width:100%" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Bad Debt Provisions</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="badDebtBook" style="width:100%" onchange="TextFieldChanged(event, 'BadDebtBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="badDebtCommutation" style="width:100%" onchange="TextFieldChanged(event, 'BadDebtCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Adjusted Value</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="adjustedValueBook" style="width:100%" onchange="TextFieldChanged(event, 'AdjustedValueBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="adjustedValueCommutation" style="width:100%" onchange="TextFieldChanged(event, 'AdjustedValueCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Proceeds</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="proceedsBook" style="width:100%" onchange="TextFieldChanged(event, 'ProceedsBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="proceedsCommutation" style="width:100%" onchange="TextFieldChanged(event, 'ProceedsCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>Collateral</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="collateralBook" style="width:100%" onchange="TextFieldChanged(event, 'CollateralBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="collateralCommutation" style="width:100%" onchange="TextFieldChanged(event, 'CollateralCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>GAAP Impact</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="gaapImpactBook" style="width:100%" onchange="TextFieldChanged(event, 'GAAPImpactBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="gaapImpactCommutation" style="width:100%" onchange="TextFieldChanged(event, 'GAAPImpactCommutation')" class="currency" />
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <h4>STAT Impact</h4>
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="statImpactBook" style="width:100%" onchange="TextFieldChanged(event, 'STATImpactBook')" class="currency" />
                                </td>
                                <td class="contentCell">
                                    <input type="text" id="statImpactCommutation" style="width:100%" onchange="TextFieldChanged(event, 'STATImpactCommutation')" class="currency" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentRow" id="activitiesTab">
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow">
                                    <h2>Activities</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <button type="button" id="addActivity" onclick="AddActivity()">Add Activity</button>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <table id="activities" class="childTable display">
                                        <thead >
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Description</th>
                                                <th style="text-align:left">Activity Category</th>
                                                <th style="text-align:left">Entry Date</th>
                                                <th style="text-align:left">Assigned To</th>
                                                <th style="text-align:left">Priority</th>
                                                <th style="text-align:left">Activity Status</th>
                                                <th style="text-align:left">Due Date</th>
                                                <th style="text-align:left">Status Change Date</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left">
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentRow" id="notesTab">
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow">
                                    <h2>Notes</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <button type="button" id="addNote" onclick="AddNote()">Add Note</button>
                                </td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <table id="notes" class="childTable display">
                                        <thead>
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Entry Type</th>
                                                <th style="text-align:left">Content</th>
                                                <th style="text-align:left">Entry Date</th>
                                                <th style="text-align:left">Modified By</th>
                                                <th style="text-align:left">Modified</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left"></tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="contentRow" id="checkListTab">
                        <table class="childTable" style="width:100%">
                            <tr>
                                <td class="subHeaderRow"><h2>Check List</h2></td>
                            </tr>
                            <tr>
                                <td class="contentCell">
                                    <table id="checkListItems" class="childTable display">
                                        <thead>
                                            <tr>
                                                <th style="width:0px"></th>
                                                <th style="text-align:left">Check List Item</th>
                                                <th style="text-align:left">Applicable</th>
                                            </tr>
                                        </thead>
                                        <tbody style="text-align:left"></tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td class="contentCell" colspan="2" style="text-align: center;">
                <input type="submit" id="btnSubmit" value="Submit" />
                <button type="button" id="btnCancel" onclick="CancelSubmit()">Cancel</button>
            </td>
        </tr>
                <tr style="visibility:collapse">
            <td class="errorCell" colspan="2" id="errorCell">
                <h4>Placeholder error</h4>
            </td>
        </tr>
    </table>
    <div id="loadingdialog" title="Loading..." style="display:none; text-align:center;">
        <img alt="Loading..." src="../../Images/loading.gif" />
    </div>
    <div id="savingdialog" title="Saving Changes..." style="display:none; text-align:center;">
        <img alt="Saving..." src="../../Images/uploading.gif" />
    </div>
    <div id="managerFunctions">
        <h3>Super Manager Functions</h3>
        <div style="text-align: center">
            <button type="button" id="btnDelete" onclick="DeleteCurrentProject()">Delete Project</button>
        </div>
    </div>
</asp:Content>
