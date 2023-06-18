import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Address } from '../shared/models/Address';
import { AccountService } from '../user-profile/account.service';
import { AddressService } from '../user-profile/addresses/address.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit{
  addresses: Address[];
  selectedAddress: Address;

  constructor(private fb: FormBuilder, private cd: ChangeDetectorRef,
    private addressService: AddressService, private basketService: BasketService) { }

  ngOnInit(): void {    
    this.getAddressFormValues();
    this.getDeliveryMethodValue();
  }

  checkoutForm = this.fb.group({
    addressForm: this.fb.group({
      displayName: ['', Validators.required],
      country: ['', Validators.required],
      state: ['', Validators.required],
      city: ['', Validators.required],
      street: ['', Validators.required],
      zipCode: ['', Validators.required]
    }),
    deliveryForm: this.fb.group({
      deliveryMethod: ['', Validators.required]
    }),
    paymentForm: this.fb.group({
      nameOnCard: ['', Validators.required]
    }),
  })

  getAddressFormValues() {
    this.addressService.getAddresses().subscribe({
      next: addresses => {
        this.addresses = addresses;
        this.addresses.sort((a, b) => { return (a === b)? 0 : a? -1 : 1 });
        console.log(this.addresses);
        this.selectedAddress = this.addresses.find(a => a.isCurrent === true);
        this.patchSelectedAddress();
      }
    })
  }

  patchSelectedAddress(address?: Address) {
    if(address !== null){
      this.selectedAddress = address;
      this.checkoutForm.get('addressForm')?.patchValue(this.selectedAddress);
    }
    else {
      this.checkoutForm.get('addressForm')?.reset();
    }
    // The code below makes angular check changes on DOM manually which prevents the ExpressionChangedAfterItHasBeenCheckedError caused 
    // by modifying the parent component "checkout.component.html" by changing the state of "checkoutForm.valid" from false to true in the template
    this.cd.detectChanges();
  }

  addAddressIfNew(addressExists: boolean) {
    if(addressExists) return;

    let address = this.checkoutForm?.get('addressForm')?.value;
    address.isCurrent = true;
    this.addressService.addAddress(address).subscribe({
      next: (a) => { console.log(a); },
      error: (err) => { console.log(err); },
      complete: () => { this.getAddressFormValues(); }
    })
  }

  getDeliveryMethodValue() {
    const basket = this.basketService.getCurrentBasketValue();
    if(basket && basket.deliveryMethodId) {
      this.checkoutForm.get('deliveryForm')?.get('deliveryMethod')
      ?.patchValue(basket.deliveryMethodId.toString());
    }
  }
}
