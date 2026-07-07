import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { JobRequestDto } from '../../../shared/models/job';
import { JobService } from '../../../core/services/job.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-add-job',
  imports: [CommonModule, ReactiveFormsModule, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './add-job.component.html',
  styleUrl: './add-job.component.css',
})
export class AddJobComponent {
  jobService = inject(JobService);
  toastService = inject(ToastService);
  skillInput = '';
  private fb = inject(FormBuilder);

  form = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(120)]],
    description: ['', [Validators.required, Validators.minLength(20)]],
    budget: [null as number | null, [Validators.required, Validators.min(1)]],
    requiredSkills: this.fb.array<FormControl<string>>([]),
    duration: [
      null as number | null,
      [Validators.required, Validators.min(1), Validators.max(365)],
    ],
  });

  get requiredSkills() {
    return this.form.get('requiredSkills') as FormArray<FormControl<string>>;
  }

  get f() {
    return this.form.controls;
  }

  addSkill(): void {
    const value = this.skillInput.trim();
    if (!value) return;

    const exists = this.requiredSkills.value.some((s) => s.toLowerCase() === value.toLowerCase());
    if (!exists) {
      this.requiredSkills.push(this.fb.nonNullable.control(value));
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
    this.requiredSkills.removeAt(index);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const raw = this.form.getRawValue();
    var date = new Date();
    date.setDate(date.getDate() + raw.duration!);
    const dto: JobRequestDto = {
      title: raw.title!.trim(),
      description: raw.description!.trim(),
      budget: raw.budget!,
      requiredSkills: raw.requiredSkills.length ? raw.requiredSkills : null,
      duration: date.toISOString(),
    };

    this.jobService.addJob(dto).subscribe((x) => {
      this.toastService.success('Created Succefully');
    });
  }

  onCancel(): void {
    this.form.reset();
    this.requiredSkills.clear();
    this.skillInput = '';
  }
}
