var appweburl;
var currentItemValues;

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
    var currentItem = GetCurrentItem();
    currentItem.done(function (item, status, xhr) {
        currentItemValues = item;
        $("#editActivityTitle").text("Activity for " + item.d.Project.Title);
        $("#createdBy").text(item.d.Author.Title);
        if (item.d.EntryDate) {
            $("#entryDate").text(new Date(item.d.EntryDate).format("M/d/yyyy"));
        }
        if (item.d.InitialDueDate) {
            $("#initialDueDate").text(new Date(item.d.InitialDueDate).format("M/d/yyyy"));
        } else { $("#dueDate").change(function () { DueDateChanged(); }) }
        $("#modifiedBy").text(item.d.Editor.Title);
        if (item.d.Modified) {
            $("#modifiedDate").text(new Date(item.d.Modified).format("M/d/yyyy h:mm tt"));
        }
        $("#description").val(item.d.Title);
        var getLookup = GetLookups("Activity Category Lookup", "Title", "asc");
        getLookup.done(function (activityCategories, status, xhr) {
            $.each(activityCategories.d.results, function (i, category) {
                $("#category").append("<option value='" + category.Id + "'>" + category.Title + "</option>");
            });
            $("#category").val(item.d.ActivityCategoryId);
        });
        getLookup.fail(function (status, xhr) {
            displayActivityError(status);
        });
        SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=assignedTo_TopSpan]")[0].id].AddUserKeys(item.d.AssignedTo.Name);
        getLookup = GetLocalChoiceLookups("Activity", "Priority");
        getLookup.done(function (activityPriorities, status, xhr) {
            $.each(activityPriorities.d.Choices.results, function (i, priority) {
                $("#priority").append("<option value='" + priority + "'>" + priority + "</option>");
            });
            $("#priority").val(item.d.Priority);
        });
        $(".datefield").datepicker({
            showOn: "button",
            buttonImage: "/_layouts/15/images/calendar.gif?rev=BEi%2Ba5GlgOA8I9XAdI4TNr33AtzDCF139o2jDGxr%2FMfwuH5n83E2%2BBqWzQKN18YeVJNwoYmtyrcI9V0uHNKvFg%3D%3D",
            buttonImageOnly: true,
            buttonText: "Select Date",
            autoSize: true
        });
        if (item.d.TaskDueDate) {
            $("#dueDate").val(new Date(item.d.TaskDueDate).format("M/d/yyyy"));
        }
        getLookup = GetLocalChoiceLookups("Activity", "Activity Status");
        getLookup.done(function (activityStatuses, status, xhr) {
            $.each(activityStatuses.d.Choices.results, function (i, activityStatus) {
                $("#status").append("<option value='" + activityStatus + "'>" + activityStatus + "</option>");
            });
            $("#status").val(item.d.ActivityStatus);
            if (item.d.ActivityStatus === "Dropped") {
                $("#droppedReason").closest('tr').removeClass('hiddenContentCell').addClass('contentCell');
            }
            $("#status").change(function () { StatusChanged(); });
        });
        getLookup.fail(function (status, xhr) {
            displayActivityError(status);
        });
        if (item.d.ActivityStatusChangeDate) {
            $("#statusChangedDate").text(new Date(item.d.ActivityStatusChangeDate).format("M/d/yyyy"));
        }
        GetLookups("Activity Dropped Reason Lookup", "Title", "asc").done(function (droppedReasons) {
            $.each(droppedReasons.d.results, function (i, droppedReason) {
                $("#droppedReason").append("<option value='" + droppedReason.Id + "'>" + droppedReason.Title + "</option>");
            });
            $("#droppedReason").val(item.d.ActivityDroppedReason.ID);
        }).fail(function (status) {
            displayActivityError(status);
        });
        //$("#droppedReason").text(item.d.ActivityDroppedReason.Title);
        var getActivityDocs = GetActivityDocuments();
        getActivityDocs.done(function (activityDocuments, status, xhr) {
            var dataTable = $("#activityDocuments").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc']
            });
            FillActivityDocumentsTable(activityDocuments.d.results);
        });
        getActivityDocs.fail(function (status, xhr) {
            displayActivityError(status);
        });
        if ($("#loadingdialog").dialog("isOpen") === true) {
            $("#loadingdialog").dialog("close");
        }
    });
    currentItem.fail(function (status, xhr) {
        displayActivityError(status);
    });
}

// Initialize the form validator
function InitializeValidation() {
    $("#aspnetForm").validate({
        submitHandler: function (form) {
            SubmitForm();
        },
        rules: {
            droppedReason: {
                min: function (element) {
                    if ($("#status").val() === "Dropped") { return 2; }
                    else { return 1; }
                }
            }
        },
        messages: {
            droppedReason: {
                min: "Please select a dropped reason"
            }
        }
    });
}

//===============================
// End Initialization
//===============================

//===============================
// Field Updates
//===============================

// Handle the due date field change
function DueDateChanged() {
    $("#initialDueDate").text(new Date($("#dueDate").val()).format("M/d/yyyy"));
}

