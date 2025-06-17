import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CustomerComponent } from './customer/customer.component';
import { EbookComponent } from './ebook/ebook.component';
import { EbookListComponent } from "./ebook-list/ebook-list.component";
import { OrderComponent } from './order/order.component';
import { OrderItemComponent } from './order-item/order-item.component';
import { CategoryComponent } from './category/category.component';
import { CategoryListComponent } from "./category-list/category-list.component";
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { OrderListComponent } from './order-list/order-list.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root', 
  standalone: true,
  imports: [
    RouterModule,
    MatMenuModule, 
    MatToolbarModule, 
    MatButtonModule, 
    MatIconModule, 
    RouterLink, 
    RouterOutlet, 
    CustomerComponent, 
    EbookComponent, 
    EbookListComponent, 
    OrderComponent, 
    OrderItemComponent, 
    CategoryComponent, 
    CategoryListComponent, 
    OrderListComponent,
  ],
  templateUrl: './app.component.html', 
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{

  customerId: number | null = null;
  title = 'AdminSystemAngular';

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    // Check if the user is authenticated
    if (this.authService.isAuthenticated()) {
      const storedId = localStorage.getItem('customerId');
      this.customerId = storedId ? parseInt(storedId, 10) : null;
    }
  }

}


