import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { environment } from "../../../../environments/environment";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  public isActive: boolean = false;
  public showMenu: string = '';
  public isAdmin: boolean = false;
  public adminModuleLink: string = environment.adminModuleLink;

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.checkIfAdmin();
  }

  private checkIfAdmin(): void {
    this.authenticationService.isAdmin().subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });
  }

  private eventCalled(): void {
    this.isActive = !this.isActive;
  }

  public addExpandClass(element: any): void {
    if (element === this.showMenu) {
      this.showMenu = '0';
    } else {
      this.showMenu = element;
    }
  }

}
