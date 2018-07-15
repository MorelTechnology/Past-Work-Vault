(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('modalErrorCtrl', ["sharedProperties", "$uibModalInstance", "$scope", "errorMessage",
            function (sharedProperties, $uibModalInstance, $scope, errorMessage) {
            $scope.errorMessage = errorMessage.value;
            $scope.ok = function () {
                $uibModalInstance.close('cancel');
            }
        }]);
})();
