(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allActivityPriorityItemsCtrl', ['$scope', 'activityPriorityItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
            .then(function (response) {
                $scope.activityPriorities = response.data.d.results;
            })
            .finally(function () {
                $scope.inProgress = false;
            });
            $scope.removeItem = function (activityPriority) {
                $scope.inProgress = true;
                itemFactory.remove(activityPriority.ID)
                .then(function (response) {
                    var itemIndex = $scope.activityPriorities.indexOf(activityPriority);
                    $scope.activityPriorities.splice(itemIndex, 1);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/ActivityPriorities/Add.html",
                    controller: "addActivityPriorityItemCtrl",
                    resolve: {
                        activityPriorities: function () {
                            return $scope.activityPriorities;
                        }
                    }
                });
                modalInstance.result.then(function (activityPriority) {
                    $scope.activityPriorities.push(activityPriority);
                });
            };
        }]);
})();
