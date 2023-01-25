import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
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
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { LoginComponent } from './components/login/login.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    LoginComponent,
    TextInputComponent,
  ],
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
    ModalModule.forRoot(),
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
    PagingHeaderComponent,
    PagerComponent,
    LoginComponent,
    TextInputComponent,
    PaginationModule,
    ModalModule,
  ]
})
export class SharedModule { }