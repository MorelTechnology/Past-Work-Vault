import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/components/common/api';
import { BreadcrumbService } from '../../../shared/services/breadcrumb.service';
import { FormGroup, FormBuilder, FormControl, Validators, FormArray } from '@angular/forms';
import { UserSearchResult } from '../../../api/models/user-search-result';
import { UsersService } from '../../../api/services/users.service';
import { RolesService } from '../../../api/services/roles.service';
import { UserRole } from '../../../api/models/user-role';
import { User } from '../../../api/models/user';
import { MessageService } from 'primeng/components/common/messageservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {
  public pageTitle: string = 'Creating new user';
  public userForm: FormGroup;
  public userFormSubmitted: boolean = false;
  public userSearchResults: UserSearchResult[];
  public selectedUser: UserSearchResult;
  public loadingActiveUserRoles: boolean = false;
  public activeUserRoles: UserRole[];
  public selectedUserRoles: UserRole[] = [];

  constructor(private breadcrumbService: BreadcrumbService, private fb: FormBuilder, private usersService: UsersService, private rolesService: RolesService,
    private messageService: MessageService, private router: Router) { }

  ngOnInit() {
    let breadcrumb: MenuItem[] = [
      { label: 'Administration', routerLink: '/admin', routerLinkActiveOptions: { exact: true } },
      { label: 'Users', routerLink: '/admin/users', routerLinkActiveOptions: { exact: true } },
      { label: 'New User', routerLink: '/admin/users/new' }
    ];
    this.breadcrumbService.setBreadcrumbs(breadcrumb);
    this.getActiveRoles();
    this.buildUserForm();
  }

  private buildUserForm(): void {
    this.userForm = this.fb.group({
      Email: new FormControl(""),
      UserName: new FormControl("", [Validators.required]),
      DisplayName: new FormControl(""),
      UserRoles: new FormArray([
      ])
    });
  }

  private getActiveRoles(): void {
    this.loadingActiveUserRoles = true;
    this.rolesService.getActive(false).finally(() => {
      this.loadingActiveUserRoles = false;
    }).subscribe(response => {
      this.activeUserRoles = <UserRole[]>response.body;
    });
  }

  public submit(): void {
    this.userFormSubmitted = true;
    let userToCreate = <User>this.userForm.value;
    this.usersService.createUser(userToCreate).finally(() => {
      this.userFormSubmitted = false;
    }).subscribe(response => {
      let createdUser = <User>response.body;
      this.messageService.add({
        severity: 'success',
        summary: `Successfully Created User`,
        detail: `Created user ${createdUser.userName}`
      });
      this.router.navigate(['/admin/users']);
    });
  }

  public cancel(): void {
    this.router.navigate(['/admin/users']);
  }

  public searchUsers(event: any) {
    let query = event.query;
    this.usersService.searchUsers(query).subscribe(response => {
      this.userSearchResults = <UserSearchResult[]>response.body;
    });
  }

  public onSelectUser(user?: UserSearchResult) {
    if (user) {
      this.userForm.controls.Email.setValue(user.email);
      this.userForm.controls.UserName.setValue(user.userName);
      this.userForm.controls.DisplayName.setValue(user.displayName);
    } else {
      this.userForm.controls.Email.setValue("");
      this.userForm.controls.UserName.setValue("");
      this.userForm.controls.DisplayName.setValue("");
    }
    this.userForm.controls.UserName.markAsDirty();
  }

  public onRolesAdded(event: any) {
    const control = <FormArray>this.userForm.controls.UserRoles;
    let roles = event.items;
    roles.forEach(role => {
      control.push(new FormGroup({
        Id: new FormControl(role.id),
        Name: new FormControl(role.name)
      }));
    });
  }

  public onRolesRemoved(event: any) {
    const control = <FormArray>this.userForm.controls.UserRoles;
    let roles = event.items;
    roles.forEach(role => {
      control.removeAt(control.value.findIndex(existingRole => existingRole.Id == role.id));
    });
  }

}
