import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/Order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: Order;

  constructor(private ordersService: OrdersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder() {
    let id = +this.route.snapshot.paramMap.get("id")
    this.ordersService.getOrderById(id).subscribe(order => {
      this.order = order;
    }, error => {
      console.log(error); // TODO: Use toastr
    });
  }

}
