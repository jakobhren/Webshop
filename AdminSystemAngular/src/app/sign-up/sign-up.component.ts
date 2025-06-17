import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Customer } from '../model/customer';
import { CustomerService } from '../services/customer.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { HttpHeaders } from '@angular/common/http';


@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent {

  constructor(
    private http: HttpClient, 
    private router: Router, 
    private customerService: CustomerService) {}

  //Form Controls
  name: FormControl = new FormControl('', [Validators.required]);
  email: FormControl = new FormControl('', [Validators.required, Validators.email]);
  password: FormControl = new FormControl('', [Validators.required, Validators.minLength(4)]);
  
  customerForm: FormGroup = new FormGroup({
  name: this.name,
  email: this.email,
  password: this.password
});

  signup ()
  {
    if (!this.customerForm.valid) {
        console.log('Data is not valid');
        return;
      }
    

          // Add Authorization header for authenticated call
        const headers = new HttpHeaders({
          'Authorization': 'Basic ' + btoa(`${this.email.value}:${this.password.value}`)
        });

          // Get the customer by email after creation
        this.customerService.getCustomerByEmail(this.email.value!).subscribe({
          next: (response) => {
            if (response.exists) {
              console.error('Email is already in use.');
              return;
          }

          // Proceed with signup
          this.customerService.createCustomer({
            name: this.name.value!,
            email: this.email.value!,
            password: this.password.value!
          }).subscribe({
            next: () => {
              this.router.navigate([`/profile/${this.email.value!}`]);
          },
            error: (err) => {
              console.error('Error creating customer', err);
            }
          });
        },
        error: (err) => {
          console.error('Error checkin email:', err);
        }
      });
    }
  }

