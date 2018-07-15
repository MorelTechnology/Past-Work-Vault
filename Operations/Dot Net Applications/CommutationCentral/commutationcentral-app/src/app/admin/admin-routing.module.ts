import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin.component';
import { ManageLookupsComponent } from './lookups/manage-lookups/manage-lookups.component';
import { LookupListComponent } from './lookups/lookup-list/lookup-list.component';
import { EditLookupComponent } from './lookups/edit-lookup/edit-lookup.component';
import { NewLookupComponent } from './lookups/new-lookup/new-lookup.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { NewUserComponent } from './users/new-user/new-user.component';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { EditRoleComponent } from './roles/edit-role/edit-role.component';

const routes: Routes = [
  { path: '', component: AdminComponent },
  { path: 'lookups', component: ManageLookupsComponent },
  { path: 'lookups/:type', component: LookupListComponent },
  { path: 'lookups/:type/new', component: NewLookupComponent },
  { path: 'lookups/:type/:id', component: EditLookupComponent },
  { path: 'users', component: UserListComponent },
  { path: 'users/new', component: NewUserComponent },
  { path: 'users/:id', component: EditUserComponent },
  { path: 'roles', component: RoleListComponent },
  { path: 'roles/:id', component: EditRoleComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
