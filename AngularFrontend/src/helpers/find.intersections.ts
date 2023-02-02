import { VesselDTO } from "../models/api.models";

export interface FindIntersectionsOptions {
  deltaDistanceInKilometers: number;
  deltaTimeInMinutes: number;
}

export interface Intersection {
  xMidZone: number,
  yMidZone: number
  vessels: VesselInZone[];
}

interface SearchZone {
  passingVessels: VesselInZone[];
}

interface VesselInZone {
  vesselId: number;
  arrivesIn: Date;
  leavesAt: Date;
}

interface SearchPosition {
  x: number;
  y: number;
  timeInMinutes: number;
}

export function findIntersectionsForRoutes(routes: VesselDTO[], options: FindIntersectionsOptions): Intersection[] {

  function recordPositionInSearchArea(position: SearchPosition, vessel: VesselDTO) {
    // find zone in search area
    const xIndexZone: number = Math.floor((position.x - xMinSearchArea) / options.deltaDistanceInKilometers);
    const yIndexZone: number = Math.floor((position.y - yMinSearchArea) / options.deltaDistanceInKilometers);
    let zone = searchArea[xIndexZone][yIndexZone];
    if (zone) {
      let myVessel: VesselInZone | undefined = zone.passingVessels.find(v => v.vesselId == vessel.vesselId);
      if (myVessel) {
        myVessel.leavesAt = new Date(position.timeInMinutes * 60 * 1000);
      } else {
        // add my vessel in zone
        const myVessel: VesselInZone = {
          vesselId: vessel.vesselId,
          arrivesIn: new Date(position.timeInMinutes * 60 * 1000),
          leavesAt: new Date(position.timeInMinutes * 60 * 1000),
        };
        zone.passingVessels.push(myVessel);
      }
      let anotherVessel: VesselInZone | undefined = zone.passingVessels.find(v => v.vesselId != vessel.vesselId);
      if (anotherVessel) {
        // this is an intersection zone
        const myIntersection = intersectionZones.find(z => z.xIndex == xIndexZone && z.yIndex == yIndexZone);
        if (!myIntersection) {
          intersectionZones.push({ xIndex: xIndexZone, yIndex: yIndexZone });
        }
      }
    } else {
      // empty zone, add my vessel
      const myVessel: VesselInZone = {
        vesselId: vessel.vesselId,
        arrivesIn: new Date(position.timeInMinutes * 60 * 1000),
        leavesAt: new Date(position.timeInMinutes * 60 * 1000),
      };
      searchArea[xIndexZone][yIndexZone] = {
        passingVessels: [myVessel]
      };
    }
  }

  let intersectionZones: {xIndex: number, yIndex: number}[] = [];
  // Initialize search area
  const xMinSearchArea: number = Math.min(...routes.map(v =>
    Math.min(...v.positions.map(p => p.x))
  ));
  const xMaxSearchArea: number = Math.max(...routes.map(v =>
    Math.max(...v.positions.map(p => p.x))
  ));
  const yMinSearchArea: number = Math.min(...routes.map(v =>
    Math.min(...v.positions.map(p => p.y))
  ));
  const yMaxSearchArea: number = Math.max(...routes.map(v =>
    Math.max(...v.positions.map(p => p.y))
  ));
  const xSizeSearchArea = Math.ceil((xMaxSearchArea - xMinSearchArea) / options.deltaDistanceInKilometers) + 1;
  const ySizeSearchArea = Math.ceil((yMaxSearchArea - yMinSearchArea) / options.deltaDistanceInKilometers) + 1;
  const searchArea: ((SearchZone | null)[])[] = [];
  for (let i = 0; i < xSizeSearchArea; i++) {
    let column: (SearchZone | null)[] = [];
    for (let j = 0; j < ySizeSearchArea; j++) {
      column.push(null);
    }
    searchArea.push(column);
  }

  const deltaTimeInMilliseconds: number = options.deltaTimeInMinutes * 60 * 1000;
  // compute each route
  for (let route of routes) {

    // record initial position in SearchArea
    let currentPosition: SearchPosition = {
      x: route.positions[0].x,
      y: route.positions[0].y,
      timeInMinutes: Math.floor((new Date(route.positions[0].timestamp)).getTime() / (1000 * 60)),
    };
    recordPositionInSearchArea(currentPosition, route);

    // move along the route
    let positionIndex = 0;
    let originPoint: SearchPosition = {
      x: route.positions[0].x,
      y: route.positions[0].y,
      timeInMinutes: Math.floor((new Date(route.positions[0].timestamp)).getTime() / (1000 * 60)),
    };
    let destinationPoint: SearchPosition = {
      x: route.positions[1].x,
      y: route.positions[1].y,
      timeInMinutes: Math.floor((new Date(route.positions[1].timestamp)).getTime() / (1000 * 60)),
    };
    let remainingDeltaTimeInMinutes: number = options.deltaTimeInMinutes;
    while (positionIndex < route.positions.length - 1) {
      // compute next position in time
      while (currentPosition.timeInMinutes + remainingDeltaTimeInMinutes > destinationPoint.timeInMinutes) {
        // move to next segment of route
        positionIndex++;
        if (positionIndex >= route.positions.length - 1) {
          break;
        }
        originPoint = destinationPoint;
        destinationPoint = {
          x: route.positions[positionIndex + 1].x,
          y: route.positions[positionIndex + 1].y,
          timeInMinutes: Math.floor((new Date(route.positions[positionIndex + 1].timestamp)).getTime() / (1000 * 60)),
        };
        // reset search variables
        remainingDeltaTimeInMinutes = remainingDeltaTimeInMinutes - (originPoint.timeInMinutes - currentPosition.timeInMinutes);
        currentPosition = originPoint;
      }
      if (positionIndex >= route.positions.length - 1) {
        break;
      }
      currentPosition.x = originPoint.x
        + (destinationPoint.x - originPoint.x) * (currentPosition.timeInMinutes + remainingDeltaTimeInMinutes - originPoint.timeInMinutes) / (destinationPoint.timeInMinutes - originPoint.timeInMinutes);
      currentPosition.y = originPoint.y
        + (destinationPoint.y - originPoint.y) * (currentPosition.timeInMinutes + remainingDeltaTimeInMinutes - originPoint.timeInMinutes) / (destinationPoint.timeInMinutes - originPoint.timeInMinutes);
      currentPosition.timeInMinutes = currentPosition.timeInMinutes + remainingDeltaTimeInMinutes;
      // reset
      remainingDeltaTimeInMinutes = options.deltaTimeInMinutes;
      // record current position in SearchArea
      recordPositionInSearchArea(currentPosition, route);
    }
  }

  let result: Intersection[] = [];
  for (let intersectionZone of intersectionZones) {
    const zone = searchArea[intersectionZone.xIndex][intersectionZone.yIndex];
    if (zone) {
      const myIntersection: Intersection = {
        xMidZone: xMinSearchArea + (intersectionZone.xIndex + 0.5) * options.deltaDistanceInKilometers,
        yMidZone: yMinSearchArea + (intersectionZone.yIndex + 0.5) * options.deltaDistanceInKilometers,
        vessels: zone.passingVessels,
      };
      result.push(myIntersection);
    }
  }
  return result;
}
