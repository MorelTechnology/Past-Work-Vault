window.RS = window.RS || {};

RS.reportsLibraryConfigItem = RS.reportsLibraryConfigItem || "";
RS.reportSetContentTypeConfigItem = RS.reportsLibraryConfigItem || "";
RS.documentContentTypeConfigItem = RS.documentContentTypeConfigItem || "";

RS.Part = {
    init: function () {
        try {
            var target = parent.postMessage ? parent : (parent.document.postMessage ? parent.document : undefined);
            target.postMessage('<message senderId=' + RS.Common.getQueryStringParameter("SenderId") + '>resize(' + ($(document).width()) + ',' + ($(document).height()) + ')</message>', '*');
        }
        catch (e) {
            console.log(e);
        }
    },
    loading: function (centerOver, isModal, location1, location2) {
        $("#loadingDialog").dialog({
            dialogClass: "no-close",
            position: { my: location1, at: location2, collision: "fit", of: $("#" + centerOver) },
            resizable: false,
            modal: isModal
        });
    },
    doneLoading: function () {
        if ($("#loadingDialog").hasClass('ui-dialog-content')) {
            if ($("#loadingDialog").dialog("isOpen")) {
                $("#loadingDialog").dialog("close");
            }
        }
    },
    ResizeAppPart: function () {
        if (window.parent == null)
            return;

        var senderId = RS.Common.getQueryStringParameter("SenderId");
        var message = "<Message senderId=" + senderId + ">resize(" + $(document).width() + "," + $("#content").height() + ")</Message>";
        window.parent.postMessage(message, RS.GetData.hostweburl);
    }
}

RS.Common = {
    getQueryStringParameter: function(param){
        var params = document.URL.split("?")[1].split("&");
        var strParams = "";
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == param) {
                return singleParam[1];
            }
        }
    }
}

RS.GetData = {
    appweburl: decodeURIComponent(RS.Common.getQueryStringParameter("SPAppWebUrl")),
    hostweburl: decodeURIComponent(RS.Common.getQueryStringParameter("SPHostUrl")),

    // Deprecated
    InitConfig: function () {
        RS.GetData.GetConfigurationList().done(function (items, status, xhr) {
            RS.reportsLibraryConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Reports Library");
            RS.reportSetContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Report Set Content Type ID");
            RS.documentContentTypeConfigItem = RS.GetData.GetItemResultsFromArray(items.d.results, "Title", "Document Content Type ID");
        });
    },

    GetItemResultsFromArray: function (Array, Property, Value) {
        for (var i = 0; i < Array.length; i++) {
            if (Array[i][Property] === Value) {
                return Array[i];
            }
        }

        return null;
    },

    GetConfigurationList: function () {
        var endpointUrl = RS.GetData.appweburl + "/_api/web/lists/getbytitle('Configuration')/Items";

        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    },

    GetManagedMetadataTermSet: function (TermSetName) {
        var dfd = $.Deferred();

        var clientContext = new SP.ClientContext.get_current();
        var site = clientContext.get_site();
        var tSession = SP.Taxonomy.TaxonomySession.getTaxonomySession(clientContext);
        var ts = tSession.getDefaultSiteCollectionTermStore();
        var group = ts.getSiteCollectionGroup(site, false);
        //var tsets = ts.getTermSetsByName(TermSetName, 1033);
        var tsets = group.get_termSets();
        var tset = tsets.getByName(TermSetName);
        var terms = tset.getAllTerms();

        clientContext.load(tSession);
        clientContext.load(ts);
        clientContext.load(tsets);
        clientContext.load(tset);
        clientContext.load(terms);

        clientContext.executeQueryAsync(
            function () {
                var termEnumerator = terms.getEnumerator();
                var termList = [];
                while (termEnumerator.moveNext()) {
                    var currentTerm = termEnumerator.get_current();
                    var jsObj = { guid: currentTerm.get_id().toString(), Name: currentTerm.get_name() };
                    termList.push(jsObj);
                }
                dfd.resolve(termList);
            },
            function () {
                var termList = [];
                dfd.resolve(termList);
            });

        return dfd;
    },

    GetDepartmentReports: function (LibraryName, ContentTypeId, DepartmentName) {
        var dfd = $.Deferred();

        $.ajax({
            url: RS.GetData.appweburl + "/_api/contextinfo",
            type: "POST",
            headers: {
                "accept": "application/json;odata=verbose",
                "contentType": "text/xml"
            },
            success: function (data) {
                var formDigest = data.d.GetContextWebInformation.FormDigestValue;
                var endpointUrl = RS.GetData.appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/GetItems?@target='" + RS.GetData.hostweburl + "'";
                var requestData = {
                    "query":
                        {
                            "__metadata":
                               { "type": "SP.CamlQuery" },
                            "ViewXml": "<View><Query><OrderBy><FieldRef Name='Report_x0020_Name' Ascending='TRUE'/></OrderBy><Where><Eq><FieldRef Name='Department'/><Value Type='TaxonomyFieldType'>" + DepartmentName + "</Value></Eq></Where></Query></View>"
                        }
                };

                return $.ajax({
                    url: endpointUrl,
                    type: "POST",
                    data: JSON.stringify(requestData),
                    contentType: "application/json; odata=verbose",
                    headers: {
                        "X-RequestDigest": formDigest,
                        "Accept": "application/json; odata=verbose"
                    },
                    success: function (deptData) {
                        dfd.resolve(deptData);
                    },
                    error: function (err) {
                        alert('error');
                    }
                });
            },
            error: function (err) {
                alert(JSON.stringify(err));
            }
        });

        return dfd;
    },

    GetHistoricalReports: function (LibraryName, ContentTypeId, ReportSetName) {
        var endpointUrl = RS.GetData.appweburl + "/_api/SP.AppContextSite(@target)/web/lists/getbytitle('" + LibraryName + "')/Items?@target='" + RS.GetData.hostweburl + "'&$filter=ContentTypeId eq '" + ContentTypeId + "' and Report_x0020_Name eq '" + ReportSetName + "'&$orderby=Distribution_x0020_Date desc&$expand=File";

        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    },

    GetCurrentUser: function () {
        var endpointUrl = RS.GetData.appweburl + "/_api/web/currentuser";

        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    },

    GetCurrentUserPreferences: function (UserId) {
        var endpointUrl = RS.GetData.appweburl + "/_api/web/lists/getbytitle('User Preferences')/items?$filter=User1/Id eq " + UserId;

        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    }
}

