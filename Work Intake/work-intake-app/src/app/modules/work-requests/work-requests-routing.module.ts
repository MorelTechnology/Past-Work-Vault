import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { WorkRequestsComponent } from './work-requests.component';
import { NewWorkRequestComponent } from "./new-work-request/new-work-request.component";
import { NewWorkRequestContainerComponent } from "./new-work-request/new-work-request-container/new-work-request-container.component";
import { NewWorkRequestSuccessComponent } from "./new-work-request/new-work-request-success/new-work-request-success.component";
import { EditWorkRequestComponent } from "./edit-work-request/edit-work-request.component";
import { EditWorkRequestSuccessComponent } from "./edit-work-request/edit-work-request-success/edit-work-request-success.component";
import { MySubmissionsComponent } from './my-submissions/my-submissions.component';
import { MyActionItemsComponent } from './my-action-items/my-action-items.component';

const routes: Routes = [
  { path: '', component: WorkRequestsComponent },
  {
    path: 'new', component: NewWorkRequestComponent,
    children: [
      { path: '', component: NewWorkRequestContainerComponent },
      { path: 'success/:id', component: NewWorkRequestSuccessComponent }
    ]
  },
  { path: 'request/:id', component: EditWorkRequestComponent },
  { path: 'request/success/:id', component: EditWorkRequestSuccessComponent },
  { path: 'my-submissions', component: MySubmissionsComponent },
  { path: 'my-action-items', component: MyActionItemsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WorkRequestsRoutingModule { }

export const routedComponents = [WorkRequestsComponent];