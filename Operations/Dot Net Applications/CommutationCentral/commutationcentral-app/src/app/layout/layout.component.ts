import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../shared/services/breadcrumb.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  public breadcrumb: MenuItem[] = [];
  public home: MenuItem = { label: 'Home', routerLink: '/', routerLinkActiveOptions: { exact: 'true' } };

  constructor(private breadcrumbService: BreadcrumbService) { }

  ngOnInit() {
    // Subscribe to the breadcrumb service so that breadcrumb items can be pushed to the layout component
    // by child components
    this.breadcrumbService.breadcrumbsChanged$.subscribe(breadcrumb => {
      this.breadcrumb.length = 0;
      breadcrumb.forEach(item => {
        this.breadcrumb.push(item);
      });
    });
  }

}
