import { Component, OnInit } from '@angular/core';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd, NavigationStart, NavigationEnd } from "@angular/router";
import { MenuItem } from "primeng/components/common/api";

import { BreadcrumbService } from "../shared/services";

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  public breadcrumb: MenuItem[] = [];
  public home: MenuItem = { label: 'Home', routerLink: '/', routerLinkActiveOptions: { exact: 'true' } };
  public loadingRouteConfig: boolean;

  constructor(private breadcrumbService: BreadcrumbService, private router: Router) { }

  ngOnInit() {
    // Subscribe to router events so that the loading animation can be displayed when route configs are loading, and hidden when they are done loading
    this.router.events.subscribe((event: any) => {
      if (event instanceof RouteConfigLoadStart || event instanceof NavigationStart) {
        this.loadingRouteConfig = true;
      } else if (event instanceof RouteConfigLoadEnd || event instanceof NavigationEnd) {
        this.loadingRouteConfig = false;
      }
    });
    // Subscribe to the breadcrumb service so that breadcrumb items can be pushed to the layout component by child components
    this.breadcrumbService.breadcrumbChanged.subscribe(breadcrumb => {
      this.breadcrumb.length = 0;
      breadcrumb.forEach(item => {
        this.breadcrumb.push(item);
      });
    });
  }

}
