(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addProjectItemCtrl', ["$scope", "projectItemFactory", "siteUsersFactory", "$location", "$uibModalInstance", "requestTypeItemFactory", "projectDocumentItemFactory", "$q", "$uibModal",
            "checklistItemFactory",
            function ($scope, itemFactory, siteUsersFactory, $location, $uibModalInstance, requestTypeFactory, projectDocumentItemFactory, $q, $uibModal, checklistItemFactory) {
                (function populateDropDowns() {
                    var promises = [];
                    $scope.inProgress = true;
                    promises.push(requestTypeFactory.getAll()
                    .then(function (response) {
                        $scope.requestTypes = response.data.d.results;
                    }));

                    $q.all(promises).then(function () {
                        $scope.inProgress = false;
                    });
                })();
                /*requestTypeFactory.getAll()
                    .then(function (response) {
                        $scope.requestTypes = response.d.results;
                    });*/
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
                $scope.addItem = function (project) {
                    $scope.project = project;
                    $scope.inProgress = true;
                    siteUsersFactory.getByLoginName(project.PrimaryManagerID[0].Key)
                    .then(function (primaryManager) {
                        $scope.project.PrimaryManagerIDId = primaryManager.data.d.Id;
                        return siteUsersFactory.getByLoginName(project.RequestorID[0].Key);
                    })
                    .then(function (requestor) {
                        $scope.project.RequestorIDId = requestor.data.d.Id;
                        return itemFactory.addNew($scope.project);
                    })
                    .then(function (response) {
                        // Create an array of checklist items to add to the project.
                        var checklistItems = [
                            "Collateral Releases",
                            "Confirm Wire Transfer",
                            "Deal Checklist",
                            "Deal Memo / Briefing Book",
                            "Post Comm Pro Forma",
                            "Regulatory Approval",
                            "Release Agreement",
                            "SOX Binder Scanned & Filed",
                            "Written Approval"
                        ];

                        // Create a checklist object to use in the loop below.
                        var checklistItem = {
                            Title: "",
                            ProjectId: response.data.d.Id
                        };

                        // Loop through the array adding each checklist item to the project.
                        for (var i = 0; i < checklistItems.length; i++) {
                            checklistItem.Title = checklistItems[i];
                            checklistItemFactory.addNew(checklistItem);
                        }

                        if ($scope.uploadFiles) {
                            angular.forEach($scope.uploadFiles, function (file) {
                                $scope.projectDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                                projectDocumentItemFactory.uploadFile(file, response.data.d.Id)
                                .then(function (successResp) {
                                    $scope.projectDocumentUploadProgress = successResp;
                                    $uibModalInstance.close();
                                }, function (errorResp) {
                                    $scope.projectDocumentUploadProgress = errorResp;
                                    var documentErrorNotify = $uibModal.open({
                                        animation: true,
                                        templateUrl: "../App/Templates/Errors/ModalError.html",
                                        controller: "modalErrorCtrl",
                                        resolve: {
                                            errorMessage: {
                                                value: "There was an error uploading some or all documents. You may need to re-upload some documents once the project has been created." +
                                                    "The server encountered the following error:\n" + errorResp.result.data.error.message.value
                                            }
                                        }
                                    });
                                    documentErrorNotify.result.then(function () {
                                        $uibModalInstance.close();
                                    });
                                }, function (notifyResp) {
                                    $scope.projectDocumentUploadProgress = notifyResp;
                                });
                            });
                        } else {
                            $scope.inProgress = false;
                            $scope.newItem = response.d;
                            $uibModalInstance.close($scope.NewItem);
                        }
                    }, function (errorResponse) {
                        $scope.inProgress = false;
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
