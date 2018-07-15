(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allLeadOfficeItemsCtrl', ['$scope', 'leadOfficeItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, leadOfficeItemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
                $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
                $scope.inProgress = true;
                leadOfficeItemFactory.getAll()
                .then(function (response) {
                    $scope.leadOffices = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.removeItem = function (leadOffice) {
                    $scope.inProgress = true;
                    leadOfficeItemFactory.remove(leadOffice.ID)
                    .then(function (response) {
                        var itemIndex = $scope.leadOffices.indexOf(leadOffice);
                        $scope.leadOffices.splice(itemIndex, 1);
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.animationsEnabled = true;
                $scope.open = function () {
                    var modalInstance = $uibModal.open({
                        animation: $scope.animationsEnabled,
                        templateUrl: "../App/Templates/LeadOffices/Add.html",
                        controller: "addLeadOfficeItemCtrl",
                        resolve: {
                            leadOffices: function () {
                                return $scope.leadOffices;
                            }
                        }
                    });
                    modalInstance.result.then(function (leadOffice) {
                        $scope.leadOffices.push(leadOffice);
                    });
                };
        }]);
})();
