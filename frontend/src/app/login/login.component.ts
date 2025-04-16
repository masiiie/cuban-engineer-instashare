import { Component } from '@angular/core';
import { AuthService } from '../services/authentication/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  username?: string;
  password?: string;

  constructor(private authService: AuthService) {}

  onSubmit() {
    //this.authService.login(this.username, this.password);
  }
}