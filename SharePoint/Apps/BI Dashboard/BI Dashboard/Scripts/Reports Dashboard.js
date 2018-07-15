var appweburl = decodeURIComponent(RS.Common.getQueryStringParameter("SPAppWebUrl"));
var hostweburl = decodeURIComponent(RS.Common.getQueryStringParameter("SPHostUrl"));
var reportsLibraryConfigItem, reportSetContentTypeConfigItem, documentContentTypeConfigItem;

$(document).ready(function () {
    RS.Part.loading("content", true, "center", "top+100px");
    if (RS.Common.getQueryStringParameter('IsWizard') == 1) {
        RS.BuildDOM.AddPreferenceButton();
    }
    GetConfigurationList().done(function (items, status, xhr) {
        reportsLibraryConfigItem = GetItemFromResultsArray(items.d.results, "Title", "Reports Library");
        reportSetContentTypeConfigItem = GetItemFromResultsArray(items.d.results, "Title", "Report Set Content Type ID");
        documentContentTypeConfigItem = GetItemFromResultsArray(items.d.results, "Title", "Document Content Type ID");

        if (reportsLibraryConfigItem === null) {
            DisplayError("Error: The BI Dashboard App is not connected to the reports library. <br/>Please click <a href='" + appweburl + "/lists/Configuration' target='_blank'>here</a>"
                + " to open the configuration list. Add a new item with a key of 'Reports Library' and set the value field to the name of the reports library.");
        }
        else if (reportSetContentTypeConfigItem === null) {
            DisplayError("Error: The BI Dashboard App is not connected to the report set content type. <br/>Please click <a href='" + appweburl + "/lists/Configuration' target='_blank'>here</a>"
                + " to open the configuration list. Add a new item with a key of 'Report Set Content Type ID' and set the value field to the ID of the report set content type.");
        }
        else if (documentContentTypeConfigItem === null) {
            DisplayError("Error: The BI Dashboard App is not connected to the document content type. <br/>Please click <a href='" + appweburl + "/lists/Configuration' target='_blank'>here</a>"
                + " to open the configuration list. Add a new item with a key of 'Document Content Type ID' and set the value field to the ID of the document content type.");
        }
        else {
            RS.GetData.GetManagedMetadataTermSet("Department").done(function (departments) {
                RS.BuildDOM.CreateDepartmentDropDown(departments);
            }).fail(function (status, xhr) {
                RS.Part.doneLoading();
            });
            InitializeTabs();
            RS.Part.doneLoading();
            $("#departmentDropDown").change(function () {
                RS.GetData.GetDepartmentReports(reportsLibraryConfigItem.Value1, reportSetContentTypeConfigItem.Value1, $(this).val()).done(function (reportSets, status, xhr) {
                    FillCurrentReportsTable(reportSets.d.results);
                    FillHistoricalReportsTable(reportSets.d.results);
                }).fail(function (status, xhr) {
                    DisplayError("Error: The reports library specified in the configuration could not be found. <br/><a href='" + appweburl + "/lists/Configuration' target='_blank'>Click here</a>"
                    + " to open the app configuration and verify the URL.")
                });
            });
            /*GetReportSets(reportsLibraryConfigItem.Value1, reportSetContentTypeConfigItem.Value1).done(function (reportSets, status, xhr) {
                
                FillCurrentReportsTable(reportSets.d.results);
                FillHistoricalReportsTable(reportSets.d.results);
            }).fail(function (status, xhr) {
                DisplayError("Error: The reports library specified in the configuration could not be found. <br/><a href='" + appweburl + "/lists/Configuration' target='_blank'>Click here</a>"
                    + " to open the app configuration and verify the URL.")
            });*/
        }
    }).fail(function (status, xhr) {
        DisplayError("Error: The configuration for this web part is missing. <br/>Please reinstall the BI Dashboard App.");
    });
});

function DisplayError(message) {
    $("#error").show();
    $("#error").html(message);
}

// Initialize the jQuery tabs UI
function InitializeTabs() {
    $("#formTabs").tabs({
        activate: function (event, ui) { RS.Part.init(); }
    }).show();
}

function GetConfigurationList() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Configuration')/Items";

    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

