import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  carouselAdd$: Observable<any>;
  private carouselAddSubject = new Subject<any>();

  constructor() {
    this.carouselAdd$ = this.carouselAddSubject.asObservable();
  }
  
  addSlide (data: any) { 
    console.log(data);
    this.carouselAddSubject.next(data);
  }
}
