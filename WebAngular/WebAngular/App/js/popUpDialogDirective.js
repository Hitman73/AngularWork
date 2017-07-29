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