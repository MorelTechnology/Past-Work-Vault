import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { HttpModule } from "@angular/http";
import { MessageService } from "primeng/components/common/messageservice";
import { ConfirmationService } from "primeng/components/common/confirmationservice";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { AuthenticationService, NotificationService, UserService, ConfigurationService } from "./shared/services";
import { AppUserGuard, NoAccessDeniedGuard } from "./shared/guards";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from './app.component';
import { AppAdminGuard } from './shared/guards/app-admin.guard';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpModule,
    AppRoutingModule,
    // Ng-bootstrap requires the forRoot() import for globalization
    NgbModule.forRoot()
  ],
  providers: [MessageService, ConfirmationService, AuthenticationService, AppUserGuard, NoAccessDeniedGuard, AppAdminGuard, NotificationService, UserService, ConfigurationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
