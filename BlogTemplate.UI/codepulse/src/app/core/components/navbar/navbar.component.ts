import { Component } from '@angular/core';
import { User } from '../features/auth/models/user.model';
import { Router } from '@angular/router';
import { AuthService } from '../features/auth/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  user?: User;
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.authService.user()
    .subscribe({
      next: (response) => {
        this.user = response;
      }
    });

    this.user = this.authService.getUser();
  }
  onLogout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
