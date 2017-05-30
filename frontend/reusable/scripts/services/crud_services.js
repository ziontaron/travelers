'use strict';

/**
 * @ngdoc service
 * @name appApp.userService
 * @description
 * # userService
 * Service in the appApp.
 */

angular.module('CRUDServices', [])

.service('utilsService', function($filter) {

    var service = {};

    service.toJavascriptDate = function(sISO_8601_Date) {
        return sISO_8601_Date ? moment(sISO_8601_Date, moment.ISO_8601).toDate() : null;
    };

    service.toServerDate = function(oDate) {
        var momentDate = moment(oDate);
        if (momentDate.isValid()) {
            momentDate.local();
            return momentDate.format();
        }
        return null;
    };

    return service;
})

.service('userService', function(crudFactory) {
    var crudInstance = new crudFactory({
        //Entity Name = WebService/API to call:
        entityName: 'User',

        catalogs: [],

        adapter: function(theEntity) {
            return theEntity;
        },

        adapterIn: function(theEntity) {},

        adapterOut: function(theEntity, self) {
            theEntity.Identicon = "";
            theEntity.Identicon64 = "";
        },

        dependencies: [

        ]
    });

    crudInstance.getByUserName = function(sUserName) {
        var _arrAllRecords = crudInstance.getAll();
        for (var i = 0; i < _arrAllRecords.length; i++) {
            if (_arrAllRecords[i].UserName == sUserName) {
                return _arrAllRecords[i];
            }
        }
        return {
            id: -1,
            Value: ''
        };
    };

    crudInstance.getUsersInRoles = function(arrRoles) {
        var _arrAllRecords = crudInstance.getAll();
        var result = [];
        for (var i = 0; i < _arrAllRecords.length; i++) {
            if (arrRoles.indexOf(_arrAllRecords[i].Role) > -1) {
                result.push(_arrAllRecords[i]);
            }
        }
        result.push(_arrAllRecords[0]);
        return result;
    };


    crudInstance.sendTestEmail = function(oUser) {
        return crudInstance.customPost('SendTestEmail', oUser);
    };

    return crudInstance;
}).service('TravelerHeaderService', function(crudFactory, utilsService) {
    var crudInstance = new crudFactory({
        entityName: 'Traveler',

        catalogs: [],

        adapter: function(theEntity) {
            theEntity.ConvertedCreatedDate = utilsService.toJavascriptDate(theEntity.CreatedDate);
            // theEntity.ConvertedClosedDate = utilsService.toJavascriptDate(theEntity.ClosedDate);
            return theEntity;
        },

        adapterOut: function(theEntity, self) {
            theEntity.CreatedDate = utilsService.toServerDate(theEntity.ConvertedCreatedDate);
            // theEntity.ClosedDate = utilsService.toServerDate(theEntity.ConvertedClosedDate);
        },

        dependencies: []
    });

    return crudInstance;

}).service('CustomerService', function(crudFactory) {
    var crudInstance = new crudFactory({
        //Entity Name = WebService/API to call:
        entityName: 'FSCustomer',

        catalogs: [],

        adapter: function(theEntity) {
            return theEntity;
        },

        adapterIn: function(theEntity) {},

        adapterOut: function(theEntity, self) {},

        dependencies: []
    });

    return crudInstance;
});
