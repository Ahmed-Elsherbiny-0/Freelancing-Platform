import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service'; // adjust path
import { ToastService } from '../services/toast.service'; // adjust path
import { getErrorCodes, getErrorMessages } from '../../shared/models/errormessage';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const codes = getErrorCodes(err);
      const firstCode = codes[0];
      const message = getErrorMessages(err).join(' , ');

      switch (err.status) {
        // Server unreachable / CORS / offline
        case 0:
          toast.error(message);
          break;

        // Validation errors: let the component/form show them
        case 400:
          break;

        case 401:
          if (firstCode === 'User.EmailNotConfirmed') {
            router.navigateByUrl('/resend-confirm-email');
            toast.normal(message);
          } else if (
            firstCode === 'User.InvalidJwtToken' ||
            firstCode === 'User.InvalidRefreshToken'
          ) {
            // session problem -> go to login
            router.navigateByUrl('/login');
            toast.error(message);
          }
          // Other 401s (wrong password, locked, disabled...) are shown
          // by the login component using getErrorMessages(err)
          break;

        case 403:
          toast.error(message);
          break;

        case 404:
          // Only redirect for page/data not found, not for "User not found" forms
          // router.navigateByUrl('/not-found');
          break;

        case 409:
          // Duplicated email/phone: let the form show it
          break;

        default:
          if (err.status >= 500) {
            toast.error(message);
            // router.navigateByUrl('/server-error', { state: { error: err.error } });
          }
          break;
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
