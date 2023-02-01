import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first, Subject, takeUntil } from 'rxjs';
import { LoginModel } from '../../../models/identity.models';
import { ErrorMessageModel, ErrorService } from '../../../services/error.service';
import { IdentityService } from '../../../services/identity.service';

@Component({
  templateUrl: './login.page.component.html'
})
export class LoginPageComponent implements OnDestroy {

  returnUrl: string = '';
  email: string = '';

  loginForm: FormGroup;
  backendErrorMessages: ErrorMessageModel[] = [];

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private errorService: ErrorService,
  ) {
    this.activatedRoute.queryParams.subscribe((params) => {
      this.returnUrl = params['returnUrl'];
      this.email = params['email'];
    });
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  onSubmit() {
    this.isLoading = true;
    let login: LoginModel = this.loginForm.value;
    this.identityService
      .login(login)
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: () => {
          if (this.returnUrl) {
            this.router.navigate([this.returnUrl]);
          } else {
            this.router.navigate(['/home']);
          }
        },
        error: (err: any) => {
          this.backendErrorMessages = [];
          this.errorService.parseBackendError(err, this.loginForm, this.backendErrorMessages);
          this.isLoading = false;
        },
      });
  }

  getBackendValidationErrorMessage(key: string): string {
    return this.backendErrorMessages.find(e => e.key == key)?.message || 'Invalid.';
  }
}
