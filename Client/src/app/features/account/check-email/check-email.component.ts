import { Component, inject, Input, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';

@Component({
  selector: 'app-check-email',
  imports: [HeaderComponent, FooterComponent],
  templateUrl: './check-email.component.html',
  styleUrl: './check-email.component.css',
})
export class CheckEmailComponent {
  userEmail = signal<string | null>(null);
  constructor() {
    const nav = this.router.getCurrentNavigation();
    this.userEmail.set(nav?.extras?.state?.['email'] ?? null);
  }

  router = inject(Router);

  backToLogin(): void {
    this.router.navigate(['/login']);
  }

  resendEmail(): void {
    this.router.navigateByUrl('/resend-confirm-email');
  }
}
