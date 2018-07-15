import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/mergeMap';

import { User } from "../models";
import { environment } from "../../../environments/environment";


@Injectable()
export class UserService {

  private baseEndpoint: string;
  private currentUser: User;
  private user: User;

  constructor(private http: Http) {
    this.baseEndpoint = environment.apiUrl + "User";
  }
  public getCurrentUser(): Observable<User> {
    
    // If the current user has already been determined, return an observable of that, otherwise get the current user from the API and store it for later use
    return this.currentUser ? Observable.of(this.currentUser) :
      this.http.get(this.baseEndpoint + "/GetCurrentUser", { withCredentials: true }).map((response: Response) => {
        this.currentUser = <User>response.json();
        return this.currentUser;
      })
        .catch(this.handleError);
  }

  public getUser(identifier: string): Observable<User> {
    return this.http.get(this.baseEndpoint + "/GetUser/" + identifier, { withCredentials: true })
      .map((response: Response) => <User>response.json())
      .catch(this.handleError);
  }

  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.text() || JSON.parse(JSON.stringify(error)).statusText || 'Server error');
  }
 
}
