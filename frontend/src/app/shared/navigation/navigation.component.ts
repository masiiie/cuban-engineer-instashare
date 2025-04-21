import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { Router } from '@angular/router';
import { AuthService } from '../../services/authentication/auth.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule
  ],
  template: `
    <mat-toolbar color="primary" class="nav-toolbar">
      <span class="logo" (click)="navigateHome()">InstaShare</span>
      
      <span class="spacer"></span>
      
      <ng-container *ngIf="!(authService.isLoggedIn() | async); else loggedIn">
        <button mat-button (click)="navigateToLogin()">
          <mat-icon>login</mat-icon>
          Log In
        </button>
        <button mat-raised-button color="accent" (click)="navigateToSignup()">Sign Up</button>
      </ng-container>
      
      <ng-template #loggedIn>
        <button mat-button>
          <mat-icon>upload_file</mat-icon>
          Upload Files
        </button>
        <button mat-button (click)="navigateToMyFiles()">
          <mat-icon>folder</mat-icon>
          My Files
        </button>
        <button mat-icon-button [matMenuTriggerFor]="userMenu">
          <mat-icon>account_circle</mat-icon>
        </button>
        <mat-menu #userMenu="matMenu">
          <button mat-menu-item>
            <mat-icon>person</mat-icon>
            <span>Profile</span>
          </button>
          <button mat-menu-item>
            <mat-icon>settings</mat-icon>
            <span>Settings</span>
          </button>
          <mat-divider></mat-divider>
          <button mat-menu-item (click)="authService.logout()">
            <mat-icon>logout</mat-icon>
            <span>Logout</span>
          </button>
        </mat-menu>
      </ng-template>
    </mat-toolbar>
  `,
  styles: [`
    .nav-toolbar {
      position: fixed;
      top: 0;
      left: 0;
      right: 0;
      z-index: 1000;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }
    
    .logo {
      font-size: 1.5rem;
      font-weight: 500;
      cursor: pointer;
    }
    
    .spacer {
      flex: 1 1 auto;
    }
    
    button {
      margin-left: 8px;
    }
    
    mat-icon {
      margin-right: 4px;
    }
  `]
})
export class NavigationComponent {
  constructor(
    private router: Router,
    public authService: AuthService
  ) {}

  navigateHome() {
    this.router.navigate(['/']);
  }

  navigateToLogin() {
    this.router.navigate(['/login']);
  }

  navigateToSignup() {
    this.router.navigate(['/signup']);
  }

  navigateToMyFiles() {
    this.router.navigate(['/files']);
  }
}