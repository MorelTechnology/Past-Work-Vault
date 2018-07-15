import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SignoutCallbackRoutingModule } from './signout-callback-routing.module';
import { SignoutCallbackComponent } from './signout-callback.component';
import { SharedModule } from "../shared/shared.module";

@NgModule({
  imports: [
    CommonModule,
    SignoutCallbackRoutingModule,
    SharedModule
  ],
  declarations: [SignoutCallbackComponent]
})
export class SignoutCallbackModule { }
