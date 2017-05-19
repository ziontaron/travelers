'use strict';

/**
 * @ngdoc overview
 * @name appApp
 * @description
 * # appApp
 *
 * Main module of the application.
 */
angular.module('appApp', [
    'ngAnimate',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'ngActivityIndicator',
    'LocalStorageModule',
    'inspiracode.crudFactory',
    'CRUDServices',
    'inspiracode.baseControllers',
    'angularUtils.directives.dirPagination',
    'ngTagsInput',
    'angularFileUpload',
    'CommonDirectives'
], function($httpProvider) {
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
    $httpProvider.defaults.headers.put['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
}).config(function($routeProvider, appConfig, $activityIndicatorProvider, $httpProvider, localStorageServiceProvider) {


    localStorageServiceProvider.setPrefix(appConfig.APP_NAME);

    $routeProvider
        .when('/', {
            templateUrl: 'views/main.html',
            controller: 'MainCtrl',
            controllerAs: 'main'
        })
        .when('/about', {
            templateUrl: 'views/about.html',
            controller: 'AboutCtrl',
            controllerAs: 'about'
        })
        .when('/login', {
            templateUrl: 'views/login.html',
            controller: 'LoginCtrl',
            controllerAs: 'loginn' //cannot have same name as view            
        })
        .when('/emailAccount', {
            templateUrl: 'views/emailaccount.html',
            controller: 'EmailaccountCtrl',
            controllerAs: 'emailAccount'
        })
        .when('/enterCount', {
          templateUrl: 'views/entercount.html',
          controller: 'EntercountCtrl',
          controllerAs: 'enterCount'
        })
        .otherwise({
            redirectTo: '/'
        });

    $activityIndicatorProvider.setActivityIndicatorStyle('CircledWhite');
    alertify.set('notifier', 'position', 'top-left');
    alertify.set('notifier', 'delay', 2);

    $httpProvider.interceptors.push('authInterceptorService');

}).run(function(authService, $rootScope, $location) {

    authService.fillAuthData();

    // register listener to watch route changes
    $rootScope.$on('$routeChangeSuccess', function(event, next, current) {
        alertify.closeAll();
        $('.modal').modal('hide');
        $('.modal-backdrop.fade.in').remove();


        var authentication = authService.authentication;
        if (!authentication || !authentication.isAuth) {
            if (next.templateUrl != "views/login.html") {
                $location.path('/login');
            }
        } else {
            //Role Validations
            // if (authentication.role == 'Usuario' || authentication.role == '') {
            //     authService.logOut();
            //     setTimeout(function() {
            //         alertify.alert('Solo Supervisores y Administradores tienen acceso a esta apliacación.').set('modal', true);
            //     }, 300);
            // }
        }

    });

    $rootScope.$on('$routeChangeSuccess', function() {
        $rootScope.activePath = $location.path();
    });


    $rootScope.logOut = function() {
        authService.logOut();
    };

});
