import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastService } from '../services/toast.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const snackbar = inject(SnackbarService);
  const toast = inject(ToastService);

  const router = inject(Router);
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 400) {
      } else if (err.status === 401) {
        console.log(err.error.errors[0]);
        if (err.error.errors[0] == 'User.EmailNotConfirmed') {
          router.navigateByUrl('/resend-confirm-email');
          toast.normal('your email not conifimed. plz confirm your email');
        } else if (err.error.errors[0] == 'InvalidToken') {
          console.log('hiihiihi');
        } else {
          // router.navigateByUrl('/login');
        }
      } else if (err.status === 403) {
        // snackbar.error('Access denied');
      } else if (err.status === 404) {
        // router.navigateByUrl('/not-found');
      } else if (err.status === 500) {
        // router.navigateByUrl('/server-error', {
        //   state: { error: err.error },
        // });
      }

      return throwError(() => err);
    }),
  );
};
// function handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//   let authService = inject(AuthService);
//   return authService.getRefreshToken().pipe(
//     switchMap((newToken: string) => {
//       this.isRefreshing = false;
//       this.refreshTokenSubject.next(newToken); // Release queued requests
//       return next.handle(this.addToken(req, newToken));
//     }),
//     catchError((err) => {
//       this.isRefreshing = false;
//       this.authService.logout(); // Refresh failed → logout
//       return throwError(() => err);
//     }),
//   );
// }
