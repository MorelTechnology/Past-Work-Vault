import { Component, OnInit } from '@angular/core';
import { MenuItem } from "primeng/components/common/api";
import { MessageService } from "primeng/components/common/messageservice";
import { ActivatedRoute, ParamMap } from "@angular/router";
import 'rxjs/add/operator/switchMap';

import { WorkRequestsService } from "../../work-requests.service";
import { BreadcrumbService } from "../../../../shared/services";
import { WorkRequest } from "../../../../shared/models";
import { WorkRequestStatus } from "../../../../shared/enums";

@Component({
  selector: 'app-edit-work-request-success',
  templateUrl: './edit-work-request-success.component.html',
  styleUrls: ['./edit-work-request-success.component.scss']
})
export class EditWorkRequestSuccessComponent implements OnInit {
  private breadcrumb: MenuItem[] = [{ label: 'Work Requests', routerLink: '/work-requests', routerLinkActiveOptions: { exact: true } }];
  public queryingWorkRequest: boolean = false;
  public retrievalError: boolean = false;
  public workRequest: WorkRequest;
  public successContents: string;
  private currentWorkRequestStatus: WorkRequestStatus;

  constructor(private breadcrumbService: BreadcrumbService, private route: ActivatedRoute, private workRequestsService: WorkRequestsService, private messageService: MessageService) { }

  ngOnInit() {
    // Get the work request referenced in the querystring parameters
    this.route.paramMap.switchMap((params: ParamMap) => {
      this.queryingWorkRequest = true;
      return this.workRequestsService.getWorkRequest(+params.get('id'));
    }).subscribe((workRequest: WorkRequest) => {
      this.workRequest = workRequest;
      this.queryingWorkRequest = false;
      // Somewhat of a hack, we need to display the friendly text of the work request status
      this.currentWorkRequestStatus = this.workRequestsService.requestStatusStringToEnum(workRequest['Status'].toString());
      // Based on the current status of the work request, display an appropriate message on the page
      switch (this.currentWorkRequestStatus) {
        case WorkRequestStatus.SubmittedToPortfolioManager:
          this.successContents = "Thank you for submitting a work request to the Portfolio Manager.";
          break;
        case WorkRequestStatus.ReadyForPrioritization:
          this.successContents = "You have successfully authorized this work request ready for prioritization.";
          break;
        case WorkRequestStatus.ReturnedToAssociate:
          this.successContents = `You have successfully returned Work Request ${this.workRequest.RequestID} to ${this.workRequest.RequestorDisplayName}`;
          break;
        case WorkRequestStatus.Denied:
          this.successContents = `Work Request ${this.workRequest.RequestID} has been denied and is now locked for editing`;
          break;
        case WorkRequestStatus.SubmittedToProductOwner:
          this.successContents = `You have successfully submitted Work Request ${this.workRequest.RequestID} to a Product Owner`;
          break;
        case WorkRequestStatus.SubmittedToDigitalStrategy:
          this.successContents = `You have successfully submitted Work Request ${this.workRequest.RequestID} to Digital Strategy`;
          break;
        case WorkRequestStatus.ReadyForScheduling:
          this.successContents = `Your request is now ready for scheduling.`;
          break;
        default:
          this.successContents = "No changes have been made to this request.";
          break;
      }
      this.breadcrumb.push({ label: workRequest.Title, routerLink: [] });
      this.breadcrumbService.setBreadcrumb(this.breadcrumb);
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Failed to Get Work Request', detail: error });
      this.retrievalError = true;
      this.queryingWorkRequest = false;
    });
  }

}
