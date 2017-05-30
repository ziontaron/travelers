'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:EntertravelerCtrl
 * @description
 * # EntertravelerCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('EntertravelerCtrl', function($scope, listController, TravelerHeaderService, $location) {

    $scope.screenTitle = 'Traveler List';

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Traveler',
        baseService: TravelerHeaderService,
        afterCreate: function(oInstance) {
            // $scope.saveItem(oInstance).then(function(oEntity) {
            //     go('/cqa?id=' + oEntity.id);
            // });
            $('#modal-traveler').off('shown.bs.modal').on('shown.bs.modal', function(e) {
                $scope.$apply(function() {
                    //on show modal
                    $scope.$broadcast('load_Traveler', oInstance);
                    $('#modal-traveler').find('input').filter(':input:visible:first').focus();
                });
            }).off('hidden.bs.modal').on('hidden.bs.modal', function(e) {
                $scope.$apply(function() {
                    //on hide modal'
                    // $scope.$broadcast('unload_user_form');
                    ctrl.load();
                });
            }).modal('show');

            // console.log(oInstance);
        },
        afterLoad: function() {
            $scope.filterLabel = "Total CQAs Current View: " + $scope.filterOptions.itemsCount;
        },
        onOpenItem: function(oItem) {
            // go('/cqa?id=' + oItem.id);
        },
        filters: {
            // 'CustomerKey': 'Customer',
            // 'StatusKey': 'Status',
            // 'filterUser': 'User'
        }
    });

    var go = function(path) {
        if (path != $location.url()) {
            $location.url(path);
            // $window.open('#!' + path, '_blank');
        }
    };

    ctrl.load();

});
