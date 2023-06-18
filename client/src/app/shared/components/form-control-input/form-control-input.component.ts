import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-form-control-input',
  templateUrl: './form-control-input.component.html',
  styleUrls: ['./form-control-input.component.scss']
})
export class FormControlInputComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() type: "text" | string;
  @Input() option: string ='all';
  @Input() isDisabled: boolean = false;
  @Input() autoComplete?: boolean = false;
  errors: Map<string, any> = new Map<string, any>();

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  get ngControlControl() {
    return this.ngControl.control as FormControl;
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }

  getValidationErrors() {
    this.errors.clear();
    const errors: Map<string, any> = new Map<string, any>();
    const controlErrors: ValidationErrors = this.ngControl.control.errors;
    if (controlErrors != null) {
      Object.keys(controlErrors).forEach(keyError => {
        errors.set(keyError, controlErrors[keyError]);
      });
    };
    this.errors = errors;
  }

  inputOption(event) {
    if(this.option == 'alphabet') {
      return this.allowAlphabetOnly(event);
      
    }
    else if(this.option == 'numeric') {
      return this.allowNumericOnly(event);
    }
    else {
      return this.allowAll(event);
    }
  }

  singleWhitespaceBetweenWords(event) {
    if(event.target.value.length > 0) {
      let value = event.target.value;
      value = value.replace(/\s+/g, ' ');
      event.target.value = value;
    }
  }

  allowNumericOnly(event){
    return event.charCode>=48 && event.charCode<=57
  }

  allowAlphabetOnly(event){
    return event.charCode>=65 && event.charCode<=90 
    || event.charCode>=97 && event.charCode<=122
    || event.charCode == 32;
  }

  allowAll(event){
    return event.charCode;
  }
}
