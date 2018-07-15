var appweburl;

$(document).ready(function () {
    appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
});

// Handle a file upload field change
function FileUploadFieldChanged(sender) {
    var lookupField;
    switch (getQueryStringParameter("ContentType")) {
        case "Project Document":
            lookupField = "Project";
            uploadDocument($("#" + sender), getQueryStringParameter("ContentType"), lookupField, getQueryStringParameter("ProjectId"), getQueryStringParameter("CounterpartyName"), CloseForm);
            break;
        case "Activity Document":
            lookupField = "Activity";
            uploadDocument($("#" + sender), getQueryStringParameter("ContentType"), lookupField, getQueryStringParameter("ActivityId"), getQueryStringParameter("CounterpartyName"), CloseForm);
            break;
    }
    var control = $("#" + sender);
    control.replaceWith(control = control.clone(true));
}

// Return a query string parameter
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

// Close the form
function CloseForm() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(1, 1);
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