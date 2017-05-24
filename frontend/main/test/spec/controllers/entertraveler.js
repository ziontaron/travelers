'use strict';

describe('Controller: EntertravelerCtrl', function () {

  // load the controller's module
  beforeEach(module('appApp'));

  var EntertravelerCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    EntertravelerCtrl = $controller('EntertravelerCtrl', {
      $scope: scope
      // place here mocked dependencies
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(EntertravelerCtrl.awesomeThings.length).toBe(3);
  });
});
