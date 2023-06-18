import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FileUploadModule } from 'ng2-file-upload';
import { Ng2TelInputModule } from 'ng2-tel-input';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { LoginComponent } from './components/login/login.component';
import { FormControlInputComponent } from './components/form-control-input/form-control-input.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { StepperComponent } from './components/stepper/stepper.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { BasketSummaryComponent } from './components/basket-summary/basket-summary.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalComponent } from './components/modal/modal.component';
import { ImageSlideComponent } from './components/image-slide/image-slide.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { NgSelectModule } from '@ng-select/ng-select';




@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    LoginComponent,
    FormControlInputComponent,
    OrderTotalsComponent,
    StepperComponent,
    BasketSummaryComponent,
    ModalComponent,
    ImageSlideComponent,
    TextInputComponent,
  ],
  imports: [
    CommonModule,
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    FileUploadModule,
    Ng2TelInputModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    CdkStepperModule,
    NgbModule,
    NgSelectModule,
  ],
  exports: [
    CommonModule,
    CarouselModule,
    BsDropdownModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule,
    FileUploadModule,
    Ng2TelInputModule,
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    LoginComponent,
    FormControlInputComponent,
    TextInputComponent,
    ImageSlideComponent,
    OrderTotalsComponent,
    StepperComponent,
    ModalComponent,
    BasketSummaryComponent,
    PaginationModule,
    ModalModule,
    CdkStepperModule,
    NgbModule,
    NgSelectModule,
  ]
})
export class SharedModule { }