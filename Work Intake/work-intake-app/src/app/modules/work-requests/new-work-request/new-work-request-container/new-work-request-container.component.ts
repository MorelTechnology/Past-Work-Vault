import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from "@angular/forms";
import { Router } from "@angular/router";
import { NgbTabChangeEvent } from "@ng-bootstrap/ng-bootstrap";
import { MessageService } from "primeng/components/common/messageservice";

import { WorkRequestStatus, WorkRequest, UserService, User, NotificationType, NotificationService, ConfigurationService, RealizationOfImpact, BusinessValueUnit } from "../../../../shared";
import { WorkRequestsService } from "../../work-requests.service";
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-new-work-request-container',
  templateUrl: './new-work-request-container.component.html',
  styleUrls: ['./new-work-request-container.component.scss']
})
export class NewWorkRequestContainerComponent implements OnInit {
  private pageTitle: string = "New Work Request";
  public workRequestForm: FormGroup;
  public workRequestFormSubmitted: boolean = false;
  public nextTab: string = "tab2";
  public previousTab: string = null;
  public currentFormGroup: FormGroup;
  public currentUser: User;
  public portfolioManager = environment.portfolioManager;
  public isProd = environment.production;
  public isProductManager: boolean = false;
  public impersonateRole: string;
  public roles: string[] = ["", "Product Owner", "Portfolio Manager"];
  private isDopm: boolean = false;

