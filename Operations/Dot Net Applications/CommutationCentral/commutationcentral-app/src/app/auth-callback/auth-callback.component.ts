import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../shared/services/breadcrumb.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.scss']
})
export class AuthCallbackComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private breadcrumbService: BreadcrumbService) {
  }

  ngOnInit() {
    this.breadcrumbService.setBreadcrumbs([]);
    this.authService.completeAuthentication().then(value => {
      this.router.navigate([this.authService.getPreAuthUrl()]);
    });
  }

}
