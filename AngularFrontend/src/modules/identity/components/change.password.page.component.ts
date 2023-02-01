import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first, Subject, takeUntil } from 'rxjs';
import { ChangePasswordModel } from '../../../models/identity.models';
import { AlertService } from '../../../services/alert.services';
import { ErrorMessageModel, ErrorService } from '../../../services/error.service';
import { IdentityService } from '../../../services/identity.service';

@Component({
  templateUrl: './change.password.page.component.html',
})
export class ChangePasswordPageComponent implements OnDestroy {

  email: string = "";
  token: string = "";

  changePasswordForm: FormGroup;
  backendErrorMessages: ErrorMessageModel[] = [];

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private alertService: AlertService,
    private errorService: ErrorService,
  ) {
    this.route.queryParams.subscribe((params) => {
      this.email = params["email"];
      this.token = params["token"];
    });
    this.changePasswordForm = this.formBuilder.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    });
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  onSubmit() {
    this.isLoading = true;
    let changePassword = this.changePasswordForm.value as ChangePasswordModel;
    changePassword.token = this.token;
    changePassword.email = this.email;
    this.isLoading = true;
    this.identityService.changePasswordAndLogin(changePassword)
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: () => {
          this.alertService.success("Your password has been changed.");
          this.router.navigate(['/home']);
        },
        error: err => {
          this.backendErrorMessages = [];
          this.errorService.parseBackendError(err, this.changePasswordForm, this.backendErrorMessages);
          this.isLoading = false;
        }
      });
  }

  getBackendValidationErrorMessage(key: string): string {
    return this.backendErrorMessages.find(e => e.key == key)?.message || 'Invalid.';
  }
}
