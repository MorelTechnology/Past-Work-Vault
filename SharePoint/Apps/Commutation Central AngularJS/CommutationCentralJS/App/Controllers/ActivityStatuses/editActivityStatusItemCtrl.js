(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editActivityStatusItemCtrl', ["$scope", "activityStatusItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.activityStatus = {
                        itemId: response.data.d.ID,
                        ActivityStatusName: response.data.d.ActivityStatusName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (activityStatus) {
                    $scope.inProgress = true;
                    itemFactory.update(activityStatus)
                    .then(function (response) {
                        $location.path("/ActivityStatuses/All");
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
