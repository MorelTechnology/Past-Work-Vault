(function () {
    'use strict';

    angular
        .module('commutationCentral')
        .controller('editProjectItemCtrl', ["$q", "$scope", "Upload", "projectId", "projectItemFactory", "$uibModalInstance", "siteUsersFactory", "leadOfficeItemFactory", "requestTypeItemFactory",
            "commutationTypeItemFactory", "commutationStatusItemFactory", "droppedReasonItemFactory", "dealPriorityItemFactory", "companyStatusItemFactory", "companyInScopeItemFactory",
            "$uibModal", "fairfaxEntityItemFactory", "fairfaxEntityInScopeItemFactory", "contactInScopeItemFactory", "financialEntryItemFactory", "projectDocumentItemFactory", "activityItemFactory",
            "activityDocumentItemFactory", "noteItemFactory", "noteDocumentItemFactory", "checklistItemFactory", "checklistDocumentItemFactory",
            function ($q, $scope, Upload, projectId, itemFactory, $uibModalInstance, siteUsersFactory, leadOfficeItemFactory, requestTypeItemFactory, commutationTypeItemFactory,
                commutationStatusItemFactory, droppedReasonItemFactory, dealPriorityItemFactory, companyStatusItemFactory, companyInScopeItemFactory, $uibModal, fairfaxEntityItemFactory,
                fairfaxEntityInScopeItemFactory, contactInScopeItemFactory, financialEntryItemFactory, projectDocumentItemFactory, activityItemFactory, activityDocumentItemFactory, 
                noteItemFactory, noteDocumentItemFactory, checklistItemFactory, checklistDocumentItemFactory) {
                $scope.projectId = projectId;
                $scope.formData = {
                    dropDownChoices: {}
                };

                //--------------------------------------
                //Populate dropdown choices
                //--------------------------------------
                $scope.inProgress = true;
                $scope.populateDropDowns = function () {
                    $scope.inProgress = true;
                    var promises = [];
                    promises.push(leadOfficeItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.leadOffices = response.data.d.results;
                        }));
                    promises.push(requestTypeItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.requestTypes = response.data.d.results;
                        }));
                    promises.push(commutationTypeItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.commutationTypes = response.data.d.results;
                        }));
                    promises.push(commutationStatusItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.commutationStatuses = response.data.d.results;
                        }));
                    promises.push(droppedReasonItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.droppedReasons = response.data.d.results;
                        }));
                    promises.push(dealPriorityItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.dealPriorities = response.data.d.results;
                        }));
                    promises.push(companyStatusItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.companyStatuses = response.data.d.results;
                        }));
                    promises.push(fairfaxEntityItemFactory.getAll()
                        .then(function (response) {
                            $scope.formData.dropDownChoices.fairfaxEntities = response.data.d.results;
                        }));

                    $q.all(promises).then(function () {
                        $scope.inProgress = false;
                        $scope.GetCurrentProject();
                    });
                    
                };
                $scope.populateDropDowns();

                //-------------------------------------
                //Get current project details when the form opens
                //This section populates advanced directives that rely on asynchronous data
                //-------------------------------------
                $scope.GetCurrentProject = function () {
                    $scope.inProgress = true;
                    itemFactory.getById(projectId)
                    .then(function (project) {
                        $scope.project = project.data.d;
                        if ($scope.project.PrimaryManagerID) $scope.project.PrimaryManager = [
                            {
                                Key: $scope.project.PrimaryManagerID.Name,
                                DisplayText: $scope.project.PrimaryManagerID.Title
                            }
                        ];
                        if ($scope.project.SecondaryManagerID) $scope.project.SecondaryManager = [
                            {
                                Key: $scope.project.SecondaryManagerID.Name,
                                DisplayText: $scope.project.SecondaryManagerID.Title
                            }
                        ];
                        if ($scope.project.LeadOffice) $scope.formData.selectedLeadOffice = $scope.formData.dropDownChoices.leadOffices.filter(function (obj) { return obj.Id === $scope.project.LeadOffice.ID; })[0];
                        if ($scope.project.RequestorID) $scope.project.Requestor = [
                            {
                                Key: $scope.project.RequestorID.Name,
                                DisplayText: $scope.project.RequestorID.Title
                            }
                        ];
                        if ($scope.project.RequestType) $scope.formData.selectedRequestType = $scope.formData.dropDownChoices.requestTypes.filter(function (obj) { return obj.ID === $scope.project.RequestType.ID; })[0];
                        if ($scope.project.RequestDate) $scope.formData.requestDate = new Date($scope.project.RequestDate);
                        if ($scope.project.CommutationType) $scope.formData.selectedCommutationType = $scope.formData.dropDownChoices.commutationTypes.filter(function (obj) { return obj.ID === $scope.project.CommutationType.ID; })[0];
                        if ($scope.project.CommutationStatus) $scope.formData.selectedCommutationStatus = $scope.formData.dropDownChoices.commutationStatuses.filter(function (obj) { return obj.ID === $scope.project.CommutationStatus.ID; })[0];
                        if ($scope.project.DroppedReason) $scope.formData.selectedDroppedReason = $scope.formData.dropDownChoices.droppedReasons.filter(function (obj) { return obj.ID === $scope.project.DroppedReason.ID; })[0];
                        $scope.commutationStatusChange();
                        if ($scope.project.OversightManagerID) $scope.project.OversightManager = [
                            {
                                Key: $scope.project.OversightManagerID.Name,
                                DisplayText: $scope.project.OversightManagerID.Title
                            }
                        ];
                        if ($scope.project.DealPriority) $scope.formData.selectedDealPriority = $scope.formData.dropDownChoices.dealPriorities.filter(function (obj) { return obj.ID === $scope.project.DealPriority.ID; })[0];
                        if ($scope.project.CompanyStatus) $scope.formData.selectedCompanyStatus = $scope.formData.dropDownChoices.companyStatuses.filter(function (obj) { return obj.ID === $scope.project.CompanyStatus.ID; })[0];
                        if ($scope.project.FinancialAuthorityGrantedBy) $scope.project.AuthorityGrantedBy = [
                            {
                                Key: $scope.project.FinancialAuthorityGrantedBy.Name,
                                DisplayText: $scope.project.FinancialAuthorityGrantedBy.Title
                            }
                        ];
                        if ($scope.project.FinancialAuthorityGrantedByDate) $scope.formData.authorityGrantedByDate = new Date($scope.project.FinancialAuthorityGrantedByDate);
                        $scope.getCompaniesInScope();
                        $scope.getFairfaxEntitiesInScope();
                        $scope.getContactsInScope();
                        $scope.getProjectDocuments();
                        $scope.getActivities();
                        $scope.getNotes();
                        $scope.getChecklists();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load Companies in Scope for the current project
                //---------------------------------------------------
                $scope.getCompaniesInScope = function () {
                    $scope.companiesInScopeInProgress = true;
                    companyInScopeItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.companiesInScope = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.companiesInScopeInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load Fairfax Entities In Scope for the current project
                //---------------------------------------------------
                $scope.getFairfaxEntitiesInScope = function () {
                    $scope.fairfaxEntitiesInScopeInProgress = true;
                    fairfaxEntityInScopeItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.fairfaxEntitiesInScope = response.data.d.results;
                        $scope.getFinancialEntries();
                    })
                    .finally(function () {
                        $scope.fairfaxEntitiesInScopeInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load Financial Entries for each Fairfax Entity in Scope
                //---------------------------------------------------
                $scope.getFinancialEntries = function () {
                    $scope.financialEntriesInProgress = true;
                    $scope.financialEntries = [];
                    var promises = [];
                    $scope.fairfaxEntitiesInScope.forEach(function (fairfaxEntity, index) {
                        promises.push(financialEntryItemFactory.getAll("FairfaxEntityInScope/ID eq " + fairfaxEntity.ID)
                        .then(function (response) {
                            var financialEntry = response.data.d.results[0];
                            financialEntry.FairfaxEntityName = fairfaxEntity.FairfaxEntity.FairfaxEntityName;
                            financialEntry.FormData = {
                                PreliminaryValuationDate: financialEntry.PreliminaryValuationDate ? new Date(financialEntry.PreliminaryValuationDate) : null,
                                FinalValuationDate: financialEntry.FinalValuationDate ? new Date(financialEntry.FinalValuationDate) : null
                            };
                            $scope.financialEntries.push(financialEntry);
                            $scope.financialEntries.sort(dynamicSort("FairfaxEntityName"));
                        }));
                    });

                    $q.all(promises).then(function () {
                        $scope.financialEntriesInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load Contacts in Scope for the current project
                //---------------------------------------------------
                $scope.getContactsInScope = function () {
                    $scope.contactsInScopeInProgress = true;
                    contactInScopeItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.contactsInScope = response.data.d.results;
                    })
                    .finally(function () {
                        $scope.contactsInScopeInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load project documents for the current project
                //---------------------------------------------------
                $scope.getProjectDocuments = function () {
                    $scope.projectDocumentUploadProgress = { percentComplete: 100, action: "Retrieving Documents", inProgress: true };
                    projectDocumentItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.projectDocuments = response.data.d.results;
                        for (var i = 0; i < $scope.projectDocuments.length; i++) {
                            $scope.projectDocuments[i].FormData = { Modified: formatDate(new Date($scope.projectDocuments[i].Modified)), Created: formatDate(new Date($scope.projectDocuments[i].Created)) };
                        }
                    })
                    .finally(function () {
                        $scope.projectDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                    });
                };

                //---------------------------------------------------
                //Load activity documents for the current project
                //---------------------------------------------------
                $scope.getActivityDocuments = function () {
                    activityDocumentItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.activityDocuments = response.data.d.results;
                        angular.forEach($scope.activities, function (activity, key) {
                            var documentsForActivity = $scope.activityDocuments.filter(function (obj) {
                                return obj.Activity.ID == activity.ID;
                            });
                            if (documentsForActivity.length > 0) {
                                $scope.activities[key].HasAttachments = true;
                            }
                        });
                    });
                };

                //---------------------------------------------------
                //Load activities for the current project
                //---------------------------------------------------
                $scope.getActivities = function () {
                    $scope.activitiesInProgress = true;
                    activityItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.activities = response.data.d.results;
                        $scope.getActivityDocuments();
                    })
                    .finally(function () {
                        $scope.activitiesInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load note documents for the current project
                //---------------------------------------------------
                $scope.getNoteDocuments = function () {
                    noteDocumentItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.noteDocuments = response.data.d.results;
                        angular.forEach($scope.notes, function (note, key) {
                            var documentsForNote = $scope.noteDocuments.filter(function (obj) {
                                return obj.Note.ID == note.ID;
                            });
                            if (documentsForNote.length > 0) {
                                $scope.notes[key].HasAttachments = true;
                            }
                        });
                    });
                };

                //---------------------------------------------------
                //Load notes for the current project
                //---------------------------------------------------
                $scope.getNotes = function () {
                    $scope.notesInProgress = true;
                    noteItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.notes = response.data.d.results;
                        $scope.getNoteDocuments();
                    })
                    .finally(function () {
                        $scope.notesInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Load checklist documents for the current project
                //---------------------------------------------------
                $scope.getChecklistDocuments = function () {
                    checklistDocumentItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.checklistDocuments = response.data.d.results;
                        angular.forEach($scope.checklists, function (checklist, key) {
                            var documentsForChecklist = $scope.checklistDocuments.filter(function (obj) {
                                return obj.Checklist.ID == checklist.ID;
                            });
                            if (documentsForChecklist.length > 0) {
                                $scope.checklists[key].HasAttachments = true;
                            }
                        });
                    });
                };

                //---------------------------------------------------
                //Load checklist for the current project
                //---------------------------------------------------
                $scope.getChecklists = function () {
                    $scope.checklistsInProgress = true;
                    checklistItemFactory.getAll("Project/ID eq " + $scope.project.Id)
                    .then(function (response) {
                        $scope.checklists = response.data.d.results;
                        $scope.getChecklistDocuments();
                    })
                    .finally(function () {
                        $scope.checklistsInProgress = false;
                    });
                };

                //---------------------------------------------------
                //Calculations
                //---------------------------------------------------
                $scope.financialSummary = {
                    PreliminaryAssumedUnpaidTotal: 0,
                    PreliminaryAssumedCaseTotal: 0,
                    PreliminaryAssumedIBNRTotal: 0,
                    PreliminaryAssumedTotalAllEntities: 0,
                    PreliminaryCededUnpaidTotal: 0,
                    PreliminaryCededCaseTotal: 0,
                    PreliminaryCededIBNRTotal: 0,
                    PreliminaryCededTotalAllEntities: 0,
                    PreliminaryUnpaidNetAllEntities: 0,
                    PreliminaryCaseNetAllEntities: 0,
                    PreliminaryIBNRNetAllEntities: 0,
                    PreliminaryTotalAllEntities: 0,
                    FinalAssumedUnpaid: 0,
                    FinalAssumedReserves: 0,
                    FinalAssumedIBNR: 0,
                    FinalAssumedTotal: 0,
                    FinalTransactionAssumed: 0,
                    FinalCededUnpaid: 0,
                    FinalCededReserves: 0,
                    FinalCededIBNR: 0,
                    FinalCededTotal: 0,
                    FinalTransactionCeded: 0,
                    FinalUnpaidNet: 0,
                    FinalReservesNet: 0,
                    FinalIBNRNet: 0,
                    FinalNetTotal: 0,
                    FinalTransactionBDPUsed: 0,
                    FinalCommutationValue: 0,
                    FinalTransactionNetCeded: 0,
                    FinalIncurredImpact: 0,
                    FinalTransactionCollateral: 0,
                    FinalUltimateImpactGross: 0,
                    FinalTransactionNetCash: 0,
                    FinalCreditProvision: 0,
                    FinalDisputeProvision: 0,
                    FinalGAAP: 0,
                    FinalPenalties: 0,
                    FinalSTAT: 0
                };
                $scope.getPreliminaryAssumed = function (financialEntry) {
                    var total = 0;
                    financialEntry.PreliminaryAssumedUnpaid && (total -= financialEntry.PreliminaryAssumedUnpaid);
                    financialEntry.PreliminaryAssumedCase && (total -= financialEntry.PreliminaryAssumedCase);
                    financialEntry.PreliminaryAssumedIBNR && (total -= financialEntry.PreliminaryAssumedIBNR);
                    return total;
                };
                $scope.getPreliminaryCeded = function (financialEntry) {
                    var total = 0;
                    financialEntry.PreliminaryCededUnpaid && (total += financialEntry.PreliminaryCededUnpaid);
                    financialEntry.PreliminaryCededCase && (total += financialEntry.PreliminaryCededCase);
                    financialEntry.PreliminaryCededIBNR && (total += financialEntry.PreliminaryCededIBNR);
                    return total;
                };
                $scope.getPreliminaryUnpaidNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.PreliminaryAssumedUnpaid && (total -= financialEntry.PreliminaryAssumedUnpaid);
                    financialEntry.PreliminaryCededUnpaid && (total += financialEntry.PreliminaryCededUnpaid);
                    return total;
                };
                $scope.getPreliminaryCaseNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.PreliminaryAssumedCase && (total -= financialEntry.PreliminaryAssumedCase);
                    financialEntry.PreliminaryCededCase && (total += financialEntry.PreliminaryCededCase);
                    return total;
                };
                $scope.getPreliminaryIBNRNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.PreliminaryAssumedIBNR && (total -= financialEntry.PreliminaryAssumedIBNR);
                    financialEntry.PreliminaryCededIBNR && (total += financialEntry.PreliminaryCededIBNR);
                    return total;
                };
                $scope.getPreliminaryTotal = function (financialEntry) {
                    var total = 0;
                    total += $scope.getPreliminaryAssumed(financialEntry);
                    total += $scope.getPreliminaryCeded(financialEntry);
                    return total;
                };
                $scope.getFinalAssumed = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalAssumedUnpaid && (total -= financialEntry.FinalAssumedUnpaid);
                    financialEntry.FinalAssumedReserves && (total -= financialEntry.FinalAssumedReserves);
                    financialEntry.FinalAssumedIBNR && (total -= financialEntry.FinalAssumedIBNR);
                    return total;
                };
                $scope.getFinalCeded = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalCededUnpaid && (total += financialEntry.FinalCededUnpaid);
                    financialEntry.FinalCededReserves && (total += financialEntry.FinalCededReserves);
                    financialEntry.FinalCededIBNR && (total += financialEntry.FinalCededIBNR);
                    return total;
                };
                $scope.getFinalUnpaidNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalAssumedUnpaid && (total -= financialEntry.FinalAssumedUnpaid);
                    financialEntry.FinalCededUnpaid && (total += financialEntry.FinalCededUnpaid);
                    return total;
                };
                $scope.getFinalReservesNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalAssumedReserves && (total -= financialEntry.FinalAssumedReserves);
                    financialEntry.FinalCededReserves && (total += financialEntry.FinalCededReserves);
                    return total;
                };
                $scope.getFinalIBNRNet = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalAssumedIBNR && (total -= financialEntry.FinalAssumedIBNR);
                    financialEntry.FinalCededIBNR && (total += financialEntry.FinalCededIBNR);
                    return total;
                };
                $scope.getFinalNetTotal = function (financialEntry) {
                    var total = 0;
                    total += $scope.getFinalAssumed(financialEntry);
                    total += $scope.getFinalCeded(financialEntry);
                    return total;
                };
                $scope.getFinalTransactionNetCeded = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalTransactionCeded && (total += financialEntry.FinalTransactionCeded);
                    financialEntry.FinalTransactionBDPUsed && (total += financialEntry.FinalTransactionBDPUsed);
                    return total;
                };
                $scope.getFinalIncurredImpact = function (financialEntry) {
                    var total = 0;
                    financialEntry.FinalCommutationValue && (total += financialEntry.FinalCommutationValue);
                    financialEntry.FinalAssumedUnpaid && (total -= financialEntry.FinalAssumedUnpaid);
                    financialEntry.FinalAssumedReserves && (total -= financialEntry.FinalAssumedReserves);
                    financialEntry.FinalCededUnpaid && (total += financialEntry.FinalCededUnpaid);
                    financialEntry.FinalCededReserves && (total += financialEntry.FinalCededReserves);
                    return total;
                };
                $scope.getFinalUltimateImpactGross = function (financialEntry) {
                    var total = 0;
                    total += $scope.getFinalIncurredImpact(financialEntry);
                    financialEntry.FinalAssumedIBNR && (total -= financialEntry.FinalAssumedIBNR);
                    financialEntry.FinalCededIBNR && (total += financialEntry.FinalCededIBNR);
                    return total;
                };
                $scope.getFinalTransactionNetCash = function (financialEntry) {
                    var total = 0;
                    total += $scope.getFinalTransactionNetCeded(financialEntry);
                    financialEntry.FinalTransactionCollateral && (total += financialEntry.FinalTransactionCollateral);
                    financialEntry.FinalTransactionAssumed && (total -= financialEntry.FinalTransactionAssumed);
                    return total;
                };
                $scope.getFinalGAAP = function (financialEntry) {
                    var total = 0;
                    total += $scope.getFinalUltimateImpactGross(financialEntry);
                    financialEntry.FinalCreditProvision && (total -= financialEntry.FinalCreditProvision);
                    financialEntry.FinalDisputeProvision && (total -= financialEntry.FinalDisputeProvision);
                    return total;
                };
                $scope.getFinalSTAT = function (financialEntry) {
                    var total = 0;
                    total += $scope.getFinalGAAP(financialEntry);
                    financialEntry.FinalPenalties && (total -= financialEntry.FinalPenalties);
                    return total;
                };
                $scope.getPreliminaryAssumedUnpaidTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryAssumedUnpaid && (total -= financialEntries[i].PreliminaryAssumedUnpaid);
                        }
                    }
                    $scope.financialSummary.PreliminaryAssumedUnpaidTotal = total;
                    return total;
                };
                $scope.getPreliminaryAssumedCaseTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryAssumedCase && (total -= financialEntries[i].PreliminaryAssumedCase);
                        }
                    }
                    $scope.financialSummary.PreliminaryAssumedCaseTotal = total;
                    return total;
                };
                $scope.getPreliminaryAssumedIBNRTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryAssumedIBNR && (total -= financialEntries[i].PreliminaryAssumedIBNR);
                        }
                    }
                    $scope.financialSummary.PreliminaryAssumedIBNRTotal = total;
                    return total;
                };
                $scope.getPreliminaryAssumedTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryAssumedUnpaidTotal;
                        total += $scope.financialSummary.PreliminaryAssumedCaseTotal;
                        total += $scope.financialSummary.PreliminaryAssumedIBNRTotal;
                    }
                    $scope.financialSummary.PreliminaryAssumedTotalAllEntities = total;
                    return total;
                };
                $scope.getPreliminaryCededUnpaidTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryCededUnpaid && (total += financialEntries[i].PreliminaryCededUnpaid);
                        }
                    }
                    $scope.financialSummary.PreliminaryCededUnpaidTotal = total;
                    return total;
                };
                $scope.getPreliminaryCededCaseTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryCededCase && (total += financialEntries[i].PreliminaryCededCase);
                        }
                    }
                    $scope.financialSummary.PreliminaryCededCaseTotal = total;
                    return total;
                };
                $scope.getPreliminaryCededIBNRTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].PreliminaryCededIBNR && (total += financialEntries[i].PreliminaryCededIBNR);
                        }
                    }
                    $scope.financialSummary.PreliminaryCededIBNRTotal = total;
                    return total;
                };
                $scope.getPreliminaryCededTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryCededUnpaidTotal;
                        total += $scope.financialSummary.PreliminaryCededCaseTotal;
                        total += $scope.financialSummary.PreliminaryCededIBNRTotal;
                    }
                    $scope.financialSummary.PreliminaryCededTotalAllEntities = total;
                    return total;
                };
                $scope.getPreliminaryUnpaidNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryAssumedUnpaidTotal;
                        total += $scope.financialSummary.PreliminaryCededUnpaidTotal;
                    }
                    $scope.financialSummary.PreliminaryUnpaidNetAllEntities = total;
                    return total;
                };
                $scope.getPreliminaryCaseNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryAssumedCaseTotal;
                        total += $scope.financialSummary.PreliminaryCededCaseTotal;
                    }
                    $scope.financialSummary.PreliminaryCaseNetAllEntities = total;
                    return total;
                };
                $scope.getPreliminaryIBNRNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryAssumedIBNRTotal;
                        total += $scope.financialSummary.PreliminaryCededIBNRTotal;
                    }
                    $scope.financialSummary.PreliminaryIBNRNetAllEntities = total;
                    return total;
                };
                $scope.getPreliminaryTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.PreliminaryAssumedTotalAllEntities;
                        total += $scope.financialSummary.PreliminaryCededTotalAllEntities;
                    }
                    $scope.financialSummary.PreliminaryTotalAllEntities = total;
                    return total;
                };
                $scope.getAuthorityGrantedNet = function (project) {
                    var total = 0;
                    if (project) {
                        project.FinancialAuthorityAssumed && (total -= project.FinancialAuthorityAssumed);
                        project.FinancialAuthorityCeded && (total += project.FinancialAuthorityCeded);
                    }
                    return total;
                };
                $scope.getFinalAssumedUnpaidTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalAssumedUnpaid && (total -= financialEntries[i].FinalAssumedUnpaid);
                        }
                    }
                    $scope.financialSummary.FinalAssumedUnpaid = total;
                    return total;
                };
                $scope.getFinalAssumedReservesTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalAssumedReserves && (total -= financialEntries[i].FinalAssumedReserves);
                        }
                    }
                    $scope.financialSummary.FinalAssumedReserves = total;
                    return total;
                };
                $scope.getFinalAssumedIBNRTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalAssumedIBNR && (total -= financialEntries[i].FinalAssumedIBNR);
                        }
                    }
                    $scope.financialSummary.FinalAssumedIBNR = total;
                    return total;
                };
                $scope.getFinalAssumedTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalAssumedUnpaid;
                        total += $scope.financialSummary.FinalAssumedReserves;
                        total += $scope.financialSummary.FinalAssumedIBNR;
                    }
                    $scope.financialSummary.FinalAssumedTotal = total;
                    return total;
                };
                $scope.getFinalTransactionAssumedTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalTransactionAssumed && (total -= financialEntries[i].FinalTransactionAssumed);
                        }
                    }
                    $scope.financialSummary.FinalTransactionAssumed = total;
                    return total;
                };
                $scope.getFinalCededUnpaidTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalCededUnpaid && (total += financialEntries[i].FinalCededUnpaid);
                        }
                    }
                    $scope.financialSummary.FinalCededUnpaid = total;
                    return total;
                };
                $scope.getFinalCededReservesTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalCededReserves && (total += financialEntries[i].FinalCededReserves);
                        }
                    }
                    $scope.financialSummary.FinalCededReserves = total;
                    return total;
                };
                $scope.getFinalCededIBNRTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalCededIBNR && (total += financialEntries[i].FinalCededIBNR);
                        }
                    }
                    $scope.financialSummary.FinalCededIBNR = total;
                    return total;
                };
                $scope.getFinalCededTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalCededUnpaid;
                        total += $scope.financialSummary.FinalCededReserves;
                        total += $scope.financialSummary.FinalCededIBNR;
                    }
                    $scope.financialSummary.FinalCededTotal = total;
                    return total;
                };
                $scope.getFinalTransactionCededTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalTransactionCeded && (total += financialEntries[i].FinalTransactionCeded);
                        }
                    }
                    $scope.financialSummary.FinalTransactionCeded = total;
                    return total;
                };
                $scope.getFinalUnpaidNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalAssumedUnpaid;
                        total += $scope.financialSummary.FinalCededUnpaid;
                    }
                    $scope.financialSummary.FinalUnpaidNet = total;
                    return total;
                };
                $scope.getFinalReservesNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalAssumedReserves;
                        total += $scope.financialSummary.FinalCededReserves;
                    }
                    $scope.financialSummary.FinalReservesNet = total;
                    return total;
                };
                $scope.getFinalIBNRNetAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalAssumedIBNR;
                        total += $scope.financialSummary.FinalCededIBNR;
                    }
                    $scope.financialSummary.FinalIBNRNet = total;
                    return total;
                };
                $scope.getFinalNetTotalAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalAssumedTotal;
                        total += $scope.financialSummary.FinalCededTotal;
                    }
                    $scope.financialSummary.FinalNetTotal = total;
                    return total;
                };
                $scope.getFinalTransactionBDPUsedTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalTransactionBDPUsed && (total += financialEntries[i].FinalTransactionBDPUsed);
                        }
                    }
                    $scope.financialSummary.FinalTransactionBDPUsed = total;
                    return total;
                };
                $scope.getFinalCommutationValueTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalCommutationValue && (total += financialEntries[i].FinalCommutationValue);
                        }
                    }
                    $scope.financialSummary.FinalCommutationValue = total;
                    return total;
                };
                $scope.getFinalTransactionNetCededAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalTransactionCeded;
                        total += $scope.financialSummary.FinalTransactionBDPUsed;
                    }
                    $scope.financialSummary.FinalTransactionNetCeded = total;
                    return total;
                };
                $scope.getFinalIncurredImpactAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalCommutationValue + $scope.financialSummary.FinalUnpaidNet + $scope.financialSummary.FinalReservesNet;
                    }
                    $scope.financialSummary.FinalIncurredImpact = total;
                    return total;
                };
                $scope.getFinalTransactionCollateralTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalTransactionCollateral && (total += financialEntries[i].FinalTransactionCollateral);
                        }
                    }
                    $scope.financialSummary.FinalTransactionCollateral = total;
                    return total;
                };
                $scope.getFinalUltimateImpactGrossAllEntities = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalIncurredImpact + $scope.financialSummary.FinalIBNRNet;
                    }
                    $scope.financialSummary.FinalUltimateImpactGross = total;
                    return total;
                };
                $scope.getFinalTransactionNetCashTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalTransactionAssumed + $scope.financialSummary.FinalTransactionNetCeded + $scope.financialSummary.FinalTransactionCollateral;
                    }
                    $scope.financialSummary.FinalTransactionNetCash = total;
                    return total;
                };
                $scope.getFinalCreditProvisionTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalCreditProvision && (total += financialEntries[i].FinalCreditProvision);
                        }
                    }
                    $scope.financialSummary.FinalCreditProvision = total;
                    return total;
                };
                $scope.getFinalDisputeProvisionTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalDisputeProvision && (total += financialEntries[i].FinalDisputeProvision);
                        }
                    }
                    $scope.financialSummary.FinalDisputeProvision = total;
                    return total;
                };
                $scope.getFinalGAAPTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalUltimateImpactGross;
                        total -= $scope.financialSummary.FinalCreditProvision;
                        total -= $scope.financialSummary.FinalDisputeProvision;
                    }
                    $scope.financialSummary.FinalGAAP = total;
                    return total;
                };
                $scope.getFinalPenaltiesTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        for (var i = 0; i < financialEntries.length; i++) {
                            financialEntries[i].FinalPenalties && (total += financialEntries[i].FinalPenalties);
                        }
                    }
                    $scope.financialSummary.FinalPenalties = total;
                    return total;
                };
                $scope.getFinalSTATTotal = function (financialEntries) {
                    var total = 0;
                    if (financialEntries) {
                        total += $scope.financialSummary.FinalGAAP;
                        total -= $scope.financialSummary.FinalPenalties;
                    }
                    $scope.financialSummary.FinalGAAP = total;
                    return total;
                };

                //---------------------------------------------------
                //Miscellaneous form options
                //---------------------------------------------------
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                $scope.calendarPopup1 = {
                    opened: false
                };
                $scope.openCalendarPopup1 = function () {
                    $scope.calendarPopup1.opened = true;
                };
                $scope.calendarPopup2 = {
                    opened: false
                };
                $scope.openCalendarPopup2 = function () {
                    $scope.calendarPopup2.opened = true;
                };
                $scope.calendarPopup3 = {
                    opened: false
                };
                $scope.openCalendarPopup3 = function () {
                    $scope.calendarPopup3.opened = true;
                };
                $scope.calendarPopup4 = {
                    opened: false
                };
                $scope.openCalendarPopup4 = function () {
                    $scope.calendarPopup4.opened = true;
                };
                $scope.droppedReasonVisible = true;
                $scope.commutationStatusChange = function () {
                    if ($scope.formData.selectedCommutationStatus.CommutationStatusName !== "Dropped") {
                        $scope.formData.selectedDroppedReason = $scope.formData.dropDownChoices.droppedReasons.filter(function (obj) { return obj.DroppedReasonName === "None" })[0];
                        $scope.droppedReasonVisible = false;
                    } else { $scope.droppedReasonVisible = true; }
                    $scope.validateDroppedReason();
                };
                $scope.validateDroppedReason = function () {
                    if ($scope.formData.selectedCommutationStatus.CommutationStatusName === "Dropped" && $scope.formData.selectedDroppedReason.DroppedReasonName === "None") {
                        $scope.editItemForm.DroppedReason.$setValidity("droppedReasonError", false);
                        $scope.droppedReasonInvalidReason = "Dropped Reason cannot be 'None'";
                    } else if ($scope.formData.selectedCommutationStatus.CommutationStatusName !== "Dropped" && $scope.formData.selectedDroppedReason.DroppedReasonName !== "None") {
                        $scope.editItemForm.DroppedReason.$setValidity("droppedReasonError", false);
                        $scope.droppedReasonInvalidReason = "Dropped Reason should be 'None'";
                    } else {
                        $scope.editItemForm.DroppedReason.$setValidity("droppedReasonError", true);
                        $scope.droppedReasonInvalidReason = "Dropped Reason is valid";
                    }
                };
                $scope.contactsInScopeDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        }
                    ],
                    order: [1, 'asc']
                };
                $scope.convertToCurrency = function (input) {
                    return input.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,');
                };
                $scope.financialEntryAlerts = [];
                $scope.closeFinancialEntryAlert = function (index) {
                    $scope.financialEntryAlerts.splice(index, 1);
                };
                $scope.projectDocumentUploadProgress = {
                    percentComplete: 0,
                    action: null,
                    inProgress: false
                };
                $scope.projectDocumentsDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };
                $scope.activitiesDTOptions = {
                    columnDefs: [
                        {
                            targets: [0],
                            orderable: false,
                            searchable: false
                        },
                    ],
                    order: [1, 'asc']
                };

                //---------------------------------------------------
                //Button actions
                //---------------------------------------------------
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
                $scope.editItem = function (project) {
                    $scope.inProgress = true;
                    project.itemId = project.Id;
                    project.LeadOfficeId = $scope.formData.selectedLeadOffice.Id;
                    project.RequestTypeId = $scope.formData.selectedRequestType.Id;
                    project.RequestDate = $scope.formData.requestDate;
                    project.CommutationTypeId = $scope.formData.selectedCommutationType.Id;
                    project.CommutationStatusId = $scope.formData.selectedCommutationStatus.Id;
                    project.DroppedReasonId = $scope.formData.selectedDroppedReason.Id;
                    project.DealPriorityId = $scope.formData.selectedDealPriority.Id;
                    project.CompanyStatusId = $scope.formData.selectedCompanyStatus.Id;
                    project.FinancialAuthorityGrantedByDate = $scope.formData.authorityGrantedByDate;
                    siteUsersFactory.getByLoginName($scope.project.PrimaryManager[0].Key)
                    .then(function (primaryManager) {
                        project.PrimaryManagerIDId = primaryManager.data.d.Id;
                        if (project.SecondaryManager && project.SecondaryManager.length > 0 && project.SecondaryManager[0].Key) {
                            return siteUsersFactory.getByLoginName($scope.project.SecondaryManager[0].Key);
                        }
                        return null;
                    })
                    .then(function (secondaryManager) {
                        if (secondaryManager && secondaryManager.data.d) {
                            project.SecondaryManagerIDId = secondaryManager.data.d.Id;
                        }
                        if (!secondaryManager) { project.SecondaryManagerIDId = null; }
                        return siteUsersFactory.getByLoginName($scope.project.Requestor[0].Key);
                    })
                    .then(function (requestor) {
                        project.RequestorIDId = requestor.data.d.Id;
                        if (project.OversightManager && project.OversightManager.length > 0 && project.OversightManager[0].Key) {
                            return siteUsersFactory.getByLoginName($scope.project.OversightManager[0].Key);
                        }
                        //return null;
                    })
                    .then(function (oversightManager) {
                        if (oversightManager && oversightManager.data.d) {
                            project.OversightManagerIDId = oversightManager.data.d.Id;
                        }
                        if (!oversightManager) { project.OversightManagerIDId = null; }
                        if (project.AuthorityGrantedBy && project.AuthorityGrantedBy.length > 0 && project.AuthorityGrantedBy[0].Key) {
                            return siteUsersFactory.getByLoginName($scope.project.AuthorityGrantedBy[0].Key);
                        }
                        //return null;
                    })
                    .then(function (authorityGrantedBy) {
                        if (authorityGrantedBy && authorityGrantedBy.data.d) {
                            project.FinancialAuthorityGrantedById = authorityGrantedBy.d.Id;
                        }
                        if (!authorityGrantedBy) { project.FinancialAuthorityGrantedById = null; }
                    })
                    .then(function () {
                        return itemFactory.update(project);
                    })
                    .then(function (response) {
                        $uibModalInstance.close();
                    })
                    .finally(function () {
                        $scope.inProgress = false;
                    });
                };
                $scope.addCompanyInScope = function (companyName) {
                    $scope.companiesInScopeInProgress = true;
                    var companyToAdd = {
                        ProjectId: $scope.project.Id,
                        CompanyName: companyName
                    };
                    companyInScopeItemFactory.addNew(companyToAdd)
                    .then(function (response) {
                        $scope.getCompaniesInScope();
                    })
                    .finally(function () {
                        $scope.companiesInScopeInProgress = false;
                    });
                };
                $scope.editCompanyInScope = function (itemId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/CompaniesInScope/Edit.html",
                        controller: "editCompanyInScopeItemCtrl",
                        backdrop: 'static',
                        windowClass: 'edit-project-dialog',
                        resolve: {
                            itemId: function () {
                                return itemId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getCompaniesInScope();
                    });
                };
                $scope.deleteCompanyInScope = function (companyId) {
                    $scope.companiesInScopeInProgress = true;
                    companyInScopeItemFactory.remove(companyId)
                    .then(function (response) {
                        $scope.getCompaniesInScope();
                    })
                    .finally(function () {
                        $scope.companiesInScopeInProgress = false;
                    });
                };
                $scope.addFairfaxEntityInScope = function (fairfaxEntity) {
                    $scope.fairfaxEntitiesInScopeInProgress = true;
                    $scope.fairfaxEntitiesInScopeAction = "Adding Fairfax Entity to Project...";
                    var fairfaxEntityToAdd = {
                        ProjectId: $scope.project.Id,
                        FairfaxEntityId: fairfaxEntity.Id
                    };
                    fairfaxEntityInScopeItemFactory.addNew(fairfaxEntityToAdd)
                    .then(function (response) {
//                            $scope.getFairfaxEntitiesInScope();
                            var financialEntryToAdd = {
                                FairfaxEntityInScopeId: response.data.d.Id
                            };
                            $scope.fairfaxEntitiesInScopeAction = "Creating financial entry for Fairfax Entity...";
                            return financialEntryItemFactory.addNew(financialEntryToAdd);
                    })
                    .then(function (response) {
                        $scope.fairfaxEntitiesInScopeAction = "";
                    })
                    .finally(function () {
                        $scope.getFairfaxEntitiesInScope();
                        $scope.fairfaxEntitiesInScopeInProgress = false;
                    });
                };
                $scope.deleteFairfaxEntityInScope = function (fairfaxEntityId) {
                    $scope.fairfaxEntitiesInScopeInProgress = true;
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to remove this Fairfax Entity from this project? Doing so will also remove any financial data for this entity as it relates to this project."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            fairfaxEntityInScopeItemFactory.remove(fairfaxEntityId)
                            .then(function (response) {
                                $scope.getFairfaxEntitiesInScope();
                            })
                            .finally(function () {
                                $scope.fairfaxEntitiesInScopeInProgress = false;
                            });
                        } else {
                            $scope.fairfaxEntitiesInScopeInProgress = false;
                        }
                    });

                };
                $scope.createContact = function () {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Contacts/Add.html",
                        controller: "addContactItemCtrl",
                        backdrop: 'static',
                        resolve: {
                            projectId: function () {
                                return $scope.project.Id;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getContactsInScope();
                    });
                };
                $scope.deleteContactInScope = function (contactId) {
                    $scope.contactsInScopeInProgress = true;
                    contactInScopeItemFactory.remove(contactId)
                    .then(function (response) {
                        $scope.getContactsInScope();
                    })
                    .finally(function () {
                        $scope.contactsInScopeInProgress = false;
                    });
                };
                $scope.assignContact = function () {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Contacts/Assign.html",
                        controller: "assignContactItemCtrl",
                        backdrop: "static",
                        size: "lg",
                        resolve: {
                            project: function () {
                                return $scope.project;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getContactsInScope();
                    });
                };
                $scope.editContactInScope = function (contactId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Contacts/Edit.html",
                        controller: "editContactItemCtrl",
                        backdrop: "static",
                        resolve: {
                            contactId: function () {
                                return contactId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getContactsInScope();
                    });
                };
                $scope.saveFinancialEntry = function (financialEntry) {
                    $scope.financialEntriesInProgress = true;
                    $scope.financialEntryInProgress = true;
                    financialEntry.PreliminaryValuationDate = financialEntry.FormData.PreliminaryValuationDate;
                    financialEntry.FinalValuationDate = financialEntry.FormData.FinalValuationDate;
                    financialEntryItemFactory.update(financialEntry)
                    .then(function (response) {
                        $scope.financialEntryAlerts.push({ msg: 'Successfully updated financial entry', type: 'success', timeout: 3000 });
                    })
                    .finally(function () {
                        $scope.financialEntriesInProgress = false;
                        $scope.financialEntryInProgress = false;
                    });
                };
                $scope.attachProjectDocuments = function (uploadFiles) {
                    if (uploadFiles && uploadFiles.length) {
                        angular.forEach(uploadFiles, function (file) {
                            $scope.projectDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: true };
                            projectDocumentItemFactory.uploadFile(file, $scope.project.Id)
                            .then(function (successResp) {
                                $scope.projectDocumentUploadProgress = successResp;
                                $scope.getProjectDocuments();
                            }, function (errorResp) {
                                $scope.projectDocumentUploadProgress = errorResp;
                            }, function (notifyResp) {
                                $scope.projectDocumentUploadProgress = notifyResp;
                            });
                        });
                    }
                };
                $scope.deleteProjectDocument = function (projectDocumentId) {
                    $scope.projectDocumentUploadProgress = { percentComplete: 100, action: "Deleting project document", inProgress: true };
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this project document? Doing so will remove it from the system with no way to recover it."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            projectDocumentItemFactory.remove(projectDocumentId)
                            .then(function (successResponse) {
                                $scope.getProjectDocuments();
                            })
                            .finally(function () {
                                $scope.projectDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                            });
                        } else {
                            $scope.projectDocumentUploadProgress = { percentComplete: 0, action: null, inProgress: false };
                        }
                    });
                };
                $scope.createActivity = function () {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Activities/Add.html",
                        controller: "addActivityItemCtrl",
                        backdrop: 'static',
                        windowClass: 'new-activity-dialog',
                        resolve: {
                            project: function () {
                                return $scope.project;
                            },
                            projectPreResolved: true
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getActivities();
                    });
                };
                $scope.deleteActivity = function (activityId) {
                    $scope.activitiesInProgress = true;
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this activity? Doing so will also delete any documents attached to this activity."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            activityItemFactory.remove(activityId)
                            .then(function (successResponse) {
                                $scope.getActivities();
                            })
                            .finally(function () {
                                $scope.activitiesInProgress = false;
                            });
                        } else {
                            $scope.activitiesInProgress = false;
                        }
                    });
                };
                $scope.editActivity = function (activityId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Activities/Edit.html",
                        controller: "editActivityItemCtrl",
                        backdrop: "static",
                        windowClass: 'edit-activity-dialog',
                        resolve: {
                            activityId: function () {
                                return activityId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getActivities();
                    });
                };
                $scope.createNote = function () {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Notes/Add.html",
                        controller: "addNoteItemCtrl",
                        backdrop: 'static',
                        windowClass: 'new-note-dialog',
                        resolve: {
                            project: function () {
                                return $scope.project;
                            },
                            projectPreResolved: true
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getNotes();
                    });
                };
                $scope.deleteNote = function (noteId) {
                    $scope.notesInProgress = true;
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/GenericDialogs/YesNoDialog.html",
                        controller: "yesNoDialogCtrl",
                        backdrop: "static",
                        resolve: {
                            dialogProperties: {
                                Title: "Confirm Delete",
                                Body: "Are you sure you want to delete this note? Doing so will also delete any documents attached to this note."
                            }
                        }
                    });
                    modalInstance.result.then(function (confirmed) {
                        if (confirmed) {
                            noteItemFactory.remove(noteId)
                            .then(function (successResponse) {
                                $scope.getNotes();
                            })
                            .finally(function () {
                                $scope.notesInProgress = false;
                            });
                        } else {
                            $scope.notesInProgress = false;
                        }
                    });
                };
                $scope.editNote = function (noteId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Notes/Edit.html",
                        controller: "editNoteItemCtrl",
                        backdrop: "static",
                        windowClass: 'edit-note-dialog',
                        resolve: {
                            noteId: function () {
                                return noteId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getNotes();
                    });
                };
                $scope.editChecklist = function (checklistId) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: "../App/Templates/Checklists/Edit.html",
                        controller: "editChecklistItemCtrl",
                        backdrop: "static",
                        windowClass: 'edit-checklist-dialog',
                        resolve: {
                            checklistId: function () {
                                return checklistId;
                            }
                        }
                    });
                    modalInstance.result.then(function () {
                        $scope.getChecklists();
                    });
                };
            }]);
})();

//Miscellaneous functions
function dynamicSort(property) {
    var sortOrder = 1;
    if (property[0] === "-") {
        sortOrder = -1;
        property = property.substr(1);
    }
    return function (a, b) {
        var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
        return result * sortOrder;
    }
}

function formatDate(date) {
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var year = date.getFullYear();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours > 12 ? 'pm' : 'am';
    hours = hours % 12 || hours;
    minutes = ('0' + minutes).slice(-2);
    var strTime = month + "/" + day + "/" + year + " " + hours + ":" + minutes + " " + ampm;
    return strTime;
}
