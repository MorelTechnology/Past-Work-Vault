window.RS = window.RS || {};

window.RS.departments = window.RS.departments || {};

$(document).ready(function () {
    RS.Part.loading("content", true, "center", "top+100px");
    if (RS.Common.getQueryStringParameter('IsWizard') == 1) {
        RS.BuildDOM.AddPreferenceButton();
    }
    RS.GetData.GetConfigurationList().done(function (items, status, xhr) {
        RS.reportsLibraryConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Reports Library");
        RS.reportSetContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Report Set Content Type ID");
        RS.documentContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Document Content Type ID");

        RS.GetData.GetManagedMetadataTermSet("Department").done(function (departments) {
            window.RS.departments = departments;
            RS.BuildDOM.CreateDepartmentSlides(departments);
            var departmentOptions = {
                $DragOrientation: 0,
                $ThumbnailNavigatorOptions: {
                    $Class: $JssorThumbnailNavigator$,
                    $ChanceToShow: 2,
                    $DisplayPieces: departments.length,
                    $DisableDrag: true
                }
            };

            var departmentSlider = RS.BuildDOM.InitializeSlider("departmentslider_container", departmentOptions);
            departmentSlider.$On($JssorSlider$.$EVT_PARK, function (slideIndex, fromIndex) {
                RS.Part.loading("content", true, "center", "top+100px");
                var currentDepartmentName = $("#departmentslider_container .pav .c").text();
                //var currentDepartmentId = window.RS.departments.filter(obj => obj.Name == currentDepartmentName)[0].guid;
                var currentDepartmentId = window.RS.departments.filter(function (dep) {
                    return dep.Name === currentDepartmentName;
                })[0].guid;

                RS.GetData.GetDepartmentReports(RS.reportsLibraryConfigItem.Value1, RS.reportSetContentTypeConfigItem.Value1, currentDepartmentName).done(function (reports, status, xhr) {
                    var departmentReportsTable;
                    if ($.fn.dataTable.isDataTable("#departmentReports" + currentDepartmentId)) {
                        departmentReportsTable = $("#departmentReports" + currentDepartmentId).DataTable().clear().draw();
                    }
                    else {
                        departmentReportsTable = $("#departmentReports" + currentDepartmentId).DataTable({
                            "searching": false,
                            "columnDefs": [
                                { "visible": false, "targets": [1, 2] }
                            ]
                        });
                    }

                    $("#departmentReports" + currentDepartmentId + " tbody").on("click", "tr", function () {
                        if ($(this).hasClass('selected')) {
                            $(this).removeClass('selected');
                            $("#reportDetails" + currentDepartmentId).hide(400);
                            $("#reportImage" + currentDepartmentId).hide(400);
                            $("#reportHistory" + currentDepartmentId).hide(400);
                        }
                        else {
                            RS.Part.loading("content", false, "top", "left top+100px");
                            departmentReportsTable.$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            var rowData = departmentReportsTable.row(this).data();
                            $("#reportDescription" + currentDepartmentId).text(rowData[1]);
                            $("#reportImg" + currentDepartmentId).attr('src', rowData[2]);

                            var reportHistoryTable;
                            if ($.fn.dataTable.isDataTable("#reportHistoryTable" + currentDepartmentId)) {
                                reportHistoryTable = $("#reportHistoryTable" + currentDepartmentId).DataTable().clear().draw();
                            }
                            else {
                                reportHistoryTable = $("#reportHistoryTable" + currentDepartmentId).DataTable({
                                    "searching": false,
                                    "order": [2, "desc"]
                                });
                            }

                            RS.GetData.GetHistoricalReports(RS.reportsLibraryConfigItem.Value1, RS.documentContentTypeConfigItem.Value1, rowData[0]).done(function (historicalReports, status, xhr) {
                                if (historicalReports.d.results.length > 0) {
                                    $("#currentReportLink" + currentDepartmentId).attr("href", historicalReports.d.results[0].File.ServerRelativeUrl);
                                }
                                else { $("#currentReportLink" + currentDepartmentId).attr("href", "#"); }
                                $.each(historicalReports.d.results, function (i, report) {
                                    reportHistoryTable.row.add([
                                        "<a href='" + report.File.ServerRelativeUrl + "'>" + report.File.Name + "</a>",
                                        report.Valuation ? new Date(report.Valuation).format("MM/dd/yyyy") : null,
                                        report.Distribution_x0020_Date ? new Date(report.Distribution_x0020_Date).format("MM/dd/yyyy") : null
                                    ]).draw();
                                });

                                $("#reportDetails" + currentDepartmentId).show(400);
                                $("#reportImage" + currentDepartmentId).show(400);
                                $("#reportHistory" + currentDepartmentId).show(400);
                                $("#reportTabs" + currentDepartmentId).tabs().show(400);
                                RS.Part.doneLoading();
                            });
                        }
                    });

                    $("#deptSlide" + currentDepartmentId).parent().css("overflow", "auto");
                    $.each(reports.d.results, function (i, report) {
                        departmentReportsTable.row.add([
                            report.Report_x0020_Name,
                            report.Report_x0020_Summary ? report.Report_x0020_Summary : "This report does not have a description",
                            report.Report_x0020_Preview ? report.Report_x0020_Preview.Url : "/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png"
                        ]).draw();
                    });
                    RS.Part.doneLoading();
                }).fail(function (status, xhr) {
                    RS.Part.doneLoading();
                });
            });
            RS.Part.init();
        });
    });
});