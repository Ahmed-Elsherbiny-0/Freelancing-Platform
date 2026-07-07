import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { UserService } from '../../../core/services/user.service';
import { ActivatedRoute, Router, RouterLinkActive, RouterLink } from '@angular/router';
import { Worker } from '../../../shared/models/user';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-freelancer-profile',
  imports: [HeaderComponent, FooterComponent, RouterLink],
  templateUrl: './freelancer-profile.component.html',
  styleUrl: './freelancer-profile.component.css',
})
export class FreelancerProfileComponent implements OnInit {
  userService = inject(UserService);
  router = inject(ActivatedRoute);
  authService = inject(AuthService);
  worker = signal<Worker | null>(null);
  toast = inject(ToastService);
  username: string | null = null;

  get displayName() {
    if (this.worker() != null) {
      return (
        (this.worker()!.firstName =
          this.worker()!.firstName[0].toUpperCase() + this.worker()!.firstName.slice(1)) +
        ' ' +
        (this.worker()!.lastName =
          this.worker()!.lastName[0].toUpperCase() + this.worker()!.lastName.slice(1))
      );
    }
    return null;
  }
  ngOnInit(): void {
    this.router.paramMap.subscribe((x) => {
      this.username = x.get('username');
      if (this.username != null && this.username != '') {
        this.userService.getWorker(this.username).subscribe((x) => {
          this.worker.set(x);
        });
      }
    });
  }

  onCopyProfileLink(event: any) {
    const fullUrl = window.location.href;
    navigator.clipboard.writeText(fullUrl).then(() => {
      this.toast.success('Coppied Successfully');
    });
  }
}
