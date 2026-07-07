import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserInfoRequestDto } from '../../../shared/models/user';
import { UserService } from '../../../core/services/user.service';
import { ToastService } from '../../../core/services/toast.service';
import { Router } from '@angular/router';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';

const COUNTRIES = [
  'Egypt',
  'United States',
  'United Kingdom',
  'Germany',
  'France',
  'Spain',
  'Italy',
  'Netherlands',
  'Sweden',
  'Poland',
  'Turkey',
  'United Arab Emirates',
  'Saudi Arabia',
  'Canada',
  'Australia',
  'India',
  'Pakistan',
  'Brazil',
  'Mexico',
  'Japan',
  'South Korea',
  'China',
  'Nigeria',
  'South Africa',
  'Other',
];

@Component({
  selector: 'app-update-account',
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './update-account.component.html',
  styleUrl: './update-account.component.css',
})
export class UpdateAccountComponent implements OnInit {
  private fb = inject(FormBuilder);
  userService = inject(UserService);
  toastService = inject(ToastService);
  router = inject(Router);

  /** Pass the user's current info in to prefill the form (e.g. on a profile/edit page). */
  initialData: UserInfoRequestDto | null = null;

  countries = COUNTRIES;
  skillInput = '';

  form = this.fb.group({
    firstName: [
      this.initialData?.firstName,
      [Validators.required, Validators.minLength(2), Validators.maxLength(50)],
    ],
    lastName: [
      this.initialData?.lastName,
      [Validators.required, Validators.minLength(2), Validators.maxLength(50)],
    ],
    country: [this.initialData?.country, [Validators.required]],
    skills: this.fb.array<FormControl<string>>([]),
    description: [
      this.initialData?.description,
      [Validators.required, Validators.minLength(10), Validators.maxLength(500)],
    ],
  });

  ngOnInit(): void {
    this.userService.getUserInfo().subscribe((x) => {
      console.log(x);
      this.initialData = x;
      this.resetToInitial();
    });
  }

  private resetToInitial(): void {
    this.form.patchValue({
      firstName: this.initialData?.firstName ?? '',
      lastName: this.initialData?.lastName ?? '',
      country: this.initialData?.country ?? '',
      description: this.initialData?.description ?? '',
    });

    this.skills.clear();
    (this.initialData?.skills ?? []).forEach((skill) =>
      this.skills.push(this.fb.nonNullable.control(skill)),
    );
  }

  get skills() {
    return this.form.get('skills') as FormArray<FormControl<string>>;
  }

  get f() {
    return this.form.controls;
  }

  addSkill(): void {
    const value = this.skillInput.trim();
    if (!value) return;

    const exists = this.skills.value.some((s) => s.toLowerCase() === value.toLowerCase());
    if (!exists) {
      this.skills.push(this.fb.nonNullable.control(value));
    }
    this.skillInput = '';
  }

  onSkillKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter' || event.key === ',') {
      event.preventDefault();
      this.addSkill();
    }
  }

  removeSkill(index: number): void {
    this.skills.removeAt(index);
  }

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();
    const dto: UserInfoRequestDto = {
      firstName: raw.firstName!.trim(),
      lastName: raw.lastName!.trim(),
      country: raw.country!,
      skills: raw.skills.length ? raw.skills : null,
      description: raw.description!.trim(),
    };

    this.userService.updateUserInfo(dto).subscribe((x) => {
      this.toastService.success('Updated Successfully');
      window.location.reload();
    });
  }

  onCancel(): void {
    if (this.initialData) {
      this.resetToInitial();
    } else {
      this.form.reset();
      this.skills.clear();
    }
    this.skillInput = '';
    this.router.navigateByUrl('/home');
  }
}
