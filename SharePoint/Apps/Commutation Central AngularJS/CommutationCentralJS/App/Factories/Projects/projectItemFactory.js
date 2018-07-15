(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('projectItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Projects';
            var selectFields = 'ID,CounterpartyName,CommutationProjectID,PrimaryManagerID/Title,PrimaryManagerID/Name,SecondaryManagerID/Title,' +
                'SecondaryManagerID/Name,LeadOffice/ID,LeadOffice/LeadOfficeName,AssignedDate,Modified,CommutationStatus/ID,CommutationStatus/CommutationStatusName' +
                ',RequestorID/Name,RequestorID/Title,RequestType/ID,RequestType/RequestTypeName,RequestDate,CommutationType/ID,CommutationType/CommutationTypeName' +
                ',DroppedReason/ID,DroppedReason/DroppedReasonName,OversightManagerID/Title,OversightManagerID/Name,DealPriority/ID,DealPriority/DealPriorityName' +
                ',CompanyStatus/ID,CompanyStatus/CompanyStatusName,FinancialAuthorityGrantedBy/Title,FinancialAuthorityGrantedBy/Name,FinancialAuthorityGrantedByDate' +
                ',FinancialAuthorityAssumed,FinancialAuthorityCeded';
            var expandFields = "PrimaryManagerID,SecondaryManagerID,LeadOffice,CommutationStatus,RequestorID,RequestType,CommutationType,DroppedReason,OversightManagerID,DealPriority" +
                ",CompanyStatus,FinancialAuthorityGrantedBy";
            var itemType = 'SP.Data.ProjectsListItem';
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
                    CounterpartyName: item.CounterpartyName,
                    PrimaryManagerIDId: item.PrimaryManagerIDId,
                    RequestorIDId: item.RequestorIDId,
                    RequestTypeId: item.RequestTypeId,
                    RequestDate: new Date(item.RequestDate),
                    CommutationStatusId: '1',
                    DroppedReasonId: '1',
                    LeadOfficeId: '1',
                    CommutationTypeId: '1',
                    DealPriorityId: '2',
                    AssignedDate: new Date(),
                    CompanyStatusId: '1'
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
                    CounterpartyName: item.CounterpartyName,
                    CommutationProjectID: item.CommutationProjectID,
                    PrimaryManagerIDId: item.PrimaryManagerIDId,
                    SecondaryManagerIDId: item.SecondaryManagerIDId,
                    LeadOfficeId: item.LeadOfficeId,
                    RequestorIDId: item.RequestorIDId,
                    RequestTypeId: item.RequestTypeId,
                    RequestDate: item.RequestDate,
                    CommutationTypeId: item.CommutationTypeId,
                    CommutationStatusId: item.CommutationStatusId,
                    DroppedReasonId: item.DroppedReasonId,
                    OversightManagerIDId: item.OversightManagerIDId,
                    DealPriorityId: item.DealPriorityId,
                    CompanyStatusId: item.CompanyStatusId,
                    FinancialAuthorityGrantedById: item.FinancialAuthorityGrantedById,
                    FinancialAuthorityGrantedByDate: item.FinancialAuthorityGrantedByDate,
                    FinancialAuthorityAssumed: item.FinancialAuthorityAssumed,
                    FinancialAuthorityCeded: item.FinancialAuthorityCeded
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