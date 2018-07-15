import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from "@angular/common";
import { AutoComplete } from "primeng/primeng";
import { MenuItem } from "primeng/components/common/api";
import { MessageService } from "primeng/components/common/messageservice";
import { ConfirmationService } from "primeng/components/common/confirmationservice";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import 'rxjs/add/operator/switchMap';
import { FormGroup, FormControl, Validators, FormBuilder, ReactiveFormsModule } from "@angular/forms";

import { WorkRequestsService } from "../work-requests.service";
import { BreadcrumbService, UserService, NotificationService, ConfigurationService } from "../../../shared/services";
import { WorkRequest, User } from "../../../shared/models";
import { WorkRequestStatus, NotificationType, BusinessValueUnit, RealizationOfImpact } from "../../../shared/enums";
import { environment } from "../../../../environments/environment";
import { SelectItem } from 'primeng/components/common/selectitem';


@Component({
  selector: 'app-edit-work-request',
  templateUrl: './edit-work-request.component.html',
  styleUrls: ['./edit-work-request.component.scss']
})
export class EditWorkRequestComponent implements OnInit {
  private breadcrumb: MenuItem[] = [{ label: 'Work Requests', routerLink: '/work-requests', routerLinkActiveOptions: { exact: true } }];
  @ViewChild(AutoComplete)
  autoCompleteComponent: AutoComplete;
  public workRequest: WorkRequest;
  public currentUser: User;
  public currentManager: User;
  public pageError: boolean = false;
  public showReturnDialog: boolean = false;
  public showDenyDialog: boolean = false;
  public returnReason: string;
  public denyReason: string;
  public workRequestForm: FormGroup;
  public currentWorkRequestStatus: WorkRequestStatus;
  public businessValueUnit: BusinessValueUnit;
  public impactValue: RealizationOfImpact;
  public businessValueAmount: string;
  public isProd: boolean = false;
  public portfolioManager = environment.portfolioManager;
  public corporateGoalOptions: string[];
  public operationInProgress: boolean = false;
  public editMode: boolean = false;
  public productManagerSearchResults: User[];
  public businessValueUnits: SelectItem[] = [
    { value: 'Dollars per year', label: 'Dollars per year' },
    { value: 'Hours per year', label: 'Hours per year' }
  ];
  public impactValues: SelectItem[] = [
    { value: 'Unspecified', label: 'Unspecified' },
    { value: 'Very High', label: 'Very High' },
    { value: 'High', label: 'High' },
    { value: 'Low', label: 'Low' },
    { value: 'Very Low', label: 'Very Low' }
  ];

  public impersonateRole: string;
  public roles: string[] = ["", "Product Owner", "Portfolio Manager"];

  public isProductManager: boolean = false;
  private isDopm: boolean = false;

  constructor(private breadcrumbService: BreadcrumbService, private route: ActivatedRoute, private workRequestsService: WorkRequestsService,
    private messageService: MessageService, private userService: UserService,
    private configurationService: ConfigurationService, private location: Location, private confirmationService: ConfirmationService,
    private fb: FormBuilder, private notificationService: NotificationService,
    private router: Router) {
    this.isProd = environment.production;
  }

