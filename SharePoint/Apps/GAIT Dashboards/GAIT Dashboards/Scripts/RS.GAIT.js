window.RS = window.RS || {}
RS.GAIT = RS.GAIT || {}
RS.GAIT.Common = RS.GAIT.Common || {
    GetQueryStringParameter: function (param) {
        var params = document.URL.split("?")[1].split("&");
        var strParams = "";
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == param) {
                return singleParam[1];
            }
        }
    },
    DisplayErrorMessage: function (message) {
        if ($("#ErrorSection").length) {
            $("#ErrorSection").text(message).show();
        }
        else {
            alert(message);
        }
    },
    AddQueryStringToGlobalLinks: function () {
        var querystring = document.URL.split("?")[1];
        $.each($(".globallink"), function (index, link) {
            if (link.href.indexOf("?") < 0) {
                $(link).attr("href", link.href + "?" + querystring);
            } else {
                $(link).attr("href", link.href + "&" + querystring);
            }
        });
        $.each($(".relativeiframe"), function (index, iframe) {
            if (iframe.src.indexOf("?") < 0) {
                $(iframe).attr("src", iframe.src + "?" + querystring);
            } else {
                $(iframe).attr("src", iframe.src + "&" + querystring);
            }
        });
    }
}
RS.GAIT.Data = RS.GAIT.Data || {
    appweburl: decodeURIComponent(RS.GAIT.Common.GetQueryStringParameter("SPAppWebUrl")),
    hostweburl: decodeURIComponent(RS.GAIT.Common.GetQueryStringParameter("SPHostUrl")),
    
    GetHostListData: function (listName, select, expand, filter, orderby, top) {
        var endpointUrl = this.appweburl + "/_api/SP.AppContextSite(@target)/web/lists/GetByTitle('" + listName + "')/Items?@target='" + this.hostweburl + "'";
        if (select) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$select=" + select;
        }
        if (expand) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$expand=" + expand;
        }
        if (filter) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$filter=" + filter;
        }
        if (orderby) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$orderby=" + orderby;
        }
        if (top) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$top=" + top;
        }
        return $.ajax({
            url: endpointUrl,
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
    },
    CompileAjaxRequestUrl: function (listName, select, expand, filter, orderby) {
        var endpointUrl = this.appweburl + "/_api/SP.AppContextSite(@target)/web/lists/GetByTitle('" + listName + "')/Items?@target='" + this.hostweburl + "'";
        if (select) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$select=" + select;
        }
        if (expand) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$expand=" + expand;
        }
        if (filter) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$filter=" + filter;
        }
        if (orderby) {
            if (endpointUrl.indexOf("?") < 0) { endpointUrl += "?" } else { endpointUrl += "&" };
            endpointUrl += "$orderby=" + orderby;
        }
        return endpointUrl;
    }
}

RS.GAIT.Actions = RS.GAIT.Actions || {
    OpenPopup: function (url) {
        var scriptbase = RS.GAIT.Data.hostweburl + "/_layouts/15/";
        $.getScript(scriptbase + 'SP.Runtime.js', function () {
            $.getScript(scriptbase + 'SP.js', function () {
                $.getScript(scriptbase + 'SP.RequestExecutor.js', function () {
                    var messageToPost = { message: "", senderId: RS.GAIT.Common.GetQueryStringParameter("SenderId") };
                    messageToPost.message = "SP.UI.ModalDialog.showModalDialog";
                    messageToPost.options = {
                        url: url,
                        dialogReturnValueCallback: function (dialogResult) {
                            if (dialogResult != SP.UI.DialogResult.cancel) {
                                SP.UI.ModalDialog.RefreshPage(dialogResult)
                            }
                        }
                    };
                    window.parent.postMessage(JSON.stringify(messageToPost), document.referrer);
                });
            });
        });
    }
}

RS.GAIT.Styling = RS.GAIT.Styling || {
    ResizeAppPart: function (forceResize, sender) {
        /// <summary>Resizes the client web part, optionally adjusting the height an width to accomodate extra space</summary>
        /// <param name="resizeHeight" type="Boolean">Optionally adds an additional 20px to the web part's height. Default is false</param>
        /// <param name="resizeWidth" type="Boolean">Optionally adds an additional 20px to the web part's width. Default is false</param>
        /// <returns type="Void"/>

        if (window.parent == null)
            return;

        var senderId = RS.GAIT.Common.GetQueryStringParameter("SenderId");
        senderId = senderId.replace('#', '');
        var docWidth, docHeight;
        if (sender && sender.className.indexOf("dashboard") > -1 && (forceResize || sender.attributes["aria-hidden"].value == "false")) {
            docWidth = Math.round($(sender).find("table.display").width()) + Math.round($(".maincontent .ui-tabs-panel").css("padding-left").replace(/[^-\d\.]/g, '')) + Math.round($(".maincontent .ui-tabs-panel").css("padding-right").replace(/[^-\d\.]/g, ''));
            docHeight = Math.round($(sender).parent("div").outerHeight());
        } else if (sender && sender.className.indexOf("controlpanel") > -1 && (forceResize || sender.attributes["aria-hidden"].value == "false")) {
            docWidth = Math.round($(sender).outerWidth());
            docHeight = Math.round($(sender).parent("div").outerHeight());
        }
 
        var message = "<Message senderId=" + senderId + ">resize(" + docWidth + "," + docHeight + ")</Message>";
        if (docWidth > 0 && docHeight > 0) {
            window.parent.postMessage(message, "*");
        }
    }
}