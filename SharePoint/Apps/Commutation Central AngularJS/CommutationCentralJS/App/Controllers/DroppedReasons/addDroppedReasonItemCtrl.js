(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addDroppedReasonItemCtrl', ["$scope", "droppedReasonItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (droppedReason) {
                $scope.inProgress = true;
                itemFactory.addNew(droppedReason)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, DroppedReasonName: response.data.d.DroppedReasonName };
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
