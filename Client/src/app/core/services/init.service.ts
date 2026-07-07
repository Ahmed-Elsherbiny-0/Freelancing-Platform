import { inject, Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { concatMap, from, of, tap } from 'rxjs';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private authService = inject(AuthService);
  private presenceService = inject(PresenceService);
  init() {
    return this.authService.getRefreshToken().pipe(
      concatMap(() => {
        return from(this.presenceService.createHubConnection(this.authService.currentUser()!));
      }),
    );
  }
}
