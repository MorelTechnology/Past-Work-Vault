(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addCommutationStatusItemCtrl', ["$scope", "commutationStatusItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (commutationStatus) {
                $scope.inProgress = true;
                itemFactory.addNew(commutationStatus)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, CommutationStatusName: response.data.d.CommutationStatusName };
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
