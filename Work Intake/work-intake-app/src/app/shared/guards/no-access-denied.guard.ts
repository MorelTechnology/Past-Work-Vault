import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

import { AuthenticationService } from "../services/authentication.service";

@Injectable()
export class NoAccessDeniedGuard implements CanActivate {
  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  /**
   * Check if the current user is authorized. If so, redirect away from the accessdenied route
   * @param next 
   * @param state 
   */
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.authenticationService.isAuthorized().map(authorized => {
      if (authorized) {
        this.router.navigate(['/']);
      }
      return !authorized;
    });
  }
}
