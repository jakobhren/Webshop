import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, pipe, tap } from 'rxjs';
import { Login } from '../model/login';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
    baseUrl: string =  "http://localhost:5059/api/login";
    
    constructor(private http: HttpClient) {}

    authenticate(email: string, password: string): Observable<any> {
       // Create the Basic Auth header
      const encodedCredentials = btoa(`${email}:${password}`);
      const headers = new HttpHeaders({
        Authorization: `Basic ${encodedCredentials}`,
        'Content-Type': 'application/json'
      });
        
      //const credentials = {email, password}
      /*const headers = new HttpHeaders({
        Authorization: `Basic ${credentials}`,
        'Content-Type': 'application/json'
      });
      */
      // Store the credentials for future requests on successful login
      return this.http.post<any>(this.baseUrl, {}, { headers }).pipe(
        tap(response => {
          // Store the encoded credentials for future authenticated requests
          localStorage.setItem('authToken', response.headerValue);
        }),
        catchError(error => {
          console.error('Login error:', error);
          return of(null);
        })
      );
    }

    //Get stored auth token
    getAuthToken(): string | null {
      return localStorage.getItem('authToken');
    }
   
    // This method adds the header for authenticated requests
    getAuthenticatedHeaders(): HttpHeaders {
      const authToken = this.getAuthToken();
      if (!authToken) {
        return new HttpHeaders({
          'Content-Type': 'application/json'
        });
    }  return new HttpHeaders({
      'Authorization': `Basic ${authToken}`,
      'Content-Type': 'application/json'
      });
    }
    // Check if user is authenticated
    isAuthenticated(): boolean {
      return !!this.getAuthToken();
    }
    // Logout user
    logout(): void {
      localStorage.removeItem('authToken');
    }

    // to get user profile after login: 
    getCustomerProfile(): Observable<any> {
      const headers = this.getAuthenticatedHeaders();
      return this.http.get<any>('http://localhost:5059/api/profile', { headers });
    }
  
    

    /*
    authenticate(email: String, password: String): Observable<Login> {
      return this.http.post<Login>(`${this.baseUrl}/login`, {
        email: email,
        password: password
      }).pipe(
        tap(auth => this.authHeader = auth.headerValue)
      );
      */

    /*
  }
  isAuthenticated(): boolean {
    return !!localStorage.getItem('headerValue');  // Adjust this key based on your login logic
  }

  getCustomerId(): number | null {
    return Number(localStorage.getItem('customerId')) || null; 
}
    */
}