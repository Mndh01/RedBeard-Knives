import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload'


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    NgxGalleryModule,
    FileUploadModule,
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
  ]
})
export class SharedModule { }
