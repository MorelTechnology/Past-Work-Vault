(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editActivityPriorityItemCtrl', ["$scope", "activityPriorityItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.activityPriority = {
                        itemId: response.data.d.ID,
                        ActivityPriorityName: response.data.d.ActivityPriorityName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (activityPriority) {
                    $scope.inProgress = true;
                    itemFactory.update(activityPriority)
                    .then(function (response) {
                        $location.path("/ActivityPriorities/All");
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
