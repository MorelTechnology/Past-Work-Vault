import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthGuard } from './guards/auth.guard';
import { AuthService } from './services/auth.service';
import { LoginButtonComponent } from './components/login-button/login-button.component';
import { LogoutButtonComponent } from './components/logout-button/logout-button.component';
import { BreadcrumbService } from './services/breadcrumb.service';
import { TransitionDialogComponent } from './components/transition-dialog/transition-dialog.component';
import { DialogModule } from "primeng/dialog";
import { NavButtonComponent } from './components/nav-button/nav-button.component';

@NgModule({
  imports: [
    CommonModule,
    DialogModule
  ],
  declarations: [LoginButtonComponent, LogoutButtonComponent, TransitionDialogComponent, NavButtonComponent],
  providers: [
  ],
  exports: [
    LoginButtonComponent,
    LogoutButtonComponent,
    TransitionDialogComponent,
    NavButtonComponent
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        AuthGuard,
        AuthService,
        BreadcrumbService
      ]
    }
  }
}
