import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../models/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;
  fakeUrl = "../assets/products.json"

  constructor(private http: HttpClient) { }

  getProducts(type: String = '', price:number = -1, inStock:number = -1, soldItems:number = -1){
    return this.http.get<Product[]>(this.baseUrl + "products?type=" + type + "&price=" + price + "&inStock=" + inStock + "&soldItems=" + soldItems);  
  }

  getProductById(id: number){
    return this.http.get<Product>(this.baseUrl + "products" + id);
  }

}

