import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { WhateverDTO } from '../../../models/whatever.models';

@Component({
  selector: 'whatever-table',
  templateUrl: './whatever.table.component.html',
  styleUrls: ['./whatever.table.component.scss']
})
export class WhateverTableComponent implements OnChanges {

  @Input() whatevers: WhateverDTO[] = [];

  @Output() edit = new EventEmitter<WhateverDTO>();
  @Output() delete = new EventEmitter<WhateverDTO>();

  dataSource!: MatTableDataSource<WhateverDTO>;
  displayedColumns: string[] = ['name', 'description', 'actions'];
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      if (propName == "whatevers") {
        let whateversProp = changes[propName];
        this.whatevers = whateversProp.currentValue as WhateverDTO[];
        this.dataSource = new MatTableDataSource<WhateverDTO>(this.whatevers);
        this.dataSource.paginator = this.paginator;
      } 
    }
  }

  onEdit(whatever: WhateverDTO): void {
    this.edit.emit(whatever);
  }

  onDelete(whatever: WhateverDTO): void {
    this.delete.emit(whatever);
  }
}
