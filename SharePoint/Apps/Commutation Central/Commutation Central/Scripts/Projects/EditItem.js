var appweburl;
var userIsSuperManager = false;

// Database variables
var dbCurrentItem;
var dbCounterpartyName;
var dbProjectId;
var dbPrimaryManagerId;
var dbSecondaryManagerId;
var dbOversightManagerId;
var dbLeadOfficeId;
var dbRequestorId;
var dbRequestTypeId;
var dbRequestDate;
var dbCommutationType;
var dbCommutationStatus;
var dbDroppedReason;
var dbDealPriority;
var dbCompanyStatus;
var dbFinancialAuthority;
var dbFinancialAuthorityGrantedById;
var dbFinancialAuthorityGrantedByDate;

// Update variables
var itemUpdates = new Object();

// When the DOM is ready, begin code execution
$(document).ready(function () {
    InitializeGlobalVars();
    InitializeTabs();
    InitializeFields();
    var getItem = GetCurrentItem();
    if (getItem != null) {
        getItem.done(function (item, status, xhr) {
            PopulateFormData();
            InitializeValidation();
        });
        getItem.fail(function (status, xhr) {
            displayError(status);
        });
    }
});

// Prevent the default submit actions from happening. No server-side code allowed!
$(document).submit(function (event) {
    event.preventDefault();
});

//===============================
// Initialization
//===============================

// Show the loading animation
$(function () {
    $("#loadingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#editItemContent") },
        modal: true,
        resizable: false
    });
});

// Initialize any required global variables
function InitializeGlobalVars() {
    //appweburl = decodeURIComponent(getQueryStringParameter("SPAppWebUrl"));
    appweburl = _spPageContextInfo.webServerRelativeUrl;
}

// Initialize the jQuery tabs UI
function InitializeTabs() {
    $("#formTabs").tabs({
        activate: function (event, ui) { ResizeDialogWindow(); }
    });
    var tabToDisplay = getQueryStringParameter("Tab");
    if (tabToDisplay) {
        $("#formTabs").tabs("option", "active", tabToDisplay);
    }
}

