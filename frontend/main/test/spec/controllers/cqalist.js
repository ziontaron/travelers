'use strict';

describe('Controller: CqalistCtrl', function () {

  // load the controller's module
  beforeEach(module('appApp'));

  var CqalistCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    CqalistCtrl = $controller('CqalistCtrl', {
      $scope: scope
      // place here mocked dependencies
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(CqalistCtrl.awesomeThings.length).toBe(3);
  });
});
