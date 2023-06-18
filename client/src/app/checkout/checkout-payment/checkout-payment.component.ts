import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Address } from 'src/app/shared/models/Address';
import { CheckoutService } from '../checkout.service';
import { loadStripe, Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement } from '@stripe/stripe-js';
import { OrderToCreate } from 'src/app/shared/models/Order';
import { Basket, IBasket } from 'src/app/basket/Basket';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  @ViewChild('cardNumber', {static: true}) cardNumberElement?: ElementRef;
  @ViewChild('cardExpiry', {static: true}) cardExpiryElement?: ElementRef;
  @ViewChild('cardCvc', {static: true}) cardCvcElement?: ElementRef;
  stripe: Stripe | null = null;
  cardNumber?: StripeCardNumberElement;
  cardExpiry?: StripeCardExpiryElement;
  cardCvc?: StripeCardCvcElement;
  cardInfoComplete: boolean[] = [false, false, false];
  cardErrors: any;
  loading: boolean = false;

  constructor(private basketService: BasketService, 
    private checkoutService: CheckoutService, private toastr: ToastrService, private router: Router) { }
  
    ngOnInit(): void {
    loadStripe(
      "pk_test_51MmIRfJBAHRKMXJHIbfkYza5E2vpB0rsK0ydwKwqTGpk2nl88WWfiMJyjNVewXbQSlkdbOYSZYBKoGGg03EHr9Se00aYI86BFx") // TODO: Move to environment variables and replace it with Tom's key
      .then(stripe => {
        this.stripe = stripe;
        const elements = stripe?.elements();
        if(elements) {
          this.cardNumber = elements.create('cardNumber', {style: { base: {'color': 'gold'}}});
          this.cardNumber.mount(this.cardNumberElement?.nativeElement);
          this.cardNumber.on('change', (event) => {
            this.cardInfoComplete[0] = event.complete;
            if(event.error) {
              this.cardErrors = event.error.message;
            } else {
              this.cardErrors = null;
            }
          });

          this.cardExpiry = elements.create('cardExpiry', {style: { base: {'color': 'gold'}}});
          this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
          this.cardExpiry.on('change', (event) => {
            this.cardInfoComplete[1] = event.complete;
            if(event.error) {
              this.cardErrors = event.error.message;
            } else {
              this.cardErrors = null;
            }
          });

          this.cardCvc = elements.create('cardCvc', {style: { base: {'color': 'gold'}}});
          this.cardCvc.mount(this.cardCvcElement?.nativeElement);
          this.cardCvc.on('change', (event) => {
            this.cardInfoComplete[2] = event.complete;
            if(event.error) {
              this.cardErrors = event.error.message;
            } else {
              this.cardErrors = null;
            }
          });
        };
      }
    )
  }

  get paymentFormComplete() {
    return this.checkoutForm?.get('paymentForm')?.valid 
    && this.cardInfoComplete.every(x => x);
  }

  async submitOrder() {
    this.loading = true;
    const basket = this.basketService.getCurrentBasketValue();
    if (!basket) throw new Error('Basket is null');
    
    try {
      const createdOrder = await this.createOrder(basket);
      const paymentResult = await this.confirmPaymentWithStripe(basket);
      
      if (paymentResult.paymentIntent) {
        this.basketService.deleteBasket(basket);
        const navigationExtras: NavigationExtras = {state: createdOrder};
        this.router.navigate(['checkout/success'], navigationExtras);
      } else {
        this.toastr.error(paymentResult.error.message);
      }
    }
    catch (error : any) {
      console.log(error);
      this.toastr.error(error.message);
    }
    finally {
      this.loading = false;
    }
  }

  private async confirmPaymentWithStripe(basket: IBasket | null) {
    if (!basket) throw new Error('Basket is null');
    const result = this.stripe?.confirmCardPayment(basket.clientSecret!, {
      payment_method: {
        card: this.cardNumber!,
        billing_details: {
          name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
        }
      }
    });
    if (!result) throw new Error('Problem attempting payment with stripe');
    return result;
  }

  private async createOrder(basket: IBasket | null) {
    if (!basket) throw new Error('Basket is null');
    const orderToCreate = this.getOrderToCreate(basket);
    return this.checkoutService.createOrder(orderToCreate).toPromise();
  }

  private getOrderToCreate(basket: Basket): OrderToCreate {
    const deliveryMethodId = this.checkoutForm?.get('deliveryForm')?.get('deliveryMethod')?.value;
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;

    if(!deliveryMethodId || !shipToAddress) throw new Error('Problem with basket');

    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress
    }
  }

}
