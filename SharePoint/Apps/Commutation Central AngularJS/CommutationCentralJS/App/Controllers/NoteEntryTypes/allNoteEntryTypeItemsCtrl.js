(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allNoteEntryTypeItemsCtrl', ['$scope', 'noteEntryTypeItemFactory', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties',
            function ($scope, itemFactory, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties) {
            $scope.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(2);
            $scope.inProgress = true;
            itemFactory.getAll()
                .then(function (response) {
                    $scope.noteEntryTypes = response.data.d.results;
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            $scope.removeItem = function (noteEntryType) {
                $scope.inProgress = true;
                itemFactory.remove(noteEntryType.ID)
                .then(function (response) {
                    var itemIndex = $scope.noteEntryTypes.indexOf(noteEntryType);
                    $scope.noteEntryTypes.splice(itemIndex, 1);
                }).finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.animationsEnabled = true;
            $scope.open = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/NoteEntryTypes/Add.html",
                    controller: "addNoteEntryTypeItemCtrl",
                    resolve: {
                        noteEntryTypes: function () {
                            return $scope.noteEntryTypes;
                        }
                    }
                });
                modalInstance.result.then(function (noteEntryType) {
                    $scope.noteEntryTypes.push(noteEntryType);
                });
            };
        }]);
})();
