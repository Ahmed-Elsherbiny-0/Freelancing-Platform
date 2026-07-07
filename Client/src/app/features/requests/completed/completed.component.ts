import { Component, Input } from '@angular/core';
import { Offer } from '../../../shared/models/offer';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-completed',
  imports: [DatePipe, RouterLink],
  templateUrl: './completed.component.html',
  styleUrl: './completed.component.css',
})
export class CompletedComponent {
  @Input({ required: true }) offers!: Offer[];
}
