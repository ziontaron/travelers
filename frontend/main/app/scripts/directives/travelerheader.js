'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:TravelerHeader
 * @description
 * # TravelerHeader
 */
angular.module('appApp')
  .directive('TravelerHeader', function () {
    return {
      template: '<div></div>',
      restrict: 'E',
      link: function postLink(scope, element, attrs) {
        element.text('this is the TravelerHeader directive');
      }
    };
  });
