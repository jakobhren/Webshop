import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EbookService } from '../services/ebook.service';
import { OrderService } from '../services/order.service';
import { OrderItemService } from '../services/order-item.service';
import { Order } from '../model/order';
import { OrderItem } from '../model/order-item';
import { CommonModule } from '@angular/common';
import { Injectable } from '@angular/core';
import {HttpInterceptor, HttpRequest, HttpHandler,HttpEvent} from '@angular/common/http';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-buy-ebook',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './buy-ebook.component.html',
  styleUrl: './buy-ebook.component.css'
})
export class BuyEbookComponent implements OnInit{
  email: string = '';
  ebookId: number = 0;
  orderId: number = 0;
  orderItemId: number =0;
  purchaseSuccess = false;
  ebook: { 
    ebookId: number,
    price: number, 
    title: string,  
    author: string, 
    publicationYear: number;
    file_url: string;
    image_url: string;
    categoryId: number; } = { ebookId: 0, price: 0, title: '',author: '', publicationYear: 0, file_url: '', image_url: '', categoryId: 0};

  constructor(
    private route: ActivatedRoute,
    private ebookService: EbookService,
    private orderService: OrderService,
    private orderItemService: OrderItemService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.ebookId = +(this.route.snapshot.paramMap.get('ebookid') ?? 0);
    this.ebookService.getEbook(this.ebookId).subscribe((data) => {
      this.ebook = data;
    });
  }

  purchase() {

    // make order component!!
    const email = localStorage.getItem('email')  || '';

    if (!email) {
      alert('You must be logged in to make a purchase!');
      this.router.navigate(['/login']); // Redirect to login if email is not found
      return;
    }

    if (this.ebook && this.ebook.price > 0) {

      const orderItem: OrderItem = {
        orderItemId: 0, // this will be ignored by backend
        orderId: 0,     // will be set by backend after order is created
        ebookId: this.ebookId
      };
  
      const order: Order = {
        orderDate: new Date(),
        orderTotal: this.ebook.price,
        orderId: 0,
        customeremail: email, // Replace with real customer ID or email logic if needed
        orderItems: [orderItem] 
      };
  
      this.orderService.createOrder(order).subscribe({
        next: (orderId) => {
          const email = localStorage.getItem('email');
          this.purchaseSuccess = true;
          console.log('Order and OrderItem created with order ID:', orderId);

          this.router.navigate([`/profile/${email}`]);
        },
        error: (err) => {
          console.error('Order creation failed:', err);
        }
      });
    }


    /*
    if (this.ebook && this.ebook.price > 0) {
    
      // Create an order object with relevant fields 
      const order: Order = {
        orderDate: new Date(),
        orderTotal: this.ebook.price,
        orderId: 0,
        customeremail: '', 
        
      };
    
      // Call the service to create the order in the Order table
      this.orderService.createOrder(order).subscribe((createdOrder) => {
        // Once the order is created, use the created order ID to create an order item
        if (createdOrder && createdOrder.orderId > 0) {
        const orderItem: OrderItem = {
          orderItemId: 0, 
          orderId: createdOrder.orderId,
          ebookId: this.ebookId,
          
        };
        
        this.orderItemService.createOrderItem(orderItem).subscribe(() => {
          this.purchaseSuccess = true;
          console.log('Purchase successful');
        }, error => {
          console.error('Failed to create order item:', error);
        });
      }
    });
  }
  }
  */
  }}