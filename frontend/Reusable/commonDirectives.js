'use strict';

/**
 * @ngdoc directive
 * @name iqsApp.directive:hideWhenLoading
 * @description
 * # hideWhenLoading
 */
angular.module('CommonDirectives', []);
angular.module('CommonDirectives').directive('hideWhenLoading', function() {
    return {
        restrict: 'A',
        scope: {
            hideWhenLoading: '='
        },
        link: function postLink(scope, element, attrs) {
            element.css('visibility', 'hidden');
            scope.$watch("hideWhenLoading", function() {
                if (scope.hideWhenLoading) {
                    element.css('visibility', 'hidden');
                } else {
                    element.css('visibility', 'visible');
                }
            });
        }
    };
}).directive('attachmentsBox', function(FileUploader, appConfig, $http, $timeout, $activityIndicator, $q) {
    var _uploadDeferred;
    return {
        restrict: 'E',
        scope: {
            ownerEntity: '=',
            printmode: '=',
            kind: '@',
            afterUpload: '&',
            readOnly: '=',
            whenChange: '&',
            afterDelete: '&'
        },
        template: '<div nv-file-drop nv-file-over uploader="uploader" class="AttachmentsBox" over-class="UploaderOverClass" style="background-color: rgb(221, 221, 221);padding: 4px;border: solid 2px gray;border-style: dotted;padding-bottom: 34px;" ng-click="$event.stopPropagation();" ng-dblclick="addAttachment();$event.stopPropagation();"><input type="file" nv-file-select uploader="uploader" multiple style="display:none;" /><div class="noselect" ng-show="ownerEntity.Attachments.length==0 || ownerEntity.Attachments == null">No files.</div><p class="input-group noselect" style="border-bottom: solid 1px #E5E5E5;width: 100%;" ng-repeat="attachment in ownerEntity.Attachments"><a ng-hide="attachment.isForUpload" ng-href="{{baseURL}}attachment_download?directory={{attachment.Directory}}&fileName={{attachment.FileName}}&attachmentKind={{kind}}" href="" class="pull-left" style="display: block; top: 4px; position: relative;">{{attachment.FileName}}</a><span ng-show="attachment.isForUpload" class="glyphicon glyphicon-upload pull-left" style="padding: 3px 0;font-size: 14px;"></span><button ng-show="attachment.isForUpload" class="btn-link pull-left" style="display: block; top: 4px; position: relative;padding:0;">{{attachment.FileName}}</button><span class="btn glyphicon glyphicon-remove pull-right btn-sm" style="padding:0;"  ng-hide="printmode || readOnly" ng-click="removeAttachment(attachment, $index);$event.stopPropagation();"></span></p><input type="button" ng-hide="readOnly" class="btn btn-success btn-xs" value="Add File" ng-click="addAttachment();$event.stopPropagation();" style="margin-top: 4px;margin-bottom: 0;position: absolute;bottom: 5px;" /></div>',
        compile: function compile(tElement, tAttrs, transclude) {
            return {
                pre: function preLink(scope, iElement, iAttrs, controller) {
                    scope.uploader = new FileUploader();
                    scope.uploader.onAfterAddingFile = function(fileItem) {
                        if (!scope.ownerEntity.Attachments) {
                            scope.ownerEntity.Attachments = [];
                        }
                        scope.ownerEntity.Attachments.push({
                            FileName: fileItem.file.name,
                            Directory: (scope.ownerEntity.AttachmentsFolder || ''),
                            isForUpload: true
                        });
                        scope.whenChange({
                            oEntity: scope.ownerEntity
                        });
                        // scope.ownerEntity.api_attachments.uploadFiles();
                    };
                    scope.uploader.onWhenAddingFileFailed = function(item, filter, options) {
                        console.debug(item);
                        console.debug(filter);
                        console.debug(options);
                    };
                    scope.uploader.onSuccessItem = function(item, response, status, headers) {
                        var backendResponse = response;
                        if (!backendResponse.ErrorThrown) {
                            scope.ownerEntity.AttachmentsFolder = backendResponse.ResponseDescription;
                            var theAttachment = scope.getAttachment(item.file.name);
                            delete theAttachment.isForUpload;
                        } else {
                            alertify.alert(backendResponse.ResponseDescription).set('modal', true);
                            console.debug(response);
                        }
                    };
                    scope.uploader.onErrorItem = function(item, response, status, headers) {
                        console.debug(item);
                        console.debug(response);
                        console.debug(status);
                        _uploadDeferred.reject(scope.ownerEntity);
                    };
                    scope.uploader.onCompleteAll = function() {
                        scope.afterUpload({
                            oEntity: scope.ownerEntity,
                            oDeferred: _uploadDeferred
                        });
                    };
                    scope.uploader.onBeforeUploadItem = function(item) {
                        item.url = appConfig.API_URL + 'attachment?attachmentKind=' + (scope.kind || '') + '&targetFolder=' + (scope.ownerEntity.AttachmentsFolder || '')
                    };
                },
                post: function(scope, iElement, iAttrs) {
                    scope.baseURL = appConfig.API_URL;
                    scope.$watch(function() {
                        return scope.ownerEntity;
                    }, function() {
                        if (scope.ownerEntity && !scope.ownerEntity.api_attachments) {
                            scope.ownerEntity.api_attachments = {};
                            scope.ownerEntity.api_attachments.uploadFiles = function() {
                                _uploadDeferred = $q.defer();
                                if (scope.uploader.getNotUploadedItems().length > 0) {
                                    scope.uploader.uploadAll();
                                } else {
                                    scope.afterUpload({
                                        oEntity: scope.ownerEntity,
                                        oDeferred: _uploadDeferred
                                    });
                                }
                                return _uploadDeferred.promise;
                            };
                            scope.ownerEntity.api_attachments.clearQueue = function() {
                                scope.uploader.clearQueue();
                            };
                        }
                    }, false);
                    scope.theElement = iElement;
                    scope.removeAttachment = function(attachment, index) {

                        if (attachment.isForUpload) {
                            scope.ownerEntity.Attachments.splice(index, 1);
                            scope.uploader.removeFromQueue(scope.getItem(attachment.FileName));
                        } else {
                            alertify.confirm(
                                'This action cannot be undo, do you want to continue?',
                                function() {
                                    scope.$apply(function() {
                                        $activityIndicator.startAnimating();
                                        $http.get(appConfig.API_URL + 'attachment_delete?directory=' + attachment.Directory + '&fileName=' + attachment.FileName + '&attachmentKind=' + scope.kind + '&noCache=' + Number(new Date())).then(function(data) {
                                            var backendResponse = data;
                                            if (!backendResponse.ErrorThrown) {
                                                scope.ownerEntity.Attachments.splice(index, 1);
                                                $timeout(function() {
                                                    alertify.success('File deleted successfully.');
                                                }, 100);
                                            } else {
                                                alertify.alert('An error has occurried, see console for more details.').set('modal', true);
                                                console.debug(response);
                                            }
                                            scope.afterDelete({
                                                oEntity: scope.ownerEntity
                                            });
                                            $activityIndicator.stopAnimating();
                                        }, function(data) {
                                            alertify.alert('An error has occurried, see console for more details.').set('modal', true);
                                            console.debug(data);
                                            $activityIndicator.stopAnimating();
                                        });
                                    });
                                });
                        }
                    };
                    scope.getItem = function(sName) {
                        for (var i = 0; i < scope.uploader.queue.length; i++) {
                            if (scope.uploader.queue[i].file.name == sName) {
                                return scope.uploader.queue[i];
                            }
                        }
                        return null;
                    };
                    scope.addAttachment = function() {
                        if (!scope.readOnly) {
                            $timeout(function() {
                                scope.theElement.find('input[type="file"]').click();
                            });
                        }
                    };
                    scope.getAttachment = function(sName) {
                        for (var i = 0; i < scope.ownerEntity.Attachments.length; i++) {
                            if (scope.ownerEntity.Attachments[i].FileName == sName) {
                                return scope.ownerEntity.Attachments[i];
                            }
                        }
                        return null;
                    };
                }
            };
        }
    };
});
