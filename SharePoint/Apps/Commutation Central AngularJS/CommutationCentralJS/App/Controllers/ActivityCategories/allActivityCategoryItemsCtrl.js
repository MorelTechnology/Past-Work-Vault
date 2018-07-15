(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allActivityCategoryItemsCtrl', ['$scope', 'activityCategoryItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
                .then(function (response) {
                    $scope.activityCategories = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            $scope.removeItem = function (activityCategory) {
                $scope.inProgress = true;
                itemFactory.remove(activityCategory.ID)
                .then(function (response) {
                    var itemIndex = $scope.activityCategories.indexOf(activityCategory);
                    $scope.activityCategories.splice(itemIndex, 1);
                }).finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/ActivityCategories/Add.html",
                    controller: "addActivityCategoryItemCtrl",
                    resolve: {
                        activityCategories: function () {
                            return $scope.activityCategories;
                        }
                    }
                });
                modalInstance.result.then(function (activityCategory) {
                    $scope.activityCategories.push(activityCategory);
                });
            };
        }]);
})();
