import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnInit,
  QueryList,
  ViewChildren,
  viewChildren,
} from '@angular/core';
import { Worker } from '../../../shared/models/user';
import { RatingModule } from 'primeng/rating';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-freelancer-card',
  imports: [RatingModule, FormsModule, RouterLink],
  templateUrl: './freelancer-card.component.html',
  styleUrl: './freelancer-card.component.css',
})
export class FreelancerCardComponent implements AfterViewInit, OnInit {
  @Input({ required: true }) worker!: Worker;
  @ViewChildren('star') stars!: QueryList<ElementRef<HTMLElement>>;
  ngOnInit(): void {
    this.worker.rating = Math.trunc(this.worker.rating! * 10) / 10;
  }
  ngAfterViewInit(): void {
    const arr = this.stars.toArray();
    let x = Math.trunc(this.worker.rating!);
    if (x != undefined) {
      for (x; x < 5; x++) {
        arr[x].nativeElement.setAttribute('fill', 'var(--theme-bg)');
        arr[x].nativeElement.setAttribute('stroke', '#fff');
      }
    }
  }
  get displayName() {
    return (
      (this.worker.firstName =
        this.worker.firstName[0].toUpperCase() + this.worker.firstName.slice(1)) +
      ' ' +
      (this.worker.lastName = this.worker.lastName[0].toUpperCase() + this.worker.lastName.slice(1))
    );
  }
}
