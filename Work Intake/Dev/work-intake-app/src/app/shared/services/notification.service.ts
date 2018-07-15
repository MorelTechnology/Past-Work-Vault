import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/observable/forkJoin';

import { NotificationType } from "../enums";
import { environment } from "../../../environments/environment";

@Injectable()
export class NotificationService {
  private baseEndpoint: string;

  constructor(private http: Http) {
    this.baseEndpoint = environment.apiUrl + "Notification";
  }

  /**
   * Sends a single notification to a user via email
   * @param workRequestId ID of the request
   * @param notifyUser The sAMAccountName or objectSid of the user to notify
   * @param template The NotificationType enum to use
   * @param comments Optional comments to include with the notification
   */
  public sendNotification(workRequestId: number, notifyUser: string, template: NotificationType, comments?: string): Observable<boolean> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers, withCredentials: true });
    let body = JSON.stringify({ WorkRequestId: workRequestId, NotifyUser: notifyUser, Template: template, Comments: comments });
    return this.http.post(this.baseEndpoint, body, options)
      .map((response: Response) => <boolean>response.json())
      .catch(this.handleError);
  }

  /**
   * Sends a batch of notifications to an array of recipients via email
   * @param workRequestId ID of the request
   * @param recipients An array of the sAMAccountNames or objectSids of the users to notify
   * @param template The NotificationType enum to use
   * @param comments Optional comments to include with the notification
   */
  public sendNotificationBatch(workRequestId: number, recipients: string[], template: NotificationType, comments?: string): Observable<boolean[]> {
    let observableBatch: Observable<boolean>[] = [];
    // Figure out how many notifications are going to be sent, and construct an array of observables
    // Then, those observables can be called asynchronously in a forkJoin
    recipients.forEach(recipient => observableBatch.push(this.sendNotification(workRequestId, recipient, template, comments)));
    return Observable.forkJoin(observableBatch)
      .map(results => results);
  }

  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.text() || JSON.parse(JSON.stringify(error)).statusText || 'Server error');
  }

}
