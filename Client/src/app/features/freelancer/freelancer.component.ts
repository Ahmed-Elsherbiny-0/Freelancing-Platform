import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../layout/header/header.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { UserService } from '../../core/services/user.service';
import { Worker } from '../../shared/models/user';
import { FreelancerCardComponent } from './freelancer-card/freelancer-card.component';
import { Paginator, PaginatorModule, PaginatorState } from 'primeng/paginator';
import { WorkerParms } from '../../shared/models/pagination';
import { FormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
@Component({
  selector: 'app-freelancer',
  imports: [
    HeaderComponent,
    FooterComponent,
    FreelancerCardComponent,
    Paginator,
    PaginatorModule,
    FormsModule,
  ],
  templateUrl: './freelancer.component.html',
  styleUrl: './freelancer.component.css',
})
export class FreelancerComponent implements OnInit {
  openDialog = signal<boolean>(false);
  userService = inject(UserService);
  workers = signal<Worker[]>([]);
  first: number = 0;
  rows: number = 6;
  search: string | null = null;
  parms: WorkerParms = {};
  ngOnInit(): void {
    this.userService.getWorkers(this.parms).subscribe((x) => {
      this.workers.set(x.data);
    });
    console.log('hello');
  }

  OnOpenDialog() {
    this.openDialog.set(!this.openDialog());
  }
  onPageChange(event: PaginatorState) {
    this.first = event.first!;
    this.rows = event.rows!;
    this.parms.pageIndex = +event.page! + 1;
    this.parms.pageSize = event.rows!;
    this.loadWorkers();
  }
  onRatingChange(event: Event) {
    this.parms.rating = +(event.target as HTMLSelectElement).value;
    this.loadWorkers();
  }
  onSelectCountry(event: Event) {
    this.parms.country = (event.target as HTMLSelectElement).value;
    if (this.parms.country == 'all') this.parms.country = null;

    this.loadWorkers();
  }
  OnSearch() {
    if (this.search != null && this.search != '') {
      this.parms.search = this.search;
      this.loadWorkers();
    }
  }
  private loadWorkers() {
    this.userService
      .getWorkers(this.parms)
      .pipe(debounceTime(300))
      .subscribe((x) => {
        this.workers.set(x.data);
        window.scrollTo({ top: 0, behavior: 'smooth' });
      });
  }
}
