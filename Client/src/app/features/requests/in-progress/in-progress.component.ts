import { Component, inject, Input } from '@angular/core';
import { UserService } from '../../../core/services/user.service';
import { Offer } from '../../../shared/models/offer';
import { DatePipe } from '../../../shared/pipes/date-pipe';
import { RouterLink } from '@angular/router';
import { ToastService } from '../../../core/services/toast.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-in-progress',
  imports: [DatePipe, RouterLink],
  templateUrl: './in-progress.component.html',
  styleUrl: './in-progress.component.css',
})
export class InProgressComponent {
  userService = inject(UserService);
  toastService = inject(ToastService);
  authService = inject(AuthService);
  @Input({ required: true }) offers!: Offer[];
  ngOnInit(): void {
    console.log(this.offers);
  }
  onComplete(workerId?: string, jobId?: number) {
    this.userService.completeOffer(workerId, jobId)?.subscribe((x) => {
      this.toastService.success('You Complete Offer Succifully');
      window.location.reload();
    });
  }
}
