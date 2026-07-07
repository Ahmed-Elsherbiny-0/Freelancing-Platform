import { Component, Input, OnInit } from '@angular/core';
import { Job } from '../../../shared/models/job';
import { DatePipe } from '../../../shared/pipes/date-pipe';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-job-card',
  imports: [DatePipe, RouterLink],
  templateUrl: './job-card.component.html',
  styleUrl: './job-card.component.css',
})
export class JobCardComponent implements OnInit {
  @Input({ required: true }) job!: Job;
  ngOnInit(): void {
    this.job.title = this.job.title[0].toUpperCase() + this.job.title.slice(1);
    this.job.fristName = this.job.fristName[0].toUpperCase() + this.job.fristName.slice(1);
    this.job.lastName = this.job.lastName[0].toUpperCase() + this.job.lastName.slice(1);
  }
}
