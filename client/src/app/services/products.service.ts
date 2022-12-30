import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResponse, PaginatedResult } from '../models/Pagination';
import { Product } from '../models/Product';
import { ProductCategory } from '../models/ProductCategory';
import { ShopParams } from '../models/ShopParams';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  fakeUrl = "../assets/products.json"
  baseUrl = environment.apiUrl;
  paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

  constructor(private http: HttpClient) { }
  
  getProducts(shopParams?: ShopParams){
    let params = new HttpParams(); 
    
    if(shopParams?.pageIndex && shopParams?.pageSize) {
      params = params.set('pageIndex', shopParams.pageIndex);
      params = params.set('pageSize', shopParams.pageSize);
    }

    if(shopParams?.categoryId){
      params = params.set('categoryId', shopParams.categoryId);
    }

    if(shopParams?.sort){
      params = params.set('sort', shopParams.sort);
    }

    if(shopParams?.search){
      params = params.set('search', shopParams.search);
    }
    
    return this.http.get<PaginatedResponse<Product[]>>(this.baseUrl + "products",  {observe: "response", params: params }).pipe(
      map(response => {
        if(response.body != null) {
          this.paginatedResult.data = response.body.data;
          this.paginatedResult.pagination.pageIndex = response.body.pageIndex;
          this.paginatedResult.pagination.pageSize = response.body.pageSize;
          this.paginatedResult.pagination.count = response.body.count;
          this.paginatedResult.pagination.totalPages = Math.ceil(response.body.count / response.body.pageSize);
        }
        return this.paginatedResult;
      })
    );  
  }

  getProductById(id: number){
    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }

  getCategories() {
    return this.http.get<ProductCategory[]>(this.baseUrl + "products/categories");
  }

  updateProduct(product: Product) {
    return this.http.put(this.baseUrl + 'products', product)
  }

  deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + "products/" + id)
  }
  
  updateMainPhoto(Params: HttpParams) {
    return this.http.put(this.baseUrl + "products/set-main-photo", null, { params: Params });
  }

  deletePhoto(params: HttpParams) {
    return this.http.delete(this.baseUrl + 'products/delete-photo', { params: params });
  }
}