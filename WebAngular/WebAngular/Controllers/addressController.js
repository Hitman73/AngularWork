var adrApp = angular.module('adrApp');
//директива для окна
adrApp.directive('popUpDialog', function () {
    return {
        restrict: 'E',
        scope: false,
        templateUrl: 'http://localhost:9476//App//SliderDialog.html',
        controller: function ($scope) {

            $scope.showPopUpDialog = false;

            $scope.closePopUpDialog = function () {
                $scope.showPopUpDialog = false;
            }

            $scope.popUpDialogApprove = function () {
                //$scope[$scope.popUpDialogCallback]();
                $scope.showPopUpDialog = false;
            }
        }
    }
})
//фабрика для пагинации
adrApp.factory('pagination', function ($sce) {
    var currentPage = 0;        //текущая страница
    var itemsPerPage = 20;      //кол-во записей на странице
    var address = [];           //массив адресов

    return {
        //наполняем массив
        fillArrayAddress: function (new_address) { 
            address = new_address;
        },

        //ввернем записи для одной страници
        getPageAddress: function (numPage) {
            var numPage = angular.isUndefined(numPage) ? 0 : numPage;
            var first = itemsPerPage * numPage;
            var last = first + itemsPerPage;

            currentPage = numPage;
            last = last > address.length ? (address.length) : last;
            return address.slice(first, last);
        },

        //вернем максимальное кол-во страниц
        getPagesMax: function () {
            return Math.ceil(address.length / itemsPerPage);
        },

        //формируем список страниц
        getPaginationList: function () {
            var pagesNum = this.getPagesMax();
            var paginationList = [];
            paginationList.push({
                name: $sce.trustAsHtml('&laquo;'),
                link: 'prev'
            });
            for (var i = 0; i < pagesNum; i++) {
                var name = i + 1;
                paginationList.push({
                    name: $sce.trustAsHtml(String(name)),
                    link: i
                });
            };
            paginationList.push({
                name: $sce.trustAsHtml('&raquo;'),
                link: 'next'
            });

            if (pagesNum > 1) {
                return paginationList;
            } else {
                return null;
            }
        },

        //возвращаем текущую страницу
        getCurrentPageNum: function () {
            return currentPage;
        },

        //вернем записи для предыдущей страници
        getPrevPageAddress: function () {
            var prevPageNum = currentPage - 1;
            if (prevPageNum < 0) prevPageNum = 0;
            return this.getPageAddress(prevPageNum);
        }, 

        //вернем записи для следующей страници
        getNextPageAddress: function () {
            var nextPageNum = currentPage + 1;
            var pagesNum = this.getPagesMax();
            if (nextPageNum >= pagesNum) nextPageNum = pagesNum - 1;
            return this.getPageAddress(nextPageNum);
        }
    }
});

adrApp.controller('myCtrl', 
    function ($scope, $http, pagination, translationService) { //$http объязателен для ajax, он и выполняет его

    $scope.sortType = '';       // значение сортировки по умолчанию
    $scope.sortReverse = true;  // обратная сортривка  
    $scope.selectedLanguage = 'ru'; //выбранный язык страницы

    $scope.my_filter = {        // значение поиска по умолчанию
        f_country: '',
        f_city: '',
        f_street: '',
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


    $scope.sliderDialog = function () {
        $scope.popUpDialogContent = 'Запустить таймер?';
        //$scope.popUpDialogCallback = 'activateTimer';
        console.log("click");
        $scope.showPopUpDialog = true;
    }

    $http.get('http://localhost:9476//Home//GetAddress') //наш контроллер с методом для получания списка
        .then(function success(result) {
            //$scope.ListAddress = result.data; //получили и передали в $scope.todos, тут нужно многое сказать про $scope, но все это уже есть в официальном справочнике на ихнем же сайте/
            pagination.fillArrayAddress(result.data);
            $scope.ListAddress = pagination.getPageAddress(0);
            $scope.paginationList = pagination.getPaginationList();
        });

    $scope.showPage = function (page) {
        if (page == 'prev') {
            $scope.ListAddress = pagination.getPrevPageAddress();
        } else if (page == 'next') {
            $scope.ListAddress = pagination.getNextPageAddress();
        } else {
            $scope.ListAddress = pagination.getPageAddress(page);
        }

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