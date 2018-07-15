function LoadAnyListView(sourceSiteUrl, listName, listViewTitle, displayLocation,
    openLinksInNewWindow, normalizeTable, collapseGroups, showRefreshIcon)
{
    if (!window.jQuery) //JQuery Check
    { document.getElementById("A2O_Webpart_Status").innerHTML = ("<b style='color:red;'>Warning: JQuery Required. </b><br />" +
            "This page does not appear to have a JQuery reference.  Unless you plan to add one elsewhere, be sure to select " +
            "<em>Include JQuery Reference</em> in this webpart's settings.");
      throw new Error("Missing JQuery Reference."); }

    var thisWebPart = "#" + displayLocation;

    var promise = $.ajax({
        url: sourceSiteUrl + "/_api/web/lists/getbytitle('"
            + listName + "')/views/getbytitle('"
            + listViewTitle + "')/renderAsHtml()",
        type: "GET",
        dataType: "json", 
        headers: {
            Accept: "application/json;odata=verbose"
        }
    });
    promise.done(function (data, textStatus, jqXHR) {
        var strHtml = data.d.RenderAsHtml;

        // fix any malformed hyperlinks possibly contained in the view
        strHtml = strHtml
            .replace(/&lt;/g, "<")
            .replace(/&gt;/g, ">")
            .replace(/&#39;/g, "'");

        // insert the html into the webpart
        $(thisWebPart).fadeOut();
        $(thisWebPart).html(strHtml);
        $(thisWebPart).fadeIn();

        // Simplify the empty view message, (remove reference to adding an item.)
        $(thisWebPart + " table td.ms-vb:contains('There are no items')").text("There are no items to display.");

        // process webpart options
        if (openLinksInNewWindow) { setOpenLinks(thisWebPart); }
        if (normalizeTable) { setNormalizeTable(thisWebPart); }
        if (collapseGroups) { setCollapseGroups(thisWebPart); }
        // insert a refresh link, (default hidden, optionally display it.)
        setRefreshLink(sourceSiteUrl, listName, listViewTitle, displayLocation, openLinksInNewWindow,
            normalizeTable, collapseGroups, showRefreshIcon, thisWebPart);

    });
    promise.fail(function (jqXHR, textStatus, errorThrown) {
        $(thisWebPart).html("<b>Error:</b> Please check webpart configuration settings.<br /><br />" +
            "<b>Additional information:</b> " + jqXHR.status + " " + errorThrown + ".<br /><em>"
            + jqXHR.responseText + "</em><br /><br />" +
            "<b>This Webpart element id:</b> "+thisWebPart);
    });
}

function setOpenLinks(thisWebPart)
{
    $(thisWebPart + ' a').attr('target', '_blank'); // Set all links to open in a new window.
    $(thisWebPart + " a[href*='javascript:']").attr('target', '_self'); // reset target on javascript 
                                                                        // links to this window, or they break! 
}

function setNormalizeTable(thisWebPart)
{
    var webpartTitleElement = thisWebPart.replace("#", "#WebPart") + "_ChromeTitle";

    $(thisWebPart).prepend('<style>' + thisWebPart + ' .ms-viewheadertr,.ms-vb-firstCell{display:none;} ' +
      thisWebPart + ' .ms-listviewtable .ms-itmhover {height: 0px!important;} ' +
      thisWebPart + ' .ms-listviewtable {width: auto; margin: 5px;} ' +
      thisWebPart + ' .ms-vb2 img {display: block; margin: auto;} ' +
      // Move Webpart title out of viewport, but don't hide it, since other page elements need to interact with it.
      webpartTitleElement + ' { position: absolute!important; top: -9999px; left: -9999px;} </style>');
}

function setCollapseGroups(thisWebPart)
{
    // click all collapse links.
     $(thisWebPart).find(' a[onclick*=ExpCollGroup]:even').each(function () { this.onclick(); });

    // Alternate methods for debugging
    // $(thisWebPart + " img[src*='minus.gif']").click() // click all the minus buttons in this webpart.
    // $("img[src*='minus.gif']").click() // click all the minus buttons on this page.
    // $(thisWebPart + ' img[alt="Expand/Collapse"]').click();  // sometimes the alt tag isn't set as expected(?)

}

function setRefreshLink(sourceSiteUrl, listName, listViewTitle, displayLocation, openLinksInNewWindow,
    normalizeTable, collapseGroups, showRefreshIcon, thisWebPart)
{
    var styleSettings;
    if (showRefreshIcon)
    {
        //styleSettings = "'position:absolute; left:98%;'";
        //styleSettings = "'margin-right: -10px; margin-bottom: -8px; float: right;'";
        styleSettings = "'display:block; float:right;'";
    }
    else
    {
        styleSettings = "'display:none;'";
    }
    var refreshHtml = "<a style=" + styleSettings + " onclick=\x22LoadAnyListView(" +
        "'" + sourceSiteUrl + "'" + ", " + "'" + listName + "'" + ", " +
        "'" + listViewTitle + "'" + ", " + "'" + displayLocation + "'" + ", " +
        openLinksInNewWindow + ", " + normalizeTable + ", " + collapseGroups + ", " + showRefreshIcon + ");\x22>" +
        "<img id='ManualRefresh' src='../_layouts/15/images/CrossSiteListView/view_refresh.png'></a>";
    $(thisWebPart).prepend(refreshHtml);
}
