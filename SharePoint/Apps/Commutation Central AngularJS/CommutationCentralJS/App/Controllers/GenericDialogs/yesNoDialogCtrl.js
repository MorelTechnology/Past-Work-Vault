(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('yesNoDialogCtrl', ["$scope", "$location", "$uibModalInstance", "dialogProperties",
            function ($scope, $location, $uibModalInstance, dialogProperties) {
                $scope.dialogProperties = dialogProperties;
                $scope.buttonYes = function () {
                    $uibModalInstance.close(true)
                }
                $scope.buttonNo = function () {
                    $uibModalInstance.close(false);
                }
            }]);
})();
