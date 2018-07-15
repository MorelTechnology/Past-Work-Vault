import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { MessageService } from "primeng/components/common/messageservice";
import { AuthService } from "./shared/services/auth.service";

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private messageService: MessageService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let authenticatedRequest = request.clone({
            setHeaders: { Authorization: this.authService.getAuthorizationHeaderValue() }
        });

        return next.handle(authenticatedRequest)
            .catch((error: HttpErrorResponse) => {
                if (error.error instanceof Error) {
                    // A client-side or network error occurred.
                    this.messageService.add({ severity: 'warning', summary: 'Error Processing Your Request', detail: error.error.message });
                } else {
                    this.messageService.add({ severity: 'error', summary: 'Server Error', detail: "A server error occurred. Please retry your request" });
                }
                return Observable.throw(error);
            });
    }
}
