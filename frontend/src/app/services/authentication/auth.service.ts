import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { BehaviorSubject, Observable, of, switchMap, catchError } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

interface AuthResponse {
  token: string;
}

interface SignupResponse {
  success?: boolean;
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
  private readonly TOKEN_NAME = 'auth_token';

  constructor(
    private http: HttpClient, 
    private router: Router,
    private cookieService: CookieService
  ) {
    // Check if token exists in cookies
    const token = this.cookieService.get(this.TOKEN_NAME);
    if (token) {
      this.loggedIn.next(true);
    }
  }

  updateLoggedInState(state: boolean) {
    this.loggedIn.next(state);
  }

  signup(userData: { fullName: string, email: string, password: string }): Observable<SignupResponse> {
    return this.http.post(`${this.apiUrl}/register`, userData).pipe(
      switchMap(() => {
        // If registration is successful (200 status), proceed with login
        return this.http.post<AuthResponse>(`${this.apiUrl}/login`, {
          email: userData.email,
          password: userData.password
        }).pipe(
          switchMap(response => {
            this.cookieService.set(this.TOKEN_NAME, response.token, { path: '/' });
            this.loggedIn.next(true);
            return of({ success: true });
          })
        );
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 400) {
          const validationError = error.error as ValidationErrorResponse;
          let errorMessage = 'Validation error';
          
          if (validationError.errors) {
            const errorMessages = Object.entries(validationError.errors)
              .map(([_, messages]) => messages[0])
              .filter(message => message);
            
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

  login(email: string, password: string, twoFactorCode: string = '', twoFactorRecoveryCode: string = ''): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      {
        email,
        password,
        twoFactorCode,
        twoFactorRecoveryCode
      }
    );
  }

  logout() {
    this.cookieService.delete(this.TOKEN_NAME, '/');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}
