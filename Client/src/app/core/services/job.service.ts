import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { JobParms, OfferParms, Pagination } from '../../shared/models/pagination';
import { Job, JobRequestDto } from '../../shared/models/job';
import { environment } from '../../../environments/environment.development';
import { tap } from 'rxjs';
import { Offer, offerDto } from '../../shared/models/offer';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root',
})
export class JobService {
  private httpClient = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  private toastService = inject(ToastService);
  jobsPage = signal<Pagination<Job> | null>(null);
  offerPage = signal<Pagination<Offer> | null>(null);
  getAllJobs(parms: JobParms | null) {
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
    if (parms && parms.budget != null) {
      sendPamars = sendPamars.append('budget', parms.budget);
    }
    if (parms && parms.status != null) {
      sendPamars = sendPamars.append('status', parms.status);
    }

    return this.httpClient
      .get<Pagination<Job>>(this.baseUrl + 'api/clients/get-all-projects', {
        params: sendPamars,
      })
      .pipe(tap((x) => this.jobsPage.set(x)));
  }

  getJob(id: string) {
    return this.httpClient.get<Job>(this.baseUrl + 'api/clients/get-project', {
      params: {
        jobid: id,
      },
    });
  }
  getAllOffersForWorker(parms: OfferParms | null, jobid: number) {
    let sendPamars = new HttpParams();
    if (parms && parms.pageIndex != null) {
      sendPamars = sendPamars.append('pageIndex', parms.pageIndex.toString());
    }
    if (parms && parms.pageSize != null) {
      sendPamars = sendPamars.append('pageSize', parms.pageSize.toString());
    }
    sendPamars = sendPamars.append('jobid', jobid.toString());
    return this.httpClient
      .get<Pagination<Offer>>(this.baseUrl + 'api/workers/get-offers', {
        params: sendPamars,
      })
      .pipe(tap((x) => this.offerPage.set(x)));
  }
  addOffer(offer: offerDto) {
    return this.httpClient.post(this.baseUrl + 'api/workers/add-offer', offer).pipe(
      tap((x) => {
        this.toastService.success('Added Offer Successfuly');
      }),
    );
  }
  addJob(job: JobRequestDto) {
    return this.httpClient.post(this.baseUrl + 'api/clients/add-project', job);
  }
}
