import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { BehaviorSubject, Observable, of, tap, map, catchError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

interface AuthResponse {
  token: string;
}

interface SignupResponse {
  token?: string;
  error?: string;
}

interface ValidationErrorResponse {
  type: string;
  title: string;
  status: number;
  errors: {
    [key: string]: string[];
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.instaShareApiUrl;
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient, private router: Router) { }

  login(email: string, password: string, twoFactorCode: string = '', twoFactorRecoveryCode: string = '') {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      {
        email, // Map email to the backend field
        password, // Map password to the backend field
        twoFactorCode, // Optional field for two-factor authentication
        twoFactorRecoveryCode // Optional field for recovery code
      }
    ).subscribe(response => {
      localStorage.setItem('token', response.token);
      this.loggedIn.next(true);
      this.router.navigate(['/']);
    });
  }

  signup(userData: { fullName: string, email: string, password: string }): Observable<SignupResponse> {
    return this.http.post<SignupResponse>(
      `${this.apiUrl}/register`,
      {
        email: userData.email,
        password: userData.password,
        fullName: userData.fullName
      }
    ).pipe(
      map(response => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          this.loggedIn.next(true);
          this.router.navigate(['/']);
        }
        return response;
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          const validationError = error.error as ValidationErrorResponse;
          let errorMessage = 'Validation error';
          
          if (validationError.errors) {
            // Collect all error messages from all error fields
            const errorMessages = Object.entries(validationError.errors)
              .map(([_, messages]) => messages[0]) // Take first message from each error type
              .filter(message => message); // Remove any undefined/empty messages
            
            if (errorMessages.length > 0) {
              errorMessage = errorMessages.join('\n');
            }
          }
          return of({ error: errorMessage });
        }
        return of({ error: 'An unexpected error occurred. Please try again.' });
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}
