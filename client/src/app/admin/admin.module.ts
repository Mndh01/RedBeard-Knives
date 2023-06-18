import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { SharedModule } from '../shared/shared.module';
import { ProductsComponent } from './components/products/products.component';
import { UsersComponent } from './components/users/users.component';
import { OrdersComponent } from './components/orders/orders.component';
import { AdminRoutingModule } from './admin-routing.module';
import { UserDetailsComponent } from './components/users/user-details/user-details.component';
import { OverviewComponent } from './components/overview/overview.component';
import { GenericTableComponent } from './components/generic-table/generic-table.component';
import { OrderDetailsComponent } from './components/orders/order-details/order-details.component';
import { ProductEditComponent } from './components/products/product-edit/product-edit.component';
import { PhotoEditorComponent } from './components/products/product-edit/photo-editor/photo-editor.component';



@NgModule({
  declarations: [
    AdminComponent,
    ProductsComponent,
    ProductEditComponent,
    PhotoEditorComponent,
    UsersComponent,
    OrdersComponent,
    OrderDetailsComponent,
    UserDetailsComponent,
    OverviewComponent,
    GenericTableComponent,
  ],
  imports: [
    SharedModule,
    AdminRoutingModule,
  ],
  exports: [
    AdminComponent,
    ProductsComponent,
    ProductEditComponent,
    UsersComponent,
    OrdersComponent,
  ]
})
export class AdminModule { }
