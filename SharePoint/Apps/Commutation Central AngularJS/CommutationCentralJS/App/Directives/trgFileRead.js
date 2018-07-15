(function() {
    'use strict';

    angular
        .module('commutationCentral')
        .directive('trgFileRead', ["$q", function ($q) {
            return {
                scope: {
                    trgFileRead: "="
                },
                link: function (scope, element, attributes) {
                    element.bind("change", function (changeEvent) {
                        scope.$apply(function () {
                            scope.trgFileRead = [];
                            for (var i = 0; i < changeEvent.target.files.length; i++) {
                                (function (file) {
                                    /*var deferred = $q.defer();
                                    var reader = new FileReader();
                                    var filename = changeEvent.target.files[i].name;
                                    var currentFile = { filename: filename };
                                    reader.onload = function (loadEvent) {
                                        currentFile.filecontents = loadEvent.target.result;
                                        scope.trgFileRead.push(currentFile);
                                    };
                                    reader.onloadend = function (loadEndEvent) {
                                        deferred.resolve(loadEndEvent.target.result);
                                    };
                                    reader.readAsArrayBuffer(changeEvent.target.files[i]);
                                    currentFile.filecontents = deferred.promise;
                                    scope.trgFileRead.push(currentFile);*/
                                    var filename = file.name;
                                    getFileBuffer(file).then(function (fileContents) {
                                        var currentFile = { filename: filename, filecontents: fileContents };
                                        scope.trgFileRead.push(currentFile);
                                    }, function (error) { var i = error; });
                                })(changeEvent.target.files[i]);
                            }
                        });
                    });
                }
            }

            function getFileBuffer(fileContents) {
                var deferred = $q.defer();
                var reader = new FileReader();
                reader.onloadend = function (e) {
                    deferred.resolve(reader.result);
                };
                reader.onerror = function (e) {
                    deferred.reject(reader.err);
                };
                reader.readAsArrayBuffer(fileContents);
                return deferred.promise;
            }
        }]);

})();