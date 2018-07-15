import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from "@angular/forms";
import { MessageService } from "primeng/components/common/messageservice";

import { WorkRequestsService } from "../../../work-requests.service";
import { ConfigurationService } from '../../../../../shared/index';

@Component({
  selector: 'app-new-form-goal-alignment',
  templateUrl: './new-form-goal-alignment.component.html',
  styleUrls: ['./new-form-goal-alignment.component.scss']
})
export class NewFormGoalAlignmentComponent implements OnInit {
  @Input() corporateGoalsFormGroup: FormGroup;
  public queryingCorporateGoals: boolean = false;
  public corporateGoalOptions: string[];

  constructor(private workRequestsService: WorkRequestsService, private messageService: MessageService, private configurationService: ConfigurationService) { }

  ngOnInit() {
    this.queryingCorporateGoals = true;
    this.configurationService.getCorporateGoals().finally(() => {
      this.queryingCorporateGoals = false;
    }).subscribe(corporateGoals => {
      this.corporateGoalOptions = corporateGoals;
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Unable to Retrieve Corporate Goals', detail: error });
    });
  }

}