// Initialize any fields that aren't built in the DOM
function InitializeFields() {
    $(".datefield").datepicker({
        showOn: "button",
        buttonImage: "/_layouts/15/images/calendar.gif?rev=BEi%2Ba5GlgOA8I9XAdI4TNr33AtzDCF139o2jDGxr%2FMfwuH5n83E2%2BBqWzQKN18YeVJNwoYmtyrcI9V0uHNKvFg%3D%3D",
        buttonImageOnly: true,
        buttonText: "Select Date",
        autoSize: true
    });

    GetPermissionGroup("Commutation Managers").done(function (group) {
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='primaryManagerId_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
        spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='secondaryManagerId_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
        spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='financialAuthorityGrantedById_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
    }).fail(function (status) {
        displayError(status);
    });

    GetPermissionGroup("Commutations Requestors").done(function (group) {
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='requestorId_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
    }).fail(function (status) {
        displayError(status);
    });

    // Lock down the super manager fields
    GetPermissionGroup("Super Managers", true).done(function (groupData) {
        if (groupData.d.results.length > 0) {
            $("#managerFunctions").accordion({ active: false, collapsible: true });
            $.each(groupData.d.results, function (i, user) {
                if (user.Id == _spPageContextInfo.userId) { userIsSuperManager = true; }
            });
            if (!userIsSuperManager) {
                NotSuperManager();
            }
        } else {
            NotSuperManager();
        }
    }).fail(function (status) {
        displayError(status);
        NotSuperManager();
    });

    GetPermissionGroup("Oversight Managers").done(function (group) {
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$='oversightManagerId_TopSpan']")[0].id];
        spPP.SharePointGroupID = group.d.Id;
    }).fail(function (status) {
        displayError(status);
    });

    function NotSuperManager() {
        $("#managerFunctions").css("display", "none");
        SetPeoplePickerFieldReadOnly("primaryManagerId_TopSpan", "Primary Manager");
        SetPeoplePickerFieldReadOnly("secondaryManagerId_TopSpan", "Secondary Manager");
        SetPeoplePickerFieldReadOnly("oversightManagerId_TopSpan", "Oversight Manager");
        $('#leadOfficeId').attr('disabled', 'disabled');
        $("#authority").attr('disabled', 'disabled');
        SetPeoplePickerFieldReadOnly("financialAuthorityGrantedById_TopSpan", "Granted By");
        $("#financialAuthorityGrantedByDate").datepicker("option", "disabled", true);
    }

    var getCurrentLookup = GetLookups("Lead Office Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("leadOfficeId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Request Type Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("requestTypeId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Commutation Type Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("commutationTypeId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Commutation Status Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("commutationStatusId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Dropped Reason Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("droppedReasonId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Deal Priority Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("dealPriorityId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Company Status Lookup");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("companyStatusId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    getCurrentLookup = GetLookups("Fairfax Entities in Scope Lookup", "Title");
    getCurrentLookup.done(function (item, status, xhr) {
        PopulateDropDown("fairfaxEntityId", item.d.results, "ID", "Title");
    });
    getCurrentLookup.fail(function (status, xhr) {
        displayError(status);
    });
    $(".projectIdField").change(function (handler) {
        $(".projectIdField").val(handler.srcElement.value);
    });
    $(".counterpartyNameField").change(function (handler) {
        $(".counterpartyNameField").val(handler.srcElement.value);
    });
    $(".currency").change(function () {
        $(".currency").formatCurrency({ colorize: true });
    });
    $("#totalAssumed, #totalCeded").change(function () {
        CalculateTotalNet();
    });
    $("#assumedBook, #cededBook").change(function () {
        $("#netBook").text(accounting.unformat($("#cededBook").val()) - accounting.unformat($("#assumedBook").val())).formatCurrency({ colorize: true });
    });
    $("#assumedCommutation, #cededCommutation").change(function () {
        $("#netCommutation").text(accounting.unformat($("#cededCommutation").val()) - accounting.unformat($("#assumedCommutation").val())).formatCurrency({ colorize: true });
    });
    UpdateLinkTokens();
    $("[id$=btnSubmit]").click(function (event) {
        ValidateEntries();
    });
}

// Initialize the form validator
function InitializeValidation() {
    $("#aspnetForm").validate({
        groups: {
            requestDate: "requestDate"
        },
        errorPlacement: function (error, element) {
            if (element.attr("id").indexOf("requestDate") >= 0) {
                error.insertAfter("#requestDateContainer");
            }
            else if (element.attr("id").indexOf("financialAuthorityGrantedByDate") >= 0) {
                error.insertAfter("#financialAuthorityGrantedByDateContainer");
            }
            else {
                error.insertAfter(element);
            }
        }
    });
    $("#droppedReasonId").rules("add", {
        min: function () {
            if ($("#droppedReasonId").closest("td").css('visibility') === 'visible') {
                return 2;
            }
            else { return 1; }
        },
        messages: {
            min: "Please select a value other than 'none'"
        }
    });
    $("#financialAuthorityGrantedByDate").rules("add", {
        required: {
            depends: function () {
                if ($("#authority").val() != "$0.00" && $("#authority").val() != "") {
                    return true;
                } else { return false; }
            }
        }
    });
}

// Set a people picker field to read only
function SetPeoplePickerFieldReadOnly(controlId, controlTitle) {
    var ppDiv = $("[id$=" + controlId + "]");
    var ppEditor = ppDiv.find("[title='" + controlTitle + "']");
    var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv[0].id];
    spPP.SetEnabledState(false);
    $('.sp-peoplepicker-delImage').css('display', 'none');
}

// Get the current item from the ID query string parameter
function GetCurrentItem() {
    var itemId = getQueryStringParameter("ID");
    if (itemId != null) {
        var restUrl = appweburl + "/_api/web/lists/getbytitle('Projects')/items?$expand=PrimaryManagerID,SecondaryManagerID,OversightManagerID,RequestorID,FinancialAuthorityGrantedBy&$select=*,PrimaryManagerID/Name,SecondaryManagerID/Name,OversightManagerID/Name,RequestorID/Name,FinancialAuthorityGrantedBy/Name&$filter=ID eq " + itemId;
        return $.ajax({
            url: restUrl,
            method: "GET",
            headers: { "accept": "application/json; odata=verbose" },
            success: function (data) {
                if (data.d.results.length > 0) {
                    dbCurrentItem = data.d.results[0];
                    itemUpdates.__metadata = dbCurrentItem.__metadata;
                }
            }
        });
    }
    else { return null };
}

// Get lookup values for a drop down menu
function GetLookups(listName, sortField) {
    var restUrl = appweburl + "/_api/web/lists/getbytitle('" + listName + "')/items";
    if (typeof sortField != 'undefined') {
        restUrl += "?$orderby=" + sortField;
    }
    return $.ajax({
        url: restUrl,
        method: "GET",
        headers: {
            "ACCEPT": "application/json;odata=verbose"
        }
    });
}

// Populate a drop down menu with results
function PopulateDropDown(controlId, results, idColumn, descriptionColumn) {
    for (var i = 0; i < results.length; i++) {
        var IDTemp = results[i][idColumn];
        $('#' + controlId).append("<option value='" + IDTemp + "'>" +
            results[i][descriptionColumn] + "</option>");
    }
}

// Using the item data that was returned via Ajax, populate the fields on the form with the current values
function PopulateFormData() {
    if (typeof dbCurrentItem != 'undefined') {
        dbCounterpartyName = dbCurrentItem.Title;
        $(".counterpartyNameField").val(dbCounterpartyName);
        dbProjectId = dbCurrentItem.CommProjectID;
        $(".projectIdField").val(dbProjectId);
        dbPrimaryManagerId = dbCurrentItem.PrimaryManagerIDId;
        //SetPeoplePickerValueFromNumericId("primaryManagerId", "Primary Manager", dbPrimaryManagerId, true);
        SetPeoplePickerValueFromLoginName("primaryManagerId", "Primary Manager", dbCurrentItem.PrimaryManagerID.Name, true);
        dbSecondaryManagerId = dbCurrentItem.SecondaryManagerIDId;
        //SetPeoplePickerValueFromNumericId("secondaryManagerId", "Secondary Manager", dbSecondaryManagerId, true);
        SetPeoplePickerValueFromLoginName("secondaryManagerId", "Secondary Manager", dbCurrentItem.SecondaryManagerID.Name, true);
        dbOversightManagerId = dbCurrentItem.OversightManagerIDId;
        //SetPeoplePickerValueFromNumericId("oversightManagerId", "Oversight Manager", dbOversightManagerId, true);
        SetPeoplePickerValueFromLoginName("oversightManagerId", "Oversight Manager", dbCurrentItem.OversightManagerID.Name, true);
        dbLeadOfficeId = dbCurrentItem.LeadOfficeId;
        $("#leadOfficeId").val(dbLeadOfficeId);
        dbRequestorId = dbCurrentItem.RequestorIDId;
        //SetPeoplePickerValueFromNumericId("requestorId", "Requestor", dbRequestorId, false);
        SetPeoplePickerValueFromLoginName("requestorId", "Requestor", dbCurrentItem.RequestorID.Name, true);
        dbRequestTypeId = dbCurrentItem.RequestTypeId;
        $("#requestTypeId").val(dbRequestTypeId);
        dbRequestDate = new Date(dbCurrentItem.RequestDate);
        dbRequestDate = dbRequestDate.getMonth() + 1 + "/" + dbRequestDate.getUTCDate() + "/" + dbRequestDate.getFullYear();
        $("#requestDate").val(dbRequestDate);
        dbCommutationType = dbCurrentItem.CommutationTypeId;
        $("#commutationTypeId").val(dbCommutationType);
        dbCommutationStatus = dbCurrentItem.CommutationStatusId;
        $("#commutationStatusId").val(dbCommutationStatus);
        if ($("#commutationStatusId").val() === '6') {
            $('#droppedReasonId').closest('td').removeClass('hiddenContentCell').addClass('contentCell').prev('td').removeClass('hiddenContentCell').addClass('contentCell');
        }
        dbDroppedReason = dbCurrentItem.DroppedReasonId;
        $("#droppedReasonId").val(dbDroppedReason);
        dbDealPriority = dbCurrentItem.DealPriorityId;
        $("#dealPriorityId").val(dbDealPriority);
        dbCompanyStatus = dbCurrentItem.CompanyStatusId;
        $("#companyStatusId").val(dbCompanyStatus);
        dbFinancialAuthority = dbCurrentItem.FinancialAuthority;
        $("#authority").val(dbFinancialAuthority).formatCurrency();
        dbFinancialAuthorityGrantedById = dbCurrentItem.FinancialAuthorityGrantedById;
        //SetPeoplePickerValueFromNumericId("financialAuthorityGrantedById", "Granted By", dbFinancialAuthorityGrantedById);
        SetPeoplePickerValueFromLoginName("financialAuthorityGrantedById", "Granted By", dbCurrentItem.FinancialAuthorityGrantedBy.Name);
        if (dbCurrentItem.FinancialAuthorityGrantedByDate != null) {
            dbFinancialAuthorityGrantedByDate = new Date(dbCurrentItem.FinancialAuthorityGrantedByDate);
            dbFinancialAuthorityGrantedByDate = dbFinancialAuthorityGrantedByDate.getMonth() + 1 + "/" + dbFinancialAuthorityGrantedByDate.getUTCDate() + "/" + dbFinancialAuthorityGrantedByDate.getFullYear();
        } else { dbFinancialAuthorityGrantedByDate = null; }
        $("#financialAuthorityGrantedByDate").val(dbFinancialAuthorityGrantedByDate);
        var dbPreliminaryValuationDate;
        if (dbCurrentItem.PreliminaryValuationDate != null) {
            dbPreliminaryValuationDate = new Date(dbCurrentItem.PreliminaryValuationDate);
            dbPreliminaryValuationDate = dbPreliminaryValuationDate.getMonth() + 1 + "/" + dbPreliminaryValuationDate.getUTCDate() + "/" + dbPreliminaryValuationDate.getFullYear();
        } else { dbPreliminaryValuationDate = null }
        $("#preliminaryValuationDate").val(dbPreliminaryValuationDate);
        $("#totalAssumed").val(dbCurrentItem.TotalAssumed).formatCurrency();
        $("#totalCeded").val(dbCurrentItem.TotalCeded).formatCurrency({ colorize: true });
        var totalNet = accounting.unformat(dbCurrentItem.TotalCeded) - accounting.unformat(dbCurrentItem.TotalAssumed);
        $("#totalNet").text(totalNet).formatCurrency({ colorize: true });
        var dbFinalValuationDate;
        if (dbCurrentItem.FinalValuationDate != null) {
            dbFinalValuationDate = new Date(dbCurrentItem.FinalValuationDate);
            dbFinalValuationDate = dbFinalValuationDate.getMonth() + 1 + "/" + dbFinalValuationDate.getUTCDate() + "/" + dbFinalValuationDate.getFullYear();
        } else { dbFinalValuationDate = null; }
        $("#finalValuationDate").val(dbFinalValuationDate);
        $("#assumedBook").val(dbCurrentItem.AssumedBook).formatCurrency({ colorize: true });
        $("#assumedCommutation").val(dbCurrentItem.AssumedCommutation).formatCurrency({ colorize: true });
        $("#cededBook").val(dbCurrentItem.CededBook).formatCurrency({ colorize: true });
        $("#cededCommutation").val(dbCurrentItem.CededCommutation).formatCurrency({ colorize: true });
        $("#netBook").text(accounting.unformat(dbCurrentItem.CededBook) - accounting.unformat(dbCurrentItem.AssumedBook)).formatCurrency({ colorize: true });
        $("#netCommutation").text(accounting.unformat(dbCurrentItem.CededCommutation) - accounting.unformat(dbCurrentItem.AssumedCommutation)).formatCurrency({ colorize: true });
        $("#badDebtBook").val(dbCurrentItem.BadDebtBook).formatCurrency({ colorize: true });
        $("#badDebtCommutation").val(dbCurrentItem.BadDebtCommutation).formatCurrency({ colorize: true });
        $("#adjustedValueBook").val(dbCurrentItem.AdjustedValueBook).formatCurrency({ colorize: true });
        $("#adjustedValueCommutation").val(dbCurrentItem.AdjustedValueCommutation).formatCurrency({ colorize: true });
        $("#proceedsBook").val(dbCurrentItem.ProceedsBook).formatCurrency({ colorize: true });
        $("#proceedsCommutation").val(dbCurrentItem.ProceedsCommutation).formatCurrency({ colorize: true });
        $("#collateralBook").val(dbCurrentItem.CollateralBook).formatCurrency({ colorize: true });
        $("#collateralCommutation").val(dbCurrentItem.CollateralCommutation).formatCurrency({ colorize: true });
        $("#gaapImpactBook").val(dbCurrentItem.GAAPImpactBook).formatCurrency({ colorize: true });
        $("#gaapImpactCommutation").val(dbCurrentItem.GAAPImpactCommutation).formatCurrency({ colorize: true });
        $("#statImpactBook").val(dbCurrentItem.STATImpactBook).formatCurrency({ colorize: true });
        $("#statImpactCommutation").val(dbCurrentItem.STATImpactCommutation).formatCurrency({ colorize: true });

        var getProjectDocs = GetProjectDocuments();
        getProjectDocs.done(function (projectDocuments, status, xhr) {
            var dataTable = $("#projectDocuments").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillProjectDocumentsTable(projectDocuments.d.results);
        });
        getProjectDocs.fail(function (status, xhr) {
            displayError(status);
        });

        var getScopedCompanies = GetCompaniesInScope();
        getScopedCompanies.done(function (companiesInScope, status, xhr) {
            var companiesInScopeDataTable = $("#companiesInScope").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillCompaniesInScopeTable(companiesInScope.d.results);
        });
        getScopedCompanies.fail(function (status, xhr) {
            displayError(status);
        });

        var getScopedEntities = GetFairfaxEntitiesInScope();
        getScopedEntities.done(function (entitiesInScope, status, xhr) {
            var entitiesInScopeDataTable = $("#fairfaxEntitiesInScope").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillFairfaxEntitiesInScopeTable(entitiesInScope.d.results);
        });
        getScopedEntities.fail(function (status, xhr) {
            displayError(status);
        });

        var getScopedContacts = GetContactsInScope();
        getScopedContacts.done(function (contactsInScope, status, xhr) {
            var contactsInScopeDataTable = $("#contactsInScope").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillContactsInScopeTable(contactsInScope.d.results);
        });
        getScopedContacts.fail(function (status, xhr) {
            displayError(status);
        });

        var getActivities = GetActivities();
        getActivities.done(function (activities, status, xhr) {
            var activitiesDataTable = $("#activities").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": [0,1] },
                    { "orderable": false, "targets": [0,1] }
                ],
                "order": [2, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillActivitiesTable(activities.d.results);
        });
        getActivities.fail(function (status, xhr) {
            displayError(status);
        });

        var getNotes = GetNotes();
        getNotes.done(function (notes, status, xhr) {
            var notesDataTable = $("#notes").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": [0,1] },
                    { "orderable": false, "targets": [0,1] }
                ],
                "order": [2, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillNotesTable(notes.d.results);
        });
        getNotes.fail(function (status, xhr) {
            displayError(status);
        });

        var getCheckList = GetDealCheckList();
        getCheckList.done(function (checkListItems, status, xhr) {
            var checkListDataTable = $("#checkListItems").DataTable({
                "autoWidth": false,
                "columnDefs": [
                    { "searchable": false, "targets": 0 },
                    { "orderable": false, "targets": 0 }
                ],
                "order": [1, 'asc'],
                "drawCallback": function () { ResizeDialogWindow(); }
            });
            FillDealCheckListTable(checkListItems.d.results);
        });
        getCheckList.fail(function (status, xhr) {
            displayError(status);
        });

        $.when(getProjectDocs, getScopedCompanies, getScopedEntities, getScopedContacts, getActivities, getNotes, getCheckList).done(function () {
            ResizeDialogWindow(true);
        });
    }
}

// Set a people picker field by using a numerical user ID
function SetPeoplePickerValueFromNumericId(peoplePickerElementId, controlName, userId, readOnly) {
    if (userId != null) {
        var ppDiv = $("[id$='" + peoplePickerElementId + "_TopSpan']");
        var ppEditor = ppDiv.find("[title='" + controlName + "']");
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv[0].id];

        var getUser = GetUserFromNumericId(userId);
        getUser.done(function (user, status, xhr) {
            ppEditor.val(user.d.LoginName);
            spPP.AddUnresolvedUserFromEditor(true);
            if (readOnly) {
                $('.sp-peoplepicker-delImage').css('display', 'none');
            }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
}

// Set a people picker field by using a user's login name
function SetPeoplePickerValueFromLoginName(peoplePickerElementId, controlName, loginName, readOnly) {
    if (loginName != null) {
        var ppDiv = $("[id$='" + peoplePickerElementId + "_TopSpan']");
        var ppEditor = ppDiv.find("[title='" + controlName + "']");
        var spPP = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv[0].id];

        var getUser = GetUserFromLogin(loginName);
        getUser.done(function (user, status, xhr) {
            ppEditor.val(user.d.LoginName);
            spPP.AddUnresolvedUserFromEditor(true);
            if (readOnly) {
                $('.sp-peoplepicker-delImage').css('display', 'none');
            }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
}

function RenderProjectDocuments() {
    GetProjectDocuments().done(function (results) {
        FillProjectDocumentsTable(results.d.results);
    }).fail(function (status) {
        displayError(status);
    });
}

// Get project documents
function GetProjectDocuments() {
    // Endpoint URL must be constructed differently for lookup columns
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Commutation Documents')/Items/?$filter=Project/Id eq " + getQueryStringParameter("ID") + "&$expand=File/ModifiedBy";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Get companies in scope
function GetCompaniesInScope() {
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Companies in Scope')/Items/?$filter=Project/Id eq " + getQueryStringParameter("ID") + "&$top=5000";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the project documents table
function FillProjectDocumentsTable(results) {
    var dataTable = $("#projectDocuments").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onclick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Project Document\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            "<a style='color:blue' href='" + item.File.ServerRelativeUrl + "'>" + item.File.Name + "</a>",
            item.OData__dlc_DocId,
            new Date(item.Modified).format('M/d/yyyy h:mm tt'),
            item.File.ModifiedBy.Title,
            item.File.MajorVersion + "." + item.File.MinorVersion
        ]).draw();
    }); 
}

// Fill the companies in scope table
function FillCompaniesInScopeTable(results) {
    var dataTable = $("#companiesInScope").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Company In Scope\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            item.CompanyName
        ]).draw();
    });
}

