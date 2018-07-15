import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../shared/services/auth.service';
import { UsersService } from '../../api/services/users.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  public isActive: boolean = false;
  public isAdmin: boolean = false;
  public showMenu: string = '';

  constructor(private authService: AuthService, private usersService: UsersService) { }

  ngOnInit() {
    if (this.authService.isLoggedIn())
    {
      this.usersService.getCurrentUserRoles().subscribe(response => {
        let userRoles = <string[]>response.body;
        if (userRoles.indexOf('CommutationCentral.dev.admin') > -1) {
          this.isAdmin = true;
        }
      });
    }
  }

  private eventCalled(): void {
    this.isActive = !this.isActive;
  }

  private addExpandClass(element: any): void {
    if (element === this.showMenu) {
      this.showMenu = '0';
    } else {
      this.showMenu = element;
    }
  }

}
