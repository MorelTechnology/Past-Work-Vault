import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../../../api/services/users.service';
import { User } from '../../../api/models/user';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { UserRole } from '../../../api/models/user-role';
import { RolesService } from '../../../api/services/roles.service';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {
  public loadingUser: boolean = false;
  public user: User;
  public pageTitle: string;
  public loadingActiveUserRoles: boolean = false;
  public activeUserRoles: UserRole[];
  public savingUser: boolean = false;

  constructor(private route: ActivatedRoute, private usersService: UsersService,
    private breadcrumbService: BreadcrumbService, private rolesService: RolesService, private router: Router,
    private messageService: MessageService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      // TODO handle param doesn't have id
      if (params.has("id")) {
        let userId = params.get("id");
        this.loadingUser = true;
        this.usersService.getById(userId).finally(() => {
          this.loadingUser = false;
        }).subscribe(response => {
          this.user = <User>response.body;
          this.pageTitle = `Editing User ${this.user.userName}`;
          let breadcrumb: MenuItem[] = [
            { label: "Administration", routerLink: "/admin", routerLinkActiveOptions: { exact: true } },
            { label: "Users", routerLink: "/admin/users", routerLinkActiveOptions: { exact: true } },
            { label: this.user.userName, routerLink: ['/admin/users', this.user.id] }
          ];
          this.breadcrumbService.setBreadcrumbs(breadcrumb);
          this.getActiveRoles();
        });
      }

    });
  }

  private getActiveRoles(): void {
    this.loadingActiveUserRoles = true;
    this.rolesService.getActive(false).finally(() => {
      this.loadingActiveUserRoles = false;
    }).subscribe(response => {
      let allActiveUserRoles = <UserRole[]>response.body;
      this.activeUserRoles = allActiveUserRoles.filter(ur => {
        return !this.user.userRoles.some(x => x.id === ur.id && x.name == ur.name);
      });
    });
  }

  public saveUser(): void {
    this.savingUser = true;
    this.usersService.updateUser(this.user).finally(() => {
      this.savingUser = false;
    }).subscribe(response => {
      this.messageService.add({
        severity: 'success',
        summary: 'Successfully Updated User',
        detail: `Updated user ${this.user.userName}`
      });
      this.router.navigate(['/admin/users']);
    });
  }

  public cancel(): void {
    this.router.navigate(["/admin/users"]);
  }

}
