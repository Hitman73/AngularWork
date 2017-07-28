var adrApp = angular.module('adrApp');
adrApp.service('translationService', function ($resource) {

        this.getTranslation = function($scope, language) {
            var languageFilePath = 'http://localhost:9476//App//data//translation_' + language + '.json';
            console.log(languageFilePath);
            $resource(languageFilePath).get(function (data) {
                $scope.translation = data;
            });
        };
    });
