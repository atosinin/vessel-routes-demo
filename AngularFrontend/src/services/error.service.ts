import { Injectable } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { AlertService } from "./alert.services";

export interface ErrorMessageModel {
  key: string,
  message: string
}

@Injectable({ providedIn: 'root' })
export class ErrorService {

  constructor(
    private alertService: AlertService,
  ) { }

  public parseBackendError(
    error: any,
    form: FormGroup,
    backendErrorMessages: ErrorMessageModel[]
  ) {
    let backendError = error.error ? error.error : error;
    if (backendError.type && backendError.type == "https://tools.ietf.org/html/rfc7231#section-6.5.1") {
      // Validation errors on backend
      for (const key in backendError.errors) {
        const camelCaseKey: string = key.charAt(0).toLowerCase() + key.substring(1);
        form.controls[camelCaseKey].setErrors({ backendValidation: true });
        backendErrorMessages.push({
          key: camelCaseKey,
          message: backendError.errors[key].join(' ')
        });
      }
      this.alertService.warning("Your form contains errors.");
    } else {
      this.alertService.error(error);
    }
  }
}
