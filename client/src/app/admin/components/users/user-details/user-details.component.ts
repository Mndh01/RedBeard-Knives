import { HttpParams } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AdminService } from 'src/app/admin/admin.service';
import { OrderParams } from 'src/app/admin/models/OrderParams';
import { UserForAdmin } from 'src/app/admin/models/UserForAdmin';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit {
  user: UserForAdmin;
  orderParams: OrderParams = new OrderParams();

  constructor(private adminService: AdminService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getUser();
  }

  getUser() {
    let id = +this.route.snapshot.paramMap.get("id")
    this.adminService.getUserById(id).subscribe({
      next: (user: UserForAdmin) => {
        debugger
        this.user = user;
      },
      error: (error) => {
        console.log(error); // TODO: Use toastr
      },
      complete: () => {
        this.getUserOrders();
    }});
  }

  getUserOrders() {
    this.adminService.getUserOrders(this.user.email).subscribe({
      next: (orders) => {
        debugger
        this.user.orders = orders.data;
        console.log(this.user.orders);
      },
      error: (error) => {
        console.log(error); // TODO: Use toastr
      }
    })
  }

}
