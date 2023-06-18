import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserProfileComponent } from './user-profile.component';
import { OrdersComponent } from './orders/orders.component';
import { OrderDetailsComponent } from './orders/order-details/order-details.component';
import { AddressesComponent } from './addresses/addresses.component';
import { AccountComponent } from './account/account.component';
import { PaymentsComponent } from './payments/payments.component';

const routes: Routes = [
  {
    path: '' , 
    component: UserProfileComponent,
    
    children: [
      {path: 'account', component: AccountComponent},
      {path: 'orders', component: OrdersComponent},
      {path: 'orders/:id', component: OrderDetailsComponent},
      {path: 'addresses', component: AddressesComponent},
      // {path: 'payments', component: PaymentsComponent},
      // {path: 'payments/:id', component: PaymentDetailsComponent},
      {path: '', redirectTo: 'account', pathMatch: 'full'}
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule
  ]
})
export class UserProfileRoutingModule { }
