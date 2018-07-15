import { Component, Input } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-logout-button',
  templateUrl: './logout-button.component.html',
  styleUrls: ['./logout-button.component.scss']
})
export class LogoutButtonComponent {
  @Input() public class: string = "btn btn-primary";
  public logoutExecuting: boolean = false;

  constructor(private authService: AuthService, private messageService: MessageService) { }

  public signout() {
    this.logoutExecuting = true;
    if (this.authService.isLoggedIn()) {
      this.authService.startLogout().catch(exception => {
        this.logoutExecuting = false;
        this.messageService.add({
          severity: 'error', summary: 'Error Signing Out', detail: `There was an error 
        signing out. The authentication server may not be responding. If the issue persists, please contact your 
        administrator.` });
      });
    }
  }

}
