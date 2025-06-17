import { Routes } from '@angular/router';
import { EbookListComponent } from './ebook-list/ebook-list.component';
import { CategoryListComponent } from './category-list/category-list.component';
import { EditCustomerComponent } from './edit-customer/edit-customer.component';
import { CustomerComponent } from './customer/customer.component';
import { EbookComponent } from './ebook/ebook.component';
import { LoginComponent } from './login/login.component';
import { BuyEbookComponent } from './buy-ebook/buy-ebook.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { AppComponent } from './app.component';

export const routes: Routes = [


    { path: 'ebooks', component: EbookListComponent},
    { path: 'categories', component: CategoryListComponent},
    { path: 'ebook/:id', component: EbookComponent},
    { path: "profile/:email", component: CustomerComponent },
    { path: "profile/:id", component: CustomerComponent },
    { path: 'buy-ebook/:ebookid', component: BuyEbookComponent},
    { path: 'purchase/:ebookid', component: BuyEbookComponent},
    { path: 'order/:orderId', component: CustomerComponent },
    { path: 'order/:email', component: CustomerComponent },
    { path: 'download', component: CustomerComponent },
    { path: 'login', component: LoginComponent},
    { path: 'sign-up', component: SignUpComponent },
      { path: '', component: AppComponent },
    
];
