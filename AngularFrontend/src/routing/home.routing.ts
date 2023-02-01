import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeLayoutComponent, MyWhateversPageComponent } from "../modules/home";

const routes: Routes = [
  {
    path: '',
    component: HomeLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'myWhatevers',
        pathMatch: "full"
      },
      {
        path: 'myWhatevers',
        component: MyWhateversPageComponent,
        data: {
          title: 'All my whatevers'
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
