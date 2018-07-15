(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('activityDroppedReasonItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Activity Dropped Reason Lookup';
            var getAll = function () {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=ID,ActivityDroppedReasonName";
                return baseService.getRequest(query);
            };
            var addNew = function (item) {
                var data = {
                    __metadata: {
                        'type': 'SP.Data.Activity_x0020_Dropped_x0020_Reason_x0020_LookupListItem'
                    },
                    ActivityDroppedReasonName: item.ActivityDroppedReasonName
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/Items";
                return baseService.postRequest(data, url);
            };
            var getById = function (itemId) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + itemId + ")?$select=ID,ActivityDroppedReasonName";
                return baseService.getRequest(query);
            };
            var update = function (item) {
                var data = {
                    __metadata: {
                        'type': 'SP.Data.Activity_x0020_Dropped_x0020_Reason_x0020_LookupListItem'
                    },
                    ActivityDroppedReasonName: item.ActivityDroppedReasonName
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