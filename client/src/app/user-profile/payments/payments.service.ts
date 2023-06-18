import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaymentIntent } from '@stripe/stripe-js';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPaymentIntents() {
    return this.http.get<PaymentIntent[]>(this.baseUrl + 'payments/user-payments');
  }
}
