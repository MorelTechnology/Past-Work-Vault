$(document).ready(function () {
    var dt = $("#tblInitiativesDashboard").show().DataTable({
        ajax: {
            url: RS.GAIT.Data.CompileAjaxRequestUrl("Initiatives", "*,ReportOwner/LastName", "ReportOwner"),
            beforeSend: function (request) {
                request.setRequestHeader("Content-Type", "application/json");
                request.setRequestHeader("Accept", "application/json;odata=verbose");
            },
            dataSrc: "d.results"
        },
        stateSave: true,
        language: {
            loadingRecords: "<img src='../Images/loading.gif' />"
        },
        columns: [
            {
                data: "Initiative_x0020_Short_x0020_Nam",
                render: function (data, type, full, meta) {
                    var itemUrl = RS.GAIT.Data.hostweburl + "/lists/Initiatives/DispForm.aspx?ID=" + full.ID;
                    return "<a href='#' onclick='RS.GAIT.Actions.OpenPopup(\"" + itemUrl + "\");'>" + data + "</a>";
                }
            },
            { data: "Title" },
            { data: "ReportOwner.LastName" },
            {
                data: "Date_x0020_Plan_x0020_Approved",
                render: function (dateApproved) {
                    if (dateApproved !== null) {
                        return moment(dateApproved).format('L');
                    } else return "";
                }
            },
            {
                defaultContent: "Loading...",
                title: "Initiative Status"
            },
            {
                defaultContent: "Loading...",
                title: "Next Scheduled Task/Milestone"
            },
            {
                defaultContent: "Loading...",
                title: "Task Owner(s)"
            },
            {
                defaultContent: "Loading...",
                title: "Due Date"
            },
            {
                defaultContent: "Loading...",
                title: "Milestone Status"
            },
            {
                defaultContent: "Loading...",
                title: "Status Comment"
            }
        ],
        columnDefs: [
            {
                targets: 4,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", null, null, "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        /*var numBehind, numOnTarget, numNotStarted;
                        numBehind = numOnTarget = numNotStarted = 0;*/
                        if (relatedTasks.d.results.length == 0) {
                            dt.cell(row, col).data("On Target");
                            $(td).css('background-color', 'green');
                        } else {
                            /*$.each(relatedTasks.d.results, function (index, task) {
                                if (moment(task.DueDate).isBefore(moment())) {
                                    numBehind++;
                                } else if (moment(task.DueDate).isAfter(moment()) && task.TaskStatus == "In Progress") {
                                    numOnTarget++;
                                } else {
                                    numNotStarted++;
                                }
                            });
                            if (numBehind >= numOnTarget + numNotStarted && numBehind !== 0) {
                                dt.cell(row, col).data("Behind Schedule");
                                $(td).css('background-color', 'red');
                            }
                            else {
                                dt.cell(row, col).data("On Target");
                                $(td).css('background-color', 'green');
                            }*/
                            if (moment().isAfter(moment(relatedTasks.d.results[0].DueDate))) {
                                dt.cell(row, col).data("Behind Schedule");
                                $(td).css('background-color', 'red');
                            } else {
                                dt.cell(row, col).data("On Target");
                                $(td).css('background-color', 'green');
                            }
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 5,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", null, null, "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("No upcoming tasks");
                        }
                        else {
                            var itemUrl = RS.GAIT.Data.hostweburl + "/lists/Initiative Tasks/DispForm.aspx?ID=" + relatedTasks.d.results[0].ID;
                            dt.cell(row, col).data("<a href='#' onclick='RS.GAIT.Actions.OpenPopup(\"" + itemUrl + "\");'>" + relatedTasks.d.results[0].Title + "</a>");
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 6,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", "*,AssignedTo/LastName", "AssignedTo", "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
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
                targets: 7,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", null, null, "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            dt.cell(row, col).data(moment(relatedTasks.d.results[0].DueDate).format('L'));
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 8,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", null, null, "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            relatedTasks.d.results[0].Status ? dt.cell(row, col).data(relatedTasks.d.results[0].Status) : dt.cell(row, col).data(relatedTasks.d.results[0].Task_x0020_Status);
                        }
                        dt.cell(row, col).draw();
                    });
                }
            },
            {
                targets: 9,
                createdCell: function (td, cellData, rowData, row, col) {
                    RS.GAIT.Data.GetHostListData("Initiative Tasks", null, null, "Task_x0020_Initiative eq " + rowData.Id + " and PercentComplete lt 1", "DueDate asc", 1).done(function (relatedTasks) {
                        if (relatedTasks.d.results.length === 0) {
                            dt.cell(row, col).data("");
                        }
                        else {
                            var cellText = "";
                            if (relatedTasks.d.results[0].Status_x0020_Comment)
                            {
                                cellText = relatedTasks.d.results[0].Status_x0020_Comment;
                            }
                            dt.cell(row, col).data(cellText);
                        }
                        $(td).find("p").addClass("commentFieldTruncate").attr("title", function () { return $(this).text(); }).tooltip();
                        dt.cell(row, col).draw();
                    });
                }
            }
        ],
        initComplete: function () {
            RS.GAIT.Styling.ResizeAppPart(false, $("#InitiativesDashboard")[0]);
        },
        drawCallback: function () {
            RS.GAIT.Styling.ResizeAppPart(false, $("#InitiativesDashboard")[0]);
        }
    });

    
});