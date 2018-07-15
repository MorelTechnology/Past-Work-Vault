$(document).ready(function () {
    RegisterCss();
    BuildQuickLaunch();
    UpdateLinkTokens();
    SetCurrentNav();
});

function RegisterCss() {
    if ($("link[href$='Content/ItemForm.css']").length == 0) {
        $('head').append('<link rel="Stylesheet" type="text/css" href="' + _spPageContextInfo.webServerRelativeUrl + '/Content/ItemForm.css" />');
    }
}

function BuildQuickLaunch() {
    if ($("[id$=quickLaunch]").length == 0) {
        $("#DeltaPlaceHolderLeftNavBar").append('<div class="RivernetQuickLaunch" id="ctl00_PlaceHolderLeftNavBar_quickLaunch" style="float: left;"><ul tabindex="0" class="level1 static" role="menu" style="width: auto; float: left; position: relative;"></ul></div>');
    }
    var quickLaunchContainer = $("[id$=quickLaunch]");
    var ul = quickLaunchContainer.find("ul").empty();
    var home = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level1 static" href="{SPAppWebUrl}/Pages/Default.aspx?{StandardTokens}">Home</a></li>').appendTo(ul);
    var projects = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Projects/AllItems.aspx?{StandardTokens}">Projects</a></li>').appendTo(ul);
    var activities = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Activities/AllItems.aspx?{StandardTokens}">Activities</a></li>').appendTo(ul);
    var notes = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Notes/AllItems.aspx?{StandardTokens}">Notes</a></li>').appendTo(ul);
    var commutationDocuments = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Lists/Commutation%20Documents/Forms/AllItems.aspx?{StandardTokens}">Commutation Documents</a></li>').appendTo(ul);
    var contacts = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Lists/Contact%20Lookup/AllItems.aspx?{StandardTokens}">Contacts</a></li>').appendTo(ul);
    var completedProjects = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Projects/Completed%20Projects.aspx?{StandardTokens}">Completed Projects</a></li>').appendTo(ul);
    var droppedProjects = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Projects/Dropped%20Projects.aspx?{StandardTokens}">Dropped Projects</a></li>').appendTo(ul);
    //var completedDroppedProjects = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Projects/CompletedDropped%20Projects.aspx?{StandardTokens}">Completed/Dropped Projects</a></li>').appendTo(ul);
    var activitiesCreatedByMe = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPAppWebUrl}/Pages/Activities/Created%20By%20Me.aspx?{StandardTokens}">Activities Created by Me</a></li>').appendTo(ul);
    var reports = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="{SPHostUrl}/Commutation%20Central%20Reports/Forms/Gallery.aspx">Reports</a></li>').appendTo(ul);
    var quickActions = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level1 static">Quick Actions</a></li>').appendTo(ul);
    var addCommutation = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" href="javascript:void(0)" onclick="NewProject()">Add Commutation</a></li>').appendTo(ul);
    var addActivity = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" onclick="NewActivity()" href="javascript:void(0)">Add Activity</a></li>').appendTo(ul);
    var addNote = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level2 static" onclick="NewNote()" href="#">Add Note</a></li>').appendTo(ul);
    //var managerActions = $('<li class="static" role="menuitem" style="position: relative;"><a tabindex="-1" class="level1 static">Manager Actions</a></li>').appendTo(ul);
}

// Return a query string parameter
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

// Update any anchor tags that have the {StandardTokens} parameter
function UpdateLinkTokens() {
    $("a[href *= '{StandardTokens}']").each(function (i, a) {
        $(a).attr("href",
            $(a).attr("href").replace("{StandardTokens}", GetStandardTokens()));
    });
    $("a[href *= '{SPAppWebUrl}']").each(function (i, a) {
        $(a).attr("href",
            $(a).attr("href").replace("{SPAppWebUrl}", decodeURIComponent(getQueryStringParameter("SPAppWebUrl"))));
    });
    $("a[href *= '{SPHostUrl}']").each(function (i, a) {
        $(a).attr("href",
            $(a).attr("href").replace("{SPHostUrl}", decodeURIComponent(getQueryStringParameter("SPHostUrl"))));
    });
}

// Get the list of standard querystring tokens
function GetStandardTokens() {
    var standardTokens = "SPHostUrl=" + getQueryStringParameter("SPHostUrl") + "&SPLanguage=" + getQueryStringParameter("SPLanguage") + "&SPClientTag=" + getQueryStringParameter("SPClientTag") +
        "&SPProductNumber=" + getQueryStringParameter("SPProductNumber") + "&SPAppWebUrl=" + getQueryStringParameter("SPAppWebUrl") + "&SPHostLogo=" + getQueryStringParameter("SPHostLogo");
    return standardTokens;
}

// Set the currently selected navigation item
function SetCurrentNav() {
    var quickLaunchContainer = $("[id$=quickLaunch]");
    var ul = quickLaunchContainer.find("ul");
    ul.find("a[href *= 'http']").each(function (i, a) {
        var searchString = $(a).attr('href').toLowerCase().substring($(a).attr('href').indexOf("/CommutationCentralListBased"), $(a).attr('href').indexOf(".aspx"));
        if (document.URL.toLowerCase().indexOf(searchString) > -1 && searchString.length > 0) {
            $(a).addClass("selected");
        }
    });
}

function NewProject() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Projects/NewItem.aspx?IsDlg=1&SPAppWebUrl=" + getQueryStringParameter("SPAppWebUrl"),
        title: 'Add New Commutation',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}

function NewActivity() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Activities/NewItem1.aspx?IsDlg=1&SPAppWebUrl=" + getQueryStringParameter("SPAppWebUrl"),
        title: 'Add New Activity',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}

function NewNote() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Notes/NewItem1.aspx?IsDlg=1&SPAppWebUrl=" + getQueryStringParameter("SPAppWebUrl"),
        title: 'Add New Note',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
}