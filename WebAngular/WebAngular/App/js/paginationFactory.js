//фабрика для пагинации
adrApp.factory('pagination', function ($sce) {
    var currentPage = 0;        //текущая страница
    var itemsPerPage = 100;      //кол-во записей на странице
    var maxPage = 10;           //макс кол-во страниц для пагинации 
    var listPage = 0;
    var address = [];           //массив адресов
    var lastPage = 0;
    var length = 0;

    return {
        //наполняем массив
        fillArrayAddress: function (new_address, new_length) {
            address = new_address;
            length = new_length;
        },

        //ввернем записи для одной страници
        getPageAddress: function (numPage) {
            //var numPage = angular.isUndefined(numPage) ? 0 : numPage;
            //var first = itemsPerPage * numPage;
            //var last = first + itemsPerPage;

            //currentPage = numPage;
            
            //last = last > address.length ? (address.length) : last;
            return address;
        },

        //вернем максимальное кол-во страниц
        getPagesMax: function () {
            return Math.ceil(length / itemsPerPage);
        },

        //формируем список страниц
        getPaginationList: function (page) {
            var pagesNum = this.getPagesMax();
            var paginationList = [];


            var startPage, endPage;
            var totalPages = pagesNum;

            currentPage = page;
            if (totalPages <= 10) {
                // less than 10 total pages so show all
                startPage = 0;
                endPage = totalPages;
            } else {
                // more than 10 total pages so calculate start and end pages
                if (currentPage < 5) {
                    startPage = 0;
                    endPage = 10;
                } else if (currentPage + 5 >= totalPages) {
                    startPage = totalPages - 10;
                    endPage = totalPages;
                } else {
                    startPage = currentPage - 4;
                    endPage = currentPage + 6;
                }
            }
            console.log(currentPage, startPage, endPage, totalPages);

            paginationList.push({
                name: $sce.trustAsHtml('&laquo;'),
                link: 'prev'
            });

            for (var i = startPage; i < (endPage) ; i++) {
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
        getPrevPageAddress: function (curPage) {
            var prevPageNum = curPage - 1;
            if (prevPageNum < 0) prevPageNum = 0;
            return prevPageNum;
        },

        //вернем записи для следующей страници
        getNextPageAddress: function (curPage) {
            var nextPageNum = curPage + 1;
            var pagesNum = this.getPagesMax();
            if (nextPageNum >= pagesNum) nextPageNum = pagesNum - 1;
            return nextPageNum;
        }
    }
});