(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allDealPriorityItemsCtrl', ['$scope', 'dealPriorityItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.dealPriorities = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (dealPriority) {
                    $scope.inProgress = true;
                    itemFactory.remove(dealPriority.ID)
                    .then(function (response) {
                        var itemIndex = $scope.dealPriorities.indexOf(dealPriority);
                        $scope.dealPriorities.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/DealPriorities/Add.html",
                        controller: "addDealPriorityItemCtrl",
                        resolve: {
                            dealPriorities: function () {
                                return $scope.dealPriorities;
                            }
                        }
                    });
                    modalInstance.result.then(function (dealPriority) {
                        $scope.dealPriorities.push(dealPriority);
                    });
                };
        }]);
})();
