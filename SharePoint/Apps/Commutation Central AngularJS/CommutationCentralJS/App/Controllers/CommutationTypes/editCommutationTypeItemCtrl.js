(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editCommutationTypeItemCtrl', ["$scope", "commutationTypeItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.commutationType = {
                        itemId: response.data.d.ID,
                        CommutationTypeName: response.data.d.CommutationTypeName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (commutationType) {
                    $scope.inProgress = true;
                    itemFactory.update(commutationType)
                    .then(function (response) {
                        $location.path("/CommutationTypes/All");
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
