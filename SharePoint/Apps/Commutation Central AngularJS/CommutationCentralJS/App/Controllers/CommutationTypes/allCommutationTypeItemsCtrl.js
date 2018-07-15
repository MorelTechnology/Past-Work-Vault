(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allCommutationTypeItemsCtrl', ['$scope', 'commutationTypeItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.commutationTypes = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (commutationType) {
                    $scope.inProgress = true;
                    itemFactory.remove(commutationType.ID)
                    .then(function (response) {
                        var itemIndex = $scope.commutationTypes.indexOf(commutationType);
                        $scope.commutationTypes.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/CommutationTypes/Add.html",
                    controller: "addCommutationTypeItemCtrl",
                    resolve: {
                        commutationTypes: function () {
                            return $scope.commutationTypes;
                        }
                    }
                });
                modalInstance.result.then(function (commutationType) {
                    $scope.commutationTypes.push(commutationType);
                });
            };
        }]);
})();
