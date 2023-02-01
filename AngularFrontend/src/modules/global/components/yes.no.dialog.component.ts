import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface YesNoDialogData {
  title: string;
  question: string;
}

@Component({
  templateUrl: 'yes.no.dialog.component.html',
})
export class YesNoDialogComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: YesNoDialogData
  ) { }
}
