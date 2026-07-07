import { Component, inject, signal } from '@angular/core';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import {
  ReactiveFormsModule,
  FormGroup,
  FormControl,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { FormHeaderComponent } from '../../../shared/components/form-header/form-header.component';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { ToastService } from '../../../core/services/toast.service';
@Component({
  selector: 'app-login',
  imports: [HeaderComponent, FooterComponent, ReactiveFormsModule, FormHeaderComponent, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  authService = inject(AuthService);
  router = inject(Router);
  toast = inject(ToastService);
  loginErrorMsg = signal<string | null>(null);
  form = new FormGroup({
    email: new FormControl('', [Validators.required, validEmailOrPhone]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  });

  loginRequest() {
    let email = this.form.get('email')?.value;
    let password = this.form.get('password')?.value;
    if (this.form.valid && email && password) {
      this.authService.login(email, password).subscribe({
        next: () => {
          this.router.navigateByUrl('/');
          this.toast.success('Login Successifully');
        },
        error: (err: any) => {
          this.loginErrorMsg.set((err.error.errors as string[]).join(' , '));
        },
      });
    }
  }
}

function validEmailOrPhone(control: AbstractControl) {
  let isPhone = control.value.length >= 9;
  for (const ch of control.value) {
    if (ch >= '0' && ch <= '9') continue;
    else {
      isPhone = false;
      break;
    }
  }

  if ((control.value.includes('.com') && control.value.includes('@')) || isPhone) {
    return null;
  }
  return { valid: false };
}
