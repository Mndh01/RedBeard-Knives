import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { ProductsComponent } from './components/products/products.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SharedModule } from './shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CarouselComponent } from './components/carousel/carousel.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { ToTopComponent } from './components/to-top/to-top.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { PhotoEditorComponent } from './components/photo-editor/photo-editor.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProductsComponent,
    NavbarComponent,
    CarouselComponent,
    AdminPanelComponent,
    NotFoundComponent,
    ProductCardComponent,
    ToTopComponent,
    TextInputComponent,
    ProductDetailsComponent,
    PhotoEditorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    BrowserAnimationsModule,
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
