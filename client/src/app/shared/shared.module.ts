import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxSpinnerModule,
  ],
  exports: [
    CarouselModule,
    BsDropdownModule,
    NgxSpinnerModule,
  ]
})
export class SharedModule { }
