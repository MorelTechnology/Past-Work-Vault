import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LayoutComponent } from "./layout.component";
import { AuthGuard } from '../shared/guards/auth.guard';

const routes: Routes = [
  {
    path: '', component: LayoutComponent, canActivateChild: [AuthGuard],
    children: [
      { path: '', loadChildren: './home/home.module#HomeModule' },
      { path: 'admin', loadChildren: '../admin/admin.module#AdminModule' }
    ]
  },
  { path: 'unauthenticated', component: LayoutComponent, loadChildren: '../unauthenticated/unauthenticated.module#UnauthenticatedModule' },
  { path: 'auth-callback', component: LayoutComponent, loadChildren: '../auth-callback/auth-callback.module#AuthCallbackModule' },
  { path: 'signout-callback', component: LayoutComponent, loadChildren: '../signout-callback/signout-callback.module#SignoutCallbackModule' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }

export const routedComponents = [LayoutComponent];