import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoggedInGuard } from "../helpers/auth.guard";
import { ErrorPageComponent } from "../modules/global";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'identity',
    pathMatch: "full"
  },
  {
    path: 'error',
    component: ErrorPageComponent,
    data: {
      title: 'Erreur'
    }
  },
  {
    path: '',
    children: [
      {
        path: 'identity',
        loadChildren: () => import('../modules/identity/identity.module').then(m => m.IdentityModule)
      },
      {
        path: 'home',
        canActivate: [LoggedInGuard],
        loadChildren: () => import('../modules/home/home.module').then(m => m.HomeModule)
      },
    ]
  },
  {
    path: '**',
    component: ErrorPageComponent,
    data: {
      title: 'Error'
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRouting { }
