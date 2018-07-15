import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/mergeMap';

import { User } from "../models";
import { environment } from "../../../environments/environment";
import { UserService } from "./user.service";

@Injectable()
export class AuthenticationService {
  private baseEndpoint: string;
  private currentUser: User;

  constructor(private http: Http, private userService: UserService) {
    this.baseEndpoint = environment.apiUrl + "Auth";
  }

  /**
   * Get the current user, then determine if the user is authorized to access the application
   */
  public isAuthorized(): Observable<boolean> {
    return this.userService.getCurrentUser()
      .mergeMap(user =>
        this.isUserAuthorized(user.sAMAccountName)
      )
      .catch(this.returnUnauthorized);
  }

  /**
   * Checks if a given user is authorized to access the application
   * @param samAccountName Account of the user to check
   */
  public isUserAuthorized(samAccountName: string): Observable<boolean> {
    return this.http.get(this.baseEndpoint + "/isAuthorized/" + samAccountName, { withCredentials: true })
      .map((response: Response) => <boolean>response.json())
      // Because the API returns a 500 error if the user isn't authorized, we use a specific catch callback to return an observable of false
      // This works nicely, because if the API is unavailable, the user gets routed away from the main application
      .catch(this.returnUnauthorized);
  }

  private returnUnauthorized(error: Response) {
    return Observable.of(false);
  }


  public isAdmin(): Observable<boolean>{
    return this.userService.getCurrentUser()
      .mergeMap(user => this.isUserAdmin(user.sAMAccountName));
}

/**
 * Checks if a given user is an Application Administrator
 * @param samAccountName Account of the user to check
 */
  public isUserAdmin(sAMAccountName: string): Observable<boolean>{
  return this.http.get(this.baseEndpoint + "/isAdmin/" + sAMAccountName, { withCredentials: true })
    .map((response: Response) => <boolean>response.json())
    .catch(this.returnUnauthorized);

}

  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.text() || JSON.parse(JSON.stringify(error)).statusText || 'Server error');
  }

}
