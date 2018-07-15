(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addNoteEntryTypeItemCtrl', ["$scope", "noteEntryTypeItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (noteEntryType) {
                $scope.inProgress = true;
                itemFactory.addNew(noteEntryType)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, NoteEntryTypeName: response.data.d.NoteEntryTypeName };
                    $uibModalInstance.close(newItem);
                }).finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
