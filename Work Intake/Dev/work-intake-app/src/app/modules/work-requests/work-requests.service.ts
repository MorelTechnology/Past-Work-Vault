import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers, ResponseOptions } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/observable/forkJoin';
import 'rxjs/add/observable/from';

import { WorkRequest, User, NotificationType, WorkRequestStatus, BusinessValueUnit, RealizationOfImpact } from "../../shared";
import { UserService } from "../../shared/services";
import { environment } from "../../../environments/environment";

@Injectable()
export class WorkRequestsService {
  private baseEndpoint: string;
  private workRequestCreatedSource = new Subject<number>();
  private workRequestUpdatedSource = new Subject<number>();

  public workRequestCreated$ = this.workRequestCreatedSource.asObservable();
  public workRequestUpdated$ = this.workRequestUpdatedSource.asObservable();

  constructor(private http: Http, private userService: UserService) {
    this.baseEndpoint = environment.apiUrl + "WorkRequest";
  }

  private workRequests(): Observable<WorkRequest[]> {

    if (!environment.production)
      if (environment.itemLimit != 0)
        return this.http.get(this.baseEndpoint + "/GetTheLast/" + environment.itemLimit, { withCredentials: true })
          .map((response: Response) => <WorkRequest[]>response.json())
          .catch(this.handleError);
      else
        return this.http.get(this.baseEndpoint + "/GetAllWorkRequests", { withCredentials: true })
          .map((response: Response) => <WorkRequest[]>response.json())
          .catch(this.handleError);
    else
      return this.http.get(this.baseEndpoint + "/GetAllWorkRequests", { withCredentials: true })
        .map((response: Response) => <WorkRequest[]>response.json())
        .catch(this.handleError);
  }

  private mySubmissions(): Observable<WorkRequest[]> {
    return this.http.get(this.baseEndpoint + "/GetMySubmissions", { withCredentials: true })
      .map((response: Response) => <WorkRequest[]>response.json())
      .catch(this.handleError);
  }

  private myAssignments(): Observable<WorkRequest[]> {
    return this.http.get(this.baseEndpoint + "/GetMyAssignments", { withCredentials: true })
      .map((response: Response) => <WorkRequest[]>response.json())
      .catch(this.handleError);
  }

  /**
   * This took forever, but it will properly bring back work requests with manager/requestor display names properly assigned
   */
  public getWorkRequests(getType: number = 0) {
    let getMethod: Observable<WorkRequest[]>;
    switch (getType) {
      case 0:
        getMethod = this.workRequests();
        break;
      case 1:
        getMethod = this.mySubmissions();
        break;
      case 2:
        getMethod = this.myAssignments();
        break;
      default:
        getMethod = this.workRequests();
    }
    // First, execute the HTTP get to retrieve requests
    return getMethod
      // MergeMap them so we have an Observable of an array of requests
      .mergeMap(requests =>
        // Separate each individual request into its own observable
        Observable.from(requests)
          .mergeMap(request => {
            request.LastModified = new Date(request.LastModified);
            request.StatusDate = new Date(request.StatusDate);
            request.RequestedCompletionDate = new Date(request.RequestedCompletionDate).toISOString() === new Date("1900-01-01T00:00:00").toISOString() ? null : new Date(request.RequestedCompletionDate);
            // Do a forkJoin to send off requests for the manager and the requestor as objects
            // I realized that if something goes wonky with getting the manager or requestor user objects, we don't want this whole observable to fail
            // Instead, we catch each error of the forkJoin array and bring in a null user. We can expand this error handling later
            return Observable.forkJoin([
              this.userService.getUser(request.Manager)
                .catch(err => {
                  return Observable.of(new User());
                }),
              this.userService.getUser(request.Requestor)
                .catch(err => {
                  return Observable.of(new User());
                })
            ])
              // When the requests in the forkJoin resolve, set the display names appropriately
              .map(users => {
                request.ManagerDisplayName = users[0].displayName;
                request.RequestorDisplayName = users[1].displayName;
                return request;
              })
          })
      );
  }


