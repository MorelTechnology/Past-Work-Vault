$(document).ready(function () {
    var dt = $("#tblWorkMattersDashboard").show().DataTable({
        ajax: {
            url: RS.GAIT.Data.CompileAjaxRequestUrl("Work Matters", "*,ReportOwner/LastName,Affiliate/Title", "ReportOwner,Affiliate"),
            beforeSend: function (request) {
                request.setRequestHeader("Content-Type", "application/json");
                request.setRequestHeader("Accept", "application/json;odata=verbose");
            },
            dataSrc: "d.results"
        },
        language: {
            loadingRecords: "<img src='../Images/loading.gif' />"
        },
        stateSave: true,
        columns: [
            {
                data: "Title",
                render: function (data, type, full, meta) {
                    var itemUrl = RS.GAIT.Data.hostweburl + "/lists/Work Matters/DispForm.aspx?ID=" + full.ID;
                    return "<a href='#' onclick='RS.GAIT.Actions.OpenPopup(\"" + itemUrl + "\");'>" + data + "</a>";
                }
            },
            { data: "Work_x0020_Matter_x0020_Number" },
            { data: "ReportOwner.LastName" },
            {
                data: "Affiliate",
                defaultContent: "",
                render: function (data, type, full, meta) {
                    if (data.results.length > 0) {
                        var affiliateString = "";
                        $.each(data.results, function (index, affiliate) {
                            affiliateString += affiliate.Title + "; ";
                        });
                        affiliateString = affiliateString.trim().replace(/\;$/, '');
                        return affiliateString;
                    }
                    return null;
                }
            },
            {
                defaultContent: "Loading...",
                title: "Next Milestone/Task"
            },
            {
                defaultContent: "Loading...",
                title: "Task Owner"
            },
            {
                defaultContent: "Loading...",
                title: "Due Date"
            },
            {
                defaultContent: "Loading...",
                title: "Status"
            },
            {
                defaultContent: "Loading...",
                title: "Issue Category"
            },
            {
                defaultContent: "Loading...",
                title: "Comments"
            }
        ],
        columnDefs: [
            {
                targets: 4,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", null, null, "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("No upcoming tasks");
                        }
                        else {
                            var itemUrl = RS.GAIT.Data.hostweburl + "/lists/Issue Tasks/DispForm.aspx?ID=" + relatedTasks.d.results[0].ID;
                            dt.cell(row, col).data("<a href='#' onclick='RS.GAIT.Actions.OpenPopup(\"" + itemUrl + "\");'>" + relatedTasks.d.results[0].Title + "</a>");
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 5,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", "*,AssignedTo/LastName", "AssignedTo", "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            if (relatedTasks.d.results[0].AssignedTo.__deferred) {
                                dt.cell(row, col).data("N/A");
                            }
                            else {
                                dt.cell(row, col).data(relatedTasks.d.results[0].AssignedTo.results[0].LastName);
                            }
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 6,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", null, null, "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            dt.cell(row, col).data(moment(relatedTasks.d.results[0].DueDate).format('L'));
                            if (moment().isAfter(moment(relatedTasks.d.results[0].DueDate))) {
                                $(td).next("td").css('background-color', 'red');
                            } else if (moment().isBefore(moment(relatedTasks.d.results[0].DueDate))) {
                                $(td).next("td").css('background-color', 'green');
                            }
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 7,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", null, null, "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            dt.cell(row, col).data(relatedTasks.d.results[0].Status);
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 8,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", "*,Task_x0020_Issue/Title", "Task_x0020_Issue", "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            dt.cell(row, col).data(relatedTasks.d.results[0].Task_x0020_Issue.Title);
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 9,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Work Matter Tasks", null, null, "Task_x0020_Work_x0020_Matter eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            dt.cell(row, col).data(relatedTasks.d.results[0].Status_x0020_Comment ? relatedTasks.d.results[0].Status_x0020_Comment : "");
                        }
                        $(td).find("p").addClass("commentFieldTruncate").attr("title", function () { return $(this).text(); }).tooltip();
                        dt.cell(row, col).draw();
                    });
                }
            }
        ],
        initComplete: function () {
            RS.GAIT.Styling.ResizeAppPart(false, $("#WorkMatterDashboard")[0]);
        },
        drawCallback: function () {
            RS.GAIT.Styling.ResizeAppPart(false, $("#WorkMatterDashboard")[0]);
        }
    });
});