(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('siteUsersFactory', ['baseItemFactory', '$http', '$q', '$uibModal', function (baseService, $http, $q, $uibModal) {
            var baseUrl = _spPageContextInfo.webAbsoluteUrl;
            var endPoint = '/_api/web/siteusers';
            var currentUserEndpoint = '/_api/web/currentUser';
            var ensureUserEndpoint = '/_api/web/ensureuser';
            var getAll = function () {
                var query = endPoint;
                return baseService.getRequest(query);
            };
            var getByLoginName = function (loginName) {
                /*var query = endPoint + "(@v)?@v='" + encodeURIComponent(loginName) + "'";
                return baseService.getRequest(query);*/
                var deferred = $q.defer();
                var userPayload = { 'logonName': loginName };
                baseService.postRequest(userPayload, ensureUserEndpoint)
                .then(function (result) {
                    var query = endPoint + "(@v)?@v='" + encodeURIComponent(loginName) + "'";
                    baseService.getRequest(query)
                    .then(function (result) {
                        deferred.resolve(result);
                    }, function (error) {
                        deferred.reject(error);
                    });
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            };
            var getCurrentUser = function () {
                var query = currentUserEndpoint;
                return baseService.getRequest(query);
            };

            return {
                getAll: getAll,
                getByLoginName: getByLoginName,
                getCurrentUser: getCurrentUser
            };
        }]);
})();