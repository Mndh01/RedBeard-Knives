import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ProductsComponent } from './components/products/products.component';
import { SharedModule } from './shared/shared.module';
import { BasketModule } from './basket/basket.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselComponent } from './components/carousel/carousel.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ToTopComponent } from './components/to-top/to-top.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { PhotoEditorComponent } from './components/photo-editor/photo-editor.component';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginTextInputComponent } from './components/login-text-input/login-text-input.component';
import { NewProductsComponent } from './components/new-products/new-products.component';
import { FeaturedComponent } from './components/featured/featured.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { CoreModule } from './core/core.module';
import { ShopModule } from './shop/shop.module';

@NgModule({
  declarations: [
    AppComponent,
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
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    CoreModule,
    BasketModule,
    ShopModule,
    BrowserAnimationsModule,
    NgbModule,
    CarouselModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

/* 
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ProductsComponent } from './components/products/products.component';
import { BasketModule } from './basket/basket.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselComponent } from './components/carousel/carousel.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ToTopComponent } from './components/to-top/to-top.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { PhotoEditorComponent } from './components/photo-editor/photo-editor.component';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginTextInputComponent } from './components/login-text-input/login-text-input.component';
import { NewProductsComponent } from './components/new-products/new-products.component';
import { FeaturedComponent } from './components/featured/featured.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductItemComponent } from './components/product-item/product-item.component';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { CoreModule } from './core/core.module';
import { ShopModule } from './components/shop/shop.module';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { Ng2TelInputModule } from 'ng2-tel-input';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CoreModule,
    BasketModule,
    BrowserAnimationsModule,
    NgbModule,
    
    CarouselModule,
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

  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
*/