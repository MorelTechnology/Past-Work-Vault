(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editNoteEntryTypeItemCtrl', ["$scope", "noteEntryTypeItemFactory", "$routeParams", "$location", "$window",
            function ($scope, itemFactory, $routeParams, $location, $window) {
                $scope.inProgress = true;
                itemFactory.getById($routeParams.itemId)
                .then(function (response) {
                    $scope.noteEntryType = {
                        itemId: response.data.d.ID,
                        NoteEntryTypeName: response.data.d.NoteEntryTypeName
                    };
                })
                .finally(function(){
                    $scope.inProgress = false;
                });

                $scope.editItem = function (noteEntryType) {
                    $scope.inProgress = true;
                    itemFactory.update(noteEntryType)
                    .then(function (response) {
                        $location.path("/NoteEntryTypes/All");
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
