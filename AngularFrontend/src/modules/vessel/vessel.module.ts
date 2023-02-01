import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RouterModule } from '@angular/router';
import { VesselTableComponent } from '.';
import { RoutesDetailsComponent } from './components/routes.details.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDialogModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatPaginatorModule,
  ],
  declarations: [
    VesselTableComponent,
    RoutesDetailsComponent,
  ],
  providers: [],
  exports: [
    VesselTableComponent,
    RoutesDetailsComponent,
  ],
})
export class VesselModule { }