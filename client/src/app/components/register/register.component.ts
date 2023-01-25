import { Component, OnInit, ViewChild } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { NgbCalendar, NgbDateAdapter, NgbDateNativeAdapter, NgbDatepicker, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  providers: [{provide: NgbDateAdapter, useClass: NgbDateNativeAdapter}]

})
export class RegisterComponent implements OnInit {
  step: number = 1;
  registerForm: FormGroup;
  addressForm: FormGroup;
  numberIsValid: boolean;
  passwordConfirmed: boolean = false;

  constructor(private fb: FormBuilder, private calendar: NgbCalendar,
    private accountService: AccountService, private router: Router) {
      this.model = { year: 1990, month: 1, day: 1 };
    }

  ngOnInit(): void {
    this.moveStep(0);
    this.creatRegisterForm();
  }
  
  creatRegisterForm() {
    this.addressForm  = this.fb.group({
      country: [null, [Validators.required]],
      state: [null, [Validators.required]],
      city: [null, [Validators.required]],
      street: [null, [Validators.required]],
      houseNumber: [null, [Validators.required]],
      zipCode: [null, [Validators.required]],
      isCurrent: true
    })

    this.registerForm = this.fb.group({
      userName: [null, [Validators.required, Validators.pattern("^[a-zA-Z ]{3,25}$")]],
      sureName: [null, [Validators.required, Validators.pattern("^[a-zA-Z ]{3,25}$")]],
      gender: [""],
      email: [null, 
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]
        // ,[this.validateEmailNotTaken()]
      ],
      password: [null, [Validators.required, Validators.minLength(4)]],
      passwordConfirm: [null, [Validators.required, Validators.minLength(4)]],
      phoneNumber: [null, [Validators.required]],
      dateOfBirth: [Date, [Validators.required]],
      address: {}
    });
  }
  
  onSubmit() {
    this.registerForm.controls.address.setValue(this.addressForm.value);    
    
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
      //To be  output to the user
      console.log(response);
    }, error => {
      //To be  output to the user
      console.log(error);
    });
  }

  moveStep(Step: number) {
    if(this.step > 2) {
      this.step = 2
    }
    if(this.step < 1) {
      this.step = 1
    }
    this.step += Step;    
  }
  
  confirmPassword() {
    if(this.registerForm.controls.password.value !== this.registerForm.controls.passwordConfirm.value) {
      this.passwordConfirmed = false;
    }
    else {
      this.passwordConfirmed = true;
    }    
  }

  //To be reconfigured...
  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.accountService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        })
      );
    };
  }

  //#region  Date picker TS
  @ViewChild('d1') datePopup? : NgbDatepicker;
  model: NgbDateStruct;
  completed: boolean;  
  
  selectToday() {
    return this.calendar.getToday();
  }
  
  closeFix(event, datePicker) {
    if(event.target.offsetParent == null)
    datePicker.close();
    else if(event.target.offsetParent.nodeName != "NGB-DATEPICKER")
    datePicker.close();
  }
  //#endregion

  //#region phone input functions
    telInputObject(obj) {
    }

    onCountryChange(e) {
    }

    getNumber(e) {
      this.registerForm.controls.phoneNumber.setValue(e);
    }

    hasError(e) {
      this.numberIsValid = e;
    }
  //#endregion

  //#region Input acception options (Numbers or Characters)
  allowNumericOnly(event){
    return event.charCode>=48 && event.charCode<=57
  }
  allowAlphabetOnly(event){
    return event.charCode>=65 && event.charCode<=90 
    || event.charCode>=97 && event.charCode>=122
    || event.charCode == 32;
  }
  //#endregion
}
