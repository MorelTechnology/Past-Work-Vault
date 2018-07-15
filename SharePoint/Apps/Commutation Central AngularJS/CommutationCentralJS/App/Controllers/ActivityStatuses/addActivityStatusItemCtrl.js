(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addActivityStatusItemCtrl', ["$scope", "activityStatusItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (activityStatus) {
                $scope.inProgress = true;
                itemFactory.addNew(activityStatus)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, ActivityStatusName: response.data.d.ActivityStatusName };
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
