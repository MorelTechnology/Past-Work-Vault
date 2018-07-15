var appweburl;
var tempFiles = [];
var tempFileNames = [];

// When the DOM is ready, begin code execution
$(document).ready(function () {
    InitializeGlobalVars();
    InitializeFields();
});

// Prevent the default submit actions from happening. No server-side code allowed!
$(document).submit(function (event) {
    event.preventDefault();
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
        GetPermissionGroup("Commutations Requestors").done(function (group) {
            var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='requestorId_TopSpan']")[0].id];
            spPP.SharePointGroupID = group.d.Id;
        }).fail(function (status) {
            displayError(status);
        });

    var finalAjaxLoading = InitializeDropDownList("requestType", "Request Type Lookup", "ID", "Title");
    InitializeDateField("requestDate");
    InitializeValidation();

    GetPermissionGroup("Commutation Managers").done(function (group) {
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='primaryManagerId_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
    }).fail(function (status) {
        displayError(status);
    });

    // Lock down the primary manager field
    GetPermissionGroup("Super Managers", true).done(function (groupData) {
        if (groupData.d.results.length > 0) {
            var userIsSuperManager = false;
            $.each(groupData.d.results, function (i, user) {
                if (user.Id == _spPageContextInfo.userId) { userIsSuperManager = true; }
            });
            if (!userIsSuperManager) { SetPeoplePickerFieldReadOnly('primaryManagerId_TopSpan', 'Primary Manager'); }
        } else { SetPeoplePickerFieldReadOnly('primaryManagerId_TopSpan', 'Primary Manager'); }
    }).fail(function (status) {
        displayError(status);
        SetPeoplePickerFieldReadOnly('primaryManagerId_TopSpan', 'Primary Manager');
    });
    $("[id$=btnSubmit]").click(function (event) {
        ValidateEntries();
    });
    finalAjaxLoading.done(function () {
        $("#loadingdialog").dialog("close");
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
        groups: {
            requestDate: "requestDate"
        },
        errorPlacement: function (error, element) {
            if (element.attr("id").indexOf("requestDate") >= 0) {
                error.insertAfter("#requestDateContainer");
            }
            else {
                error.insertAfter(element);
            }
        }
    });
}

// Initialize a dropdown list
function InitializeDropDownList(controlId, getValuesFrom, valueColumn, descriptionColumn) {
    var url = appweburl + "/_api/web/lists/getbytitle('" + getValuesFrom + "')/items";
    return $.ajax({
        url: url,
        method: "GET",
        headers: {
            "ACCEPT": "application/json;odata=verbose"
        },
        success: function (data) {
            PopulateDropDownList(controlId, data.d.results, valueColumn, descriptionColumn);
        },
        error: function (data) {
            displayError(data);
        }
    });
}

// Populate a dropdown list
function PopulateDropDownList(controlId, data, valueColumn, descriptionColumn) {
    for (var i = 0; i < data.length; i++) {
        var IDTemp = data[i][valueColumn];
        $("[id$=" + controlId + "]").append("<option value='" + IDTemp + "'>" +
            data[i][descriptionColumn] + "</option>");
    }
}

// Initialize a date field
function InitializeDateField(controlId) {
    $("[id$=" + controlId + "]").datepicker({
        showOn: "button",
        buttonImage: "/_layouts/15/images/calendar.gif?rev=BEi%2Ba5GlgOA8I9XAdI4TNr33AtzDCF139o2jDGxr%2FMfwuH5n83E2%2BBqWzQKN18YeVJNwoYmtyrcI9V0uHNKvFg%3D%3D",
        buttonImageOnly: true,
        buttonText: "Select Date",
        autoSize: true
    });
}


// Set a people picker field to read only
function SetPeoplePickerFieldReadOnly(controlId, controlTitle) {
    var ppDiv = $("[id$=" + controlId + "]");
    var ppEditor = ppDiv.find("[title='" + controlTitle + "']");
    var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv[0].id];
    spPP.SetEnabledState(false);
    $('.sp-peoplepicker-delImage').css('display', 'none');
}

//===============================
// End Initialization
//===============================

//===============================
// Form Submit
//===============================

// Validate any fields not controlled through ASP field validators
function ValidateEntries() {
    // Check to make sure the requestor field is populated and a user is resolved
    var requestorPeoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=requestorId_TopSpan]")[0].id];
    if (requestorPeoplePicker.GetAllUserInfo().length < 1) {
        requestorPeoplePicker.ShowErrorMessage("Requestor ID is required");
        return;
    }

    // Check to make sure the primary manager field is populated and a user is resolved
    var primaryManagerPeoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=primaryManagerId_TopSpan]")[0].id];
    if (primaryManagerPeoplePicker.GetAllUserInfo().length < 1) {
        primaryManagerPeoplePicker.ShowErrorMessage("Primary Manager ID is required");
        return;
    }

    // Verify that the remaining fields are valid
    if ($("[id$=counterpartyName]").valid() && $("[id$=requestType]").valid() && $("[id$=requestDate]").valid()) {
        var requestorId, counterpartyName, primaryManagerId, requestTypeId, requestDate;

        // Get the numerical ID of the user in the requestor field
        var getRequestorUser = GetUserFromLogin(requestorPeoplePicker.GetAllUserInfo()[0].Key);
        getRequestorUser.done(function (user, status, xhr) {
            requestorId = user.d.Id;

            //Get the numerical ID of the user in the primary manager field
            var getManagerUser = GetUserFromLogin(primaryManagerPeoplePicker.GetAllUserInfo()[0].Key);
            getManagerUser.done(function (user, status, xhr) {
                primaryManagerId = user.d.Id;

                // Get the remaining fields
                counterpartyName = $("[id$=counterpartyName]").val();
                requestTypeId = $("[id$=requestType]").val();
                requestDate = $("[id$=requestDate]").datepicker("getDate");

                // Submit the project
                SubmitProject(requestorId, counterpartyName, primaryManagerId, requestTypeId, requestDate);
            });
            getManagerUser.fail(function (status, xhr) {
                displayError(status);
                return;
            });
        });
        getRequestorUser.fail(function (status, xhr) {
            displayError(status);
            return;
        });
    }
    else {
        return;
    }
}

