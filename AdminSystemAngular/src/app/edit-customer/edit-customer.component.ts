import { Component, Input, OnInit } from '@angular/core';
import { CustomerService } from '../services/customer.service';
import { Customer } from '../model/customer';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-customer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-customer.component.html',
  styleUrl: './edit-customer.component.css'
})
export class EditCustomerComponent implements OnInit{

  @Input() customerid!: number;
  customer!: Customer;

  constructor(private customerService: CustomerService, private router: Router){}

  ngOnInit(){
    this.customerService.getCustomer(this.customerid!).subscribe(customer => {
      this.customer = customer;
    })
  }

  updateCustomer(){
    this.customerService.updateCustomer(this.customer).subscribe(() => {
      this.router.navigate(["/profile"]);
    });
  }
}