  private workRequest(requestId: number): Observable<WorkRequest> {
    return this.http.get(this.baseEndpoint + "/GetWorkRequest/" + requestId, { withCredentials: true })
      .map((response: Response) => <WorkRequest>response.json()).catch(this.handleError);
  }


  /**
   * Similar to the getWorkRequests, this function chains the AJAX calls to get the manager and the requestor, setting the display names appropriately
   * @param requestId The ID of the work request
   */
  public getWorkRequest(requestId: number): Observable<WorkRequest> {
    return this.workRequest(requestId)
      .mergeMap(workRequest => {
        workRequest.LastModified = new Date(workRequest.LastModified);
        workRequest.StatusDate = new Date(workRequest.StatusDate);
        workRequest.RequestedCompletionDate = new Date(workRequest.RequestedCompletionDate).toISOString() === new Date("1900-01-01T00:00:00").toISOString() ? null : new Date(workRequest.RequestedCompletionDate);
        return Observable.forkJoin([
          // I realized that if something goes wonky with getting the manager or requestor user objects, we don't want this whole observable to fail
          // Instead, we catch each error of the forkJoin array and bring in a null user. We can expand this error handling later
          this.userService.getUser(workRequest.Manager)
            .catch(error => {
              return Observable.of(new User());
            }),
          this.userService.getUser(workRequest.Requestor)
            .catch(error => {
              return Observable.of(new User());
            })
        ])
          .map(users => {
            workRequest.ManagerDisplayName = users[0].displayName;
            workRequest.RequestorDisplayName = users[1].displayName;
            return workRequest;
          })
      })
      .catch(this.handleError);
  }


