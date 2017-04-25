'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:sendEmail
 * @description
 * # sendEmail
 */
angular.module('appApp').directive('sendEmail', function() {
    return {
        templateUrl: 'views/sendEmail.html',
        restrict: 'E',
        controller: function($scope, formController) {
        	
        }
    };
});
