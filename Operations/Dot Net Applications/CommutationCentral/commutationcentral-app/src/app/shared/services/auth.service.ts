import { Injectable } from '@angular/core';
import { UserManagerSettings, UserManager, User } from "oidc-client";
import { environment } from "../../../environments/environment";
import { MessageService } from 'primeng/components/common/messageservice';

export function getClientSettings(): UserManagerSettings {
  return {
    authority: environment.identity_url,
    client_id: 'commutationCentralApp',
    redirect_uri: environment.origin_url + 'auth-callback',
    post_logout_redirect_uri: environment.origin_url + 'signout-callback',
    response_type: "id_token token",
    scope: "openid profile commutationCentralApi",
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: environment.origin_url + 'silent-refresh.html'
  };
}

@Injectable()
export class AuthService {
  private manager = new UserManager(getClientSettings());
  private user: User = null;
  private userRoles: string[];

  constructor(private messageService: MessageService) {
    this.manager.getUser().then(user => {
      this.user = user;
    });
    this.manager.events.addUserLoaded(user => {
      this.user = user;
    });
  }

  public isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  public getClaims(): any {
    return this.user.profile;
  }

  public getAuthorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  public startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  public completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
    });
  }

  public startLogout(): Promise<void> {
    return this.manager.signoutRedirect();
  }

  public completeLogout(): Promise<void> {
    return this.manager.signoutRedirectCallback().then(() => {
      this.user = null;
    });
  }

  public getPreAuthUrl(): string {
    return sessionStorage.getItem('preAuthUrl') || "/";
  }

  public setPreAuthUrl(url: string): void {
    sessionStorage.setItem('preAuthUrl', url);
  }

  public signinSilentCallback(): Promise<void> {
    return this.manager.signinSilentCallback();
  }

}