// Get Fairfax entities in scope
function GetFairfaxEntitiesInScope() {
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Fairfax Entities in Scope')/Items/?$select=FairfaxEntity/Title&$filter=Project/Id eq " + getQueryStringParameter("ID") + "&$expand=FairfaxEntity/Title";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the Fairfax entities in scope table
function FillFairfaxEntitiesInScopeTable(results) {
    var dataTable = $("#fairfaxEntitiesInScope").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Fairfax Entity In Scope\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            item.FairfaxEntity.Title
        ]).draw();
    });
}

// Get Fairfax entities in scope
function GetContactsInScope() {
    var endpointUrl = appweburl + "/_api/lists/getbytitle('Project Contacts')/Items/?$filter=Project/Id eq " + getQueryStringParameter("ID");
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the Contacts in scope table
function FillContactsInScopeTable(results) {
    var dataTable = $("#contactsInScope").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        var allContactDetails = $.ajax({
            url: appweburl + "/_api/web/lists/getbytitle('Contact Lookup')/items(" + item.ContactId + ")",
            type: "GET",
            headers: { "accept": "application/json;odata=verbose" }
        });
        allContactDetails.done(function (contactDetails, status, xhr) {
            dataTable.row.add([
            "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Project Contact\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
            "<a style='color:blue' href='javascript:void(0)' onClick='EditContact(" + contactDetails.d.Id + ")'>" + contactDetails.d.Title + "</a>",
            contactDetails.d.FirstName,
            "<a style='color:blue' href='mailto:" + contactDetails.d.Email + "'>" + contactDetails.d.Email + "</a>",
            contactDetails.d.WorkPhone,
            contactDetails.d.WorkAddress,
            contactDetails.d.WorkCity,
            contactDetails.d.WorkState
            ]).draw();
        });
        allContactDetails.error(function (status, xhr) {
            displayError(status);
        });
    });
}

