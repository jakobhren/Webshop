
import { Component, OnInit } from '@angular/core';
import { Order } from '../model/order';
import { OrderComponent } from '../order/order.component';
import { OrderService } from '../services/order.service';
import { HttpClientModule } from '@angular/common/http';


@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [OrderComponent, HttpClientModule],
  providers: [OrderService],
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: Order[] = [];  

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.orderService.getOrders().subscribe(orders => {
      this.orders = orders;
    });
  }
}