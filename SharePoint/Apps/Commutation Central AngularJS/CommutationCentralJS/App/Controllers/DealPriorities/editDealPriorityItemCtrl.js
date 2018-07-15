(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editDealPriorityItemCtrl', ["$scope", "dealPriorityItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.dealPriority = {
                        itemId: response.data.d.ID,
                        DealPriorityName: response.data.d.DealPriorityName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (dealPriority) {
                    $scope.inProgress = true;
                    itemFactory.update(dealPriority)
                    .then(function (response) {
                        $location.path("/DealPriorities/All");
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
