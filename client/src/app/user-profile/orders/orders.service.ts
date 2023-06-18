import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { PaginatedResponse } from 'src/app/shared/models/Pagination';
import { Order } from 'src/app/shared/models/Order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;
  pageParams: any = {
    pageIndex: 1,
    pageSize: 10
  }

  constructor(private http: HttpClient) { }

  getOrdersForUser() : Observable<PaginatedResponse<Order[]>>{
    let params = new HttpParams();
    params = params.set('pageIndex', this.pageParams.pageIndex);
    params = params.set('pageSize', this.pageParams.pageSize);

    return this.http.get<PaginatedResponse<Order[]>>(this.baseUrl + 'orders', {params: params});
  }

  getOrderById(id: number) {
    return this.http.get<Order>(this.baseUrl + `orders/${id}`);
  }

  setPageParams(pageIndex: number, pageSize: number) {
    this.pageParams.pageIndex = pageIndex;
    this.pageParams.pageSize = pageSize;
  }
}
