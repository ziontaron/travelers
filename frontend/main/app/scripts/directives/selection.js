'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:selection
 * @description
 * # selection
 */
angular.module('appApp').directive('selection', function() {
    return {
        templateUrl: 'views/selection.html',
        restrict: 'E',
        link: function postLink(scope, element, attrs) {

            scope.$watch(function() {
                return element.find('.tags').outerHeight();
            }, function(newValue) {
                adjustSameTagsHeight(newValue);
            });

            function adjustSameTagsHeight(newValue) {
                element.find('.btn').outerHeight(newValue);
            }

        }
    };
});
