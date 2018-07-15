(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allFairfaxEntityItemsCtrl', ['$scope', 'fairfaxEntityItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.fairfaxEntities = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (fairfaxEntity) {
                    $scope.inProgress = true;
                    itemFactory.remove(fairfaxEntity.ID)
                    .then(function (response) {
                        var itemIndex = $scope.fairfaxEntities.indexOf(fairfaxEntity);
                        $scope.fairfaxEntities.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/FairfaxEntities/Add.html",
                        controller: "addFairfaxEntityItemCtrl",
                        resolve: {
                            fairfaxEntities: function () {
                                return $scope.fairfaxEntities;
                            }
                        }
                    });
                    modalInstance.result.then(function (fairfaxEntity) {
                        $scope.fairfaxEntities.push(fairfaxEntity);
                    });
                };
        }]);
})();
