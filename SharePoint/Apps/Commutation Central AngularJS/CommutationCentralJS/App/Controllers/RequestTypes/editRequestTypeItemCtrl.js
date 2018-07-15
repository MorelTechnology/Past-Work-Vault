(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editRequestTypeItemCtrl', ["$scope", "requestTypeItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.requestType = {
                        itemId: response.data.d.ID,
                        RequestTypeName: response.data.d.RequestTypeName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (requestType) {
                    $scope.inProgress = true;
                    itemFactory.update(requestType)
                    .then(function (response) {
                        $location.path("/RequestTypes/All");
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
