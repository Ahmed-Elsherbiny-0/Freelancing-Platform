import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { HeaderComponent } from './layout/header/header.component';
import { Toast } from 'primeng/toast';
import { BusyService } from './core/services/busy.service';
import { InitialSplashComponent } from './shared/components/initial-splash/initial-splash.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HomeComponent, HeaderComponent, Toast, InitialSplashComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  busyService = inject(BusyService);
  protected readonly title = signal('Client');
}
