import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LookupsService } from "./services/lookups.service";
import { BaseApiService } from './services/base-api.service';
import { UsersService } from './services/users.service';
import { RolesService } from './services/roles.service';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: []
})
export class ApiModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ApiModule,
      providers: [
        BaseApiService,
        LookupsService,
        UsersService,
        RolesService
      ]
    }
  }
}
