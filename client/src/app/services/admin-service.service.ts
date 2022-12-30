import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Product } from '../models/Product';
import { ProductCategory } from '../models/ProductCategory';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  carouselAdd$: Observable<any>;
  private carouselAddSubject = new Subject<any>();

  constructor(private http: HttpClient) {
    this.carouselAdd$ = this.carouselAddSubject.asObservable();
  }
  
  addSlide (data: any) { 
    console.log(data);
    this.carouselAddSubject.next(data);
  }

  addProduct(model: Product) {
    return this.http.post<Product>(this.baseUrl + "products/add-product", model);
  }

  addCategory(newCategory: ProductCategory) {
    return this.http.post<ProductCategory>(this.baseUrl + "products/add-category", newCategory);
  }

}
