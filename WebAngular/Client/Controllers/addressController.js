
var adrApp = angular.module('adrApp');
adrApp.controller('addressController',
    function addressController($scope, $http) { //$http объязателен для ajax, он и выполняет его
        $scope.todos = $http.get('http://localhost:9476//Home//GetAddress'). //наш контроллер с методом для получания списка
                then(function success(result) {
                    $scope.ListAddress = result.data.Address; //получили и передали в $scope.todos, тут нужно многое сказать про $scope, но все это уже есть в официальном справочнике на ихнем же сайте
                    $scope.aaa = 123;
      //  $http({method: 'GET', url: 'question.json'}).
      //      then(function success(response) {
           //     $scope.question=response.data.question;
                });
        
});
