import { inject, Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private messageService = inject(MessageService);

  error(message: string) {
    this.messageService.add({
      severity: 'error',
      summary: 'Error',
      detail: message,
      life: 5000,
    });
  }

  success(message: string) {
    this.messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: message,
      life: 5000,
    });
  }

  normal(message: string) {
    this.messageService.add({
      severity: 'info',
      summary: 'Info',
      detail: message,
      life: 5000,
    });
  }
}
