'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:cqaLine
 * @description
 * # cqaLine
 */
angular.module('appApp').directive('cqaLine', function() {
    return {
        templateUrl: 'views/cqaLine.html',
        restrict: 'E',
        scope: {
            entity: '='
        }
    };
});
