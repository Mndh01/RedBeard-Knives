import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload'
import { Ng2TelInputModule } from 'ng2-tel-input';
import { RouterModule } from '@angular/router';

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
  ]
})
export class SharedModule { }

/*
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CarouselComponent, CarouselModule } from 'ngx-bootstrap/carousel';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload'
import { Ng2TelInputModule } from 'ng2-tel-input';
import { AppComponent } from '../app.component';
import { HomeComponent } from '../components/home/home.component';
import { ProductsComponent } from '../components/products/products.component';
import { AdminPanelComponent } from '../components/admin-panel/admin-panel.component';
import { NotFoundComponent } from '../errors/not-found/not-found.component';
import { ProductCardComponent } from '../components/product-card/product-card.component';
import { ToTopComponent } from '../components/to-top/to-top.component';
import { TextInputComponent } from '../components/text-input/text-input.component';
import { ProductDetailsComponent } from '../components/product-details/product-details.component';
import { PhotoEditorComponent } from '../components/photo-editor/photo-editor.component';
import { ProductEditComponent } from '../components/product-edit/product-edit.component';
import { TestErrorsComponent } from '../errors/test-errors/test-errors.component';
import { ServerErrorComponent } from '../errors/server-error/server-error.component';
import { LoginComponent } from '../components/login/login.component';
import { RegisterComponent } from '../components/register/register.component';
import { LoginTextInputComponent } from '../components/login-text-input/login-text-input.component';
import { NewProductsComponent } from '../components/new-products/new-products.component';
import { FeaturedComponent } from '../components/featured/featured.component';
import { DatepickerComponent } from 'ng2-datepicker';
import { ProductItemComponent } from '../components/product-item/product-item.component';

@NgModule({
  declarations: [
    HomeComponent,
    ProductsComponent,
    CarouselComponent,
    AdminPanelComponent,
    NotFoundComponent,
    ProductCardComponent,
    ToTopComponent,
    TextInputComponent,
    ProductDetailsComponent,
    PhotoEditorComponent,
    ProductEditComponent,
    TestErrorsComponent,
    ServerErrorComponent,
    LoginComponent,
    RegisterComponent,
    LoginTextInputComponent,
    NewProductsComponent,
    FeaturedComponent,
    DatepickerComponent,
    ProductItemComponent,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    HomeComponent,
    ProductsComponent,
    CarouselComponent,
    AdminPanelComponent,
    NotFoundComponent,
    ProductCardComponent,
    ToTopComponent,
    TextInputComponent,
    ProductDetailsComponent,
    PhotoEditorComponent,
    ProductEditComponent,
    TestErrorsComponent,
    ServerErrorComponent,
    LoginComponent,
    RegisterComponent,
    LoginTextInputComponent,
    NewProductsComponent,
    FeaturedComponent,
    DatepickerComponent,
    ProductItemComponent,
  ]
})
export class SharedModule { }
*/