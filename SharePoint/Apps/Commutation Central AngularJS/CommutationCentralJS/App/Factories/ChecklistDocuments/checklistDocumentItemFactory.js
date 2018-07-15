(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('checklistDocumentItemFactory', ['baseItemFactory', '$q', '$http', 'Upload', '$uibModal', 'sharedProperties', function (baseService, $q, $http, Upload, $uibModal, sharedProperties) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Commutation Documents';
            var selectFields = '*,Checklist/ID,Author/Title,Editor/Title,Project/ID';
            var expandFields = "Checklist,File,Author,Editor,Project";
            var itemType = 'SP.Data.Commutation_x0020_DocumentsListItem';
            var contentTypeId = '0x0100B657747FA1744D95B9A09BF1191C7641';
            var getAll = function (filterText) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=" + selectFields;
                if (expandFields) query += "&$expand=" + expandFields;
                if (filterText) query += "&$filter=" + filterText + " and startswith(ContentTypeId, '" + contentTypeId + "')";
                return baseService.getRequest(query);
            };
            var addNew = function (file, folder, digest) {
                return Upload.http({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/GetFolderByServerRelativeUrl('Lists/Commutation Documents/" + folder + "')/Files/Add(overwrite=true, url='" + file.name + "')",
                    headers: {
                        "accept": "application/json;odata=verbose",
                        "X-RequestDigest": digest
                    },
                    data: file
                });
            };
            var createFolder = function (folderName) {
                var query = "/_api/Web/Folders/add('Lists/Commutation Documents/" + folderName + "')";
                return baseService.postRequest(null, query);
            };
            var getById = function (itemId) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items(" + itemId + ")?$select=" + selectFields;
                if (expandFields) query += "&$expand=" + expandFields;
                return baseService.getRequest(query);
            };
            var getByAbsoluteUrl = function (itemUrl) {
                var query = itemUrl;
                return baseService.getAbsoluteRequest(query);
            };
            var update = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    ChecklistId: item.ChecklistId,
                    ProjectId: item.ProjectId,
                    ContentTypeId: contentTypeId
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + item.Id + ")";
                return baseService.updateRequest(data, url);
            };
            var remove = function (itemId) {
                var url = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + itemId + ")";
                return baseService.deleteRequest(url);
            };
            var uploadFile = function (file, projectId, checklistId) {
                var deferred = $q.defer();
                deferred.notify({ percentComplete: 0, action: "Creating project folder...", inProgress: true });
                createFolder(projectId)
                .then(function (result) {
                    deferred.notify({ percentComplete: 0, action: "Project folder created successfully", inProgress: true });
                    deferred.notify({ percentComplete: 0, action: "Creating checklist documents subfolder...", inProgress: true });
                    createFolder(projectId + "/Checklist Documents")
                    .then(function (result) {
                        deferred.notify({ percentComplete: 0, action: "Checklist documents subfolder created successfully", inProgress: true });
                        deferred.notify({ percentComplete: 0, action: "Uploading document...", inProgress: true });
                        baseService.digest()
                        .then(function (digest) {
                            addNew(file, projectId + "/Checklist Documents", digest)
                            .then(function (result) {
                                deferred.notify({ percentComplete: 100, action: "Document uploaded successfully", inProgress: true });
                                deferred.notify({ percentComplete: 100, action: "Retrieving document to update metadata...", inProgress: true });
                                getByAbsoluteUrl(result.data.d.ListItemAllFields.__deferred.uri)
                                .then(function (result) {
                                    deferred.notify({ percentComplete: 100, action: "Document retrieved successfully", inProgress: true });
                                    deferred.notify({ percentComplete: 100, action: "Updating document metadata...", inProgress: true });
                                    result.data.d.ChecklistId = checklistId;
                                    result.data.d.ProjectId = projectId;
                                    update(result.data.d)
                                    .then(function (result) {
                                        deferred.notify({ percentComplete: 100, action: "Document metadata updated successfully", inProgress: true });
                                        deferred.resolve({ percentComplete: 100, action: "Document metadata updated successfully", inProgress: false, result: result });
                                    }, function (error) {
                                        deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                                    });
                                }, function (error) {
                                    deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                                });
                            }, function (error) {
                                deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                            }, function (updates) {
                                var percentComplete = parseInt(100.0 * updates.loaded / updates.total);
                                deferred.notify({ percentComplete: percentComplete, action: "Uploading document...", inProgress: true });
                            });
                        }, function (error) {
                            deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                        });
                    }, function (error) {
                        deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                    });
                }, function (error) {
                    deferred.reject({ percentComplete: 0, action: null, inProgress: false, result: error });
                });

                return deferred.promise;
            };
            return {
                getAll: getAll,
                addNew: addNew,
                createFolder: createFolder,
                uploadFile: uploadFile,
                getById: getById,
                getByAbsoluteUrl: getByAbsoluteUrl,
                update: update,
                remove: remove
            };
        }]);
})();