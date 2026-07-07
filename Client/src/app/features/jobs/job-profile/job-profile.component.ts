import {
  Component,
  computed,
  ElementRef,
  HostListener,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { HeaderComponent } from '../../../layout/header/header.component';
import { FooterComponent } from '../../../layout/footer/footer.component';
import { JobService } from '../../../core/services/job.service';
import { Job } from '../../../shared/models/job';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { DatePipe } from '../../../shared/pipes/date-pipe';
import { OfferParms } from '../../../shared/models/pagination';
import { Offer, offerDto } from '../../../shared/models/offer';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Paginator, PaginatorState } from 'primeng/paginator';
@Component({
  selector: 'app-job-profile',
  imports: [HeaderComponent, FooterComponent, DatePipe, ReactiveFormsModule, Paginator, RouterLink],
  templateUrl: './job-profile.component.html',
  styleUrl: './job-profile.component.css',
})
export class JobProfileComponent implements OnInit {
  jobService = inject(JobService);
  router = inject(ActivatedRoute);
  authService = inject(AuthService);
  job = signal<Job | null>(null);
  first: number = 0;
  rows: number = 2;
  id: string | null = null;
  parms: OfferParms = { pageIndex: 1, pageSize: this.rows };
  offers = signal<Offer[]>([]);
  visible = signal<boolean>(false);

  hideSubmitProposal = computed(() => {
    for (let item of this.offers()) {
      if (item.workerEamil == this.authService.currentUser()?.email) {
        return true;
      }
    }
    return false;
  });
  form = new FormGroup({
    description: new FormControl('', Validators.required),
    price: new FormControl(0, [Validators.required]),
    duration: new FormControl('', Validators.required),
  });
  @ViewChild('overlay') overlay?: ElementRef<HTMLDivElement>;
  ngOnInit(): void {
    this.router.paramMap.subscribe((x) => {
      this.id = x.get('id');
      if (this.id != null && this.id != '') {
        this.jobService.getJob(this.id).subscribe((x) => {
          this.job.set(x);
        });
        this.jobService.getAllOffersForWorker(this.parms, +this.id).subscribe((x) => {
          this.offers.set(x.data);
        });
      }
    });
  }

  displayDuration(duration: string) {
    let now = new Date();
    let expiresAt = new Date(duration);
    let diffms = expiresAt.getTime() - now.getTime();
    let diffdays = (diffms / (60 * 60 * 24 * 1000)).toFixed(0);
    if (+diffdays < 0) {
      return 0;
    }
    return diffdays;
  }
  showDialog(event: Event) {
    event.stopPropagation();
    this.visible.set(true);
  }
  closedDialog() {
    this.visible.set(false);
    this.form.reset();
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event) {
    if (!this.overlay) return;
    if (event.target === this.overlay.nativeElement) {
      this.closedDialog();
    }
  }
  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    if (this.id == null) return;
    const futureDate = new Date();
    futureDate.setDate(futureDate.getDate() + +this.form.get('duration')?.value!);
    let offer: offerDto = {
      description: this.form.get('description')?.value!,
      Duration: futureDate.toISOString(),
      jobId: +this.id,
      Price: +this.form.get('price')?.value!,
    };
    this.jobService.addOffer(offer).subscribe();
    this.closedDialog();
  }
  onPageChange(event: PaginatorState) {
    this.first = event.first!;
    this.rows = event.rows!;
    this.parms.pageIndex = +event.page! + 1;
    this.parms.pageSize = event.rows!;
    this.loadOffers();
  }
  private loadOffers() {
    this.jobService.getAllOffersForWorker(this.parms, +this.id!).subscribe((x) => {
      this.offers.set(x.data);
    });
  }
}
