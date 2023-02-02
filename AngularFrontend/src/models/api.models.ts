export interface VesselDTO {
  vesselId: number;
  name: string;
  positions: PositionDTO[];
}

export interface PositionDTO {
  positionId: number;
  x: number;
  y: number;
  timestamp: Date;
  vesselId: number;
}
