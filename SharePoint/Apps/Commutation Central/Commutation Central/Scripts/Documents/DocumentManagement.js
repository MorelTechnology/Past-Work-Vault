'use strict';

var appWebUrl, hostWebUrl

$(document).ready(function () {
    // Check for FileReader API (HTML5) support.
    if (!window.FileReader) {
        alert('This browser does not support file uploads. File upload boxes are disabled.');
        $(".fileUpload").attr('disabled', 'disabled');
    }

    // Get the app web and host web URLs.
    appWebUrl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
    hostWebUrl = decodeURIComponent(getQueryStringParameter("SPHostUrl"));
});

// Upload a document.
// You can upload files up to 2 GB with the REST API.
// Accepts the types 'Project Document', 'Activity Document', 'Note Document', 'Deal Check List Document'
function uploadDocument(sender, contentType, lookupField, lookupFieldValue, counterpartyName, finalCallback) {
    // Display the saving dialog
    $("#savingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: sender },
        modal: true,
        resizable: false,
        height: 120
    });

    // Define the foler path for the library.
    var serverRelativeUrlToFolder = 'Lists/Commutation Documents';

    // Get values from the file input page control.
    var fileInput = sender;
    var parts = fileInput[0].value.split('\\');
    var fileName = parts[parts.length - 1];

    // Get the local file as an array buffer
    getFileBuffer().done(function (arrayBuffer) {

        // Check if the file exists. If it does, we need to figure out how to handle the versioning
        CheckFileExists(fileName).done(function (fileCheckResults) {

            if (fileCheckResults.d.results.length > 0) {

                // See if the existing file was already added to this item
                if (fileCheckResults.d.results[0].ListItemAllFields[lookupField + "Id"] == lookupFieldValue) {

                    // If this file was already added to this item, we still want to upload and overwrite the version
                    ExecuteFileUpload(arrayBuffer);
                } else {
                    // Otherwise, the already existing file has been attached to another entity
                    // Don't want to overwrite it, so we'll change the file name
                    fileName = counterpartyName + " " + fileName;
                    ExecuteFileUpload(arrayBuffer);
                }
            }
                // Otherwise, there wasn't a file in the destination library. Ok to create as-is
            else {
                ExecuteFileUpload(arrayBuffer);
            }
        }).fail(function (status) {
            displayError(status);
        });
    }).fail(function (status) {
        displayError(status);
    });

    // Get the local file as an array buffer.
    function getFileBuffer() {
        var deferred = $.Deferred();
        var reader = new FileReader();
        reader.onloadend = function (e) {
            deferred.resolve(e.target.result);
        }
        reader.onerror = function (e) {
            deferred.reject(e.target.err);
        }
        reader.readAsArrayBuffer(fileInput[0].files[0]);
        return deferred.promise();
    }

    // Execute the file upload
    function ExecuteFileUpload(arrayBuffer) {
        addFileToFolder(arrayBuffer).done(function (file) {

            // Get the appropriate content type from the site
            GetContentTypeFromName(contentType).done(function (siteContentType) {

                // Update the list item with the new metadata
                var newMetadata = { "ContentTypeId": siteContentType.d.results[0].StringId };
                newMetadata[lookupField] = lookupFieldValue;
                newMetadata["CounterpartyName"] = counterpartyName;
                updateMetadataNoVersion(file.d.ServerRelativeUrl, newMetadata).complete(function () {
                    $("#savingdialog").dialog("close");
                    finalCallback();
                }).fail(function (status) {
                    displayError(status);
                });
            }).fail(function (status) {
                displayError(status);
            });
        }).fail(function (status) {
            displayError(status);
        });
    }

    // Check to see if file already exists
    function CheckFileExists(fileName) {
        var endpointUrl = appWebUrl + "/_api/web/lists/getbytitle('Commutation Documents')/rootfolder/files?$filter=Name eq '" + fileName + "'&$expand=ListItemAllFields,ListItemAllFields/ContentType";
        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    }

    // Add the file to the file collection in the Commutation Documents folder.
    function addFileToFolder(arrayBuffer) {

        // Construct the endpoint.
        var fileCollectionEndpoint = String.format(
            "{0}/_api/web/getfolderbyserverrelativeurl('{1}')/files" +
            "/add(overwrite=true, url='{2}')",
            appWebUrl, serverRelativeUrlToFolder, fileName);

        // Send the request and return the response.
        // This call returns the SharePoint file.
        return $.ajax({
            url: fileCollectionEndpoint,
            type: "POST",
            data: arrayBuffer,
            processData: false,
            headers: {
                "accept": "application/json;odata=verbose",
                "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                "content-length": arrayBuffer.byteLength
            }
        });
    }

    // Get a content type from a content type name
    function GetContentTypeFromName(contentTypeName) {
        var endpointUrl = appWebUrl + "/_api/web/ContentTypes?$filter=Name eq '" + contentTypeName + "'";
        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    }

    // Update the item metadata
    function updateMetadataNoVersion(fileUrl, newItemMetadata) {
        var endpointUrl = appWebUrl + "/_api/web/getfilebyserverrelativeurl('" + fileUrl + "')/listitemallfields/validateupdatelistitem";
        var data = {
            'formValues': [],
            'bNewDocumentUpdate': true,
            'checkInComment': ''
        };
        $.each(newItemMetadata, function (key, value) {
            var field = {
                '__metadata': { 'type': 'SP.ListItemFormUpdateValue' },
                'FieldName': key,
                'FieldValue': value
            };
            data.formValues.push(field);
        });
        return $.ajax({
            url: endpointUrl,
            type: "POST",
            data: JSON.stringify(data),
            headers: {
                "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                "content-type": "application/json;odata=verbose",
                "accept": "application/json;odata=verbose"
            }
        });
    }
}

