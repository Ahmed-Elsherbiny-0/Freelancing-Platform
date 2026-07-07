import { Photo } from './photo';

export interface User {
  id: string;
  fristName: string;
  lastName: string;
  email: string;
  refreshToken: string;
  title?: string;
  rate?: number;
  level?: string;
  pictureUrl?: string;
  picturePublicId?: string;
  description?: string;
  salary?: number;
  lastOnline?: string;
  roles?: string[];
  permissions?: string[];
  token: string;
  isClient: boolean;
}
export interface Worker {
  firstName: string;
  lastName: string;
  email: string;
  country: string;
  photoUrl?: string;
  rating?: number;
  description?: string;
  skills?: string[];
  title?: string;
  completedTasks: number;
  salary?: number;
  lastOnline?: string;
}

export interface SimpleUser {
  firstName: string;
  lastName: string;
  email: string;
  refreshToken: string;
  photo: string;
}
export interface UserInfoRequestDto {
  firstName: string;
  lastName: string;
  country: string;
  skills: string[] | null;
  description: string;
}
