'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:ngSelectize
 * @description
 * # ngSelectize
 */
angular.module('appApp').directive('ngSelectize', function($timeout) {
    return {
        template: '<select></select>',
        restrict: 'E',
        scope: {
            theModel: '=',
            service: '=',
            // options: '=',
            placeholder: '@',
            labelField: '@',
            valueField: '@'
        },
        link: function postLink(scope, element, attrs, ngModel) {

            var loading = true;
            scope.options = [];

            var selectize = element.selectize({
                create: true,
                sortField: scope.labelField,
                searchField: scope.labelField,
                placeholder: scope.placeholder,
                valueField: scope.valueField,
                labelField: scope.labelField,
                maxItems: 1,
                persist: true,
                //Callbacks
                onChange: function(value) {
                    $timeout(function() {
                        // if (value == '') {
                        //     if (scope.theModel) {
                        //         scope.theModel.Value = '';
                        //     }
                        // } else 
                        if (value != '' && scope.options) {
                            var selectedOption = scope.options.find(function(option) {
                                return option[scope.valueField] == value;
                            });
                            if (selectedOption) {
                                scope.theModel = selectedOption[scope.valueField];
                            }
                        }
                    });
                },
                onOptionAdd: function(value, data) {
                    if (!loading) {
                        var oNew = {
                            id: 0
                        };
                        oNew[scope.labelField] = value;
                        scope.service.save(oNew, scope.options).then(function(data) {
                            scope.theModel = data[scope.valueField];
                            refresh();
                        });
                    }
                },
                onItemAdd: function(value, $item) {
                    // if (!loading) {
                    //     scope.service.save({
                    //         id: 0,
                    //         Value: value,
                    //         MetricKey: scope.metric.id
                    //     }, scope.scope.options).then(function(data) {
                    //         console.log(data, scope.metric);
                    //         scope.theModel = data.Result;
                    //     });
                    // }
                }
            })[0].selectize;

            scope.$watch('theModel', function(newValue) {
                if (scope.theModel !== undefined) {
                    loadSelected(scope.theModel);
                }
            });

            function load() {
                loading = true;
                scope.service.loadEntities().then(function(data) {
                    scope.options = data.Result;
                    refresh();
                });
            }

            function refresh() {
                loading = true;
                loadOptions(scope.options ? scope.options : []);
                loadSelected(scope.theModel);
                loading = false;
            }

            function loadSelected(idSelected) {
                selectize.clear();
                if (idSelected != undefined) {
                    selectize.addItem(idSelected, true);
                }
                selectize.refreshItems();
                $timeout(function() {
                    selectize.close();
                }, 100);
            }

            function loadOptions(newOptions) {
                scope.options = newOptions;
                selectize.clearOptions();
                if (newOptions) {
                    selectize.addOption(newOptions);
                }
                // selectize.refreshOptions();
            }

            load();

            scope.$on('DeleteMetricYear', function(s, metricYear) {
                if (metricYear && metricYear.id) {
                    alertify.confirm(
                        'Are you sure you want to delete this Year: ' + metricYear.Value + '?',
                        function() {
                            scope.$apply(function() {
                                scope.service.remove(metricYear, scope.options).then(function(data) {
                                    load(scope.metric);
                                });

                            });
                        });
                } else {
                    alertify.message('Nothing selected');
                }
            });

        }
    };
});
