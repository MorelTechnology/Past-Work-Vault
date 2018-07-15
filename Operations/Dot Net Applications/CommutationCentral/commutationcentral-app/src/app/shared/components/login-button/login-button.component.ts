import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'app-login-button',
  templateUrl: './login-button.component.html',
  styleUrls: ['./login-button.component.scss']
})
export class LoginButtonComponent {
  public loginExecuting: boolean = false;

  constructor(private authService: AuthService, private messageService: MessageService) { }

  public login() {
    this.loginExecuting = true;
    this.authService.setPreAuthUrl("/");
    this.authService.startAuthentication().catch(exception => {
      this.loginExecuting = false;
      this.messageService.add({
        severity: 'error', summary: 'Error Authenticating', detail: `There was an error 
        authenticating. The authentication server may not be responding. If the issue persists, please contact your 
        administrator.` });
    });
  }

}
