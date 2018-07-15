(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addRequestTypeItemCtrl', ["$scope", "requestTypeItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (requestType) {
                $scope.inProgress = true;
                itemFactory.addNew(requestType)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, RequestTypeName: response.data.d.RequestTypeName };
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
