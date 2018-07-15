(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editDroppedReasonItemCtrl', ["$scope", "droppedReasonItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.droppedReason = {
                        itemId: response.data.d.ID,
                        DroppedReasonName: response.data.d.DroppedReasonName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (droppedReason) {
                    $scope.inProgress = true;
                    itemFactory.update(droppedReason)
                    .then(function (response) {
                        $location.path("/DroppedReasons/All");
                    })
                    .finally(function(){
                        $scope.inProgress = false;
                    });
                };

                $scope.cancel = function () {
                    $window.history.back();
                };
        }]);
})();
