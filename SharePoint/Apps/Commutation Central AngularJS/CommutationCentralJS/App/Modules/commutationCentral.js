(function () {
    'use strict';

    angular.module('commutationCentral', [
        // Angular modules 
        'ngRoute',

        // Custom modules 

        // 3rd Party Modules
        'ui.bootstrap',
        'ngAnimate',
        'datatables',
        'spUtils',
        'ng-currency',
        'ngFileUpload',
        'ui.validate'
    ])
    .config(["$routeProvider", function ($routeProvider) {
        $routeProvider.when("/", {
            templateUrl: "../App/Templates/Dashboards/Default.html",
            controller: "defaultDashboardCtrl"
        }).when("/AllOpenProjects", {
            templateUrl: "../App/Templates/Dashboards/AllOpenProjects.html",
            controller: "allOpenProjectsCtrl"
        }).when("/AllActivities", {
            templateUrl: "../App/Templates/Dashboards/AllActivities.html",
            controller: "allActivitiesCtrl"
        }).when("/AllNotes", {
            templateUrl: "../App/Templates/Dashboards/AllNotes.html",
            controller: "allNotesCtrl"
        }).when("/CommutationDocuments", {
            templateUrl: "../Lists/Commutation Documents/Forms/AllItems.aspx?IsDlg=1"
        }).when("/AllContacts", {
            templateUrl: "../App/Templates/Dashboards/AllContacts.html",
            controller: "allContactsCtrl"
        }).when("/CompletedProjects", {
            templateUrl: "../App/Templates/Dashboards/CompletedProjects.html",
            controller: "completedProjectsCtrl"
        }).when("/DroppedProjects", {
            templateUrl: "../App/Templates/Dashboards/DroppedProjects.html",
            controller: "droppedProjectsCtrl"
        }).when("/MyActivities", {
            templateUrl: "../App/Templates/Dashboards/MyActivities.html",
            controller: "myActivitiesCtrl"
        }).when("/Admin/ControlPanel", {
            templateUrl: "../App/Templates/Admin/ControlPanel.html"
        }).when("/LeadOffices/All", {
            templateUrl: "../App/Templates/LeadOffices/All.html",
            controller: "allLeadOfficeItemsCtrl"
        }).when("/LeadOffices/Edit/:itemId", {
            templateUrl: "../App/Templates/LeadOffices/Edit.html",
            controller: "editLeadOfficeItemCtrl"
        }).when("/RequestTypes/All", {
            templateUrl: "../App/Templates/RequestTypes/All.html",
            controller: "allRequestTypeItemsCtrl"
        }).when("/RequestTypes/Edit/:itemId", {
            templateUrl: "../App/Templates/RequestTypes/Edit.html",
            controller: "editRequestTypeItemCtrl"
        }).when("/DealPriorities/All", {
            templateUrl: "../App/Templates/DealPriorities/All.html",
            controller: "allDealPriorityItemsCtrl"
        }).when("/DealPriorities/Edit/:itemId", {
            templateUrl: "../App/Templates/DealPriorities/Edit.html",
            controller: "editDealPriorityItemCtrl"
        }).when("/CommutationStatuses/All", {
            templateUrl: "../App/Templates/CommutationStatuses/All.html",
            controller: "allCommutationStatusItemsCtrl"
        }).when("/CommutationStatuses/Edit/:itemId", {
            templateUrl: "../App/Templates/CommutationStatuses/Edit.html",
            controller: "editCommutationStatusItemCtrl"
        }).when("/DroppedReasons/All", {
            templateUrl: "../App/Templates/DroppedReasons/All.html",
            controller: "allDroppedReasonItemsCtrl"
        }).when("/DroppedReasons/Edit/:itemId", {
            templateUrl: "../App/Templates/DroppedReasons/Edit.html",
            controller: "editDroppedReasonItemCtrl"
        }).when("/CommutationTypes/All", {
            templateUrl: "../App/Templates/CommutationTypes/All.html",
            controller: "allCommutationTypeItemsCtrl"
        }).when("/CommutationTypes/Edit/:itemId", {
            templateUrl: "../App/Templates/CommutationTypes/Edit.html",
            controller: "editCommutationTypeItemCtrl"
        }).when("/CompanyStatuses/All", {
            templateUrl: "../App/Templates/CompanyStatuses/All.html",
            controller: "allCompanyStatusItemsCtrl"
        }).when("/CompanyStatuses/Edit/:itemId", {
            templateUrl: "../App/Templates/CompanyStatuses/Edit.html",
            controller: "editCompanyStatusItemCtrl"
        }).when("/FairfaxEntities/All", {
            templateUrl: "../App/Templates/FairfaxEntities/All.html",
            controller: "allFairfaxEntityItemsCtrl"
        }).when("/FairfaxEntities/Edit/:itemId", {
            templateUrl: "../App/Templates/FairfaxEntities/Edit.html",
            controller: "editFairfaxEntityItemCtrl"
        }).when("/ActivityCategories/All", {
            templateUrl: "../App/Templates/ActivityCategories/All.html",
            controller: "allActivityCategoryItemsCtrl"
        }).when("/ActivityCategories/Edit/:itemId", {
            templateUrl: "../App/Templates/ActivityCategories/Edit.html",
            controller: "editActivityCategoryItemCtrl"
        }).when("/ActivityPriorities/All", {
            templateUrl: "../App/Templates/ActivityPriorities/All.html",
            controller: "allActivityPriorityItemsCtrl"
        }).when("/ActivityPriorities/Edit/:itemId", {
            templateUrl: "../App/Templates/ActivityPriorities/Edit.html",
            controller: "editActivityPriorityItemCtrl"
        }).when("/ActivityStatuses/All", {
            templateUrl: "../App/Templates/ActivityStatuses/All.html",
            controller: "allActivityStatusItemsCtrl"
        }).when("/ActivityStatuses/Edit/:itemId", {
            templateUrl: "../App/Templates/ActivityStatuses/Edit.html",
            controller: "editActivityStatusItemCtrl"
        }).when("/ActivityDroppedReasons/All", {
            templateUrl: "../App/Templates/ActivityDroppedReasons/All.html",
            controller: "allActivityDroppedReasonItemsCtrl"
        }).when("/ActivityDroppedReasons/Edit/:itemId", {
            templateUrl: "../App/Templates/ActivityDroppedReasons/Edit.html",
            controller: "editActivityDroppedReasonItemCtrl"
        }).when("/NoteEntryTypes/All", {
            templateUrl: "../App/Templates/NoteEntryTypes/All.html",
            controller: "allNoteEntryTypeItemsCtrl"
        }).when("/NoteEntryTypes/Edit/:itemId", {
            templateUrl: "../App/Templates/NoteEntryTypes/Edit.html",
            controller: "editNoteEntryTypeItemCtrl"
        });
    }]);
})();