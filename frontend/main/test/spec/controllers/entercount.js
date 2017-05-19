'use strict';

describe('Controller: EntercountCtrl', function () {

  // load the controller's module
  beforeEach(module('appApp'));

  var EntercountCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    EntercountCtrl = $controller('EntercountCtrl', {
      $scope: scope
      // place here mocked dependencies
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(EntercountCtrl.awesomeThings.length).toBe(3);
  });
});
