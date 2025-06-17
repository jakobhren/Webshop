import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Category } from '../model/category'; 

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl: string = "http://localhost:5059/api";


  get authHeader(): string | null
   {return localStorage.getItem  ('headerValue'); }

  constructor(private http: HttpClient) {}

  // Get all categories
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.baseUrl}/category`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Get category by ID
  getCategory(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/category/${id}`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Create a new category
  createCategory(category: Category): Observable<any> {
    return this.http.post(`${this.baseUrl}/category`, category, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  // Delete category by ID
  deleteCategory(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/category/${id}`, {
      headers: {
     "Authorization": `Basic ${this.authHeader}`
     }
   });
  }

  
}
