(function () {
    'use strict';

    /*define([
        'angular'
    ]), function (angular) {*/
        return angular.module('pluggableViews', [])
            .provider('$pluggableViews', [
                '$controllerProvider',
                '$compileProvider',
                '$filterProvider',
                '$provide',
                '$injector',
                '$routeProvider',
                function ($controllerProvider, $compileProvider, $filterProvider, $provide, $injector, $routeProvider) {
                    var providers = {
                        $compileProvider: $compileProvider,
                        $controllerProvider: $controllerProvider,
                        $filterProvider: $filterProvider,
                        $provide: $provide
                    };
                    this.views = [];

                    var pluggableViews = this;

                    this.registerModule = function (moduleName) {
                        var module = angular.module(moduleName);

                        if (module.requires) {
                            for (var i = 0; i < module.requires.length; i++) {
                                this.registerModule(module.requires[i]);
                            }
                        }

                        angular.forEach(module._invokeQueue, function (invokeArgs) {
                            var provider = providers[invokeArgs[0]];
                            provider[invokeArgs[1]].apply(provider, invokeArgs[2]);
                        });
                        angular.forEach(module._configBlocks, function (fn) {
                            $injector.invoke(fn);
                        });
                        angular.forEach(module._runBlocks, function (fn) {
                            $injector.invoke(fn);
                        });
                    };

                    this.toTitleCase = function (str) {
                        return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
                    };

                    this.registerView = function (viewConfig) {
                        if (!viewConfig.viewUrl) {
                            viewConfig.viewUrl = '/' + viewConfig.ID;
                        }
                        if (!viewConfig.templateUrl) {
                            viewConfig.templateUrl = 'views/' + viewConfig.ID + '/' + viewConfig.ID + '.html';
                        }
                        if (!viewConfig.controller) {
                            viewConfig.controller = this.toTitleCase(viewConfig.ID) + 'Controller';
                        }
                        if (!viewConfig.navigationText) {
                            viewConfig.navigationText = this.toTitleCase(viewConfig.ID);
                        }
                        if (!viewConfig.requirejsName) {
                            viewConfig.requirejsName = viewConfig.ID;
                        }
                        if (!viewConfig.moduleName) {
                            viewConfig.moduleName = viewConfig.ID;
                        }
                        if (!viewConfig.cssId) {
                            viewConfig.cssId = viewConfig.ID + "-css";
                        }
                        if (!viewConfig.cssUrl) {
                            viewConfig.cssUrl = 'views/' + viewConfig.ID + '/' + viewConfig.ID + '.css';
                        }

                        this.views.push(viewConfig);

                        $routeProvider.when(viewConfig.viewUrl, {
                            templateUrl: viewConfig.templateUrl,
                            controller: viewConfig.controller,
                            resolve: {
                                resolver: ["$q", "$timeout", function ($q, $timeout) {
                                    var deferred = $q.defer();
                                    if (angular.element("#" + viewConfig.cssId).length == 0) {
                                        var link = document.createElement('link');
                                        link.id = viewConfig.cssId;
                                        link.rel = "stylesheet";
                                        link.type = "text/css";
                                        link.href = viewConfig.cssUrl;
                                        angular.element('head').append(link);
                                    }
                                    if (viewConfig.requirejsConfig) {
                                        require.config(viewConfig.requirejsConfig);
                                    }
                                    require([viewConfig.requirejsName], function () {
                                        pluggableViews.registerModule(viewConfig.moduleName);
                                        $timeout(function () {
                                            deferred.resolve();
                                        });
                                    });
                                    return deferred.promise;
                                }]
                            }
                        });
                    };
                    this.$get = function () {
                        return {
                            views: pluggableViews.views,
                            registerModule: pluggableViews.registerModule,
                            registerView: pluggableViews.registerView,
                            toTitleCase: pluggableViews.toTitleCase
                        };
                    }
                }
            ]);
    //}
})();