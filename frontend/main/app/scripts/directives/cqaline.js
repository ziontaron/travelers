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
        controller: function($scope, formController, CQALineService, $activityIndicator) {

            $scope.screenTitle = 'CQA Line';

            var ctrl = new formController({
                scope: $scope,
                entityName: 'CQALine',
                baseService: CQALineService,
                afterCreate: function(oEntity) {},
                afterLoad: function(oEntity) {}
            });


            $scope.$on('modal_ok', function() {
                $scope.baseEntity.CQAHeaderKey = $scope.baseEntity.CQAHeaderKey || _forCQAHeader.id;
                $activityIndicator.startAnimating();
                return $scope.baseEntity.api_attachments.uploadFiles();
            });

            var _forCQAHeader
            $scope.$on('create_cqaLine', function(scope, forCQAHeader) {
                _forCQAHeader = forCQAHeader;
                $scope.create();
            });

            $scope.$on('load_cqaLine_form', function(scope, oEntity) {
                ctrl.load(oEntity);
            });


            $scope.saveAfterUpload = function(oEntity, oDeferred) {
                $scope.save(oEntity).then(function() {
                    $('#modal-CQALine').modal('hide');
                    oDeferred.resolve(oEntity);
                }, function() {
                    oDeferred.reject(oEntity);
                    $activityIndicator.stopAnimating();
                });
            };

            $scope.afterRemoveFile = function(oEntity) {
                var originalEntity = CQALineService.getById(oEntity.id);
                angular.copy(oEntity.api_attachments, originalEntity.api_attachments);
                angular.copy(oEntity.Attachments, originalEntity.Attachments);
            };

        }
    };
});
