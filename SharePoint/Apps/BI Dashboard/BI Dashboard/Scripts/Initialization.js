window.RS = window.RS || {};

$(document).ready(function () {
    RS.Part.loading("content", true, "center", "top+100px");
    RS.GetData.GetCurrentUser().done(function (items, status) {
        var userId = items.d.Id;
        RS.GetData.GetCurrentUserPreferences(userId).done(function (prefs, status) {
            if (prefs.d.results.length == 0 || RS.Common.getQueryStringParameter("WizardOverride") == 1) {
                RS.BuildDOM.CreateTemplatePicker();
                RS.Part.ResizeAppPart();
                RS.Part.doneLoading();
            }
            else {
                document.location = prefs.d.results[0].DisplayPreference + "?" + document.URL.split('?')[1];
            }
        }).fail(function (status) {
            alert('Failed to get the app configuration. Please check the configuration of BI Reports.');
            RS.Part.doneLoading();
        });
    }).fail(function (status) {
        alert('Failed to get the app configuration. Please check the configuration of BI Reports.');
        RS.Part.doneLoading();
    });
});