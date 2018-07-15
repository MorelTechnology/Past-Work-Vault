import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UnauthenticatedComponent } from './unauthenticated.component';

const routes: Routes = [
  { path: '', component: UnauthenticatedComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UnauthenticatedRoutingModule { }
