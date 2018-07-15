import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthenticationService } from '../services';

@Injectable()
export class AppAdminGuard implements CanActivateChild {
constructor( 
  private authenticationService: AuthenticationService,
  private router: Router,
) {}
  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
  return this.authenticationService.isAdmin().map(authorized => {
    if (!authorized) {
      console.log( 'You are not permitted access to this module.');
      this.router.navigate(['/']);
    }
    return authorized;
  });
  
    }}