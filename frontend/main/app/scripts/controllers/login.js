'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:LoginCtrl
 * @description
 * # LoginCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('LoginCtrl', function($scope, $location, authService, $activityIndicator, appConfig) {

    $scope.appName = appConfig.APP_NAME;

    $activityIndicator.stopAnimating();
    alertify.closeAll();

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.ErrorMessage = null;

    $scope.login = function() {
        $scope.ErrorMessage = null;
        $activityIndicator.startAnimating();
        authService.login($scope.loginData).then(function(response) {
                $location.path('/');
                $activityIndicator.stopAnimating();
            },
            function(err) {
                $activityIndicator.stopAnimating();
                if (err == 'invalid_grant') {
                    err = 'Usuario o contraseña incorrecta.';
                }
                $scope.ErrorMessage = err;
            });
    };
});
