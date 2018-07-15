(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allDroppedReasonItemsCtrl', ['$scope', 'droppedReasonItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.droppedReasons = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (droppedReason) {
                    $scope.inProgress = true;
                    itemFactory.remove(droppedReason.ID)
                    .then(function (response) {
                        var itemIndex = $scope.droppedReasons.indexOf(droppedReason);
                        $scope.droppedReasons.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/DroppedReasons/Add.html",
                        controller: "addDroppedReasonItemCtrl",
                        resolve: {
                            droppedReasons: function () {
                                return $scope.droppedReasons;
                            }
                        }
                    });
                    modalInstance.result.then(function (droppedReason) {
                        $scope.droppedReasons.push(droppedReason);
                    });
                };
        }]);
})();