RS.BuildDOM = {
    CreateDepartmentSlides: function (Departments) {
        $.each(Departments, function (i, Department) {
            $("#departmentslider_container div[u='slides']:first").append("<div><div id='deptSlide" + Department.guid + "' class='DepartmentSlide'></div><div u='thumb'>" + Department.Name + "</div></div>");
            $("#deptSlide" + Department.guid).append("<table id='reportsContainer" + Department.guid + "' style='width: 100%'>" +
                "<tr>" +
                    "<td style='width: 25%; vertical-align: top'>" +
                        "<table id='departmentReports" + Department.guid + "' class='display'>" +
                            "<thead>" +
                                "<tr>" +
                                    "<th style='text-align: left'>Report Name</th>" +
                                    "<th style='text-align: left'>Report Description</th>" +
                                    "<th style='text-align: left'>Image Preview</th>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody style='text-align: left'>" +
                            "</tbody>" +
                        "</table>" +
                    "</td>" +
                    "<td style='vertical-align: top'>" +
                        "<div id='reportTabs" + Department.guid + "' style='display: none; font-size: 1em;'>" +
                            "<ul>" +
                                "<li><a href='#reportCurrent" + Department.guid + "'>Current Report</a></li>" +
                                "<li><a href='#reportHistorical" + Department.guid + "'>Report History</a></li>" +
                            "</ul>" +
                            "<div id='reportCurrent" + Department.guid + "'>" +
                                "<div id='reportDetails" + Department.guid + "' style='margin-right: 20px; margin-bottom: 20px; border: thin black solid; display: none'>" +
                                    "<p style='margin-left: 5px;'>Report Description:</p>" +
                                    "<p id='reportDescription" + Department.guid + "' style='margin-left: 5px;'></p>" +
                                "</div>" +
                                "<div id='reportImage" + Department.guid + "' style='margin-right: 20px; margin-bottom: 20px; border: thin black solid; display: none; max-width: 744px; max-height: 400px; overflow: auto;'>" +
                                    "<p style='margin-left: 5px'>Report Preview:</p>" +
                                    "<p id='reportPreview" + Department.guid + "' style='margin-left: 5px'>" +
                                        "<a id='currentReportLink" + Department.guid + "' href='#'>" +
                                            "<img id='reportImg" + Department.guid + "' src='/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png' />" +
                                        "</a>" +
                                    "</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id='reportHistorical" + Department.guid + "'>" +
                                "<div id='reportHistory" + Department.guid + "' style='margin-right: 20px; margin-bottom: 20px; border: thin black solid; display: none'>" +
                                    "<p style='margin-left: 5px'>Report History:</p>" +
                                    "<p style='margin-left: 5px'>" +
                                        "<table id='reportHistoryTable" + Department.guid + "' class='display'>" +
                                            "<thead>" +
                                                "<tr>" +
                                                    "<th style='text-align: left'>Filename</th>" +
                                                    "<th style='text-align: left'>Valuation Date</th>" +
                                                    "<th style='text-align: left'>Distribution Date</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody style='text-align: left'>" +
                                            "</tbody>" +
                                        "</table>" +
                                    "</p>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</td>" +
                "</tr>" +
            "</table>");
        });
    },

    CreateDepartmentDropDown: function (Departments) {
        $.each(Departments, function (i, Department) {
            $('<option />', { value: Department.Name, text: Department.Name }).appendTo($('#departmentDropDown'));
        });
    },

    CreateVerticalDepartmentTabs: function (Departments) {
        $("#content").append("<div id='departmentTabs'><ul></ul></div>");
        $.each(Departments, function (i, department) {
            $("#departmentTabs ul").append("<li><a href='#departmentTabs" + department.guid + "'>" + department.Name + "</a></li>");
            $("#departmentTabs").append("<div id='departmentTabs" + department.guid + "' department='" + department.Name + "' departmentId='" + department.guid + "'></div>");
        });

        $("#departmentTabs").tabs({
            create: function (event, ui) {
                var dept = $(ui.panel).attr("department");
                var departmentId = $(ui.panel).attr("departmentId");
                RS.BuildDOM.CreateReportsAccordion(ui.panel, dept, departmentId);
                RS.Part.ResizeAppPart();
            },
            beforeActivate: function (event, ui) {
                var dept = $(ui.newPanel).attr("department");
                var departmentId = $(ui.newPanel).attr("departmentId");
                RS.BuildDOM.CreateReportsAccordion(ui.newPanel, dept, departmentId);
                RS.Part.ResizeAppPart();
            }
        }).show().addClass("ui-tabs-vertical ui-helper-clearfix");
        $("#departmentTabs li").removeClass("ui-corner-top").addClass("ui-corner-left");
    },

    CreateReportsAccordion: function (container, departmentName, departmentId) {
        $(container).html("<div id='accordionReports" + departmentId + "'></div>");
        RS.Part.loading("content", true, "center", "top+100px");
        RS.GetData.GetDepartmentReports(RS.reportsLibraryConfigItem.Value1, RS.reportSetContentTypeConfigItem.Value1, departmentName).done(function (reportSets) {
            RS.Part.doneLoading();
            if (reportSets.d.results.length == 0) {
                $("#accordionReports" + departmentId).append("<h3>No reports to show for this department.</h3>");
                return;
            }

            $.each(reportSets.d.results, function (i, reportSet) {
                $("#accordionReports" + departmentId).append("<h3>" + reportSet.Report_x0020_Name + "</h3>" +
                    "<div>" +
                    "<div class='reportsTabsContainer' id='reportTabs" + encodeURIComponent(departmentId + reportSet.Report_x0020_Name) + "'>" +
                            "<ul>" +
                    "<li><a href='#reportCurrent" + encodeURIComponent(departmentId + reportSet.Report_x0020_Name) + "'>Current Report</a></li>" +
                    "<li><a href='#reportHistorical" + encodeURIComponent(departmentId + reportSet.Report_x0020_Name) + "'>Report History</a></li>" +
                    "</ul>" +
                    "<div id='reportCurrent" + encodeURIComponent(departmentId + reportSet.Report_x0020_Name) + "'>" +
                    "<div id='reportDetails" + departmentId + reportSet.Report_x0020_Name + "' style='margin: 20px; border: thin black solid;'>" +
                                    "<p style='margin-left: 5px'>Report Description:</p>" +
                                    "<p style='margin-left: 5px'>" + (reportSet.Report_x0020_Summary ? reportSet.Report_x0020_Summary : "This report does not have a description") + "</p>" +
                    "</div>" +
                    "<div id='reportImage" + departmentId + reportSet.Report_x0020_Name + "' style='margin: 20px; border: thin black solid; max-width: 1000px; max-height: 400px; overflow: auto;'>" +
                    "<p style='margin-left: 5px'>Report Preview:</p>" +
                    "<p id='reportPreview" + departmentId + reportSet.Report_x0020_Name + "' style='margin-left: 5px'>" +
                    "<a id='currentReportLink" + departmentId + reportSet.Report_x0020_Name + "' href='#'>" +
                    "<img id='reportImg" + departmentId + reportSet.Report_x0020_Name + "' src='" + (reportSet.Report_x0020_Preview ? reportSet.Report_x0020_Preview.Url : "/sites/BIReports/_layouts/15/images/RiverStonelogotransparent.png") + "' />" +
                                        "</a>" +
                                    "</p>" +
                                "</div>" +
                    "</div>" +
                    "<div id='reportHistorical" + encodeURIComponent(departmentId + reportSet.Report_x0020_Name) + "'>" +
                    "<div id='reportHistory" + departmentId + reportSet.Report_x0020_Name + "' style='margin: 20px; border: thin black solid'>" +
                                    "<p style='margin-left: 5px'>Report History:</p>" +
                    "<p style='margin-left: 5px'>" +
                    "<table id='reportHistoryTable" + departmentId + reportSet.Report_x0020_Name + "' class='display'>" +
                                            "<thead>" +
                                                "<tr>" +
                                                    "<th style='text-align: left'>Filename</th>" +
                                                    "<th style='text-align: left'>Valuation Date</th>" +
                                                    "<th style='text-align: left'>Distribution Date</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                        "</table>" +
                                    "</p>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>");

                RS.Part.loading("content", true, "center", "top+100px");
                $(".reportsTabsContainer").tabs().removeClass("ui-tabs-vertical").removeClass("ui-helper-clearfix");
                RS.GetData.GetHistoricalReports(RS.reportsLibraryConfigItem.Value1, RS.documentContentTypeConfigItem.Value1, reportSet.Report_x0020_Name).done(function (historicalReports) {
                    RS.Part.doneLoading();
                    if (historicalReports.d.results.length > 0) { $("a[id='currentReportLink" + departmentId + reportSet.Report_x0020_Name + "']").attr("href", historicalReports.d.results[0].File.ServerRelativeUrl); }

                    $("table[id='reportHistoryTable" + departmentId + reportSet.Report_x0020_Name + "']").DataTable({
                        "autoWidth": false,
                        "data": historicalReports.d.results,
                        "columns": [
                            {
                                "data": "File.Name",
                                "render": function (data, type, row) {
                                    return "<a style='display: block' href='" + row.File.ServerRelativeUrl + "'>" + data + "</a>";
                                }
                            },
                            {
                                "data": "Valuation",
                                "defaultContent": "",
                                "render": function (data, type, row) {
                                    if (typeof data !== "undefined") {
                                        return new Date(data).format("MM/dd/yyyy");
                                    }
                                    return "";
                                }
                            },
                            {
                                "data": "Distribution_x0020_Date",
                                "defaultContent": "",
                                "render": function (data, type, row) {
                                    if (typeof data !== "undefined") {
                                        return new Date(data).format("MM/dd/yyyy");
                                    }
                                    return "";
                                }
                            }

                        ]
                    });
                }).fail(function (status) {
                    alert('Failed to get historical reports for ' + reportSet.Report_x0020_Name);
                });
            });
            $("#accordionReports" + departmentId).accordion({
                collapsible: true,
                heightStyle: "content",
                active: false,
                activate: function () {
                    RS.Part.ResizeAppPart();
                }
            });
            RS.Part.ResizeAppPart();
        }).fail(function (status) {
            alert('Failed to get reports for ' + departmentName);
        });
    },

    CreateTemplatePicker: function () {
        var pickerDOM = "<div style='position: relative; width: 100%; background-color: #003399; overflow: hidden;'>" +
                            "<div style='position: relative; left: 50%; width: 5000px; text-align: center; margin-left: -2500px;'>" +
                                "<div id='slider1_container' style='position: relative; margin: 0 auto; top: 0px; left: 0px; width: 980px; height: 400px; background: url(../Images/main_bg.jpg) top center no-repeat;'>" +
                                    "<div u='slides' style='position: absolute; left: 0px; top: 0px; width: 980px; height: 400px; overflow: hidden;'>" +
                                        "<div>" +
                                            "<div style='position: absolute; width: 480px; height: 300px; top: 10px; left: 10px; text-align: left; line-height: 1.8em; font-size: 12px;'>" +
                                                "<br/>" +
                                                "<span style='display: block; line-height: 1em; text-transform: uppercase; font-size: 52px; color: #FFFFFF;'>tab view</span>" +
                                                "<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><div class='tryItButton'><a href='../Pages/Reports Dashboard Horizontal Slider.aspx?" + document.URL.split("?")[1] + "&IsWizard=1'>Try it!</a></div>" +
                                            "</div>" +
                                            "<img src='../Images/Tab View.png' style='position: absolute; top: 23px; left: 300px; width: 700px;'/>" +
                                            "<img u='thumb' src='../Images/Tab View Icon.png'/>" +
                                        "</div>" +
                                        "<div>" +
                                            "<div style='position: absolute; width: 480px; height: 300px; top: 10px; left: 10px; text-align: left; line-height: 1.8em; font-size: 12px;'>" +
                                                "<br/>" +
                                                "<span style='display: block; line-height: 1em; text-transform: uppercase; font-size: 52px; color: #FFFFFF;'>Table View</span>" +
                                                "<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><div class='tryItButton'><a href='../Pages/Reports Dashboard.aspx?" + document.URL.split("?")[1] + "&IsWizard=1'>Try it!</a></div>" +
                                            "</div>" +
                                            "<img src='../Images/Table View.png' style='position: absolute; top: 23px; left: 500px; width: 450px;'/>" +
                                            "<img u='thumb' src='../Images/Table View Icon.png'/>" +
                                        "</div>" +
                                        "<div>" +
                                            "<div style='position: absolute; width: 480px; height: 300px; top: 10px; left: 10px; text-align: left; line-height: 1.8em; font-size: 12px;'>" +
                                                "<br/>" +
                                                "<span style='display: block; line-height: 1em; text-transform: uppercase; font-size: 52px; color: #FFFFFF; width: 225px;'>Vertical Tab View</span>" +
                                                "<br/><br/><br/><br/><br/><div class='tryItButton'><a href='../Pages/Reports Dashboard Vertical Tabs.aspx?" + document.URL.split("?")[1] + "&IsWizard=1'>Try it!</a></div>" +
                                            "</div>" +
                                            "<img src='../Images/Vertical Tab View.png' style='position: absolute; top: 23px; left: 280px; width: 700px;'/>" +
                                            "<img u='thumb' src='../Images/Vertical Tab View Icon.png'/>" +
                                        "</div>" +
                                    "</div>" +
                                    "<span u='arrowleft' class='jssora07l' style='width: 50px; height: 50px; top: 123px; left: 8px;'></span>" +
                                    "<span u='arrowright' class='jssora07r' style='width: 50px; height: 50px; top: 123px; right: 8px'></span>" +
                                    "<div u='thumbnavigator' class='jssort04' style='position: absolute; width: 600px; height: 60px; right: 0px; bottom: 0px;'>" +
                                        "<div u='slides' style='bottom: 25px; right: 30px;'>" +
                                            "<div u='prototype' class='p' style='position: absolute; width: 62px; height: 32px; top: 0; left: 0;'>" +
                                                "<div class='w'>" +
                                                    "<div u='thumbnailtemplate' style='width: 100%; height: 100%; border: none; position: absolute; top: 0; left: 0;'></div>" +
                                                "</div>" +
                                                "<div class='c' style='position: absolute; background-color: #fff; top: 0; left: 0'>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>";
        $("#content").width(980).append(pickerDOM);

        var welcomeContent = "<h1>Welcome to the Business Intelligence reporting portal!</h1>" +
                            "<div>To begin, pick a dashboard view below, then press the 'Try it!' button. Once you have found the view you want to use, you will have the option to set it as your default.</div><br/>";
        $("#content").prepend(welcomeContent);

        var options = {
            $DragOrientation: 0,
            $ArrowNavigatorOptions: {
                $Class: $JssorArrowNavigator$,
                $ChanceToShow: 1,
                $AutoCenter: 2
            },
            $ThumbnailNavigatorOptions: {
                $Class: $JssorThumbnailNavigator$,
                $ChanceToShow: 2,
                $AutoCenter: 0,
                $SpacingX: 3,
                $SpacingY: 3,
                $DisplayPieces: 3,
                $ParkingPosition: 260,
                $DisableDrag: true
            }
        };

        var jssor_slider1 = new $JssorSlider$("slider1_container", options);
    },

    AddPreferenceButton: function () {
        var buttonContents = "<input type='button' value='Set as my default' onclick='RS.Actions.SavePreference()' style='font-size: 18px; background-color: black; color: white; margin-bottom: 1em;'></input>";
        var backButtonContents = "<input type='button' value='Back to view selection' onclick='RS.Actions.ReturnToWizardHome()' style='font-size: 18px; background-color: black; color: white; margin-bottom: 1em;'></input>";
        $("#content").prepend(backButtonContents).prepend(buttonContents);
    },

    InitializeSlider: function (ElementName, options) {
        var slider = new $JssorSlider$(ElementName, options);
        return slider;
    }
}

