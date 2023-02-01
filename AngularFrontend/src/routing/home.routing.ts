import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { NotFoundPageComponent } from "../modules/global";
import { AllVesselsPageComponent } from "../modules/home";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        redirectTo: 'AllVesselRoutes',
        pathMatch: "full"
      },
      {
        path: 'AllVesselRoutes',
        component: AllVesselsPageComponent,
        data: {
          title: 'All vessel routes'
        }
      },
      {
        path: 'NotFound',
        component: NotFoundPageComponent,
        data: {
          title: 'Not found'
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
export class HomeRouting { }
