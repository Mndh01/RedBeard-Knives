import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { CarouselComponent } from './components/carousel/carousel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ToTopComponent } from './components/to-top/to-top.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { RegisterComponent } from './components/register/register.component';
import { NewProductsComponent } from './components/new-products/new-products.component';
import { FeaturedComponent } from './components/featured/featured.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { CoreModule } from './core/core.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { SharedModule } from './shared/shared.module';
import { BasketModule } from './basket/basket.module';
import { BrowserModule } from '@angular/platform-browser';
import { AdminModule } from './admin/admin.module';
import { UserProfileModule } from './user-profile/user-profile.module';
import { ShopModule } from './shop/shop.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CarouselComponent,
    NotFoundComponent,
    ProductCardComponent,
    ToTopComponent,
    TestErrorsComponent,
    ServerErrorComponent,
    RegisterComponent,
    NewProductsComponent,
    FeaturedComponent,
    DatepickerComponent,
    UserProfileComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    CoreModule,
    SharedModule,
    ShopModule,
    BasketModule,
    AdminModule,
    UserProfileModule,
  ],
  exports: [

  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }