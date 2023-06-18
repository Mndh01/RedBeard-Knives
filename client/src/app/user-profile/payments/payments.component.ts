import { Component, OnInit } from '@angular/core';
import { PaymentService } from './payments.service';
import { PaymentIntent } from '@stripe/stripe-js';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html',
  styleUrls: ['./payments.component.scss']
})
export class PaymentsComponent implements OnInit {
  payments: PaymentIntent[];
  constructor(private paymentService: PaymentService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getPayments();
  }

  getPayments() {
    this.paymentService.getPaymentIntents().subscribe({
      next: (payments) => {
        this.payments = payments;
        console.log(this.payments);
      },
      error: (error) => {
        console.log(error);
        
        this.toastr.error(error.error.message);
      }
    })
  }

}
