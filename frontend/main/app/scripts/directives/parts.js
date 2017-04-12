'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:parts
 * @description
 * # parts
 */
angular.module('appApp').directive('parts', function() {
    return {
        templateUrl: 'views/parts.html',
        restrict: 'E',
        controller: function($scope, listController, FSItemService) {

            $scope.screenTitle = 'FS Parts';

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'FSItem',
                baseService: FSItemService,
                afterCreate: function(oEntity) {},
                afterLoad: function() {
                    $scope.filterLabel = "Total Parts Current View: " + $scope.filterOptions.itemsCount;
                },
                onOpenItem: function(oEntity) {},
                filters: []
            });

            var forCQAHeader = null;
            $scope.$on('load_parts', function(scope, oCQA) {
                forCQAHeader = oCQA;
                listCtrl.load();
            });

            $scope.selectItem = function(item) {
                alert(JSON.stringify(item));
                // FSItemService.addToParent('CQAHeader', forCQAHeader.id, item).then(function(response) {
                //     $('#modal-areas').modal('hide');
                // });
            };


            $scope.$on('unload_parts', function() {
                $scope.baseList = [];
                $scope.isLoading = true; //To hide elements.
            });

        }
    };
});
