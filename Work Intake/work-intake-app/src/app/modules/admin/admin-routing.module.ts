import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin.component'
import { RestoreDeniedRequestsComponent } from './restore-denied-requests/restore-denied-requests.component';
import { AppAdminGuard } from '../../shared/guards/app-admin.guard';


const routes: Routes = [
  {
    // For all child routes, use the AppUserGuard to prevent unauthorized users from activating the routes
    path: '', 
    component: AdminComponent, 
    canActivateChild: [AppAdminGuard],
    children: [
      { path: '', component: AdminComponent},
      { path: 'restore-denied-requests', component: RestoreDeniedRequestsComponent }
    ]
  }
];



// const routes: Routes = [
//   { 
//     path: '',
//     canActivate: [AppAdminGuard],
//     component: AdminComponent
//   },
//   {
//     path: 'restore-denied-requests', component: RestoreDeniedRequestsComponent,
//     // children: [
//     //   { path: '', component: NewWorkRequestContainerComponent },
//     //   { path: 'success/:id', component: NewWorkRequestSuccessComponent }
//     // ]
//   },
// //   { path: 'request/:id', component: EditWorkRequestComponent },
// //   { path: 'request/success/:id', component: EditWorkRequestSuccessComponent }
// ];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