// Get activities associated with the project
function GetActivities() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Activity')/Items/?$select=*,ActivityCategory/Title,AssignedTo/Title&$expand=ActivityCategory,AssignedTo&$filter=Project/Id eq " + getQueryStringParameter("ID");
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Get active activities associated with the project
function GetActiveActivities() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Activity')/Items/?$select=*,ActivityCategory/Title,AssignedTo/Title&$expand=ActivityCategory,AssignedTo&$filter=Project/Id eq " + getQueryStringParameter("ID") + " and ActivityStatus eq 'Active'";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the Activities table
function FillActivitiesTable(results) {
    var dataTable = $("#activities").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        var getAttachments = GetItemAttachments('Activity', item.Id);
        getAttachments.done(function (attachments, status, xhr) {
            dataTable.row.add([
                "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Activity\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
                (attachments.d.results.length >= 1) ? "<img alt='' src='" + _spPageContextInfo.webServerRelativeUrl + "/Images/PaperClip3_Black.png'/>" : "",
                "<a style='color:blue' href='javascript:void(0)' onClick='EditActivity(" + item.Id + ")'>" + item.Title + "</a>",
                (item.ActivityCategory.Title) ? item.ActivityCategory.Title : null,
                (item.EntryDate) ? new Date(item.EntryDate).format("M/d/yyyy") : null,
                (item.AssignedTo.Title) ? item.AssignedTo.Title : null,
                item.Priority,
                item.ActivityStatus,
                (item.TaskDueDate) ? new Date(item.TaskDueDate).format("M/d/yyyy") : null,
                (item.ActivityStatusChangeDate) ? new Date(item.ActivityStatusChangeDate).format("M/d/yyyy") : null
            ]).draw();
        });
        getAttachments.fail(function (status, xhr) {
            displayError(status);
        });
    });
}

