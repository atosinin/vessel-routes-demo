import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first, Subject, takeUntil } from 'rxjs';
import { RegisterModel } from '../../../models/identity.models';
import { AlertService } from '../../../services/alert.services';
import { ErrorMessageModel, ErrorService } from '../../../services/error.service';
import { IdentityService } from '../../../services/identity.service';

@Component({
  templateUrl: './register.page.component.html',
})
export class RegisterPageComponent implements OnDestroy {

  registerForm: FormGroup;
  backendErrorMessages: ErrorMessageModel[] = [];

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private alertService: AlertService,
    private errorService: ErrorService,
  ) {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      hasAcceptedTerms: [false, Validators.requiredTrue],
    });
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  onSubmit() {
    this.isLoading = true;
    let registerUser: RegisterModel = this.registerForm.value;
    this.identityService
      .postRegister(registerUser)
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: () => {
          this.alertService.success('An email has been sent with a link to confirm your email address.');
          this.isLoading = false;
        },
        error: (err: any) => {
          this.backendErrorMessages = [];
          this.errorService.parseBackendError(err, this.registerForm, this.backendErrorMessages);
          this.isLoading = false;
        },
      });
  }

  getBackendValidationErrorMessage(key: string): string {
    return this.backendErrorMessages.find(e => e.key == key)?.message || 'Invalid.';
  }
}
