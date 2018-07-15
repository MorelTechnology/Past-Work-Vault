(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editCompanyInScopeItemCtrl', ["$scope", "companyInScopeItemFactory", "$routeParams", "$location", "$window", "itemId", "$uibModalInstance",
            function ($scope, itemFactory, $routeParams, $location, $window, itemId, $uibModalInstance) {
                $scope.inProgress = true;
                itemFactory.getById(itemId)
                .then(function (response) {
                    $scope.companyInScope = {
                        itemId: response.data.d.ID,
                        ProjectId: response.data.d.ProjectId,
                        CompanyName: response.data.d.CompanyName
                    };
                })
                .finally(function () {
                    $scope.inProgress = false;
                });

                $scope.editItem = function (company) {
                    $scope.inProgress = true;
                    itemFactory.update(company)
                    .then(function (response) {
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
        }]);
})();
