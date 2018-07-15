'use strict';

var appweburl;

// When the DOM is ready, begin code execution
$(document).ready(function () {
    ShowLoadDialog();
    InitializeGlobalVars();
    InitializeFields();
});

//===============================
// Initialization
//===============================

// Show the loading animation
function ShowLoadDialog() {
    $("#loadingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#aspnetForm") },
        modal: true,
        resizable: false
    });
}

// Hide the loading animation
function HideLoadDialog() {
    if ($("#loadingdialog").dialog("isOpen") === true) {
        $("#loadingdialog").dialog("close");
    }
}

// Initialize any required global variables
function InitializeGlobalVars() {
    appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
}

// Initialize any fields that aren't automatically built in the DOM
function InitializeFields() {
    $(".ms-siteicon-img").attr('src', decodeURIComponent(getQueryStringParameter("SPHostLogo")));

    var activitiesDataTable = $("#activities").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { "searchable": false, "targets": [0] },
            { "orderable": false, "targets": [0] },
            { "className": "tableCellNoWrap", "targets": [6, 7] }
        ],
        "order": [1, 'asc'],
        "rowCallback": function (row, data) {
            if (data[8] === "Active") {
                if (data[6]) {
                    var today = new Date();
                    var dueDate = new Date(data[6]);
                    var oneDay = 24 * 60 * 60 * 1000;
                    var diffDays = Math.ceil((dueDate.getTime() - today.getTime()) / oneDay);
                    var $tableCell = $("td:eq(6)", row);
                    if ($tableCell.find(".indicator").length === 0) {
                        if (diffDays >= 7) {
                            $tableCell.prepend("<img class='indicator' width='16' style='padding-right: 5px' alt='Upcoming' src='../../Images/green-circle.png'/>");
                        } else if (7 > diffDays && diffDays > 0) {
                            $tableCell.prepend("<img class='indicator' width='16' style='padding-right: 5px' alt='Upcoming' src='../../Images/yellow-triangle.jpg'/>");
                        } else if (diffDays <= 0) {
                            $tableCell.prepend("<img class='indicator' width='16' style='padding-right: 5px' alt='Upcoming' src='../../Images/stop-sign.png'/>");
                        }
                    }
                }
            }
        }
    });

    RenderActivities();
    $(document).ajaxComplete(function () {
        HideLoadDialog();
    });
}

//===============================
// End Initialization
//===============================

//===============================
// Field Updates
//===============================

//===============================
// End Field Updates
//===============================

//===============================
// Form Submit
//===============================

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
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
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

// Render the activities table
function RenderActivities(assignedTo) {
    GetActivities(assignedTo).done(function (results) {
        FillActivitiesTable(results.d.results);
    }).fail(function (status) {
        displayError(status);
    });
}

// Get activities
function GetActivities() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Activity')/Items?$select=*,Project/Title,AssignedTo/Title,Author/Title,ActivityCategory/Title&$expand=Project,AssignedTo,Author,ActivityCategory&$top=5000";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the activities table
function FillActivitiesTable(results) {
    var dataTable = $("#activities").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            "<a style='color:blue' href='javascript:void(0)' onClick=\"AttachProjectDocument('Activity Document', 'ActivityId', " + item.Id + ", '" + escapeHtml(item.Project.Title) + "')\"><img width='16' height='16' title='Attach Document' alt='AttachDocument' src='../../Images/document.png'/></a>",
            item.Title ? "<a style='color:blue' href='javascript:void(0)' onClick='EditActivity(" + item.Id + ")'>" + item.Title + "</a>" : "",
            item.Project.Title ? item.Project.Title : "",
            item.AssignedTo.Title ? item.AssignedTo.Title : "",
            item.Author.Title ? item.Author.Title : "",
            item.ActivityCategory.Title ? item.ActivityCategory.Title : "",
            item.TaskDueDate ? new Date(item.TaskDueDate).format("M/d/yyyy") : "",
            item.EntryDate ? new Date(item.EntryDate).format("M/d/yyyy") : "",
            item.ActivityStatus ? item.ActivityStatus : ""
        ]);
    });
    dataTable.draw();
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

// Open the activity in edit mode
function EditActivity(activityId) {
    var itemUrl = _spPageContextInfo.webServerRelativeUrl + "/Pages/Activities/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + activityId;
    var options = {
        url: itemUrl,
        title: "Edit Activity",
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}

// Open a modal dialog to attach a document to an entity
function AttachProjectDocument(contentType, idField, idVal, counterpartyName) {
    var pageUrl = _spPageContextInfo.webServerRelativeUrl + "/Pages/Documents/AttachDocument.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ContentType=" + contentType + "&" + idField + "=" + idVal + "&CounterpartyName=" + counterpartyName;

    var options = {
        url: pageUrl,
        title: "Attach Document for " + counterpartyName,
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}

//===============================
// End Utilities
//===============================