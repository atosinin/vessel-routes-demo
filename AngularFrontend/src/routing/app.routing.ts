import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NotFoundPageComponent } from "../modules/global";
import { LayoutComponent } from "../modules/global/components/layout.component";

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('../modules/home/home.module').then(m => m.HomeModule)
      },
    ]
  },
  {
    path: '**',
    redirectTo: 'NotFound',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRouting { }
