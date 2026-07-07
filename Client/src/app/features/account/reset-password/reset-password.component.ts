import { ChangeDetectorRef, Component, inject, signal } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastService } from '../../../core/services/toast.service';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { passwordRegex } from '../../../shared/constants/Regex';

@Component({
  selector: 'app-reset-password',
  imports: [HeaderComponent, FooterComponent, ReactiveFormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
  status = signal<'idle' | 'success' | 'error'>('idle');
  private authService = inject(AuthService);
  form = new FormGroup({
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(7),
      Validators.pattern(passwordRegex),
    ]),
    confirmPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(7),
      Validators.pattern(passwordRegex),
    ]),
  });
  email: string | null = null;
  code: string | null = null;
  constructor(
    private route: ActivatedRoute,
    private toast: ToastService,
  ) {}

  ngOnInit() {
    this.code = this.route.snapshot.queryParamMap.get('code');
    this.email = this.route.snapshot.queryParamMap.get('email');
  }

  onResetPassword() {
    if (this.code && this.email && this.form.valid) {
      this.authService
        .resetPassword(this.code, this.email, this.form.controls.password.value!)
        .subscribe({
          next: () => {
            this.status.set('success');
            this.toast.success('Password Changed Successfully');
          },
          error: () => {
            this.status.set('error');
            this.toast.error('invalid code or email');
          },
        });
    } else {
      console.log(this.code, this.email);
      this.status.set('error');
    }
  }
}
