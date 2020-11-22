var app = angular.module('umbraco');

app.controller('UmakersPickerController', function ($scope, $http) {

    $scope.categories = [];

    var fileurl = $scope.model.config.fileurl;
    if (!fileurl) {
        console.error("UmakersPicker - error: " + fileurl + " not found");

    }

    $scope.onLoad = function () {
        $http({
            method: 'GET',
            url: fileurl
        }).then(function successCallback(response) {
       
            $scope.itemList = response.data;
            if ($scope.model.value === null || $scope.model.value === "" || $scope.model.value === {}) {
              
                if ($scope.itemList) {

                    // if no value then set the default value or first if no default value
                    if ($scope.itemList.filter(xx => xx.default)[0]) {
                        $scope.model.value = $scope.itemList.filter(xx => xx.default)[0];
                    } else {
                        $scope.model.value = $scope.itemList[0];
                    }
                }

           
            }
        }, function errorCallback(response) {
            console.error("UmakersPicker - error: " + fileurl + " not found");
        });
    };

    $scope.onLoad();

    $scope.save = function (item) {
        $scope.show = 0;
        $scope.model.value = item;

    };
});

