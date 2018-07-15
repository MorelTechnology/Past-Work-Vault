(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editActivityDroppedReasonItemCtrl', ["$scope", "activityDroppedReasonItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.activityDroppedReason = {
                        itemId: response.data.d.ID,
                        ActivityDroppedReasonName: response.data.d.ActivityDroppedReasonName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (activityDroppedReason) {
                    $scope.inProgress = true;
                    itemFactory.update(activityDroppedReason)
                    .then(function (response) {
                        $location.path("/ActivityDroppedReasons/All");
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
