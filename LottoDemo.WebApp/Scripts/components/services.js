home_app.factory("services", ['$http', function($http) {
  var obj = {};
  obj.getCarouselImages = function(culture){
    return $http.get('/'+culture+'/home/carousel.html');
  };

  return obj;
}]);
