import { Component, Input } from '@angular/core';
import { Order } from '../model/order';
import { OrderService } from '../services/order.service';
import { OrderItem } from '../model/order-item';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent {

  constructor(private orderService: OrderService) {}

  mode = 0;
      @Input() order?: Order;
     /*
    orderId: 1,
    customerId: 1,
    orderDate: new Date (2000,2,2),
    orderTotal: 3.4,
    */
  
  deleteOrder(): void {
    this.orderService.deleteOrder(this.order!.orderId).subscribe();
    }
}

