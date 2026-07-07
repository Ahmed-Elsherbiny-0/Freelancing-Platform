import { Component, input, signal } from '@angular/core';

@Component({
  selector: 'app-form-header',
  imports: [],
  templateUrl: './form-header.component.html',
  styleUrl: './form-header.component.css',
})
export class FormHeaderComponent {
  primareyheading = input.required<string>();
  Secondaryheading = input.required<string>();
}
