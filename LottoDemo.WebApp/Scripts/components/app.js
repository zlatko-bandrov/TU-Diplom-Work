var home_app = angular.module('homeApp', ['ngRoute', 'angular-carousel', 'timer', 'ngSanitize']);

home_app.config(['$routeProvider', '$httpProvider', '$locationProvider', function($routeProvider, $httpProvider, $locationProvider) {
  $locationProvider.html5Mode(true);
  $locationProvider.hashPrefix('!');

  //$httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
  $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
  $httpProvider.defaults.withCredentials = true;

}]);

home_app.run(function($rootScope, $window) {
  var host = $window.location.hostname;
  $rootScope.api_base = host.replace(/(^www\.)/, "") + '/api';
});

