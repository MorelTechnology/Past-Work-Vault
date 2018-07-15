(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addCommutationTypeItemCtrl', ["$scope", "commutationTypeItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (commutationType) {
                $scope.inProgress = true;
                itemFactory.addNew(commutationType)
                .then(function (response) {
                        var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, CommutationTypeName: response.data.d.CommutationTypeName };
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
