(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .factory('sharedProperties', function () {
            var properties = {};
            return {
                getProperty: function (name) {
                    return properties[name];
                },
                setProperty: function (name, value) {
                    properties[name] = value;
                }
            };
        });
})();