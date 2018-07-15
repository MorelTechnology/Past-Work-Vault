var appweburl;
var counterpartyName;
var tempFiles = [];
var tempFileNames = [];

// When the DOM is ready, begin code execution
$(document).ready(function () {
    InitializeGlobalVars();
    InitializeFields();
    InitializeValidation();
});

//===============================
// Initialization
//===============================

// Show the loading animation
$(function () {
    $("#loadingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#newItemContent") },
        modal: true,
        resizable: false
    });
});

// Initialize any required global variables
function InitializeGlobalVars() {
    //appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
    appweburl = _spPageContextInfo.webServerRelativeUrl;
}

// Initialize any fields that aren't automatically built in the DOM
function InitializeFields() {
    GetLookups("Projects", "Title", "asc").done(function (projects) {
        $("#project").append("<option value=''></option>");
        $.each(projects.d.results, function (i, project) {
            $("#project").append("<option value='" + project.Id + "'>" + project.Title + "</option>");
        });
    }).fail(function (status) {
        displayError(status);
    });
    var getLookup = GetLookups("Note Entry Type Lookup", "Title", "asc");
    getLookup.done(function (entryTypes, status, xhr) {
        $.each(entryTypes.d.results, function (i, entryType) {
            $("#noteEntryType").append("<option value='" + entryType.Id + "'>" + entryType.Title + "</option>");
        });
        $("#loadingdialog").dialog("close");
    });
    getLookup.fail(function (status, xhr) {
        displayNoteError(status);
    });
    $(".datefield").datepicker({
        showOn: "button",
        buttonImage: "/_layouts/15/images/calendar.gif?rev=BEi%2Ba5GlgOA8I9XAdI4TNr33AtzDCF139o2jDGxr%2FMfwuH5n83E2%2BBqWzQKN18YeVJNwoYmtyrcI9V0uHNKvFg%3D%3D",
        buttonImageOnly: true,
        buttonText: "Select Date",
        autoSize: true
    });
    $("#content1").jqte();
    var dataTable = $("#newItemDocuments").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { "searchable": false, "targets": 0 },
            { "orderable": false, "targets": 0 }
        ],
        "order": [1, 'asc']
    });
}

// Initialize the form validator
function InitializeValidation() {
    $("#aspnetForm").validate({
        submitHandler: function (form) {
            SubmitForm();
        },
        errorPlacement: function (error, element) {
            if (element.attr("id").indexOf("entryDate") >= 0) {
                error.insertAfter("#entryDateContainer");
            }
        }
    });
}

//===============================
// End Initialization
//===============================

//===============================
// Form Submit
//===============================

function SubmitForm() {
    if ($("#content1").val().length === 0) {
        displayNoteError("Please enter Note contents");
        return;
    }
    if ($("#aspnetForm").valid()) {
        $("#savingdialog").dialog({
            dialogClass: "no-close",
            position: { my: "center", at: "center", of: $("#newItemContent") },
            modal: true,
            resizable: false,
            height: 120
        });

        var theForm = $("#aspnetForm")[0];
        var itemData = {
            '__metadata': { 'type': 'SP.Data.NotesListItem' },
            'Title': 'No Title',
            'ProjectId': $("#project option:selected").val(),
            'EntryTypeId': theForm.noteEntryType.value,
            'EntryDate': new Date(theForm.entryDate.value),
            'Content1': $("#content1").val()
        }

        $.ajax({
            url: appweburl + "/_api/web/lists/getbytitle('Notes')/items",
            type: "POST",
            contentType: "application/json;odata=verbose",
            data: JSON.stringify(itemData),
            headers: {
                "Accept": "application/json;odata=verbose",
                "X-RequestDigest": $("#__REQUESTDIGEST").val()
            },
            error: function (data) {
                displayNoteError(data);
            },
            complete: function (jqXHR, textStatus) {
                if (textStatus == "success") {
                    if (tempFiles.length > 0) {
                        var itemId = $.parseJSON(jqXHR.responseText).d.Id;
                        UploadTempFiles("Note", itemId.toString(), "Note Document");
                    } else {
                        $("#savingDialog").dialog("close");
                        CloseForm();
                    }
                }
            }
        });
    }
}

