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

    accounting.settings.currency.format = {
        pos: "%s %v",   // for positive values, eg. "$ 1.00" (required)
        neg: "%s (%v)", // for negative values, eg. "$ (1.00)" [optional]
        zero: "%s  -- "  // for zero values, eg. "$  --" [optional]
    };

    var projectsDataTable = $("#projects").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { "searchable": false, "targets": [] },
            { "orderable": false, "targets": [] }
        ],
        "order": [0, 'asc'],
        "rowCallback": function (row, data) {
            if (accounting.unformat(data[7]) < 0) {
                $("td:eq(7)", row).css('color', 'red');
            }
        }
    });

    RenderProjects();
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

// Render the project table
function RenderProjects() {
    GetProjects().done(function (results) {
        FillProjectsTable(results.d.results);
    }).fail(function (status) {
        displayError(status);
    });
}

// Get projects
function GetProjects() {
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Projects')/Items?$select=*,PrimaryManagerID/Title,SecondaryManagerID/Title,LeadOffice/Title,CommutationStatus/Title&$expand=PrimaryManagerID,SecondaryManagerID,LeadOffice,CommutationStatus&$filter=CommutationStatus/Title eq 'Completed' or CommutationStatus/Title eq 'Dropped'&$top=5000";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the projects table
function FillProjectsTable(results) {
    var dataTable = $("#projects").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        var compliantTitle = escapeHtml(item.Title);
        dataTable.row.add([
            item.Id ? item.Id : "",
            item.Title ? "<a style='color:blue' href='javascript:void(0)' onClick='EditProject(" + item.Id + ")'>" + item.Title + "</a>" : "",
            item.PrimaryManagerID.Title ? item.PrimaryManagerID.Title : "",
            item.SecondaryManagerID.Title ? item.SecondaryManagerID.Title : "",
            item.LeadOffice.Title ? item.LeadOffice.Title : "",
            item.AssignedDate ? new Date(item.AssignedDate).format("M/d/yyyy") : "",
            item.CommutationStatus.Title,
            item.FinancialAuthority ? accounting.formatMoney(item.FinancialAuthority) : ""
        ]).draw();
    });
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

// Open the project in edit mode
function EditProject(projectId, tabIndex) {
    var itemUrl = _spPageContextInfo.webServerRelativeUrl + "/Pages/Projects/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + projectId;
    tabIndex ? itemUrl += "&Tab=" + tabIndex : itemUrl;
    var options = {
        url: itemUrl,
        title: "Edit Project",
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