import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UnauthenticatedRoutingModule } from './unauthenticated-routing.module';
import { UnauthenticatedComponent } from './unauthenticated.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    UnauthenticatedRoutingModule,
    SharedModule
  ],
  declarations: [UnauthenticatedComponent]
})
export class UnauthenticatedModule { }
