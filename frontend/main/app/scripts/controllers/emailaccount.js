'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:EmailaccountCtrl
 * @description
 * # EmailaccountCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('EmailaccountCtrl', function($scope, formController, userService, $rootScope, $timeout) {

    var ctrl = new formController({
        scope: $scope,
        entityName: 'User',
        baseService: userService
    });

    $timeout(function() {
        ctrl.load($rootScope.currentUser);
    }, 500);

    $scope.sendTestEmail = function(oUser) {
        userService.sendTestEmail(oUser).then(function() {
            alertify.success('Email sent successfully.');
        });
    };
});
