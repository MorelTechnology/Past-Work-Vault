import { Component, OnInit } from '@angular/core';
import { Router, RouteConfigLoadStart, NavigationStart, RouteConfigLoadEnd, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-transition-dialog',
  templateUrl: './transition-dialog.component.html',
  styleUrls: ['./transition-dialog.component.scss']
})
export class TransitionDialogComponent implements OnInit {
  public loadingRouteConfig: boolean;

  constructor(private router: Router) { }

  ngOnInit() {
    // Subscribe to router events so that the loading animation can be displayed when route configs are loading, and hidden when they are done loading
    this.router.events.subscribe((event: any) => {
      if (event instanceof RouteConfigLoadStart || event instanceof NavigationStart) {
        this.loadingRouteConfig = true;
      } else if (event instanceof RouteConfigLoadEnd || event instanceof NavigationEnd) {
        this.loadingRouteConfig = false;
      }
    });
  }

}
