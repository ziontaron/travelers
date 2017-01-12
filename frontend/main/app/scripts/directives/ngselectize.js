'use strict';

/**
 * @ngdoc directive
 * @name appApp.directive:ngSelectize
 * @description
 * # ngSelectize
 */
angular.module('appApp').directive('ngSelectize', function() {
    return {
        template: '<select></select>',
        restrict: 'E',
        require: 'ngModel',
        scope: {
            placeholder: '@',
            valueField: '@',
            labelField: '@',
            options: '='
        },
        link: function postLink(scope, element, attrs, ngModel) {


            var selectize = element.selectize({
                create: true,
                sortField: scope.labelField,
                placeholder: scope.placeholder,
                valueField: scope.valueField,
                labelField: scope.labelField,
                options: scope.options,
                maxItems: 1,
                persist: false,
                //Callbacks
                onChange: function(value) {
                    if (scope.options) {
                        scope.entity = scope.options.find(function(option) {
                            return option[scope.valueField] == value;
                        });
                        if (!scope.entity) {
                            scope.entity = {};
                            scope.entity[scope.valueField] = null;
                            scope.entity[scope.labelField] = value;
                        }
                        ngModel.$setViewValue(scope.entity);

                    }
                },
                onOptionAdd: function(value, data) {
                    // console.log(value, data);
                }
            })[0].selectize;


            scope.$watch('options', function(newValue) {
                selectize.clearOptions();
                if (newValue) {
                    selectize.addOption(newValue);
                }
                // selectize.refreshOptions();
            });

            ngModel.$render = function() {
                selectize.addItem(ngModel.$modelValue);
                selectize.refreshItems();
            };
            // var newOption = {};
            // newOption[scope.valueField] = 3;
            // newOption[scope.labelField] = 'Agregado Programmatically';
            // selectize.addOption(newOption);


        }
    };
});
