import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { VesselDTO } from '../../../models/api.models';

@Component({
  selector: 'vessel-routes-details',
  templateUrl: './routes.details.component.html',
})
export class RoutesDetailsComponent {

  _vessels: any;

  @Input() set vessels(value: VesselDTO[]) {
    this._vessels = value;
    // update graph data
    this.data = [];
    for (let vessel of this._vessels) {
      let xPositions = [];
      let yPositions = [];
      for (let position of vessel.positions) {
        xPositions.push(position.x);
        yPositions.push(position.y);
      }
      this.data.push({
        name: vessel.name,
        x: xPositions,
        y: yPositions,
        mode: "lines",
        type: "scatter"
      });
    }
  };
  get vessels(): VesselDTO[] {
    return this._vessels;
  }

  // plotly graph
  data: any = [];
  layout: any = {
    xaxis: { range: [0, 70], title: "Longitude" },
    yaxis: { range: [0, 40], title: "Lattitude" },
    title: "Routes map",
    images: [
      {
        "source": "assets/images/atlantique.jpg",
        "xref": "x",
        "yref": "y",
        "x": 0,
        "y": 40,
        "sizex": 70,
        "sizey": 40,
        "sizing": "stretch",
        "opacity": 1,
        "layer": "below"
      }
    ],
  };

  constructor() { }
}
