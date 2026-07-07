import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'date',
})
export class DatePipe implements PipeTransform {
  transform(value: string | Date, format: boolean = false): string {
    if (!value) return '';

    const date = this.parseDate(value);
    const now = new Date();

    const diffMs = now.getTime() - date.getTime();
    const diffSeconds = Math.floor(diffMs / 1000);
    const diffMinutes = Math.floor(diffSeconds / 60);
    const diffHours = Math.floor(diffMinutes / 60);
    const diffDays = Math.floor(diffHours / 24);
    if (format) {
      return this.formatTime(date);
    }

    if (diffMs < 0) {
      const futureDays = Math.ceil(-diffMs / (1000 * 60 * 60 * 24));
      if (futureDays === 1) return 'Tomorrow';
      else return `${futureDays} days`;
    }

    if (diffSeconds < 60) {
      return 'Just now';
    }

    if (diffMinutes < 60) {
      return `${diffMinutes}m ago`;
    }

    if (this.isToday(date, now)) {
      return this.formatTime(date);
    }

    if (diffDays === 1 || this.isYesterday(date, now)) {
      return `Yesterday `;
    }

    if (diffDays < 7) {
      const day = date.toLocaleDateString('en-US', { weekday: 'long' });
      return `${day}`;
    }

    const diffWeeks = Math.floor(diffDays / 7);
    if (diffWeeks < 52) {
      return `${diffWeeks}w`;
    }

    // Years
    const diffYears = Math.floor(diffDays / 365);
    return `${diffYears}y`;
  }

  private isToday(date: Date, now: Date): boolean {
    return date.toDateString() === now.toDateString();
  }

  private isYesterday(date: Date, now: Date): boolean {
    const yesterday = new Date(now);
    yesterday.setDate(now.getDate() - 1);
    return date.toDateString() === yesterday.toDateString();
  }

  private formatTime(date: Date): string {
    return date.toLocaleTimeString('en-EG', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });
  }
  private parseDate(value: string | Date): Date {
    if (value instanceof Date) return value;
    const hasTimezone = value.endsWith('Z') || /[+-]\d{2}:\d{2}$/.test(value);
    return new Date(hasTimezone ? value : value.replace(' ', 'T'));
  }
}
