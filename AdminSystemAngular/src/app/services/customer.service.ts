import { HttpClient, HttpHeaders,  HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { Customer } from '../model/customer';
import { map, catchError } from 'rxjs/operators';

@Injectable({
providedIn: 'root'
})

export class CustomerService {
  baseUrl: string = "http://localhost:5059/api";

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders().set("Authorization", `Bearer ${token}`);
  }

  getCustomers(): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.baseUrl}/customer`, {
      headers: this.getAuthHeaders()
    });
  }

  getCustomer(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/customer/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  getCustomerByEmail(email: string): Observable<{ exists: boolean }> {
    return this.http.get<{ exists: boolean }>(`${this.baseUrl}/public/email-exists/${email}`);
  }

  getCustomerDetails(email: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/customer/details/${email}`, {
      headers: this.getAuthHeaders()
    });
  }
  getCustomerOrders(email: string): Observable<any[]> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      'Authorization': token ?? '',  // This must match your backend auth middleware
      'Content-Type': 'application/json'
    });
    return this.http.get<any[]>(`${this.baseUrl}/orders/${email}`, {headers});
  }

  deleteCustomer(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/customer/${id}`, {
      headers: this.getAuthHeaders()
    });
  }

  updateCustomer(customer: Customer): Observable<any> {
    return this.http.put(`${this.baseUrl}/customer`, customer, {
      headers: this.getAuthHeaders()
    });
  }

  createCustomer(customerData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/customer`, customerData)
      .pipe(
        catchError(error => {
          if (error.status === 409) {
            return throwError(() => new Error('Email already exists'));
          }
          return throwError(() => error);
        })
      );
  }

// Public endpoint for email checking (no auth required)
checkEmailExists(email: string): Observable<boolean> {
  return this.http.get<{ exists: boolean }>(`${this.baseUrl}/public/email-exists/${email}`)
    .pipe(
      map(response => response.exists),
      catchError(error => throwError(() => error))
    );
}

}


