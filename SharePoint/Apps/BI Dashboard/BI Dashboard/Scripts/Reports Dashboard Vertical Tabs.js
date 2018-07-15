window.RS = window.RS || {};

$(document).ready(function () {
    RS.Part.loading("content", true, "center", "top+100px");
    if (RS.Common.getQueryStringParameter('IsWizard') == 1) {
        RS.BuildDOM.AddPreferenceButton();
    }
    RS.GetData.GetConfigurationList().done(function (items, status) {
        RS.reportsLibraryConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Reports Library");
        RS.reportSetContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Report Set Content Type ID");
        RS.documentContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Document Content Type ID");

        RS.GetData.GetManagedMetadataTermSet("Department").done(function (departments) {
            RS.BuildDOM.CreateVerticalDepartmentTabs(departments);
            RS.Part.ResizeAppPart();
            RS.Part.doneLoading();
        }).fail(function (status) {
            alert('Failed to get the list of departments from the managed metadata service.');
            RS.Part.doneLoading();
        });
    }).fail(function (status) {
        alert('Failed to get the app configuration. Please check the configuration of BI Reports.');
        RS.Part.doneLoading();
    });
});