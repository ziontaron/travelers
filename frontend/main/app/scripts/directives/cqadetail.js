'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:cqaDetail
 * @description
 * # cqaDetail
 */
angular.module('appApp').directive('cqaDetail', function() {
    return {
        templateUrl: 'views/cqaDetail.html',
        restrict: 'E',
        link: function postLink(scope, element, attrs) {}
    };
});
