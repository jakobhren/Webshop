import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Ebook } from '../model/ebook';  // Change to import Ebook model

@Injectable({
  providedIn: 'root'
})
export class EbookService {
  baseUrl: string = "http://localhost:5059/api";

  get authHeader(): string | null
  {return localStorage.getItem  ('headerValue'); }

  constructor(private http: HttpClient) {}

  // Get all ebooks
  getEbooks(): Observable<Ebook[]> {
    return this.http.get<Ebook[]>(`${this.baseUrl}/ebook`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Get ebook by ID
  getEbook(id: number): Observable<Ebook> {
    return this.http.get<Ebook>(`${this.baseUrl}/ebook/${id}`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Create a new ebook
  createEbook(ebook: Ebook): Observable<any> {
    return this.http.post(`${this.baseUrl}/ebook`, ebook, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Delete ebook by ID
  deleteEbook(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/ebook/${id}`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }
}
