import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Alert, AlertType } from '../models/alert.models';
import { AlertDialogComponent } from '../modules/global/components/alert.dialog.component';

@Injectable({ providedIn: 'root' })
export class AlertService {

  constructor(
    private matDialog: MatDialog
  ) { }

  // Convenience methods

  success(message: string): void {
    this.alert(new Alert({ type: AlertType.Success, message }));
  }

  info(message: string): void {
    this.alert(new Alert({ type: AlertType.Info, message }));
  }

  warning(message: string): void {
    this.alert(new Alert({ type: AlertType.Warning, message }));
  }

  error(error: any): void {
    console.log(error);
    let myError = error.error ? error.error : error;
    let message: string = "";
    // Parse backend error message
    message += myError.Message ?
      myError.Message :
      // Parse direct error message
      typeof myError == "string" ?
        myError :
        "Unspecified error.";
    this.alert(new Alert({ type: AlertType.Error, message }));
  }

  // Main method

  alert(alert: Alert): void {
    this.matDialog.open(AlertDialogComponent, {
      data: alert
    });
  }
}
