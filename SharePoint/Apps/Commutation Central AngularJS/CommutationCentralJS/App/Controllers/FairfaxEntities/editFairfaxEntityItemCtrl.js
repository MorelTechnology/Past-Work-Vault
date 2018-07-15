(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editFairfaxEntityItemCtrl', ["$scope", "fairfaxEntityItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.fairfaxEntity = {
                        itemId: response.data.d.ID,
                        FairfaxEntityName: response.data.d.FairfaxEntityName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });
                $scope.editItem = function (fairfaxEntity) {
                    $scope.inProgress = true;
                    itemFactory.update(fairfaxEntity)
                    .then(function (response) {
                        $location.path("/FairfaxEntities/All");
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
