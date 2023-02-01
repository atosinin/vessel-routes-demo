import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { VesselWithDisplay } from '../../../models/api.models';

@Component({
  selector: 'vessel-table',
  templateUrl: './vessel.table.component.html',
  styleUrls: ['./vessel.table.component.scss']
})
export class VesselTableComponent implements OnChanges {

  @Input() vessels: VesselWithDisplay[] = [];

  @Output() onEdit = new EventEmitter<VesselWithDisplay>();
  @Output() onView = new EventEmitter<VesselWithDisplay>();
  @Output() onDelete = new EventEmitter<VesselWithDisplay>();

  dataSource!: MatTableDataSource<VesselWithDisplay>;
  displayedColumns: string[] = ['name', 'actions'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      if (propName == "vessels") {
        let vesselsProp = changes[propName];
        this.vessels = vesselsProp.currentValue as VesselWithDisplay[];
        this.dataSource = new MatTableDataSource<VesselWithDisplay>(this.vessels);
        this.dataSource.paginator = this.paginator;
      }
    }
  }

  handleEdit(vessel: VesselWithDisplay): void {
    this.onEdit.emit(vessel);
  }

  handleView(vessel: VesselWithDisplay): void {
    this.onView.emit(vessel);
  }

  handleDelete(vessel: VesselWithDisplay): void {
    this.onDelete.emit(vessel);
  }
}
