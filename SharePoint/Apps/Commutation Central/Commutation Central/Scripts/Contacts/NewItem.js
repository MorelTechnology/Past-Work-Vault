var appweburl;

// Execute when the DOM is ready
$(document).ready(function () {
    InitializeGlobalVars();
    InitializeFields();
    var theForm = $("#aspnetForm");
    $("#aspnetForm").validate({
        submitHandler: function (form) {
            SubmitForm();
        }
    });
});

//===============================
// Initialization
//===============================

// Initialize any required global variables
function InitializeGlobalVars() {
    //appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
    appweburl = _spPageContextInfo.webServerRelativeUrl;
}

// Initialize any fields that aren't built in the DOM
function InitializeFields() {
    $("#notes").jqte();
}

//===============================
// End Initialization
//===============================

//===============================
// Form Submit
//===============================

// Submit the form
function SubmitForm() {
    $("#savingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#newItemContent") },
        modal: true,
        resizable: false,
        height: 120
    });
    var theForm = $("#aspnetForm")[0];
    var itemData = {
        '__metadata': { 'type': 'SP.Data.Contact_x0020_LookupListItem' },
        'Title': theForm.lastName.value,
        'FirstName': theForm.firstName.value,
        'FullName': theForm.fullName.value,
        'Email': theForm.emailAddress.value,
        'Company': theForm.company.value,
        'JobTitle': theForm.jobTitle.value,
        'WorkPhone': theForm.businessPhone.value,
        'HomePhone': theForm.homePhone.value,
        'CellPhone': theForm.mobileNumber.value,
        'WorkFax': theForm.faxNumber.value,
        'WorkAddress': theForm.address.value,
        'WorkCity': theForm.city.value,
        'WorkState': theForm.state.value,
        'WorkZip': theForm.zip.value,
        'WorkCountry': theForm.country.value,
        'Comments': theForm.notes.value
    };

    $.ajax({
        url: appweburl + "/_api/web/lists/getbytitle('Contact Lookup')/items",
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
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
}
//===============================
// End Utilities
//===============================