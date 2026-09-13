import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { FormHeaderComponent } from '../../../shared/components/form-header/form-header.component';
import { Router, RouterLink } from '@angular/router';
import { StepperModule } from 'primeng/stepper';
import { ButtonModule } from 'primeng/button';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { passwordRegex } from '../../../shared/constants/Regex';
import { AuthService } from '../../../core/services/auth.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { FileUploadEvent, FileUploadModule } from 'primeng/fileupload';
import { Photo } from '../../../shared/models/photo';

@Component({
  selector: 'app-register',
  imports: [
    HeaderComponent,
    FooterComponent,
    FormHeaderComponent,
    RouterLink,
    StepperModule,
    ButtonModule,
    ReactiveFormsModule,
    FileUploadModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  form1 = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.pattern(passwordRegex), Validators.required]),
    firstName: new FormControl('', [Validators.minLength(2), Validators.required]),
    lastName: new FormControl('', [Validators.minLength(2), Validators.required]),
  });
  form2 = new FormGroup({
    country: new FormControl('', [Validators.required]),
    gender: new FormControl('', [Validators.required]),
    phoneNumber: new FormControl('', [Validators.required, Validators.min(8)]),
    checkbox: new FormControl(false, [Validators.requiredTrue]),
  });
  private snackbar = inject(SnackbarService);
  private authService = inject(AuthService);
  private destoryRef = inject(DestroyRef);
  private router = inject(Router);
  userPostion = signal<string | null>(null);
  registerErrorMsg = signal<string | null>(null);
  userPhoto = signal<Photo | null>(null);
  countries: string[] = ['Egypt', 'Suddan', 'Syria'];
  private httpClient = inject(HttpClient);
  ngOnInit() {
    var sub = this.getCountries();
    this.destoryRef.onDestroy(() => sub.unsubscribe());
  }

  private getCountries() {
    return this.httpClient
      .get<any>(
        'https://api.restcountries.com/countries/v5?response_fields=names.common%2Cflag.emoji&pretty=1',
        {
          headers: {},
        },
      )
      .subscribe((x) => {
        x.data.objects.forEach((y: { names: { common: string } }) => {
          this.countries.push(y.names.common);
        });
      });
  }

  onSelectPosition(postion: string) {
    this.userPostion.set(postion);
  }

  onCreateAccount() {
    let payload: any = {
      email: this.form1.get('email')?.value,
      password: this.form1.get('password')?.value,
      firstName: this.form1.get('firstName')?.value,
      lastName: this.form1.get('lastName')?.value,
      phoneNumber: this.form2.get('phoneNumber')?.value,
      country: this.form2.get('country')?.value,
      type: this.userPostion(),
    };

    if (this.userPhoto()) {
      payload.photo = {
        PublicId: this.userPhoto()?.publicId,
        url: this.userPhoto()?.url,
      };
    }

    if (this.form1.valid && this.form2.valid) {
      this.authService.register(payload).subscribe({
        next: () => {
          this.router.navigate(['check-email'], {
            state: {
              email: this.form1.value.email,
            },
          });
          this.snackbar.success('plz check Your email');
          this.registerErrorMsg.set(null);
          this.authService.logout();
        },
        error: (err: any) => {
          this.registerErrorMsg.set((err.error.errors as string[]).join(' , '));
        },
      });
      return;
    }
    this.form1.markAsTouched();
    this.form1.markAllAsTouched();

    this.form2.markAsTouched();
    this.form2.markAllAsTouched();
  }

  onUploadImage(event: any) {
    this.userPhoto.set(event.originalEvent.body);
  }
  removeUserPhoto() {
    if (this.userPhoto()) {
      this.httpClient
        .delete('https://localhost:7148/users/delete-photo-cloudinary', {
          params: { publicId: this.userPhoto()!.publicId },
        })
        .subscribe();
    }
    this.userPhoto.set(null);
  }
}
