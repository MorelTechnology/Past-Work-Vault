import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from './app.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap'
import { MessageService } from "primeng/components/common/messageservice";
import { SharedModule } from './shared/shared.module';
import { ApiModule } from './api/api.module';
import { AppHttpInterceptor } from './app-http-interceptor';
import { ConfirmationService } from 'primeng/components/common/api';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NgbModule.forRoot(),
    SharedModule.forRoot(),
    ApiModule.forRoot(),
    HttpClientModule
  ],
  providers: [
    MessageService,
    ConfirmationService,
    { provide: HTTP_INTERCEPTORS, useClass: AppHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
