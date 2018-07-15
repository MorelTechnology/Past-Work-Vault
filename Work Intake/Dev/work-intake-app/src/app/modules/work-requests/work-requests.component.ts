import { Component, OnInit } from '@angular/core';
import { MenuItem } from "primeng/components/common/api";
import { MessageService } from "primeng/components/common/messageservice";
import { ConfirmationService } from "primeng/components/common/confirmationservice";
import { Router } from "@angular/router";
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/finally';
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";

import { WorkRequest } from "../../shared/models";
import { BreadcrumbService } from "../../shared/services";
import { WorkRequestsService } from "./work-requests.service";

@Component({
  selector: 'app-work-requests',
  templateUrl: './work-requests.component.html',
  styleUrls: ['./work-requests.component.scss']
})
export class WorkRequestsComponent implements OnInit {
  private pageTitle: string = "Work Requests";
  private breadcrumb: MenuItem[] = [];
  public workRequests: WorkRequest[] = [];
  public loadingWorkRequests: boolean = false;

  constructor(private messageService: MessageService, private breadcrumbService: BreadcrumbService, private confirmationService: ConfirmationService, private router: Router, private workRequestsService: WorkRequestsService) { }

  ngOnInit() {
    this.breadcrumb = [
      { label: "Work Requests", routerLink: "/work-requests" }
    ];
    this.breadcrumbService.setBreadcrumb(this.breadcrumb);
    this.getWorkRequests();
  }

  private getWorkRequests(): void {
    this.loadingWorkRequests = true;
    // Get the list of work requests from the work requests service
    // When the observable is done emitting values, sort the resulting array by ID by default
    this.workRequestsService.getWorkRequests().finally(() => {
      this.workRequests.sort((a, b) => {
        return a.RequestID - b.RequestID;
      });
      this.loadingWorkRequests = false;
    }).subscribe(request => {
      // The observable emits one request at a time, so we use the spread operator to push the new values into the array
      // PrimeNG documentation indicates that the data source array on a datatable should be immutable, and the spread operator
      // adds to the collection without mutation, versus using array.push()
      this.workRequests = [...this.workRequests, request];
    }, error => {
      this.messageService.add({ severity: "error", summary: "Failed to Retrieve Work Requests", detail: error });
    });
  }

  public goToRequest(requestId: number): void {
    this.router.navigate(['/work-requests/request', requestId]);
  }

  public exportXLSX(): void {
    // Uses the SheetJS/js-xlsx library to export to Excel
    // This code taken from the SheetJS demos at https://github.com/SheetJS/js-xlsx/tree/master/demos/angular2
    let exportData = this.workRequests.map(request => {
      let formattedRequest = {
        /*Benefit: request.Benefit,
        'Conditions of Satisfaction': request.ConditionsOfSatisfaction,
        'Corporate Goals': request.CorporateGoals ? request.CorporateGoals.toString() : '',
        'Department Goal Support': request.DeptGoalSupport,
        'Goal': request.Goal,
        'Goal Support': request.GoalSupport,
        'Last Modified': request.LastModified,
        'Manager': request.ManagerDisplayName,
        'Problem': request.Problem,
        //'Quantification': request.Quantification,
        'Business Value Unit': request.BusinessValueUnit,
        'Business Value Amount': request.BusinessValueAmount,
        'Request ID': request.RequestID,
        'Requestor': request.RequestorDisplayName,
        'Status': request.Status,
        'Status Date': request.StatusDate,
        'Supports Department Goals': request.SupportsDept,
        'Title': request.Title*/
        ID: request.RequestID,
        Title: request.Title,
        Goal: request.Goal,
        'Business Value Unit': request.BusinessValueUnit,
        'Business Value Amount': request.BusinessValueAmount,
        'Impact of Non-Implementation': request.NonImplementImpact,
        'Realization of Impact': request.RealizationOfImpact,
        'Requested Completion Date': request.RequestedCompletionDate,
        'Corporate Goals': this.corporateGoalsEval(request.CorporateGoals),
        'Goal Support': request.GoalSupport,
        'Supports Department Goals': request.SupportsDept,
        'Department Goal Support': request.DeptGoalSupport,
        'Conditions of Satisfaction': request.ConditionsOfSatisfaction,
        Requestor: request.RequestorDisplayName,
        Owner: request.ManagerDisplayName,
        'Last Modified': request.LastModified,
        Status: request.Status,
        'Status Date': request.StatusDate
      }
      return formattedRequest;
    });
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    const wbout: string = XLSX.write(wb, { bookType: 'xlsx', type: 'binary', cellDates: true });
    saveAs(new Blob([this.s2ab(wbout)]), 'WorkRequests.xlsx');
  }

  // Taken from SheetJS demo at https://github.com/SheetJS/js-xlsx/blob/master/demos/angular2/src/app/sheetjs.component.ts
  private s2ab(s: string): ArrayBuffer {
    const buf: ArrayBuffer = new ArrayBuffer(s.length);
    const view: Uint8Array = new Uint8Array(buf);
    for (let i = 0; i !== s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }

  // Arrays weren't displaying in the export, so we attempt to turn it
  // into a string first.  If it can't be done, it's either null or
  // shitty data, so instead return a typical excel #Error string.
  private corporateGoalsEval(corporateGoals:object)
  {
    try {
      var goals = corporateGoals.toString();
      return goals;
    }
    catch(error)
    {
      return "#Error";
    }
  }
}
