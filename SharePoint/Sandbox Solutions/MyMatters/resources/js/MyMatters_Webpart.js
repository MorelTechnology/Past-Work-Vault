var environment = config.env.qa;
var clientSettings = config.env.storageAgent;
$(document)
.bind("ajaxSend", function() {
  waitingDialog.show("Getting New Data...",{percentComplete:"25%"});
  setTimeout(waitingDialog.show("Getting New Data...",{percentComplete:"50%"}) , 2000);
})
.bind("ajaxComplete", function(){ waitingDialog.hide(); });

$(function() {
    waitingDialog.show("One Moment...");
    init();
    afterInit();

	});

function init() {
    if (!isEmpty(clientSettings("myMattersData"))) { // Build table with Cached Data
        console.log('Using data from cache...');
        buildTable(clientSettings("myMattersData"));
    } else { // Cache was empty, get data from rest service, cache it, then build table.
        $.ajaxSetup({ xhrFields: { withCredentials: true } });
        console.log('Calling WebService at: ' + environment.restServiceUrl + '...');
        $.post(environment.restServiceUrl, {
                "Url": environment.litManSiteUrl,
                "ExcludeKeysWhichContain": environment.excludedKeyPatterns
            })
            .done(function(data) {
                // this block used to ensure localStorage cache isn't full.
                var i = 0;
                while (true) {
                    try {
                        clientSettings("myMattersData", data, {
                            expires: (environment.dataCacheInHours * 3600000)
                        });
                        break;
                    } catch (error) {
                        if (++i == 2) throw e;
                        console.log('Error while trying to cache data.  Will purge cache and retry.')
                        clientSettings("myMattersData", null); // purge cache, then attempt to re-store the data.
                        clientSettings("myMattersData", data, {
                            expires: (environment.dataCacheInHours * 3600000)
                        });
                        console.log('Looks like that did the trick!')
                        break;
                    }
                }
                console.log('Data will be stored in cache for the next ' + environment.dataCacheInHours + ' hours.');
                buildTable(data);
            });
    }
}

function buildTable(data) {
    //Build Table
    /** Typical Order of Events:
    1. tableBuilding, 2. dataLoading, 3. dataLoaded, 4. renderStarted,
    5. renderComplete, 6. tableBuilt
    In this implementation, 3 and 4 are related to ajax functionality which is handled external to
    tabulator.
    **/
    waitingDialog.show("Rendering data...",{percentComplete:"50%"});
    $("#MyMatters").css('width', config.display.tableWidth);
    $("#MyMatters").tabulator({
        index: "ID",
        tableBuilding: config.tableFunctions.onTableBuilding,
		    dataLoading: config.tableFunctions.onDataLoading,
		    dataLoaded: config.tableFunctions.onDataLoaded,
        renderStarted: config.tableFunctions.onRenderStarted,
        renderComplete: config.tableFunctions.onRenderComplete,
        tableBuilt: config.tableFunctions.onTableBuilt,
        progressiveRender: true, //enable progressive rendering
        progressiveRenderSize: 20, //sets the number of rows to render per block (default = 20)
        progressiveRenderMargin: 100, //distance in px before end of scroll before progressive render is triggered (default = 200)
        height: config.display.tableHeight,
        headerFilterPlaceholder:"Search for a site...",
        fitColumns: true,
        columns: [
          /** Site Link **/
          { title: "Site Link", formatter: config.tableFunctions.openIcon, field: "Url", width: 60, align: "center", sortable: false, visible: true },
          /** Site Type Decorator **/
        //  { title: "Site Type", formatter: config.tableFunctions.siteIcon, colMinWidth: 80, sortable: false, visible: false },
          /** Site Title **/
          { headerFilter: true, title: "Title", field: "Title", sorter: "string", colMinWidth: 260, onClick: config.tableFunctions.showDetails, visible: true },
          /** Assigned Lit. Manager **/
          { headerFilter: true, title: "Litigation Manager", field: "Litigation_Manager", sorter: "string", colMinWidth: 80, visible: false, sortable:true },
          /** Account Name **/
          { headerFilter: true, title: "Account Name", field: "Account_Name", sortable: true, sorter: "string", visible: false },               
          /** Affiliate **/
          { headerFilter: true, title: "Affiliate", field: "Affiliate", sortable: true, sorter: "string", visible: false },               
          /** Case Caption **/
          { headerFilter:true, title: "Case Caption", field: "Case_Caption", sortable: true, sorter: "string", visible: true },               
          /** Country **/
          { headerFilter:true, title: "Country", field: "Country", sortable: true, sorter: "string", visible: false },               
          /** Docket Number **/
          { headerFilter:true, title: "Docket Number", field: "Docket_Number", sortable: true, sorter: "string", visible: false },               
          /** Last Synchronized **/
          { headerFilter:true, title: "Last Synchronized", field: "Last_Synchronized", sortable: true, sorter: "date", visible: false },               
          /** Legacy Site ID **/
          { headerFilter:true, title: "Legacy Site ID", field: "Legacy_Site_ID", sortable: true, sorter: "number", visible: false },               
          /** Litigation Type **/
          { headerFilter:true, title: "Litigation Type", field: "Litigation_Type", sortable: true, sorter: "string", visible: false },               
          /** Matter Name **/
          { headerFilter:true, title: "Matter Name", field: "Matter_Name", sortable: true, sorter: "string", visible: false },               
          /** Litigation Matter Number **/
          { headerFilter:true, title: "Litigation Matter Number", field: "Matter_Number", sortable: true, sorter: "string", visible: false },               
          /** Matter Status **/
          { headerFilter:true, title: "Matter Status", field: "Matter_Status", sortable: true, sorter: "string", visible: false },               
          /** <atter Site Created **/
          { headerFilter:true, title: "Matter Site Created", field: "Site_Created", sortable: true, sorter: "date", visible: false },               
          /** State Filed **/
          { headerFilter:true, title: "State Filed", field: "State_Filed", sortable: true, sorter: "string", visible: false },               
          /** Venue **/
          { headerFilter:true, title: "Venue", field: "Venue", sortable: true, sorter: "string", visible: false },               
          /** Work/Matter Type **/
          { headerFilter:true, title: "Work/Matter Type", field: "Work_Matter_Type", sortable: true, sorter: "string", visible: false }
        ]
    });

    // Feed the data to the table
    var currentUser = function()
    { try
      {
        return parent.document.getElementById("zz4_Menu").childNodes[0];
      }
      catch(e)
      {
        return "Unknown";
      }
    }

    $("#MyMatters").tabulator("setData", data);
    //$("#MyMatters").tabulator("setFilter", "Litigation_Manager", currentUser());
}
