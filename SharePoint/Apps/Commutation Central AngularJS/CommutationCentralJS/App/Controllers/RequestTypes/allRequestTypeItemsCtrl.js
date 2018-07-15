(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allRequestTypeItemsCtrl', ['$scope', 'requestTypeItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.requestTypes = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (requestType) {
                    $scope.inProgress = true;
                    itemFactory.remove(requestType.ID)
                    .then(function (response) {
                        var itemIndex = $scope.requestTypes.indexOf(requestType);
                        $scope.requestTypes.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/RequestTypes/Add.html",
                        controller: "addRequestTypeItemCtrl",
                        resolve: {
                            requestTypes: function () {
                                return $scope.requestTypes;
                            }
                        }
                    });
                    modalInstance.result.then(function (requestType) {
                        $scope.requestTypes.push(requestType);
                    });
                };
        }]);
})();
