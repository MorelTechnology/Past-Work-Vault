(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('checklistItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Checklists';
            var selectFields = 'ID,Title,Project/ID,Project/CounterpartyName,ChecklistApplicable';
            var expandFields = "Project";
            var itemType = 'SP.Data.ChecklistsListItem';
            var getAll = function (filterText) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=" + selectFields;
                if (expandFields) query += "&$expand=" + expandFields;
                if (filterText) query += "&$filter=" + filterText;
                return baseService.getRequest(query);
            };

            var addProjectChecklist = function (projectId) {
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

                // Loop through the array adding each checklist item to the project.
                for (var i = 0; i < checklistItems.length; i++) {
                    var data = {
                        __metadata: {
                            'type': itemType
                        },
                        ProjectId: projectId,
                        Title: checklistItems[i]
                    };
                    var url = listEndPoint + "/GetByTitle('" + listName + "')/Items";
                    var results = baseService.postRequest(data, url);
                }
                return true;
            };

            var addNew = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    ProjectId: item.ProjectId,
                    Title: item.Title,
                    ChecklistApplicable: item.ChecklistApplicable
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/Items";
                return baseService.postRequest(data, url);
            };

            var getById = function (itemId) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items(" + itemId + ")?$select=" + selectFields;
                if (expandFields) query += "&$expand=" + expandFields;
                return baseService.getRequest(query);
            };
            var update = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    ChecklistApplicable: item.ChecklistApplicable
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + item.itemId + ")";
                return baseService.updateRequest(data, url);
            };
            var remove = function (itemId) {
                var url = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + itemId + ")";
                return baseService.deleteRequest(url);
            };
            return {
                getAll: getAll,
                addNew: addNew,
                getById: getById,
                update: update,
                remove: remove
            };
        }]);
})();