import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminService } from '../../admin.service';
import { ToastrService } from 'ngx-toastr';
import { OrderForAdmin } from '../../models/OrderForAdmin';
import { PaginatedResponse, Pagination } from 'src/app/shared/models/Pagination';
import { GenericTableComponent } from '../generic-table/generic-table.component';
import { OrderParams } from '../../models/OrderParams';
import { OrderStatus } from 'src/app/user-profile/orders/OrderStatus';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit{
  @ViewChild('table') genericTable?: GenericTableComponent;
  orders: OrderForAdmin[];
  orderParams: OrderParams = new OrderParams();
  orderStatus = Object.keys(OrderStatus).filter(key => !isNaN(Number(OrderStatus[key])));
  selectedStatus: string = this.orderStatus[0];
  pagination: Pagination = {} as Pagination;

  constructor(private adminService: AdminService, private toastrService: ToastrService) { }
  
  ngOnInit(): void {
    console.log(this.orderParams.status);
    this.getOrders();
  }
  
  getOrders(orderParams?: OrderParams) {
    if (orderParams) {
      this.orderParams = orderParams;
    }
    this.adminService.getOrders().subscribe({
      next: (orders: PaginatedResponse<OrderForAdmin[]>) => {
        this.orders = orders.data.sort((a, b) => b.id - a.id);
        this.pagination.pageIndex = orders.pageIndex;
        this.pagination.pageSize = orders.pageSize;
        this.pagination.count = orders.count;
        this.pagination.totalPages = Math.ceil(orders.count / orders.pageSize);
        this.adminService.setPageParams(1, 10)
      },
      error: error => {
        this.toastrService.error(error.message);
      },
      complete: () => {
        this.orders.forEach(order => {
          order.status = OrderStatus[order.status];
        })
        this.genericTable?.setTableValues(this.orders);
      }
    })
  }
  
  onPageChanged(event: any) {
    if (this.pagination.pageIndex !== event){
      this.pagination.pageIndex = event;
      this.adminService.setPageParams(this.pagination.pageIndex, this.pagination.pageSize);
      this.getOrders();
    }
  }
  
  customSearchFn = (term: string, item: string) => {
    term = term.toLowerCase();
    return item.toLowerCase().indexOf(term) > -1;
  }

  onStatusChange(status: string) {
    this.selectedStatus = status;
    this.orderParams.status = OrderStatus[status];
    this.getOrders(this.orderParams);    
  }

  onBuyerChange(event: string) {
    console.log(event);
    this.orderParams.buyerEmail = event;
    console.log(this.orderParams.buyerEmail);    
  }

  onReset() {
  }
}
