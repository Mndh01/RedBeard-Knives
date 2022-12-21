import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() type: "text" | string;
  @Input() option: string ='all';

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
