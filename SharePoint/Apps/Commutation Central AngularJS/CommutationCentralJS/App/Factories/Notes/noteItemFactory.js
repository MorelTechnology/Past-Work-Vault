(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('noteItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Notes';
            var selectFields = 'ID,Project/ID,Project/CounterpartyName,NoteEntryType/ID,NoteEntryType/NoteEntryTypeName,NoteContent,EntryDate,Author/Name,Author/Title,Editor/Name,Editor/Title,Modified';
            var expandFields = "Project,NoteEntryType,Author,Editor";
            var itemType = 'SP.Data.NotesListItem';
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
                    NoteContent: item.NoteContent,
                    NoteEntryTypeId: item.NoteEntryTypeId,
                    EntryDate: item.EntryDate
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
                    NoteContent: item.NoteContent,
                    NoteEntryTypeId: item.NoteEntryTypeId,
                    EntryDate: item.EntryDate
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