import { Component, inject, OnInit, signal } from '@angular/core';
import { HeaderComponent } from '../../layout/header/header.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { FormsModule } from '@angular/forms';
import { JobService } from '../../core/services/job.service';
import { JobParms } from '../../shared/models/pagination';
import { Job } from '../../shared/models/job';
import { JobCardComponent } from './job-card/job-card.component';
import { Paginator, PaginatorState } from 'primeng/paginator';

@Component({
  selector: 'app-jobs',
  imports: [HeaderComponent, FooterComponent, FormsModule, JobCardComponent, Paginator],
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.css',
})
export class JobsComponent implements OnInit {
  openDialog = signal<boolean>(false);
  jobService = inject(JobService);
  parms: JobParms = {};
  jobs = signal<Job[]>([]);
  budgetText: string | null = null;
  statusOption: string | null = 'all';
  search: string | null = null;
  first: number = 0;
  rows: number = 6;

  ngOnInit(): void {
    this.jobService.getAllJobs(null).subscribe((x) => {
      this.jobs.set(x.data);
    });
  }

  OnOpenDialog() {
    this.openDialog.set(!this.openDialog());
  }
  OnSearch() {
    if (this.search != null && this.search != '') {
      this.parms.search = this.search;
      this.loadJobs();
    } else {
      this.parms.search = null;
      this.loadJobs();
    }
  }

  onChangeBudget(text: string) {
    if (text.trim() !== '' && Number.isFinite(Number(text))) {
      this.parms.budget = +text;
      this.loadJobs();
    }
  }
  onChangeStatus(event: Event) {
    var res = (event.target as HTMLSelectElement).value;
    if (res == 'all') {
      this.parms.status = null;
    } else {
      this.parms.status = res;
    }
    this.loadJobs();
  }
  onPageChange(event: PaginatorState) {
    this.first = event.first!;
    this.rows = event.rows!;
    this.parms.pageIndex = +event.page! + 1;
    this.parms.pageSize = event.rows!;
    this.loadJobs();
  }
  private loadJobs() {
    this.jobService.getAllJobs(this.parms).subscribe((x) => {
      this.jobs.set(x.data);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    });
  }
}
