import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { SharedModule } from "../shared/shared.module";
import { ManageLookupsComponent } from './lookups/manage-lookups/manage-lookups.component';
import { LookupListComponent } from './lookups/lookup-list/lookup-list.component';
import { TableModule } from "primeng/table";
import { PanelModule } from "primeng/panel";
import { TooltipModule } from "primeng/tooltip";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { PickListModule } from "primeng/picklist";
import { AutoCompleteModule } from "primeng/autocomplete";
import { EditLookupComponent } from './lookups/edit-lookup/edit-lookup.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NewLookupComponent } from './lookups/new-lookup/new-lookup.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { NewUserComponent } from './users/new-user/new-user.component';
import { RoleListComponent } from './roles/role-list/role-list.component';
import { EditRoleComponent } from './roles/edit-role/edit-role.component';

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    TableModule,
    ConfirmDialogModule,
    ReactiveFormsModule,
    PanelModule,
    TooltipModule,
    PickListModule,
    AutoCompleteModule,
    FormsModule
  ],
  declarations: [AdminComponent, ManageLookupsComponent, LookupListComponent, EditLookupComponent, NewLookupComponent, UserListComponent, EditUserComponent, NewUserComponent, RoleListComponent, EditRoleComponent]
})
export class AdminModule { }