// Get notes associated with the project
function GetNotes() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Notes')/Items/?$select=*,EntryType/Title,Author/Title&$expand=EntryType,Author&$filter=Project/Id eq " + getQueryStringParameter("ID");
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the Notes table
function FillNotesTable(results) {
    var dataTable = $("#notes").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var content = item.Content1;
        var isHtml = /<[a-z][\s\S]*>/i.test(content);
        //isHtml ? content = $(content).text(): content=content;
        content = $('<p>' + content + '</p>').text();
        var itemUrl = escapeHtml(item.__metadata.uri);
        var itemEtag = item.__metadata.etag;
        var getAttachments = GetItemAttachments('Note', item.Id);
        getAttachments.done(function (attachments, status, xhr) {
            dataTable.row.add([
                "<a class='deleteItemLink' style='color:blue' href='javascript:void(0)' onClick='DeleteListItem(\"" + itemUrl + "\"," + itemEtag + ", true, \"Note\")'><span class=' ms-cui-img-16by16 ms-cui-img-cont-float' unselectable='on'><img style='left: -271px; top: -271px;' alt='' src='/_layouts/15/1033/images/formatmap16x16.png?rev=23' unselectable='on'/></span></a>",
                (attachments.d.results.length >= 1) ? "<img alt='' src='" + _spPageContextInfo.webServerRelativeUrl + "/Images/PaperClip3_Black.png'/>" : "",
                "<a style='color:blue' href='javascript:void(0)' onClick='EditNote(" + item.Id + ")'>" + item.EntryType.Title + "</a>",
                content.substring(0, 30) + "...",
                (item.EntryDate) ? new Date(item.EntryDate).format("M/d/yyyy") : null,
                item.Author.Title,
                new Date(item.Modified).format("M/d/yyyy h:mm tt")
            ]).draw();
        });
        getAttachments.fail(function (status, xhr) {
            displayError(status);
        });
    });
}

