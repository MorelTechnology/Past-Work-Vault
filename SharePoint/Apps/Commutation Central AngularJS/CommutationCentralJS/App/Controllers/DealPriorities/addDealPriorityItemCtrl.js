(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addDealPriorityItemCtrl', ["$scope", "dealPriorityItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (dealPriority) {
                $scope.inProgress = true;
                itemFactory.addNew(dealPriority)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, DealPriorityName: response.data.d.DealPriorityName };
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
