import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/mergeMap';
import { User } from "../models";
import { environment } from "../../../environments/environment";

@Injectable()
export class ConfigurationService {
  private baseEndpoint: string;
  private productManagers: User[];

  constructor(private http: Http) {
    this.baseEndpoint = environment.apiUrl + "Config";
  }

  public getCorporateGoals(): Observable<string[]> {

    return this.http.get(this.baseEndpoint + "/GetCorporateGoals", { withCredentials: true })
      .map((response: Response) => <string[]>response.json())
      .catch(this.handleError);  
  }


  public getProductManagers(): Observable<User[]> {
    
    // If the product managers have already been queried, return an observable of that, otherwise get the list from the API and store it for later use
    return this.productManagers ? Observable.of(this.productManagers) :
      this.http.get(this.baseEndpoint + "/getProductManagers", { withCredentials: true }).map((response: Response) => {
        this.productManagers = <User[]>response.json();
        return this.productManagers;
      })
        .catch(this.handleError);
  }








  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.text() || JSON.parse(JSON.stringify(error)).statusText || 'Server error');
  }
}