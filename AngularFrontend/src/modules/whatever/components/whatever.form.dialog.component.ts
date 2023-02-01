import { Component, Inject, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { first, Subject, takeUntil } from 'rxjs';
import { WhateverDTO } from '../../../models/whatever.models';
import { AlertService } from '../../../services/alert.services';
import { ErrorMessageModel, ErrorService } from '../../../services/error.service';
import { WhateverService } from '../../../services/whatever.service';

export interface WhateverFormDialogData {
  whatever: WhateverDTO;
}

@Component({
  templateUrl: 'whatever.form.dialog.component.html',
})
export class WhateverFormDialogComponent implements OnDestroy {

  whateverForm: FormGroup;
  backendErrorMessages: ErrorMessageModel[] = [];

  title: string = 'Create whatever';
  buttonText: string = 'Create';
  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: WhateverFormDialogData,
    public dialogRef: MatDialogRef<WhateverFormDialogComponent>,
    private formBuilder: FormBuilder,
    private whateverService: WhateverService,
    private alertService: AlertService,
    private errorService: ErrorService,
  ) {
    this.whateverForm = this.formBuilder.group({
      whateverId: [data.whatever.whateverId],
      name: [data.whatever.name, Validators.required],
      description: [data.whatever.description],
      userAccountId: [data.whatever.userAccountId, Validators.required],
    });
    if (data.whatever.whateverId) {
      this.title = "Edit whatever";
      this.buttonText = "Update";
    }
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  onSubmit(): void {
    this.isLoading = true;
    const whatever = this.whateverForm.value as WhateverDTO;
    if (this.data.whatever.whateverId) {
      // Update
      this.whateverService.updateWhatever(whatever)
        .pipe(takeUntil(this.unsubscriber))
        .pipe(first())
        .subscribe({
          next: () => {
            this.dialogRef.close('success');
            this.alertService.success("Whatever updated.")
          },
          error: err => {
            this.backendErrorMessages = [];
            this.errorService.parseBackendError(err, this.whateverForm, this.backendErrorMessages);
            this.isLoading = false;
          }
        });
    } else {
      // Create
      this.whateverService.createWhatever(whatever)
        .pipe(takeUntil(this.unsubscriber))
        .pipe(first())
        .subscribe({
          next: () => {
            this.dialogRef.close('success');
            this.alertService.success("Whatever created.")
          },
          error: err => {
            this.backendErrorMessages = [];
            this.errorService.parseBackendError(err, this.whateverForm, this.backendErrorMessages);
            this.isLoading = false;
          }
        });
    }
  }

  getBackendValidationErrorMessage(key: string): string {
    return this.backendErrorMessages.find(e => e.key == key)?.message || 'Invalid.';
  }
}
