'use strict';

/**
 * @ngdoc function
 * @name appApp.controller:CqalistCtrl
 * @description
 * # CqalistCtrl
 * Controller of the appApp
 */
angular.module('appApp').controller('CqalistCtrl', function($scope, listController, CQAHeaderService, $activityIndicator, $window, $location) {

    $scope.screenTitle = 'CQA List';

    var ctrl = new listController({
        scope: $scope,
        entityName: 'CQAHeader',
        baseService: CQAHeaderService,
        afterCreate: function(oResult) {
            go('/cqa?id=' + oResult.id);
        },
        afterLoad: function() {
            $scope.filterLabel = "Total CQAs Current View: " + $scope.filterOptions.itemsCount;
        },
        filters: ['User'],
        filterStorageName: 'CQAList_filter'
    });

    $scope.openCQA = function(oEntity) {
        go('/cqa?id=' + oEntity.id);
    };

    var go = function(path) {
        if (path != $location.url()) {
            $location.url(path);
            // $window.open('#!' + path, '_blank');
        }
    };

    ctrl.load();
});

var list;
