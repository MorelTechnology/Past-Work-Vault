(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addActivityCategoryItemCtrl', ["$scope", "activityCategoryItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (activityCategory) {
                $scope.inProgress = true;
                itemFactory.addNew(activityCategory)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, ActivityCategoryName: response.data.d.ActivityCategoryName };
                    $uibModalInstance.close(newItem);
                }).finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
