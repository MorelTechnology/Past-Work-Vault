(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('baseItemFactory', ['$http', '$q', 'sharedProperties', '$uibModal',
            function ($http, $q, sharedProperties, $uibModal) {
                var baseUrl = _spPageContextInfo.webAbsoluteUrl;
                UpdateFormDigest(_spPageContextInfo.webAbsoluteUrl, _spFormDigestRefreshInterval);
                var getRequest = function (query) {
                    var deferred = $q.defer();
                    $http({
                        url: baseUrl + query,
                        method: "GET",
                        headers: {
                            "accept": "application/json;odata=verbose",
                            "content-Type": "application/json;odata=verbose"
                        }
                    })
                    .then(function (result) {
                        deferred.resolve(result);
                    }, function (result) {
                        deferred.reject(result);
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: "../App/Templates/Errors/ModalError.html",
                            controller: "modalErrorCtrl",
                            resolve: {
                                errorMessage: {
                                    value: result.data.error.message.value
                                }
                            }
                        });
                    }, function (result) {
                        return result;
                    });
                    return deferred.promise;
                };
                var getAbsoluteRequest = function (query) {
                    var deferred = $q.defer();
                    $http({
                        url: query,
                        method: "GET",
                        headers: {
                            "accept": "application/json;odata=verbose",
                            "content-Type": "application/json;odata=verbose"
                        }
                    })
                    .then(function (result) {
                        deferred.resolve(result);
                    }, function (result) {
                        deferred.reject(result);
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: "../App/Templates/Errors/ModalError.html",
                            controller: "modalErrorCtrl",
                            resolve: {
                                errorMessage: {
                                    value: result.data.error.message.value
                                }
                            }
                        });
                    });
                    return deferred.promise;
                };
                var postRequest = function (data, url) {
                    var deferred = $q.defer();
                    digest()
                    .then(function (digest) {
                        $http({
                            url: baseUrl + url,
                            method: "POST",
                            headers: {
                                "accept": "application/json;odata=verbose",
                                "X-RequestDigest": digest,
                                "content-Type": "application/json;odata=verbose"
                            },
                            data: data ? JSON.stringify(data) : undefined
                        })
                        .then(function (result) {
                            deferred.resolve(result);
                        }, function (result) {
                            deferred.reject(result);
                            var modalInstance = $uibModal.open({
                                animation: true,
                                templateUrl: "../App/Templates/Errors/ModalError.html",
                                controller: "modalErrorCtrl",
                                resolve: {
                                    errorMessage: {
                                        value: result.data.error.message.value
                                    }
                                }
                            });
                        });
                    }, function (error) {
                        deferred.reject(error);
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: "../App/Templates/Errors/ModalError.html",
                            controller: "modalErrorCtrl",
                            resolve: {
                                errorMessage: {
                                    value: error.data.error.message.value
                                }
                            }
                        });
                    });

                    return deferred.promise;
                };
                var updateRequest = function (data, url) {
                    var deferred = $q.defer();
                    digest()
                    .then(function (digest) {
                        $http({
                            url: baseUrl + url,
                            method: "PATCH",
                            headers: {
                                "accept": "application/json;odata=verbose",
                                "X-RequestDigest": digest,
                                "content-Type": "application/json;odata=verbose",
                                "X-Http-Method": "PATCH",
                                "If-Match": "*"
                            },
                            data: JSON.stringify(data)
                        })
                        .then(function (result) {
                            deferred.resolve(result);
                        }, function (result) {
                            deferred.reject(result);
                            var modalInstance = $uibModal.open({
                                animation: true,
                                templateUrl: "../App/Templates/Errors/ModalError.html",
                                controller: "modalErrorCtrl",
                                resolve: {
                                    errorMessage: {
                                        value: result.data.error.message.value
                                    }
                                }
                            });
                        });
                    }, function (error) {
                        deferred.reject(error);
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: "../App/Templates/Errors/ModalError.html",
                            controller: "modalErrorCtrl",
                            resolve: {
                                errorMessage: {
                                    value: error.data.error.message.value
                                }
                            }
                        });
                    });
                    
                    return deferred.promise;
                };
                var deleteRequest = function (url) {
                    var deferred = $q.defer();
                    digest()
                    .then(function (digest) {
                        $http({
                            url: baseUrl + url,
                            method: "DELETE",
                            headers: {
                                "accept": "application/json;odata=verbose",
                                "X-RequestDigest": digest,
                                "IF-MATCH": "*"
                            }
                        })
                        .then(function (result) {
                            deferred.resolve(result);
                        }, function (result) {
                            deferred.reject(result);
                            var modalInstance = $uibModal.open({
                                animation: true,
                                templateUrl: "../App/Templates/Errors/ModalError.html",
                                controller: "modalErrorCtrl",
                                resolve: {
                                    errorMessage: {
                                        value: result.data.error.message.value
                                    }
                                }
                            });
                        });
                    }, function (error) {
                        deferred.reject(error);
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: "../App/Templates/Errors/ModalError.html",
                            controller: "modalErrorCtrl",
                            resolve: {
                                errorMessage: {
                                    value: error.data.error.message.value
                                }
                            }
                        });
                    });
                    
                    return deferred.promise;
                };
                var digest = function () {
                    var deferred = $q.defer();
                    $http({
                        url: baseUrl + '/_api/contextinfo',
                        method: 'POST',
                        headers: {
                            'accept': 'application/json;odata=verbose',
                            'content-type': 'application/json;odata=verbose'
                        }
                    })
                    .then(function (data) {
                        if (typeof data.data.d.GetContextWebInformation.FormDigestValue !== 'undefined') {
                            deferred.resolve(data.data.d.GetContextWebInformation.FormDigestValue);
                        } else {
                            var errorMsg = 'The security validation for this page has expired. Please refresh the page and try making your changes again.';
                            deferred.reject(errorMsg);
                        }
                    }, function (err) {
                        deferred.reject(err);
                    });
                    return deferred.promise;
                };
                return {
                    getRequest: getRequest,
                    getAbsoluteRequest: getAbsoluteRequest,
                    postRequest: postRequest,
                    updateRequest: updateRequest,
                    deleteRequest: deleteRequest,
                    digest: digest
                };
            }]);
})();