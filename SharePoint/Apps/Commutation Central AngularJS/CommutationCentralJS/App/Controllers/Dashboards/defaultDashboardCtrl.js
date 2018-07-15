(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('defaultDashboardCtrl', ['$scope', 'projectItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties', 'projectFinancialsFactory', 'activityItemFactory',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties, projectFinancialsFactory, activityItemFactory) {
                $scope.dtOptions = {
                    columnDefs: [
                        {
                            aTargets: [3,4,5,6,7,8,9,10,11,12],
                            class: "tableCellNoWrap"
                        },
                        {
                            targets: [0, 1, 2],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [3, 'asc']
                };
                $scope.activityDTOptions = {
                    columnDefs: [
                        {
                            aTargets: [1, 2, 3, 4, 5, 6, 7, 8],
                            class: "tableCellNoWrap"
                        },
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };
                $scope.inProgress = true;
                $scope.getItems = function () {
                    $scope.inProgress = true;
                    itemFactory.getAll("CommutationStatus/CommutationStatusName ne 'Completed' and CommutationStatus/CommutationStatusName ne 'Dropped'")
                    .then(function (response) {
                        $scope.projects = response.data.d.results;
                        angular.forEach($scope.projects, function (project, key) {
                            projectFinancialsFactory.getProjectSummaryFinancials(project.ID)
                            .then(function (response) {
                                $scope.projects[key].SummaryFinancials = {
                                    TotalAssumed: response.TotalAssumed,
                                    TotalCeded: response.TotalCeded,
                                    TotalNet: response.TotalNet
                                }
                            });
                        });
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                ($scope.getActivities = function () {
                    $scope.inProgress = true;
                    activityItemFactory.getAll("AssignedTo/ID eq " + _spPageContextInfo.userId)
                    .then(function (response) {
                        $scope.activities = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();
                $scope.getItems();
                $scope.editProject = function (projectId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Projects/Edit.html",
                        controller: "editProjectItemCtrl",
                        backdrop: 'static',
                        windowClass: 'edit-project-dialog',
                        resolve: {
                            projectId: function () {
                                return projectId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getItems();
                    });
                };
                $scope.editActivity = function (activityId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Activities/Edit.html",
                        controller: "editActivityItemCtrl",
                        backdrop: 'static',
                        windowClass: 'edit-activity-dialog',
                        resolve: {
                            activityId: function () {
                                return activityId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getActivities();
                    });
                };
                $scope.attachDocument = function (entityType, entityId) {
                    var entityData = {
                        entityType: entityType,
                        entityId: entityId
                    };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Documents/AttachBlind.html",
                        controller: "attachDocumentBlindCtrl",
                        backdrop: "static",
                        resolve: {
                            entityData: function () {
                                return entityData;
                            }
                        }
                    });
                };
                $scope.addNote = function (projectId, counterpartyName) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Notes/Add.html",
                        controller: "addNoteItemCtrl",
                        backdrop: 'static',
                        windowClass: 'new-note-dialog',
                        resolve: {
                            project: {
                                ID: projectId,
                                CounterpartyName: counterpartyName
                            },
                            projectPreResolved: true
                        }
                    });
                };
            }]);
})();
