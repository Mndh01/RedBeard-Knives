import { Injectable } from '@angular/core';
import {
        HttpRequest, HttpHandler,
        HttpEvent, HttpInterceptor 
      } from '@angular/common/http';
import { Observable } from 'rxjs';
import { delay, finalize } from 'rxjs/operators'
import { BusyService } from 'src/app/services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyService.busy();
    return next.handle(request).pipe(
      // delay(6000),
      finalize(() => {
        this.busyService.idle();
      })
    );
  }
}
