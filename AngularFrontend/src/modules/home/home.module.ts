import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterModule } from '@angular/router';
import { AllVesselsPageComponent } from '.';
import { HomeRouting } from '../../routing/home.routing';
import { GlobalModule } from '../global/global.module';
import { VesselModule } from '../vessel/vessel.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    HomeRouting,
    GlobalModule,
    VesselModule,
  ],
  declarations: [
    AllVesselsPageComponent,
  ],
  providers: [],
  exports: [],
})
export class HomeModule { }
