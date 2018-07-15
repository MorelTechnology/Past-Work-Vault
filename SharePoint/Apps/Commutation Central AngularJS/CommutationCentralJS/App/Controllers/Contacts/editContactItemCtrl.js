(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editContactItemCtrl', ["$scope", "contactItemFactory", "$location", "$uibModalInstance", "contactId",
            function ($scope, itemFactory, $location, $uibModalInstance, contactId) {
                $scope.inProgress = true;
                itemFactory.getById(contactId)
                .then(function (response) {
                    $scope.contact = response.data.d;
                    $scope.contact.itemId = $scope.contact.Id;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
                $scope.editItem = function (contact) {
                    $scope.inProgress = true;
                    itemFactory.update(contact)
                    .then(function (response) {
                        $uibModalInstance.close();
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
