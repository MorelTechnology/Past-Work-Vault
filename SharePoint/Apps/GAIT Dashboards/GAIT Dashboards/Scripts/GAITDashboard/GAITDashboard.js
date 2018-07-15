$(document).ready(function () {
    $("#dialog-message").dialog({
        modal: true,
        width: 200,
        closeOnEscape: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
            setTimeout("$('#dialog-message').dialog('close')", 3000);
        }
    });
    var cookieName = 'stickyTab',
        $tabs = $("#DashboardTabs"),
        $lis = $tabs.find('ul').eq(0).find('li');
    $tabs.tabs({
        active: ($.cookie(cookieName) || 0),
        activate: function (event, ui) {
            $.cookie(cookieName, $lis.index(ui.newTab));
            RS.GAIT.Styling.ResizeAppPart(true, ui.newPanel[0]);
        }
    });
    RS.GAIT.Data.GetHostListData("Initiative Actions").done(function (actions) {
        if (actions.d.results.length > 0) {
            $.each(actions.d.results, function (i, action) {
                var tileContents = "<div class='live-tile blue' id='tile" + i + "' data-mode='flip' stops='50%' data-stack='true'>" +
                    "<div style='background-image:url(" + action.BackgroundImageLocation.Url + ");background-repeat:no-repeat;background-size:contain;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p style='background-color:rgba( 0,0,0,0.6 );'>" + action.Title + "</p>" +
                        "</a>" +
                    "</div>" +
                    "<div style='background-color:navy;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p>" + action.Description + "</p>" +
                        "</a>" +
                    "</div>" +
                "</div>";
                $("#InitiativeActions").append(tileContents);
            });
        }
        $(".live-tile").liveTile({
            playOnHover: true,
            repeatCount: 0,
            delay: 0,
            startNow: false,
            onHoverOutDelay: -500
        });
    });
    RS.GAIT.Data.GetHostListData("Work Matter Actions").done(function (actions) {
        if (actions.d.results.length > 0) {
            $.each(actions.d.results, function (i, action) {
                var tileContents = "<div class='live-tile blue' id='tile" + i + "' data-mode='flip' stops='50%' data-stack='true'>" +
                    "<div style='background-image:url(" + action.BackgroundImageLocation.Url + ");background-repeat:no-repeat;background-size:contain;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p style='background-color:rgba( 0,0,0,0.6 );'>" + action.Title + "</p>" +
                        "</a>" +
                    "</div>" +
                    "<div style='background-color:navy;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p>" + action.Description + "</p>" +
                        "</a>" +
                    "</div>" +
                "</div>";
                $("#WorkMatterActions").append(tileContents);
            });
        }
        $(".live-tile").liveTile({
            playOnHover: true,
            repeatCount: 0,
            delay: 0,
            startNow: false,
            onHoverOutDelay: -500
        });
    });
    RS.GAIT.Data.GetHostListData("Admin Control Panel").done(function (actions) {
        if (actions.d.results.length > 0) {
            $.each(actions.d.results, function (i, action) {
                var tileContents = "<div class='live-tile blue' id='tile" + i + "' data-mode='flip' stops='50%' data-stack='true'>" +
                    "<div style='background-image:url(" + action.BackgroundImageLocation.Url + ");background-repeat:no-repeat;background-size:contain;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p style='background-color:rgba( 0,0,0,0.6 );'>" + action.Title + "</p>" +
                        "</a>" +
                    "</div>" +
                    "<div style='background-color:navy;'>" +
                        "<a class='full' onclick='RS.GAIT.Actions.OpenPopup(\"" + action.LinkLocation.Url + "\");' href='#'>" +
                            "<p>" + action.Description + "</p>" +
                        "</a>" +
                    "</div>" +
                "</div>";
                $("#AdminControlPanel").append(tileContents);
            });
        }
        $(".live-tile").liveTile({
            playOnHover: true,
            repeatCount: 0,
            delay: 0,
            startNow: false,
            onHoverOutDelay: -500
        });
    });
    RS.GAIT.Styling.ResizeAppPart(false, $("#DashboardTabs div.ui-tabs-panel[aria-hidden='false']")[0]);
});