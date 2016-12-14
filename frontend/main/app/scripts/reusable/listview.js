'use strict';

/**
 * @ngdoc directive
 * @name iqsApp.directive:listView
 * @description
 * # listView
 */
angular.module('iqsApp').directive('listView', function() {
    return {
        restrict: 'E',
        templateUrl: 'views/listview.html',
        transclude: true,
        link: function postLink(scope, element, attrs) {}
    };
});
