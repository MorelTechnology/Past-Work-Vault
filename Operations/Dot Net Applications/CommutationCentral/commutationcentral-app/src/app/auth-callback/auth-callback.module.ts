import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthCallbackRoutingModule } from './auth-callback-routing.module';
import { AuthCallbackComponent } from './auth-callback.component';

@NgModule({
  imports: [
    CommonModule,
    AuthCallbackRoutingModule
  ],
  declarations: [AuthCallbackComponent]
})
export class AuthCallbackModule { }
