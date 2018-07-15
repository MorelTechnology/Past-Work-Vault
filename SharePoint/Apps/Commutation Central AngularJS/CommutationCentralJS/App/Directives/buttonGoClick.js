(function() {
    'use strict';

    angular
        .module('commutationCentral')
        .directive('trgClick', function ($location) {
            return function (scope, element, attrs) {
                var path;
                attrs.$observe('trgClick', function (val) {
                    path = val;
                });
                element.bind('click', function () {
                    scope.$apply(function () {
                        $location.path(path);
                    });
                });
            };
        });
})();