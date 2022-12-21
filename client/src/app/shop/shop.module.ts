import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { FormsModule } from '@angular/forms';
import { ProductItemComponent } from './product-item/product-item.component';
import { AppRoutingModule } from '../app-routing.module';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
  ],
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    SharedModule,
  ],
  exports: [
    ShopComponent,
    ProductItemComponent,
  ]
})
export class ShopModule { }
