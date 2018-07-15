import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import 'rxjs/add/operator/switchMap';
import { MessageService } from "primeng/components/common/messageservice";

import { WorkRequestsService } from "../../work-requests.service";
import { WorkRequest } from "../../../../shared/models";

@Component({
  selector: 'app-new-work-request-success',
  templateUrl: './new-work-request-success.component.html',
  styleUrls: ['./new-work-request-success.component.scss']
})
export class NewWorkRequestSuccessComponent implements OnInit {
  public queryingWorkRequest: boolean = false;
  public workRequest: WorkRequest;
  public retrievalError: boolean = false;

  constructor(private route: ActivatedRoute, private workRequestsService: WorkRequestsService, private messageService: MessageService) { }

  ngOnInit() {
    this.route.paramMap.switchMap((params: ParamMap) => {
      this.queryingWorkRequest = true;
      return this.workRequestsService.getWorkRequest(+params.get('id'));
    }).subscribe((workRequest: WorkRequest) => {
      this.workRequest = workRequest;
      this.queryingWorkRequest = false;
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Failed to Get Work Request', detail: error });
      this.retrievalError = true;
      this.queryingWorkRequest = false;
    });
  }

}
