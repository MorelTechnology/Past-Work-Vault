(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addActivityDroppedReasonItemCtrl', ["$scope", "activityDroppedReasonItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (activityDroppedReason) {
                $scope.inProgress = true;
                itemFactory.addNew(activityDroppedReason)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, ActivityDroppedReasonName: response.data.d.ActivityDroppedReasonName };
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
