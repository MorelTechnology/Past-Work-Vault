import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignoutCallbackComponent } from './signout-callback.component';

const routes: Routes = [
  { path: '', component: SignoutCallbackComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SignoutCallbackRoutingModule { }
