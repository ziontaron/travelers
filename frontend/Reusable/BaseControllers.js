'use strict';

/**
 * @ngdoc service
 * @name iqsApp.baseControllers
 * @description
 * # baseControllers
 * Service in the iqsApp.
 */
angular.module('inspiracode.baseControllers', [])

.factory('listController', function($log, $activityIndicator, $q, localStorageService) {

    var log = $log;

    return function(oMainConfig) {

        //INIT CONFIG/////////////////////////////////////
        var scope = oMainConfig.scope;

        var _baseService = oMainConfig.baseService;
        if (!oMainConfig.entityName || oMainConfig.entityName == '') {
            oMainConfig.entityName = '';
        }

        //After Load callback
        var _afterLoadCallBack = oMainConfig.afterLoad;
        if (!_afterLoadCallBack || typeof _afterLoadCallBack != 'function') {
            _afterLoadCallBack = function() {};
        }

        //After create entity callback
        var _afterCreateCallBack = oMainConfig.afterCreate;
        if (!_afterCreateCallBack || typeof _afterCreateCallBack != 'function') {
            _afterCreateCallBack = function() {};
        }

        var _filterStorageName = oMainConfig.filterStorageName;
        var _filters = oMainConfig.filters || [];
        var _perPage = oMainConfig.perPage || 10;
        //END CONFIG/////////////////////////////////////


        //scope---------------------------------------------
        //let's use normal variables (without underscore) so they can be
        //accessed in view normally
        scope.isLoading = true;
        scope.removeItem = function(oEntity) {
            alertify.confirm(
                'Are you sure you want to delete the ' + oMainConfig.entityName + '?',
                function() {
                    scope.$apply(function() {
                        $activityIndicator.startAnimating();
                        _baseService.remove(oEntity, scope.baseList).then(function(data) {
                            _updateList();
                        }, function() {
                            $activityIndicator.stopAnimating();
                        });
                    });
                });
        };
        scope.take = function(objToTake, toUser) {
            _baseService.take(objToTake, toUser).then(function(data) {
                objToTake.assignedTo = toUser.Value;
                objToTake.AssignationMade = false;
            });
        };
        scope.checkItem = function(bSelected) {
            bSelected ? ++scope.selectedCount : --scope.selectedCount;
        };
        scope.deleteSelected = function() {
            if (scope.selectedCount > 0) {
                alertify.confirm(
                    'Are you sure you want to delete all entities selected?',
                    function() {
                        scope.$apply(function() {
                            _baseService.removeSelected(scope.baseList).then(function(data) {
                                _updateList();
                            });
                        });
                    });
            }
        };
        scope.create = function() {
            alertify.confirm('Create new ' + oMainConfig.entityName + '?', function() {
                scope.$apply(function() {
                    $activityIndicator.startAnimating();
                    _baseService.createEntity().then(function(oNewEntity) {
                        _baseService.save(oNewEntity).then(function(data) {
                            var theCreatedEntity = angular.copy(data.Result);
                            scope.baseList.push(theCreatedEntity);
                            _afterCreateCallBack(theCreatedEntity);
                            $activityIndicator.stopAnimating();
                        });

                    });
                });
            });
        };
        scope.on_filter_changed = function() {
            scope.filterOptions.page = 1;
            _updateList();
        };
        scope.pageChanged = function(newPage) {
            scope.filterOptions.page = newPage;
            _updateList();
        };

        //Updating items:*******************************
        scope.addQty = 1;
        scope.addItems = function(addQty) {
            $activityIndicator.startAnimating();
            _baseService.addBatch(addQty, scope.baseList).then(function(data) {
                scope.selectedCount = _getSelectedCount();
                $activityIndicator.stopAnimating();
            });
        };
        scope.saveItem = function(oItem) {
            $activityIndicator.startAnimating();
            _baseService.save(oItem).then(function(data) {
                scope.selectedCount = _getSelectedCount();
                $activityIndicator.stopAnimating();
            });
        };
        scope.on_input_change = function(oItem) {
            oItem.editMode = true;
        };
        scope.undoItem = function(oItem) {
            var originalItem = _baseService.getById(oItem.id);
            angular.copy(originalItem, oItem);
        };
        //end scope----------------------------------------

        var _getSelectedCount = function() {
            var result = 0;
            var arrItems = scope.baseList || [];
            arrItems.forEach(function(oEntity) {
                if (oEntity.checked) {
                    result++;
                }
            });
            return result;
        };

        //todo unselectAll and filters should be done in listcontroller
        scope.unselectAll = function() {
            var arrItems = scope.baseList || [];
            arrItems.forEach(function(oEntity) {
                oEntity.checked = false;
            });
            scope.selectedCount = 0;
        };

        var _afterLoad = function() {
            _afterLoadCallBack(scope.baseList);
            scope.selectedCount = _getSelectedCount();
            $activityIndicator.stopAnimating();
        };

        var _load = function() {
            $activityIndicator.startAnimating();
            alertify.closeAll();
            return _baseService.loadDependencies().then(function(data) {
                _setFilterOptions();
                _fillCatalogs();
                return _updateList();

                // scope.$evalAsync(function() {
                // });
            });
        };


        var _loadByParentKey = function(parentType, parentKey) {
            $activityIndicator.startAnimating();
            alertify.closeAll();

            return _baseService.loadDependencies().then(function(data) {
                _setFilterOptionsByParent(parentType, parentKey);
                _fillCatalogs();
                return _updateList();

                // scope.$evalAsync(function() {
                // });
            });
        };

        var _fillCatalogs = function() {
            //for filters:
            _filters.forEach(function(filter) {

                var theCatalog = 'the' + capitalizeFirstLetter(filter);
                if (theCatalog.slice(-1) != 's') {
                    theCatalog += 's';
                }
                if (_baseService.catalogs[filter]) {
                    scope[theCatalog] = _baseService.catalogs[filter].getAll();
                    scope[theCatalog].unshift({
                        id: null,
                        Value: 'All'
                    });
                }

            });
        };

        function capitalizeFirstLetter(sWord) {
            var result = sWord.charAt(0).toUpperCase() + sWord.slice(1).toLowerCase();
            return result;
        }

        var _setFilterOptions = function() {

            scope.filterOptions = localStorageService.get(_filterStorageName);

            if (!scope.filterOptions) {
                scope.clearFilters();
            } else {
                scope.filterOptions = JSON.parse(scope.filterOptions);
            }

        };

        var _setFilterOptionsByParent = function(parentType, parentKey) {
            _setFilterOptions();
            scope.filterOptions.parentField = parentType + 'Key';
            scope.filterOptions.parentKey = parentKey;
        };

        var _persistFilter = function() {
            localStorageService.set(_filterStorageName, JSON.stringify(scope.filterOptions));
        };

        var _makeQueryParameters = function() {
            var result = '?';

            for (var prop in scope.filterOptions) {
                if (scope.filterOptions.hasOwnProperty(prop)) {
                    result += prop + '=' + scope.filterOptions[prop] + '&';
                }
            }

            return result;
        };

        var _updateList = function() {
            $activityIndicator.startAnimating();
            scope.isLoading = true;


            var perPage = scope.filterOptions.perPage;
            var page = scope.filterOptions.page;

            var queryParameters = _makeQueryParameters();

            return _baseService.getFilteredPage(perPage, page, queryParameters).then(function(data) {
                scope.baseList = data.Result;

                scope.filterOptions.itemsCount = data.AdditionalData.total_filtered_items;
                scope.filterOptions.totalItems = data.AdditionalData.total_items;
                _persistFilter();

                for (var i = 0; i < scope.baseList.length; i++) {
                    var current = scope.baseList[i];
                    current.itemNumber = (scope.filterOptions.page - 1) * scope.filterOptions.perPage + i + 1;
                }
                _afterLoad();
                scope.isLoading = false;
            });

        };

        scope.clearFilters = function() {
            scope.filterOptions = {
                perPage: _perPage,
                page: 1,
                itemsCount: 0
            };

            _filters.forEach(function(filter) {
                scope.filterOptions['filter' + capitalizeFirstLetter(filter)] = null;
            });

            _persistFilter();
            _updateList();
        };

        // Public baseController API:////////////////////////////////////////////////////////////
        var oAPI = {
            load: _load,
            loadByParentKey: _loadByParentKey,
            // unselectAll: _unselectAll
        };
        return oAPI;
    };
})


.directive('listView', function() {
    return {
        restrict: 'E',
        templateUrl: 'scripts/reusable/listview.html',
        transclude: true,
        link: function postLink(scope, element, attrs) {}
    };
})

.directive('toolbarList', function() {
    return {
        templateUrl: 'scripts/reusable/toolbarlist.html',
        restrict: 'E',
        transclude: {
            'filters': '?filters'
        },
        controller: function($scope) {
            this.getBaseList = function() {
                return $scope.baseList;
            };
            this.setBaseList = function(baseList) {
                $scope.baseList = baseList;
            };
        },
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                pre: function(scope, iElement, iAttrs, controller) {},
                post: function(scope, iElement, iAttrs) {

                }
            }
        }
    };
})

.directive('toolbarListFilter', function($filter) {
    return {
        restrict: 'E',
        transclude: true,
        require: '^toolbarList',
        scope: {
            by: '@'
        },
        template: '<label style="margin: 0;" ng-transclude></label><select style="height:20px;padding:0" class="form-control" ng-model="filterBy.Item" ng-options="item.value for item in theItems" ng-change="setFilter(filterBy.Item);"></select>',
        link: function postLink(scope, element, attrs, toolbarListCtrl) {

            var arrAllRecords;

            function getAvailableValues(by) {
                var result = [];
                var tmp = {};
                if (arrAllRecords === undefined) arrAllRecords = [];
                for (var i = 0; i < arrAllRecords.length; i++) {
                    if (!tmp.hasOwnProperty(arrAllRecords[i][by])) {
                        tmp[arrAllRecords[i][by]] = arrAllRecords[i][by];
                    }
                }
                for (var prop in tmp) {
                    if (tmp.hasOwnProperty(prop)) {
                        result.push({ value: prop });
                    }
                }
                result.push({ value: 'All' });
                return result;
            };


            scope.setFilter = function(item) {
                // var result = [];
                // for (var i = 0; i < arrAllRecords.length; i++) {
                //     if (arrAllRecords[i][scope.by] == value) result.push(arrAllRecords[i]);
                // }
                var expression = {};
                expression[scope.by] = item.value;

                arrAllRecords = $filter('filter')(arrAllRecords, expression);
                toolbarListCtrl.setBaseList(arrAllRecords);
                // return result;
            };


            scope.$watch(
                function() {
                    return toolbarListCtrl.getBaseList();
                },
                function(newValue, oldValue) {
                    arrAllRecords = newValue;
                    scope.theItems = getAvailableValues(scope.by);
                });
        }
    };
})