  public createWorkRequest(workRequest: WorkRequest): Observable<number> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers, withCredentials: true });
    let body: string = JSON.stringify(workRequest);
    return this.http.post(this.baseEndpoint + "/Create", body, options)
      .map((response: Response) => {
        // I like to emit an announcement that items have been created, that way other components can subscribe to the service if necessary
        this.announceCreateWorkRequest(<number>response.json());
        return <number>response.json();
      })
      .catch(this.handleError);
  }

  private announceCreateWorkRequest(requestId: number): void {
    this.workRequestCreatedSource.next(requestId);
  }

  public updateWorkRequest(workRequest: WorkRequest): Observable<number> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers, withCredentials: true });
    let body: string = JSON.stringify(workRequest);
    return this.http.post(this.baseEndpoint + "/UpdateWorkRequestItem", body, options)
      .map((response: Response) => {
        this.announceUpdateWorkRequest(<number>response.json());
        return <number>response.json();
      })
      .catch(this.handleError);
  }

  private announceUpdateWorkRequest(requestId: number): void {
    this.workRequestUpdatedSource.next(requestId);
  }

  public rejectWorkRequest(workRequestId: number, isDenial: boolean, comments?: string): Observable<boolean> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers, withCredentials: true });
    let body: string = JSON.stringify({ WorkRequestId: workRequestId, IsDenial: isDenial, Comment: comments });
    return this.http.post(this.baseEndpoint + "/RejectWorkRequest", body, options)
      .map((response: Response) => {
        return <boolean>response.json();
      })
      .catch(this.handleError);
  }

  /**
   * Hack to translate the enum value of work request status into friendly text
   * @param requestStatus 
   */
  public requestStatusToString(requestStatus: WorkRequestStatus): string {
    switch (requestStatus) {
      case WorkRequestStatus.InProgress:
        return "In Progress";
      case WorkRequestStatus.SubmittedToProductOwner:
        return "Submitted to Product Owner";
      case WorkRequestStatus.SubmittedToPortfolioManager:
        return "Submitted to Portfolio Manager";
      case WorkRequestStatus.ReturnedToAssociate:
        return "Returned To Associate";
      case WorkRequestStatus.ReadyForPrioritization:
        return "Ready for Prioritization";
      case WorkRequestStatus.Cancelled:
        return "Cancelled";
      case WorkRequestStatus.Archived:
        return "Archived";
      case WorkRequestStatus.Denied:
        return "Denied";
      case WorkRequestStatus.SubmittedToDigitalStrategy:
        return "Submitted to Digital Strategy";
      case WorkRequestStatus.ReadyForScheduling:
        return "Ready For Scheduling";
      default:
        throw "Work request type was not recognized";
    }
  }

  /**
   * Hack to translate the string value of a work request status to the enum value
   * @param requestStatusString 
   */
  public requestStatusStringToEnum(requestStatusString: string): WorkRequestStatus {
    switch (requestStatusString.toLowerCase()) {
      case "in progress":
        return WorkRequestStatus.InProgress;
      case "submitted to product owner":
        return WorkRequestStatus.SubmittedToProductOwner;
      case "submitted to portfolio manager":
        return WorkRequestStatus.SubmittedToPortfolioManager;
      case "returned to associate":
        return WorkRequestStatus.ReturnedToAssociate;
      case "approved": case "ready for prioritization":
        return WorkRequestStatus.ReadyForPrioritization;
      case "cancelled":
        return WorkRequestStatus.Cancelled;
      case "archived":
        return WorkRequestStatus.Archived;
      case "denied":
        return WorkRequestStatus.Denied;
      case "submitted to digital strategy":
        return WorkRequestStatus.SubmittedToDigitalStrategy;
      case "ready for scheduling":
        return WorkRequestStatus.ReadyForScheduling;
      default:
        throw "Work request status was not recognized";
    }
  }

  /**
   * Hack to translate enum value of business value unit into friendly text
   * @param businessValueUnit 
   */
  public businessValueUnitToString(businessValueUnit: BusinessValueUnit): string {
    switch (businessValueUnit) {
      case BusinessValueUnit.Unknown:
        return "Update required to convert legacy Quantification statement.";
      case BusinessValueUnit.Dollars:
        return "Dollars per year";
      case BusinessValueUnit.Hours:
        return "Hours per year";
      default:
        return "Business Value Unit was not recognized";
    }
  }

  /**
   * Hack to translate the string value of a business value unit to the enum value
   * @param businessValueUnitString 
   */
  public businessValueUnitStringToEnum(businessValueUnitString: string): BusinessValueUnit {
    switch (businessValueUnitString.toLowerCase()) {
      case "update required to convert legacy quantification statement.":
        return BusinessValueUnit.Unknown;
      case "dollars per year":
        return BusinessValueUnit.Dollars;
      case "hours per year":
        return BusinessValueUnit.Hours;
      default:
        return BusinessValueUnit.Unknown;
    }
  }


  /**
   * Hack to translate enum value of business value unit into friendly text
   * @param businessValueUnit 
   */
  public impactValueToString(impactValue: RealizationOfImpact): string {
    switch (impactValue) {
      case RealizationOfImpact.Unspecified:
        return "Unspecified";
      case RealizationOfImpact.VeryHigh:
        return "Very High";
      case RealizationOfImpact.High:
        return "High";
      case RealizationOfImpact.Low:
        return "Low";
      case RealizationOfImpact.VeryLow:
        return "Very Low";
      default:
        return "Unspecified";
    }
  }


  /**
   * Hack to translate the string value of an impact Value to the enum value
   * @param impactValueString 
   */
  public impactValueStringToEnum(impactValueString: string): RealizationOfImpact {
    switch (impactValueString.toLowerCase()) {
      case "unspecified":
        return RealizationOfImpact.Unspecified;
      case "very high":
        return RealizationOfImpact.VeryHigh;
      case "high":
        return RealizationOfImpact.High;
      case "low":
        return RealizationOfImpact.Low;
      case "very low":
        return RealizationOfImpact.VeryLow;
      default:
        return RealizationOfImpact.Unspecified;
    }
  }


  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.text() || JSON.parse(JSON.stringify(error)).statusText || 'Server error');
  }

}
