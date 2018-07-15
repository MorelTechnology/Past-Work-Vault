(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('financialEntryItemFactory', ['baseItemFactory', function (baseService) {
            var listEndPoint = '/_api/web/lists';
            var listName = 'Financial Entries';
            var selectFields = 'ID,FairfaxEntityInScope/ID,PreliminaryValuationDate,PreliminaryAssumedUnpaid,PreliminaryAssumedCase,PreliminaryAssumedIBNR,PreliminaryCededUnpaid,PreliminaryCededCase' +
                ',PreliminaryCededIBNR,FinalValuationDate,FinalAssumedUnpaid,FinalAssumedReserves,FinalAssumedIBNR,FinalCededUnpaid,FinalCededReserves,FinalCededIBNR,FinalCommutationValue' +
                ',FinalCreditProvision,FinalDisputeProvision,FinalPenalties,FinalTransactionAssumed,FinalTransactionCeded,FinalTransactionBDPUsed,FinalTransactionCollateral';
            var expandFields = "FairfaxEntityInScope";
            var itemType = 'SP.Data.Financial_x0020_EntriesListItem';
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
                    FairfaxEntityInScopeId: item.FairfaxEntityInScopeId,
                    PreliminaryValuationDate: item.PreliminaryValuationDate,
                    PreliminaryAssumedUnpaid: item.PreliminaryAssumedUnpaid,
                    PreliminaryAssumedCase: item.PreliminaryAssumedCase,
                    PreliminaryAssumedIBNR: item.PreliminaryAssumedIBNR,
                    PreliminaryCededUnpaid: item.PreliminaryCededUnpaid,
                    PreliminaryCededCase: item.PreliminaryCededCase,
                    PreliminaryCededIBNR: item.PreliminaryCededIBNR,
                    FinalValuationDate: item.FinalValuationDate,
                    FinalAssumedUnpaid: item.FinalAssumedUnpaid,
                    FinalAssumedReserves: item.FinalAssumedReserves,
                    FinalAssumedIBNR: item.FinalAssumedIBNR,
                    FinalCededUnpaid: item.FinalCededUnpaid,
                    FinalCededReserves: item.FinalCededReserves,
                    FinalCededIBNR: item.FinalCededIBNR,
                    FinalCommutationValue: item.FinalCommutationValue,
                    FinalCreditProvision: item.FinalCreditProvision,
                    FinalDisputeProvision: item.FinalDisputeProvision,
                    FinalPenalties: item.FinalPenalties,
                    FinalTransactionAssumed: item.FinalTransactionAssumed,
                    FinalTransactionCeded: item.FinalTransactionCeded,
                    FinalTransactionBDPUsed: item.FinalTransactionBDPUsed,
                    FinalTransactionCollateral: item.FinalTransactionCollateral
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
                    FairfaxEntityInScopeId: item.FairfaxEntityInScopeId,
                    PreliminaryValuationDate: item.PreliminaryValuationDate,
                    PreliminaryAssumedUnpaid: item.PreliminaryAssumedUnpaid,
                    PreliminaryAssumedCase: item.PreliminaryAssumedCase,
                    PreliminaryAssumedIBNR: item.PreliminaryAssumedIBNR,
                    PreliminaryCededUnpaid: item.PreliminaryCededUnpaid,
                    PreliminaryCededCase: item.PreliminaryCededCase,
                    PreliminaryCededIBNR: item.PreliminaryCededIBNR,
                    FinalValuationDate: item.FinalValuationDate,
                    FinalAssumedUnpaid: item.FinalAssumedUnpaid,
                    FinalAssumedReserves: item.FinalAssumedReserves,
                    FinalAssumedIBNR: item.FinalAssumedIBNR,
                    FinalCededUnpaid: item.FinalCededUnpaid,
                    FinalCededReserves: item.FinalCededReserves,
                    FinalCededIBNR: item.FinalCededIBNR,
                    FinalCommutationValue: item.FinalCommutationValue,
                    FinalCreditProvision: item.FinalCreditProvision,
                    FinalDisputeProvision: item.FinalDisputeProvision,
                    FinalPenalties: item.FinalPenalties,
                    FinalTransactionAssumed: item.FinalTransactionAssumed,
                    FinalTransactionCeded: item.FinalTransactionCeded,
                    FinalTransactionBDPUsed: item.FinalTransactionBDPUsed,
                    FinalTransactionCollateral: item.FinalTransactionCollateral
                };
                var url = listEndPoint + "/GetByTitle('" + listName + "')/GetItemById(" + item.Id + ")";
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