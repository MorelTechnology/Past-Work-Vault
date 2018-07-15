(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allActivityDroppedReasonItemsCtrl', ['$scope', 'activityDroppedReasonItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
            .then(function (response) {
                $scope.activityDroppedReasons = response.data.d.results;
            })
            .finally(function () {
                $scope.inProgress = false;
            });
            $scope.removeItem = function (activityDroppedReason) {
                $scope.inProgress = true;
                itemFactory.remove(activityDroppedReason.ID)
                .then(function (response) {
                    var itemIndex = $scope.activityDroppedReasons.indexOf(activityDroppedReason);
                    $scope.activityDroppedReasons.splice(itemIndex, 1);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/ActivityDroppedReasons/Add.html",
                    controller: "addActivityDroppedReasonItemCtrl",
                    resolve: {
                        activityDroppedReasons: function () {
                            return $scope.activityDroppedReasons;
                        }
                    }
                });
                modalInstance.result.then(function (activityDroppedReason) {
                    $scope.activityDroppedReasons.push(activityDroppedReason);
                });
            };
        }]);
})();
