import { Injectable } from '@angular/core';
import {
        HttpRequest, HttpHandler,
        HttpEvent, HttpInterceptor 
      } from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators'
import { BusyService } from 'src/app/core/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.url.includes('emailExists') || request.method === 'POST' && request.url.includes('orders')) {
      return next.handle(request);
    }

    this.busyService.busy();
    return next.handle(request).pipe(
      // delay(6000),
      finalize(() => {
        this.busyService.idle();
      })
    );
  }
}
