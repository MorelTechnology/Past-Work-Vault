function afterInit() {
  // show the assigned to button if the current user sees more than their own sites
  setAssignedToButton();
  // redraw the table anytime the window is resized.
  $(this).resize(function(){ $("#MyMatters").tabulator("redraw"); });
  // Show the loading indicator (makes for nicer refreshes.)
  $(this).on('beforeunload', function(){
        waitingDialog.show("One Moment...");
    });
}
function GetSiteDecorators(data){
  switch(tryParseBoolean(data.isLinkedMatter, false))
  {
    case true:
      return "Linked" + " | " + data.Title;
      break;
    case false:
      if(!isEmpty(data.Work_Matter_Type))
      {
        return "Non-Linked" + " | " + data.Title;
      }
    default:
    return data.Title;
  }
}
function isEmpty(value) {
    return (value == null || value.length === 0);
}
function openExplorer() {
	// Specifically used for SharePoint Web Part to call Modal for full-table view
	var opt = {url: '/auxiliaryservices/mymatters', title: 'My Matters Explorer', allowMaximize:true, allowClose:true, showMaximized:true};
	SP.SOD.execute('sp.ui.dialog.js','SP.UI.ModalDialog.showModalDialog', opt);
}
function Refresh(){
  clientSettings("myMattersData", null);
  console.log("Purged Stored Data and will now refresh with a new request.");
  location.reload();
}
function setAssignedToButton(){
  if (clientSettings("myMatters_Managers").Total > 2)
  {
    // More than two managers.  Set a dropdown button to select managers.
    $(".conditionalButtons").html(
    "<span class='dropdown'>\
      <button type='button' class='btn btn-primary btn-sm dropdown-toggle' data-toggle='dropdown'>\
        <span class='glyphicon glyphicon-user'></span> Assigned To... <span class='caret'></span>\
      </button>\
      <ul class='dropdown-menu columns'>\
        <button class='btn btn-primary btn-sm' onclick='$(\"#MyMatters\").tabulator({groupBy: \"Litigation_Manager\"});'>\
          <span class='glyphicon glyphicon-list'></span> Group by assigned</button>\
        <li role='separator' class='divider'></li>\
        <li role='separator' class='divider afterManagerLinks'></li>\
        <button class='btn btn-primary btn-sm' onclick='$(\"#MyMatters\").tabulator(\"clearFilter\");'>\
          <span class='glyphicon glyphicon-ban-circle'></span> Reset to default</button>\
      </ul>\
    </span>");


    $.each(clientSettings("myMatters_Managers").Names, function(index, value){
              $(".afterManagerLinks")
              .before("<li onclick='$(\"#MyMatters\").tabulator(\"setFilter\", \"Litigation_Manager\", \""+ value +"\");'><a>" +
              value + "</a></li>");
              });
  }
}
function showDetailDialog(row) {
  // Loop through all the columns used in the table
  // in order to get the friendly name, then write the value of the current row.
  var col = $("#MyMatters").tabulator("getColumns");
  var val = "";
  var htmlMessage = "";
  $.each(col, function( key, value){
    //alert(value.title + " " +row[value.field] );
    if(row[value.field].indexOf("http") == 0){
      val = "<a href = '"+ row[value.field] + "' target='_blank'>" + row[value.field] + "</a>";
    }else {
      val = row[value.field];
    }

    htmlMessage += "<b>" + value.title + ":</b>" + " " + val + "<br>";
  });
  BootstrapDialog.show({
      title: 'Details for ' + row.Title,
      message: htmlMessage
  });
}
function showHelpDialog() {
var html = '<h3>How do I use this thing?</h3>\
This table shows a list of the the Litigation Matter and Project sites \
to which you have access.\
<ul><li><b>Pick <u>your</u> columns.</b> Using the "Show/Hide Columns" button, you can toggle the visibility of \
available site data, in order to create an effective "at-a-glance" view for yourself. One word of note: each additional column displayed \
adds a small delay to the total time it takes to render the data. <i>(Have you checked out what happens when you click a site\'s title?)</i> \
<li><b>Order things <u>your</u> way.</b> Drag columns and drop them where you want them.  The position will be remembered.  If \
you make a mistake, you can reset things by selecting the reset option in the "Show/Hide Columns" drop-down menu.\
<li><b>Find stuff. <i>Fast</i>.</b> Know part of an account name or have the Matter number handy? \
Use the Search Filters tool to perform a "live" text search on any column in your display.\
<li><b>Get Updates.</b> While the data in the table will always be from today, we cache it in your browser in order \
to speed up the site.  If you\'re not in a rush, click the "Force Data Refresh" to trade a few second delay for the freshest table data.\
<li><b>Need More Help?</b> No worries.\
<a href = "https://servicedesk.trg.com/WorkOrder.do?title=I%20need%20help%20with%20the%20Litigation%20Management%20Site&priority=5180000000006801&requestType=5180000000008391" target="_blank">\
<b>Submit a ticket with the Service Desk</b></a> and we\'ll be happy to assist!</ul>';

BootstrapDialog.show({
  title: 'A Quick Overview',
  message:html

});
}
function showFilterDialog() {
  //var htmlMessage = "Custom Filters...";
  //var htmlMessage =+ "<button type='button' class='btn btn-primary' onclick='$(\"#MyMatters\").tabulator(\"clearFilter\");'><span class='glyphicon glyphicon-remove'></span> Reset Filters</button>"
  var html = '<label>Field: </label>\
              <select id="filter-field">\
                  <option value="Litigation_Manager">Lit Manager</option>\
              </select>\
              <label>Type: </label>\
              <select id="filter-type">\
                  <option value="=">=</option>\
                  <option value="<">&lt;</option>\
                  <option value="<=">&lt;=</option>\
                  <option value=">">&gt;</option>\
                  <option value=">=">&gt;=</option>\
                  <option value="!=">!=</option>\
                  <option value="like">like</option>\
              </select>\
            <label>Value: </label>\
            <input id="filter-value" type="text" placeholder="value to filter"><br>'
            html = html + "<button onclick = '$(\"#MyMatters\").tabulator(\"setFilter\", $(\"#filter-field\").val(), $(\"#filter-type\").val(), $(\"#filter-value\").val());'> Set Filter</button>"
            html = html + "<button onclick = '$(\"#MyMatters\").tabulator(\"clearFilter\");'>Clear Filter</button>"
  BootstrapDialog.show({
      title: 'Custom Filter Settings',
      message: html
  });
}
function showModal(title,html,buttons) {
  BootstrapDialog.show({ title: title, message: html, buttons:buttons});
}
function storeManagers() {
  // Clear if it exists
  clientSettings("myMatters_Managers", myMatters_Managers);
  var tempData=clientSettings("myMattersData");
  var map = {};
  tempData.map(function(tempData){map[tempData.Litigation_Manager] = undefined;});

  var myMatters_Managers = { Total: (Object.keys(map).length), Names: (Object.keys(map)) };
  $.each(myMatters_Managers.Names, function(key,value){
    if (value == "undefined") myMatters_Managers.Names[key] = "[Not Assigned]";});
  myMatters_Managers.Names.sort();
  clientSettings("myMatters_Managers", myMatters_Managers);
}
function toggleDisplayOptions() {
  $('#displayOptionsPanel').slideToggle('400');
}
function toggleField(fieldId) {
  //$('#MyMatters').tabulator('toggleCol', fieldId);
  $('#MyMatters').tabulator('toggleCol', fieldId).delay(5000).tabulator('redraw');
}
function toggleSearch() {
  $(".tabulator-col").toggleClass('showSearch', '800');
}
function tryParseBoolean(value, defaultBoolean) {
    if (typeof (value) == "boolean")
    { return value; }
    else if (typeof (value) == "string")
    {
      try {
        var result = JSON.parse(value.toLowerCase());
        return result;
          }
      catch (e)
      { console.log('Error running function \'tryParseBoolean(value)\', processing a value of: \''
      + value + '\'.  Assigned default boolean value of \'' + defaultBoolean +
      '\'. (Exception thrown was: \'' + e + '\'.)'); return defaultBoolean;}
    }
    else { return defaultBoolean; }
}

