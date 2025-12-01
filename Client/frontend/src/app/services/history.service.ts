import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators'; 

export interface History {
  id: number;
  text: string;
  fullName: string;
  dateTime: string;
  eventType: string;
}

export interface PaginatedHistory {
  Histories: History[];
  TotalCount: number;
}

@Injectable({ providedIn: 'root' })
export class HistoryService {
  private apiUrl = 'http://localhost:5000/History/getall';

  constructor(private http: HttpClient) { }

  getAllServerSide(
    CurrentPage: number,
    PageSize: number,
    OrderBy: string,
    TextFilter: string,
    UserFilter: string,
    EventTypeFilter: string,
    StartDate?: string,
    EndDate?: string
  ): Observable<PaginatedHistory> {
    const order = OrderBy && OrderBy.trim() ? OrderBy : 'dateTime desc';

    let params = new HttpParams()
      .set('CurrentPage', CurrentPage)
      .set('PageSize', PageSize)
      .set('OrderBy', order);

    if (TextFilter) params = params.set('TextFilter', TextFilter);
    if (UserFilter) params = params.set('UserFilter', UserFilter);
    if (EventTypeFilter) params = params.set('EventTypeFilter', EventTypeFilter);
    if (StartDate) params = params.set('StartDate', StartDate);
    if (EndDate) params = params.set('EndDate', EndDate);

    return this.http.get<PaginatedHistory>(this.apiUrl, { params }).pipe(
      tap(data => {
        console.log('Данные от сервера (PaginatedHistory):', data);
      })
    );
  }
}