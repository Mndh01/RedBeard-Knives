import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { AddressesComponent } from './addresses/addresses.component';
import { AccountComponent } from './account/account.component';
import { UserProfileRoutingModule } from './user-profile-routing.module';
import { OrdersComponent } from './orders/orders.component';
import { OrderDetailsComponent } from './orders/order-details/order-details.component';
import { PaymentsComponent } from './payments/payments.component';



@NgModule({
  declarations: [
    AddressesComponent,
    AccountComponent,
    OrdersComponent,
    OrderDetailsComponent,
    PaymentsComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    UserProfileRoutingModule,
  ],
  exports: [
    AddressesComponent,
    AccountComponent,
    OrdersComponent,
    OrderDetailsComponent,
    PaymentsComponent,
  ]
})
export class UserProfileModule { }
