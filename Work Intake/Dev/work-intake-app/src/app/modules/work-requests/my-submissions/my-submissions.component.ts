import { Component, OnInit } from '@angular/core';
import { BreadcrumbService, WorkRequest } from '../../../shared';
import { MenuItem } from 'primeng/primeng';
import { WorkRequestsService } from '../work-requests.service';
import { MessageService } from 'primeng/components/common/messageservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-submissions',
  templateUrl: './my-submissions.component.html',
  styleUrls: ['./my-submissions.component.css']
})
export class MySubmissionsComponent implements OnInit {
  public loadingWorkRequests: boolean = false;
  public workRequests: WorkRequest[] = [];

  private breadcrumb: MenuItem[];

  constructor(private breadcrumbService: BreadcrumbService, private workRequestsService: WorkRequestsService, private messageService: MessageService,
    private router: Router) { }

  ngOnInit() {
    this.breadcrumb = [
      { label: 'Work Requests', routerLink: "/work-requests", routerLinkActiveOptions: { exact: true } },
      { label: 'My Submissions', routerLink: "/work-requests/my-submissions" }
    ];
    this.breadcrumbService.setBreadcrumb(this.breadcrumb);
    this.getWorkRequests();
  }

  private getWorkRequests(): void {
    this.loadingWorkRequests = true;
    this.workRequestsService.getWorkRequests(1).finally(() => {
      this.workRequests.sort((a, b) => {
        return a.RequestID - b.RequestID;
      });
      this.loadingWorkRequests = false;
    }).subscribe(request => {
      this.workRequests = [...this.workRequests, request];
    }, error => {
      this.messageService.add({ severity: "error", summary: "Failed to Retrieve Work Requests", detail: error });
    });
  }

  public goToRequest(requestId: number): void {
    this.router.navigate(['/work-requests/request', requestId]);
  }

}
