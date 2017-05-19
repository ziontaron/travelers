'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:EntercountCtrl
 * @description
 * # EntercountCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('EntercountCtrl', function($scope, listController, TicketService) {
    var listCtrl = new listController({
        scope: $scope,
        entityName: 'Ticket',
        baseService: TicketService,
        onOpenItem: function(oEntity) {
            $('#modal-ticket').off('shown.bs.modal').on('shown.bs.modal', function(e) {
                $scope.$apply(function() {
                    //on show modal
                    // $scope.$broadcast('load_user_form', oEntity);
                    $('#modal-ticket').find('input').filter(':input:visible:first').focus();
                });
            }).off('hidden.bs.modal').on('hidden.bs.modal', function(e) {
                $scope.$apply(function() {
                    //on hide modal'
                    // $scope.$broadcast('unload_user_form');
                    // listCtrl.load();
                });
            }).modal('show');
        }
    });

    // listCtrl.load();

});
