(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addActivityItemCtrl', ["$scope", "activityItemFactory", "$location", "$uibModalInstance", "project", "projectItemFactory", "projectPreResolved", "activityCategoryItemFactory",
            "activityPriorityItemFactory", "siteUsersFactory", "activityDocumentItemFactory",
            function ($scope, itemFactory, $location, $uibModalInstance, project, projectItemFactory, projectPreResolved, activityCategoryItemFactory, activityPriorityItemFactory, siteUsersFactory, activityDocumentItemFactory) {
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

                (function getActivityCategories() {
                    activityCategoryItemFactory.getAll()
                    .then(function (resp) {
                        $scope.formData.dropDownChoices.activityCategories = resp.data.d.results;
                    });
                })();
                (function getActivityPriorities() {
                    activityPriorityItemFactory.getAll()
                    .then(function (resp) {
                        $scope.formData.dropDownChoices.activityPriorities = resp.data.d.results;
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

                $scope.addItem = function (activity) {
                    $scope.inProgress = true;
                    activity.ProjectId = $scope.project.ID;
                    activity.ActivityCategoryId = $scope.formData.selectedActivityCategory.Id;
                    activity.ActivityPriorityId = $scope.formData.selectedActivityPriority ? $scope.formData.selectedActivityPriority.Id : null;
                    activity.TaskDueDate = $scope.formData.selectedDueDate;
                    activity.InitialDueDate = $scope.formData.selectedDueDate;
                    siteUsersFactory.getByLoginName($scope.formData.selectedAssignedTo[0].Key)
                    .then(function (assignedTo) {
                        activity.AssignedToId = assignedTo.data.d.Id;
                        return itemFactory.addNew(activity);
                    })
                    .then(function (response) {
                        if ($scope.uploadFiles) {
                            angular.forEach($scope.uploadFiles, function (file) {
                                $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                                activityDocumentItemFactory.uploadFile(file, $scope.project.ID, response.data.d.Id)
                                .then(function (successResp) {
                                    $scope.activityDocumentUploadProgress = successResp;
                                    $uibModalInstance.close();
                                }, function (errorResp) {
                                    $scope.activityDocumentUploadProgress = errorResp;
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
                                    $scope.activityDocumentUploadProgress = notifyResp;
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
