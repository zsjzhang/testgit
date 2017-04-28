var questionnaire = angular.module('questionnaire', ['ngGrid']);

questionnaire.controller('OptionCtrl', function ($scope, $http, TreeData) {
    $scope.addChildrenOption = function () {
        $http.post("",
            { content: $scope.optionData.Content })
    };
});
