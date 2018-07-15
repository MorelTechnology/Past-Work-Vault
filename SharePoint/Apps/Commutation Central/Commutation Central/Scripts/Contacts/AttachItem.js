var appweburl;

// Execute when the DOM is ready
$(document).ready(function () {
    InitializeGlobalVars();
    InitializeFields();
});

//===============================
// Initialization
//===============================

// Show the loading animation
$(function () {
    $("#loadingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#attachItemContent") },
        modal: true,
        resizable: false
    });
});

// Initialize any required global variables
function InitializeGlobalVars() {
    appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
}

// Initialize any fields that aren't built in the DOM
function InitializeFields() {
    var getContacts = $.ajax({
        url: appweburl + "/_api/lists/getbytitle('Contact Lookup')/items?$orderby=FullName",
        type: "GET",
        headers: { "Accept": "application/json; odata=verbose" }
    });
    getContacts.done(function (item, status, xhr) {
        $.each(item.d.results, function (i, contact) {
            var itemId = contact.Id;
            var itemFullName = contact.FullName;
            $('#contactId').append("<option value='" + itemId + "'>" + itemFullName + "</option>");
        });
        $("#loadingdialog").dialog("close");
    });
    getContacts.fail(function (status, xhr) {
        $("#loadingdialog").dialog("close");
        displayError(status);
    });
}

//===============================
// End Initialization
//===============================

//===============================
// Form Submit
//===============================

// Submit the form
function SubmitForm() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(1, $("#contactId").val());
    }
    else {
        window.location = appweburl;
    }
}

// Cancel the submit
function CancelSubmit() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(0, 0);
    }
    else {
        window.location = appweburl;
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
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
}
//===============================
// End Utilities
//===============================