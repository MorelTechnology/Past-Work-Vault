import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AuthenticationService } from "../services/authentication.service";

@Injectable()
export class AppUserGuard implements CanActivate, CanActivateChild {
  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.canActivateChild(next, state);
  }

  /**
   * Check if the current user is authorized. If not, redirect to the accessdenied route
   * @param next 
   * @param state 
   */
  canActivateChild(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.authenticationService.isAuthorized().map(authorized => {
      if (!authorized) {
        this.router.navigate(['/accessdenied']);
      }
      return authorized;
    });
  }
}
