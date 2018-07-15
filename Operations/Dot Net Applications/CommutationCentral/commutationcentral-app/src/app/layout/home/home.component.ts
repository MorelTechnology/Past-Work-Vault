import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/services/auth.service';
import { BreadcrumbService } from '../../shared/services/breadcrumb.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public claims: any;

  constructor(private authService: AuthService, private breadcrumbService: BreadcrumbService) { }

  ngOnInit() {
    this.breadcrumbService.setBreadcrumbs([]);
  }

  public getClaims() {
    this.claims = this.authService.getClaims();
  }
}
