(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editCommutationStatusItemCtrl', ["$scope", "commutationStatusItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.commutationStatus = {
                        itemId: response.data.d.ID,
                        CommutationStatusName: response.data.d.CommutationStatusName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (commutationStatus) {
                    $scope.inProgress = true;
                    itemFactory.update(commutationStatus)
                    .then(function (response) {
                        $location.path("/CommutationStatuses/All");
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
