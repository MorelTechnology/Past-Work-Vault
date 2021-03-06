﻿(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('addLeadOfficeItemCtrl', ["$scope", "leadOfficeItemFactory", "$location", "$uibModalInstance", function ($scope, itemFactory, $location, $uibModalInstance) {
            $scope.addItem = function (leadOffice) {
                $scope.inProgress = true;
                itemFactory.addNew(leadOffice)
                .then(function (response) {
                    var newItem = { ID: response.data.d.ID, Id: response.data.d.ID, LeadOfficeName: response.data.d.LeadOfficeName };
                    $uibModalInstance.close(newItem);
                })
                .finally(function () {
                    $scope.inProgress = false;
                });
            };
            $scope.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            }
        }]);
})();
