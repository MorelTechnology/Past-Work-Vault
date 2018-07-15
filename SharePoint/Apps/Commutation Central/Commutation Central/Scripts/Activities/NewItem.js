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
    var getCurrentProject = GetSpecificItem("Projects", getQueryStringParameter("ProjectId"));
    getCurrentProject.done(function (project, status, xhr) {
        counterpartyName = project.d.Title;
        $("#addActivityTitle").text("Add Activity for " + project.d.Title);
    });
    getCurrentProject.fail(function (status, xhr) {
        displayError(status);
    });
    var getLookup = GetLookups("Activity Category Lookup", "Title", "asc");
    getLookup.done(function (activityCategories, status, xhr) {
        $.each(activityCategories.d.results, function (i, category) {
            $("#category").append("<option value='" + category.Id + "'>" + category.Title + "</option>");
        });
    });
    getLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getLookup = GetLocalChoiceLookups("Activity", "Priority");
    getLookup.done(function (priorities, status, xhr) {
        $.each(priorities.d.Choices.results, function (i, priority) {
            $("#priority").append("<option value='" + i + "'>" + priority + "</option>");
        });
        $("#loadingdialog").dialog("close");
    });
    getLookup.fail(function (status, xhr) {
        displayError(status);
    });
    $(".datefield").datepicker({
        showOn: "button",
        buttonImage: "/_layouts/15/images/calendar.gif?rev=BEi%2Ba5GlgOA8I9XAdI4TNr33AtzDCF139o2jDGxr%2FMfwuH5n83E2%2BBqWzQKN18YeVJNwoYmtyrcI9V0uHNKvFg%3D%3D",
        buttonImageOnly: true,
        buttonText: "Select Date",
        autoSize: true
    });
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
    var assignedToPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=assignedTo_TopSpan]")[0].id];
    if (assignedToPP.GetAllUserInfo().length < 1) {
        assignedToPP.ShowErrorMessage("Assigned to is required");
        return;
    } else { assignedToPP.ShowErrorMessage(""); }
    var getAssignedTo = GetUserFromLogin(assignedToPP.GetAllUserInfo()[0].Key);
    getAssignedTo.done(function (user, status, xhr) {
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
                '__metadata': { 'type': 'SP.Data.ActivityListItem' },
                'Title': theForm.description.value,
                'ProjectId': getQueryStringParameter("ProjectId"),
                'ActivityCategoryId': theForm.category.value,
                'EntryDate': new Date(),
                'AssignedToId': user.d.Id,
                'Priority': $("#priority option:selected").text(),
                'TaskDueDate': theForm.dueDate.value.length > 0 ? new Date(theForm.dueDate.value) : null,
                'ActivityStatusChangeDate': new Date(),
                'ActivityDroppedReasonId': 1,
                'InitialDueDate': theForm.dueDate.value.length > 0 ? new Date(theForm.dueDate.value) : null
            };

            $.ajax({
                url: appweburl + "/_api/web/lists/getbytitle('Activity')/items?$select=*,Project/Title",
                type: "POST",
                contentType: "application/json;odata=verbose",
                data: JSON.stringify(itemData),
                headers: {
                    "Accept": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val()
                },
                error: function (data) {
                    displayError(data);
                },
                complete: function (jqXHR, textStatus) {
                    if (textStatus == "success") {
                        if (tempFiles.length > 0) {
                            var itemId = $.parseJSON(jqXHR.responseText).d.Id;
                            UploadTempFiles("Activity", itemId.toString(), "Activity Document");
                        } else {
                            $("#savingdialog").dialog("close");
                            CloseForm();
                        }
                    }
                }
            });
        }
    });
    getAssignedTo.fail(function (status, xhr) {
        displayError(status);
    });
}

// Upload any files that may have been attached
function UploadTempFiles(lookupField, lookupFieldValue, contentType) {
    UploadNewItemDocuments(tempFiles, tempFileNames, $("#aspnetForm"), lookupField, lookupFieldValue, contentType, counterpartyName, CloseForm);
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
function displayError(data) {
    if ($("#loadingdialog").dialog("isOpen") === true) {
        $("#loadingdialog").dialog("close");
    }
    $("#errorCell").css('visibility', 'visible').html("<h4>" + data.responseText + "</h4").parent("tr").show();
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