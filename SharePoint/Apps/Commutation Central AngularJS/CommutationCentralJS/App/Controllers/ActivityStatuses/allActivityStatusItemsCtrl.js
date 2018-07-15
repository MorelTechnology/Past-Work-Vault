(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allActivityStatusItemsCtrl', ['$scope', 'activityStatusItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
            .then(function (response) {
                $scope.activityStatuses = response.data.d.results;
            })
            .finally(function () {
                $scope.inProgress = false;
            });
            $scope.removeItem = function (activityStatus) {
                $scope.inProgress = true;
                itemFactory.remove(activityStatus.ID)
                .then(function (response) {
                    var itemIndex = $scope.activityStatuses.indexOf(activityStatus);
                    $scope.activityStatuses.splice(itemIndex, 1);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/ActivityStatuses/Add.html",
                    controller: "addActivityStatusItemCtrl",
                    resolve: {
                        activityStatuses: function () {
                            return $scope.activityStatuses;
                        }
                    }
                });
                modalInstance.result.then(function (activityStatus) {
                    $scope.activityStatuses.push(activityStatus);
                });
            };
        }]);
})();
