import { Component, inject, signal } from '@angular/core';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { FormHeaderComponent } from '../../../shared/components/form-header/form-header.component';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-resend-confirm-email',
  imports: [FooterComponent, FormHeaderComponent, HeaderComponent, ReactiveFormsModule, RouterLink],
  templateUrl: './resend-confirm-email.component.html',
  styleUrl: './resend-confirm-email.component.css',
})
export class ResendConfirmEmailComponent {
  form = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
  });
  confirmEmailMsg = signal<string | null>(null);
  authService = inject(AuthService);
  toast = inject(ToastService);
  router = inject(Router);
  onSubmitEmail() {
    if (this.form.valid && this.form.controls.email.value) {
      this.authService.resendConfirmationEamil(this.form.get('email')?.value!).subscribe({
        next: () => {
          this.toast.success('REsend email Successfully');
        },
        error: () => {
          this.toast.error('your email is already Confirmed');
        },
      });
    }
  }
}
