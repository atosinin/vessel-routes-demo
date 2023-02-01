import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, Subject, takeUntil } from 'rxjs';
import { ForgottenPasswordModel } from '../../../models/identity.models';
import { AlertService } from '../../../services/alert.services';
import { ErrorMessageModel, ErrorService } from '../../../services/error.service';
import { IdentityService } from '../../../services/identity.service';

@Component({
  templateUrl: './forgotten.password.page.component.html',
})
export class ForgottenPasswordPageComponent implements OnDestroy {

  forgottenPasswordForm: FormGroup;
  backendErrorMessages: ErrorMessageModel[] = [];

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private alertService: AlertService,
    private errorService: ErrorService,
  ) {
    this.forgottenPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  onSubmit() {
    this.isLoading = true;
    let forgottenPassword: ForgottenPasswordModel = this.forgottenPasswordForm.value;
    this.identityService
      .postForgottenPassword(forgottenPassword)
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: () => {
          this.alertService.success('An email has been sent with a link to reset your password.');
          this.isLoading = false;
        },
        error: (err: any) => {
          this.backendErrorMessages = [];
          this.errorService.parseBackendError(err, this.forgottenPasswordForm, this.backendErrorMessages);
          this.isLoading = false;
        },
      });
  }

  getBackendValidationErrorMessage(key: string): string {
    return this.backendErrorMessages.find(e => e.key == key)?.message || 'Invalid.';
  }
}
