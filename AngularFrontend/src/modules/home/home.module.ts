import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterModule } from '@angular/router';
import { HomeLayoutComponent, MyWhateversPageComponent } from '.';
import { HomeRouting } from '../../routing/home.routing';
import { GlobalModule } from '../global/global.module';
import { WhateverModule } from '../whatever/whatever.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    HomeRouting,
    GlobalModule,
    WhateverModule,
  ],
  declarations: [
    HomeLayoutComponent,
    MyWhateversPageComponent,
  ],
  providers: [],
  exports: [],
})
export class HomeModule { }