/** Deprecated, keeping until each is regression tested *******
function showDetailDialogx(data) {
  var isLinkedMatter = data.isLinkedMatter == 'true';
  var html = document.createElement('div');
  html.innerHTML = '<h1>' + data.Title + '</h1>';
  if (isLinkedMatter)
  { html.innerHTML = html.innerHTML +
    '<table style="margin:auto; border-collapse:collapse;\
     border: 2px solid #dddddd; text-align:center;"><tr style\
    = "text-align:center; background-color:#99ccff;"><th style\
    = "padding:10px;">Litigation<br>Matter Number</th>\
    <th style = "padding:10px;">Matter Created<br>on</th><th\
     style = "padding:10px;">Last Synchronized<br>with ClaimCenter</th>\
     </tr><tr style = "text-align:center; background-color:lightyellow;">\
     <td>' + data.Matter_Number + '</td>\
     <td>' + data.Site_Created + '</td>\
     <td>' + data.Last_Synchronized +'</td>\
     </tr></table>';
  }
  html.innerHTML = html.innerHTML + '\
  <b>Account Name:</b> ' + data.Account_Name + '<br>\
  <b>Affiliate:</b> ' + data.Affiliate + '<br>\
  <b>Case Caption:</b> ' + data.Case_Caption+ '<br>\
  <b>Country:</b> ' + data.Country+ '<br>\
  <b>Docket Number:</b> ' + data.Docket_Number+ '<br>\
  <b>Last Modified:</b> ' + data.Last_Modified+ '<br>\
  <b>Last Synchronized:</b> ' + data.Last_Synchronized+ '<br>\
  <b>Litigation Manager:</b> ' + data.Litigation_Manager+ '<br>\
  <b>Litigation Type:</b> ' + data.Litigation_Type+ '<br>\
  <b>Matter Name:</b> ' + data.Matter_Name+ '<br>\
  <b>Matter Number:</b> ' + data.Matter_Number+ '<br>\
  <b>Matter Status:</b> ' + data.Matter_Status+ '<br>\
  <b>Site Created:</b> ' + data.Site_Created+ '<br>\
  <b>State Filed:</b> ' + data.State_Filed+ '<br>\
  <b>Venue:</b> ' + data.Venue+ '<br>\
  <b>Work/Matter type:</b> ' + data.Work_Matter_Type+ '<br>\
  <b>Is linked:</b> ' + data.isLinkedMatter + '<br>\
  <b>Direct Link:</b> <a href="' + data.Url + '">'+data.Url+'</a>'+ '<br><br>\
  <button onclick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK); return false;">Close</button>';
  OpenPopUpPageWithDialogOptions(
  {
    title: "Details for... ",
    html:html,
    dialogReturnValueCallback: function(dialogResult)
    {
    //alert(dialogResult); //Add your custom code here.
    //If you want to refresh your page base on the dialog result , OK=1, cancel = 0, run the following
    //SP.UI.ModalDialog.RefreshPage(dialogResult);
    }
  });
}
function getQueryStringValue(paramName) {
    var params = document.URLUnencoded.split("?")[1].split("&");
    var strParams = "";
    for (var i = 0; i < params.length; i++) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramName)
            return decodeURIComponent(singleParam[1]);
    }
}
function getRootUrl(url) {
    var urlParts = url.split("/");
    return urlParts[0] + "//" + urlParts[2];
}
function initPanels() {
    $(document).on('click', '.panel-heading span.clickable', function(e) {
        var $this = $(this);
        if (!$this.hasClass('panel-collapsed')) {
            $this.parents('.panel').find('.panel-body').slideUp();
            $this.addClass('panel-collapsed');
            $this.find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
        } else {
            $this.parents('.panel').find('.panel-body').slideDown();
            $this.removeClass('panel-collapsed');
            $this.find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
        }
    });
}
function ResizeAppPart(width,height) {
    var w = width;
    var h = height;
    window.parent.postMessage("<message senderId=" + getQueryStringValue("SenderId") + ">resize(" + w + "," + h + ")</message>", "*");
}
function ResizeAppPartDynamic() {
    var w = "100%"; // or some other dynamic way to get the width
    var h = $(document).height(); // or the height of your wrapper in the app part
    window.parent.postMessage("<message senderId=" + getQueryStringValue("SenderId") + ">resize(" + w + "," + h + ")</message>", "*");
}
***************************************************************/
