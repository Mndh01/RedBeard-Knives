import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Product } from '../shop/models/Product';
import { ProductCategory } from '../shop/models/ProductCategory';
import { PaginatedResponse } from '../shared/models/Pagination';
import { OrderForAdmin } from './models/OrderForAdmin';
import { UserForAdmin } from './models/UserForAdmin';
import { OrderParams } from './models/OrderParams';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  carouselAdd$: Observable<any>;
  private carouselAddSubject = new Subject<any>();
  orderParams: OrderParams = new OrderParams();
  pageParams: any = {
    pageIndex: 1,
    pageSize: 10
  }

  constructor(private http: HttpClient) {
    this.carouselAdd$ = this.carouselAddSubject.asObservable();
  }

  setPageParams(pageIndex: number, pageSize: number) {
    this.pageParams.pageIndex = pageIndex;
    this.pageParams.pageSize = pageSize;
  }

  resetPageParams() {
    this.pageParams.pageIndex = 1;
    this.pageParams.pageSize = 10;
  }
  
  getUsers() : Observable<PaginatedResponse<UserForAdmin[]>> {
    let params = new HttpParams();
    params = params.set('pageIndex', this.pageParams.pageIndex);
    params = params.set('pageSize', this.pageParams.pageSize);
    return this.http.get<PaginatedResponse<UserForAdmin[]>>(this.baseUrl + "admin/users-with-roles", {params: params});
  }

  getUserById(id: number) : Observable<UserForAdmin> {
    return this.http.get<UserForAdmin>(this.baseUrl + "admin/users-with-roles/" + id);
  }

  getOrders() : Observable<PaginatedResponse<OrderForAdmin[]>>{
    let params = new HttpParams();
    params = params.set('pageIndex', this.pageParams.pageIndex);
    params = params.set('pageSize', this.pageParams.pageSize);
    return this.http.get<PaginatedResponse<OrderForAdmin[]>>(this.baseUrl + "admin/orders", {params: params});
  }

  getUserOrders(buyerEmail: string) : Observable<PaginatedResponse<OrderForAdmin[]>>{
    let params = new HttpParams();
    params = params.set('pageIndex', this.pageParams.pageIndex);
    params = params.set('pageSize', this.pageParams.pageSize);
    params = params.set('buyerEmail', buyerEmail);
    return this.http.get<PaginatedResponse<OrderForAdmin[]>>(this.baseUrl + "admin/orders" , {params: params});
  }

  getOrderById(id: number) : Observable<OrderForAdmin> {
    return this.http.get<OrderForAdmin>(this.baseUrl + "admin/orders/" + id);
  }
  
  addProduct(product1: Product, files: File[]) : Observable<Product>{
    const product = new FormData();

    for (let key in product1) {
      if(key === 'category') {
        product.append('categoryJson', JSON.stringify(product1.category));
      }
      product.append(key, product1[key]);
    }

    if(files) {  
      for (let i = 0; i < files.length; i++) {
        product.append('files', files[i]);
      }
    }

    return this.http.post<Product>(this.baseUrl + "products/add-product",  product);
  }

  addPhotos(photos: File[], productId: number) : Observable<any>{
    const formData = new FormData();
    formData.append('productId', productId.toString());
    for (let i = 0; i < photos.length; i++) {
      formData.append('files', photos[i]);
    }
    console.log(formData.getAll('files'));
    return this.http.post(this.baseUrl + "products/add-photos", formData);
  }

  addCategory(newCategory: ProductCategory) : Observable<ProductCategory>{
    return this.http.post<ProductCategory>(this.baseUrl + "products/add-category", newCategory);
  }

  updateProduct(product: Product) {
    return this.http.put(this.baseUrl + 'products', product)
  }

  updateMainPhoto(Params: HttpParams) {
    return this.http.put(this.baseUrl + "products/set-main-photo", null, { params: Params });
  }

  deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + "products/" + id)
  }
  
  deletePhoto(params: HttpParams) {
    return this.http.delete(this.baseUrl + 'products/delete-photo', { params: params });
  }
}
