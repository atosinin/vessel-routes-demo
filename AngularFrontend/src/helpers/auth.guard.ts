import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { IdentityService } from '../services/identity.service';

@Injectable({ providedIn: 'root' })
export class LoggedInGuard implements CanActivate {
  constructor(
    private router: Router,
    private identityService: IdentityService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.identityService.isLoggedIn()) {
      return true;
    }
    // user is not logged in
    // redirected to login page with the return url
    this.router.navigate(['/identity/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
