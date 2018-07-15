import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { AppUserGuard } from "../shared/guards";

const routes: Routes = [
  {
    // For all child routes, use the AppUserGuard to prevent unauthorized users from activating the routes
    path: '', component: LayoutComponent, canActivateChild: [AppUserGuard],
    children: [
      { path: '', loadChildren: './home/home.module#HomeModule' },
      { path: 'blank-page', loadChildren: './blank-page/blank-page.module#BlankPageModule' },
      { path: 'work-requests', loadChildren: '../modules/work-requests/work-requests.module#WorkRequestsModule' },
      { path: 'admin', loadChildren: '../modules/admin/admin.module#AdminModule'}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule { }

export const routedComponents = [LayoutComponent];