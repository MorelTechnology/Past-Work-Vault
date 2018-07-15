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

// Upload the file.
// You can upload files up to 2 GB with the REST API.
// Accepts the types 'Project Document', 'Activity Document', 'Note Document', 'Deal Check List Document'
function uploadFile(contentType, renderClient, sender) {
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
    var newTitle = "A test from edit";
    // Get the file name from the file input control on the page.
    var parts = fileInput[0].value.split('\\');
    var fileName = parts[parts.length - 1];

    // Initiate method calls using jQuery promises.
    // Get the local file as an array buffer.
    var getFile = getFileBuffer();
    getFile.done(function (arrayBuffer) {

        // Check if the file exists
        CheckFileExists(fileName).done(function (results) {
            if (results.d.results.length > 0) {
                alert('File already exists');
                return;
            }
            // Add the file to the SharePoint folder.
            var addFile = addFileToFolder(arrayBuffer);
            addFile.done(function (file, status, xhr) {

                // Get the list item that corresponds to the uploaded file.
                var getItem = getListItem(file.d.ListItemAllFields.__deferred.uri);
                getItem.done(function (listItem, status, xhr) {

                    // Update the list item with correct properties.
                    var newMetadata = new Object();
                    var getContentType = GetContentTypeFromName(contentType);
                    getContentType.done(function (siteContentType, status, xhr) {
                        newMetadata.ContentTypeId = siteContentType.d.results[0].StringId;
                        switch (contentType) {
                            case 'Project Document':
                                newMetadata.Project = getQueryStringParameter("ID");
                                break;
                            case 'Activity Document':
                                newMetadata.Activity = getQueryStringParameter("ID");
                                break;
                            case 'Note Document':
                                newMetadata.Note = getQueryStringParameter("ID");
                                break;
                            case 'Deal Check List Document':
                                newMetadata.DealCheckListItem = getQueryStringParameter("ID");
                                break;
                        }
                        var updateItem = updateMetadataNoVersion(file.d.ServerRelativeUrl, listItem.d.__metadata, newMetadata); //updateListItem(listItem.d.__metadata, newMetadata);
                        updateItem.done(function (data, status, xhr) {
                            $("#savingdialog").dialog("close");
                            if (renderClient) {
                                switch (contentType) {
                                    case 'Project Document':
                                        var getProjectDocs = GetProjectDocuments();
                                        getProjectDocs.done(function (projectDocuments, status, xhr) {
                                            FillProjectDocumentsTable(projectDocuments.d.results);
                                        });
                                        getProjectDocs.fail(function (status, xhr) {
                                            displayError(status);
                                        });
                                        break;
                                    case 'Activity Document':
                                        var getActivityDocs = GetActivityDocuments();
                                        getActivityDocs.done(function (activityDocuments, status, xhr) {
                                            FillActivityDocumentsTable(activityDocuments.d.results);
                                        });
                                        getActivityDocs.fail(function (status, xhr) {
                                            displayError(status);
                                        });
                                        break;
                                    case 'Note Document':
                                        var getNoteDocs = GetNoteDocuments();
                                        getNoteDocs.done(function (noteDocuments, status, xhr) {
                                            FillNoteDocumentsTable(noteDocuments.d.results);
                                        });
                                        getNoteDocs.fail(function (status, xhr) {
                                            displayError(status);
                                        });
                                        break;
                                    case 'Deal Check List Document':
                                        var getCheckListDocs = GetCheckListDocuments();
                                        getCheckListDocs.done(function (checkListDocuments, status, xhr) {
                                            FillCheckListDocumentsTable(checkListDocuments.d.results);
                                        });
                                        getCheckListDocs.fail(function (status, xhr) {
                                            displayError(status);
                                        });
                                        break;
                                }
                            }
                        });
                        updateItem.fail(function (status, xhr) {
                            displayError(status);
                        });
                    });
                    getContentType.fail(function (status, xhr) {
                        displayError(status);
                    });
                });
                getItem.fail(function (status, xhr) {
                    displayError(status);
                });
            });
            addFile.fail(function (status, xhr) {
                displayError(status);
            });
        }).fail(function (status) {
            displayError(status);
        });
    });
    getFile.fail(function (status, xhr) {
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

    // Get the list item that corresponds to the file by calling the file's ListItemAllFields property.
    function getListItem(fileListItemUri) {
        // Send the request and return the response.
        return $.ajax({
            url: fileListItemUri,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    }

    // Update the item metadata
    function updateMetadataNoVersion(fileUrl, originalItemMetadata, newItemMetadata) {
        var endpointUrl = appWebUrl + "/_api/web/lists/getbytitle('Commutation Documents')/rootfolder/files/getbyurl(url='" + fileUrl + "')/listitemallfields/validateupdatelistitem";
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

    // Get a content type from a content type name
    function GetContentTypeFromName(contentTypeName) {
        var endpointUrl = appWebUrl + "/_api/web/ContentTypes?$filter=Name eq '" + contentTypeName + "'";
        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
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
}

// Get parameters from the query string.
// For production purposes you may want to use a library to handle the query string.
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve) return singleParam[1];
    }
}

// Display an error message (typically failed AJAX calls)
function displayError(data) {
    if ($("#loadingdialog").dialog("isOpen") === true) {
        $("#loadingdialog").dialog("close");
    }
    if ($("#savingdialog").dialog("isOpen") === true) {
        $("#savingdialog").dialog("close");
    }
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
}