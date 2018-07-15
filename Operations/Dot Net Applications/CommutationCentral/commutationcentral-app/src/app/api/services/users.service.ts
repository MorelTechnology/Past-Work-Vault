import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BaseApiService } from './base-api.service';
import { User } from '../models/user';

@Injectable()
export class UsersService {
  private baseEndpoint: string = environment.api_url + "users";

  constructor(private baseApiService: BaseApiService) { }

  public getAll(queryString?: string) {
    let url = this.baseEndpoint;
    if (queryString) url += `?${queryString}`;
    return this.baseApiService.get(url);
  }

  public getActiveUsersUnpaged(queryString?: string) {
    let url = `${this.baseEndpoint}/GetActiveUsersUnpaged`;
    if (queryString) url += `?${queryString}`;
    return this.baseApiService.get(url);
  }

  public getById(userId: string) {
    let url = `${this.baseEndpoint}/${userId}`;
    return this.baseApiService.get(url);
  }

  public deactivateUser(userId: string) {
    let url = `${this.baseEndpoint}/${userId}`;
    return this.baseApiService.delete(url);
  }

  public updateUser(user: User) {
    let url = `${this.baseEndpoint}/${user.id}`;
    return this.baseApiService.put(url, user);
  }

  public searchUsers(query: string) {
    let url = `${this.baseEndpoint}/SearchUsers('${encodeURI(query)}')`;
    return this.baseApiService.get(url);
  }

  public createUser(user: User) {
    let url = `${this.baseEndpoint}`;
    return this.baseApiService.post(url, user);
  }

  public getUsersInRole(roleName: string) {
    let url = `${this.baseEndpoint}/GetUsersInRole(${roleName})`;
    return this.baseApiService.get(url);
  }

  public getCurrentUserRoles() {
    let url = `${this.baseEndpoint}/GetCurrentUserRoles`;
    return this.baseApiService.get(url);
  }

}
