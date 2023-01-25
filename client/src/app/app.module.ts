import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { SharedModule } from './shared/shared.module';
import { BasketModule } from './basket/basket.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselComponent } from './components/carousel/carousel.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ToTopComponent } from './components/to-top/to-top.component';
import { PhotoEditorComponent } from './components/photo-editor/photo-editor.component';
import { ProductEditComponent } from './components/product-edit/product-edit.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginTextInputComponent } from './components/login-text-input/login-text-input.component';
import { NewProductsComponent } from './components/new-products/new-products.component';
import { FeaturedComponent } from './components/featured/featured.component';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from './core/core.module';
import { ErrorInterceptor } from './core/interceptors/error.interceptor';
import { LoadingInterceptor } from './core/interceptors/loading.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CarouselComponent,
    AdminPanelComponent,
    NotFoundComponent,
    ProductCardComponent,
    ToTopComponent,
    PhotoEditorComponent,
    ProductEditComponent,
    TestErrorsComponent,
    ServerErrorComponent,
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
    BrowserAnimationsModule,
    NgbModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi:true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }