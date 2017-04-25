'use strict';

describe('Controller: EmailaccountCtrl', function () {

  // load the controller's module
  beforeEach(module('appApp'));

  var EmailaccountCtrl,
    scope;

  // Initialize the controller and a mock scope
  beforeEach(inject(function ($controller, $rootScope) {
    scope = $rootScope.$new();
    EmailaccountCtrl = $controller('EmailaccountCtrl', {
      $scope: scope
      // place here mocked dependencies
    });
  }));

  it('should attach a list of awesomeThings to the scope', function () {
    expect(EmailaccountCtrl.awesomeThings.length).toBe(3);
  });
});
