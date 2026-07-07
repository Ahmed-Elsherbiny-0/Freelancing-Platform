import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-verify-email',
  imports: [RouterLink, RouterLink],
  templateUrl: './verify-email.component.html',
  styleUrl: './verify-email.component.css',
})
export class VerifyEmailComponent {
  status = signal<'loading' | 'success' | 'error'>('loading');
  private authService = inject(AuthService);
  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    const code = this.route.snapshot.queryParamMap.get('code');
    const userId = this.route.snapshot.queryParamMap.get('userId');
    if (code && userId) {
      this.authService.confirmEmail(code, userId).subscribe({
        next: () => this.status.set('success'),
        error: () => this.status.set('error'),
      });
    } else {
      console.log(code, userId);
      this.status.set('error');
    }
  }
}
