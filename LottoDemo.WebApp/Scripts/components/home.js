/**
 * Created with JetBrains PhpStorm.
 * User: tafadzwa
 * Date: 9/19/14
 * Time: 11:59 AM
 * To change this template use File | Settings | File Templates.
 */
'use strict';
home_app.controller('homeCtrl', function ($scope) {
});


home_app.controller('carouselCtrl', function ($scope, $http, services) {
    $scope.w = window.innerWidth;
    $scope.h = window.innerHeight-20;

    $scope.culture = document.getElementById("culture").value;

    services.getCarouselImages($scope.culture).then(function(data){
        $scope.images = data['data']['images'];
        $scope.jackpot_details = data['data']['jackpot_details'];
    });

//  $scope.images = [
//    {"id":0, "src":"/site/playhugelottos_com/images/playnow_bg-en.png", "alt": "Jackpot"},
//    {"id":1, "src":"/images/frontend/slider/AbuDhabi-Grand-prix-final-en_1409582792.jpg", "alt": "Campaign 1"},
//    {"id":2, "src":"/images/frontend/slider/PHL-carousel-new-en-de-pl_1404220903.jpg", "alt": "Campaign 2"},
//    {"id":3, "src":"/images/frontend/slider/New-SuperenaMax-Carousel-en_1406274235.jpg", "alt": "Campaign 3"}
//  ];

});