  constructor(private fb: FormBuilder, private workRequestsService: WorkRequestsService, private messageService: MessageService, private router: Router,
    private userService: UserService, private notificationService: NotificationService, private configurationService: ConfigurationService) { }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(user => {
      this.currentUser = user;
      this.configurationService.getProductManagers().subscribe(productManagers => {
        this.isProductManager = productManagers.some(productManager => {
          return this.currentUser.objectSid === productManager.objectSid;
        });
      });
      this.buildWorkRequestForm();
      this.currentFormGroup = <FormGroup>this.workRequestForm.controls.general;
    });
  }

  private buildWorkRequestForm(): void {
    // Break up the request form into several groups that can be passed to the child components
    this.workRequestForm = this.fb.group({
      corporateGoals: this.fb.group({
        title: new FormControl("", [Validators.required]),
        corporateGoals: new FormControl("", [Validators.required]),
        goalSupport: new FormControl("", [Validators.required]),
        supportsDept: new FormControl(null, [Validators.required]),
        deptGoalSupport: new FormControl("")
      }),
      detail: this.fb.group({
        goal: new FormControl("", [Validators.required]),
        NonImplementImpact: new FormControl("", [Validators.required]),
        RealizationOfImpact: new FormControl("",[Validators.required]),
        businessValueUnit: new FormControl(BusinessValueUnit.Dollars, [Validators.required]),
        businessValueAmount: new FormControl("", [Validators.required, Validators.pattern(/^[0-9]*$/)]),
        requestedCompletionDate: new FormControl(""),
        status: new FormControl(WorkRequestStatus.SubmittedToProductOwner, [Validators.required]),
        conditionsOfSatisfaction: new FormControl("", [Validators.required])
      }),
      general: this.fb.group({
        requestId: (0),
        requestor: new FormControl(this.currentUser.objectSid, [Validators.required]),
        Manager: new FormControl("", [Validators.required])
      })
    });
    // Department goal support is conditionally required if Supports Department Goals equals true. To accomplish that, we can subscribe to
    // the Supports Department Goals control and set the required validator if necessary
    this.workRequestForm.controls.corporateGoals.get('supportsDept').valueChanges.subscribe((supportsDept: boolean) => {
      if (supportsDept) {
        this.workRequestForm.controls.corporateGoals.get('deptGoalSupport').setValidators([Validators.required]);
      } else {
        this.workRequestForm.controls.corporateGoals.get('deptGoalSupport').setValidators([]);
      }
      this.workRequestForm.controls.corporateGoals.get('deptGoalSupport').updateValueAndValidity();
    });
    this.workRequestForm.controls.detail.get('businessValueUnit').valueChanges.subscribe((businessValueUnit: number) => {
      if (businessValueUnit === 1) {
        this.workRequestForm.controls.detail.get('businessValueAmount').setValidators([Validators.required, Validators.pattern(/^[0-9]*$/)]);
      } else if (businessValueUnit === 2) {
        this.workRequestForm.controls.detail.get('businessValueAmount').setValidators([Validators.required, Validators.pattern(/^[0-9]+(\.[0-9]{1,2})?$/)]);
      }
    });
  

    // Requested Completion Date is conditionally required if Realization of Impact is set to Very High. To accomplish that, we can subscribe to
    // the Requested Completion Date control and set the required validator if necessary
    this.workRequestForm.controls.detail.get('RealizationOfImpact').valueChanges.subscribe((impactValues: number) => {
      if (impactValues === 1) //Very High
       {
        this.workRequestForm.controls.detail.get('requestedCompletionDate').setValidators([Validators.required]);
      } else {
        this.workRequestForm.controls.detail.get('requestedCompletionDate').setValidators([]);
      }
      this.workRequestForm.controls.detail.get('requestedCompletionDate').updateValueAndValidity();
    });
    
  }




  /**
   * When the user changes tabs, we internally set what the current, next and previous tabs are, so we can show or hide buttons in the DOM appropriately
   * @param event The event that triggered the change
   */
  public onTabChange(event: NgbTabChangeEvent): void {
    switch (event.nextId) {
      case "tab1":
        this.nextTab = "tab2";
        this.previousTab = null;
        this.currentFormGroup = <FormGroup>this.workRequestForm.controls.corporateGoals;
        break;
      case "tab2":
        this.nextTab = "tab3";
        this.previousTab = "tab1";
        this.currentFormGroup = <FormGroup>this.workRequestForm.controls.detail;
        break;
      case "tab3":
        this.nextTab = null;
        this.previousTab = "tab2";

    }
  }

  /**
   * Construct a work request item, submit it to the createWorkRequest function, and then send out notifications as necessary
   */
  public onSubmit(): void {
    this.workRequestFormSubmitted = true;
    let newWorkRequest = this.prepareSubmit();
    this.workRequestsService.createWorkRequest(newWorkRequest).subscribe(requestId => {
      this.workRequestFormSubmitted = false;
      this.messageService.add({ severity: 'success', summary: 'Successfully Created Work Request', detail: `Created work request titled ${newWorkRequest['title']}` });
      this.buildWorkRequestForm();
      if (this.isDopm) {
        this.router.navigate(["/work-requests/new/success", requestId]);
      } else {
        let notificationRecipient = this.isProductManager ? environment.portfolioManagerDistribution : newWorkRequest.Manager;
        let notificationType = this.isProductManager ? NotificationType.PortfolioManagerSubmission : NotificationType.ManagerSubmission;
        this.notificationService.sendNotification(requestId, notificationRecipient, notificationType).subscribe(success => {
          this.router.navigate(["/work-requests/new/success", requestId]);
        }, error => {
          this.router.navigate(["/work-requests/new/success", requestId]);
        });
      }
    }, error => {
      this.workRequestFormSubmitted = false;
      this.messageService.add({ severity: 'error', summary: 'Failed to Create Work Request', detail: error });
    });
  }

  /**
   * Merge the values of the different form groups into one work request object
   * Manager has to be set separately, as we need the objectSid of the nested FormControl object
   */
  private prepareSubmit(): WorkRequest {
    let workRequest = <WorkRequest>Object.assign(this.workRequestForm.controls.general.value, this.workRequestForm.controls.detail.value, this.workRequestForm.controls.corporateGoals.value);
    workRequest.Manager = this.workRequestForm.value.general.Manager.objectSid;
    if (this.currentUser.sAMAccountName === this.portfolioManager || (!this.isProd && this.impersonateRole === 'Portfolio Manager')) {
      workRequest.Status = WorkRequestStatus.ReadyForPrioritization;
      this.isDopm = true;
    } else if (this.isProductManager || (!this.isProd && this.impersonateRole === 'Product Owner')) {
      workRequest.Status = WorkRequestStatus.SubmittedToPortfolioManager;
      this.isProductManager = true;
    }
    return workRequest;
  }

}