function UploadNewItemDocuments(tempFiles, tempFileNames, sender, lookupField, lookupFieldValue, contentType, counterpartyName, finalCallback) {
    // Display the saving dialog
    $("#savingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: sender },
        modal: true,
        resizable: false,
        height: 120
    });

    $.each(tempFiles, function (i, tempFile) {

        // Define the foler path for the library.
        var serverRelativeUrlToFolder = 'Lists/Commutation Documents';

        // Get values from the file input page control.
        var fileName = tempFileNames[i];

        // Check if the file exists. If it does, we need to figure out how to handle the versioning
        CheckFileExists(fileName).done(function (fileCheckResults) {

            if (fileCheckResults.d.results.length > 0) {

                // See if the existing file was already added to this item
                if (fileCheckResults.d.results[0].ListItemAllFields[lookupField + "Id"] == lookupFieldValue) {

                    // If this file was already added to this item, we still want to upload and overwrite the version
                    ExecuteFileUpload(tempFile, (i === tempFiles.length - 1));
                }
                else {

                    // Otherwise, the already existing file has been attached to another entity
                    // Don't want to overwrite it, so we'll change the file name
                    fileName = counterpartyName + " " + fileName;
                    ExecuteFileUpload(tempFile, i === tempFiles.length - 1);
                }
            }
                // Otherwise, there wasn't a file in the destination library. Ok to create as-is
            else {
                ExecuteFileUpload(tempFile, i === tempFiles.length - 1);
            }

        }).fail(function (status) {
            displayError(status);
        });

        // Check to see if file already exists
        function CheckFileExists(fileName) {
            var endpointUrl = appWebUrl + "/_api/web/lists/getbytitle('Commutation Documents')/rootfolder/files?$filter=Name eq '" + fileName + "'&$expand=ListItemAllFields,ListItemAllFields/ContentType";
            return $.ajax({
                url: endpointUrl,
                type: "GET",
                headers: { "accept": "application/json;odata=verbose" }
            });
        }

        // Execute the file upload
        function ExecuteFileUpload(arrayBuffer, isLastItem) {
            addFileToFolder(arrayBuffer).done(function (file) {

                // Get the appropriate content type from the site
                GetContentTypeFromName(contentType).done(function (siteContentType) {

                    // Update the list item with the new metadata
                    var newMetadata = { "ContentTypeId": siteContentType.d.results[0].StringId };
                    newMetadata[lookupField] = lookupFieldValue;
                    newMetadata["CounterpartyName"] = counterpartyName;
                    updateMetadataNoVersion(file.d.ServerRelativeUrl, newMetadata).complete(function () {
                        if (isLastItem) {
                            $("#savingdialog").dialog("close");
                            finalCallback();
                        }
                    }).fail(function (status) {
                        displayError(status);
                    });
                }).fail(function (status) {
                    displayError(status);
                });
            }).fail(function (status) {
                displayError(status);
            });
        }

        // Add the file to the file collection in the Commutation Documents folder.
        function addFileToFolder(arrayBuffer) {

            // Construct the endpoint.
            var fileCollectionEndpoint = String.format(
                "{0}/_api/web/getfolderbyserverrelativeurl('{1}')/files" +
                "/add(overwrite=true, url='{2}')",
                appWebUrl, serverRelativeUrlToFolder, fileName);

            // Send the request and return the response.
            // This call returns the SharePoint file.
            return $.ajax({
                url: fileCollectionEndpoint,
                type: "POST",
                data: arrayBuffer,
                processData: false,
                headers: {
                    "accept": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                    "content-length": arrayBuffer.byteLength
                }
            });
        }

        // Get a content type from a content type name
        function GetContentTypeFromName(contentTypeName) {
            var endpointUrl = appWebUrl + "/_api/web/ContentTypes?$filter=Name eq '" + contentTypeName + "'";
            return $.ajax({
                url: endpointUrl,
                type: "GET",
                headers: { "accept": "application/json;odata=verbose" }
            });
        }

        // Update the item metadata
        function updateMetadataNoVersion(fileUrl, newItemMetadata) {
            var endpointUrl = appWebUrl + "/_api/web/getfilebyserverrelativeurl('" + fileUrl + "')/listitemallfields/validateupdatelistitem";
            var data = {
                'formValues': [],
                'bNewDocumentUpdate': true,
                'checkInComment': ''
            };
            $.each(newItemMetadata, function (key, value) {
                var field = {
                    '__metadata': { 'type': 'SP.ListItemFormUpdateValue' },
                    'FieldName': key,
                    'FieldValue': value
                };
                data.formValues.push(field);
            });
            return $.ajax({
                url: endpointUrl,
                type: "POST",
                data: JSON.stringify(data),
                headers: {
                    "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                    "content-type": "application/json;odata=verbose",
                    "accept": "application/json;odata=verbose"
                }
            });
        }
    });
}

// Display an error message (typically failed AJAX calls)
function displayError(data) {
    if ($("#loadingdialog").hasClass('ui-dialog-content')) {
        if ($("#loadingdialog").dialog("isOpen") === true) {
            $("#loadingdialog").dialog("close");
        }
    }
    if ($("#savingdialog").hasClass('ui-dialog-content')) {
        if ($("#savingdialog").dialog("isOpen") === true) {
            $("#savingdialog").dialog("close");
        }
    }
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
}