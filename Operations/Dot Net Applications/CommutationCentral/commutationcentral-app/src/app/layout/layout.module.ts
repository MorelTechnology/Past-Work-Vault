import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';

import { GrowlModule } from "primeng/growl";
import { BreadcrumbModule } from "primeng/breadcrumb";
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    NgbModule,
    GrowlModule,
    SharedModule,
    BreadcrumbModule
  ],
  declarations: [
    LayoutComponent,
    HeaderComponent,
    SidebarComponent
  ],
  providers: [
  ]
})
export class LayoutModule { }
