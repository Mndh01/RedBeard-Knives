import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxSpinnerModule,
    FormsModule,
  ],
  exports: [
    CarouselModule,
    BsDropdownModule,
    NgxSpinnerModule,
    FormsModule,
  ]
})
export class SharedModule { }
