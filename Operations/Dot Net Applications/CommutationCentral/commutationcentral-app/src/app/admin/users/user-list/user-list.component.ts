import { Component, OnInit, ViewChild } from '@angular/core';
import { MenuItem, LazyLoadEvent, ConfirmationService } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { UsersService } from '../../../api/services/users.service';
import { User } from '../../../api/models/user';
import { MessageService } from 'primeng/components/common/messageservice';
import { Table } from 'primeng/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  public tableColumns: any[];
  public loadingUsers: boolean = true;
  public users: User[];
  public pageSize: number;
  public totalCount: number;
  @ViewChild(Table) table: Table;

  constructor(private breadcrumbService: BreadcrumbService, private usersService: UsersService,
    private confirmationService: ConfirmationService, private messageService: MessageService,
    private router: Router) { }

  ngOnInit() {
    let breadcrumb: MenuItem[] = [
      { label: "Administration", routerLink: "/admin", routerLinkActiveOptions: { exact: true } },
      { label: "Users", routerLink: "/admin/users" }
    ];
    this.breadcrumbService.setBreadcrumbs(breadcrumb);
    this.setTableColumns();
  }

  private setTableColumns(): void {
    this.tableColumns = [
      { field: "userName", header: "User Name" },
      { field: "email", header: "Email" }
    ];
  }

  private getAllUsers(queryString?: string): void {
    this.loadingUsers = true;
    this.usersService.getAll(queryString).finally(() => {
      this.loadingUsers = false;
    }).subscribe(users => {
      this.users = <User[]>users.body;
      let paginationData = JSON.parse(users.headers.get("X-Pagination"));
      this.pageSize = paginationData.pageSize;
      this.totalCount = paginationData.totalCount;
    });
  }

  public loadUsersLazy(event: LazyLoadEvent) {
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
    this.getAllUsers(queryString === '' ? null : queryString);
  }

  public confirmDeactivateUser(user: User) {
    this.confirmationService.confirm({
      message: `Are you sure you want to deactivate the user '${user.userName}'?`,
      accept: () => {
        this.usersService.deactivateUser(user.id).subscribe(() => {
          this.messageService.add({
            severity: 'success',
            summary: `Successfully Deactivated User`,
            detail: `Deactivated user ${user.userName}`
          });
          let loadEvent: LazyLoadEvent = {
            rows: this.table.rows,
            first: this.table.first,
            sortField: this.table.sortField,
            sortOrder: this.table.sortOrder
          };
          this.loadUsersLazy(loadEvent);
        });
      }
    });
  }

  public editUser(user: User) {
    this.router.navigate(['/admin/users', user.id]);
  }

  public addUser() {
    this.router.navigate(['/admin/users/new']);
  }

}
