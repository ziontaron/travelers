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

    $scope.CQALineService = CQALineService;

    $scope.addCQALine = function() {
        CQALineService.createEntity().then(function(oNewEntity) {
            $scope.CQALineToBeSaved = oNewEntity;
            $scope.CQALineToBeSaved.CQAHeaderKey = $scope.baseEntity.id;
            angular.element('#modal-CQALine').modal('show');
        });
    };

    $scope.afterSaveCQALine = function(entitySaved) {
        angular.element('#modal-CQALine').modal('hide');
        $scope.$broadcast('RefreshCQADetail');
    };

});
