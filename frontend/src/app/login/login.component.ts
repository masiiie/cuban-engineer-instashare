import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/authentication/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NavigationComponent } from '../shared/navigation/navigation.component';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    FormsModule,
    NavigationComponent
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email?: string;
  password?: string;
  rememberMe: boolean = false;
  hidePassword: boolean = true;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cookieService: CookieService
  ) {}

  onSubmit() {
    if (!this.email || !this.password) {
      return;
    }
    
    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        const cookieOptions = this.rememberMe ? 
          { path: '/', expires: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000) } : // 30 days
          { path: '/' }; // Session cookie
        
        this.cookieService.set('auth_token', response.token, cookieOptions);
        this.authService.updateLoggedInState(true);
        this.router.navigate(['/']);
      },
      error: (error) => {
        console.error('Login failed:', error);
      }
    });
  }

  navigateToSignup() {
    this.router.navigate(['/signup']);
  }
}