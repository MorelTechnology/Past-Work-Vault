import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem, LazyLoadEvent, ConfirmationService } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { RolesService } from '../../../api/services/roles.service';
import { UserRole } from '../../../api/models/user-role';
import { MessageService } from 'primeng/components/common/messageservice';
import { Table } from 'primeng/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.scss']
})
export class RoleListComponent implements OnInit {
  public tableColumns: any[];
  public loadingRoles: boolean = true;
  public roles: UserRole[];
  public pageSize: number;
  public totalCount: number;
  @ViewChild(Table) table: Table;

  constructor(private breadcrumbService: BreadcrumbService, private rolesService: RolesService,
    private confirmationService: ConfirmationService, private messageService: MessageService,
    private router: Router) { }

  ngOnInit() {
    let breadcrumb: MenuItem[] = [
      { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
      { label: 'Roles', routerLink: '/admin/roles' }
    ];
    this.breadcrumbService.setBreadcrumbs(breadcrumb);
    this.setTableColumns();
  }

  private setTableColumns(): void {
    this.tableColumns = [
      { field: "name", header: "Role Name" }
    ];
  }

  private getAllRoles(queryString?: string): void {
    this.loadingRoles = true;
    this.rolesService.getActive(true, queryString).finally(() => {
      this.loadingRoles = false;
    }).subscribe(response => {
      this.roles = <UserRole[]>response.body;
      let paginationData = JSON.parse(response.headers.get("X-Pagination"));
      this.pageSize = paginationData.pageSize;
      this.totalCount = paginationData.totalCount;
    });
  }

  public loadRolesLazy(event: LazyLoadEvent) {
    let queryString: string = '';
    //TODO Filters
    if (event.rows) {
      if (queryString !== '') queryString += '&';
      queryString += `PageSize=${event.rows}`;
    }
    if (event.first && event.rows) {
      if (queryString !== '') queryString += '&';
      let page = Math.ceil((event.first + 1) / event.rows);
      queryString += `PageNumber=${page}`;
    }
    if (event.sortField) {
      if (queryString !== '') queryString += '&';
      queryString += `OrderBy=${event.sortField}`;
      if (event.sortOrder) {
        queryString += ` ${event.sortOrder === 1 ? 'asc' : 'desc'}`;
      }
    }
    this.getAllRoles(queryString === '' ? null : queryString);
  }

  public confirmDeactivateRole(role: UserRole) {
    this.confirmationService.confirm({
      message: `Are you sure you want to disable the role '${role.name}'?`,
      accept: () => {
        this.rolesService.disableRole(role).subscribe(() => {
          this.messageService.add({
            severity: 'success',
            summary: `Successfully Disabled Role`,
            detail: `Disabled role ${role.name}`
          });
          let loadEvent: LazyLoadEvent = {
            rows: this.table.rows,
            first: this.table.first,
            sortField: this.table.sortField,
            sortOrder: this.table.sortOrder
          };
          this.loadRolesLazy(loadEvent);
        });
      }
    });
  }

  public editRole(role: UserRole) {
    this.router.navigate(['/admin/roles', role.id]);
  }

  public addRole() {
    this.router.navigate(['/admin/roles/new']);
  }

}
