import { Component, input, signal } from '@angular/core';
import { User } from '../../../shared/models/user';

@Component({
  selector: 'app-home-freelancer-card',
  imports: [],
  templateUrl: './home-freelancer-card.component.html',
  styleUrl: './home-freelancer-card.component.css',
})
export class HomeFreelancerCardComponent {
  user = input<User>();
}
