import { VesselDTO } from "./api.models";

export interface VesselWithDisplay extends VesselDTO {
  isOnDisplay: boolean;
};
