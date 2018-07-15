(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('contactItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Contact Lookup';
            var selectFields = 'ID,Title,FirstName,FullName,Email,WorkPhone,WorkAddress,WorkCity,WorkState,Company,HomePhone';
            var itemType = 'SP.Data.Contact_x0020_LookupListItem';
            var getAll = function (filterText) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items?$select=" + selectFields;
                if (filterText) query += "&$filter=" + filterText;
                return baseService.getRequest(query);
            };
            var addNew = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    Title: item.Title,
                    LastNamePhonetic: item.LastNamePhonetic,
                    FirstName: item.FirstName,
                    FirstNamePhonetic: item.FirstNamePhonetic,
                    FullName: item.FullName,
                    Email: item.Email,
                    Company: item.Company,
                    CompanyPhonetic: item.CompanyPhonetic,
                    JobTitle: item.JobTitle,
                    WorkPhone: item.WorkPhone,
                    HomePhone: item.HomePhone,
                    CellPhone: item.CellPhone,
                    WorkFax: item.WorkFax,
                    WorkAddress: item.WorkAddress,
                    WorkCity: item.WorkCity,
                    WorkState: item.WorkState,
                    WorkZip: item.WorkZip,
                    WorkCountry: item.WorkCountry,
                    Comments: item.Comments
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/Items";
                return baseService.postRequest(data, url);
            };
            var getById = function (itemId) {
                var query = listEndPoint + "/GetByTitle('" + listName + "')/Items(" + itemId + ")?$select=" + selectFields;
                return baseService.getRequest(query);
            };
            var update = function (item) {
                var data = {
                    __metadata: {
                        'type': itemType
                    },
                    Title: item.Title,
                    LastNamePhonetic: item.LastNamePhonetic,
                    FirstName: item.FirstName,
                    FirstNamePhonetic: item.FirstNamePhonetic,
                    FullName: item.FullName,
                    Email: item.Email,
                    Company: item.Company,
                    CompanyPhonetic: item.CompanyPhonetic,
                    JobTitle: item.JobTitle,
                    WorkPhone: item.WorkPhone,
                    HomePhone: item.HomePhone,
                    CellPhone: item.CellPhone,
                    WorkFax: item.WorkFax,
                    WorkAddress: item.WorkAddress,
                    WorkCity: item.WorkCity,
                    WorkState: item.WorkState,
                    WorkZip: item.WorkZip,
                    WorkCountry: item.WorkCountry,
                    Comments: item.Comments
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