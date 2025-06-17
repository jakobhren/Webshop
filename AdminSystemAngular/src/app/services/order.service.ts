import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../model/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseUrl: string = "http://localhost:5059/api";

  constructor(private http: HttpClient) {}

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/order`);
  }
  
  getOrder(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}/order/${id}`);
  }
  
  createOrder(order: Order): Observable<any> {
    return this.http.post(`${this.baseUrl}/orders`, order);
  }
  
  deleteOrder(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/order/${id}`);
  }
  getCustomerOrders(email: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/customer/${email}`);  // Adjust API endpoint as needed
  }
}

