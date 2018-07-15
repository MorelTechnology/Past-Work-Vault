(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('projectFinancialsFactory', ["baseItemFactory", "projectItemFactory", "financialEntryItemFactory", "fairfaxEntityInScopeItemFactory", "$q",
            function (baseItemFactory, projectItemFactory, financialEntryItemFactory, fairfaxEntityInScopeItemFactory, $q) {
            //$scope.financialEntries = {};
            var getProjectSummaryFinancials = function (projectId) {
                var deferred = $q.defer();
                fairfaxEntityInScopeItemFactory.getAll("Project/ID eq " + projectId)
                .then(function (response) {
                    var promises = [];
                    angular.forEach(response.data.d.results, function (entityInScope) {
                        promises.push(financialEntryItemFactory.getAll("FairfaxEntityInScope/ID eq " + entityInScope.ID));
                    });
                    $q.all(promises)
                    .then(function (responses) {
                        var summaryFinancials = {
                            TotalAssumed: 0,
                            TotalCeded: 0,
                            TotalNet: 0
                        };
                        angular.forEach(responses, function (entriesPerEntity) {
                            var relevantEntry = entriesPerEntity.data.d.results[0];
                            summaryFinancials.TotalAssumed = summaryFinancials.TotalAssumed - relevantEntry.PreliminaryAssumedUnpaid - relevantEntry.PreliminaryAssumedCase - relevantEntry.PreliminaryAssumedIBNR;
                            summaryFinancials.TotalCeded = summaryFinancials.TotalCeded + relevantEntry.PreliminaryCededUnpaid + relevantEntry.PreliminaryCededCase + relevantEntry.PreliminaryCededIBNR;
                            summaryFinancials.TotalNet = summaryFinancials.TotalNet + summaryFinancials.TotalCeded + summaryFinancials.TotalAssumed;
                        });
                        deferred.resolve(summaryFinancials);
                    })
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            };
            return {
                getProjectSummaryFinancials: getProjectSummaryFinancials
            };
        }]);
})();