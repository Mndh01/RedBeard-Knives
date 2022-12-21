import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { Ng2TelInputModule } from 'ng2-tel-input';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
    Ng2TelInputModule,
    PaginationModule.forRoot(),
  ],
  exports: [
    CarouselModule,
    BsDropdownModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule,
    NgxGalleryModule,
    FileUploadModule,
    Ng2TelInputModule,
    PaginationModule,
  ]
})
export class SharedModule { }