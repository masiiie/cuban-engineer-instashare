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

  login(username: string, password:string) {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      {username, password})
      .subscribe(response => {
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
