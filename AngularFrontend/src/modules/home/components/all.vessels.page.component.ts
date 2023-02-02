import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { first, Subject, takeUntil } from 'rxjs';
import { findIntersectionsForRoutes } from '../../../helpers/find.intersections';
import { VesselDTO } from '../../../models/api.models';
import { VesselWithDisplay } from '../../../models/vessel.models';
import { AlertService } from '../../../services/alert.services';
import { PositionService } from '../../../services/position.service';
import { VesselService } from '../../../services/vessel.service';
import { YesNoDialogComponent } from '../../global';

@Component({
  templateUrl: './all.vessels.page.component.html'
})
export class AllVesselsPageComponent implements OnInit, OnDestroy {

  allVesselRoutes: VesselWithDisplay[] = [];
  routesToDisplay: VesselDTO[] = [];
  myVesselRoute: VesselDTO | null = null;

  isLoading: boolean = false;
  unsubscriber = new Subject<void>;

  constructor(
    private vesselService: VesselService,
    private positionService: PositionService,
    private alertService: AlertService,
    private matDialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.loadAllVessels();
  }

  ngOnDestroy(): void {
    this.unsubscriber.next();
    this.unsubscriber.complete();
  }

  private loadAllVessels() {
   this.isLoading = true;
   this.vesselService.getAllVessels()
     .pipe(takeUntil(this.unsubscriber))
     .pipe(first())
     .subscribe({
       next: allVessels => {
         this.allVesselRoutes = allVessels.map(v => {
           let result = v as VesselWithDisplay;
           result.isOnDisplay = false;
           return result;
         });
         this.isLoading = false;
       },
       error: err => {
         this.alertService.error(err);
         this.isLoading = false;
       }
     });
  }

  handleAdd(): void {
    this.alertService.warning("TODO");
  }

  handleEdit(vessel: VesselWithDisplay): void {
    this.alertService.warning("TODO");
  }

  handleView(vessel: VesselWithDisplay): void {
    vessel.isOnDisplay = !vessel.isOnDisplay;
    if (vessel.isOnDisplay) {
      this.positionService.getAllPositionsByVesselId(vessel.vesselId)
        .pipe(takeUntil(this.unsubscriber))
        .pipe(first())
        .subscribe({
          next: allPositions => {
            vessel.positions = allPositions;
            this.routesToDisplay.push(vessel as VesselDTO);
            // to force detect change in child component
            this.routesToDisplay = [...this.routesToDisplay];
          },
          error: err => {
            this.alertService.error(err);
          }
        })
    } else {
      let indexToDisplay = this.routesToDisplay.findIndex(v => v.vesselId == vessel.vesselId);
      this.routesToDisplay.splice(indexToDisplay, 1);
      // to force detect change in child component
      this.routesToDisplay = [...this.routesToDisplay];
    }
  }

  handleDelete(vessel: VesselWithDisplay): void {
    const confirmDeleteDialogRef = this.matDialog.open(YesNoDialogComponent, {
      data: { question: `Are you sure you want to delete ${vessel.name}?` }
    });
    confirmDeleteDialogRef.afterClosed()
      .pipe(takeUntil(this.unsubscriber))
      .pipe(first())
      .subscribe({
        next: result => {
          if (result == 'yes') {
            this.alertService.warning("TODO");
          }
        },
        error: err => {
          this.alertService.error(err);
        }
      });
  }

  handleCheckIntersections(): void {
    let intersections = findIntersectionsForRoutes(this.routesToDisplay, { deltaDistanceInKilometers: 2, deltaTimeInMinutes: 5 });
    if (intersections.length) {
      let message = `Found intersection for routes:`;
      for (let i of intersections) {
        const allVessels = i.vessels.map(v => ` ${this.routesToDisplay.find(r => r.vesselId == v.vesselId)?.name} will be present around ${v.arrivesIn}`)
        message += ` Close by (${i.xMidZone}, ${i.yMidZone}),${allVessels}.`;
      }
      this.alertService.success(message);
    } else {
      this.alertService.warning("No intersection found for these routes.");
    }
  }
}
