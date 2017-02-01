'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:cqaDetail
 * @description
 * # cqaDetail
 */
angular.module('appApp').directive('cqaDetail', function() {
    return {
        templateUrl: 'views/cqaDetail.html',
        restrict: 'E',
        scope: {
            parentEntity: '=',
            parentType: '@'
        },
        link: function postLink(scope, element, attrs) {},
        controller: function($scope, listController, CQALineService, $q, $activityIndicator) {

            var list = new listController({
                entityName: 'CQA Line',
                baseService: CQALineService,
                scope: $scope,
                afterLoad: function(data) {
                    _adapt($scope.baseList);
                    // table.loadData($scope.baseList);
                }
            });

            var _adapt = function(items) {
                if (!items) {
                    items = [];
                }

                if ($scope.parentEntity && $scope.parentEntity.id > -1) {
                    insertItemForAdd(items);

                    // items.sort(function(a, b) { TODO
                    //     return b.ConvertedMetricDate - a.ConvertedMetricDate;
                    // });
                }

                return items;
            };

            var insertItemForAdd = function(items, change) {
                if (!items.length || (items[0] && items[0].id > -1)) {
                    items.unshift({});
                    if (change) {
                        // table.selectCellByProp(1, change[1]); TODO
                    }
                }
            }

            $scope.$watch(function() {
                return $scope.parentEntity;
            }, function() {
                if ($scope.parentEntity) {
                    list.loadByParentKey($scope.parentType, $scope.parentEntity.id);
                } else {
                    list.loadByParentKey($scope.parentType, 0); //Clear list
                }
            });

            var savePending = function() {

                // $activityIndicator.startAnimating();
                // var arrPromiseConstructors = [];


                // //Items to be updated or inserted:
                // var arrItemsToBeSaved = $scope.baseList.filter(function(item) {
                //     if (item.edited) {
                //         item.ConvertedMetricDate = HTDate_To_JSDate(item);
                //     }
                //     return item.edited;
                // });
                // arrItemsToBeSaved.forEach(function(item) {
                //     item.FormatKey = $scope.parentEntity.FormatKey;
                //     var promiseConstructor = function() {
                //         return CQALineService.save(item);
                //     }
                //     arrPromiseConstructors.push(promiseConstructor);
                // });

                // //Items to be deleted:
                // var arrItemsToBeDeleted = $scope.baseList.filter(function(item) {
                //     return item.removed;
                // });
                // arrItemsToBeDeleted.forEach(function(item) {
                //     var promiseConstructor = function() {
                //         return CQALineService.remove(item);
                //     }
                //     arrPromiseConstructors.push(promiseConstructor);
                // });

                // //Metric Year update
                // if ($scope.parentEntity && $scope.parentEntity.editMode) {
                //     var promiseConstructor = function() {
                //         return metricYearService.save($scope.parentEntity, $scope.metric[$scope.parentEntity] s);
                //     }
                //     arrPromiseConstructors.push(promiseConstructor);
                // }


                // //Reloading all
                // var promiseConstructor = function() {
                //     return list.loadByParentKey($scope.parentType, $scope.parentEntity.id);
                // }

                // arrPromiseConstructors.push(promiseConstructor);

                // //Sending transactions in serial way:
                // $q.serial(arrPromiseConstructors).finally(function(data) {
                //     $scope.$emit('RefreshMetric', $scope.metric);
                //     $timeout(function() {
                //         alertify.message('Process completed.');
                //     }, 100);
                // });

            };

            $scope.$on('SaveCQALines', function() {
                var MetricYearKey = $scope.parentEntity.id;
                if (!$scope.parentEntity || !$scope.parentEntity.id) {
                    alertify.message('Nothing to save.');
                } else {
                    savePending();
                }
            });
        }
    };
});
