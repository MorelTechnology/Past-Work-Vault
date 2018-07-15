(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('allNotesCtrl', ['$scope', 'DTOptionsBuilder', 'DTColumnDefBuilder', '$uibModal', 'sharedProperties', 'noteItemFactory',
            function ($scope, DTOptionsBuilder, DTColumnDefBuilder, $uibModal, sharedProperties, noteItemFactory) {
                $scope.noteDTOptions = {
                    columnDefs: [
                        {
                            aTargets: [1, 2, 3, 4, 5],
                            class: "tableCellNoWrap"
                        },
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };
                ($scope.getNotes = function () {
                    $scope.inProgress = true;
                    noteItemFactory.getAll()
                    .then(function (response) {
                        $scope.notes = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();
                $scope.editNote = function (noteId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Notes/Edit.html",
                        controller: "editNoteItemCtrl",
                        backdrop: 'static',
                        windowClass: 'edit-note-dialog',
                        resolve: {
                            noteId: function () {
                                return noteId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getNotes();
                    });
                };
                $scope.attachDocument = function (entityType, entityId) {
                    var entityData = {
                        entityType: entityType,
                        entityId: entityId
                    };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Documents/AttachBlind.html",
                        controller: "attachDocumentBlindCtrl",
                        backdrop: "static",
                        resolve: {
                            entityData: function () {
                                return entityData;
                            }
                        }
                    });
                };
            }]);
})();
