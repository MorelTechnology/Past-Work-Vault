(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editNoteItemCtrl', ["$q", "$scope", "noteItemFactory", "$location", "$uibModalInstance", "projectItemFactory", "noteEntryTypeItemFactory", 
            "noteDocumentItemFactory", "noteId", "$uibModal",
            function ($q, $scope, itemFactory, $location, $uibModalInstance, projectItemFactory, noteEntryTypeItemFactory, noteDocumentItemFactory, noteId, $uibModal) {
                $scope.formData = {
                    dropDownChoices: {}
                };
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                $scope.calendarPopup1 = {
                    opened: false
                };
                $scope.openCalendarPopup1 = function () {
                    $scope.calendarPopup1.opened = true;
                };
                $scope.noteDocumentsDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };

                $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                
                $scope.getNote = function () {
                    $scope.inProgress = true;
                    itemFactory.getById(noteId)
                    .then(function (response) {
                        $scope.note = response.data.d;
                        $scope.getProject($scope.note.Project.ID);
                        $scope.getNoteDocuments();
                        if ($scope.note.NoteEntryType) $scope.formData.selectedNoteEntryType = $scope.formData.dropDownChoices.noteEntryTypes.filter(function (obj) { return obj.Id === $scope.note.NoteEntryType.ID; })[0];                        
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };

                $scope.getNoteDocuments = function () {
                    var noteId = $scope.note.Id;
                    $scope.noteDocumentUploadProgress = { percentComplete: 50, action: "Retrieving note documents", inProgress: true };
                    noteDocumentItemFactory.getAll("Note/ID eq " + noteId)
                    .then(function (response) {
                        $scope.noteDocuments = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                    });
                };

                $scope.getProject = function (projectId) {
                    $scope.inProgress = true;
                    projectItemFactory.getById(projectId)
                    .then(function (response) {
                        $scope.project = response.data.d;
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };

                ($scope.populateDropdowns = function () {
                    $scope.inProgress = true;
                    var promises = [];
                    promises.push(noteEntryTypeItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.noteEntryTypes = response.data.d.results;
                        }));
                    $q.all(promises).then(function () {
                        $scope.getNote();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();

                $scope.editItem = function (note) {
                    $scope.inProgress = true;
                    note.projectId = note.Project.ID;
                    note.noteEntryTypeId = $scope.formData.selectedNoteEntryType.Id;
                    note.noteContent = note.NoteContent;
                    note.itemId = note.Id;
                    return itemFactory.update(note)
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
                $scope.deleteNoteDocument = function (noteDocumentId) {
                    $scope.noteDocumentUploadProgress = { percentComplete: 100, action: "Deleting note document", inProgress: true };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this note document? Doing so will remove it from the system with no way to recover it."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            noteDocumentItemFactory.remove(noteDocumentId)
                            .then(function (successResponse) {
                                $scope.getNoteDocuments();
                            })
                            .finally(function () {
                                $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                            });
                        } else {
                            $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                        }
                    });
                };
                $scope.attachNoteDocuments = function (uploadFiles) {
                    if (uploadFiles && uploadFiles.length) {
                        angular.forEach(uploadFiles, function (file) {
                            $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                            noteDocumentItemFactory.uploadFile(file, $scope.note.Project.ID, $scope.note.Id)
                            .then(function (successResp) {
                                $scope.noteDocumentUploadProgress = successResp;
                                $scope.getNoteDocuments();
                            }, function (errorResp) {
                                $scope.noteDocumentUploadProgress = errorResp;
                            }, function (notifyResp) {
                                $scope.noteDocumentUploadProgress = notifyResp;
                            });
                        });
                    }
                };
            }]);
})();
