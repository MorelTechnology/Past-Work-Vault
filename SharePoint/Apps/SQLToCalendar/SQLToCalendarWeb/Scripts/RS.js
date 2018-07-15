window.RS = window.RS || {}

RS.SP = RS.SP || {
    Context: null,
    Factory: null,
    RegisterContextAndProxy: function(appweburl){
        RS.SP.Context = new SP.ClientContext(appweburl);
        RS.SP.Factory = new SP.ProxyWebRequestExecutorFactory(appweburl);
        RS.SP.Context.set_webRequestExecutorFactory(RS.SP.Factory);
    }
}

RS.Common = RS.Common || {
    GetQueryStringParameter: function (paramToRetrieve) {
        // Function to retrieve a query string value.
        // For production purposes you may want to use
        //  a library to handle the query string.
        var params =
            document.URL.split("?")[1].split("&");
        var strParams = "";
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == paramToRetrieve)
                return singleParam[1];
        }
    },
    GetStandardTokens: function () {
        var standardTokens = "SPHostUrl=" + RS.Common.GetQueryStringParameter("SPHostUrl") + "&SPLanguage=" + RS.Common.GetQueryStringParameter("SPLanguage") + "&SPClientTag=" + RS.Common.GetQueryStringParameter("SPClientTag") +
        "&SPProductNumber=" + RS.Common.GetQueryStringParameter("SPProductNumber") + "&SPAppWebUrl=" + RS.Common.GetQueryStringParameter("SPAppWebUrl") + "&SPHostLogoUrl=" + RS.Common.GetQueryStringParameter("SPHostLogoUrl");
        return standardTokens;
    }
}

RS.BuildDOM = RS.BuildDOM || {
    CorrectRelativeLinks: function () {
        var appweburl = decodeURIComponent(RS.Common.GetQueryStringParameter("SPAppWebUrl"));
        var standardTokens = RS.Common.GetStandardTokens();
        $("a[href*='{SPAppWebUrl}']").each(function (i, link) {
            var currentLink = $(link).attr("href").substring($(link).attr("href").indexOf("{SPAppWebUrl}"));
            currentLink = currentLink.replace("{SPAppWebUrl}", appweburl);
            $(link).attr("href", currentLink);
        });
        $("a[href*='{StandardTokens}']").each(function (i, link) {
            $(link).attr("href", $(link).attr("href").replace("{StandardTokens}", RS.Common.GetStandardTokens()));
        });
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
    }
}

