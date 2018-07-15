import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AccessDeniedComponent } from './access-denied.component';
import { NoAccessDeniedGuard } from "../shared/guards";

const routes: Routes = [
  // Use the NoAccessDeniedGuard to prevent authenticated users from seeing the access denied screen
  { path: '', component: AccessDeniedComponent, canActivate: [NoAccessDeniedGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccessDeniedRoutingModule { }

export const routedComponents = [AccessDeniedComponent];