(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('activityItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Activities';
            var selectFields = 'ID,Description,Project/ID,Project/CounterpartyName,ActivityCategory/ID,ActivityCategory/ActivityCategoryName,EntryDate,AssignedTo/ID,AssignedTo/Title,AssignedTo/Name' +
                ',ActivityPriority/ID,ActivityPriority/ActivityPriorityName,ActivityStatus/ID,ActivityStatus/ActivityStatusName,TaskDueDate,ActivityStatusChangeDate,ActivityDroppedReason/ID' +
                ',ActivityDroppedReason/ActivityDroppedReasonName,InitialDueDate,Author/Title,Author/Name,Created,Editor/Title,Editor/Name,Modified';
            var expandFields = "Project,ActivityCategory,AssignedTo,ActivityPriority,ActivityStatus,ActivityDroppedReason,Author,Editor";
            var itemType = 'SP.Data.ActivitiesListItem';
            var getAll = function (filterText) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=" + selectFields;
                if (expandFields) query += "&$expand=" + expandFields;
                if (filterText) query += "&$filter=" + filterText;
                return baseService.getRequest(query);
            };
            var addNew = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    Description: item.Description,
                    ProjectId: item.ProjectId,
                    ActivityCategoryId: item.ActivityCategoryId,
                    EntryDate: new Date(),
                    AssignedToId: item.AssignedToId,
                    ActivityPriorityId: item.ActivityPriorityId ? item.ActivityPriorityId : '2',
                    ActivityStatusId: '1',
                    TaskDueDate: item.TaskDueDate,
                    ActivityStatusChangeDate: new Date(),
                    ActivityDroppedReasonId: 1,
                    InitialDueDate: item.InitialDueDate
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
                    Description: item.Description,
                    ProjectId: item.ProjectId,
                    ActivityCategoryId: item.ActivityCategoryId,
                    EntryDate: item.EntryDate,
                    AssignedToId: item.AssignedToId,
                    ActivityPriorityId: item.ActivityPriorityId,
                    ActivityStatusId: item.ActivityStatusId,
                    TaskDueDate: item.TaskDueDate,
                    ActivityStatusChangeDate: item.ActivityStatusChangeDate,
                    ActivityDroppedReasonId: item.ActivityDroppedReasonId,
                    InitialDueDate: item.InitialDueDate
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