RS.Actions = {
    SavePreference: function(){
        RS.Part.loading("content", true, "center", "top+100px");
        RS.GetData.GetCurrentUser().done(function (items, status) {
            var userId = items.d.Id;
            RS.GetData.GetCurrentUserPreferences(userId).done(function (prefs, status) {
                $.ajax({
                    url: RS.GetData.appweburl + "/_api/contextinfo",
                    type: "POST",
                    headers: {
                        "accept": "application/json;odata=verbose",
                        "contentType": "text/xml"
                    }
                }).success(function (data) {
                    var itemData = {
                        '__metadata': { 'type': 'SP.Data.User_x0020_PreferencesListItem' },
                        'Title': items.d.Title,
                        'User1Id': userId,
                        'DisplayPreference': document.URL.split('?')[0]
                    };
                    if (prefs.d.results.length == 0) {
                        $.ajax({
                            url: RS.GetData.appweburl + "/_api/web/lists/getbytitle('User Preferences')/items",
                            type: "POST",
                            contentType: "application/json;odata=verbose",
                            data: JSON.stringify(itemData),
                            headers: {
                                "X-RequestDigest": data.d.GetContextWebInformation.FormDigestValue,
                                "Accept": "application/json;odata=verbose"
                            }
                        }).success(function () {
                            alert('Your preferences have been saved!');
                            RS.Part.doneLoading();
                        }).fail(function (status) {
                            alert('There was an error saving your preference. Please try again.');
                            RS.Part.doneLoading();
                        });
                    }
                    else {
                        itemData.__metadata.uri = prefs.d.results[0].__metadata.uri;
                        itemData.__metadata.etag = prefs.d.results[0].__metadata.etag;
                        $.ajax({
                            url: itemData.__metadata.uri,
                            type: "POST",
                            contentType: "application/json;odata=verbose",
                            data: JSON.stringify(itemData),
                            headers: {
                                "Accept": "application/json;odata=verbose",
                                "X-RequestDigest": data.d.GetContextWebInformation.FormDigestValue,
                                "X-HTTP-Method": "MERGE",
                                "If-Match": itemData.__metadata.etag
                            }
                        }).success(function () {
                            alert('Your preferences have been saved!');
                            RS.Part.doneLoading();
                        }).fail(function (status) {
                            alert('There was an error saving your preference. Please try again.');
                            RS.Part.doneLoading();
                        });
                    }
                }).fail(function (status) {
                    alert('There was an error saving your preferences. Please try again');
                    RS.Part.doneLoading();
                })
            }).fail(function (status) {
                alert('Failed to get the app configuration. Please check the configuration of BI Reports.');
                RS.Part.doneLoading();
            });
        }).fail(function (status) {
            alert('Failed to get the app configuration. Please check the configuration of BI Reports.');
            RS.Part.doneLoading();
        });
    },

    ReturnToWizardHome: function () {
        document.location = RS.GetData.appweburl + "/Pages/Initialization.aspx?" + document.URL.split("?")[1];
    },

    ResetView: function () {
        document.location = RS.GetData.appweburl + "/Pages/Initialization.aspx?" + document.URL.split("?")[1] + "&WizardOverride=1";
    }
}