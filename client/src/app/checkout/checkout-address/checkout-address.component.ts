import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Address } from 'src/app/shared/models/Address';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent implements OnInit, OnChanges {
  @Input() checkoutForm?: FormGroup;
  @Input() addresses: Address[];
  @Output() selectAddress = new EventEmitter<Address>();
  @Output() newAddress = new EventEmitter<boolean>();
  disableInput: boolean = false;
  
  ngOnInit(): void {
    this.changeSelectedAddress(this.addresses?.find(a => a.isCurrent)?.displayName || "add");
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes.addresses) {
      this.changeSelectedAddress(this.addresses?.find(a => a.isCurrent)?.displayName);
    }
  }

  changeSelectedAddress(address: string) {
    if(address == "add") {
      this.selectAddress.emit(null);
      this.disableInput = false;
      return;
    }
    this.addresses.forEach(a => {
      if(a.displayName.toLowerCase().trim() == address.toLowerCase().trim())
        this.selectAddress.emit(a);
    })
    this.disableInput = true;
  }

  addAddressIfNew() {
    this.newAddress.emit(this.disableInput)
  }
}
