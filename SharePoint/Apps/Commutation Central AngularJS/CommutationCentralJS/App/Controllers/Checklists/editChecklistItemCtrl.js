(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editChecklistItemCtrl', ["$q", "$scope", "checklistItemFactory", "$location", "$uibModalInstance", "projectItemFactory", "checklistDocumentItemFactory",
            "checklistId", "$uibModal",
            function ($q, $scope, itemFactory, $location, $uibModalInstance, projectItemFactory, checklistDocumentItemFactory, checklistId, $uibModal) {

                $scope.formData = {
                    dropDownChoices: {}
                };

                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };

                $scope.checklistDocumentsDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };

                $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                
                $scope.getChecklist = function () {
                    $scope.inProgress = true;
                    itemFactory.getById(checklistId)
                    .then(function (response) {
                        $scope.checklist = response.data.d;
                        $scope.getProject($scope.checklist.Project.ID);
                        $scope.getChecklistDocuments();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };

                $scope.getChecklistDocuments = function () {
                    var checklistId = $scope.checklist.Id;
                    $scope.checklistDocumentUploadProgress = { percentComplete: 50, action: "Retrieving checklist documents", inProgress: true };
                    checklistDocumentItemFactory.getAll("Checklist/ID eq " + checklistId)
                    .then(function (response) {
                        $scope.checklistDocuments = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
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
                    $q.all(promises).then(function () {
                        $scope.getChecklist();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();

                $scope.editItem = function (checklist) {
                    $scope.inProgress = true;
                    checklist.projectId = checklist.Project.ID;
                    checklist.ChecklistApplicable = checklist.ChecklistApplicable;
                    checklist.itemId = checklist.Id;
                    checklist.title = checklist.Title;
                    return itemFactory.update(checklist).then(function (response) {
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.deleteChecklistDocument = function (checklistDocumentId) {
                    $scope.checklistDocumentUploadProgress = { percentComplete: 100, action: "Deleting checklist document", inProgress: true };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this checklist document? Doing so will remove it from the system with no way to recover it."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            checklistDocumentItemFactory.remove(checklistDocumentId)
                            .then(function (successResponse) {
                                $scope.getChecklistDocuments();
                            })
                            .finally(function () {
                                $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                            });
                        } else {
                            $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                        }
                    });
                };
                $scope.attachChecklistDocuments = function (uploadFiles) {
                    if (uploadFiles && uploadFiles.length) {
                        angular.forEach(uploadFiles, function (file) {
                            $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                            checklistDocumentItemFactory.uploadFile(file, $scope.checklist.Project.ID, $scope.checklist.Id)
                            .then(function (successResp) {
                                $scope.checklistDocumentUploadProgress = successResp;
                                $scope.getChecklistDocuments();
                            }, function (errorResp) {
                                $scope.checklistDocumentUploadProgress = errorResp;
                            }, function (notifyResp) {
                                $scope.checklistDocumentUploadProgress = notifyResp;
                            });
                        });
                    }
                };
            }]);
})();
