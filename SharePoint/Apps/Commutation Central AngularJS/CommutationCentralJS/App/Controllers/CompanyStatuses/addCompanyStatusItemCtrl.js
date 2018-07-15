(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addCompanyStatusItemCtrl', ["$scope", "companyStatusItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (companyStatus) {
                $scope.inProgress = true;
                itemFactory.addNew(companyStatus)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, CompanyStatusName: response.data.d.CompanyStatusName };
                    $uibModalInstance.close(newItem);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
