'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:travelerHeaderRO
 * @description
 * # travelerHeaderRO
 */
angular.module('appApp')
    .directive('travelerHeaderRo', function() {
        return {
            templateUrl: 'views/travelerheaderro.html',
            restrict: 'E',
            scope: { baseEntity: "=" },
            controller: function($scope, formController, TravelerHeaderService, $activityIndicator) {

                $scope.screenTitle = 'Traveler';

                var ctrl = new formController({
                    scope: $scope,
                    entityName: 'Traveler',
                    baseService: TravelerHeaderService,
                    afterCreate: function(oEntity) {},
                    afterLoad: function(oEntity) {}
                });


                // $scope.$on('modal_ok', function() {
                //     // $scope.baseEntity.CQAHeaderKey = $scope.baseEntity.CQAHeaderKey || _forCQAHeader.id;
                //     // $activityIndicator.startAnimating();
                //     // return $scope.baseEntity.api_attachments.uploadFiles();
                //     // alert("hola")
                //     $scope.save($scope.baseEntity).then(function() {
                //             $('#modal-traveler').modal('hide');
                //             // oDeferred.resolve(oEntity);
                //         }
                //         // , function() {
                //         //     oDeferred.reject(oEntity);
                //         //     $activityIndicator.stopAnimating();
                //         // }
                //     );
                // });


                $scope.$on('load_Traveler', function(scope, oEntity) {
                    ctrl.load(oEntity);

                });


                $scope.saveAfterUpload = function(oEntity, oDeferred) {
                    // $scope.save(oEntity).then(function() {
                    //     $('#modal-CQALine').modal('hide');
                    //     oDeferred.resolve(oEntity);
                    // }, function() {
                    //     oDeferred.reject(oEntity);
                    //     $activityIndicator.stopAnimating();
                    // });
                };

                $scope.afterRemoveFile = function(oEntity) {
                    // var originalEntity = TravelerHeaderService.getById(oEntity.id);
                    // angular.copy(oEntity.api_attachments, originalEntity.api_attachments);
                    // angular.copy(oEntity.Attachments, originalEntity.Attachments);
                };

            }

        };
    });
