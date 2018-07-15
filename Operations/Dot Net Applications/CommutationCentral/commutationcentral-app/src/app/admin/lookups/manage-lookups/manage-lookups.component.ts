import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';

@Component({
  selector: 'app-manage-lookups',
  templateUrl: './manage-lookups.component.html',
  styleUrls: ['./manage-lookups.component.scss']
})
export class ManageLookupsComponent implements OnInit {
  private pageTitle: string = "Manage Lookups";
  private breadcrumb: MenuItem[] = [
    { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
    { label: this.pageTitle, routerLink: '/admin/lookups' }
  ];

  constructor(private breadcrumbService: BreadcrumbService) { }

  ngOnInit() {
    this.breadcrumbService.setBreadcrumbs(this.breadcrumb);
  }

}
