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
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
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
    MatSnackBarModule,
    MatProgressSpinnerModule,
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
  isLoading: boolean = false;
  hidePassword: boolean = true;
  hideConfirmPassword: boolean = true;
  serverError: string = '';

  constructor(
    private router: Router, 
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {}

  onSubmit() {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    this.serverError = '';
    
    this.authService.signup({
      fullName: this.fullName,
      email: this.email,
      password: this.password
    }).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.error) {
          this.serverError = response.error;
        } else {
          this.snackBar.open('Account created successfully!', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
          });
          this.router.navigate(['/']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.serverError = 'An unexpected error occurred. Please try again.';
        console.error('Signup error:', error);
      }
    });
  }

  private validateForm(): boolean {
    if (!this.fullName || !this.email || !this.password || !this.confirmPassword) {
      this.snackBar.open('Please fill in all required fields', 'Close', {
        duration: 3000
      });
      return false;
    }

    if (this.password !== this.confirmPassword) {
      this.snackBar.open('Passwords do not match', 'Close', {
        duration: 3000
      });
      return false;
    }

    if (!this.agreeToTerms) {
      this.snackBar.open('Please agree to Terms and Conditions', 'Close', {
        duration: 3000
      });
      return false;
    }

    return true;
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}