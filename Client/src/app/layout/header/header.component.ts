import {
  Component,
  computed,
  EventEmitter,
  HostListener,
  inject,
  OnInit,
  Output,
  output,
  signal,
} from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { PresenceService } from '../../core/services/presence.service';

@Component({
  selector: 'app-header',
  imports: [RouterLink, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  router = inject(Router);
  theme = signal<boolean>(false);
  menuOpen = signal(false);
  authService = inject(AuthService);
  profileDialog = signal<boolean>(false);
  presence = inject(PresenceService);
  staticUnreadMessage = this.presence.unReadedMessages();

  unreaded = computed(() => {
    let unreaded = 0;
    this.presence.unReadedMessages().forEach((x) => {
      unreaded += x.count;
    });
    return unreaded;
  });

  constructor() {
    const word = window.localStorage.getItem('theme');
    if (word !== 'true') {
      this.theme.set(false);
      document.body.classList.add('dark');
    } else {
      this.theme.set(true);
    }
  }

  toggleLight() {
    this.theme.set(!this.theme());
    document.body.classList.toggle('dark');
    window.localStorage.setItem('theme', `${this.theme()}`);
  }

  toggleMenu() {
    this.menuOpen.set(!this.menuOpen());
  }

  onClickProfile(event: Event) {
    event.stopPropagation();
    this.profileDialog.set(!this.profileDialog());
  }
  @HostListener('document:click')
  closeMenu() {
    if (this.profileDialog()) {
      this.profileDialog.set(!this.profileDialog());
    }
  }

  onClickLogin() {
    this.router.navigateByUrl('/login');
  }

  async onClickLogout() {
    await this.authService.logout(true);
  }
}
