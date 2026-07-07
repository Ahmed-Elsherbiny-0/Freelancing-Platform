import {
  ApplicationConfig,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth-interceptor';
import { InitService } from './core/services/init.service';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { errorInterceptor } from './core/interceptors/error-interceptor';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';
import { MessageService } from 'primeng/api';
import { loadingInterceptor } from './core/interceptors/loading-interceptor';

const initializeApp = async () => {
  const initService = inject(InitService);
  return firstValueFrom(initService.init()).finally(() => {
    const splash = document.getElementById('initial-splash');
    if (splash) {
      splash.remove();
    }
  });
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withViewTransitions()),
    provideZonelessChangeDetection(),
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor, loadingInterceptor])),
    provideAppInitializer(initializeApp),
    providePrimeNG({ theme: { preset: Aura } }),
    MessageService,
  ],
};
