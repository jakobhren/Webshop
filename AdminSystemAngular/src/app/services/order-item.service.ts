import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderItem } from '../model/order-item';  // Assuming you have an OrderItem model

@Injectable({
  providedIn: 'root'
})
export class OrderItemService {
  baseUrl: string = "http://localhost:5059/api";

  constructor(private http: HttpClient) {}

  // Get all order items
  getOrderItems(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.baseUrl}/orderitem`);
  }

  // Get a specific order item by ID
  getOrderItem(id: number): Observable<OrderItem> {
    return this.http.get<OrderItem>(`${this.baseUrl}/orderitem/${id}`);
  }

  // Create a new order item
  createOrderItem(orderItem: OrderItem): Observable<any> {
    return this.http.post(`${this.baseUrl}/orderitem`, orderItem);
  }

  // Delete an order item by ID
  deleteOrderItem(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/orderitem/${id}`);
  }
}
