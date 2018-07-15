(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('myActivitiesCtrl', ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties', 'activityItemFactory', 'siteUsersFactory',
            function ($scope, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties, activityItemFactory, siteUsersFactory) {
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
                ($scope.getActivities = function () {
                    $scope.inProgress = true;
                    siteUsersFactory.getCurrentUser()
                    .then(function (response) {
                        $scope.inProgress = true;
                        var userId = response.data.d.Id;
                        activityItemFactory.getAll("AssignedTo/ID eq " + userId)
                        .then(function (response) {
                            $scope.activities = response.data.d.results;
                        })
                        .finally(function () {
                            $scope.inProgress = false;
                        });
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();
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
            }]);
})();