// Upload any files that may have been attached
function UploadTempFiles(lookupField, lookupFieldValue, contentType) {
    UploadNewItemDocuments(tempFiles, tempFileNames, $("#aspnetForm"), lookupField, lookupFieldValue, contentType, $("#project option:selected").text(), CloseForm);
}

// Cancel the submit
function CancelSubmit() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(0, 0);
    }
    else {
        if (getQueryStringParameter("Source")) {
            var sourceParam = "Source=" + getQueryStringParameter("Source") + "&";
            window.location = getQueryStringParameter("Source") + "?" + decodeURIComponent(document.URL.split("?")[1].replace(sourceParam, ""));
        } else {
            window.location = appweburl;
        }
    }
}

//===============================
// End Form Submit
//===============================

//===============================
// Utilities
//===============================

// Return a query string parameter
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

// Display an error message (typically failed AJAX calls)
function displayNoteError(data) {
    if ($("#loadingdialog").dialog("isOpen") === true) {
        $("#loadingdialog").dialog("close");
    }
    var responseText = $.parseJSON(data.responseText);
    var error;
    if (responseText) { error = responseText.error.message.value; }
    else { error = data; }
    $("#errorCell").css('visibility', 'visible').html("<h4>" + error + "</h4").parent("tr").show();
    alert(error);
}


// Get a specific list item
function GetSpecificItem(listName, itemId) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('" + listName + "')/items(" + itemId + ")";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Get all the items in a lookup list
function GetLookups(listName, orderBy, direction, top) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('" + listName + "')/items?";
    if (orderBy && direction) { endpointUrl += "&$OrderBy=" + orderBy + " " + direction; }
    if (top) { endpointUrl += "&$top=" + top; }
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Get the choices of a choice column local to a SP List
function GetLocalChoiceLookups(listName, columnName) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('" + listName + "')/fields/getbytitle('" + columnName + "')/choices";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Gets a user object from the SharePoint site using a login name
function GetUserFromLogin(loginName) {
    var siteUrl = appweburl + "/_api/web/siteusers(@v)?@v='" + encodeURIComponent(loginName) + "'";

    return $.ajax({
        url: siteUrl,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" }
    });
}

// Handle a file upload field change
function FileUploadFieldChanged(contentType, sender) {
    var fileInput = $("#" + sender);
    var getFile = getFileBuffer();
    getFile.done(function (arrayBuffer) {
        tempFiles.push(arrayBuffer);

        // Get the file name from the file input control on the page.
        var parts = fileInput[0].value.split('\\');
        var fileName = parts[parts.length - 1];
        tempFileNames.push(fileName);
        FillTempFileDataTable();

        fileInput.replaceWith(fileInput = fileInput.clone(true));
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
}

// Fill the temp file datatable
function FillTempFileDataTable() {
    var dataTable = $("#newItemDocuments").DataTable();
    dataTable.clear().draw();
    $.each(tempFileNames, function (i, fileName) {
        dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onclick='RemoveTempFile(" + i + ")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            fileName
        ]).draw();
    });
}

// Remove a temp file from the array
function RemoveTempFile(id) {
    tempFiles.splice(id, 1);
    tempFileNames.splice(id, 1);
    FillTempFileDataTable();
}

// Close the form
function CloseForm() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(1, 1);
    }
    else {
        if (getQueryStringParameter("Source")) {
            var sourceParam = "Source=" + getQueryStringParameter("Source") + "&";
            window.location = getQueryStringParameter("Source") + "?" + decodeURIComponent(document.URL.split("?")[1].replace(sourceParam, ""));
        } else {
            window.location = appweburl;
        }
    }
}

//===============================
// End Utilities
//===============================