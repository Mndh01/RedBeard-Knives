import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../models/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;
  fakeUrl = "../assets/products.json"
  params = new HttpParams();

  constructor(private http: HttpClient) { }
  
  setParams(category: string, price:number, inStock:number, soldItems:number) {
    this.params = this.params.set("category", `${category}`);
    this.params = this.params.set("price", price);
    this.params = this.params.set("inStock", inStock);
    this.params = this.params.set("soldItems", soldItems);

  }
  getProducts(){
    return this.http.get<Product[]>(this.baseUrl + "products",  { params: this.params });  
  }

  getProductById(id: number){
    return this.http.get<Product>(this.baseUrl + "products/" + id);
  }

  deleteProduct(id: number) {
    return this.http.delete(this.baseUrl + "products/" + id)
  }
}