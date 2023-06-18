import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminService } from 'src/app/admin/admin.service';
import { OrderForAdmin } from 'src/app/admin/models/OrderForAdmin';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: OrderForAdmin;

  constructor(private adminService: AdminService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder() {
    let id = +this.route.snapshot.paramMap.get("id")
    this.adminService.getOrderById(id).subscribe(order => {
      this.order = order;
    }, error => {
      console.log(error); // TODO: Use toastr
    });
  }

}
