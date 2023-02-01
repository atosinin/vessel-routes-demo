import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { mergeMap, Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { WhateverDTO } from '../models/whatever.models';
import { IdentityService } from './identity.service';

@Injectable({ providedIn: 'root' })
export class WhateverService {
    
  constructor(
    private http: HttpClient,
    private identityService: IdentityService,
  ) { }

  public getAllWhatevers(): Observable<WhateverDTO[]> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.get<WhateverDTO[]>(`${environment.apiUrl}/Whatever`, httpOptions)));
  }

  public getAllWhateversByUserAccountId(userAccountId: string): Observable<WhateverDTO[]> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.get<WhateverDTO[]>(`${environment.apiUrl}/Whatever/UserAccount/${userAccountId}`, httpOptions)));
  }

  public getWhateverById(whateverId: number): Observable<WhateverDTO> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.get<WhateverDTO>(`${environment.apiUrl}/Whatever/${whateverId}`, httpOptions)));
  }

  public createWhatever(whateverCreate: WhateverDTO): Observable<void> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.post<void>(`${environment.apiUrl}/Whatever/`, whateverCreate, httpOptions)));
  }

  public updateWhatever(whateverUpdate: WhateverDTO): Observable<void> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.put<void>(`${environment.apiUrl}/Whatever/`, whateverUpdate, httpOptions)));
  }

  public deleteWhateverById(whateverId: number): Observable<void> {
    return this.identityService.getHttpOptionsWithBearer()
      .pipe(mergeMap(httpOptions => this.http.delete<void>(`${environment.apiUrl}/Whatever/${whateverId}`, httpOptions)));
  }
}
