import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BreadcrumbModule, GrowlModule, DialogModule } from "primeng/primeng";

import { LayoutRoutingModule } from "./layout-routing.module";
import { LayoutComponent } from './layout.component';
// Even though these components are defined in the shared folder, the LayoutModule is where the app will declare and render them
import { HeaderComponent, SidebarComponent, BreadcrumbService } from "../shared";

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    BreadcrumbModule,
    GrowlModule,
    DialogModule
  ],
  declarations: [
    LayoutComponent,
    HeaderComponent,
    SidebarComponent
  ],
  providers: [BreadcrumbService]
})
export class LayoutModule { }
