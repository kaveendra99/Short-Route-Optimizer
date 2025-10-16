import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ShortestPathResult {
  success: boolean;
  shortestPath: string[];
  totalDistance: number;
  errorMessage: string | null;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
  error: string | null;
  timestamp: string;
}

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  private apiUrl = 'http://localhost:5029/api';

  constructor(private http: HttpClient) { }

  getAllNodes(): Observable<ApiResponse<string[]>> {
    return this.http.get<ApiResponse<string[]>>(`${this.apiUrl}/route/nodes`);
  }

  calculateShortestRoute(startNode: string, endNode: string): Observable<ApiResponse<ShortestPathResult>> {
    const params = new HttpParams()
      .set('start', startNode)
      .set('end', endNode);

    return this.http.get<ApiResponse<ShortestPathResult>>(
      `${this.apiUrl}/route/shortest-path`, 
      { params }
    );
  }
}