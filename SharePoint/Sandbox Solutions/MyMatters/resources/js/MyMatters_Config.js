var config = {
    env: {
        storageAgent: amplify.store.localStorage,
        dev: {
            restServiceUrl: "https://rivernet2ndev.trg.com/AuxiliaryServices/GetSubWebs",
            litManSiteUrl: "https://rivernet2ndev.trg.com/sites/litman",
            excludedKeyPatterns: ["__","_web","cachedwebs","moss","nocrawl","v4","vti_"],
            dataCacheInHours: 0.05
        },
        prod: {
            restServiceUrl: "https://rivernet2n.trg.com/AuxiliaryServices/GetSubWebs",
            litManSiteUrl: "https://rivernet2n.trg.com/sites/litman",
            excludedKeyPatterns: ["__","_web","cachedwebs","moss","nocrawl","v4","vti_"],
            dataCacheInHours: 4
        },
        qa: {
            restServiceUrl: "https://rivernetqa.trg.com/AuxiliaryServices/GetSubWebs",
            litManSiteUrl: "https://rivernetqa.trg.com/sites/litman",
            excludedKeyPatterns: ["__","_web","cachedwebs","moss","nocrawl","v4","vti_"],
            propertyFilter:  { "Matter_Status": ["Open", "Stayed"] },
            dataCacheInHours: 4
        }
    },
    display: {
        tableHeight: "600px",
        tableWidth: "100%",
        optionsIcon: "<i class='fa fa'>&#xf022;</i>"

    },
    tableFunctions: {
      groupHeader: function(value, count) {
            if (!isEmpty(value)) {
                return "Assigned To: " + value +
                    "<span class='badge'>" + count + "</span>";
            } else {
                return "[Not Assigned] <span class='badge'> " + count + "</span>";
            }
        },
      /** Table Event Callbacks **/
      onTableBuilding: function() {
        console.log('Tabulator: Table Building ');
      },
      onDataLoading: function() {
        console.log('Tabulator: Data Loading');
      },
      onDataLoaded: function() {
        console.log('Tabulator: Data Loaded');
      },
      onRenderStarted: function() {
        // Careful... this gets called a lot!
        console.log('Tabulator: Render Started');
        //waitingDialog.show("Rendering...",{percentComplete:"75%"});
      },
      onRenderComplete:function() {
        // Careful... this gets called a lot!
        console.log('Tabulator: Render Complete');
        waitingDialog.hide();
      },
      onTableBuilt: function() {
        console.log('Tabulator: Table Built');
        waitingDialog.hide();
        storeManagers(); // save managers for display options.
      },
      /** End Table Event Callbacks **/
      openIcon: function(value) {
        var html = "<button class='btn btn-basic btn-xs' onclick='window.open(\"" + value + "\",\"_blank\");'>\
        <span class ='glyphicon glyphicon-new-window'></span></button>";
        return html;
      },
      setDropdown: function() {
        if ( $('.dropdown-menu li.toggleField').length == 0 )
        {
          var col = $("#MyMatters").tabulator("getColumns")
          $.each(col, function( key, value){
          $(".afterToggleLinks")
          .before("<li id='" + value.field +"' class='toggleField' onclick='toggleField(this.id);'><a>" +
          value.title + "</a></li>");
          });
        }

    },
      siteIcon: function(value, data) {
        return GetSiteDecorators(data); },
      showDetails: function(e, id, data, row) {
        showDetailDialog(row);}
    }
}
