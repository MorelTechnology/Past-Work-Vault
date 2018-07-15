(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addChecklistItemCtrl', ["$scope", "checklistItemFactory", "$location", "$uibModalInstance", "project", "projectItemFactory", "projectPreResolved", "checklistApplicableItemFactory",
            "checklistDocumentItemFactory",
            function ($scope, itemFactory, $location, $uibModalInstance, project, projectItemFactory, projectPreResolved, checklistApplicableItemFactory, checklistDocumentItemFactory) {
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

                (function getChecklistApplicables() {
                    checklistApplicableItemFactory.getAll()
                    .then(function (resp) {
                        $scope.formData.dropDownChoices.getChecklistApplicables = resp.data.d.results;
                    });
                })();

                $scope.getProjects = function (filterValue) {
                    return projectItemFactory.getAll("substringof('" + filterValue + "', CounterpartyName)")
                    .then(function (response) {
                        return response.data.d.results.map(function (project) {
                            return project.CounterpartyName;
                        });
                    });
                };

                $scope.addItem = function (checklist) {
                    $scope.inProgress = true;
                    checklist.Title = $scope.title;
                    checklist.ProjectId = $scope.project.ID;
                    checklist.ChecklistApplicable = $scope.formData.selectedChecklistApplicable.Id;
                    var addChecklist = itemFactory.addNew(checklist)
                    .then(function (response) {
                        if ($scope.uploadFiles) {
                            angular.forEach($scope.uploadFiles, function (file) {
                                $scope.checklistDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                                checklistDocumentItemFactory.uploadFile(file, $scope.project.ID, response.data.d.Id)
                                .then(function (successResp) {
                                    $scope.checklistDocumentUploadProgress = successResp;
                                    $uibModalInstance.close();
                                }, function (errorResp) {
                                    $scope.checklistDocumentUploadProgress = errorResp;
                                    var documentErrorNotify = $uibModal.open({
                                        animation: true,
                                        templateUrl: "../App/Templates/Errors/ModalError.html",
                                        controller: "modalErrorCtrl",
                                        resolve: {
                                            errorMessage: {
                                                value: "There was an error uploading some or all documents. You may need to re-upload some documents once the activity has been created." +
                                                    "The server encountered the following error:\n" + errorResp.result.data.error.message.value
                                            }
                                        }
                                    });
                                    documentErrorNotify.result.then(function () {
                                        $uibModalInstance.close();
                                    });
                                }, function (notifyResp) {
                                    $scope.checklistDocumentUploadProgress = notifyResp;
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
