'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:CqalistCtrl
 * @description
 * # CqalistCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('CqalistCtrl', function($scope, listController, CQAHeaderService, $location) {

    $scope.screenTitle = 'CQA List';

    var ctrl = new listController({
        scope: $scope,
        entityName: 'CQAHeader',
        baseService: CQAHeaderService,
        afterCreate: function(oInstance) {
            $scope.saveItem(oInstance).then(function(oEntity) {
                go('/cqa?id=' + oEntity.id);
            });
        },
        afterLoad: function() {
            $scope.filterLabel = "Total CQAs Current View: " + $scope.filterOptions.itemsCount;
        },
        onOpenItem: function(oItem) {
            go('/cqa?id=' + oItem.id);
        },
        filters: {
            'CustomerKey': 'Customer',
            'StatusKey': 'Status',
            'filterUser': 'User'
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

var list;
