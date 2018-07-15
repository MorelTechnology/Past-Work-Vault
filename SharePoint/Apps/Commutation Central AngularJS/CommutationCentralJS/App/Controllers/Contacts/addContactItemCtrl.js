(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addContactItemCtrl', ["$scope", "contactItemFactory", "$location", "$uibModalInstance", "projectId", "contactInScopeItemFactory", "$q",
            function ($scope, itemFactory, $location, $uibModalInstance, projectId, contactInScopeItemFactory, $q) {
                $scope.addItem = function (contact) {
                    $scope.inProgress = true;
                    var promises = [];
                    var addContact = itemFactory.addNew(contact)
                    .then(function (response) {
                        if (projectId) {
                            var contactEntityToAdd = {
                                ProjectId: projectId,
                                ContactId: response.data.d.Id
                            };
                            var attachContact = contactInScopeItemFactory.addNew(contactEntityToAdd)
                            .then(function (response) {
                                $uibModalInstance.close();
                            });
                            promises.push(attachContact);
                        } else {
                            $uibModalInstance.close();
                        }
                    })
                    .finally(function () {
                        //$scope.inProgress = false;
                    });

                    promises.push(addContact);
                    $q.all(promises).finally(function (responses) {
                        $scope.inProgress = false;
                    });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
