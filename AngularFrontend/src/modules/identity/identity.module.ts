import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import {
    ChangePasswordPageComponent,
    ForgottenPasswordPageComponent,
    IdentityLayoutComponent,
    LoginPageComponent,
    RegisterPageComponent
} from '.';
import { IdentityRouting } from '../../routing/identity.routing';
import { GlobalModule } from '../global/global.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    IdentityRouting,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatInputModule,
    MatCheckboxModule,
    GlobalModule,
  ],
  declarations: [
    IdentityLayoutComponent,
    RegisterPageComponent,
    LoginPageComponent,
    ForgottenPasswordPageComponent,
    ChangePasswordPageComponent,
  ],
  providers: [],
  exports: [],
})
export class IdentityModule { }