.directive('toolbarFooter', function() {
    return {
        templateUrl: 'scripts/reusable/toolbarfooter.html',
        restrict: 'E',
        // scope: {
        //     oEntity: '=entity',
        //     oSIF: '=sif',
        //     sSearchPlaceHolder: '@searchPlaceHolder'
        // },
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                pre: function(scope, iElement, iAttrs, controller) {
                    // scope.search = scope.$parent.search;
                    // scope.sif = scope.$parent.sif;
                    // scope.bEditMode = scope.$parent.bEditMode;
                },
                post: function(scope, iElement, iAttrs) {

                }
            }
        }
    };
})


.factory('formController', function($log, $activityIndicator, $routeParams, validatorService, appConfig, $timeout, $rootScope) {

    var log = $log;

    return function(oMainConfig) {

        //INIT CONFIG/////////////////////////////////////

        var scope = oMainConfig.scope;

        var _baseService = oMainConfig.baseService;
        if (!oMainConfig.entityName || oMainConfig.entityName == '') {
            oMainConfig.entityName = '';
        }

        //After Load callback
        var _afterLoadCallBack = oMainConfig.afterLoad;
        if (!_afterLoadCallBack || typeof _afterLoadCallBack != "function") {
            _afterLoadCallBack = function() {};
        }

        //After create entity callback
        var _afterCreateCallBack = oMainConfig.afterCreate;
        if (!_afterCreateCallBack || typeof _afterCreateCallBack != "function") {
            _afterCreateCallBack = function() {};
        }

        //END CONFIG/////////////////////////////////////


        //scope---------------------------------------------
        //let's use normal variables (without underscore) so they can be
        //accessed in view normally
        scope.isLoading = true;
        scope.remove = function(oEntity) {
            alertify.confirm(
                'Are you sure you want to delete this ' + oMainConfig.entityName + '?',
                function() {
                    scope.$apply(function() {
                        $activityIndicator.startAnimating();
                        _baseService.remove(oEntity, scope.baseList).then(function(data) {
                            _updateList();
                        }, function() {
                            $activityIndicator.stopAnimating();
                        });
                    });
                });
        };
        scope.take = function(objToTake, toUser) {
            _baseService.take(objToTake, toUser).then(function(data) {
                objToTake.assignedTo = toUser.Value;
                objToTake.AssignationMade = false;
            });
        };

        scope.create = function() {
            alertify.confirm('Create new ' + oMainConfig.entityName + '?', function() {
                scope.$apply(function() {
                    $activityIndicator.startAnimating();
                    _baseService.createEntity().then(function(oNewEntity) {
                        _baseService.save(oNewEntity).then(function(data) {
                            var theCreatedEntity = angular.copy(data.Result);
                            scope.baseList.push(theCreatedEntity);
                            _afterCreateCallBack(theCreatedEntity);
                            $activityIndicator.stopAnimating();
                        });

                    });
                });
            });
        };

        //Updating items:*******************************
        scope.save = function(oItem) {
            $activityIndicator.startAnimating();
            _baseService.save(oItem).then(function(data) {
                $activityIndicator.stopAnimating();
            });
        };
        scope.on_input_change = function(oItem) {
            oItem.editMode = true;
        };
        scope.undo = function(oItem) {
            var originalItem = _baseService.getById(oItem.id);
            angular.copy(originalItem, oItem);
        };
        //end scope----------------------------------------

        var _afterLoad = function() {
            _afterLoadCallBack(scope.baseEntity);
            $activityIndicator.stopAnimating();
        };

        var _load = function() {
            $activityIndicator.startAnimating();
            alertify.closeAll();



            _baseService.loadDependencies().then(function(data) {
                _fillCatalogs();
                _updateForm();
            });
        };

        var _fillCatalogs = function() {
            //for filters:

            for (var catalog in _baseService.catalogs) {
                if (_baseService.catalogs.hasOwnProperty(catalog)) {

                    var theCatalog = 'the' + capitalizeFirstLetter(catalog);
                    if (theCatalog.slice(-1) != 's') {
                        theCatalog += 's';
                    }
                    scope[theCatalog] = _baseService.catalogs[catalog].getAll();
                }
            }

        };

        function capitalizeFirstLetter(sWord) {
            var result = sWord.charAt(0).toUpperCase() + sWord.slice(1).toLowerCase();
            return result;
        }

        var _makeQueryParameters = function() {
            var result = '?';

            for (var prop in scope.filterOptions) {
                if (scope.filterOptions.hasOwnProperty(prop)) {
                    result += prop + '=' + scope.filterOptions[prop] + '&';
                }
            }

            return result;
        };

        var _updateForm = function() {
            $activityIndicator.startAnimating();
            scope.isLoading = true;

            switch (true) {
                case $routeParams.id !== true && $routeParams.id > 0: //Get By id
                    scope.openingMode = 'id';
                    _baseService.loadEntity($routeParams.id).then(function(data) {
                        $activityIndicator.stopAnimating();
                        var theSelectedEntity = data;
                        if (!theSelectedEntity) {
                            alertify.alert('Nonexistent record.').set('modal', true).set('closable', false);
                            scope.openingMode = 'error';
                            return;
                        }
                        scope.baseEntity = angular.copy(theSelectedEntity);
                        _afterLoad();
                        scope.isLoading = false;
                    });
                    break;

                default:
                    $activityIndicator.stopAnimating();
                    scope.openingMode = 'error';
                    alertify.alert('Verify URL parameters.').set('modal', true).set('closable', false);
                    return;
            }

        };

        // Public baseController API:////////////////////////////////////////////////////////////
        var oAPI = {
            load: _load,
            // unselectAll: _unselectAll
        };
        return oAPI;
    };

});
