import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { RestoreDeniedRequestsComponent } from './restore-denied-requests/restore-denied-requests.component';
import { AdminComponent } from './admin.component';
@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule
  ],
  declarations: [RestoreDeniedRequestsComponent, AdminComponent]
})
export class AdminModule { }
