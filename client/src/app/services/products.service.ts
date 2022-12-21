import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../models/Pagination';
import { Product } from '../models/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  fakeUrl = "../assets/products.json"
  baseUrl = environment.apiUrl;
  params = new HttpParams();
  paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

  constructor(private http: HttpClient) { }
  
  setParams(category: string = "", price:number = -1, inStock:number = -1, soldItems:number = -1) {
    this.params = this.params.set("category", `${category}`);
    this.params = this.params.set("price", price);
    this.params = this.params.set("inStock", inStock);
    this.params = this.params.set("soldItems", soldItems);
  }

  getProducts(page?:number, itemsPerPage?:number){
    if(page && itemsPerPage) {
      this.params = this.params.set('pageNumber', page);
      this.params = this.params.set('pageSize', itemsPerPage);
    }

    return this.http.get<Product[]>(this.baseUrl + "products",  {observe: "response", params: this.params }).pipe(
      map(response => {
        if(response.body) {
          this.paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if(pagination) {
          this.paginatedResult.pagination = JSON.parse(pagination);
        }
        return this.paginatedResult;
      })
    );  
  }

  getProductById(id: number){
    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }

  updateProduct(product: Product) {
    return this.http.put(this.baseUrl + 'products', product)
  }

  
  deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + "products/" + id)
  }
  
  updateMainPhoto(Params:HttpParams) {
    return this.http.put(this.baseUrl + "products/set-main-photo", null, { params: Params });
  }

  deletePhoto(params: HttpParams) {
    return this.http.delete(this.baseUrl + 'products/delete-photo', { params: params });
  }
}