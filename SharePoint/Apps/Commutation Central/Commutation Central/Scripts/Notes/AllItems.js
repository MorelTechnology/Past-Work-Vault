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

    var notesDataTable = $("#notes").DataTable({
        "autoWidth": false,
        "columnDefs": [
            { "searchable": false, "targets": [] },
            { "orderable": false, "targets": [] }
        ],
        "order": [0, 'asc'],
        "rowCallback": function (row, data) {
        }
    });

    RenderNotes();
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
function RenderNotes() {
    GetNotes().done(function (results) {
        FillNotesTable(results.d.results);
    }).fail(function (status) {
        displayError(status);
    });
}

// Get activities
function GetNotes() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Notes')/Items?$select=*,Project/Title,Project/CommProjectID,EntryType/Title&$expand=Project,EntryType&$top=5000";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the activities table
function FillNotesTable(results) {
    var dataTable = $("#notes").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var content = item.Content1;
        var isHtml = /<[a-z][\s\S]*>/i.test(content);
        content = $('<p>' + content + '</p>').text();
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            item.Title ? "<a style='color:blue' href='javascript:void(0)' onClick='EditNote(" + item.Id + ")'>" + item.Title + "</a>" : "<a style='color:blue' href='javascript:void(0)' onClick='EditNote(" + item.Id + ")'>No Title</a>",
            item.Project.Title ? "<a style='color:blue' href='javascript:void(0)' onClick='EditProject(" + item.ProjectId + ")'>" + item.Project.Title + "</a>" : "",
            item.ProjectId ? item.ProjectId : "",
            item.Project.Title ? item.Project.Title : "",
            item.Project.CommProjectID ? item.Project.CommProjectID : "",
            item.EntryType.Title ? item.EntryType.Title : "",
            content.substring(0, 30) + "...",
            item.EntryDate ? new Date(item.EntryDate).format("M/d/yyyy") : null,
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

// Open the activity in edit mode
function EditNote(noteId) {
    var itemUrl = _spPageContextInfo.webServerRelativeUrl + "/Pages/Notes/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + noteId;
    var options = {
        url: itemUrl,
        title: "Edit Note",
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}

// Open the project in edit mode
function EditProject(projectId) {
    var itemUrl = _spPageContextInfo.webServerRelativeUrl + "/Pages/Projects/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + projectId;
    var options = {
        url: itemUrl,
        title: "Edit Note",
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