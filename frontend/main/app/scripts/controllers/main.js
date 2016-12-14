'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('MainCtrl', function($scope, appConfig, CQAHeaderService) {
    $scope.appName = appConfig.APP_NAME;



    $scope.test = function() {
        CQAHeaderService.createEntity().then(function(data) {
            $scope.data = data;
        });
    };

    $scope.fillFromServer = function() {

        CQAHeaderService.loadEntities().then(function(data) {
            $scope.dataInServer = data;
        });
    };

    $scope.save = function() {
        CQAHeaderService.save($scope.data);
    };
});
