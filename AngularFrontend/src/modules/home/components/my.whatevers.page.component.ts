import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { first, Subject, takeUntil } from 'rxjs';
import { UserAccountDTO } from '../../../models/identity.models';
import { WhateverDTO } from '../../../models/whatever.models';
import { AlertService } from '../../../services/alert.services';
import { IdentityService } from '../../../services/identity.service';
import { WhateverService } from '../../../services/whatever.service';
import { YesNoDialogComponent } from '../../global';
import { WhateverFormDialogComponent } from '../../whatever/components/whatever.form.dialog.component';

@Component({
  templateUrl: './my.whatevers.page.component.html'
})
export class MyWhateversPageComponent implements OnInit, OnDestroy {

  myUser: UserAccountDTO | null = null;
  allMyWhatevers: WhateverDTO[] = [];
  myWhatever: WhateverDTO | null = null;

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private identityService: IdentityService,
    private whateverService: WhateverService,
    private alertService: AlertService,
    private matDialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.myUser = this.identityService.getMyProfile();
    this.loadAllMyWhatevers();
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  private loadAllMyWhatevers() {
    this.isLoading = true;
    this.whateverService.getAllWhateversByUserAccountId(this.myUser!.id)
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: allWhatevers => {
          this.allMyWhatevers = allWhatevers;
          this.isLoading = false;
        },
        error: err => {
          this.alertService.error(err);
          this.isLoading = false;
        }
      });
  }

  onAdd(): void {
    const createDialogRef = this.matDialog.open(WhateverFormDialogComponent, {
      data: {
        whatever: {
          whateverId: 0,
          name: '',
          description: '',
          userAccountId: this.myUser!.id
        }
      }
    });
    createDialogRef.afterClosed()
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: result => {
          if (result == 'success') {
            this.loadAllMyWhatevers();
          }
        },
        error: err => {
          this.alertService.error(err);
        }
      });
  }

  onEdit(whatever: WhateverDTO): void {
    const updateDialogRef = this.matDialog.open(WhateverFormDialogComponent, {
      data: { whatever }
    });
    updateDialogRef.afterClosed()
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: result => {
          if (result == 'success') {
            this.loadAllMyWhatevers();
          }
        },
        error: err => {
          this.alertService.error(err);
        }
      });
  }

  onDelete(whatever: WhateverDTO): void {
    const confirmDeleteDialogRef = this.matDialog.open(YesNoDialogComponent, {
      data: { question: `Are you sure you want to delete ${whatever.name}?` }
    });
    confirmDeleteDialogRef.afterClosed()
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: result => {
          if (result == 'yes') {
            this.whateverService.deleteWhateverById(whatever.whateverId)
              .subscribe({
                next: () => {
                  this.alertService.success("Whatever deleted.");
                  this.loadAllMyWhatevers();
                },
                error: err => {
                  this.alertService.error(err);
                }
              })
          }
        },
        error: err => {
          this.alertService.error(err);
        }
      });
  }
}
