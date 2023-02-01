import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import {
  ErrorPageComponent,
  FooterComponent,
  HeaderComponent,
  AlertDialogComponent,
  YesNoDialogComponent
} from '.';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
  ],
  declarations: [
    ErrorPageComponent,
    HeaderComponent,
    FooterComponent,
    AlertDialogComponent,
    YesNoDialogComponent,
  ],
  providers: [],
  exports: [
    HeaderComponent,
    FooterComponent,
  ],
})
export class GlobalModule { }
