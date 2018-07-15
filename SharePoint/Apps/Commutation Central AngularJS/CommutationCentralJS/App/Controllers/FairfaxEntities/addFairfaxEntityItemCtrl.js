(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addFairfaxEntityItemCtrl', ["$scope", "fairfaxEntityItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (fairfaxEntity) {
                $scope.inProgress = true;
                itemFactory.addNew(fairfaxEntity)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, FairfaxEntityName: response.data.d.FairfaxEntityName };
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
