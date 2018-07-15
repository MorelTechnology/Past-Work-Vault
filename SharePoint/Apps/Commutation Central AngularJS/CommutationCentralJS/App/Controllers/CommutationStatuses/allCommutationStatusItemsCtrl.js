(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allCommutationStatusItemsCtrl', ['$scope', 'commutationStatusItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
            .then(function (response) {
                $scope.commutationStatuses = response.data.d.results;
            })
            .finally(function () {
                $scope.inProgress = false;
            });
            $scope.removeItem = function (commutationStatus) {
                $scope.inProgress = true;
                itemFactory.remove(commutationStatus.ID)
                .then(function (response) {
                    var itemIndex = $scope.commutationStatuses.indexOf(commutationStatus);
                    $scope.commutationStatuses.splice(itemIndex, 1);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/CommutationStatuses/Add.html",
                    controller: "addCommutationStatusItemCtrl",
                    resolve: {
                        commutationStatuses: function () {
                            return $scope.commutationStatuses;
                        }
                    }
                });
                modalInstance.result.then(function (commutationStatus) {
                    $scope.commutationStatuses.push(commutationStatus);
                });
            };
        }]);
})();
