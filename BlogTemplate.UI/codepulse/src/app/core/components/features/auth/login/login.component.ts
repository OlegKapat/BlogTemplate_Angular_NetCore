import { Component, OnInit } from '@angular/core';
import { LoginRequest } from '../models/login-request.model';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  model!: LoginRequest;
  constructor(
    private router: Router,
    private authService: AuthService,
    private cookieService: CookieService
  ) {
    this.model = {
      email: '',
      password: '',
    };
  }

  ngOnInit() {}

  onFormSubmit(): void {
    this.authService.login(this.model).subscribe({
      next: (response: any) => {
        // Set Auth Cookie
        this.cookieService.set(
          'Authorization',
          `Bearer ${response.token}`,
          undefined,
          '/',
          undefined,
          true,
          'Strict'
        );

        // Set User
        this.authService.setUser({
          email: response.email,
          roles: response.roles,
        });

        // Redirect back to Home
        this.router.navigateByUrl('/');
      },
    });
  }
}
