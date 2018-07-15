import { Component, OnInit } from '@angular/core';
import { MenuItem } from "primeng/components/common/api";

import { BreadcrumbService } from "../../../shared";

@Component({
  selector: 'app-new-work-request',
  templateUrl: './new-work-request.component.html',
  styleUrls: ['./new-work-request.component.scss']
})
export class NewWorkRequestComponent implements OnInit {
  private breadcrumb: MenuItem[] = [];

  constructor(private breadcrumbService: BreadcrumbService) { }

  ngOnInit() {
    this.breadcrumb = [
      { label: "Work Requests", routerLink: "/work-requests", routerLinkActiveOptions: { exact: true } },
      { label: "New Work Request", routerLink: "/work-requests/new" }
    ];
    this.breadcrumbService.setBreadcrumb(this.breadcrumb);
  }

}
