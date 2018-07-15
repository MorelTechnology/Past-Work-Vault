<%-- The following 4 lines are ASP.NET directives needed when using SharePoint components --%>

<%@ Page Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" MasterPageFile="~masterurl/default.master" Language="C#" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%-- The markup and script in the following Content element will be placed in the <head> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="Stylesheet" href="../Content/App.css" />
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <SharePoint:ScriptLink name="sp.js" runat="server" OnDemand="true" LoadAfterUI="true" Localizable="false" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.5.0/angular.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.5.0/angular-animate.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.5.0/angular-route.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/1.2.1/ui-bootstrap-tpls.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/datatables/1.10.11/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-currency/0.9.3/ng-currency.min.js"></script>
    <script type="text/javascript" src="../Scripts/angular-datatables.min.js"></script>
    <script type="text/javascript" src="../Scripts/Sp-ngUtils.module.js"></script>
    <script type="text/javascript" src="../Scripts/spPeoplePicker.directive.js"></script>
    <script type="text/javascript" src="../Scripts/spPpJsdepLoader.service.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/danialfarid-angular-file-upload/12.0.4/ng-file-upload.min.js"></script>
    <script type="text/javascript" src="../Scripts/validate.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/datatables/1.10.11/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.6/css/bootstrap.min.css"/>
    <!--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css"/>-->

    <%-- App-specific scripts go here  --%>
    <%-- Angular Modules --%>
    <script type="text/javascript" src="../App/Modules/commutationCentral.js"></script>

    <%-- Angular Directives --%>
    <script type="text/javascript" src="../App/Directives/buttonGoClick.js"></script>
    <script type="text/javascript" src="../App/Directives/trgErrorModal.js"></script>
    <script type="text/javascript" src="../App/Directives/trgFileRead.js"></script>

    <%-- Angular Factories --%>
    <script type="text/javascript" src="../App/Factories/baseItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/LeadOffices/leadOfficeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/RequestTypes/requestTypeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/DealPriorities/dealPriorityItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/CommutationStatuses/commutationStatusItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/DroppedReasons/droppedReasonItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/CommutationTypes/commutationTypeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/CompanyStatuses/companyStatusItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/Projects/projectItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/sharedProperties.js"></script>
    <script type="text/javascript" src="../App/Factories/SiteUsers/siteUsersFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/CompaniesInScope/companyInScopeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/FairfaxEntities/fairfaxEntityItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/FairfaxEntitiesInScope/fairfaxEntityInScopeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/Contacts/contactItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ContactsInScope/contactInScopeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/FinancialEntries/financialEntryItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ProjectDocuments/projectDocumentItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ActivityCategories/activityCategoryItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ActivityPriorities/activityPriorityItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ActivityStatuses/activityStatusItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ActivityDroppedReasons/activityDroppedReasonItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/Activities/activityItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ActivityDocuments/activityDocumentItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ProjectFinancials/projectFinancialsFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/NoteEntryTypes/noteEntryTypeItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/Notes/noteItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/NoteDocuments/noteDocumentItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/Checklists/checklistItemFactory.js"></script>
    <script type="text/javascript" src="../App/Factories/ChecklistDocuments/checklistDocumentItemFactory.js"></script>

    <%-- Angular Controllers --%>
    <script type="text/javascript" src="../App/Controllers/Dashboards/defaultDashboardCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Navigation/leftNav.js"></script>
    <script type="text/javascript" src="../App/Controllers/Errors/modalErrorCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/LeadOffices/allItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/LeadOffices/editLeadOfficeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/LeadOffices/addLeadOfficeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/RequestTypes/addRequestTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/RequestTypes/allRequestTypeItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/RequestTypes/editRequestTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DealPriorities/addDealPriorityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DealPriorities/allDealPriorityItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DealPriorities/editDealPriorityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationStatuses/addCommutationStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationStatuses/allCommutationStatusItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationStatuses/editCommutationStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DroppedReasons/addDroppedReasonItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DroppedReasons/allDroppedReasonItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/DroppedReasons/editDroppedReasonItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationTypes/addCommutationTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationTypes/allCommutationTypeItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CommutationTypes/editCommutationTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CompanyStatuses/addCompanyStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CompanyStatuses/allCompanyStatusItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CompanyStatuses/editCompanyStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Projects/addProjectItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Projects/editProjectItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/CompaniesInScope/editCompanyInScopeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/FairfaxEntities/addFairfaxEntityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/FairfaxEntities/allFairfaxEntityItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/FairfaxEntities/editFairfaxEntityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Contacts/addContactItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Contacts/assignContactItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Contacts/editContactItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/GenericDialogs/yesNoDialogCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityCategories/addActivityCategoryItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityCategories/allActivityCategoryItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityCategories/editActivityCategoryItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityPriorities/addActivityPriorityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityPriorities/allActivityPriorityItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityPriorities/editActivityPriorityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityStatuses/addActivityStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityStatuses/allActivityStatusItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityStatuses/editActivityStatusItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityDroppedReasons/addActivityDroppedReasonItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityDroppedReasons/allActivityDroppedReasonItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/ActivityDroppedReasons/editActivityDroppedReasonItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Activities/addActivityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Activities/editActivityItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/allOpenProjectsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/allActivitiesCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/allContactsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/completedProjectsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/droppedProjectsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/myActivitiesCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/NoteEntryTypes/addNoteEntryTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/NoteEntryTypes/allNoteEntryTypeItemsCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/NoteEntryTypes/editNoteEntryTypeItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Dashboards/allNotesCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Notes/addNoteItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Notes/editNoteItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Checklists/editChecklistItemCtrl.js"></script>
    <script type="text/javascript" src="../App/Controllers/Documents/attachDocumentBlindCtrl.js"></script>


    <meta name="WebPartPageExpansion" content="full" />
</asp:Content>

<%-- The markup in the following Content element will be placed in the TitleArea of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Commutation Central
</asp:Content>

<%-- The markup and script in the following Content element will be placed in the <body> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div data-ng-app="commutationCentral" style="padding-top:50px;position:relative;">
        <div ng-controller="leftNav">
            <uib-accordion close-others="oneAtATime" class="leftNav" template-url="../App/Templates/Navigation/LeftNav.html">
                <!--<uib-accordion-group heading="Static Header" template-url="../App/Templates/Navigation/LeftNav.html"></uib-accordion-group>-->
            </uib-accordion>
        </div>
        <div data-ng-view class="commutationCentral-app" style="position: absolute; left: 220px;"></div>
    </div>

</asp:Content>
