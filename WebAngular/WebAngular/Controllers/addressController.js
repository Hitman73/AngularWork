var adrApp = angular.module('adrApp');


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
            last = last > address.length ? (address.length - 1) : last;
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

adrApp.controller('myCtrl', function ($scope, $http, pagination) { //$http объязателен для ajax, он и выполняет его

    $scope.sortType = 'Id'; // значение сортировки по умолчанию
    $scope.sortReverse = false;  // обратная сортривка
    $scope.searchFish = '';     // значение поиска по умолчанию


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
        $http({ method: 'POST', url: 'http://localhost:9476//Home//SortColumn', params: { 'sortType': $scope.sortType, 'sortReverse': $scope.sortReverse } }).
         then(function success(response) {
             pagination.fillArrayAddress(response.data);
             $scope.ListAddress = pagination.getPageAddress(0);
             $scope.paginationList = pagination.getPaginationList();
             console.log(response.data);
         })
    };
    $scope.send = function (answer) {
        $http({ method: 'POST', url: 'http://localhost:9476//Home//SetPages', params: { 'id': answer } }).
         then(function success(response) {
             console.log(response.data);
         })
    };

    

    //$http.get('http://localhost:9476//Home//GetPages') //наш контроллер с методом для получания кол-ва страниц
    //    .then(function success(result) {
    //        $scope.pages = result.data; //получили и передали в $scope.todos, тут нужно многое сказать про $scope, но все это уже есть в официальном справочнике на ихнем же сайте
    //    });

    
});