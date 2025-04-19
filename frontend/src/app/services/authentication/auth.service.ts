import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { BehaviorSubject } from 'rxjs';


interface AuthResponse {
  token: string;
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

  logout() {
    localStorage.removeItem('token');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn() {
    return this.loggedIn.asObservable();
  }
}
