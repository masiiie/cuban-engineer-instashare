import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', loadComponent: () => import('./welcome/welcome.component').then(m => m.WelcomeComponent) },
    { path: 'login', loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
    { path: 'signup', loadComponent: () => import('./signup/signup.component').then(m => m.SignupComponent) }
];
