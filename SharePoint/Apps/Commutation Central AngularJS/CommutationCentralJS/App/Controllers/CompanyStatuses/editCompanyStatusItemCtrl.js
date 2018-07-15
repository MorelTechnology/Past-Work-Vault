(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editCompanyStatusItemCtrl', ["$scope", "companyStatusItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.companyStatus = {
                        itemId: response.data.d.ID,
                        CompanyStatusName: response.data.d.CompanyStatusName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (companyStatus) {
                    $scope.inProgress = true;
                    itemFactory.update(companyStatus)
                    .then(function (response) {
                        $location.path("/CompanyStatuses/All");
                    })
                    .finally(function(){
                        $scope.inProgress = false;
                    });
                };

                $scope.cancel = function () {
                    $window.history.back();
                };
        }]);
})();
