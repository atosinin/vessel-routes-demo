import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Alert, AlertType } from '../../../models/alert.models';

@Component({
  templateUrl: 'alert.dialog.component.html',
})
export class AlertDialogComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) public alertDialogData: Alert
  ) { }

  public getNameForALert(): string {
    let name: string;
    switch (this.alertDialogData.type) {
      case AlertType.Success:
        name = "SUCCESS";
        break;
      case AlertType.Info:
        name = "INFORMATION";
        break;
      case AlertType.Warning:
        name = "WARNING";
        break;
      case AlertType.Error:
      default:
        name = "ERROR";
        break;
    }
    return name;
  }

  public getStyleForALert(): string {
    let style: string;
    switch (this.alertDialogData.type) {
      case AlertType.Success:
        style = "color: green";
        break;
      case AlertType.Info:
        style = "color: blue";
        break;
      case AlertType.Warning:
        style = "color: orange";
        break;
      case AlertType.Error:
      default:
        style = "color: red";
        break;
    }
    return style;
  }
}
