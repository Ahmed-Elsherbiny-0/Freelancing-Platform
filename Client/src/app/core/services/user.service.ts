import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { tap } from 'rxjs';
import { SimpleUser, User, UserInfoRequestDto, Worker } from '../../shared/models/user';
import { OfferParms, Pagination, WorkerParms } from '../../shared/models/pagination';
import { Offer } from '../../shared/models/offer';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private httpClient = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  WorkersPage = signal<Pagination<Worker> | null>(null);
  workerOffersPage = signal<Pagination<Offer> | null>(null);
  getWorkers(parms: WorkerParms | null) {
    let sendPamars = new HttpParams();
    if (parms && parms.pageIndex != null) {
      sendPamars = sendPamars.append('pageIndex', parms.pageIndex);
    }
    if (parms && parms.pageSize != null) {
      sendPamars = sendPamars.append('pageSize', parms.pageSize);
    }
    if (parms && parms.search != null) {
      sendPamars = sendPamars.append('search', parms.search);
    }
    if (parms && parms.country != null) {
      sendPamars = sendPamars.append('country', parms.country);
    }
    if (parms && parms.rating != null) {
      sendPamars = sendPamars.append('rating', parms.rating);
    }
    return this.httpClient
      .get<Pagination<Worker>>(this.baseUrl + 'users/get-workers', { params: sendPamars })
      .pipe(tap((x) => this.WorkersPage.set(x)));
  }

  getWorker(username: string) {
    return this.httpClient.get<Worker>(this.baseUrl + 'users/get-worker', {
      params: {
        username: username,
      },
    });
  }
  getUser(username: string) {
    return this.httpClient.get<SimpleUser>(this.baseUrl + 'users/get-user', {
      params: {
        username: username,
      },
    });
  }
  getoffersForWorker(parms: OfferParms | null) {
    let sendPamars = new HttpParams();
    if (parms && parms.pageIndex != null) {
      sendPamars = sendPamars.append('pageIndex', parms.pageIndex.toString());
    }
    if (parms && parms.pageSize != null) {
      sendPamars = sendPamars.append('pageSize', parms.pageSize.toString());
    }
    return this.httpClient
      .get<Pagination<Offer>>(this.baseUrl + 'api/workers/get-worker-offers', {
        params: sendPamars,
      })
      .pipe(
        tap((x) => {
          this.workerOffersPage.set(x);
          console.log(x);
        }),
      );
  }

  getOffersForClient() {
    let sendPamars = new HttpParams();
    return this.httpClient.get<Offer[]>(this.baseUrl + 'api/clients/get-all-offers-for-job', {
      params: sendPamars,
    });
  }

  approveOffer(workerId?: string, jobId?: number) {
    let sendPamars = new HttpParams();
    if (workerId == null || jobId == null) {
      return;
    }
    sendPamars = sendPamars.append('workerId', workerId);
    sendPamars = sendPamars.append('jobId', jobId.toString());
    console.log(sendPamars);
    return this.httpClient.post(
      this.baseUrl + 'api/clients/approve-project',
      {},
      {
        params: sendPamars,
      },
    );
  }

  completeOffer(workerId?: string, jobId?: number) {
    let sendPamars = new HttpParams();
    if (workerId == null || jobId == null) {
      return;
    }
    sendPamars = sendPamars.append('workerId', workerId);
    sendPamars = sendPamars.append('jobId', jobId.toString());
    return this.httpClient.post(
      this.baseUrl + 'api/clients/complete-project',
      {},
      {
        params: sendPamars,
      },
    );
  }

  getUserInfo() {
    return this.httpClient.get<UserInfoRequestDto>(this.baseUrl + 'users/get-user-Info');
  }
  updateUserInfo(request: UserInfoRequestDto) {
    return this.httpClient.post(this.baseUrl + 'users/update-user-Info', request);
  }
}
