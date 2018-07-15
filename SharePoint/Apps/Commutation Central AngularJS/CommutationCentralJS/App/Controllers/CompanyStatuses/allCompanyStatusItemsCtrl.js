(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allCompanyStatusItemsCtrl', ['$scope', 'companyStatusItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.companyStatuses = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (companyStatus) {
                    $scope.inProgress = true;
                    itemFactory.remove(companyStatus.ID)
                    .then(function (response) {
                        var itemIndex = $scope.companyStatuses.indexOf(companyStatus);
                        $scope.companyStatuses.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/CompanyStatuses/Add.html",
                        controller: "addCompanyStatusItemCtrl",
                        resolve: {
                            companyStatuses: function () {
                                return $scope.companyStatuses;
                            }
                        }
                    });
                    modalInstance.result.then(function (companyStatus) {
                        $scope.companyStatuses.push(companyStatus);
                    });
                };
        }]);
})();
