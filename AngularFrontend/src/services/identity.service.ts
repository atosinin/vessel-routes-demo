import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { first, map, mergeMap, Observable, switchMap } from 'rxjs';
import { environment } from '../environments/environment';
import { TokenHelpers } from '../helpers/token.helpers';
import {
    ChangePasswordModel,
    ForgottenPasswordModel,
    LoginModel,
    RegisterModel,
    TokenModel,
    UserAccountDTO,
    WhateverUser
} from '../models/identity.models';
import { AlertService } from './alert.services';

@Injectable({ providedIn: 'root' })
export class IdentityService {

  private basicHttpOptions: { headers: HttpHeaders } = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  private withCredentialsHttpOptions: { headers: HttpHeaders, withCredentials: boolean } = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true,
  }; 
  
  constructor(
    private router: Router,
    private http: HttpClient,
    private alertService: AlertService,
  ) { }

  // Identity methods

  public postRegister(register: RegisterModel): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Account/Register`, register, this.basicHttpOptions);
  }

  public login(login: LoginModel): Observable<void> {
    return this.postLogin(login)
      .pipe(mergeMap(tokenModel =>
        this.getProfile(tokenModel)
          .pipe(map(userResponse => {
            let myUser = userResponse as WhateverUser;
            myUser.token = tokenModel;
            this.setStoredUser(myUser);
          }))
      ));
  }

  private postLogin(login: LoginModel): Observable<TokenModel> {
    return this.http.post<TokenModel>(`${environment.apiUrl}/Account/Login`, login, this.withCredentialsHttpOptions);
  }

  public logout(): void {
    if (this.isLoggedIn()) {
      // revoke refresh token
      this.postRevokeToken()
        .pipe(first())
        .subscribe({
          next: () => {
            this.deleteStoredUser();
            this.router.navigate(['/identity/login']);
          },
          error: err => {
            this.alertService.error(err);
            this.deleteStoredUser();
            this.router.navigate(['/identity/login']);
          }
        })
    }
  }

  public postForgottenPassword(forgottenPassword: ForgottenPasswordModel): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Account/ForgottenPassword`, forgottenPassword, this.basicHttpOptions);
  }

  public changePasswordAndLogin(changePassword: ChangePasswordModel): Observable<void> {
    return this.postChangePassword(changePassword)
      .pipe(switchMap(tokenModel =>
        this.getProfile(tokenModel)
          .pipe(map(userResponse => {
            let myUser = userResponse as WhateverUser;
            myUser.token = tokenModel;
            this.setStoredUser(myUser);
          }))
      ));
  }

  private postChangePassword(changePassword: ChangePasswordModel): Observable<TokenModel> {
    return this.http.post<TokenModel>(`${environment.apiUrl}/Account/ChangePassword`, changePassword, this.basicHttpOptions);
  }

  // Tokens

  private postRefreshToken(): Observable<TokenModel> {
    return this.http.post<TokenModel>(`${environment.apiUrl}/Account/RefreshToken`, null, this.withCredentialsHttpOptions);
  }

  private postRevokeToken(): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Account/RevokeToken`, null, this.withCredentialsHttpOptions);
  }

  // Profile helpers

  public isLoggedIn(): boolean {
    return !!this.getStoredUser();
  }

  public getHttpOptionsWithBearer(): Observable<{ headers: HttpHeaders }> {
    return new Observable<{ headers: HttpHeaders }>(subscriber => {
      let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      const user: WhateverUser | null = this.getStoredUser();
      if (!user) {
        subscriber.error(`No user logged in.`);
      } else {
        if (!TokenHelpers.isValid(user.token.token)) {
          // refresh token
          this.postRefreshToken()
            .pipe(first())
            .subscribe({
              next: tokenModel => {
                // reset stored user
                user.token = tokenModel;
                this.setStoredUser(user);
                headers = headers.set('Authorization', 'Bearer ' + user.token.token);
                subscriber.next({ headers });
              },
              error: err => {
                subscriber.error(err);
              }
            });
        } else {
          headers = headers.set('Authorization', 'Bearer ' + user.token.token);
          subscriber.next({ headers });
        }
      }
    });
  }

  public getMyProfile(): UserAccountDTO | null {
    const user: WhateverUser | null = this.getStoredUser();
    if (!user)
      return null;
    return user as UserAccountDTO;
  }

  // Profile API

  private getProfile(token: TokenModel): Observable<UserAccountDTO> {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + token.token);
    return this.http.get<UserAccountDTO>(`${environment.apiUrl}/Profile`, { headers });
  }

  public putProfile(profileUpdate: UserAccountDTO): Observable<void> {
    return this.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.put<void>(`${environment.apiUrl}/Profile`, profileUpdate, httpOptions)));
  }

  public setCulture(culture: string): Observable<void> {
    return this.http.get<void>(`${environment.apiUrl}/Profile/SetCulture/${culture}`, this.basicHttpOptions);
  }

  // Profile in local storage

  private getStoredUser(): WhateverUser | null {
    const storedUser: string | null = localStorage.getItem('whatever-logged-user');
    if (storedUser) {
      return JSON.parse(storedUser) as WhateverUser;
    } else {
      return null;
    };
  }

  private setStoredUser(user: WhateverUser): void {
    localStorage.setItem('whatever-logged-user', JSON.stringify(user));
  }

  private deleteStoredUser(): void {
    localStorage.removeItem('whatever-logged-user');
  }
}
