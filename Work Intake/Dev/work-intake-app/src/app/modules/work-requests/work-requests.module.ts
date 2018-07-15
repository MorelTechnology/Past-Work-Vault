import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { PanelModule, InputTextModule, InputTextareaModule, AutoCompleteModule, DataTableModule, SharedModule, ConfirmDialogModule, CheckboxModule, RadioButtonModule, DialogModule, TooltipModule, OverlayPanelModule, DropdownModule, CalendarModule } from "primeng/primeng";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { WorkRequestsRoutingModule } from "./work-requests-routing.module";
import { WorkRequestsComponent } from './work-requests.component';
import { NewWorkRequestComponent } from './new-work-request/new-work-request.component';
import { NewFormGeneralComponent } from './new-work-request/new-work-request-container/new-form-general/new-form-general.component';
import { NewFormDetailComponent } from './new-work-request/new-work-request-container/new-form-detail/new-form-detail.component';
import { WorkRequestsService } from "./work-requests.service";
import { NewWorkRequestContainerComponent } from './new-work-request/new-work-request-container/new-work-request-container.component';
import { NewWorkRequestSuccessComponent } from './new-work-request/new-work-request-success/new-work-request-success.component';
import { ConfigurationService } from "../../shared/services";
import { EditWorkRequestComponent } from './edit-work-request/edit-work-request.component';
import { EditWorkRequestSuccessComponent } from './edit-work-request/edit-work-request-success/edit-work-request-success.component';
import { NewFormGoalAlignmentComponent } from './new-work-request/new-work-request-container/new-form-goal-alignment/new-form-goal-alignment.component';
import { MySubmissionsComponent } from './my-submissions/my-submissions.component';
import { MyActionItemsComponent } from './my-action-items/my-action-items.component';

@NgModule({
  imports: [
    CommonModule,
    WorkRequestsRoutingModule,
    ReactiveFormsModule,
    PanelModule,
    InputTextModule,
    NgbModule,
    InputTextareaModule,
    AutoCompleteModule,
    DataTableModule,
    SharedModule,
    FormsModule,
    ConfirmDialogModule,
    CheckboxModule,
    RadioButtonModule,
    DialogModule,
    TooltipModule,
    OverlayPanelModule,
    DropdownModule,
    CalendarModule
  ],
  declarations: [WorkRequestsComponent, NewWorkRequestComponent, NewFormGeneralComponent, NewFormDetailComponent, NewWorkRequestContainerComponent, NewWorkRequestSuccessComponent, EditWorkRequestComponent, EditWorkRequestSuccessComponent, NewFormGoalAlignmentComponent, MySubmissionsComponent, MyActionItemsComponent],
  providers: [WorkRequestsService, ConfigurationService]
})
export class WorkRequestsModule { }
