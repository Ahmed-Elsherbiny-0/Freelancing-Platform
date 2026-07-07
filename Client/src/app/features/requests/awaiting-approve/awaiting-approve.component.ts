import { Component, inject, Input, input, OnInit, signal } from '@angular/core';
import { UserService } from '../../../core/services/user.service';
import { Offer } from '../../../shared/models/offer';
import { OfferParms } from '../../../shared/models/pagination';
import { DatePipe } from '../../../shared/pipes/date-pipe';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-awaiting-approve',
  imports: [DatePipe, RouterLink],
  templateUrl: './awaiting-approve.component.html',
  styleUrl: './awaiting-approve.component.css',
})
export class AwaitingApproveComponent implements OnInit {
  authService = inject(AuthService);
  userService = inject(UserService);
  toastService = inject(ToastService);
  @Input({ required: true }) offers!: Offer[];
  ngOnInit(): void {}
  displayDuration() {}
  onApprove(workerId?: string, jobId?: number) {
    this.userService.approveOffer(workerId, jobId)?.subscribe((x) => {
      this.toastService.success('You Approve Offer Succifully');
      window.location.reload();
    });
  }
}
