import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { ProductsComponent } from './components/products/products.component';
import { OrdersComponent } from './components/orders/orders.component';
import { UsersComponent } from './components/users/users.component';
import { UserDetailsComponent } from './components/users/user-details/user-details.component';
import { ProductEditComponent } from './components/products/product-edit/product-edit.component';
import { OrderDetailsComponent } from './components/orders/order-details/order-details.component';


const routes: Routes = [
  { 
    path: '', 
    component: AdminComponent,
    children: [
      {path: 'products', component: ProductsComponent},
      {path: 'products/:id', component: ProductEditComponent},
      {path: 'orders', component: OrdersComponent},
      {path: 'orders/:id', component: OrderDetailsComponent},
      {path: 'users', component: UsersComponent},
      {path: 'users/:id', component: UserDetailsComponent},
      {path: '', redirectTo: 'products', pathMatch: 'full'}
    ]
  },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class AdminRoutingModule { }
