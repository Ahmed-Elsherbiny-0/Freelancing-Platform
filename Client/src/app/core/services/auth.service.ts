import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { catchError, finalize, firstValueFrom, map, Observable, of, shareReplay, tap } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { Mutex } from '../../shared/models/mutex';
import { ToastService } from './toast.service';
import { PresenceService } from './presence.service';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = environment.apiUrl;
  private httpClient = inject(HttpClient);
  private router = inject(Router);
  private toast = inject(ToastService);
  private presenceService = inject(PresenceService);
  currentUser = signal<User | null>(null);
  private refreshSub$: Observable<any> | null = null;
  get profileIconName() {
    if (this.currentUser()) {
      return this.currentUser()!.fristName[0] + this.currentUser()!.lastName[0];
    }
    return 'ab';
  }
  constructor() {}

  login(email: string, password: string) {
    return this.httpClient.post<User>(this.baseUrl + 'auth/login', { email, password }).pipe(
      tap(async (x) => {
        this.currentUser.set(x);
        if (this.currentUser() != null)
          await this.presenceService.createHubConnection(this.currentUser()!);
        localStorage.setItem('refreshToken', x.refreshToken);
      }),
    );
  }

  getRefreshToken() {
    let refreshToken = localStorage.getItem('refreshToken');

    if (this.refreshSub$) {
      return this.refreshSub$;
    }
    if (refreshToken) {
      this.refreshSub$ = this.httpClient
        .post<User>(this.baseUrl + 'auth/refresh', { refreshToken })
        .pipe(
          tap((x) => {
            this.currentUser.set(x);
            localStorage.setItem('refreshToken', x.refreshToken);
          }),
          catchError((err) => {
            return this.logout(true);
          }),
          shareReplay(1),
          finalize(() => {
            this.refreshSub$ = null;
          }),
        );
      return this.refreshSub$;
    }
    return this.logout();
  }

  logout(routeToLogin = false) {
    localStorage.removeItem('refreshToken');
    if (routeToLogin) {
      this.router.navigateByUrl('/login');
    }
    this.toast.success('logout Successfully');
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();

    return of(null);
  }

  isAuthinticated() {
    return this.httpClient.get(this.baseUrl + 'auth/me').pipe(
      map(() => true),
      catchError(() => of(false)),
    );
  }
  register<T>(obj: T) {
    return this.httpClient.post<T>(this.baseUrl + 'auth/register', obj, {
      headers: {
        origin: 'https://localhost:4200/verify-email',
      },
    });
  }
  confirmEmail(code: string, userId: string) {
    return this.httpClient.post(
      this.baseUrl + 'auth/confirm-email',
      {},
      {
        params: { code, userId },
      },
    );
  }

  forgetPassword(email: string) {
    return this.httpClient.post(this.baseUrl + 'auth/forget-password', { email });
  }

  resetPassword(code: string, email: string, password: string) {
    return this.httpClient.post(this.baseUrl + 'auth/reset-password', {
      Email: email,
      Code: code,
      NewPassword: password,
    });
  }

  resendConfirmationEamil(email: string) {
    return this.httpClient.post(this.baseUrl + 'auth/resend-confirmation-email', {
      Email: email,
    });
  }
}
