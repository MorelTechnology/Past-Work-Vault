(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editActivityItemCtrl', ["$q", "$scope", "activityItemFactory", "$location", "$uibModalInstance", "projectItemFactory", "activityCategoryItemFactory", "activityStatusItemFactory",
            "activityPriorityItemFactory", "siteUsersFactory", "activityDocumentItemFactory", "activityId", "activityDroppedReasonItemFactory", "$uibModal",
            function ($q, $scope, itemFactory, $location, $uibModalInstance, projectItemFactory, activityCategoryItemFactory, activityStatusItemFactory, activityPriorityItemFactory, siteUsersFactory,
                activityDocumentItemFactory, activityId, activityDroppedReasonItemFactory, $uibModal) {
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
                $scope.activityDocumentsDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };
                $scope.showDroppedReason = false;

                $scope.showHideDroppedReason = function (value) {
                    var i = value;
                };
                $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                
                $scope.getActivity = function () {
                    $scope.inProgress = true;
                    itemFactory.getById(activityId)
                    .then(function (response) {
                        $scope.activity = response.data.d;
                        $scope.getProject($scope.activity.Project.ID);
                        $scope.getActivityDocuments();
                        if ($scope.activity.ActivityCategory) $scope.formData.selectedActivityCategory = $scope.formData.dropDownChoices.activityCategories.filter(function (obj) { return obj.Id === $scope.activity.ActivityCategory.ID; })[0];
                        if ($scope.activity.AssignedTo) $scope.formData.selectedAssignedTo = [
                            {
                                Key: $scope.activity.AssignedTo.Name,
                                DisplayText: $scope.activity.AssignedTo.Title
                            }
                        ];
                        if ($scope.activity.ActivityPriority) $scope.formData.selectedActivityPriority = $scope.formData.dropDownChoices.activityPriorities.filter(function (obj) { return obj.Id === $scope.activity.ActivityPriority.ID; })[0];
                        if ($scope.activity.TaskDueDate) $scope.formData.selectedDueDate = new Date($scope.activity.TaskDueDate);
                        if ($scope.activity.ActivityStatus) $scope.formData.selectedActivityStatus = $scope.formData.dropDownChoices.activityStatuses.filter(function (obj) { return obj.Id === $scope.activity.ActivityStatus.ID; })[0];
                        if ($scope.activity.ActivityDroppedReason) $scope.formData.selectedActivityDroppedReason = $scope.formData.dropDownChoices.activityDroppedReasons.filter(function (obj) { return obj.Id === $scope.activity.ActivityDroppedReason.ID; })[0];
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };

                $scope.getActivityDocuments = function () {
                    var activityId = $scope.activity.Id;
                    $scope.activityDocumentUploadProgress = { percentComplete: 50, action: "Retrieving activity documents", inProgress: true };
                    activityDocumentItemFactory.getAll("Activity/ID eq " + activityId)
                    .then(function (response) {
                        $scope.activityDocuments = response.data.d.results;
                        /*for (var i = 0; i < $scope.activityDocuments.length; i++) {
                            $scope.activityDocuments[i].FormData = { Modified: formatDate(new Date($scope.activityDocuments[i].Modified)), Created: formatDate(new Date($scope.projectDocuments[i].Created)) };
                        }*/
                    })
                    .finally(function () {
                        $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
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
                    promises.push(activityCategoryItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.activityCategories = response.data.d.results;
                        }));
                    promises.push(activityPriorityItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.activityPriorities = response.data.d.results;
                        }));
                    promises.push(activityStatusItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.activityStatuses = response.data.d.results;
                        }));
                    promises.push(activityDroppedReasonItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.activityDroppedReasons = response.data.d.results;
                        }));
                    $q.all(promises).then(function () {
                        $scope.getActivity();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                })();

                $scope.editItem = function (activity) {
                    $scope.inProgress = true;
                    activity.projectId = activity.Project.ID;
                    activity.ActivityCategoryId = $scope.formData.selectedActivityCategory.Id;
                    activity.ActivityPriorityId = $scope.formData.selectedActivityPriority.Id;
                    activity.ActivityStatusId = $scope.formData.selectedActivityStatus.Id;
                    activity.TaskDueDate = $scope.formData.selectedDueDate;
                    activity.ActivityStatus.ID === $scope.formData.selectedActivityStatus.Id ? activity.ActivityStatusChangeDate = activity.ActivityStatusChangeDate : activity.ActivityStatusChangeDate = new Date();
                    activity.InitialDueDate ? activity.InitialDueDate = activity.InitialDueDate : activity.InitialDueDate = $scope.formData.selectedDueDate;
                    activity.itemId = activity.Id;
                    siteUsersFactory.getByLoginName($scope.formData.selectedAssignedTo[0].Key)
                    .then(function (assignedTo) {
                        activity.AssignedToId = assignedTo.data.d.Id;
                        return itemFactory.update(activity);
                    })
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
                $scope.deleteActivityDocument = function (activityDocumentId) {
                    $scope.activityDocumentUploadProgress = { percentComplete: 100, action: "Deleting activity document", inProgress: true };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this activity document? Doing so will remove it from the system with no way to recover it."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            activityDocumentItemFactory.remove(activityDocumentId)
                            .then(function (successResponse) {
                                $scope.getActivityDocuments();
                            })
                            .finally(function () {
                                $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                            });
                        } else {
                            $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                        }
                    });
                };
                $scope.attachActivityDocuments = function (uploadFiles) {
                    if (uploadFiles && uploadFiles.length) {
                        angular.forEach(uploadFiles, function (file) {
                            $scope.activityDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                            activityDocumentItemFactory.uploadFile(file, $scope.activity.Project.ID, $scope.activity.Id)
                            .then(function (successResp) {
                                $scope.activityDocumentUploadProgress = successResp;
                                $scope.getActivityDocuments();
                            }, function (errorResp) {
                                $scope.activityDocumentUploadProgress = errorResp;
                            }, function (notifyResp) {
                                $scope.activityDocumentUploadProgress = notifyResp;
                            });
                        });
                    }
                };
            }]);
})();
