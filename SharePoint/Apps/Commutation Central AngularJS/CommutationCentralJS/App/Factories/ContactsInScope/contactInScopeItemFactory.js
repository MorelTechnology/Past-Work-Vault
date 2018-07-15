(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('contactInScopeItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Contacts In Scope';
            var selectFields = 'ID,Project/ID,Project/CounterpartyName,Contact/ID,Contact/FullName,Contact/Title,Contact/FirstName,Contact/Email,Contact/WorkPhone,Contact/WorkCity,Contact/WorkState';
            var expandFields = "Project,Contact";
            var itemType = 'SP.Data.Contacts_x0020_In_x0020_ScopeListItem';
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
                    ProjectId: item.ProjectId,
                    ContactId: item.ContactId
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
                    ProjectId: item.ProjectId,
                    ContactId: item.ContactId
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