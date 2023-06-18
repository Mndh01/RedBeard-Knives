import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { AddressService } from 'src/app/user-profile/addresses/address.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/shared/models/User';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {
  user: User;
  passwordForm: FormGroup;
  isDisabled = 'disabled';
  
  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) {
    this.accountService.currentUser$.subscribe(user => { this.user = user; })
   };

  ngOnInit(): void {
    this.initializePasswordChangeForm();
  }

  initializePasswordChangeForm() {
    this.passwordForm = this.fb.group({
      oldPassword: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(25)]],
      newPassword: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(25)]],
      newPasswordConfirm: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(25)]],
    })
  }

  changePassword() {
    if (this.passwordForm.value.newPassword != this.passwordForm.value.newPasswordConfirm) {
      this.toastr.error('Passwords do not match');
      return;
    }
    
    this.accountService.changePassword(this.passwordForm.value).subscribe({ 
      next: () => {
        this.toastr.success('Password was changed successfully');
      },
      error: (error) => {
        (error.forEach(element => {
          this.toastr.error(element.description);  
        }));
      }
    })
  }
}
