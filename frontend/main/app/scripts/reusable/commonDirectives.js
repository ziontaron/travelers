'use strict';

/**
 * @ngdoc directive
 * @name iqsApp.directive:hideWhenLoading
 * @description
 * # hideWhenLoading
 */
angular.module('CommonDirectives', []);
angular.module('CommonDirectives').directive('hideWhenLoading', function() {
    return {
        restrict: 'A',
        scope: {
            hideWhenLoading: '='
        },
        link: function postLink(scope, element, attrs) {
            element.css('visibility', 'hidden');
            scope.$watch("hideWhenLoading", function() {
                if (scope.hideWhenLoading) {
                    element.css('visibility', 'hidden');
                } else {
                    element.css('visibility', 'visible');
                }
            });
        }
    };
});