function SubmitProject(requestorId, counterpartyName, primaryManagerId, requestTypeId, requestDate) {
    $("#savingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#newItemContent") },
        modal: true,
        resizable: false,
        height: 120
    });

    var metadata = {
        '__metadata': { 'type': 'SP.Data.ProjectsListItem' },
        'Title': counterpartyName,
        'PrimaryManagerIDId': primaryManagerId,
        'RequestorIDId': requestorId,
        'RequestTypeId': requestTypeId,
        'RequestDate': requestDate,
        'CommutationStatusId': '1',
        'DroppedReasonId': '1',
        'LeadOfficeId': '1',
        'CommutationTypeId': '1',
        'DealPriorityId': '2',
        'AssignedDate': new Date()
    };

    $.ajax({
        url: appweburl + "/_api/web/lists/getbytitle('Projects')/items",
        type: "POST",
        contentType: "application/json;odata=verbose",
        data: JSON.stringify(metadata),
        headers: {
            "Accept": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val()
        },
        error: function (data) {
            displayError(data);
        },
        complete: function (jqXHR, textStatus) {
            $("#savingdialog").dialog("close");
            SetupDealCheckList(jqXHR);
        }
    });
}

function SetupDealCheckList(newItem) {
    $("#creatingCheckList").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#newItemContent") },
        modal: true,
        resizable: false,
        height: 180,
        width: 360
    });
    newItem = $.parseJSON(newItem.responseText);
    var projectId = newItem.d.Id;
    var dealMemo, writtenApproval, releaseAgreement, confirmWireTransfer, postCommProForma, soxBinderScannedFiled, regulatoryApproval, dealChecklist, collateralReleases;
    dealMemo = CreateDealCheckListItem(projectId, "Deal Memo", 1);
    writtenApproval = CreateDealCheckListItem(projectId, "Written Approval", 2);
    releaseAgreement = CreateDealCheckListItem(projectId, "Release Agreement", 3);
    confirmWireTransfer = CreateDealCheckListItem(projectId, "Confirm Wire Transfer", 4);
    postCommProForma = CreateDealCheckListItem(projectId, "Post Comm Pro Forma", 5);
    soxBinderScannedFiled = CreateDealCheckListItem(projectId, "Sox Binder Scanned & Filed", 6);
    regulatoryApproval = CreateDealCheckListItem(projectId, "Regulatory Approval", 7);
    dealChecklist = CreateDealCheckListItem(projectId, "Deal Checklist", 8);
    collateralReleases = CreateDealCheckListItem(projectId, "Collateral Releases", 9);
    var executeCalls1 = (dealMemo, writtenApproval, releaseAgreement, confirmWireTransfer, postCommProForma, soxBinderScannedFiled, regulatoryApproval, dealChecklist, collateralReleases).complete(function (jqXHR, status) {
        $("#creatingCheckList").dialog("close");
        if (status == "success") {
            if (tempFiles.length > 0) {
                UploadTempFiles("Project", projectId.toString(), "Project Document");
            }
            else {
                CloseForm();
            }
        }
    });
    /*var executeCalls = $.when(dealMemo, writtenApproval, releaseAgreement, confirmWireTransfer, postCommProForma, soxBinderScannedFiled, regulatoryApproval, dealChecklist, collateralReleases).done(function (a1, a2, a3, a4, a5, a6, a7, a8, a9) {
        $("#creatingCheckList").dialog("close");
        if (a1[1], a2[1], a3[1], a4[1], a5[1], a6[1], a7[1], a8[1], a9[1] == "success") {
            if (tempFiles.length > 0) {
                UploadTempFiles("Project", projectId.toString(), "Project Document");
            }
            else {
                CloseForm();
            }
        }
    });
    executeCalls.fail(function (status, xhr) {
        displayError(status);
    });*/
}

function UploadTempFiles(lookupField, lookupFieldValue, contentType) {
    UploadNewItemDocuments(tempFiles, tempFileNames, $("#aspnetForm"), lookupField, lookupFieldValue, contentType, $("[id$=counterpartyName]").val(), CloseForm);
}

function CreateDealCheckListItem(projectId, CheckListItemTypeStr, CheckListItemTypeId) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Deal Check List')/items";
    var itemData = {
        '__metadata': { 'type': 'SP.Data.Deal_x0020_Check_x0020_ListListItem' },
        'Title': CheckListItemTypeStr + " for Project Id " + projectId,
        'ProjectId': projectId,
        'CheckListItemId': CheckListItemTypeId
    }

    return $.ajax({
        url: endpointUrl,
        type: "POST",
        contentType: "application/json;odata=verbose",
        data: JSON.stringify(itemData),
        headers: {
            "Accept": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val()
        }
    });
}

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
    $("#errorCell").css('visibility', 'visible').html("<h4>" + data.responseText + "</h4").parent("tr").show();
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

// Get a permission group
function GetPermissionGroup(groupName, getUsers) {
    var endpointUrl = appweburl + "/_api/web/sitegroups/getbyname('" + groupName + "')";
    getUsers ? endpointUrl += "/users" : "";
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