(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editActivityCategoryItemCtrl', ["$scope", "activityCategoryItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.activityCategory = {
                        itemId: response.data.d.ID,
                        ActivityCategoryName: response.data.d.ActivityCategoryName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (activityCategory) {
                    $scope.inProgress = true;
                    itemFactory.update(activityCategory)
                    .then(function (response) {
                        $location.path("/ActivityCategories/All");
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
