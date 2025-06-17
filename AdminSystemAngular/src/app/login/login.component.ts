import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { CustomerService } from '../services/customer.service';
import { Customer } from '../model/customer';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  loading = false;

  constructor(
    public auth: AuthService, 
    private router: Router,
    private customerService: CustomerService) {}


  login(): void {
    if (!this.email || !this.password) {
      this.errorMessage = 'Email and password are required.';
      return;
    }

    this.loading = true;

    this.auth.authenticate(this.email, this.password).subscribe({
      next: (auth) => {
        const token = auth?.headerValue?.replace(/^const token = auth?.headerValue;\s*/, '');

        if (!token) {
          this.showError('Invalid login response.');
          return;
        }

        localStorage.setItem('token', 'Basic ' + btoa(this.email + ':' + this.password));

        this.customerService.getCustomerDetails(this.email).subscribe({
          next: (customer) => {
            this.router.navigate([`/profile/${customer.email}`]);
          },
          error: () => this.showError('Failed to load profile.')
        });
      },
      error: () => this.showError('Invalid email or password.')
    });
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.loading = false;
  }
}

      /*
      if (this.email != null && this.password != null) {
        this.auth.authenticate(this.email, this.password).subscribe({
          next: (auth) => {
            if (auth != null) {
              localStorage.setItem('headerValue', auth.headerValue);

              // Build header with auth for subsequent calls
              const headers = new HttpHeaders({
              'Authorization': auth.headerValue
              });

              // Get the customer by email after successful authentication
              this.customerService.getCustomerByEmail(this.email).subscribe({
                next: (response) => {
                  if (response.exists) {
                      // Email exists, proceed to retrieve full customer details
                    this.getCustomerDetails();
                  } else {
                     // Email doesn't exist
                      this.errorMessage = 'Email does not exist.';
                      console.error('Email not found');
                  }
                },
                error: (err) => {
                  console.error('Error finding email:', err);
                  this.errorMessage = 'Error checking email.';
                }
              });
            }else{
              this.errorMessage = 'Invalid login response.';
              console.error('Missing authentication header value.');
          }
        },
        error: (err) => {
            console.error('Authentication failed:', err);
          }
        });
      }
    }
    getCustomerDetails() {
      this.customerService.getCustomerDetails(this.email).subscribe({
        next: (customer: Customer) => {
          console.log('Customer retrieved:', customer);
          this.router.navigate([`/profile/${customer.email}`]); // Navigate to profile
        },
        error: (err) => {
          console.error('Error retrieving customer details:', err);
          this.errorMessage = 'Error retrieving customer details.';
        }
      });
    }
}
*/
  /*  
  login(): void {
    if (this.email && this.password) 
    {
      // send credentials to backend
      this.auth.authenticate(this.email, this.password).subscribe(
        (auth) => {
          if (auth && auth.headerValue) 
          {
            //store authentification token in local storage
            localStorage.setItem('authToken', auth.headerValue);
            const customerId = auth.customer.customerId;
            localStorage.setItem('customerid', customerId.toString());

            //navigate to profile
            this.authenticated = true;
            this.router.navigate(['/profile', customerId]);
          } else{
            console.error('Missing authentication data in response.');
          }      
        },
        (error) => {
          console.error('Authentication error', error);
        }
      );
    }
  }
}
*/
/*
// After a successful login, store the token (e.g., JWT)
localStorage.setItem('authToken', auth.token);

// Send the token with the header in future requests
const token = localStorage.getItem('authToken');
const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

*/


/*{
  if (this.email != null && this.password != null) {
    this.auth.authenticate(this.email, this.password).subscribe((auth) => {
      if (auth != null) {
        // Save to the local storage
        localStorage.setItem('headerValue', auth.headerValue);
        this.authenticated = true;
        this.router.navigate(['profile', auth.customer.id]);
        }
      });
    }
  }

*/
