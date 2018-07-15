(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addActivityPriorityItemCtrl', ["$scope", "activityPriorityItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (activityPriority) {
                $scope.inProgress = true;
                itemFactory.addNew(activityPriority)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, ActivityPriorityName: response.data.d.ActivityPriorityName };
                    $uibModalInstance.close(newItem);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
