var adrApp = angular.module('adrApp');

adrApp.controller('myCtrl', 
    function ($scope, $http, pagination, translationService) { //$http объязателен для ajax, он и выполняет его 
    $scope.selectedLanguage = 'ru'; //выбранный язык страницы

    $scope.my_filter = {        // значение поиска по умолчанию
        f_country: '',
        f_city: '',
        f_street: '',
        f_house: '',
        f_number_min: 1,
        f_number_max: 200,
        f_index: '',
        f_date: '',
        f_StartDate: '',
        f_EndDate: '',
        sortType: '',           // значение сортировки по умолчанию
        sortReverse: true       // обратная сортривка 
    };

    $scope.old_my_filter = {        // значение поиска по умолчанию
        f_country: '',
        f_city: '',
        f_street: '',
        f_house: '',
        f_number_min: 1,
        f_number_max: 200,
        f_index: '',
        f_date: '',
        f_StartDate: '',
        f_EndDate: '',
        sortType: '',
        sortReverse: true
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
        $scope.my_filter.f_house = ''+$scope.my_filter.f_number_min +'-'+ $scope.my_filter.f_number_max;//$scope.my_filter.f_number_min - $scope.my_filter.f_number_max;
        console.log("activateFilter");
    }

    //установка выбранного диапазона дат
    $scope.setDate = function (startDate, endDate) {
        console.log(startDate, endDate);
        $scope.my_filter.f_StartDate = startDate;
        $scope.my_filter.f_EndDate = endDate;
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
    // сортировка по столбцу
    $scope.sortColumn = function (sortType) {
        $scope.my_filter.sortType = sortType;
        $scope.my_filter.sortReverse = !$scope.my_filter.sortReverse;

        $scope.old_my_filter.sortType = $scope.my_filter.sortType;
        $scope.old_my_filter.sortReverse = $scope.my_filter.sortReverse;
        $http({ method: 'POST', url: 'http://localhost:9476//Home//SortFilterColumn', params: $scope.old_my_filter }).
         then(function success(response) {
             pagination.fillArrayAddress(response.data);
             $scope.ListAddress = pagination.getPageAddress(0);
             $scope.paginationList = pagination.getPaginationList();
             console.log(response.data);
         })
    };

        //проверим сортируется ли столбец,
        // или применяется ли к нему фильтр
    $scope.isSortColumn = function (name) {
        var isSortFilter = false;
        var x = false;
        isSortFilter = ($scope.my_filter.sortType == name);
        
        switch(name) {
            case 'Country': x = ($scope.my_filter.f_country != '');
                break;
            case 'City': x = ($scope.my_filter.f_city != '');
                break;
            case 'Srtreet': x = ($scope.my_filter.f_street != '');
                break;
            case 'Num': x = ($scope.my_filter.f_house != '');
                break;
            case 'Index': x = ($scope.my_filter.f_index != '');
                break;
            case 'Date': x = ($scope.my_filter.f_date != '');
                break;
        }
        isSortFilter = isSortFilter || x;
        return isSortFilter;
    };

    $scope.filter = function () {
        $scope.saveFilter();
        $http({ method: 'POST', url: 'http://localhost:9476//Home//SortFilterColumn', params: $scope.my_filter }).
         then(function success(response) {
             pagination.fillArrayAddress(response.data);
             $scope.ListAddress = pagination.getPageAddress(0);
             $scope.paginationList = pagination.getPaginationList();
             console.log(response.data);
         })
    };
        //Функция сброса фильтра
    $scope.resetFilter = function () {
        console.log($scope.filterForm.$valid);
        $scope.my_filter.f_country = '';
        $scope.my_filter.f_city = '';
        $scope.my_filter.f_street = '';
        $scope.my_filter.f_house = '';
        $scope.my_filter.f_number_min = 1;
        $scope.my_filter.f_number_max = 200;
        $scope.my_filter.f_index = '';
        $scope.my_filter.f_date = '';
        $scope.my_filter.f_StartDate = '';
        $scope.my_filter.f_EndDate = '';
        $scope.filter();
    };

        //сохраним настройки фильтра
    $scope.saveFilter = function () {
        console.log($scope.filterForm.$valid);
        $scope.old_my_filter.f_country = $scope.my_filter.f_country;
        $scope.old_my_filter.f_city = $scope.my_filter.f_city;
        $scope.old_my_filter.f_street = $scope.my_filter.f_street;
        $scope.old_my_filter.f_house = $scope.my_filter.f_house;
        $scope.old_my_filter.f_number_min = $scope.my_filter.f_number_min;
        $scope.old_my_filter.f_number_max = $scope.my_filter.f_number_max;
        $scope.old_my_filter.f_index = $scope.my_filter.f_index;
        $scope.old_my_filter.f_date = $scope.my_filter.f_date;
        $scope.old_my_filter.f_StartDate = $scope.my_filter.f_StartDate;
        $scope.old_my_filter.f_EndDate = $scope.my_filter.f_EndDate;
    };
});