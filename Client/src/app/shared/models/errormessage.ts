import { HttpErrorResponse } from '@angular/common/http';

/**
 * Front-end messages keyed by the backend error code (Error.Code).
 * Change the text here without touching the backend.
 */
export const ERROR_MESSAGES: Record<string, string> = {
  // User errors
  'User.InvalidCredentials': 'Incorrect email or password.',
  'User.DisabledUser': 'Your account is disabled. Please contact your administrator.',
  'User.LockedUser': 'Your account is locked. Please contact your administrator.',
  'User.EmailNotConfirmed': 'Your email is not confirmed. Please confirm your email.',
  'User.DuplicatedEmail': 'This email is already registered.',
  'User.DuplicatedPhone': 'This phone number is already registered.',
  'User.InvalidJwtToken': 'Your session is invalid. Please log in again.',
  'User.InvalidRefreshToken': 'Your session has expired. Please log in again.',
  'User.InvalidCode': 'The code you entered is invalid.',
  'User.DuplicatedConfirmation': 'Your email is already confirmed.',
  'User.NotFounded': 'User not found.',

  // Job / Project errors
  'Project.NotFounded': 'Project not found.',
};

/** Fallback messages by HTTP status (used when the code is unknown). */
export const STATUS_MESSAGES: Record<number, string> = {
  0: 'Cannot reach the server. Check your internet connection.',
  400: 'Invalid request. Please check your data.',
  401: 'You are not authorized. Please log in.',
  403: 'You do not have permission to do this.',
  404: 'The requested item was not found.',
  409: 'This item already exists.',
  500: 'Something went wrong on our side. Please try again later.',
};

/** Backend may send errors as string[] or as [{ code, description }]. Normalize to codes. */
export function getErrorCodes(err: HttpErrorResponse): string[] {
  const raw = err?.error?.errors;
  if (!Array.isArray(raw)) return [];
  return raw
    .map((e: any) => (typeof e === 'string' ? e : (e?.code ?? e?.description ?? '')))
    .filter(Boolean);
}

/** Returns user-friendly messages for any HttpErrorResponse. Use this in components too. */
export function getErrorMessages(err: HttpErrorResponse): string[] {
  const codes = getErrorCodes(err);

  // 1) Known codes -> show only the friendly messages (ignore backend descriptions)
  const known = codes.filter((c) => ERROR_MESSAGES[c]).map((c) => ERROR_MESSAGES[c]);

  if (known.length) {
    return [...new Set(known)]; // remove duplicates
  }

  // 2) Unknown codes (e.g. validation text) -> show them as they are
  if (codes.length) {
    return [...new Set(codes)];
  }

  // 3) No body at all -> message by HTTP status
  return [STATUS_MESSAGES[err.status] ?? 'Unexpected error. Please try again.'];
}
