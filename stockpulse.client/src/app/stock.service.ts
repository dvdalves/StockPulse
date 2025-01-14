import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StockService {
  private readonly apiUrl = '/api/stocks'; // Matches the proxy configuration

  constructor(private http: HttpClient) { }

  getStock(symbol: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${symbol}`);
  }
}