function GetReportSets(LibraryName, ContentTypeId) {
    var endpointUrl = appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/Items?@target='" + hostweburl + "'&$filter=ContentTypeId eq '" + ContentTypeId + "'";

    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

function GetItemFromResultsArray(Array, Property, Value) {
    for (var i = 0; i < Array.length; i++) {
        if (Array[i][Property] === Value) {
            return Array[i];
        }
    }

    return null;
}

function GetReportSetByName(LibraryName, ContentTypeId, ReportSetName) {
    var endpointUrl = appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/Items?@target='" + hostweburl + "'&$filter=ContentTypeId eq '" + ContentTypeId + "' and Title eq '" + ReportSetName + "'";

    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

function GetCurrentReport(LibraryName, ContentTypeId, ReportSetName) {
    var endpointUrl = appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/Items?@target='" + hostweburl + "'&$filter=ContentTypeId eq '" + ContentTypeId + "' and Report_x0020_Name eq '" + ReportSetName + "'&$orderby=Distribution_x0020_Date desc&$top=1&$expand=File";

    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

function FillCurrentReportsTable(results) {
    var dataTable;
    if ($.fn.dataTable.isDataTable("#currentReports")) {
        dataTable = $("#currentReports").DataTable().clear().draw();
    }
    else {
        dataTable = $("#currentReports").DataTable({
            "drawCallback": function () { RS.Part.init(); },
            "columnDefs": [
                { "className": "tableCellNoWrap", "targets": [0, 1] },
                { "visible": false, "targets": [2] }
            ]
        });
    }

    var activePromises = [];

    $("#currentReports tbody").on("click", "tr", function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#currentReportDetails").hide(400);
            $("#currentReportImage").hide(400);
        }
        else {
            RS.Part.loading("currentReportCell", false, "top", "left top+100px");
            dataTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            var reportSetName = $('td:first', $(this)).text();
            var rowData = dataTable.row(this).data();
            if (rowData != undefined) {
                GetReportSetByName(reportsLibraryConfigItem.Value1, reportSetContentTypeConfigItem.Value1, reportSetName).done(function (reportSet, status, xhr) {
                    $("#currentReportDescription").text(reportSet.d.results[0].Report_x0020_Summary ? reportSet.d.results[0].Report_x0020_Summary : "This report does not have a description");
                    $("#currentReportDetails").show(400);
                    $("#currentReportLink").attr('href', rowData[2]);
                    $("#currentReportImg").attr('src', (reportSet.d.results[0].Report_x0020_Preview ? reportSet.d.results[0].Report_x0020_Preview.Url : "/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png"));
                    $("#currentReportImage").show(400, function () { RS.Part.init(); });
                    RS.Part.init();
                    RS.Part.doneLoading();
                });
            }
            else {
                RS.Part.doneLoading();
            }
        }
    })

    $.each(results, function (i, item) {
        var reportSetName = item.Report_x0020_Name;
        activePromises.push(GetCurrentReport(reportsLibraryConfigItem.Value1, documentContentTypeConfigItem.Value1, reportSetName).done(function (currentReport, status, xhr) {
            if (currentReport.d.results.length > 0) {
                    dataTable.row.add([
                    item.Report_x0020_Name,
                    currentReport.d.results[0].Distribution_x0020_Date ? new Date(currentReport.d.results[0].Distribution_x0020_Date).format("MM/dd/yyyy") : "",
                    currentReport.d.results[0].File ? currentReport.d.results[0].File.ServerRelativeUrl : ""
                ]);
            }
        }));
    });

    $.when.apply(null, activePromises).done(function () {
        dataTable.draw();
        RS.Part.doneLoading();
    })
}

function GetHistoricalReports(LibraryName, ContentTypeId, ReportSetName) {
    var endpointUrl = appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/Items?@target='" + hostweburl + "'&$filter=ContentTypeId eq '" + ContentTypeId + "' and Report_x0020_Name eq '" + ReportSetName + "'&$orderby=Distribution_x0020_Date desc&$expand=File";

    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

function FillHistoricalReportsTable(results) {
    var dataTable;
    if ($.fn.dataTable.isDataTable("#historicalReports")) {
        dataTable = $("#historicalReports").DataTable().clear().draw();
    }
    else {
        dataTable = $("#historicalReports").DataTable({
            "drawCallback": function () { RS.Part.init(); },
            "columnDefs": [
                { "className": "tableCellNoWrap", "targets": [0] }
            ]
        });
    }

    $("#historicalReports tbody").on("click", "tr", function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $("#historicalReportsDetail").hide(400);
        }
        else {
            RS.Part.loading("historicalReportCell", false, "top", "left top+100px");
            dataTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            var rowData = dataTable.row(this).data();
            GetHistoricalReports(reportsLibraryConfigItem.Value1, documentContentTypeConfigItem.Value1, rowData[0]).done(function (historicalReports, status, xhr) {
                var reportHistoryTable;
                if ($.fn.dataTable.isDataTable("#historicalReportsTable")) {
                    reportHistoryTable = $("#historicalReportsTable").DataTable().clear();
                }
                else {
                    reportHistoryTable = $("#historicalReportsTable").DataTable({
                        "searching": false
                    });
                }

                $.each(historicalReports.d.results, function (i, item) {
                    reportHistoryTable.row.add([
                        "<a href='" + item.File.ServerRelativeUrl + "'>" + item.File.Name + "</a>",
                        item.Valuation ? new Date(item.Valuation).format("MM/dd/yyyy") : null,
                        item.Distribution_x0020_Date ? new Date(item.Distribution_x0020_Date).format("MM/dd/yyyy") : null
                    ]).draw();
                });
                $("#historicalReportsDetail").show(400);
                RS.Part.init();
                RS.Part.doneLoading();
            });
            /*GetReportSetByName(reportsLibraryConfigItem.Value1, reportSetContentTypeConfigItem.Value1, reportSetName).done(function (reportSet, status, xhr) {
                $("#currentReportDescription").text(reportSet.d.results[0].Report_x0020_Summary ? reportSet.d.results[0].Report_x0020_Summary : "This report does not have a description");
                $("#currentReportDetails").show(400);
                $("#currentReportLink").attr('href', rowData[2]);
                $("#currentReportImg").attr('src', (reportSet.d.results[0].Report_x0020_Preview ? reportSet.d.results[0].Report_x0020_Preview.Url : "/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png"));
                $("#currentReportImage").show(400, function () { RS.Part.init(); });
                RS.Part.init();
                RS.Part.doneLoading();
            });*/
        }
    })

    $.each(results, function (i, item) {
        dataTable.row.add([
            item.Report_x0020_Name
        ]).draw();
    });
}