(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('assignContactItemCtrl', ["$scope", "contactItemFactory", "$location", "$uibModalInstance", "project", "contactInScopeItemFactory",
            function ($scope, itemFactory, $location, $uibModalInstance, project, contactInScopeItemFactory) {
                $scope.project = project;
                $scope.inProgress = true;
                $scope.contactsInProgress = true;
                itemFactory.getAll()
                .then(function (response) {
                    $scope.contacts = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                    $scope.contactsInProgress = false;
                });
                $scope.assignContact = function (contact) {
                    $scope.inProgress = true;
                    var contactInScopeToAdd = {
                        ProjectId: project.Id,
                        ContactId: contact.Id
                    };
                    contactInScopeItemFactory.addNew(contactInScopeToAdd)
                    .then(function (response) {
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.contactsDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        }
                    ],
                    order: [1, 'asc']
                };
            }]);
})();
