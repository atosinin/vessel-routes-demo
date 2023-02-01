import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { PositionDTO } from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class PositionService {
    
  constructor(
    private http: HttpClient
  ) { }

  public getAllPositions(): Observable<PositionDTO[]> {
    return this.http.get<PositionDTO[]>(`${environment.apiUrl}/Position`);
  }

  public getAllPositionsByVesselId(vesselId: number): Observable<PositionDTO[]> {
    return this.http.get<PositionDTO[]>(`${environment.apiUrl}/Position/Vessel/${vesselId}`);
  }

  public getPositionById(vesselId: number): Observable<PositionDTO> {
    return this.http.get<PositionDTO>(`${environment.apiUrl}/Position/${vesselId}`);
  }

  public createPosition(vesselCreate: PositionDTO): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/Position/`, vesselCreate);
  }

  public updatePosition(vesselUpdate: PositionDTO): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/Position/`, vesselUpdate);
  }

  public deletePositionById(vesselId: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/Position/${vesselId}`);
  }
}
