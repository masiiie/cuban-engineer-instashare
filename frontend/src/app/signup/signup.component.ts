import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AuthService } from '../services/authentication/auth.service';
import { NavigationComponent } from '../shared/navigation/navigation.component';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    NavigationComponent
  ],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  fullName: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  agreeToTerms: boolean = false;

  constructor(private router: Router, private authService: AuthService) {}

  onSubmit() {
    if (!this.validateForm()) {
      return;
    }

    this.authService.signup({
      fullName: this.fullName,
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response) => {
        if (response.success) {
          // Automatically log in the user after successful signup
          this.authService.login(this.email, this.password);
        }
      },
      error: (error) => {
        console.error('Signup failed:', error);
        // TODO: Show error message to user
      }
    });
  }

  private validateForm(): boolean {
    if (!this.fullName || !this.email || !this.password || !this.confirmPassword) {
      // TODO: Show error message for required fields
      return false;
    }

    if (this.password !== this.confirmPassword) {
      // TODO: Show password mismatch error
      return false;
    }

    if (!this.agreeToTerms) {
      // TODO: Show terms agreement error
      return false;
    }

    return true;
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}