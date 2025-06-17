import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';  // Correct import
import { AppComponent } from './app/app.component';
import { authInterceptor } from './app/services/auth-interceptor.service';
import { provideRouter, RouterModule } from '@angular/router'; // Import RouterModule
import { Routes } from '@angular/router';
import { routes } from './app/app.routes';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';



bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    provideRouter(routes),
    NoopAnimationsModule, provideAnimationsAsync()
    
  ]
})
  .catch((err) => console.error(err));
