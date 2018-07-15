(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allContactsCtrl', ["$scope", "contactItemFactory", "$location", "$uibModal",
            function ($scope, contactItemFactory, $location, $uibModal) {
                $scope.inProgress = true;
                ($scope.getAllContacts = function () {
                    contactItemFactory.getAll()
                    .then(function (response) {
                        $scope.contacts = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();
                $scope.contactsDTOptions = {
                };
                $scope.editContact = function (contactId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Contacts/Edit.html",
                        controller: "editContactItemCtrl",
                        backdrop: "static",
                        resolve: {
                            contactId: function () {
                                return contactId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getAllContacts();
                    });
                };
            }]);
})();
