import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BaseApiService } from './base-api.service';
import { UserRole } from '../models/user-role';

@Injectable()
export class RolesService {
  private baseEndpoint: string = environment.api_url + "roles";

  constructor(private baseApiService: BaseApiService) { }

  public getActive(paged: boolean, queryString?: string) {
    let url = paged ? `${this.baseEndpoint}/GetActiveRolesPaged` : `${this.baseEndpoint}/GetActiveRolesUnpaged`;
    if (queryString) url += `?${queryString}`;
    return this.baseApiService.get(url);
  }

  public get(roleId: string) {
    let url = `${this.baseEndpoint}/${roleId}`;
    return this.baseApiService.get(url);
  }

  public disableRole(role: UserRole) {
    let url = `${this.baseEndpoint}/DisableRole(${role.id})`;
    return this.baseApiService.post(url);
  }

  public updateRole(role: UserRole) {
    let url = `${this.baseEndpoint}/${role.id}`;
    return this.baseApiService.put(url, role);
  }

}
