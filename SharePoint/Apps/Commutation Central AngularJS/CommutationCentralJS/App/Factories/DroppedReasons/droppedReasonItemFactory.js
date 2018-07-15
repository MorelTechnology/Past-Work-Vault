(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('droppedReasonItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Dropped Reason Lookup';
            var selectFields = 'ID,DroppedReasonName';
            var itemType = 'SP.Data.Dropped_x0020_Reason_x0020_LookupListItem';
            var getAll = function () {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=" + selectFields;
                return baseService.getRequest(query);
            };
            var addNew = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    DroppedReasonName: item.DroppedReasonName
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/Items";
                return baseService.postRequest(data, url);
            };
            var getById = function (itemId) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + itemId + ")?$select=" + selectFields;
                return baseService.getRequest(query);
            };
            var update = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    DroppedReasonName: item.DroppedReasonName
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