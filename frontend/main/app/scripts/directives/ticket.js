'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:ticket
 * @description
 * # ticket
 */
angular.module('appApp').directive('ticket', function() {
    return {
        templateUrl: 'views/ticket.html',
        restrict: 'E'
    };
});
