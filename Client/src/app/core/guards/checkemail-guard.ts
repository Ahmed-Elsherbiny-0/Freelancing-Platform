import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const checkemailGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const nav = router.currentNavigation();
  const email = nav?.extras?.state?.['email'];

  if (email) return true;

  router.navigate(['/']);
  return false;
};
