import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UrlShortener {
  urlShortenerId: number;
  originalUrl: string;
  shortUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class UrlShortnerService {

  private apiUrl = 'http://localhost:8080/urlshortener'; 

  constructor(private http: HttpClient) { }

  // Fetch all shortened URLs from API
  getShortenedUrls(): Observable<UrlShortener[]> {
    return this.http.get<UrlShortener[]>(this.apiUrl);
  }

  // Post a new URL to shorten
  shortenUrl(originalUrl: string): Observable<UrlShortener> {
    
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.post<UrlShortener>(this.apiUrl,  {originalUrl}, { headers });
  }
}
