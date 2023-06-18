import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginatedResponse } from '../shared/models/Pagination';
import { Product } from './models/Product';
import { ProductCategory } from './models/ProductCategory';
import { ShopParams } from './models/ShopParams';
import { Observable, of } from 'rxjs';
import { Review } from '../shared/models/Review';

@Injectable({
  providedIn: 'root'
})

export class ProductsService {
  baseUrl = environment.apiUrl;
  paginatedResponse: PaginatedResponse<Product[]> = new PaginatedResponse<Product[]>();
  productCache = new Map<string, PaginatedResponse<Product[]>>();
  categories: ProductCategory[] = [];
  shopParams = new ShopParams();
  useCache = false;
  fakeUrl = "../assets/products.json";

  constructor(private http: HttpClient) { }
  
  getProducts(roles?: string[]) : Observable<PaginatedResponse<Product[]>> { 
    if(this.productCache.size > 0 && this.useCache && !roles?.includes('Admin')) {
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        this.paginatedResponse = this.productCache.get(Object.values(this.shopParams).join('-'));
        if (this.paginatedResponse) return of(this.paginatedResponse);
      }
    }
    
    let params = new HttpParams(); 
    
    if(this.shopParams?.pageIndex && this.shopParams?.pageSize) {
      params = params.set('pageIndex', this.shopParams.pageIndex);
      params = params.set('pageSize', this.shopParams.pageSize); 
    }
    if(this.shopParams?.categoryId){ params = params.set('categoryId', this.shopParams.categoryId); }
    if(this.shopParams?.sort){ params = params.set('sort', this.shopParams.sort); }
    if(this.shopParams?.search){ params = params.set('search', this.shopParams.search); }
    
    return this.http.get<PaginatedResponse<Product[]>>(this.baseUrl + "products",  {params: params }).pipe(
      map(response => {
        if(response != null) {
          this.productCache.set(Object.values(this.shopParams).join('-'), response);
          this.paginatedResponse = response;
          this.resetPageParams();
          this.useCache = true;          
        }
        return this.paginatedResponse;
      }
    ));  
  }
    
  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }
  
  resetPageParams() {
    this.shopParams.pageIndex = 1;
    this.shopParams.pageSize = 10;
  }
  
  getShopParams() {
    return this.shopParams;
  }
  
  getProductById(id: number){
    const product = [...this.productCache.values()]
      .reduce((acc, paginatedResponse) => {
        return {...acc, ...paginatedResponse.data.find(p => p.id === id)}
      }, {} as Product)

    if(Object.keys(product).length !== 0) return of(product);

    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }

  getCategories() {    
    return this.http.get<ProductCategory[]>(this.baseUrl + "products/categories").pipe(
      map ( response => {
        this.categories = response;
        return response;
      })
    )
  }

  addReviewToProduct(review: Review) {
    return this.http.post<Review>(this.baseUrl + 'products/add-review', review);
  }

  deleteReview(id: number) {
    return this.http.delete(this.baseUrl + 'products/delete-review/' + id);
  } //TODO: Implement this in the product-details component
}