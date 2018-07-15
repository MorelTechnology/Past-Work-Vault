(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editLeadOfficeItemCtrl', ["$scope", "leadOfficeItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.leadOffice = {
                        itemId: response.data.d.ID,
                        LeadOfficeName: response.data.d.LeadOfficeName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (leadOffice) {
                    $scope.inProgress = true;
                    itemFactory.update(leadOffice)
                    .then(function (response) {
                        $location.path("/LeadOffices/All");
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
