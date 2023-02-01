import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import {
    ChangePasswordPageComponent,
    ForgottenPasswordPageComponent,
    IdentityLayoutComponent,
    LoginPageComponent,
    RegisterPageComponent
} from "../modules/identity";

const routes: Routes = [
  {
    path: '',
    component: IdentityLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'login',
        pathMatch: "full"
      },
      {
        path: 'register',
        component: RegisterPageComponent,
        data: {
          title: 'Register'
        }
      },
      {
        path: 'login',
        component: LoginPageComponent,
        data: {
          title: 'Login'
        }
      },
      {
        path: 'forgottenPassword',
        component: ForgottenPasswordPageComponent,
        data: {
          title: 'Forgotten password'
        }
      },
      {
        path: 'changePassword',
        component: ChangePasswordPageComponent,
        data: {
          title: 'Change password'
        }
      },
    ]
  }
];

@NgModule(
  {
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  }
)
export class IdentityRouting { }
