import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { HeaderComponent } from '../../layout/header/header.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { UserService } from '../../core/services/user.service';
import { catchError, throwError } from 'rxjs';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AwaitingApproveComponent } from './awaiting-approve/awaiting-approve.component';
import { InProgressComponent } from './in-progress/in-progress.component';
import { CompletedComponent } from './completed/completed.component';
import { Offer } from '../../shared/models/offer';
import { OfferParms } from '../../shared/models/pagination';

@Component({
  selector: 'app-requests',
  imports: [
    HeaderComponent,
    FooterComponent,
    RouterLink,
    RouterOutlet,
    AwaitingApproveComponent,
    InProgressComponent,
    CompletedComponent,
  ],
  templateUrl: './requests.component.html',
  styleUrl: './requests.component.css',
})
export class RequestsComponent implements OnInit {
  authService = inject(AuthService);
  userService = inject(UserService);
  currentPage = signal<string>('Awaiting Approval');
  offersForWorkers = signal<Offer[]>([]);
  first: number = 0;
  rows: number = 50;
  workerParms: OfferParms = { pageIndex: 1, pageSize: this.rows };

  ngOnInit(): void {
    if (!this.authService.currentUser()?.isClient) {
      this.userService.getoffersForWorker(this.workerParms).subscribe((x) => {
        this.offersForWorkers.set(x.data);
        console.log(x);
      });
    } else {
      this.userService.getOffersForClient().subscribe((x) => {
        this.offersForWorkers.set(x);
      });
    }
  }
  onClickbutton(value: string) {
    this.currentPage.set(value);
  }
}
