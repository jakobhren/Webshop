import { Component, Input } from '@angular/core';
import { OrderItem } from '../model/order-item';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-order-item',
  standalone: true,
  imports: [],
  templateUrl: './order-item.component.html',
  styleUrl: './order-item.component.css'
})
export class OrderItemComponent {

  constructor(private orderService: OrderService) {}

  mode = 0;
      @Input() orderItem?: OrderItem = {

    orderItemId: 1,
    orderId: 1,
    ebookId: 1,
  }
  
}
