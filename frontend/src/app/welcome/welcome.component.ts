import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { NavigationComponent } from '../shared/navigation/navigation.component';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    NavigationComponent
  ],
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent {
  constructor(private router: Router) {}

  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  navigateToSignup() {
    this.router.navigate(['/signup']);
  }
}