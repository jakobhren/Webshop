import { Component, Input, OnInit } from '@angular/core';
import { Customer } from '../model/customer';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { CustomerService } from '../services/customer.service';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  providers: [CustomerService],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})

export class CustomerComponent implements OnInit{
   
  customer: any;
  email: string = ''; 
  orders: any[] = [];
 
  constructor(
    private customerService: CustomerService, 
    private router: Router,
    private auth: AuthService,
    private route: ActivatedRoute) {}

    ngOnInit(): void {

      const param = this.email = this.route.snapshot.paramMap.get('email')!;
      if (!param) {
        alert("Invalid customer email");
        return;
      }
      this.email = param;
      this.customerService.getCustomerDetails(this.email).subscribe({
        next: (customer) => this.customer = customer,
        error: (err) => alert("Customer not found.")
      });

      // Call the API to get the customer's orders
      this.customerService.getCustomerOrders(this.email).subscribe({
      next: (orders) => {
        this.orders = orders;  // Assuming the API response contains an array of orders
      },
      error: (err) => {
        console.error('Error retrieving orders:', err);
      }
    });
  }

      
  editCustomer(email: string) {
    this.router.navigate(["edit-customer", email]);
  }

  deleteCustomer(): void {
    this.customerService.deleteCustomer(this.customer.email).subscribe();
  }

 

}
