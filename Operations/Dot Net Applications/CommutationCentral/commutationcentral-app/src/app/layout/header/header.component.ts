import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from "@angular/router";
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  private pushRightClass: string = 'push-right';
  public user;

  constructor(private router: Router, private authService: AuthService) {
    this.router.events.subscribe((val) => {
      if (val instanceof NavigationEnd && window.innerWidth <= 992 && this.isToggled()) {
        this.toggleSidebar();
      }
    });
  }

  ngOnInit() {
    if (this.authService.isLoggedIn())
    {
      this.user = this.authService.getClaims();
    }
  }

  private isToggled(): boolean {
    const dom: Element = document.querySelector('body');
    return dom.classList.contains(this.pushRightClass);
  }

  public toggleSidebar(): void {
    const dom: Element = document.querySelector('body');
    dom.classList.toggle(this.pushRightClass);
  }

  public logout() {

  }

}
