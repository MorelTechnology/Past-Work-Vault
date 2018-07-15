(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('attachDocumentBlindCtrl', ["$scope", "$uibModalInstance", "entityData", "activityItemFactory", "projectItemFactory", "activityDocumentItemFactory", "projectDocumentItemFactory",
            "noteItemFactory", "noteDocumentItemFactory",
            function ($scope, $uibModalInstance, entityData, activityItemFactory, projectItemFactory, activityDocumentItemFactory, projectDocumentItemFactory, noteItemFactory, noteDocumentItemFactory) {
            ($scope.getEntityData = function () {
                switch (entityData.entityType) {
                    case "Project":
                        projectItemFactory.getById(entityData.entityId)
                        .then(function (response) {
                            $scope.fullEntity = response.data.d;
                            $scope.attachEntityTitle = response.data.d.CounterpartyName;
                        });
                        break;
                    case "Activity":
                        activityItemFactory.getById(entityData.entityId)
                        .then(function (response) {
                            $scope.fullEntity = response.data.d;
                            $scope.attachEntityTitle = response.data.d.Description;
                        });
                        break;
                    case "Note":
                        noteItemFactory.getById(entityData.entityId)
                        .then(function (response) {
                            $scope.fullEntity = response.data.d;
                            $scope.attachEntityTitle = response.data.d.NoteEntryType.NoteEntryTypeName;
                        });
                        break;
                    case "Checklist":
                        checklistItemFactory.getById(entityData.entityId)
                        .then(function (response) {
                            $scope.fullEntity = response.data.d;
                            $scope.attachEntityTitle = response.data.d.Title;
                        });
                        break;
                }
            })();
            $scope.documentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
            $scope.attach = function () {
                if ($scope.uploadFiles) {
                    angular.forEach($scope.uploadFiles, function (file) {
                        $scope.documentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                        var uploadTask = null;
                        switch (entityData.entityType) {
                            case "Project":
                                uploadTask = projectDocumentItemFactory.uploadFile(file, $scope.fullEntity.ID);
                                break;
                            case "Activity":
                                uploadTask = activityDocumentItemFactory.uploadFile(file, $scope.fullEntity.Project.ID, $scope.fullEntity.ID);
                                break;
                            case "Note":
                                uploadTask = noteDocumentItemFactory.uploadFile(file, $scope.fullEntity.Project.ID, $scope.fullEntity.ID);
                                break;
                            case "Checklist":
                                uploadTask = checklistDocumentItemFactory.uploadFile(file, $scope.fullEntity.Project.ID, $scope.fullEntity.ID);
                                break;
                        }

                        uploadTask
                        .then(function (successResp) {
                            $scope.documentUploadProgress = successResp;
                            $uibModalInstance.close();
                        }, function (errorResp) {
                            $scope.documentUploadProgress = errorResp;
                            var documentErrorNotify = $uibModal.open({
                                animation: true,
                                templateUrl: "../App/Templates/Errors/ModalError.html",
                                controller: "modalErrorCtrl",
                                resolve: {
                                    errorMessage: {
                                        value: "There was an error uploading some or all documents. You may need to verify which documents were uploaded successfully, and re-try any of the ones that failed to upload." +
                                            "The server encountered the following error:\n" + errorResp.result.data.error.message.value
                                    }
                                }
                            });
                            documentErrorNotify.result.then(function () {
                                $uibModalInstance.close();
                            });
                        }, function (notifyResp) {
                            $scope.documentUploadProgress = notifyResp;
                        });
                    });
                }
                else { $scope.cancel(); }
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
