(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('leftNav', ['$scope', '$location', '$uibModal', function ($scope, $location, $uibModal, path) {
            $scope.oneAtATime = false;
            $scope.animationsEnabled = true;
            $scope.getClass = function (path) {
                if ($location.path() === path) {
                    return 'level2 static selected';
                } else {
                    return 'level2 static';
                }
            };
            $scope.openModal = function (templateUrl, controllerName, windowClass) {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: templateUrl,
                    controller: controllerName,
                    windowClass: windowClass,
                    backdrop: 'static'
                });
            };
            $scope.openBlankActivityModal = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/Activities/Add.html",
                    controller: "addActivityItemCtrl",
                    windowClass: "new-activity-dialog",
                    backdrop: 'static',
                    resolve: {
                        project: { ID: null, CounterpartyName: null },
                        projectPreResolved: false
                    }
                });
            };
            $scope.openBlankNoteModal = function () {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: "../App/Templates/Notes/Add.html",
                    controller: "addNoteItemCtrl",
                    windowClass: "new-note-dialog",
                    backdrop: 'static',
                    resolve: {
                        project: { ID: null, CounterpartyName: null },
                        projectPreResolved: false
                    }
                });
            };
        }]);
})();
