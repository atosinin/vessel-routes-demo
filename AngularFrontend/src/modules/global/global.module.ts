import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';
import {
    AlertDialogComponent, FooterComponent,
    HeaderComponent, NotFoundPageComponent, YesNoDialogComponent
} from '.';
import { LayoutComponent } from './components/layout.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    FlexLayoutModule,
    MatCardModule,
  ],
  declarations: [
    NotFoundPageComponent,
    LayoutComponent,
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
