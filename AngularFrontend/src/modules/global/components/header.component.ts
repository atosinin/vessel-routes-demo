import { Component, OnInit } from '@angular/core';
import { UserAccountDTO } from '../../../models/identity.models';
import { IdentityService } from '../../../services/identity.service';

@Component({
  selector: 'whatever-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  public isLoggedIn: boolean = false;

  constructor(
    private identityService: IdentityService,
  ) { }

  ngOnInit(): void {
    this.isLoggedIn = this.identityService.isLoggedIn();
  }

  logout(): void {
    this.identityService.logout();
  }
}
