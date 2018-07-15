import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from '../shared/services/breadcrumb.service';
import { MenuItem } from 'primeng/components/common/api';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  private pageTitle: string = "Administration";
  private breadcrumb: MenuItem[] = [{ label: this.pageTitle, routerLink: "/admin" }];

  constructor(private breadcrumbService: BreadcrumbService) { }

  ngOnInit() {
    this.breadcrumbService.setBreadcrumbs(this.breadcrumb);
  }

}
