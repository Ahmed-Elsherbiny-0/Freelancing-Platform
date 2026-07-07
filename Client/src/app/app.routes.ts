import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { CheckEmailComponent } from './features/account/check-email/check-email.component';
import { checkemailGuard } from './core/guards/checkemail-guard';
import { VerifyEmailComponent } from './features/account/verify-email/verify-email.component';
import { ForgetPasswordComponent } from './features/account/forget-password/forget-password.component';
import { ResetPasswordComponent } from './features/account/reset-password/reset-password.component';
import { ResendConfirmEmailComponent } from './features/account/resend-confirm-email/resend-confirm-email.component';
import { MessagesComponent } from './features/messages/messages.component';
import { MessageChatComponent } from './features/messages/message-chat/message-chat.component';
import { FreelancerComponent } from './features/freelancer/freelancer.component';
import { FreelancerProfileComponent } from './features/freelancer/freelancer-profile/freelancer-profile.component';
import { JobsComponent } from './features/jobs/jobs.component';
import { JobProfileComponent } from './features/jobs/job-profile/job-profile.component';
import { RequestsComponent } from './features/requests/requests.component';
import { authGuard } from './core/guards/auth-guard';
import { AddJobComponent } from './features/jobs/add-job/add-job.component';
import { UpdateAccountComponent } from './features/account/update-account/update-account.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'signup',
    component: RegisterComponent,
  },
  { path: 'check-email', component: CheckEmailComponent, canActivate: [checkemailGuard] },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'forget-password', component: ForgetPasswordComponent },
  { path: 'resend-confirm-email', component: ResendConfirmEmailComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'home', component: HomeComponent },
  {
    path: 'messages',
    component: MessagesComponent,
    canActivate: [authGuard],
    children: [
      {
        path: ':username',
        component: MessageChatComponent,
      },
    ],
  },
  { path: 'workers', component: FreelancerComponent },
  {
    path: 'workers/:username',
    component: FreelancerProfileComponent,
  },
  {
    path: 'announcements',
    component: JobsComponent,
  },
  {
    path: 'announcements/:id',
    component: JobProfileComponent,
  },
  {
    path: 'requests',
    component: RequestsComponent,
    canActivate: [authGuard],
  },
  {
    path: 'addjob',
    component: AddJobComponent,
    canActivate: [authGuard],
  },
  {
    path: 'update-account',
    component: UpdateAccountComponent,
    canActivate: [authGuard],
  },
];
