'use strict';

describe('Controller: CqaCtrl', function () {

  // load the controller's module
  beforeEach(module('appApp'));

  var CqaCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    CqaCtrl = $controller('CqaCtrl', {
      $scope: scope
      // place here mocked dependencies
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(CqaCtrl.awesomeThings.length).toBe(3);
  });
});