RS.Styling = RS.Styling || {
    RenderChrome: function () {
        // The Help, Account and Contact pages receive the 
        //   same query string parameters as the main page
        var web = RS.SP.Context.get_web();
        RS.SP.Context.load(web);
        RS.SP.Context.executeQueryAsync(function () {
            var options = {
                "appIconUrl": decodeURIComponent(RS.Common.GetQueryStringParameter("SPHostLogoUrl")),
                "appTitle": web.get_title(),
                // The onCssLoaded event allows you to 
                //  specify a callback to execute when the
                //  chrome resources have been loaded.
                "onCssLoaded": "RS.Styling.ChromeLoaded()"
            };

            var nav = new SP.UI.Controls.Navigation(
                                    "chrome_ctrl_placeholder",
                                    options
                                );
            nav.setVisible(true);
        }, function (sender, args) {
            var a = sender;
        });
    },
    ChromeLoaded: function () {
        // When the page has loaded the required
        //  resources for the chrome control,
        //  display the page body.
        $("body").show();
    },
    ApplyMasterPage: function () {
        RS.BuildDOM.loading("chrome_ctrl_placeholder", true, "center", "top");
        var hostweburl = decodeURIComponent(RS.Common.GetQueryStringParameter("SPHostUrl"));
        var appweburl = decodeURIComponent(RS.Common.GetQueryStringParameter("SPAppWebUrl"));
        var scriptbase = hostweburl + "/_layouts/15/";

        $.getScript(scriptbase + "SP.Runtime.js", function () {
            $.getScript(scriptbase + "SP.js", function () {
                $.getScript(scriptbase + "SP.RequestExecutor.js", function () {
                    RS.SP.RegisterContextAndProxy(appweburl);
                    $.getScript(scriptbase + "init.js", function () {
                        $.getScript(scriptbase + "SP.UI.Controls.js", function () {
                            $.getScript(scriptbase + "SP.core.js", function () {
                                RS.Styling.RenderChrome();
                                RS.BuildDOM.doneLoading();
                            });
                        });
                    });
                });
            });
        });
    },
    ApplyMetroTiles: function (controls) {
        var $tiles = $(controls).liveTile({
            playOnHover: true,
            repeatCount: 0,
            delay: 0,
            initDelay: 0,
            startNow: false,
            animationComplete: function (tileData) {
                $(this).liveTile("play");
                tileData.animationComplete = function () { };
            }
        }).each(function (idx, ele) {
            var delay = idx * 1000;
            $(ele).liveTile("play", delay);
        });
    },
    RenderCalendar: function (controlId) {
        if ($("#eventsJsonArray").val() != null && $("#eventsJsonArray").val() != "") {
            var eventsJson = JSON.parse($("#eventsJsonArray").val());
            var events = [];
            $(eventsJson).each(function () {
                events.push({
                    title: $(this).attr("Subject"),
                    start: $(this).attr("StartTime"),
                    end: $(this).attr("EndTime"),
                    allDay: $(this).attr("IsAllDayEvent"),
                    department: $(this).attr("Department")
                });
            });

            $("#" + controlId).fullCalendar({
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                views: {
                    agenda: {
                        minTime: '08:00:00',
                        maxTime: '18:00:00'
                    }
                },
                defaultView: 'month',
                eventLimit: 8,
                events: events,
                windowResize: function (view) {
                    RS.Styling.ResizeAppPart();
                },
                eventRender: function (event, element) {
                    switch (event.department) {
                        case "Acquisitions":
                            $(element).css("background-color", "#335ba5");
                            $("#legendAcquisitions").show();
                            break;
                        case "Actuarial":
                            $(element).css("background-color", "#0d830e");
                            $("#legendActuarial").show();
                            break;
                        case "Claims":
                            $(element).css("background-color", "#1793ea");
                            $("#legendClaims").show();
                            break;
                        case "Commutations":
                            $(element).css("background-color", "#00282c");
                            $("#legendCommutations").show();
                            break;
                        case "EXCO":
                            $(element).css("background-color", "#d11202");
                            $("#legendEXCO").show();
                            break;
                        case "Finance":
                            $(element).css("background-color", "#94b111");
                            $("#legendFinance").show();
                            break;
                        case "Human Resources":
                            $(element).css("background-color", "#eb17ae");
                            $("#legendHumanResources").show();
                            break;
                        case "Office of General Counsel":
                            $(element).css("background-color", "#bb2170");
                            $("#legendOfficeOfGeneralCounsel").show();
                            break;
                        case "Operations":
                            $(element).css("background-color", "#597628");
                            $("#legendOperations").show();
                            break;
                        case "Reinsurance":
                            $(element).css("background-color", "#a97b9e");
                            $("#legendReinsurance").show();
                            break;
                    }
                },
                eventMouseover: function (calEvent, jsEvent) {
                    var bgColor = $(jsEvent.currentTarget).css('background-color');
                    var color = $(jsEvent.currentTarget).css('color');
                    var tooltip = '<div class="tooltipevent" style="width:300px;padding:10px;background:' + bgColor + ';color:' + color + ';border:2px solid black;position:absolute;z-index:10001;">' +
                                        '<b>' + calEvent.title + '</b><br/>' +
                                        'Start: ' + moment(calEvent.start._i).format("dddd, MMMM Do YYYY, h:mm:ss a") + '<br/>' +
                                        'End: ' + moment(calEvent.end._i).format("dddd, MMMM Do YYYY, h:mm:ss a") + '<br/>' +
                                        'Department: ' + calEvent.department +
                                  '</div>';
                    $("body").append(tooltip);
                    $(this).mouseover(function (e) {
                        $(this).css('z-index', 10000);
                        $('.tooltipevent').fadeIn('500');
                        $('.tooltipevent').fadeTo('10', 1.9);
                    }).mousemove(function (e) {
                        $('.tooltipevent').position({
                            my: 'left+20 top+10',
                            at: "right bottom",
                            of: e
                        });
                    });
                },

                eventMouseout: function (calEvent, jsEvent) {
                    $(this).css('z-index', 8);
                    $('.tooltipevent').remove();
                }
            });

            RS.Styling.ResizeAppPart();
        }
        else RS.BuildDOM.doneLoading();
    },
    ResizeAppPart: function () {
        if (window.parent == null)
            return;
        
        var hostUrl = decodeURIComponent(RS.Common.GetQueryStringParameter("SPHostUrl"));
        var senderId = RS.Common.GetQueryStringParameter("SenderId");
        var docWidth = $(document).width();
        docWidth = "100%";
        var docHeight = $("#content").height() + 20;
        docHeight = $("body").height();
        var message = "<Message senderId=" + senderId + ">resize(" + docWidth + "," + docHeight + ")</Message>";
        window.parent.postMessage(message, "*");
    }
}