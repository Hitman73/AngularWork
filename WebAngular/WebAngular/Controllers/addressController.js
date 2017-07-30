var adrApp = angular.module('adrApp');

adrApp.controller('myCtrl', 
    function ($scope, $http, pagination, translationService) { //$http объязателен для ajax, он и выполняет его

    $scope.sortType = '';       // значение сортировки по умолчанию
    $scope.sortReverse = true;  // обратная сортривка  
    $scope.selectedLanguage = 'ru'; //выбранный язык страницы

    $scope.my_filter = {        // значение поиска по умолчанию
        f_country: '',
        f_city: '',
        f_street: '',
        f_house: '',
        f_number_min: 1,
        f_number_max: 200,
        f_index: '',
        f_date: ''
    };

    $scope.DateRange = moment()
    $scope.dateRangeChanged = function () {
        console.log($scope.dateRange);
    }

    //Выполняем перевод, если произошло событие смены языка
    $scope.translate = function () {
        translationService.getTranslation($scope, $scope.selectedLanguage);
    };
    // Инициализация
    $scope.translate();

    //откроем диалоговое окно
    $scope.sliderDialog = function () {
        $scope.popUpDialogCallback = 'activateFilter';        
        $scope.showPopUpDialog = true;
    }

    $scope.activateFilter = function () {
        $scope.my_filter.f_house = 'zxcv';//$scope.my_filter.f_number_min - $scope.my_filter.f_number_max;
        console.log("activateFilter");
    }

    $http.get('http://localhost:9476//Home//GetAddress') //наш контроллер с методом для получания списка
        .then(function success(result) {
            //$scope.ListAddress = result.data; //получили и передали в $scope.todos, тут нужно многое сказать про $scope, но все это уже есть в официальном справочнике на ихнем же сайте/
            pagination.fillArrayAddress(result.data);
            $scope.ListAddress = pagination.getPageAddress(0);
            $scope.paginationList = pagination.getPaginationList();
        });

    $scope.showPage = function (page) {
        console.log(page);
        if (page == 'prev') {
            $scope.ListAddress = pagination.getPrevPageAddress();
        } else if (page == 'next') {
            $scope.ListAddress = pagination.getNextPageAddress();
        } else {
            $scope.ListAddress = pagination.getPageAddress(page);
        }
        $scope.paginationList = pagination.getPaginationList();
        
    };

    $scope.currentPageNum = function () {
        return pagination.getCurrentPageNum();
    };

    $scope.sortColumn = function (sortType) {
        $scope.sortType = sortType;
        $scope.sortReverse = !$scope.sortReverse;
        $http({ method: 'POST', url: 'http://localhost:9476//Home//SortColumn', params: { 'sortType': $scope.sortType, 'sortReverse': $scope.sortReverse } }).
         then(function success(response) {
             pagination.fillArrayAddress(response.data);
             $scope.ListAddress = pagination.getPageAddress(0);
             $scope.paginationList = pagination.getPaginationList();
             console.log(response.data);
         })
    };

    $scope.filter = function () {
        console.log("f_country %s f_city  %s f_street  %s f_number  %s", 
                    $scope.my_filter.f_country, $scope.my_filter.f_city, $scope.my_filter.f_street, $scope.my_filter.f_number);
        $http({ method: 'POST', url: 'http://localhost:9476//Home//FilterDateTable', params: $scope.my_filter }).
         then(function success(response) {
             pagination.fillArrayAddress(response.data);
             $scope.ListAddress = pagination.getPageAddress(0);
             $scope.paginationList = pagination.getPaginationList();
             console.log(response.data);
         })
    };
});