// Get deal check list items associated with the project
function GetDealCheckList() {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Deal Check List')/Items/?$select=*,CheckListItem/Title&$expand=CheckListItem&$filter=Project/Id eq " + getQueryStringParameter("ID");
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Fill the Deal Check List table
function FillDealCheckListTable(results) {
    var dataTable = $("#checkListItems").DataTable();
    dataTable.clear().draw();
    $.each(results, function (i, item) {
        var getAttachments = GetItemAttachments('DealCheckListItem', item.Id);
        getAttachments.done(function (attachments, status, xhr) {
            dataTable.row.add([
                (attachments.d.results.length >= 1) ? "<img alt='' src='" + _spPageContextInfo.webServerRelativeUrl + "/Images/PaperClip3_Black.png'/>" : "",
                "<a style='color:blue' href='javascript:void(0)' onClick='EditCheckListItem(" + item.Id + ")'>" + item.CheckListItem.Title + "</a>",
                item.Applicable
            ]).draw();
        });
    });
}

//===============================
// End Initialization
//===============================

//===============================
// Field Updates
//===============================

// Handle a text field change
function TextFieldChanged(event, dbProperty) {
    var fieldVal = $("#" + event.target.id).val();
    var dbVal = dbCurrentItem[dbProperty];
    if (!(fieldVal === dbVal)) {
        itemUpdates[dbProperty] = fieldVal;
        if ($("#" + event.target.id).hasClass("currency") && fieldVal === "") {
            fieldVal = 0;
            itemUpdates[dbProperty] = null;
        };
    }
    else { delete itemUpdates[dbProperty]; }

    // Need to handle Commutation Status to show/hide the dropped reason field
    if (event.target.id === 'commutationStatusId') {
        if (!userIsSuperManager && fieldVal === '5') { alert("You must be a 'Super Manager' to marka project as complete."); $("#" + event.target.id).val(dbVal); }
        if (fieldVal === '6') {
            $('#droppedReasonId').closest('td').removeClass('hiddenContentCell').addClass('contentCell').prev('td').removeClass('hiddenContentCell').addClass('contentCell');
        }
        else {
            $('#droppedReasonId').val('1');
            itemUpdates['DroppedReasonId'] = '1';
            $('#droppedReasonId').closest('td').removeClass('contentCell').addClass('hiddenContentCell').prev('td').removeClass('contentCell').addClass('hiddenContentCell');
        }
    }
    $("#aspnetForm").valid();
}

// Handle a date field change
function DateFieldChanged(event, dbProperty) {
    var fieldVal = new Date($("#" + event.target.id).val());
    var dbVal = new Date(dbCurrentItem[dbProperty]);
    if (!(fieldVal === dbVal)) {
        itemUpdates[dbProperty] = fieldVal;
    }
    else { delete itemUpdates[dbProperty]; }
    $("#aspnetForm").valid();
}

// Handle a primary manager field change
function PrimaryManagerFieldChanged(topElementId, users) {
    if (users.length > 0) {
        var getUser = GetUserFromLogin(users[0].Key);
        getUser.done(function (user, status, xhr) {
            var fieldVal = user.d.Id;
            var dbVal = dbCurrentItem.PrimaryManagerIDId;
            if (!(fieldVal === dbVal)) {
                itemUpdates["PrimaryManagerIDId"] = fieldVal;
            }
            else { delete itemUpdates["PrimaryManagerIDId"]; }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
    $("#aspnetForm").valid();
}

// Handle a secondary manager field change
function SecondaryManagerFieldChanged(topElementId, users) {
    if (users.length > 0) {
        var getUser = GetUserFromLogin(users[0].Key);
        getUser.done(function (user, status, xhr) {
            var fieldVal = user.d.Id;
            var dbVal = dbCurrentItem.SecondaryManagerIDId;
            if (!(fieldVal === dbVal)) {
                itemUpdates["SecondaryManagerIDId"] = fieldVal;
            }
            else { delete itemUpdates["SecondaryManagerIDId"]; }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
    $("#aspnetForm").valid();
}

// Handle an oversight manager field change
function OversightManagerFieldChanged(topElementId, users) {
    if (users.length > 0) {
        var getUser = GetUserFromLogin(users[0].Key);
        getUser.done(function (user, status, xhr) {
            var fieldVal = user.d.Id;
            var dbVal = dbCurrentItem.OversightManagerIDId;
            if (!(fieldVal === dbVal)) {
                itemUpdates["OversightManagerIDId"] = fieldVal;
            }
            else { delete itemUpdates["OversightManagerIDId"]; }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
    $("#aspnetForm").valid();
}

// Handle a requestor field change
function RequestorFieldChanged(topElementId, users) {
    if (users.length > 0) {
        var getUser = GetUserFromLogin(users[0].Key);
        getUser.done(function (user, status, xhr) {
            var fieldVal = user.d.Id;
            var dbVal = dbCurrentItem.RequestorIDId;
            if (!(fieldVal === dbVal)) {
                itemUpdates["RequestorIDId"] = fieldVal;
            }
            else { delete itemUpdates["RequestorIDId"]; }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
    else { itemUpdates["RequestorIDId"] = null; }
    $("#aspnetForm").valid();
}

// Handle the financial authority granted by field change
function FinancialAuthorityGrantedByFieldChanged(topElementId, users) {
    if (users.length > 0) {
        var getUser = GetUserFromLogin(users[0].Key);
        getUser.done(function (user, status, xhr) {
            var fieldVal = user.d.Id;
            var dbVal = dbCurrentItem.FinancialAuthorityGrantedById;
            if (!(fieldVal === dbVal)) {
                itemUpdates["FinancialAuthorityGrantedById"] = fieldVal;
            }
            else { delete itemUpdates["FinancialAuthorityGrantedById"]; }
        });
        getUser.fail(function (status, xhr) {
            displayError(status);
        });
    }
    else { itemUpdates["FinancialAuthorityGrantedById"] = null; }
    $("#aspnetForm").valid();
}

// Handle a file upload field change
function FileUploadFieldChanged(contentType, sender, lookupField) {
    //uploadFile(contentType, true, $("#" + sender));
    uploadDocument($("#" + sender), contentType, lookupField, getQueryStringParameter("ID"), $("#counterpartyName").val(), RenderProjectDocuments);
    var control = $("#" + sender);
    control.replaceWith(control = control.clone(true));
}

//===============================
// End Field Updates
//===============================

//===============================
// Form Submit
//===============================

// Validate any fields not controlled through ASP field validators
function ValidateEntries() {
    // Check to make sure the requestor field is populated and a user is resolved
    var requestorPeoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=requestorId_TopSpan]")[0].id];
    if (requestorPeoplePicker.GetAllUserInfo().length < 1) {
        requestorPeoplePicker.ShowErrorMessage("Requestor ID is required");
        return;
    }

    // Check to make sure the primary manager field is populated and a user is resolved
    var primaryManagerPeoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=primaryManagerId_TopSpan]")[0].id];
    if (primaryManagerPeoplePicker.GetAllUserInfo().length < 1) {
        primaryManagerPeoplePicker.ShowErrorMessage("Primary Manager ID is required");
        return;
    }

    // Require the financial authority granted by field if authority is provided
    if ($("#authority").val() != "$0.00" && $("#authority").val() != "") {
        var financialAuthorityGrantedByPeoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[$("[id$=financialAuthorityGrantedById_TopSpan]")[0].id];
        if (financialAuthorityGrantedByPeoplePicker.GetAllUserInfo().length < 1) {
            financialAuthorityGrantedByPeoplePicker.ShowErrorMessage("Granted By is required when Authority has been granted");
            return;
        }
    }

    // Verify that the remaining fields are valid
    var isvalid = $("#aspnetForm").valid();
    if (isvalid) {
        var commutationStatusId = $("#commutationStatusId").val();
        if (commutationStatusId === '5' || commutationStatusId === '6') {
            $("#savingdialog").dialog({
                dialogClass: "no-close",
                position: { my: "center", at: "center", of: $("#editItemContent") },
                modal: true,
                resizable: false,
                height: 120
            });

            GetActiveActivities().success(function (activeActivities) {
                if (activeActivities.d.results.length > 0) {
                    var droppedReasonId = (commutationStatusId === '5') ? 3 : 2;
                    $.each(activeActivities.d.results, function (i, activity) {
                        DropActivity(activity, droppedReasonId).complete(function () {
                            if (i == activeActivities.d.results.length - 1) {
                                Submit();
                            }
                        }).fail(function (status) {
                            displayError(status);
                        });
                    });
                } else {
                    Submit();
                }
            }).fail(function (status) {
                displayError(status);
            });
        } else {
            Submit();
        }
    }
}

// Submit the form
function Submit() {
    $("#savingdialog").dialog({
        dialogClass: "no-close",
        position: { my: "center", at: "center", of: $("#editItemContent") },
        modal: true,
        resizable: false,
        height: 120
    });

    $.ajax({
        url: itemUpdates.__metadata.uri,
        type: "POST",
        contentType: "application/json;odata=verbose",
        data: JSON.stringify(itemUpdates),
        headers: {
            "Accept": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val(),
            "X-HTTP-Method": "MERGE",
            "If-Match": itemUpdates.__metadata.etag
        },
        error: function (data) {
            displayError(data);
        },
        complete: function (jqXHR, textStatus) {
            $("#savingdialog").dialog("close");
            if (textStatus == "success") {
                if (getQueryStringParameter("IsDlg") == "1") {
                    window.commonModalDialogClose(1, 1);
                }
                else {
                    if (getQueryStringParameter("Source")) {
                        var sourceParam = "Source=" + getQueryStringParameter("Source") + "&";
                        window.location = getQueryStringParameter("Source") + "?" + decodeURIComponent(document.URL.split("?")[1].replace(sourceParam, ""));
                    } else {
                        window.location = appweburl;
                    }
                }
            }
        }
    });
}

// Cancel the submit
function CancelSubmit() {
    if (getQueryStringParameter("IsDlg") == "1") {
        window.commonModalDialogClose(0, 0);
    }
    else {
        if (getQueryStringParameter("Source")) {
            var sourceParam = "Source=" + getQueryStringParameter("Source") + "&";
            window.location = getQueryStringParameter("Source") + "?" + decodeURIComponent(document.URL.split("?")[1].replace(sourceParam, ""));
        } else {
            window.location = appweburl;
        }
    }
}

//===============================
// End Form Submit
//===============================

//===============================
// Utilities
//===============================

// Return a query string parameter
function getQueryStringParameter(paramToRetrieve) {
    var params = document.URL.split("?")[1].split("&");
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

// Display an error message (typically failed AJAX calls)
function displayError(data) {
    var responseText = $.parseJSON(data.responseText);
    $("#errorCell").css('visibility', 'visible').html("<h4>" + responseText.error.message.value + "</h4").parent("tr").show();
    alert(responseText.error.message.value);
}

// Gets a user object from the SharePoint site using a login name
function GetUserFromLogin(loginName) {
    var siteUrl = appweburl + "/_api/web/siteusers(@v)?@v='" + encodeURIComponent(loginName) + "'";

    return $.ajax({
        url: siteUrl,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" }
    });
}

// Gets a user object from the SharePoint site using the numerical login value
function GetUserFromNumericId(idNumber) {
    var siteUrl = appweburl + "/_api/web/GetUserById(" + idNumber + ")";

    return $.ajax({
        url: siteUrl,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" }
    });
}

// Delete the current project
function DeleteCurrentProject() {
    if (confirm('Are you sure you want to delete this project?\nAll associated activities, notes and documents will be deleted')) {
        $("#savingdialog").dialog({
            dialogClass: "no-close",
            position: { my: "center", at: "center", of: $("#editItemContent") },
            modal: true,
            resizable: false,
            height: 120
        });
        var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Projects')/items(" + getQueryStringParameter("ID") + ")/recycle()";
        $.ajax({
            url: endpointUrl,
            type: "POST",
            headers: {
                "Accept": "application/json;odata=verbose",
                //"X-Http-Method": "DELETE",
                "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                "If-Match": "*"
            },
            error: function (data) {
                displayError(data);
            },
            complete: function (jqXHR, textStatus) {
                $("#savingdialog").dialog("close");
                if (textStatus == "success") {
                    if (getQueryStringParameter("IsDlg") == "1") {
                        window.commonModalDialogClose(1, 1);
                    }
                    else {
                        if (getQueryStringParameter("Source")) {
                            var sourceParam = "Source=" + getQueryStringParameter("Source") + "&";
                            window.location = getQueryStringParameter("Source") + "?" + decodeURIComponent(document.URL.split("?")[1].replace(sourceParam, ""));
                        } else {
                            window.location = appweburl;
                        }
                    }
                }
            }
        });
    }
}

// Deletes a list item
function DeleteListItem(itemUrl, itemEtag, renderClient, contentType) {
    if (confirm('Are you sure you want to delete this ' + contentType + '?')) {
        var deleteItem = ExecuteDelete();
        deleteItem.done(function (something, status, xhr) {
            if (renderClient) {
                switch (contentType) {
                    case "Project Document":
                        var getProjectDocs = GetProjectDocuments();
                        getProjectDocs.done(function (projectDocuments, status, xhr) {
                            FillProjectDocumentsTable(projectDocuments.d.results);
                        });
                        getProjectDocs.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                    case "Company In Scope":
                        var getScopedCompanies = GetCompaniesInScope();
                        getScopedCompanies.done(function (companiesInScope, status, xhr) {
                            FillCompaniesInScopeTable(companiesInScope.d.results);
                        });
                        getScopedCompanies.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                    case "Fairfax Entity In Scope":
                        var getScopedEntities = GetFairfaxEntitiesInScope();
                        getScopedEntities.done(function (entitiesInScope, status, xhr) {
                            FillFairfaxEntitiesInScopeTable(entitiesInScope.d.results);
                        });
                        getScopedEntities.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                    case "Project Contact":
                        var getScopedContacts = GetContactsInScope();
                        getScopedContacts.done(function (contactsInScope, status, xhr) {
                            FillContactsInScopeTable(contactsInScope.d.results);
                        });
                        getScopedContacts.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                    case "Activity":
                        var getActivities = GetActivities();
                        getActivities.done(function (activities, status, xhr) {
                            FillActivitiesTable(activities.d.results);
                        });
                        getActivities.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                    case "Note":
                        var getNotes = GetNotes();
                        getNotes.done(function (notes, status, xhr) {
                            FillNotesTable(notes.d.results);
                        });
                        getNotes.fail(function (status, xhr) {
                            displayError(status);
                        });
                        break;
                }
            }
        });
        deleteItem.fail(function (status, xhr) {
            $("#savingdialog").dialog("close");
            displayError(status);
        });
    }

    function ExecuteDelete() {
        var endpointUrl = decodeURIComponent(itemUrl);
        return $.ajax({
            url: endpointUrl,
            type: "POST",
            headers: {
                "Accept": "application/json;odata=verbose",
                "X-Http-Method": "DELETE",
                "X-RequestDigest": $("#__REQUESTDIGEST").val(),
                "If-Match": "*"
            }
        });
    }
}

// Escape a string to be HTML compliant
function escapeHtml(text) {
    var map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };

    return text.replace(/[&<>"']/g, function (m) { return map[m]; });
}

// Add a company in scope
function AddCompanyInScope() {
    if (!IsStrNullOrEmpty($("#newCompanyInScope").val())) {
        $("#savingdialog").dialog({
            dialogClass: "no-close",
            position: { my: "center", at: "center", of: $("#editItemContent") },
            modal: true,
            resizable: false,
            height: 120
        });

        var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Companies in Scope')/items";
        var metadata = {
            '__metadata': { 'type': 'SP.Data.Companies_x0020_In_x0020_ScopeListItem' },
            'Title': $("#newCompanyInScope").val(),
            'ProjectId': getQueryStringParameter("ID"),
            'CompanyName': $("#newCompanyInScope").val()
        };

        $.ajax({
            url: endpointUrl,
            type: "POST",
            contentType: "application/json;odata=verbose",
            data: JSON.stringify(metadata),
            headers: {
                "Accept": "application/json;odata=verbose",
                "X-RequestDigest": $("#__REQUESTDIGEST").val()
            },
            error: function (data) {
                displayError(data);
            },
            complete: function () {
                $("#savingdialog").dialog("close");
                var getScopedCompanies = GetCompaniesInScope();
                getScopedCompanies.done(function (companiesInScope, status, xhr) {
                    FillCompaniesInScopeTable(companiesInScope.d.results);
                });
                getScopedCompanies.fail(function (status, xhr) {
                    displayError(status);
                });
            }
        });
    }
}

// Add a Fairfax Entity in Scope
function AddFairfaxEntityInScope() {
    if (!IsStrNullOrEmpty($("#fairfaxEntityId").val())) {
        $("#savingdialog").dialog({
            dialogClass: "no-close",
            position: { my: "center", at: "center", of: $("#editItemContent") },
            modal: true,
            resizable: false,
            height: 120
        });

        var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Fairfax Entities In Scope')/items";
        var metadata = {
            '__metadata': { 'type': 'SP.Data.Fairfax_x0020_Entities_x0020_In_x0020_ScopeListItem' },
            'Title': 'No Title',
            'ProjectId': getQueryStringParameter("ID"),
            'FairfaxEntityId': $("#fairfaxEntityId").val()
        };

        $.ajax({
            url: endpointUrl,
            type: "POST",
            contentType: "application/json;odata=verbose",
            data: JSON.stringify(metadata),
            headers: {
                "Accept": "application/json;odata=verbose",
                "X-RequestDigest": $("#__REQUESTDIGEST").val()
            },
            error: function (data) {
                displayError(data);
            },
            complete: function () {
                $("#savingdialog").dialog("close");
                var getScopedEntities = GetFairfaxEntitiesInScope();
                getScopedEntities.done(function (entitiesInScope, status, xhr) {
                    FillFairfaxEntitiesInScopeTable(entitiesInScope.d.results);
                });
                getScopedEntities.fail(function (status, xhr) {
                    displayError(status);
                });
            }
        });
    }
}

// Edit a contact
function EditContact(contactId) {
    var options = {
        url: appweburl + "/Lists/Contact Lookup/EditForm.aspx?ID=" + contactId + "&IsDlg=1",
        title: 'Edit Contact',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getScopedContacts = GetContactsInScope();
            getScopedContacts.done(function (contactsInScope, status, xhr) {
                FillContactsInScopeTable(contactsInScope.d.results);
            });
            getScopedContacts.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Recalculate total net
function CalculateTotalNet() {
    var totalNet = accounting.unformat($("#totalCeded").val()) - accounting.unformat($("#totalAssumed").val());
    $("#totalNet").text(totalNet).formatCurrency({ colorize: true });
}

// Update any anchor tags that have the {StandardTokens} parameter
function UpdateLinkTokens() {
    $("a[href *= '{StandardTokens}']").each(function (i, a) {
        $(a).attr("href",
            $(a).attr("href").replace("{StandardTokens}", GetStandardTokens()));
    });
}

// Get the list of standard querystring tokens
function GetStandardTokens() {
    var standardTokens = "SPHostUrl=" + getQueryStringParameter("SPHostUrl") + "&SPLanguage=" + getQueryStringParameter("SPLanguage") + "&SPClientTag=" + getQueryStringParameter("SPClientTag") +
        "&SPProductNumber=" + getQueryStringParameter("SPProductNumber") + "&SPAppWebUrl=" + getQueryStringParameter("SPAppWebUrl");
    return standardTokens;
}

// Open a modal dialog for adding a project
function OpenNewProject() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Projects/NewItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl,
        title: 'Add a Project',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {

    }
}

// Open a modal dialog for adding a contact
function AddContact() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Contacts/NewItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl,
        title: 'Add a Contact',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {

    }
}

// Open a modal dialog for attaching a contact
function AttachContact() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Contacts/AttachItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl,
        title: 'Add a Contact',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (returnValue > 0) {
            $("#savingdialog").dialog({
                dialogClass: "no-close",
                position: { my: "center", at: "center", of: $("#editItemContent") },
                modal: true,
                resizable: false,
                height: 120
            });

            var itemData = {
                '__metadata': { 'type': 'SP.Data.Project_x0020_ContactsListItem' },
                'ContactId': returnValue,
                'ProjectId': getQueryStringParameter("ID")
            }
            $.ajax({
                url: appweburl + "/_api/web/lists/getbytitle('Project Contacts')/items",
                type: "POST",
                contentType: "application/json;odata=verbose",
                data: JSON.stringify(itemData),
                headers: {
                    "Accept": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val()
                },
                error: function (data) {
                    displayError(data);
                },
                complete: function () {
                    $("#savingdialog").dialog("close");
                    var getScopedContacts = GetContactsInScope();
                    getScopedContacts.done(function (contactsInScope, status, xhr) {
                        FillContactsInScopeTable(contactsInScope.d.results);
                    });
                    getScopedContacts.fail(function (status, xhr) {
                        displayError(status);
                    });
                }
            });
        }
    }
}

// Open a modal dialog for editing an activity
function EditActivity(Id) {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Activities/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + Id,
        title: 'Edit Activity',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getActivities = GetActivities();
            getActivities.done(function (activities, status, xhr) {
                FillActivitiesTable(activities.d.results);
            });
            getActivities.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Open a modal dialog for adding an activity
function AddActivity() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Activities/NewItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ProjectId=" + getQueryStringParameter("ID"),
        title: 'Add an Activity',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getActivities = GetActivities();
            getActivities.done(function (activities, status, xhr) {
                FillActivitiesTable(activities.d.results);
            });
            getActivities.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Open a modal dialog for adding a note
function AddNote() {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Notes/NewItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ProjectId=" + getQueryStringParameter("ID"),
        title: 'Add a Note',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getNotes = GetNotes();
            getNotes.done(function (notes, status, xhr) {
                FillNotesTable(notes.d.results);
            });
            getNotes.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Open a modal dialog for editing an note
function EditNote(Id) {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/Notes/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + Id,
        title: 'Edit Note',
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getNotes = GetNotes();
            getNotes.done(function (notes, status, xhr) {
                FillNotesTable(notes.d.results);
            });
            getNotes.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Open a modal dialog for editing a check list item
function EditCheckListItem(Id) {
    var options = {
        url: _spPageContextInfo.webServerRelativeUrl + "/Pages/CheckList/EditItem.aspx?IsDlg=1&SPAppWebUrl=" + appweburl + "&ID=" + Id,
        title: "Edit Check List Item",
        dialogReturnValueCallback: DialogCallback
    };

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);

    function DialogCallback(dialogResult, returnValue) {
        if (dialogResult === 1) {
            var getCheckList = GetDealCheckList();
            getCheckList.done(function (checkListItems, status, xhr) {
                FillDealCheckListTable(checkListItems.d.results);
            });
            getCheckList.fail(function (status, xhr) {
                displayError(status);
            });
        }
    }
}

// Detect if the attachment indicator should be added to a datatable row
function GetItemAttachments(filterColumn, filterValue) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Commutation Documents')/Items/?$filter=" + filterColumn + "/Id eq " + filterValue;
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}

// Drop an associated activity with a corresponding drop reason
function DropActivity(activity, dropReasonId) {
    var endpointUrl = appweburl + "/_api/web/lists/getbytitle('Activity')/items(" + activity.Id + ")";
    var itemUpdates = {
        "__metadata": activity.__metadata,
        "ActivityStatus": "Dropped",
        "ActivityDroppedReasonId": dropReasonId
    };

    return $.ajax({
        url: itemUpdates.__metadata.uri,
        type: "POST",
        contentType: "application/json;odata=verbose",
        data: JSON.stringify(itemUpdates),
        headers: {
            "Accept": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val(),
            "X-HTTP-Method": "MERGE",
            "If-Match": itemUpdates.__metadata.etag
        }
    });
}

// Resize the dialog window
function ResizeDialogWindow(final) {
    var dlg = SP.UI.ModalDialog.get_childDialog();
    if (dlg != null) {
        dlg.autoSize();
    }
    if (final && $("#loadingdialog").hasClass('ui-dialog-content')) {
        if ($("#loadingdialog").dialog("isOpen")) {
            $("#loadingdialog").dialog("close");
        }
    }
}

// Get a permission group
function GetPermissionGroup(groupName, getUsers) {
    var endpointUrl = appweburl + "/_api/web/sitegroups/getbyname('" + groupName + "')";
    getUsers ? endpointUrl += "/users" : "";
    return $.ajax({
        url: endpointUrl,
        type: "GET",
        headers: { "accept": "application/json;odata=verbose" }
    });
}


//===============================
// End Utilities
//===============================