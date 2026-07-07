import { Component, inject, signal } from '@angular/core';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FormHeaderComponent } from '../../../shared/components/form-header/form-header.component';
import { Router, RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-forget-password',
  imports: [FooterComponent, HeaderComponent, FormHeaderComponent, RouterLink, ReactiveFormsModule],
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.css',
})
export class ForgetPasswordComponent {
  form = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
  });
  forgetPasswrodMsg = signal<string | null>(null);
  authService = inject(AuthService);
  router = inject(Router);
  onSubmitEmail() {
    if (this.form.valid) {
      this.authService.forgetPassword(this.form.get('email')?.value!).subscribe({
        next: () => {
          this.router.navigate(['/check-email'], {
            state: {
              email: this.form.get('email')?.value,
            },
          });
        },
        error: (err: any) => {
          console.log(err);
          this.forgetPasswrodMsg.set(err.message);
        },
      });
    }
  }
}
