(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('completedProjectsCtrl', ['$scope', 'projectItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties', 'projectFinancialsFactory',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties, projectFinancialsFactory) {
                $scope.dtOptions = {
                    columnDefs: [
                        {
                            aTargets: [0,1,2,3,4,5,6,7],
                            class: "tableCellNoWrap"
                        }
                    ],
                    order: [0, 'asc']
                };
                $scope.inProgress = true;
                $scope.getItems = function () {
                    $scope.inProgress = true;
                    itemFactory.getAll("CommutationStatus/CommutationStatusName eq 'Completed'")
                    .then(function (response) {
                        $scope.projects = response.data.d.results;
                        angular.forEach($scope.projects, function (project, key) {
                            projectFinancialsFactory.getProjectSummaryFinancials(project.ID)
                            .then(function (response) {
                                $scope.projects[key].SummaryFinancials = {
                                    TotalAssumed: response.TotalAssumed,
                                    TotalCeded: response.TotalCeded,
                                    TotalNet: response.TotalNet,
                                    FinancialAuthority: 0 - $scope.projects[key].FinancialAuthorityAssumed + $scope.projects[key].FinancialAuthorityCeded
                                }
                            });
                        });
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
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
            }]);
})();