  ngOnInit() {
    this.route.paramMap.switchMap((params: ParamMap) => {
      return this.workRequestsService.getWorkRequest(+params.get('id'));
    }).subscribe((workRequest: WorkRequest) => {
      this.workRequest = workRequest;
      this.breadcrumb.push({ label: workRequest.Title, routerLink: [] });
      this.breadcrumbService.setBreadcrumb(this.breadcrumb);
      this.currentWorkRequestStatus = this.workRequestsService.requestStatusStringToEnum(workRequest['Status'].toString());
      this.businessValueUnit = this.workRequestsService.businessValueUnitStringToEnum(workRequest['BusinessValueUnit'].toString());
      this.businessValueAmount = this.parseBusinessValueAmount(this.businessValueUnit, this.workRequest.BusinessValueAmount);
      this.impactValue = this.workRequestsService.impactValueStringToEnum(workRequest['RealizationOfImpact'].toString());
      this.userService.getUser(workRequest.Manager).subscribe(manager => {
        this.currentManager = manager;
        this.configurationService.getCorporateGoals().subscribe(options => {
          this.corporateGoalOptions = options;
          this.buildWorkRequestForm();
        }, error => {
          this.pageError = true;
          this.messageService.add({ severity: 'error', summary: 'Unable to Retrieve Corporate Goal Options', detail: error });
        });
      }, error => {
        this.pageError = true;
        this.messageService.add({ severity: 'error', summary: 'Unable to get Product Owner', detail: error });
      });
    }, error => {
      this.pageError = true;
      this.messageService.add({ severity: 'error', summary: 'Unable to Retrieve Work Request', detail: error });
      this.breadcrumb.push({ label: 'N/A', routerLink: [] });
      this.breadcrumbService.setBreadcrumb(this.breadcrumb);
    });
    this.userService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
      this.configurationService.getProductManagers().subscribe(productManagers => {
        this.isProductManager = productManagers.some(productManager => {
          return this.currentUser.objectSid === productManager.objectSid;
        });
      });
    });
  }

  private parseBusinessValueAmount(businessValueUnit: BusinessValueUnit, businessValueAmount: number): string {
    switch (businessValueUnit) {
      case (BusinessValueUnit.Dollars):
        return `\$${businessValueAmount} / year`;
      case (BusinessValueUnit.Hours):
        return `${businessValueAmount} hours / year`;
      default:
        return this.workRequestsService.businessValueUnitToString(businessValueUnit);
    }
  }

  private buildWorkRequestForm(): void {
    this.workRequestForm = this.fb.group({
      RequestID: new FormControl(this.workRequest.RequestID, [Validators.required]),
      Status: new FormControl(this.workRequest.Status, [Validators.required]),
      Requestor: new FormControl(this.workRequest.Requestor, [Validators.required]),
      Manager: new FormControl({ value: this.currentManager, disabled: true }, [Validators.required]),
      Title: new FormControl({ value: this.workRequest.Title, disabled: true }, [Validators.required]),
      Goal: new FormControl({ value: this.workRequest.Goal, disabled: true }, [Validators.required]),
      NonImplementImpact: new FormControl({ value: this.workRequest.NonImplementImpact, disabled: true }, [Validators.required]),
      RealizationOfImpact: new FormControl({ value: this.workRequest.RealizationOfImpact, disabled: true }, [Validators.required]),
      BusinessValueUnit: new FormControl(this.workRequest.BusinessValueUnit, [Validators.required]),
      BusinessValueAmount: new FormControl(this.workRequest.BusinessValueAmount, [Validators.required, Validators.pattern(/^[0-9]*$/)]),
      RequestedCompletionDate: new FormControl({ value: this.workRequest.RequestedCompletionDate, disabled: true }),
      LastModified: new FormControl(""),
      StatusDate: new FormControl(""),
      CorporateGoals: new FormControl({ value: this.workRequest.CorporateGoals, disabled: true }, [Validators.required]),
      GoalSupport: new FormControl({ value: this.workRequest.GoalSupport, disabled: true }, [Validators.required]),
      SupportsDept: new FormControl({ value: this.workRequest.SupportsDept, disabled: true }, [Validators.required]),
      DeptGoalSupport: new FormControl({ value: this.workRequest.DeptGoalSupport, disabled: true }),
      ConditionsOfSatisfaction: new FormControl({ value: this.workRequest.ConditionsOfSatisfaction, disabled: true }, [Validators.required])
    });
    this.workRequestForm.get('SupportsDept').valueChanges.subscribe((supportsDept: boolean) => {
      let ctrl = this.workRequestForm.get('DeptGoalSupport');
      if (supportsDept) {
        ctrl.setValidators([Validators.required]);
      } else {
        ctrl.setValidators([]);
      }
      ctrl.updateValueAndValidity();
    });
    this.workRequestForm.get('BusinessValueAmount').valueChanges.subscribe((businessValueAmount: number) => {
      this.businessValueAmount = this.parseBusinessValueAmount(this.businessValueUnit, businessValueAmount);
    });
  }

  public goBack(): void {
    this.location.back();
  }

  /**
   * Examine the current status of the work request, and send it forward to the next recipient
   */
  public sendForward(): void {
    let message: string, nextStatus: WorkRequestStatus, notificationRecipient: string[] = [], notificationType: NotificationType;
    switch (this.currentWorkRequestStatus) {
      case WorkRequestStatus.SubmittedToProductOwner:
        message = "Are you sure you want to send this request to the Portfolio Manager?";
        nextStatus = WorkRequestStatus.SubmittedToPortfolioManager;
        //notificationRecipient = [environment.portfolioManager];
        notificationRecipient = [environment.portfolioManagerDistribution];
        notificationType = NotificationType.PortfolioManagerSubmission;
        break;
      case WorkRequestStatus.SubmittedToPortfolioManager:
        message = "Are you sure this request is ready for prioritization?";
        nextStatus = WorkRequestStatus.ReadyForPrioritization;
        notificationRecipient = [this.workRequest.Manager, this.workRequest.Requestor];
        notificationType = NotificationType.Approved;
        break;
    }

    this.confirmationService.confirm({
      message: message,
      header: "Confirm",
      icon: "fa fa-question-circle",
      accept: () => {
        this.workRequestForm.setControl('Status', new FormControl(nextStatus, [Validators.required]));
        this.workRequestForm.setControl('StatusDate', new FormControl(new Date(), [Validators.required]));
        let updatedWorkRequest = <WorkRequest>this.workRequestForm.getRawValue();
        updatedWorkRequest.Manager = this.workRequestForm.getRawValue().Manager.objectSid;
        this.workRequestsService.updateWorkRequest(updatedWorkRequest).subscribe(rowsUpdated => {
          if (notificationRecipient && notificationType) {
            this.notificationService.sendNotificationBatch(this.workRequest.RequestID, notificationRecipient, notificationType).finally(() => {
              this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
            }).subscribe();
          } else {
            this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
          }
        }, error => {
          this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
        });
      }
    });
  }

  /**
   * Send the current request to digital strategy
   */
  public sendToDigitalStrategy(): void {
    let message: string, nextStatus: WorkRequestStatus, notificationRecipient: string[] = [], notificationType: NotificationType;
    message = "Are you sure you want to send this request to Digital Strategy?";
    nextStatus = WorkRequestStatus.SubmittedToDigitalStrategy;
    notificationRecipient = [environment.digitalStrategyDistribution];
    notificationType = NotificationType.DigitalStrategySubmission;

    this.confirmationService.confirm({
      message: message,
      header: "Confirm",
      icon: "fa fa-question-circle",
      accept: () => {
        this.workRequestForm.setControl('Status', new FormControl(nextStatus, [Validators.required]));
        this.workRequestForm.setControl('StatusDate', new FormControl(new Date(), [Validators.required]));
        let updatedWorkRequest = <WorkRequest>this.workRequestForm.getRawValue();
        updatedWorkRequest.Manager = this.workRequestForm.getRawValue().Manager.objectSid;
        this.workRequestsService.updateWorkRequest(updatedWorkRequest).subscribe(rowsUpdated => {
          this.notificationService.sendNotificationBatch(this.workRequest.RequestID, notificationRecipient, notificationType).finally(() => {
            this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
          }).subscribe(() => {

          }, error => {
            this.messageService.add({ severity: 'warning', summary: 'Failed to Notify Digital Strategy', detail: 'There was an issue sending a notification email to Digital Strategy' });
          });
        }, error => {
          this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
        });
      }
    });
  }

  /**
   * Mark the current request as ready for scheduling
   */
  public readyForScheduling(): void {
    let message: string, nextStatus: WorkRequestStatus, notificationRecipient: string[] = [], notificationType: NotificationType;
    message = "Are you sure you want to make this request ready for scheduling?";
    nextStatus = WorkRequestStatus.ReadyForScheduling;
    notificationRecipient = [this.workRequest.Manager, this.workRequest.Requestor, environment.digitalStrategyDistribution];
    notificationType = NotificationType.ReadyForScheduling;

    this.confirmationService.confirm({
      message: message,
      header: "Confirm",
      icon: "fa fa-question-circle",
      accept: () => {
        this.workRequestForm.setControl('Status', new FormControl(nextStatus, [Validators.required]));
        this.workRequestForm.setControl('StatusDate', new FormControl(new Date(), [Validators.required]));
        let updatedWorkRequest = <WorkRequest>this.workRequestForm.getRawValue();
        updatedWorkRequest.Manager = this.workRequestForm.getRawValue().Manager.objectSid;
        this.workRequestsService.updateWorkRequest(updatedWorkRequest).subscribe(rowsUpdated => {
          this.notificationService.sendNotificationBatch(this.workRequest.RequestID, notificationRecipient, notificationType).finally(() => {
            this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
          }).subscribe(() => {
          }, error => {
            this.messageService.add({ severity: 'warning', summary: 'Failed to Send Notification Emails', detail: 'There was an issue sending notification emails to one or more recipients' });
          });
        }, error => {
          this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
        });
      }
    });
  }

  /**
   * Returns a request to the person who created it
   */
  public returnRequest(): void {
    this.operationInProgress = true;
    this.workRequestsService.rejectWorkRequest(this.workRequest.RequestID, false, this.returnReason).finally(() => {
      this.operationInProgress = false;
    }).subscribe(() => {
      this.notificationService.sendNotification(this.workRequest.RequestID, this.workRequest.Requestor, NotificationType.ReturnToAssociate, this.returnReason).finally(() => {
        this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
      }).subscribe(() => { }, error => {
        this.messageService.add({ severity: 'warning', summary: `Couldn't Send Notification Email`, detail: `We were unable to email the requestor. Please notify them that you have returned their request` });
      });
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
    });
  }

  /**
   * Denies a request
   */
  public denyRequest(): void {
    this.operationInProgress = true;
    this.workRequestsService.rejectWorkRequest(this.workRequest.RequestID, true, this.denyReason).finally(() => {
      this.operationInProgress = false;
    }).subscribe(() => {
      let notificationRecipients: string[] = [this.workRequest.Requestor];
      if ((this.currentUser.sAMAccountName === this.portfolioManager || !this.isProd) && (this.currentWorkRequestStatus === WorkRequestStatus.SubmittedToPortfolioManager || this.currentWorkRequestStatus === WorkRequestStatus.SubmittedToDigitalStrategy)) {
        notificationRecipients = [...notificationRecipients, this.workRequest.Manager];
      }
      this.notificationService.sendNotificationBatch(this.workRequest.RequestID, notificationRecipients, NotificationType.Denied, this.denyReason).finally(() => {
        this.router.navigate(['/work-requests/request/success', this.workRequest.RequestID]);
      }).subscribe(() => { },
        error => {
          this.messageService.add({ severity: 'warning', summary: `Couldn't Send Notification Email`, detail: `We were unable to email the requestor. Please notify them that you have denied their request` });
        });
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
    });
  }

  public searchProductManagers(query: string): void {
    this.configurationService.getProductManagers().subscribe(productManagers => {
      this.productManagerSearchResults = productManagers.filter(productManager => {
        return productManager.displayName.toLowerCase().includes(query.toLowerCase());
      });
    });
  }

  public onDropdownSelect() {
    if (this.autoCompleteComponent.panelVisible) this.autoCompleteComponent.hide();
  }

  public toggleEdit(editMode: boolean, resetForm: boolean = true): void {
    this.editMode = editMode;
    if (editMode) {
      this.workRequestForm.get('Manager').enable();
      this.workRequestForm.get('Title').enable();
      this.workRequestForm.get('CorporateGoals').enable();
      this.workRequestForm.get('GoalSupport').enable();
      this.workRequestForm.get('SupportsDept').enable();
      this.workRequestForm.get('DeptGoalSupport').enable();
      this.workRequestForm.get('Goal').enable();
      this.workRequestForm.get('NonImplementImpact').enable();
      this.workRequestForm.get('RealizationOfImpact').enable();
      this.workRequestForm.get('RequestedCompletionDate').enable();
      this.workRequestForm.get('ConditionsOfSatisfaction').enable();
    } else {
      this.workRequestForm.get('Manager').disable();
      this.workRequestForm.get('Title').disable();
      this.workRequestForm.get('CorporateGoals').disable();
      this.workRequestForm.get('GoalSupport').disable();
      this.workRequestForm.get('SupportsDept').disable();
      this.workRequestForm.get('DeptGoalSupport').disable();
      this.workRequestForm.get('Goal').disable();
      this.workRequestForm.get('NonImplementImpact').disable();
      this.workRequestForm.get('RealizationOfImpact').disable();
      this.workRequestForm.get('RequestedCompletionDate').disable();
      this.workRequestForm.get('ConditionsOfSatisfaction').disable();
      if (resetForm) {
        this.businessValueAmount = this.parseBusinessValueAmount(this.businessValueUnit, this.workRequest.BusinessValueAmount);
        this.buildWorkRequestForm();
      }
    }
  }

  /**
   * submitEdit
   */
  public submitEdit(updateStatus = false) {
    this.operationInProgress = true;
    let updatedWorkRequest = this.prepareSubmit(updateStatus);
    this.workRequestsService.updateWorkRequest(updatedWorkRequest).finally(() => {
      this.operationInProgress = false;
    }).subscribe(successRecords => {
      this.messageService.add({ severity: 'success', summary: 'Successfully Submitted Work Request', detail: `Updated work request titled ${updatedWorkRequest.Title}` });
      if (updatedWorkRequest.Status !== this.workRequest.Status) {
        if (!this.isDopm && !this.isProductManager) {
          this.notificationService.sendNotification(updatedWorkRequest.RequestID, updatedWorkRequest.Manager, NotificationType.ManagerSubmission).finally(() => {
            this.router.navigate(["/work-requests/request/success", updatedWorkRequest.RequestID]);
          }).subscribe(() => {
          }, error => {
            this.messageService.add({ severity: 'warning', summary: `Couldn't Send Notification Email`, detail: `We were unable to email the Product Owner. Please notify them that you have re-submitted your request` });
          });
        }
      }
      this.toggleEdit(false, false);
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Failed to Update Work Request', detail: error });
    });
  }

  /**
   * Merge the values of the different form groups into one work request object
   * Manager has to be set separately, as we need the objectSid of the nested FormControl object
   */
  private prepareSubmit(updateStatus = true): WorkRequest {
    let workRequest = <WorkRequest>Object.assign(this.workRequestForm.getRawValue());
    workRequest.Manager = this.workRequestForm.getRawValue().Manager.objectSid;
    if (updateStatus) {
      if (this.currentUser.sAMAccountName === this.portfolioManager || (!this.isProd && this.impersonateRole === 'Portfolio Manager')) {
        workRequest.Status = WorkRequestStatus.ReadyForPrioritization;
        this.isDopm = true;
      } else if (this.isProductManager || (!this.isProd && this.impersonateRole === 'Product Owner')) {
        workRequest.Status = WorkRequestStatus.SubmittedToPortfolioManager;
        this.isProductManager = true;
      } else {
        workRequest.Status = WorkRequestStatus.SubmittedToProductOwner;
      }

      workRequest.StatusDate = new Date();
    }
    workRequest.LastModified = new Date();
    return workRequest;
  }

}
