import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { VesselDTO } from '../../../models/api.models';

@Component({
  selector: 'vessel-routes-details',
  templateUrl: './routes.details.component.html',
})
export class RoutesDetailsComponent implements OnChanges {

  @Input() vessels: VesselDTO[] = [];

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      if (propName == "vessels") {
        let vesselsProp = changes[propName];
        this.vessels = vesselsProp.currentValue as VesselDTO[];
      }
    }
  }
}
