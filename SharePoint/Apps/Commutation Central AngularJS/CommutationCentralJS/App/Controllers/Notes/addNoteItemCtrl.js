(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addNoteItemCtrl', ["$scope", "noteItemFactory", "$location", "$uibModalInstance", "project", "projectItemFactory", "projectPreResolved", "noteEntryTypeItemFactory",
           "noteDocumentItemFactory",
            function ($scope, itemFactory, $location, $uibModalInstance, project, projectItemFactory, projectPreResolved, noteEntryTypeItemFactory, noteDocumentItemFactory) {
                $scope.project = {
                    ID: project.ID,
                    CounterpartyName: project.CounterpartyName
                };
                $scope.projectPreResolved = projectPreResolved;
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

                (function getNoteEntryTypes() {
                    noteEntryTypeItemFactory.getAll()
                    .then(function (resp) {
                        $scope.formData.dropDownChoices.noteEntryTypes = resp.data.d.results;
                    });
                })();
 

                $scope.getProjects = function (filterValue) {
                    return projectItemFactory.getAll("substringof('" + filterValue + "', CounterpartyName)")
                    .then(function (response) {
                        return response.data.d.results.map(function (project) {
                            return project;
                        });
                    });
                };

                $scope.addItem = function (note) {
                    $scope.inProgress = true;
                    note.ProjectId = $scope.project.ID;
                    note.NoteEntryTypeId = $scope.formData.selectedNoteEntryType.Id;
                    note.EntryDate = $scope.formData.selectedEntryDate;
                    var addNote = itemFactory.addNew(note)
                    .then(function (response) {
                        if ($scope.uploadFiles) {
                            angular.forEach($scope.uploadFiles, function (file) {
                                $scope.noteDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                                noteDocumentItemFactory.uploadFile(file, $scope.project.ID, response.data.d.Id)
                                .then(function (successResp) {
                                    $scope.noteDocumentUploadProgress = successResp;
                                    $uibModalInstance.close();
                                }, function (errorResp) {
                                    $scope.noteDocumentUploadProgress = errorResp;
                                    var documentErrorNotify = $uibModal.open({
                                        animation: true,
                                        templateUrl: "../App/Templates/Errors/ModalError.html",
                                        controller: "modalErrorCtrl",
                                        resolve: {
                                            errorMessage: {
                                                value: "There was an error uploading some or all documents. You may need to re-upload some documents once the note has been created." +
                                                    "The server encountered the following error:\n" + errorResp.result.data.error.message.value
                                            }
                                        }
                                    });
                                    documentErrorNotify.result.then(function () {
                                        $uibModalInstance.close();
                                    });
                                }, function (notifyResp) {
                                    $scope.noteDocumentUploadProgress = notifyResp;
                                });
                            });
                        } else {
                            $scope.inProgress = false;
                            $scope.newItem = response.d;
                            $uibModalInstance.close($scope.NewItem);
                        }
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

            }]);
})();
