import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { VesselDTO } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class VesselService {
    
  constructor(
    private http: HttpClient
  ) { }

  public getAllVessels(): Observable<VesselDTO[]> {
    return this.http.get<VesselDTO[]>(`${environment.apiUrl}/Vessel`);
  }

  public getVesselById(vesselId: number): Observable<VesselDTO> {
    return this.http.get<VesselDTO>(`${environment.apiUrl}/Vessel/${vesselId}`);
  }

  public createVessel(vesselCreate: VesselDTO): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Vessel/`, vesselCreate);
  }

  public updateVessel(vesselUpdate: VesselDTO): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/Vessel/`, vesselUpdate);
  }

  public deleteVesselById(vesselId: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/Vessel/${vesselId}`);
  }
}
