'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:CqaCtrl
 * @description
 * # CqaCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('CqaCtrl', function($scope, formController, CQAHeaderService, CustomerService, catResultService, catStatusService, catConcernTypeService, CQALineService, $routeParams) {

    $scope.screenTitle = 'CQA Form';

    var ctrl = new formController({
        scope: $scope,
        entityName: 'CQAHeader',
        baseService: CQAHeaderService,
        afterCreate: function(oResult) {
            // go('/cqa?id=' + oResult.id);
        },
        afterLoad: function() {}
    });

    ctrl.load($routeParams.id);

    $scope.CustomerService = CustomerService;
    $scope.catResultService = catResultService;
    $scope.catConcernTypeService = catConcernTypeService;
    $scope.catStatusService = catStatusService;


    $scope.addCQALine = function() {
        $('#modal-CQALine').off('shown.bs.modal').on('shown.bs.modal', function(e) {
            $scope.$apply(function() {
                //on show modal
                $scope.$broadcast('create_cqaLine', $scope.baseEntity);
                $('#modal-CQALine').find('input').filter(':input:visible:first').focus();
            });
        }).off('hidden.bs.modal').on('hidden.bs.modal', function(e) {
            $scope.$apply(function() {
                //on hide modal'
                $scope.$broadcast('RefreshCQADetail');
            });
        }).modal('show');
    };

    $scope.selectPart = function() {
        $('#modal-parts').off('shown.bs.modal').on('shown.bs.modal', function(e) {
            $scope.$apply(function() {
                //on show modal
                $scope.$broadcast('load_parts', $scope.baseEntity);
                $('#modal-parts').find('input').filter(':input:visible:first').focus();
            });
        }).off('hidden.bs.modal').on('hidden.bs.modal', function(e) {
            $scope.$apply(function() {
                //on hide modal'
                $scope.$broadcast('unload_parts');
            });
        }).modal('show');
    };

    $scope.openSendEmail = function() {
        $('#modal-email').off('shown.bs.modal').on('shown.bs.modal', function(e) {
            $scope.$apply(function() {
                //on show modal
                // $scope.$broadcast('load_parts', $scope.baseEntity);
                $('#modal-email').find('input').filter(':input:visible:first').focus();
            });
        }).off('hidden.bs.modal').on('hidden.bs.modal', function(e) {
            $scope.$apply(function() {
                //on hide modal'
                // $scope.$broadcast('unload_parts');
            });
        }).modal('show');
    };

    $scope.emailTitle = "Send Email";

});
