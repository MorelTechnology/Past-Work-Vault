import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RolesService } from '../../../api/services/roles.service';
import { UserRole } from '../../../api/models/user-role';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { MessageService } from 'primeng/components/common/messageservice';
import { User } from '../../../api/models/user';
import { UsersService } from '../../../api/services/users.service';

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.scss']
})
export class EditRoleComponent implements OnInit {
  public role: UserRole;
  public pageTitle: string;
  public roleForm: FormGroup;
  public roleFormSubmitted: boolean = false;
  public loadingUsersInRole: boolean = false;
  public usersInRole: User[];
  public loadingActiveAppUsers: boolean = false;
  public activeAppUsers: User[];

  constructor(private route: ActivatedRoute, private rolesService: RolesService,
    private breadcrumbService: BreadcrumbService, private fb: FormBuilder, private messageService: MessageService,
    private router: Router, private usersService: UsersService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      // TODO handle params doesn't have id
      if (params.has('id')) {
        let roleId = params.get("id");
        this.rolesService.get(roleId).subscribe(response => {
          this.role = <UserRole>response.body;
          this.pageTitle = `Editing Role - ${this.role.name}`;
          let breadcrumb: MenuItem[] = [
            { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
            { label: 'Roles', routerLink: '/admin/roles', routerLinkActiveOptions: { exact: true } },
            {
              label: this.role.name,
              routerLink: ['/admin/roles', this.role.id]
            }
          ];
          this.breadcrumbService.setBreadcrumbs(breadcrumb)
          this.buildRoleForm();
          this.prepareAppUsers();
        });
      }
    });
  }

  private prepareAppUsers() {
    this.loadingUsersInRole = true;
    this.usersService.getUsersInRole(this.role.name).finally(() => {
      this.loadingUsersInRole = false;
    }).subscribe(response => {
      this.usersInRole = <User[]>response.body;
      const control = <FormArray>this.roleForm.controls.users;
      this.usersInRole.forEach(user => {
        control.push(new FormGroup({
          id: new FormControl(user.id),
          name: new FormControl(user.userName),
          email: new FormControl(user.email)
        }));
      });
      this.loadingActiveAppUsers = true;
      this.usersService.getActiveUsersUnpaged().finally(() => {
        this.loadingActiveAppUsers = false;
      }).subscribe(response => {
        let allActiveAppUsers = <User[]>response.body;
        this.activeAppUsers = allActiveAppUsers.filter(u => {
          return !this.usersInRole.some(x => x.id === u.id && x.userName == u.userName);
        });
      });
    });
  }

  private buildRoleForm() {
    this.roleForm = this.fb.group({
      id: new FormControl(this.role.id, [Validators.required]),
      name: new FormControl(this.role.name, [Validators.required]),
      users: new FormArray([
      ])
    });
  }

  public onUsersAdded(event: any) {
    const control = <FormArray>this.roleForm.controls.users;
    let users = event.items;
    users.forEach(user => {
      control.push(new FormGroup({
        id: new FormControl(user.id),
        name: new FormControl(user.userName),
        email: new FormControl(user.email)
      }));
    });
  }

  public onUsersRemoved(event: any) {
    const control = <FormArray>this.roleForm.controls.users;
    let users = event.items;
    users.forEach(user => {
      control.removeAt(control.value.findIndex(existingUser => existingUser.id == user.id));
    });
  }

  public submit() {
    this.roleFormSubmitted = true;
    let modifiedRole = <UserRole>this.roleForm.value;
    this.rolesService.updateRole(modifiedRole).finally(() => {
      this.roleFormSubmitted = false;
    }).subscribe(() => {
      this.messageService.add({
        severity: 'success',
        summary: `Successfully Updated Role`,
        detail: `Edited Role ${modifiedRole.name}`
      });
      this.router.navigate(['/admin/roles']);
    });
  }

  public cancelEdit() {
    this.router.navigate(['/admin/roles']);
  }

}
