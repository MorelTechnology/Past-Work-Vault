import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../services/auth.service';
import { MessageService } from 'primeng/components/common/messageservice';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private authService: AuthService, private messageService: MessageService, private router: Router) {

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.canActivateChild(next, state);
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.authService.setPreAuthUrl(state.url);
    this.authService.startAuthentication().catch(exception => {
      this.messageService.add({
        severity: 'error', summary: 'Error Authenticating', detail: `There was an error 
        authenticating. The authentication server may not be responding. If the issue persists, please contact your 
        administrator.` });
      this.router.navigate(['/unauthenticated']);
    });
    return false;
  }
}
