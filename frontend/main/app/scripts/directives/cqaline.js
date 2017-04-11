'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:cqaLine
 * @description
 * # cqaLine
 */
angular.module('appApp').directive('cqaLine', function() {
    return {
        templateUrl: 'views/cqaLine.html',
        restrict: 'E',
        controller: function($scope, formController, CQALineService) {

            $scope.screenTitle = 'CQA Line';

            var ctrl = new formController({
                scope: $scope,
                entityName: 'CQALine',
                baseService: CQALineService,
                afterCreate: function(oEntity) {},
                afterLoad: function() {}
            });


            $scope.$on('modal_ok', function() {
                $scope.baseEntity.CQAHeaderKey = _forCQAHeader.id;
                $scope.save($scope.baseEntity).then(function() {
                    $('#modal-CQALine').modal('hide');
                });
            });

            var _forCQAHeader
            $scope.$on('create_cqaLine', function(scope, forCQAHeader) {
                _forCQAHeader = forCQAHeader;
                $scope.create();
            });

            $scope.$on('load_cqaLine_form', function(scope, oEntity) {
                ctrl.load(oEntity);
            });

        }
    };
});
