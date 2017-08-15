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
        sortReverse: true,       // обратная сортривка 
        page : 0
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
        sortReverse: true,
        page: 0
    };

    $scope.DateRange = moment()
    $scope.dateRangeChanged = function () {
        console.log($scope.dateRange);
    }

    $scope.saveToLog = function (operation, text) {
        console.log(operation, text);
        $http({ method: 'GET', url: '/Home/SaveLog', params: { operation: operation, text: text } }).
            then(function success(response) {
                console.log(response.data);
            })
    };

    //Выполняем перевод, если произошло событие смены языка
    $scope.translate = function () {
        //запишем лог
        var strText = 'переключения языка на ' + $scope.selectedLanguage;
        $scope.saveToLog('Переключения языка', strText);
        //переведем страницу
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

    //получаем данные из таблицы
    $scope.getAddress = function (prm, page) {
        $scope.my_filter.page = $scope.old_my_filter.page = page;
        $http({ method: 'GET', url: '/Home/SortFilterColumn', params: prm })
        .then(function success(result) {
            console.log(result.data);
            pagination.fillArrayAddress(result.data.data, result.data.count);
            $scope.ListAddress = pagination.getPageAddress(0);
            $scope.paginationList = pagination.getPaginationList(page);
        });
    }

    $scope.getAddress($scope.my_filter, 0);    //получим данные
    //переход на страницу
    $scope.showPage = function (page) {
        //запишем лог
        var strText = 'переход на новую страницу ';
        var new_page;
        $scope.saveToLog('Переход на страницу', strText);

        if (page == 'prev') {
            new_page = pagination.getPrevPageAddress($scope.my_filter.page);
        } else if (page == 'next') {
            new_page = pagination.getNextPageAddress($scope.my_filter.page);
        } else {
            new_page = page;
        }
        $scope.getAddress($scope.my_filter, new_page);
    };

    $scope.currentPageNum = function () {
        return pagination.getCurrentPageNum();
    };
    // сортировка по столбцу
    $scope.sortColumn = function (sortType) {        
        $scope.my_filter.sortType = sortType;
        $scope.my_filter.sortReverse = !$scope.my_filter.sortReverse;
        //запишем лог
        var strText = 'сортировка по столбцу ' + sortType + ($scope.my_filter.sortReverse == false ? ' по убыванию' : ' по возрастанию');
        $scope.saveToLog('Сортировка', strText);

        $scope.old_my_filter.sortType = $scope.my_filter.sortType;
        $scope.old_my_filter.sortReverse = $scope.my_filter.sortReverse;

        $scope.getAddress($scope.old_my_filter, 0);
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
        //запишем лог
        var strText = 'параметры фильтра : Страна = ' + $scope.my_filter.f_country + ', Город = ' + $scope.my_filter.f_city + 
            ', Улица = ' + $scope.my_filter.f_street + ', Номер дома = ' +  $scope.my_filter.f_house + 
            ', Индекс = ' + $scope.my_filter.f_index + ', Дата = ' + $scope.my_filter.f_date;
        $scope.saveToLog('Фильтрация', strText);
        //сохраним фильтр
        $scope.saveFilter();

        $scope.getAddress($scope.my_filter, 0);

    };
        //Функция сброса фильтра
    $scope.resetFilter = function () {
        $scope.saveToLog('Сброс фильтра', 'сброс фильтра');
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

    $scope.clear_filter = function (param) {
        if (param == 'country') {
            $scope.my_filter.f_country = '';
        } else if (param == 'city') {
            $scope.my_filter.f_city = '';
        } else if (param == 'street') {
          $scope.my_filter.f_street = '';
        } else if (param == 'number') {
            $scope.my_filter.f_house = '';
            $scope.my_filter.f_number_min = 1;
            $scope.my_filter.f_number_max = 200;
        } else if (param == 'index') {
            $scope.my_filter.f_index = '';
        } else if (param == 'date') {
            $scope.my_filter.f_date = '';
            $scope.my_filter.f_StartDate = '';
            $scope.my_filter.f_EndDate = '';
        }
        $scope.saveToLog('Сброс фильтра по полю ', param);
        //сохраним фильтр
        $scope.saveFilter();
        console.log(param);
    }

});