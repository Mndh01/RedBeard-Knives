import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { OrdersService } from './orders.service';
import { Order } from 'src/app/shared/models/Order';
import { PaginatedResponse, Pagination } from 'src/app/shared/models/Pagination';

@Component({
  selector: 'app-order',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: Order[];
  pagination: Pagination = {} as Pagination;

  constructor(private ordersService: OrdersService, private toastrService: ToastrService) { }

  ngOnInit(): void {
    this.getOrders();
  }
  
  getOrders() {
    this.ordersService.getOrdersForUser().subscribe({
      next: (orders: PaginatedResponse<Order[]>) => {
        this.orders = orders.data.sort((a, b) => b.id - a.id);
        this.pagination.pageIndex = orders.pageIndex;
        this.pagination.pageSize = orders.pageSize;
        this.pagination.count = orders.count;
        this.pagination.totalPages = Math.ceil(orders.count / orders.pageSize);
        this.ordersService.setPageParams(1, 10)
      },
      error: error => {
        this.toastrService.error(error.message);
      }
    })
  }

  onPageChanged(event: any) {
    if (this.pagination.pageIndex !== event){
      this.pagination.pageIndex = event;
      this.ordersService.setPageParams(this.pagination.pageIndex, this.pagination.pageSize);
      this.getOrders();
    }
  }

}