// Handle the status field change
function StatusChanged() {
    $("#statusChangedDate").text(new Date().format("M/d/yyyy"));
    if ($("#status").val() === "Dropped") {
        $("#droppedReason").closest('tr').removeClass('hiddenContentCell').addClass('contentCell');
    } else {
        $("#droppedReason").closest('tr').removeClass('contentCell').addClass('hiddenContentCell');
        $("#droppedReason").val(0);
    }
}

// Handle a file upload field change
function FileUploadFieldChanged(contentType, sender, lookupField) {
    uploadDocument($("#" + sender), contentType, lookupField, getQueryStringParameter("ID"), currentItemValues.d.Project.Title, RenderActivityDocuments);
    var control = $("#" + sender);
    control.replaceWith(control = control.clone(true));
}

//===============================
// End Field Updates
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
                '__metadata': currentItemValues.d.__metadata,
                'Title': theForm.description.value,
                'ActivityCategoryId': theForm.category.value,
                'AssignedToId': user.d.Id,
                'Priority': $("#priority option:selected").text(),
                'TaskDueDate': theForm.dueDate.value.length > 0 ? new Date(theForm.dueDate.value) : null,
                'ActivityStatus': $("#status option:selected").text(),
                'ActivityStatusChangeDate': new Date($("#statusChangedDate").text()),
                'ActivityDroppedReasonId': $("#droppedReason option:selected").val(),
                'InitialDueDate': $("#initialDueDate").text().length > 0 ? new Date($("#initialDueDate").text()) : null
            };

            $.ajax({
                url: itemData.__metadata.uri,
                type: "POST",
                contentType: "application/json;odata=verbose",
                data: JSON.stringify(itemData),
                headers: {
                    "Accept": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                    "X-HTTP-Method": "MERGE",
                    "If-Match": itemData.__metadata.etag
                },
                error: function (data) {
                    displayActivityError(data);
                },
                complete: function (jqXHR, textStatus) {
                    $("#savingdialog").dialog("close");
                    if (textStatus == "success") {
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
                }
            });
        }
    });
    getAssignedTo.fail(function (status, xhr) {
        displayActivityError(status);
    });
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
function displayActivityError(data) {
    if ($("#loadingdialog").dialog("isOpen") === true) {
        $("#loadingdialog").dialog("close");
    }
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
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

// Get the currently opened item
function GetCurrentItem() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Activity')/items(" + getQueryStringParameter("ID") + ")?$select=*,Project/Title,Author/Title,Editor/Title,AssignedTo/Name,ActivityDroppedReason/Title,ActivityDroppedReason/ID&$expand=Project,Author,Editor,AssignedTo,ActivityDroppedReason";

    return $.ajax({
        url: endpointUrl,
        method: "GET",
        headers: { "Accept": "application/json;odata=verbose" }
    });
}

// Render the activity documents
function RenderActivityDocuments() {
    GetActivityDocuments().done(function (results) {
        FillActivityDocumentsTable(results.d.results);
    }).fail(function (status) {
        displayError(status);
    });
}

// Get activity documents
function GetActivityDocuments() {
    // Endpoint URL must be constructed differently for lookup columns
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Commutation Documents')/Items/?$filter=Activity/Id eq " + getQueryStringParameter("ID") + "&$expand=File/ModifiedBy";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the project documents table
function FillActivityDocumentsTable(results) {
    var dataTable = $("#activityDocuments").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteActivityDocument(\"" + itemUrl + "\"," + itemEtag + ", true, \"Activity Document\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            "<a style='color:blue' href='" + item.File.ServerRelativeUrl + "'>" + item.File.Name + "</a>",
            item.OData__dlc_DocId,
            new Date(item.Modified).format('MM/dd/yyyy hh:mm tt'),
            item.File.ModifiedBy.Title,
            item.File.MajorVersion + "." + item.File.MinorVersion
        ]).draw();
    });
    ResizeDialogWindow();
}

// Resize the dialog window
function ResizeDialogWindow() {
    var dlg = SP.UI.ModalDialog.get_childDialog();
    if (dlg != null) {
        dlg.autoSize();
    }
}

// Escape a string to be HTML compliant
function escapeHtml(text) {
    var map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };

    return text.replace(/[&<>"']/g, function (m) { return map[m]; });
}

// Deletes a list item
function DeleteActivityDocument(itemUrl, itemEtag, renderClient, contentType) {
    if (confirm('Are you sure you want to delete this ' + contentType + '?')) {
        var deleteItem = ExecuteDelete();
        deleteItem.done(function (something, status, xhr) {
            if (renderClient) {
                switch (contentType) {
                    case "Activity Document":
                        RenderActivityDocuments();
                        break;
                }
            }
        });
        deleteItem.fail(function (status, xhr) {
            displayActivityError(status);
        });
    }

    function ExecuteDelete() {
        var endpointUrl = decodeURIComponent(itemUrl);
        return $.ajax({
            url: endpointUrl,
            type: "POST",
            headers: {
                "Accept": "application/json;odata=verbose",
                "X-Http-Method": "DELETE",
                "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                "If-Match": "*"
            }
        });
    }
}

//===============================
// End Utilities
//===============================