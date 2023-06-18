import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../account.service';
import { User } from 'src/app/shared/models/User';
import { AddressService } from 'src/app/user-profile/addresses/address.service';
import { Address } from 'src/app/shared/models/Address';
import { ToastrService } from 'ngx-toastr';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';

@Component({
  selector: 'app-addresses',
  templateUrl: './addresses.component.html',
  styleUrls: ['./addresses.component.scss']
})
export class AddressesComponent implements OnInit {
  user: User;
  addresses: Address[];
  addressForm: FormGroup;
  @ViewChild('newAddressModal', { static: false }) newAddressModal?: ModalComponent;

  constructor(private accountService: AccountService, private addressService: AddressService,
    private toastr: ToastrService, private fb: FormBuilder) {
      this.accountService.currentUser$.subscribe(user => { this.user = user; });
    }

  ngOnInit(): void {
    this.initializeAddressAddForm();
    this.getAddresses();
  }

  initializeAddressAddForm() {
    this.addressForm  = this.fb.group({
      displayName: [null, [Validators.required]],
      country: [null, [Validators.required]],
      state: [null, [Validators.required]],
      city: [null, [Validators.required]],
      street: [null, [Validators.required]],
      zipCode: [null, [Validators.required]],
      isCurrent: false
    })
  }

  onSubmit() {
    if (this.addressForm.valid) {
      this.addAddress();
      this.onModalHide();
    }
  }

  onShowModal() {
    this.newAddressModal.show();
  }

  onModalHide() {
    this.addressForm.reset();
    this.newAddressModal.hide();
  }

  getAddresses() {
    this.addressService.getAddresses().subscribe({
      next: (addresses: Address[]) => {
        this.addresses = addresses;
        this.user.addresses = this.addresses;
      }, error: (error) => {
        this.toastr.error(error.error);
      }
    });
  }

  addAddress() {
    this.addressService.addAddress(this.addressForm.value).subscribe({
      next: (address: Address) => {
        this.toastr.success('Address added successfully');
        this.getAddresses();
        this.user.addresses = this.addresses;
        this.addressForm.reset();
      }, error: (error) => {
        this.toastr.error(error.error);
      }
    });
  }

  updateAddress(address: Address) {
    this.addressService.updateAddress(address).subscribe({
      next: (address: Address) => {
        let index = this.addresses.findIndex(a => a.id === address.id);
        this.addresses[index] = address;
        this.user.addresses = this.addresses;
        this.toastr.success('Address updated successfully');
      }, error: (error) => {
        this.toastr.error(error.error);
      }
    });
  }

  setCurrentAddress(id: number) {
    this.addressService.setCurrentAddress(id).subscribe({
      next: () => {
        let index = this.user.addresses.findIndex(a => a.id === id);
        this.addresses.forEach(a => a.isCurrent = false);
        this.addresses[index].isCurrent = true;
        this.user.addresses = this.addresses;
        this.toastr.success('Current address updated successfully');
      }, error: (error) => {
        this.toastr.error(error.error);
      }
    });
  }

  deleteAddress(id: number) {
    this.addressService.deleteAddress(id).subscribe({
      next: () => {
        debugger;
        let index = this.user.addresses.findIndex(a => a.id === id);
        this.addresses.splice(index, 1);
        this.user.addresses = this.addresses;
        this.toastr.success('Address deleted successfully');
      }, error: (error) => {
        this.toastr.error(error.error);
      }
    });
  }
}
